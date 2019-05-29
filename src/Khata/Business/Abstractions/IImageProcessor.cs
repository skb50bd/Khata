using System.IO;

namespace Business.Abstractions
{
    public interface IImageProcessor
    {
        MemoryStream Resize(
            MemoryStream imageStream, 
            int          width,
            int          height);
    }
}