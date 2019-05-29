using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Net.Mime;
using Business.Abstractions;

namespace Business.Implementations
{
    public class ImageProcessor : IImageProcessor
    {
        public MemoryStream Resize(
            MemoryStream imageStream,
            int width,
            int height)
        {
            imageStream.Position = 0;
            var output = new MemoryStream();
            using (var image = Image.Load(imageStream))
            {
                image.Mutate(
                    x => x.Resize(width, height));
                image.SaveAsPng(output);
            }

            return output;
        }
    }
}
