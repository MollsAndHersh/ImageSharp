// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Runtime.CompilerServices;
using SixLabors.ImageSharp.Memory;
using SixLabors.ImageSharp.PixelFormats;

namespace SixLabors.ImageSharp.Formats.Tiff
{
    /// <summary>
    /// Implements the 'BlackIsZero' photometric interpretation (optimised for bilevel images).
    /// </summary>
    internal static class BlackIsZero1TiffColor
    {
        /// <summary>
        /// Decodes pixel data using the current photometric interpretation.
        /// </summary>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <param name="data">The buffer to read image data from.</param>
        /// <param name="pixels">The image buffer to write pixels to.</param>
        /// <param name="left">The x-coordinate of the left-hand side of the image block.</param>
        /// <param name="top">The y-coordinate of the  top of the image block.</param>
        /// <param name="width">The width of the image block.</param>
        /// <param name="height">The height of the image block.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Decode<TPixel>(byte[] data, Buffer2D<TPixel> pixels, int left, int top, int width, int height)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            TPixel color = default(TPixel);

            uint offset = 0;

            for (int y = top; y < top + height; y++)
            {
                for (int x = left; x < left + width; x += 8)
                {
                    byte b = data[offset++];
                    int maxShift = Math.Min(left + width - x, 8);

                    for (int shift = 0; shift < maxShift; shift++)
                    {
                        int bit = (b >> (7 - shift)) & 1;
                        byte intensity = (bit == 1) ? (byte)255 : (byte)0;
                        color.FromRgba32(new Rgba32(intensity, intensity, intensity, 255));
                        pixels[x + shift, y] = color;
                    }
                }
            }
        }
    }
}
