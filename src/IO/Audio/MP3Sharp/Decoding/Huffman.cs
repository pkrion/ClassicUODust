#region license

// Copyright (c) 2021, andreakarasho
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
// 1. Redistributions of source code must retain the above copyright
//    notice, this list of conditions and the following disclaimer.
// 2. Redistributions in binary form must reproduce the above copyright
//    notice, this list of conditions and the following disclaimer in the
//    documentation and/or other materials provided with the distribution.
// 3. All advertising materials mentioning features or use of this software
//    must display the following acknowledgement:
//    This product includes software developed by andreakarasho - https://github.com/andreakarasho
// 4. Neither the name of the copyright holder nor the
//    names of its contributors may be used to endorse or promote products
//    derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS ''AS IS'' AND ANY
// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

using ClassicUO.IO.Audio.MP3Sharp.Support;

namespace ClassicUO.IO.Audio.MP3Sharp.Decoding
{
    /// <summary>
    ///     Implements a Huffman decoder.
    /// </summary>
    internal sealed class Huffman
    {
        private const int MXOFF = 250;
        private const int HTN = 34;
        private static readonly int[][] ValTab0 = { new[] { 0, 0 } };

        private static readonly int[][] ValTab1 =
        {
            new[] { 2, 1 }, new[] { 0, 0 }, new[] { 2, 1 },
            new[] { 0, 16 }, new[] { 2, 1 }, new[] { 0, 1 }, new[] { 0, 17 }
        };

        private static readonly int[][] ValTab2 =
        {
            new[] { 2, 1 }, new[] { 0, 0 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 16 }, new[] { 0, 1 }, new[] { 2, 1 }, new[] { 0, 17 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 32 }, new[] { 0, 33 }, new[] { 2, 1 }, new[] { 0, 18 },
            new[] { 2, 1 }, new[] { 0, 2 }, new[] { 0, 34 }
        };

        private static readonly int[][] ValTab3 =
        {
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 0 },
            new[] { 0, 1 }, new[] { 2, 1 }, new[] { 0, 17 }, new[] { 2, 1 }, new[] { 0, 16 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 32 }, new[] { 0, 33 }, new[] { 2, 1 }, new[] { 0, 18 },
            new[] { 2, 1 }, new[] { 0, 2 }, new[] { 0, 34 }
        };

        private static readonly int[][] ValTab4 = { new[] { 0, 0 } }; // dummy

        private static readonly int[][] ValTab5 =
        {
            new[] { 2, 1 }, new[] { 0, 0 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 16 }, new[] { 0, 1 }, new[] { 2, 1 }, new[] { 0, 17 }, new[] { 8, 1 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 32 }, new[] { 0, 2 }, new[] { 2, 1 }, new[] { 0, 33 },
            new[] { 0, 18 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 34 },
            new[] { 0, 48 }, new[] { 2, 1 }, new[] { 0, 3 }, new[] { 0, 19 }, new[] { 2, 1 },
            new[] { 0, 49 }, new[] { 2, 1 }, new[] { 0, 50 }, new[] { 2, 1 }, new[] { 0, 35 },
            new[] { 0, 51 }
        };

        private static readonly int[][] ValTab6 =
        {
            new[] { 6, 1 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 0 }, new[] { 0, 16 }, new[] { 0, 17 }, new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 1 },
            new[] { 2, 1 }, new[] { 0, 32 }, new[] { 0, 33 }, new[] { 6, 1 }, new[] { 2, 1 },
            new[] { 0, 18 }, new[] { 2, 1 }, new[] { 0, 2 }, new[] { 0, 34 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 49 }, new[] { 0, 19 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 48 },
            new[] { 0, 50 }, new[] { 2, 1 }, new[] { 0, 35 }, new[] { 2, 1 }, new[] { 0, 3 },
            new[] { 0, 51 }
        };

        private static readonly int[][] ValTab7 =
        {
            new[] { 2, 1 }, new[] { 0, 0 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 16 }, new[] { 0, 1 }, new[] { 8, 1 }, new[] { 2, 1 }, new[] { 0, 17 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 32 }, new[] { 0, 2 }, new[] { 0, 33 },
            new[] { 18, 1 }, new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 18 }, new[] { 2, 1 },
            new[] { 0, 34 }, new[] { 0, 48 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 49 },
            new[] { 0, 19 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 3 }, new[] { 0, 50 }, new[] { 2, 1 },
            new[] { 0, 35 }, new[] { 0, 4 }, new[] { 10, 1 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 64 }, new[] { 0, 65 }, new[] { 2, 1 }, new[] { 0, 20 }, new[] { 2, 1 },
            new[] { 0, 66 }, new[] { 0, 36 }, new[] { 12, 1 }, new[] { 6, 1 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 51 }, new[] { 0, 67 }, new[] { 0, 80 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 52 }, new[] { 0, 5 }, new[] { 0, 81 }, new[] { 6, 1 }, new[] { 2, 1 },
            new[] { 0, 21 }, new[] { 2, 1 }, new[] { 0, 82 }, new[] { 0, 37 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 68 }, new[] { 0, 53 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 83 }, new[] { 0, 84 }, new[] { 2, 1 }, new[] { 0, 69 }, new[] { 0, 85 }
        };

        private static readonly int[][] ValTab8 =
        {
            new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 0 },
            new[] { 2, 1 }, new[] { 0, 16 }, new[] { 0, 1 }, new[] { 2, 1 }, new[] { 0, 17 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 33 }, new[] { 0, 18 }, new[] { 14, 1 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 32 }, new[] { 0, 2 }, new[] { 2, 1 }, new[] { 0, 34 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 48 }, new[] { 0, 3 }, new[] { 2, 1 }, new[] { 0, 49 },
            new[] { 0, 19 }, new[] { 14, 1 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 50 }, new[] { 0, 35 }, new[] { 2, 1 }, new[] { 0, 64 }, new[] { 0, 4 },
            new[] { 2, 1 }, new[] { 0, 65 }, new[] { 2, 1 }, new[] { 0, 20 }, new[] { 0, 66 },
            new[] { 12, 1 }, new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 36 }, new[] { 2, 1 },
            new[] { 0, 51 }, new[] { 0, 80 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 67 },
            new[] { 0, 52 }, new[] { 0, 81 }, new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 21 },
            new[] { 2, 1 }, new[] { 0, 5 }, new[] { 0, 82 }, new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 37 },
            new[] { 2, 1 }, new[] { 0, 68 }, new[] { 0, 53 }, new[] { 2, 1 }, new[] { 0, 83 },
            new[] { 2, 1 }, new[] { 0, 69 }, new[] { 2, 1 }, new[] { 0, 84 }, new[] { 0, 85 }
        };

        private static readonly int[][] ValTab9 =
        {
            new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 0 }, new[] { 0, 16 }, new[] { 2, 1 }, new[] { 0, 1 }, new[] { 0, 17 },
            new[] { 10, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 32 }, new[] { 0, 33 },
            new[] { 2, 1 }, new[] { 0, 18 }, new[] { 2, 1 }, new[] { 0, 2 }, new[] { 0, 34 },
            new[] { 12, 1 }, new[] { 6, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 48 }, new[] { 0, 3 },
            new[] { 0, 49 }, new[] { 2, 1 }, new[] { 0, 19 }, new[] { 2, 1 }, new[] { 0, 50 },
            new[] { 0, 35 }, new[] { 12, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 65 },
            new[] { 0, 20 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 64 }, new[] { 0, 51 },
            new[] { 2, 1 }, new[] { 0, 66 }, new[] { 0, 36 }, new[] { 10, 1 }, new[] { 6, 1 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 4 }, new[] { 0, 80 }, new[] { 0, 67 }, new[] { 2, 1 },
            new[] { 0, 52 }, new[] { 0, 81 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 21 }, new[] { 0, 82 }, new[] { 2, 1 }, new[] { 0, 37 }, new[] { 0, 68 },
            new[] { 6, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 5 }, new[] { 0, 84 }, new[] { 0, 83 },
            new[] { 2, 1 }, new[] { 0, 53 }, new[] { 2, 1 }, new[] { 0, 69 }, new[] { 0, 85 }
        };

        private static readonly int[][] ValTab10 =
        {
            new[] { 2, 1 }, new[] { 0, 0 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 16 }, new[] { 0, 1 },
            new[] { 10, 1 }, new[] { 2, 1 }, new[] { 0, 17 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 32 }, new[] { 0, 2 }, new[] { 2, 1 }, new[] { 0, 33 }, new[] { 0, 18 },
            new[] { 28, 1 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 34 },
            new[] { 0, 48 }, new[] { 2, 1 }, new[] { 0, 49 }, new[] { 0, 19 }, new[] { 8, 1 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 3 }, new[] { 0, 50 }, new[] { 2, 1 }, new[] { 0, 35 },
            new[] { 0, 64 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 65 }, new[] { 0, 20 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 4 }, new[] { 0, 51 }, new[] { 2, 1 }, new[] { 0, 66 },
            new[] { 0, 36 }, new[] { 28, 1 }, new[] { 10, 1 }, new[] { 6, 1 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 80 }, new[] { 0, 5 }, new[] { 0, 96 }, new[] { 2, 1 },
            new[] { 0, 97 }, new[] { 0, 22 }, new[] { 12, 1 }, new[] { 6, 1 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 67 }, new[] { 0, 52 }, new[] { 0, 81 }, new[] { 2, 1 },
            new[] { 0, 21 }, new[] { 2, 1 }, new[] { 0, 82 }, new[] { 0, 37 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 38 }, new[] { 0, 54 }, new[] { 0, 113 }, new[] { 20, 1 },
            new[] { 8, 1 }, new[] { 2, 1 }, new[] { 0, 23 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 68 },
            new[] { 0, 83 }, new[] { 0, 6 }, new[] { 6, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 53 },
            new[] { 0, 69 }, new[] { 0, 98 }, new[] { 2, 1 }, new[] { 0, 112 }, new[] { 2, 1 },
            new[] { 0, 7 }, new[] { 0, 100 }, new[] { 14, 1 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 114 }, new[] { 0, 39 }, new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 99 },
            new[] { 2, 1 }, new[] { 0, 84 }, new[] { 0, 85 }, new[] { 2, 1 }, new[] { 0, 70 },
            new[] { 0, 115 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 55 },
            new[] { 0, 101 }, new[] { 2, 1 }, new[] { 0, 86 }, new[] { 0, 116 },
            new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 71 }, new[] { 2, 1 }, new[] { 0, 102 },
            new[] { 0, 117 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 87 }, new[] { 0, 118 },
            new[] { 2, 1 }, new[] { 0, 103 }, new[] { 0, 119 }
        };

        private static readonly int[][] ValTab11 =
        {
            new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 0 }, new[] { 2, 1 }, new[] { 0, 16 }, new[] { 0, 1 },
            new[] { 8, 1 }, new[] { 2, 1 }, new[] { 0, 17 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 32 },
            new[] { 0, 2 }, new[] { 0, 18 }, new[] { 24, 1 }, new[] { 8, 1 }, new[] { 2, 1 },
            new[] { 0, 33 }, new[] { 2, 1 }, new[] { 0, 34 }, new[] { 2, 1 }, new[] { 0, 48 },
            new[] { 0, 3 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 49 }, new[] { 0, 19 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 50 }, new[] { 0, 35 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 64 }, new[] { 0, 4 }, new[] { 2, 1 }, new[] { 0, 65 }, new[] { 0, 20 },
            new[] { 30, 1 }, new[] { 16, 1 }, new[] { 10, 1 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 66 }, new[] { 0, 36 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 51 },
            new[] { 0, 67 }, new[] { 0, 80 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 52 },
            new[] { 0, 81 }, new[] { 0, 97 }, new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 22 },
            new[] { 2, 1 }, new[] { 0, 6 }, new[] { 0, 38 }, new[] { 2, 1 }, new[] { 0, 98 }, new[] { 2, 1 },
            new[] { 0, 21 }, new[] { 2, 1 }, new[] { 0, 5 }, new[] { 0, 82 }, new[] { 16, 1 },
            new[] { 10, 1 }, new[] { 6, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 37 },
            new[] { 0, 68 }, new[] { 0, 96 }, new[] { 2, 1 }, new[] { 0, 99 }, new[] { 0, 54 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 112 }, new[] { 0, 23 }, new[] { 0, 113 },
            new[] { 16, 1 }, new[] { 6, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 7 },
            new[] { 0, 100 }, new[] { 0, 114 }, new[] { 2, 1 }, new[] { 0, 39 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 83 }, new[] { 0, 53 }, new[] { 2, 1 }, new[] { 0, 84 },
            new[] { 0, 69 }, new[] { 10, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 70 },
            new[] { 0, 115 }, new[] { 2, 1 }, new[] { 0, 55 }, new[] { 2, 1 }, new[] { 0, 101 },
            new[] { 0, 86 }, new[] { 10, 1 }, new[] { 6, 1 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 85 }, new[] { 0, 87 }, new[] { 0, 116 }, new[] { 2, 1 }, new[] { 0, 71 },
            new[] { 0, 102 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 117 }, new[] { 0, 118 },
            new[] { 2, 1 }, new[] { 0, 103 }, new[] { 0, 119 }
        };

        private static readonly int[][] ValTab12 =
        {
            new[] { 12, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 16 }, new[] { 0, 1 }, new[] { 2, 1 },
            new[] { 0, 17 }, new[] { 2, 1 }, new[] { 0, 0 }, new[] { 2, 1 }, new[] { 0, 32 }, new[] { 0, 2 },
            new[] { 16, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 33 }, new[] { 0, 18 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 34 }, new[] { 0, 49 }, new[] { 2, 1 },
            new[] { 0, 19 }, new[] { 2, 1 }, new[] { 0, 48 }, new[] { 2, 1 }, new[] { 0, 3 },
            new[] { 0, 64 }, new[] { 26, 1 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 50 }, new[] { 0, 35 }, new[] { 2, 1 }, new[] { 0, 65 }, new[] { 0, 51 },
            new[] { 10, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 20 }, new[] { 0, 66 },
            new[] { 2, 1 }, new[] { 0, 36 }, new[] { 2, 1 }, new[] { 0, 4 }, new[] { 0, 80 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 67 }, new[] { 0, 52 }, new[] { 2, 1 }, new[] { 0, 81 },
            new[] { 0, 21 }, new[] { 28, 1 }, new[] { 14, 1 }, new[] { 8, 1 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 82 }, new[] { 0, 37 }, new[] { 2, 1 }, new[] { 0, 83 },
            new[] { 0, 53 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 96 }, new[] { 0, 22 },
            new[] { 0, 97 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 98 }, new[] { 0, 38 },
            new[] { 6, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 5 }, new[] { 0, 6 }, new[] { 0, 68 },
            new[] { 2, 1 }, new[] { 0, 84 }, new[] { 0, 69 }, new[] { 18, 1 }, new[] { 10, 1 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 99 }, new[] { 0, 54 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 112 }, new[] { 0, 7 }, new[] { 0, 113 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 23 }, new[] { 0, 100 }, new[] { 2, 1 }, new[] { 0, 70 }, new[] { 0, 114 },
            new[] { 10, 1 }, new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 39 }, new[] { 2, 1 },
            new[] { 0, 85 }, new[] { 0, 115 }, new[] { 2, 1 }, new[] { 0, 55 }, new[] { 0, 86 },
            new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 101 },
            new[] { 0, 116 }, new[] { 2, 1 }, new[] { 0, 71 }, new[] { 0, 102 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 117 }, new[] { 0, 87 }, new[] { 2, 1 }, new[] { 0, 118 },
            new[] { 2, 1 }, new[] { 0, 103 }, new[] { 0, 119 }
        };

        private static readonly int[][] ValTab13 =
        {
            new[] { 2, 1 }, new[] { 0, 0 }, new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 16 }, new[] { 2, 1 },
            new[] { 0, 1 }, new[] { 0, 17 }, new[] { 28, 1 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 32 }, new[] { 0, 2 }, new[] { 2, 1 }, new[] { 0, 33 }, new[] { 0, 18 },
            new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 34 }, new[] { 0, 48 }, new[] { 2, 1 },
            new[] { 0, 3 }, new[] { 0, 49 }, new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 19 }, new[] { 2, 1 },
            new[] { 0, 50 }, new[] { 0, 35 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 64 },
            new[] { 0, 4 }, new[] { 0, 65 }, new[] { 70, 1 }, new[] { 28, 1 }, new[] { 14, 1 },
            new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 20 }, new[] { 2, 1 }, new[] { 0, 51 },
            new[] { 0, 66 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 36 }, new[] { 0, 80 },
            new[] { 2, 1 }, new[] { 0, 67 }, new[] { 0, 52 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 81 }, new[] { 0, 21 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 5 },
            new[] { 0, 82 }, new[] { 2, 1 }, new[] { 0, 37 }, new[] { 2, 1 }, new[] { 0, 68 },
            new[] { 0, 83 }, new[] { 14, 1 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 96 }, new[] { 0, 6 }, new[] { 2, 1 }, new[] { 0, 97 }, new[] { 0, 22 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 128 }, new[] { 0, 8 }, new[] { 0, 129 },
            new[] { 16, 1 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 53 },
            new[] { 0, 98 }, new[] { 2, 1 }, new[] { 0, 38 }, new[] { 0, 84 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 69 }, new[] { 0, 99 }, new[] { 2, 1 }, new[] { 0, 54 },
            new[] { 0, 112 }, new[] { 6, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 7 },
            new[] { 0, 85 }, new[] { 0, 113 }, new[] { 2, 1 }, new[] { 0, 23 }, new[] { 2, 1 },
            new[] { 0, 39 }, new[] { 0, 55 }, new[] { 72, 1 }, new[] { 24, 1 }, new[] { 12, 1 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 24 }, new[] { 0, 130 }, new[] { 2, 1 },
            new[] { 0, 40 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 100 }, new[] { 0, 70 },
            new[] { 0, 114 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 132 },
            new[] { 0, 72 }, new[] { 2, 1 }, new[] { 0, 144 }, new[] { 0, 9 }, new[] { 2, 1 },
            new[] { 0, 145 }, new[] { 0, 25 }, new[] { 24, 1 }, new[] { 14, 1 }, new[] { 8, 1 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 115 }, new[] { 0, 101 }, new[] { 2, 1 },
            new[] { 0, 86 }, new[] { 0, 116 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 71 },
            new[] { 0, 102 }, new[] { 0, 131 }, new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 56 },
            new[] { 2, 1 }, new[] { 0, 117 }, new[] { 0, 87 }, new[] { 2, 1 }, new[] { 0, 146 },
            new[] { 0, 41 }, new[] { 14, 1 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 103 }, new[] { 0, 133 }, new[] { 2, 1 }, new[] { 0, 88 }, new[] { 0, 57 },
            new[] { 2, 1 }, new[] { 0, 147 }, new[] { 2, 1 }, new[] { 0, 73 }, new[] { 0, 134 },
            new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 160 }, new[] { 2, 1 }, new[] { 0, 104 },
            new[] { 0, 10 }, new[] { 2, 1 }, new[] { 0, 161 }, new[] { 0, 26 }, new[] { 68, 1 },
            new[] { 24, 1 }, new[] { 12, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 162 },
            new[] { 0, 42 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 149 }, new[] { 0, 89 },
            new[] { 2, 1 }, new[] { 0, 163 }, new[] { 0, 58 }, new[] { 8, 1 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 74 }, new[] { 0, 150 }, new[] { 2, 1 }, new[] { 0, 176 },
            new[] { 0, 11 }, new[] { 2, 1 }, new[] { 0, 177 }, new[] { 0, 27 }, new[] { 20, 1 },
            new[] { 8, 1 }, new[] { 2, 1 }, new[] { 0, 178 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 118 }, new[] { 0, 119 }, new[] { 0, 148 }, new[] { 6, 1 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 135 }, new[] { 0, 120 }, new[] { 0, 164 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 105 }, new[] { 0, 165 }, new[] { 0, 43 }, new[] { 12, 1 },
            new[] { 6, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[]
            {
                0,
                90
            },
            new[] { 0, 136 }, new[] { 0, 179 }, new[] { 2, 1 }, new[] { 0, 59 }, new[] { 2, 1 },
            new[] { 0, 121 }, new[] { 0, 166 }, new[] { 6, 1 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 106 }, new[] { 0, 180 }, new[] { 0, 192 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 12 }, new[] { 0, 152 }, new[] { 0, 193 }, new[] { 60, 1 }, new[] { 22, 1 },
            new[] { 10, 1 }, new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 28 }, new[] { 2, 1 },
            new[] { 0, 137 }, new[] { 0, 181 }, new[] { 2, 1 }, new[] { 0, 91 }, new[] { 0, 194 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 44 }, new[] { 0, 60 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 182 }, new[] { 0, 107 }, new[] { 2, 1 }, new[] { 0, 196 }, new[] { 0, 76 },
            new[] { 16, 1 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 168 },
            new[] { 0, 138 }, new[] { 2, 1 }, new[] { 0, 208 }, new[] { 0, 13 }, new[] { 2, 1 },
            new[] { 0, 209 }, new[] { 2, 1 }, new[] { 0, 75 }, new[] { 2, 1 }, new[] { 0, 151 },
            new[] { 0, 167 }, new[] { 12, 1 }, new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 195 },
            new[] { 2, 1 }, new[] { 0, 122 }, new[] { 0, 153 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 197 }, new[] { 0, 92 }, new[] { 0, 183 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 29 }, new[] { 0, 210 }, new[] { 2, 1 }, new[] { 0, 45 }, new[] { 2, 1 },
            new[] { 0, 123 }, new[] { 0, 211 }, new[] { 52, 1 }, new[] { 28, 1 }, new[] { 12, 1 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 61 }, new[] { 0, 198 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 108 }, new[] { 0, 169 }, new[] { 2, 1 }, new[] { 0, 154 },
            new[] { 0, 212 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 184 },
            new[] { 0, 139 }, new[] { 2, 1 }, new[] { 0, 77 }, new[] { 0, 199 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 124 }, new[] { 0, 213 }, new[] { 2, 1 }, new[] { 0, 93 },
            new[] { 0, 224 }, new[] { 10, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 225 },
            new[] { 0, 30 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 14 }, new[] { 0, 46 }, new[] { 0, 226 },
            new[] { 8, 1 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 227 }, new[] { 0, 109 }, new[] { 2, 1 },
            new[] { 0, 140 }, new[] { 0, 228 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 229 },
            new[] { 0, 186 }, new[] { 0, 240 }, new[] { 38, 1 }, new[] { 16, 1 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 241 }, new[] { 0, 31 }, new[] { 6, 1 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 170 }, new[] { 0, 155 }, new[] { 0, 185 }, new[] { 2, 1 },
            new[] { 0, 62 }, new[] { 2, 1 }, new[] { 0, 214 }, new[] { 0, 200 }, new[] { 12, 1 },
            new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 78 }, new[] { 2, 1 }, new[] { 0, 215 },
            new[] { 0, 125 }, new[] { 2, 1 }, new[] { 0, 171 }, new[] { 2, 1 }, new[] { 0, 94 },
            new[] { 0, 201 }, new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 15 }, new[] { 2, 1 },
            new[] { 0, 156 }, new[] { 0, 110 }, new[] { 2, 1 }, new[] { 0, 242 }, new[] { 0, 47 },
            new[] { 32, 1 }, new[] { 16, 1 }, new[] { 6, 1 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 216 }, new[] { 0, 141 }, new[] { 0, 63 }, new[] { 6, 1 }, new[] { 2, 1 },
            new[] { 0, 243 }, new[] { 2, 1 }, new[] { 0, 230 }, new[] { 0, 202 }, new[] { 2, 1 },
            new[] { 0, 244 }, new[] { 0, 79 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 187 }, new[] { 0, 172 }, new[] { 2, 1 }, new[] { 0, 231 }, new[] { 0, 245 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 217 }, new[] { 0, 157 }, new[] { 2, 1 },
            new[] { 0, 95 }, new[] { 0, 232 }, new[] { 30, 1 }, new[] { 12, 1 }, new[] { 6, 1 },
            new[] { 2, 1 }, new[] { 0, 111 }, new[] { 2, 1 }, new[] { 0, 246 }, new[] { 0, 203 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 188 }, new[] { 0, 173 }, new[] { 0, 218 },
            new[] { 8, 1 }, new[] { 2, 1 }, new[] { 0, 247 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 126 }, new[] { 0, 127 }, new[] { 0, 142 }, new[] { 6, 1 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 158 }, new[] { 0, 174 }, new[] { 0, 204 }, new[] { 2, 1 }, new[] { 0, 248 },
            new[] { 0, 143 }, new[] { 18, 1 },
            new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 219 }, new[] { 0, 189 },
            new[] { 2, 1 }, new[] { 0, 234 }, new[] { 0, 249 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 159 }, new[] { 0, 235 }, new[] { 2, 1 }, new[] { 0, 190 }, new[] { 2, 1 },
            new[] { 0, 205 }, new[] { 0, 250 }, new[] { 14, 1 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 221 }, new[] { 0, 236 }, new[] { 6, 1 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 233 }, new[] { 0, 175 }, new[] { 0, 220 }, new[] { 2, 1 }, new[] { 0, 206 },
            new[] { 0, 251 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 191 },
            new[] { 0, 222 }, new[] { 2, 1 }, new[] { 0, 207 }, new[] { 0, 238 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 223 }, new[] { 0, 239 }, new[] { 2, 1 }, new[] { 0, 255 },
            new[] { 2, 1 }, new[] { 0, 237 }, new[] { 2, 1 }, new[] { 0, 253 }, new[] { 2, 1 },
            new[] { 0, 252 }, new[] { 0, 254 }
        };

        private static readonly int[][] ValTab14 = { new[] { 0, 0 } };

        private static readonly int[][] ValTab15 =
        {
            new[] { 16, 1 }, new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 0 }, new[] { 2, 1 }, new[] { 0, 16 },
            new[] { 0, 1 }, new[] { 2, 1 }, new[] { 0, 17 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 32 },
            new[] { 0, 2 }, new[] { 2, 1 }, new[] { 0, 33 }, new[] { 0, 18 }, new[] { 50, 1 },
            new[] { 16, 1 }, new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 34 }, new[] { 2, 1 },
            new[] { 0, 48 }, new[] { 0, 49 }, new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 19 },
            new[] { 2, 1 }, new[] { 0, 3 }, new[] { 0, 64 }, new[] { 2, 1 }, new[] { 0, 50 },
            new[] { 0, 35 }, new[] { 14, 1 }, new[] { 6, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 4 },
            new[] { 0, 20 }, new[] { 0, 65 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 51 },
            new[] { 0, 66 }, new[] { 2, 1 }, new[] { 0, 36 }, new[] { 0, 67 }, new[] { 10, 1 },
            new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 52 }, new[] { 2, 1 }, new[] { 0, 80 }, new[] { 0, 5 },
            new[] { 2, 1 }, new[] { 0, 81 }, new[] { 0, 21 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 82 }, new[] { 0, 37 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 68 },
            new[] { 0, 83 }, new[] { 0, 97 }, new[] { 90, 1 }, new[] { 36, 1 }, new[] { 18, 1 },
            new[] { 10, 1 }, new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 53 }, new[] { 2, 1 },
            new[] { 0, 96 }, new[] { 0, 6 }, new[] { 2, 1 }, new[] { 0, 22 }, new[] { 0, 98 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 38 }, new[] { 0, 84 }, new[] { 2, 1 },
            new[] { 0, 69 }, new[] { 0, 99 }, new[] { 10, 1 }, new[] { 6, 1 }, new[] { 2, 1 },
            new[] { 0, 54 }, new[] { 2, 1 }, new[] { 0, 112 }, new[] { 0, 7 }, new[] { 2, 1 },
            new[] { 0, 113 }, new[] { 0, 85 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 23 },
            new[] { 0, 100 }, new[] { 2, 1 }, new[] { 0, 114 }, new[] { 0, 39 }, new[] { 24, 1 },
            new[] { 16, 1 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 70 },
            new[] { 0, 115 }, new[] { 2, 1 }, new[] { 0, 55 }, new[] { 0, 101 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 86 }, new[] { 0, 128 }, new[] { 2, 1 }, new[] { 0, 8 },
            new[] { 0, 116 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 129 }, new[] { 0, 24 },
            new[] { 2, 1 }, new[] { 0, 130 }, new[] { 0, 40 }, new[] { 16, 1 }, new[] { 8, 1 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 71 }, new[] { 0, 102 }, new[] { 2, 1 },
            new[] { 0, 131 }, new[] { 0, 56 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 117 },
            new[] { 0, 87 }, new[] { 2, 1 }, new[] { 0, 132 }, new[] { 0, 72 }, new[] { 6, 1 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 144 }, new[] { 0, 25 }, new[] { 0, 145 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 146 }, new[] { 0, 118 }, new[] { 2, 1 },
            new[] { 0, 103 }, new[] { 0, 41 }, new[] { 92, 1 }, new[] { 36, 1 }, new[] { 18, 1 },
            new[] { 10, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 133 }, new[] { 0, 88 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 9 }, new[] { 0, 119 }, new[] { 0, 147 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 57 }, new[] { 0, 148 }, new[] { 2, 1 },
            new[] { 0, 73 }, new[] { 0, 134 }, new[] { 10, 1 }, new[] { 6, 1 }, new[] { 2, 1 },
            new[] { 0, 104 }, new[] { 2, 1 }, new[] { 0, 160 }, new[] { 0, 10 }, new[] { 2, 1 },
            new[] { 0, 161 }, new[] { 0, 26 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 162 },
            new[] { 0, 42 }, new[] { 2, 1 }, new[] { 0, 149 }, new[] { 0, 89 }, new[] { 26, 1 },
            new[] { 14, 1 }, new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 163 }, new[] { 2, 1 },
            new[] { 0, 58 }, new[] { 0, 135 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 120 },
            new[] { 0, 164 }, new[] { 2, 1 }, new[] { 0, 74 }, new[] { 0, 150 }, new[] { 6, 1 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 105 }, new[] { 0, 176 }, new[] { 0, 177 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 27 }, new[] { 0, 165 }, new[] { 0, 178 },
            new[] { 14, 1 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 90 },
            new[] { 0, 43 }, new[] { 2, 1 }, new[] { 0, 136 }, new[]
            {
                0, 151
            },
            new[] { 2, 1 }, new[] { 0, 179 }, new[] { 2, 1 }, new[] { 0, 121 }, new[] { 0, 59 },
            new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 106 }, new[] { 0, 180 },
            new[] { 2, 1 }, new[] { 0, 75 }, new[] { 0, 193 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 152 }, new[] { 0, 137 }, new[] { 2, 1 }, new[] { 0, 28 }, new[] { 0, 181 },
            new[] { 80, 1 }, new[] { 34, 1 }, new[] { 16, 1 }, new[] { 6, 1 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 91 }, new[] { 0, 44 }, new[] { 0, 194 }, new[] { 6, 1 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 11 }, new[] { 0, 192 }, new[] { 0, 166 },
            new[] { 2, 1 }, new[] { 0, 167 }, new[] { 0, 122 }, new[] { 10, 1 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 195 }, new[] { 0, 60 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 12 }, new[] { 0, 153 }, new[] { 0, 182 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 107 }, new[] { 0, 196 }, new[] { 2, 1 }, new[] { 0, 76 }, new[] { 0, 168 },
            new[] { 20, 1 }, new[] { 10, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 138 },
            new[] { 0, 197 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 208 }, new[] { 0, 92 },
            new[] { 0, 209 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 183 }, new[] { 0, 123 },
            new[] { 2, 1 }, new[] { 0, 29 }, new[] { 2, 1 }, new[] { 0, 13 }, new[] { 0, 45 },
            new[] { 12, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 210 }, new[] { 0, 211 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 61 }, new[] { 0, 198 }, new[] { 2, 1 },
            new[] { 0, 108 }, new[] { 0, 169 }, new[] { 6, 1 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 154 }, new[] { 0, 184 }, new[] { 0, 212 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 139 }, new[] { 0, 77 }, new[] { 2, 1 }, new[] { 0, 199 }, new[] { 0, 124 },
            new[] { 68, 1 }, new[] { 34, 1 }, new[] { 18, 1 }, new[] { 10, 1 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 213 }, new[] { 0, 93 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 224 }, new[] { 0, 14 }, new[]
            {
                0,
                225
            },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 30 }, new[] { 0, 226 }, new[] { 2, 1 },
            new[] { 0, 170 }, new[] { 0, 46 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 185 }, new[] { 0, 155 }, new[] { 2, 1 }, new[] { 0, 227 }, new[] { 0, 214 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 109 }, new[] { 0, 62 }, new[] { 2, 1 },
            new[] { 0, 200 }, new[] { 0, 140 }, new[] { 16, 1 }, new[] { 8, 1 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 228 }, new[] { 0, 78 }, new[] { 2, 1 }, new[] { 0, 215 },
            new[] { 0, 125 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 229 }, new[] { 0, 186 },
            new[] { 2, 1 }, new[] { 0, 171 }, new[] { 0, 94 }, new[] { 8, 1 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 201 }, new[] { 0, 156 }, new[] { 2, 1 }, new[] { 0, 241 },
            new[] { 0, 31 }, new[] { 6, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 240 },
            new[] { 0, 110 }, new[] { 0, 242 }, new[] { 2, 1 }, new[] { 0, 47 }, new[] { 0, 230 },
            new[] { 38, 1 }, new[] { 18, 1 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 216 }, new[] { 0, 243 }, new[] { 2, 1 }, new[] { 0, 63 }, new[] { 0, 244 },
            new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 79 }, new[] { 2, 1 }, new[] { 0, 141 },
            new[] { 0, 217 }, new[] { 2, 1 }, new[] { 0, 187 }, new[] { 0, 202 }, new[] { 8, 1 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 172 }, new[] { 0, 231 }, new[] { 2, 1 },
            new[] { 0, 126 }, new[] { 0, 245 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 157 }, new[] { 0, 95 }, new[] { 2, 1 }, new[] { 0, 232 }, new[] { 0, 142 },
            new[] { 2, 1 }, new[] { 0, 246 }, new[] { 0, 203 }, new[] { 34, 1 }, new[] { 18, 1 },
            new[] { 10, 1 }, new[] { 6, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 15 },
            new[] { 0, 174 }, new[] { 0, 111 }, new[] { 2, 1 }, new[] { 0, 188 }, new[] { 0, 218 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 173 }, new[] { 0, 247 }, new[] { 2, 1 },
            new[] { 0, 127 }, new[] { 0, 233 }, new[]
            {
                8, 1
            },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 158 }, new[] { 0, 204 }, new[] { 2, 1 },
            new[] { 0, 248 }, new[] { 0, 143 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 219 },
            new[] { 0, 189 }, new[] { 2, 1 }, new[] { 0, 234 }, new[] { 0, 249 }, new[] { 16, 1 },
            new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 159 }, new[] { 0, 220 },
            new[] { 2, 1 }, new[] { 0, 205 }, new[] { 0, 235 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 190 }, new[] { 0, 250 }, new[] { 2, 1 }, new[] { 0, 175 }, new[] { 0, 221 },
            new[] { 14, 1 }, new[] { 6, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 236 },
            new[] { 0, 206 }, new[] { 0, 251 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 191 },
            new[] { 0, 237 }, new[] { 2, 1 }, new[] { 0, 222 }, new[] { 0, 252 }, new[] { 6, 1 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 207 }, new[] { 0, 253 }, new[] { 0, 238 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 223 }, new[] { 0, 254 }, new[] { 2, 1 },
            new[] { 0, 239 }, new[] { 0, 255 }
        };

        private static readonly int[][] ValTab16 =
        {
            new[] { 2, 1 }, new[] { 0, 0 }, new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 16 }, new[] { 2, 1 },
            new[] { 0, 1 }, new[] { 0, 17 }, new[] { 42, 1 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 32 }, new[] { 0, 2 }, new[] { 2, 1 }, new[] { 0, 33 }, new[] { 0, 18 },
            new[] { 10, 1 }, new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 34 }, new[] { 2, 1 },
            new[] { 0, 48 }, new[] { 0, 3 }, new[] { 2, 1 }, new[] { 0, 49 }, new[] { 0, 19 },
            new[] { 10, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 50 }, new[] { 0, 35 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 64 }, new[] { 0, 4 }, new[] { 0, 65 }, new[] { 6, 1 },
            new[] { 2, 1 }, new[] { 0, 20 }, new[] { 2, 1 }, new[] { 0, 51 }, new[] { 0, 66 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 36 }, new[] { 0, 80 }, new[] { 2, 1 },
            new[] { 0, 67 }, new[] { 0, 52 }, new[] { 138, 1 }, new[] { 40, 1 }, new[] { 16, 1 },
            new[] { 6, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 5 }, new[] { 0, 21 }, new[] { 0, 81 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 82 }, new[] { 0, 37 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 68 }, new[] { 0, 53 }, new[] { 0, 83 }, new[] { 10, 1 }, new[] { 6, 1 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 96 }, new[] { 0, 6 }, new[] { 0, 97 }, new[] { 2, 1 },
            new[] { 0, 22 }, new[] { 0, 98 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 38 }, new[] { 0, 84 }, new[] { 2, 1 }, new[] { 0, 69 }, new[] { 0, 99 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 54 }, new[] { 0, 112 }, new[] { 0, 113 },
            new[] { 40, 1 }, new[] { 18, 1 }, new[] { 8, 1 }, new[] { 2, 1 }, new[] { 0, 23 },
            new[] { 2, 1 }, new[] { 0, 7 }, new[] { 2, 1 }, new[] { 0, 85 }, new[] { 0, 100 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 114 }, new[] { 0, 39 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 70 }, new[] { 0, 101 }, new[] { 0, 115 }, new[] { 10, 1 },
            new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 55 }, new[] { 2, 1 }, new[] { 0, 86 }, new[] { 0, 8 },
            new[] { 2, 1 }, new[] { 0, 128 },
            new[] { 0, 129 }, new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 24 }, new[] { 2, 1 },
            new[] { 0, 116 }, new[] { 0, 71 }, new[] { 2, 1 }, new[] { 0, 130 }, new[] { 2, 1 },
            new[] { 0, 40 }, new[] { 0, 102 }, new[] { 24, 1 }, new[] { 14, 1 }, new[] { 8, 1 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 131 }, new[] { 0, 56 }, new[] { 2, 1 },
            new[] { 0, 117 }, new[] { 0, 132 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 72 },
            new[] { 0, 144 }, new[] { 0, 145 }, new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 25 },
            new[] { 2, 1 }, new[] { 0, 9 }, new[] { 0, 118 }, new[] { 2, 1 }, new[] { 0, 146 },
            new[] { 0, 41 }, new[] { 14, 1 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 133 }, new[] { 0, 88 }, new[] { 2, 1 }, new[] { 0, 147 }, new[] { 0, 57 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 160 }, new[] { 0, 10 }, new[] { 0, 26 },
            new[] { 8, 1 }, new[] { 2, 1 }, new[] { 0, 162 }, new[] { 2, 1 }, new[] { 0, 103 },
            new[] { 2, 1 }, new[] { 0, 87 }, new[] { 0, 73 }, new[] { 6, 1 }, new[] { 2, 1 },
            new[] { 0, 148 }, new[] { 2, 1 }, new[] { 0, 119 }, new[] { 0, 134 }, new[] { 2, 1 },
            new[] { 0, 161 }, new[] { 2, 1 }, new[] { 0, 104 }, new[] { 0, 149 }, new[] { 220, 1 },
            new[] { 126, 1 }, new[] { 50, 1 }, new[] { 26, 1 }, new[] { 12, 1 }, new[] { 6, 1 },
            new[] { 2, 1 }, new[] { 0, 42 }, new[] { 2, 1 }, new[] { 0, 89 }, new[] { 0, 58 },
            new[] { 2, 1 }, new[] { 0, 163 }, new[] { 2, 1 }, new[] { 0, 135 }, new[] { 0, 120 },
            new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 164 }, new[] { 0, 74 },
            new[] { 2, 1 }, new[] { 0, 150 }, new[] { 0, 105 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 176 }, new[] { 0, 11 }, new[] { 0, 177 }, new[] { 10, 1 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 27 }, new[] { 0, 178 }, new[] { 2, 1 }, new[] { 0, 43 },
            new[] { 2, 1 }, new[] { 0, 165 }, new[] { 0, 90 }, new[] { 6, 1 },
            new[] { 2, 1 }, new[] { 0, 179 }, new[] { 2, 1 }, new[] { 0, 166 }, new[] { 0, 106 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 180 }, new[] { 0, 75 }, new[] { 2, 1 },
            new[] { 0, 12 }, new[] { 0, 193 }, new[] { 30, 1 }, new[] { 14, 1 }, new[] { 6, 1 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 181 }, new[] { 0, 194 }, new[] { 0, 44 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 167 }, new[] { 0, 195 }, new[] { 2, 1 },
            new[] { 0, 107 }, new[] { 0, 196 }, new[] { 8, 1 }, new[] { 2, 1 }, new[] { 0, 29 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 136 }, new[] { 0, 151 }, new[] { 0, 59 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 209 }, new[] { 0, 210 }, new[] { 2, 1 },
            new[] { 0, 45 }, new[] { 0, 211 }, new[] { 18, 1 }, new[] { 6, 1 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 30 }, new[] { 0, 46 }, new[] { 0, 226 }, new[] { 6, 1 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 121 }, new[] { 0, 152 }, new[] { 0, 192 },
            new[] { 2, 1 }, new[] { 0, 28 }, new[] { 2, 1 }, new[] { 0, 137 }, new[] { 0, 91 },
            new[] { 14, 1 }, new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 60 }, new[] { 2, 1 },
            new[] { 0, 122 }, new[] { 0, 182 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 76 },
            new[] { 0, 153 }, new[] { 2, 1 }, new[] { 0, 168 }, new[] { 0, 138 }, new[] { 6, 1 },
            new[] { 2, 1 }, new[] { 0, 13 }, new[] { 2, 1 }, new[] { 0, 197 }, new[] { 0, 92 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 61 }, new[] { 0, 198 }, new[] { 2, 1 },
            new[] { 0, 108 }, new[] { 0, 154 }, new[] { 88, 1 }, new[] { 86, 1 }, new[] { 36, 1 },
            new[] { 16, 1 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 139 },
            new[] { 0, 77 }, new[] { 2, 1 }, new[] { 0, 199 }, new[] { 0, 124 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 213 }, new[] { 0, 93 }, new[] { 2, 1 }, new[] { 0, 224 },
            new[] { 0, 14 }, new[] { 8, 1 }, new[] { 2, 1 }, new[] { 0, 227 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 208 }, new[] { 0, 183 },
            new[] { 0, 123 }, new[] { 6, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 169 },
            new[] { 0, 184 }, new[] { 0, 212 }, new[] { 2, 1 }, new[] { 0, 225 }, new[] { 2, 1 },
            new[] { 0, 170 }, new[] { 0, 185 }, new[] { 24, 1 }, new[] { 10, 1 }, new[] { 6, 1 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 155 }, new[] { 0, 214 }, new[] { 0, 109 },
            new[] { 2, 1 }, new[] { 0, 62 }, new[] { 0, 200 }, new[] { 6, 1 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 140 }, new[] { 0, 228 }, new[] { 0, 78 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 215 }, new[] { 0, 229 }, new[] { 2, 1 }, new[] { 0, 186 },
            new[] { 0, 171 }, new[] { 12, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 156 },
            new[] { 0, 230 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 110 }, new[] { 0, 216 },
            new[] { 2, 1 }, new[] { 0, 141 }, new[] { 0, 187 }, new[] { 8, 1 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 231 }, new[] { 0, 157 }, new[] { 2, 1 }, new[] { 0, 232 },
            new[] { 0, 142 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 203 }, new[] { 0, 188 },
            new[] { 0, 158 }, new[] { 0, 241 }, new[] { 2, 1 }, new[] { 0, 31 }, new[] { 2, 1 },
            new[] { 0, 15 }, new[] { 0, 47 }, new[] { 66, 1 }, new[] { 56, 1 }, new[] { 2, 1 },
            new[] { 0, 242 }, new[] { 52, 1 }, new[] { 50, 1 }, new[] { 20, 1 }, new[] { 8, 1 },
            new[] { 2, 1 }, new[] { 0, 189 }, new[] { 2, 1 }, new[] { 0, 94 }, new[] { 2, 1 },
            new[] { 0, 125 }, new[] { 0, 201 }, new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 202 },
            new[] { 2, 1 }, new[] { 0, 172 }, new[] { 0, 126 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 218 }, new[] { 0, 173 }, new[] { 0, 204 }, new[] { 10, 1 }, new[] { 6, 1 },
            new[] { 2, 1 }, new[] { 0, 174 }, new[] { 2, 1 }, new[] { 0, 219 }, new[] { 0, 220 },
            new[] { 2, 1 }, new[] { 0, 205 }, new[] { 0, 190 }, new[] { 6, 1 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 235 }, new[] { 0, 237 }, new[] { 0, 238 }, new[] { 6, 1 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[]
            {
                0,
                217
            },
            new[] { 0, 234 }, new[] { 0, 233 }, new[] { 2, 1 }, new[] { 0, 222 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 221 }, new[] { 0, 236 }, new[] { 0, 206 }, new[] { 0, 63 },
            new[] { 0, 240 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 243 }, new[] { 0, 244 },
            new[] { 2, 1 }, new[] { 0, 79 }, new[] { 2, 1 }, new[] { 0, 245 }, new[] { 0, 95 },
            new[] { 10, 1 }, new[] { 2, 1 }, new[] { 0, 255 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 246 }, new[] { 0, 111 }, new[] { 2, 1 }, new[] { 0, 247 }, new[] { 0, 127 },
            new[] { 12, 1 }, new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 143 }, new[] { 2, 1 },
            new[] { 0, 248 }, new[] { 0, 249 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 159 },
            new[] { 0, 250 }, new[] { 0, 175 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 251 }, new[] { 0, 191 }, new[] { 2, 1 }, new[] { 0, 252 }, new[] { 0, 207 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 253 }, new[] { 0, 223 }, new[] { 2, 1 },
            new[] { 0, 254 }, new[] { 0, 239 }
        };

        private static readonly int[][] ValTab24 =
        {
            new[] { 60, 1 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 0 }, new[] { 0, 16 },
            new[] { 2, 1 }, new[] { 0, 1 }, new[] { 0, 17 }, new[] { 14, 1 }, new[] { 6, 1 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 32 }, new[] { 0, 2 }, new[] { 0, 33 }, new[] { 2, 1 },
            new[] { 0, 18 }, new[] { 2, 1 }, new[] { 0, 34 }, new[] { 2, 1 }, new[] { 0, 48 },
            new[] { 0, 3 }, new[] { 14, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 49 },
            new[] { 0, 19 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 50 }, new[] { 0, 35 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 64 }, new[] { 0, 4 }, new[] { 0, 65 }, new[] { 8, 1 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 20 }, new[] { 0, 51 }, new[] { 2, 1 },
            new[] { 0, 66 }, new[] { 0, 36 }, new[] { 6, 1 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 67 }, new[] { 0, 52 }, new[] { 0, 81 }, new[] { 6, 1 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 80 }, new[] { 0, 5 }, new[] { 0, 21 }, new[] { 2, 1 },
            new[] { 0, 82 }, new[] { 0, 37 }, new[] { 250, 1 }, new[] { 98, 1 }, new[] { 34, 1 },
            new[] { 18, 1 }, new[] { 10, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 68 },
            new[] { 0, 83 }, new[] { 2, 1 }, new[] { 0, 53 }, new[] { 2, 1 }, new[] { 0, 96 },
            new[] { 0, 6 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 97 }, new[] { 0, 22 }, new[] { 2, 1 },
            new[] { 0, 98 }, new[] { 0, 38 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 84 }, new[] { 0, 69 }, new[] { 2, 1 }, new[] { 0, 99 }, new[] { 0, 54 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 113 }, new[] { 0, 85 }, new[] { 2, 1 },
            new[] { 0, 100 }, new[] { 0, 70 }, new[] { 32, 1 }, new[] { 14, 1 }, new[] { 6, 1 },
            new[] { 2, 1 }, new[] { 0, 114 }, new[] { 2, 1 }, new[] { 0, 39 }, new[] { 0, 55 },
            new[] { 2, 1 }, new[] { 0, 115 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 112 },
            new[] { 0, 7 }, new[] { 0, 23 }, new[] { 10, 1 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 101 }, new[] { 0, 86 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 128 },
            new[] { 0, 8 }, new[] { 0, 129 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 116 },
            new[] { 0, 71 }, new[] { 2, 1 }, new[] { 0, 24 }, new[] { 0, 130 }, new[] { 16, 1 },
            new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 40 }, new[] { 0, 102 },
            new[] { 2, 1 }, new[] { 0, 131 }, new[] { 0, 56 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 117 }, new[] { 0, 87 }, new[] { 2, 1 }, new[] { 0, 132 }, new[] { 0, 72 },
            new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 145 }, new[] { 0, 25 },
            new[] { 2, 1 }, new[] { 0, 146 }, new[] { 0, 118 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 103 }, new[] { 0, 41 }, new[] { 2, 1 }, new[] { 0, 133 }, new[] { 0, 88 },
            new[] { 92, 1 }, new[] { 34, 1 }, new[] { 16, 1 }, new[] { 8, 1 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 147 }, new[] { 0, 57 }, new[] { 2, 1 }, new[] { 0, 148 },
            new[] { 0, 73 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 119 }, new[] { 0, 134 },
            new[] { 2, 1 }, new[] { 0, 104 }, new[] { 0, 161 }, new[] { 8, 1 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 162 }, new[] { 0, 42 }, new[] { 2, 1 }, new[] { 0, 149 },
            new[] { 0, 89 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 163 }, new[] { 0, 58 },
            new[] { 2, 1 }, new[] { 0, 135 }, new[] { 2, 1 }, new[] { 0, 120 }, new[] { 0, 74 },
            new[] { 22, 1 }, new[] { 12, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 164 },
            new[] { 0, 150 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 105 }, new[] { 0, 177 },
            new[] { 2, 1 }, new[] { 0, 27 }, new[] { 0, 165 }, new[] { 6, 1 }, new[] { 2, 1 },
            new[] { 0, 178 }, new[] { 2, 1 }, new[] { 0, 90 }, new[] { 0, 43 }, new[] { 2, 1 },
            new[] { 0, 136 }, new[] { 0, 179 }, new[] { 16, 1 }, new[] { 10, 1 }, new[] { 6, 1 },
            new[] { 2, 1 }, new[] { 0, 144 }, new[] { 2, 1 }, new[] { 0, 9 }, new[] { 0, 160 },
            new[] { 2, 1 }, new[] { 0, 151 }, new[] { 0, 121 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 166 }, new[] { 0, 106 }, new[] { 0, 180 }, new[] { 12, 1 },
            new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 26 }, new[] { 2, 1 }, new[] { 0, 10 },
            new[] { 0, 176 }, new[] { 2, 1 }, new[] { 0, 59 }, new[] { 2, 1 }, new[] { 0, 11 },
            new[] { 0, 192 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 75 }, new[] { 0, 193 },
            new[] { 2, 1 }, new[] { 0, 152 }, new[] { 0, 137 }, new[] { 67, 1 }, new[] { 34, 1 },
            new[] { 16, 1 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 28 },
            new[] { 0, 181 }, new[] { 2, 1 }, new[] { 0, 91 }, new[] { 0, 194 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 44 }, new[] { 0, 167 }, new[] { 2, 1 }, new[] { 0, 122 },
            new[] { 0, 195 }, new[] { 10, 1 }, new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 60 },
            new[] { 2, 1 }, new[] { 0, 12 }, new[] { 0, 208 }, new[] { 2, 1 }, new[] { 0, 182 },
            new[] { 0, 107 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 196 }, new[] { 0, 76 },
            new[] { 2, 1 }, new[] { 0, 153 }, new[] { 0, 168 }, new[] { 16, 1 }, new[] { 8, 1 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 138 }, new[] { 0, 197 }, new[] { 2, 1 },
            new[] { 0, 92 }, new[] { 0, 209 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 183 },
            new[] { 0, 123 }, new[] { 2, 1 }, new[] { 0, 29 }, new[] { 0, 210 }, new[] { 9, 1 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 45 }, new[] { 0, 211 }, new[] { 2, 1 },
            new[] { 0, 61 }, new[] { 0, 198 }, new[] { 85, 250 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 108 }, new[] { 0, 169 }, new[] { 2, 1 }, new[] { 0, 154 }, new[] { 0, 212 },
            new[] { 32, 1 }, new[] { 16, 1 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 184 }, new[] { 0, 139 }, new[] { 2, 1 }, new[] { 0, 77 }, new[] { 0, 199 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 124 }, new[] { 0, 213 }, new[] { 2, 1 },
            new[] { 0, 93 }, new[] { 0, 225 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 30 }, new[] { 0, 226 }, new[]
            {
                2, 1
            },
            new[] { 0, 170 }, new[] { 0, 185 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 155 },
            new[] { 0, 227 }, new[] { 2, 1 }, new[] { 0, 214 }, new[] { 0, 109 }, new[] { 20, 1 },
            new[] { 10, 1 }, new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 62 }, new[] { 2, 1 },
            new[] { 0, 46 }, new[] { 0, 78 }, new[] { 2, 1 }, new[] { 0, 200 }, new[] { 0, 140 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 228 }, new[] { 0, 215 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 125 }, new[] { 0, 171 }, new[] { 0, 229 }, new[] { 10, 1 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 186 }, new[] { 0, 94 }, new[] { 2, 1 },
            new[] { 0, 201 }, new[] { 2, 1 }, new[] { 0, 156 }, new[] { 0, 110 }, new[] { 8, 1 },
            new[] { 2, 1 }, new[] { 0, 230 }, new[] { 2, 1 }, new[] { 0, 13 }, new[] { 2, 1 },
            new[] { 0, 224 }, new[] { 0, 14 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 216 },
            new[] { 0, 141 }, new[] { 2, 1 }, new[] { 0, 187 }, new[] { 0, 202 }, new[] { 74, 1 },
            new[] { 2, 1 }, new[] { 0, 255 }, new[] { 64, 1 }, new[] { 58, 1 }, new[] { 32, 1 },
            new[] { 16, 1 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 172 },
            new[] { 0, 231 }, new[] { 2, 1 }, new[] { 0, 126 }, new[] { 0, 217 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 157 }, new[] { 0, 232 }, new[] { 2, 1 }, new[] { 0, 142 },
            new[] { 0, 203 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 188 },
            new[] { 0, 218 }, new[] { 2, 1 }, new[] { 0, 173 }, new[] { 0, 233 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 158 }, new[] { 0, 204 }, new[] { 2, 1 }, new[] { 0, 219 },
            new[] { 0, 189 }, new[] { 16, 1 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 234 }, new[] { 0, 174 }, new[] { 2, 1 }, new[] { 0, 220 }, new[] { 0, 205 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 235 }, new[] { 0, 190 }, new[] { 2, 1 },
            new[] { 0, 221 }, new[] { 0, 236 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 206 }, new[] { 0, 237 }, new[] { 2, 1 },
            new[] { 0, 222 }, new[] { 0, 238 }, new[] { 0, 15 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 240 }, new[] { 0, 31 }, new[] { 0, 241 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 242 }, new[] { 0, 47 }, new[] { 2, 1 }, new[] { 0, 243 }, new[] { 0, 63 },
            new[] { 18, 1 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 244 },
            new[] { 0, 79 }, new[] { 2, 1 }, new[] { 0, 245 }, new[] { 0, 95 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 246 }, new[] { 0, 111 }, new[] { 2, 1 }, new[] { 0, 247 },
            new[] { 2, 1 }, new[] { 0, 127 }, new[] { 0, 143 }, new[] { 10, 1 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 248 }, new[] { 0, 249 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 159 }, new[] { 0, 175 }, new[] { 0, 250 }, new[] { 8, 1 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 251 }, new[] { 0, 191 }, new[] { 2, 1 }, new[] { 0, 252 },
            new[] { 0, 207 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 253 }, new[] { 0, 223 },
            new[] { 2, 1 }, new[] { 0, 254 }, new[] { 0, 239 }
        };

        private static readonly int[][] ValTab32 =
        {
            new[] { 2, 1 }, new[] { 0, 0 }, new[] { 8, 1 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 8 }, new[] { 0, 4 }, new[] { 2, 1 }, new[] { 0, 1 },
            new[] { 0, 2 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 12 }, new[] { 0, 10 },
            new[] { 2, 1 }, new[] { 0, 3 }, new[] { 0, 6 }, new[] { 6, 1 }, new[] { 2, 1 }, new[] { 0, 9 },
            new[] { 2, 1 }, new[] { 0, 5 }, new[] { 0, 7 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 14 },
            new[] { 0, 13 }, new[] { 2, 1 }, new[] { 0, 15 }, new[] { 0, 11 }
        };

        private static readonly int[][] ValTab33 =
        {
            new[] { 16, 1 }, new[] { 8, 1 }, new[] { 4, 1 },
            new[] { 2, 1 }, new[] { 0, 0 }, new[] { 0, 1 }, new[] { 2, 1 }, new[] { 0, 2 }, new[] { 0, 3 },
            new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 4 }, new[] { 0, 5 }, new[] { 2, 1 }, new[] { 0, 6 },
            new[] { 0, 7 }, new[] { 8, 1 }, new[] { 4, 1 }, new[] { 2, 1 }, new[] { 0, 8 }, new[] { 0, 9 },
            new[] { 2, 1 }, new[] { 0, 10 }, new[] { 0, 11 }, new[] { 4, 1 }, new[] { 2, 1 },
            new[] { 0, 12 }, new[] { 0, 13 }, new[] { 2, 1 }, new[] { 0, 14 }, new[] { 0, 15 }
        };

        public static Huffman[] ht;             //Simulate extern struct
        private int[] hlen;                     //pointer to array[xlen][ylen]
        private readonly int linbits;           //number of linbits
        private int linmax;                     //max number to be stored in linbits
        private int ref_Renamed;                //a positive value indicates a reference
        private int[] table;                    //pointer to array[xlen][ylen]
        private readonly char tablename0 = ' '; //string, containing table_description
        private readonly char tablename1 = ' '; //string, containing table_description
        private char tablename2 = ' ';          //string, containing table_description
        private readonly int treelen;           //length of decoder tree
        private readonly int[][] val;           //decoder tree
        private readonly int xlen;              //max. x-index+
        private readonly int ylen;              //max. y-index+

        static Huffman()
        {
        }

        /// <summary>
        ///     Big Constructor : Computes all Huffman Tables.
        /// </summary>
        private Huffman
        (
            string S,
            int XLEN,
            int YLEN,
            int LINBITS,
            int LINMAX,
            int REF,
            int[] TABLE,
            int[] HLEN,
            int[][] VAL,
            int TREELEN
        )
        {
            tablename0 = S[0];
            tablename1 = S[1];
            tablename2 = S[2];
            xlen = XLEN;
            ylen = YLEN;
            linbits = LINBITS;
            linmax = LINMAX;
            ref_Renamed = REF;
            table = TABLE;
            hlen = HLEN;
            val = VAL;
            treelen = TREELEN;
        }

        /// <summary>
        ///     Do the huffman-decoding.
        ///     NOTE: for counta, countb -the 4 bit value is returned in y, discard x.
        /// </summary>
        public static int Decode
        (
            Huffman h,
            int[] x,
            int[] y,
            int[] v,
            int[] w,
            BitReserve br
        )
        {
            // array of all huffcodtable headers
            // 0..31 Huffman code table 0..31
            // 32,33 count1-tables

            int dmask = 1 << (4 * 8 - 1);
            int point = 0;
            int error = 1;
            int level = dmask;

            if (h.val == null)
            {
                return 2;
            }

            /* table 0 needs no bits */
            if (h.treelen == 0)
            {
                x[0] = y[0] = 0;

                return 0;
            }

            /* Lookup in Huffman table. */

            /*int bitsAvailable = 0;     
            int bitIndex = 0;
            
            int bits[] = bitbuf;*/
            do
            {
                if (h.val[point][0] == 0)
                {
                    /*end of tree*/
                    x[0] = SupportClass.URShift(h.val[point][1], 4);
                    y[0] = h.val[point][1] & 0xf;
                    error = 0;

                    break;
                }

                // hget1bit() is called thousands of times, and so needs to be
                // ultra fast. 
                /*
                if (bitIndex==bitsAvailable)
                {
                bitsAvailable = br.readBits(bits, 32);            
                bitIndex = 0;
                }
                */
                //if (bits[bitIndex++]!=0)
                if (br.ReadOneBit() != 0)
                {
                    while (h.val[point][1] >= MXOFF)
                    {
                        point += h.val[point][1];
                    }

                    point += h.val[point][1];
                }
                else
                {
                    while (h.val[point][0] >= MXOFF)
                    {
                        point += h.val[point][0];
                    }

                    point += h.val[point][0];
                }

                level = SupportClass.URShift(level, 1);
                // MDM: ht[0] is always 0;
            } while (level != 0 || point < 0);

            // put back any bits not consumed
            /*    
            int unread = (bitsAvailable-bitIndex);
            if (unread>0)
            br.rewindNbits(unread);
            */
            /* Process sign encodings for quadruples tables. */
            // System.out.println(h.tablename);
            if (h.tablename0 == '3' && (h.tablename1 == '2' || h.tablename1 == '3'))
            {
                v[0] = (y[0] >> 3) & 1;
                w[0] = (y[0] >> 2) & 1;
                x[0] = (y[0] >> 1) & 1;
                y[0] = y[0] & 1;

                /* v, w, x and y are reversed in the bitstream.
                switch them around to make test bistream work. */

                if (v[0] != 0)
                {
                    if (br.ReadOneBit() != 0)
                    {
                        v[0] = -v[0];
                    }
                }

                if (w[0] != 0)
                {
                    if (br.ReadOneBit() != 0)
                    {
                        w[0] = -w[0];
                    }
                }

                if (x[0] != 0)
                {
                    if (br.ReadOneBit() != 0)
                    {
                        x[0] = -x[0];
                    }
                }

                if (y[0] != 0)
                {
                    if (br.ReadOneBit() != 0)
                    {
                        y[0] = -y[0];
                    }
                }
            }
            else
            {
                // Process sign and escape encodings for dual tables.
                // x and y are reversed in the test bitstream.
                // Reverse x and y here to make test bitstream work.

                if (h.linbits != 0)
                {
                    if (h.xlen - 1 == x[0])
                    {
                        x[0] += br.ReadBits(h.linbits);
                    }
                }

                if (x[0] != 0)
                {
                    if (br.ReadOneBit() != 0)
                    {
                        x[0] = -x[0];
                    }
                }

                if (h.linbits != 0)
                {
                    if (h.ylen - 1 == y[0])
                    {
                        y[0] += br.ReadBits(h.linbits);
                    }
                }

                if (y[0] != 0)
                {
                    if (br.ReadOneBit() != 0)
                    {
                        y[0] = -y[0];
                    }
                }
            }

            return error;
        }

        public static void Initialize()
        {
            if (ht != null)
            {
                return;
            }

            ht = new Huffman[HTN];

            ht[0] = new Huffman
            (
                "0  ",
                0,
                0,
                0,
                0,
                -1,
                null,
                null,
                ValTab0,
                0
            );

            ht[1] = new Huffman
            (
                "1  ",
                2,
                2,
                0,
                0,
                -1,
                null,
                null,
                ValTab1,
                7
            );

            ht[2] = new Huffman
            (
                "2  ",
                3,
                3,
                0,
                0,
                -1,
                null,
                null,
                ValTab2,
                17
            );

            ht[3] = new Huffman
            (
                "3  ",
                3,
                3,
                0,
                0,
                -1,
                null,
                null,
                ValTab3,
                17
            );

            ht[4] = new Huffman
            (
                "4  ",
                0,
                0,
                0,
                0,
                -1,
                null,
                null,
                ValTab4,
                0
            );

            ht[5] = new Huffman
            (
                "5  ",
                4,
                4,
                0,
                0,
                -1,
                null,
                null,
                ValTab5,
                31
            );

            ht[6] = new Huffman
            (
                "6  ",
                4,
                4,
                0,
                0,
                -1,
                null,
                null,
                ValTab6,
                31
            );

            ht[7] = new Huffman
            (
                "7  ",
                6,
                6,
                0,
                0,
                -1,
                null,
                null,
                ValTab7,
                71
            );

            ht[8] = new Huffman
            (
                "8  ",
                6,
                6,
                0,
                0,
                -1,
                null,
                null,
                ValTab8,
                71
            );

            ht[9] = new Huffman
            (
                "9  ",
                6,
                6,
                0,
                0,
                -1,
                null,
                null,
                ValTab9,
                71
            );

            ht[10] = new Huffman
            (
                "10 ",
                8,
                8,
                0,
                0,
                -1,
                null,
                null,
                ValTab10,
                127
            );

            ht[11] = new Huffman
            (
                "11 ",
                8,
                8,
                0,
                0,
                -1,
                null,
                null,
                ValTab11,
                127
            );

            ht[12] = new Huffman
            (
                "12 ",
                8,
                8,
                0,
                0,
                -1,
                null,
                null,
                ValTab12,
                127
            );

            ht[13] = new Huffman
            (
                "13 ",
                16,
                16,
                0,
                0,
                -1,
                null,
                null,
                ValTab13,
                511
            );

            ht[14] = new Huffman
            (
                "14 ",
                0,
                0,
                0,
                0,
                -1,
                null,
                null,
                ValTab14,
                0
            );

            ht[15] = new Huffman
            (
                "15 ",
                16,
                16,
                0,
                0,
                -1,
                null,
                null,
                ValTab15,
                511
            );

            ht[16] = new Huffman
            (
                "16 ",
                16,
                16,
                1,
                1,
                -1,
                null,
                null,
                ValTab16,
                511
            );

            ht[17] = new Huffman
            (
                "17 ",
                16,
                16,
                2,
                3,
                16,
                null,
                null,
                ValTab16,
                511
            );

            ht[18] = new Huffman
            (
                "18 ",
                16,
                16,
                3,
                7,
                16,
                null,
                null,
                ValTab16,
                511
            );

            ht[19] = new Huffman
            (
                "19 ",
                16,
                16,
                4,
                15,
                16,
                null,
                null,
                ValTab16,
                511
            );

            ht[20] = new Huffman
            (
                "20 ",
                16,
                16,
                6,
                63,
                16,
                null,
                null,
                ValTab16,
                511
            );

            ht[21] = new Huffman
            (
                "21 ",
                16,
                16,
                8,
                255,
                16,
                null,
                null,
                ValTab16,
                511
            );

            ht[22] = new Huffman
            (
                "22 ",
                16,
                16,
                10,
                1023,
                16,
                null,
                null,
                ValTab16,
                511
            );

            ht[23] = new Huffman
            (
                "23 ",
                16,
                16,
                13,
                8191,
                16,
                null,
                null,
                ValTab16,
                511
            );

            ht[24] = new Huffman
            (
                "24 ",
                16,
                16,
                4,
                15,
                -1,
                null,
                null,
                ValTab24,
                512
            );

            ht[25] = new Huffman
            (
                "25 ",
                16,
                16,
                5,
                31,
                24,
                null,
                null,
                ValTab24,
                512
            );

            ht[26] = new Huffman
            (
                "26 ",
                16,
                16,
                6,
                63,
                24,
                null,
                null,
                ValTab24,
                512
            );

            ht[27] = new Huffman
            (
                "27 ",
                16,
                16,
                7,
                127,
                24,
                null,
                null,
                ValTab24,
                512
            );

            ht[28] = new Huffman
            (
                "28 ",
                16,
                16,
                8,
                255,
                24,
                null,
                null,
                ValTab24,
                512
            );

            ht[29] = new Huffman
            (
                "29 ",
                16,
                16,
                9,
                511,
                24,
                null,
                null,
                ValTab24,
                512
            );

            ht[30] = new Huffman
            (
                "30 ",
                16,
                16,
                11,
                2047,
                24,
                null,
                null,
                ValTab24,
                512
            );

            ht[31] = new Huffman
            (
                "31 ",
                16,
                16,
                13,
                8191,
                24,
                null,
                null,
                ValTab24,
                512
            );

            ht[32] = new Huffman
            (
                "32 ",
                1,
                16,
                0,
                0,
                -1,
                null,
                null,
                ValTab32,
                31
            );

            ht[33] = new Huffman
            (
                "33 ",
                1,
                16,
                0,
                0,
                -1,
                null,
                null,
                ValTab33,
                31
            );
        }
    }
}