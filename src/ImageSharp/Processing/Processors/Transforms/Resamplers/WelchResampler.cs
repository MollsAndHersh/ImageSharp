// Copyright (c) Six Labors and contributors.
// Licensed under the GNU Affero General Public License, Version 3.

using System.Runtime.CompilerServices;
using SixLabors.ImageSharp.PixelFormats;

namespace SixLabors.ImageSharp.Processing.Processors.Transforms
{
    /// <summary>
    /// The function implements the welch algorithm.
    /// <see href="http://www.imagemagick.org/Usage/filter/"/>
    /// </summary>
    public readonly struct WelchResampler : IResampler
    {
        /// <inheritdoc/>
        public float Radius => 3;

        /// <inheritdoc/>
        [MethodImpl(InliningOptions.ShortMethod)]
        public float GetValue(float x)
        {
            if (x < 0F)
            {
                x = -x;
            }

            if (x < 3F)
            {
                return ImageMaths.SinC(x) * (1F - (x * x / 9F));
            }

            return 0F;
        }

        /// <inheritdoc/>
        [MethodImpl(InliningOptions.ShortMethod)]
        public void ApplyTransform<TPixel>(IResamplingTransformImageProcessor<TPixel> processor)
            where TPixel : unmanaged, IPixel<TPixel>
            => processor.ApplyTransform(in this);
    }
}
