using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
namespace HyperShop.Utility
{
    public class ImageTool
    {
        public static void Image_resize(string input_Image_Path, string output_Image_Path, int width = 592, int height = 592)
        {
            using (var image = Image.Load(input_Image_Path))
            {
                image.Mutate(x => x
                     .Resize(image.Width > width ? width : image.Width, image.Height > height ? height : image.Height));
                image.Save(output_Image_Path);
            }
        }
    }
}
