using System.Text.RegularExpressions;
using GuessingGame.Infrastructure.Data;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Formats.Png;

namespace GuessingGame.Core.Domain.Images.Services
{
    public class ImageService : IImageService
    {
        private readonly GuessingGameDbContext _db;

        public ImageService(GuessingGameDbContext db)
        {
            _db = db;
        }
        public List<byte[]> ManualSliceImage(string drawnImage, IFormFile uploadedImage, string colors)
        {
            //Convert the drawimage and uploaded image to imagesharp image so we can process them
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                uploadedImage.CopyTo(ms);
                fileBytes = ms.ToArray();

            }
            var imageUser = SixLabors.ImageSharp.Image.Load<Rgba32>(fileBytes);

            byte[] byteBuffer = Convert.FromBase64String(Regex.Replace(drawnImage, @"^data:image\/[a-zA-Z]+;base64,", string.Empty));
            using SixLabors.ImageSharp.Image<Rgba32> image = SixLabors.ImageSharp.Image.Load<Rgba32>(byteBuffer);
            imageUser.Mutate(x => x.Resize(image.Width, image.Height, KnownResamplers.Lanczos3));

            //Creating a dictionary with the color as key and the image Rgba32 to fast retrieve given segments
            Dictionary<Rgba32, Image<Rgba32>> getImage = new Dictionary<Rgba32, Image<Rgba32>>();
            Dictionary<Rgba32, int> amountOfPixels = new Dictionary<Rgba32, int>();



            image.ProcessPixelRows(accessor =>
            {
                //Retrive the colors and insert it into the dictionary with a new image with the same height and width
                string[] colorCode = colors.Split(',');
                for (var i = 0; i < colorCode.Length - 1; i = i + 4)
                {
                    var r = Int32.Parse(colorCode[i]);
                    var g = Int32.Parse(colorCode[i + 1]);
                    var b = Int32.Parse(colorCode[i + 2]);
                    var a = Int32.Parse(colorCode[i + 3]);
                    Rgba32 colorUsed = new Rgba32((byte)r, (byte)g, (byte)b);
                    getImage.Add(colorUsed, new Image<Rgba32>(accessor.Width, accessor.Height));
                    amountOfPixels.Add(colorUsed, 0);
                }
                //Also adding the transparent color since the undrawm areas will be a segment
                getImage.Add(new Rgba32(0, 0, 0, 0), new Image<Rgba32>(accessor.Width, accessor.Height));
                amountOfPixels.Add(new Rgba32(0, 0, 0, 0), 0);

                //Loops through each pixek
                for (int i = 0; i < accessor.Width; i++)
                {
                    for (int j = 0; j < accessor.Height; j++)
                    {
                        Rgba32 pixel = image[i, j];
                        //Checks if pixel has a color that is used so it can retrieve given segment and copy the pixel color at that given position
                        if (getImage.ContainsKey(pixel))
                        {
                            //Sets the current pixel in segment to the pixel of the image uploaded from user
                            getImage[pixel][i, j] = imageUser[i, j];
                            amountOfPixels[pixel] += 1;
                        }
                    }
                }
            }
            );

            //Convert the image from Image to byte array (using Base64 now since we want to test the algorithm)
            List<byte[]> imagesByteArray = new List<byte[]>();
            foreach (var entry in getImage)
            {
                //Checks if a segment contain under 15 pixels of image. (overdrawn by another pixel or to small of a segment to consider it)
                if (amountOfPixels[entry.Key] < 15)
                {
                    continue;
                }
                MemoryStream ms1 = new MemoryStream();
                entry.Value.Save(ms1, new PngEncoder());
                var result = ms1.ToArray();
                imagesByteArray.Add(result);
            }


            //Returns the list of base64 or byte array
            return imagesByteArray;
        }


        public List<byte[]> SliceImageAutomatic(IFormFile image)
        {
            //Changes random segments number. So total segments will be horizontal*vertical
            Random rnd = new Random();
            var NumbersofSegmentsHorizontal = rnd.Next(6, 10);
            var NumbersofSegmentsVertical = rnd.Next(6, 10);


            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                image.CopyTo(ms);
                fileBytes = ms.ToArray();

            }
            var img = SixLabors.ImageSharp.Image.Load<Rgba32>(fileBytes);

            // wants to create an image from the user input. 

            // if the image is very small, should divide the image into fewer segments(?)
            if ((img.Height < 300) | (img.Width < 300))
            {
                NumbersofSegmentsHorizontal = rnd.Next(2, 4); ;
                NumbersofSegmentsVertical = rnd.Next(2, 4);
            }

            // makes an empty array which will contain the segments from the image, has the size like number of segments. 
            var imgList = new List<Image<Rgba32>>();

            // resize image if its too big
            if ((img.Height > 500) | (img.Width > 500))
            {
                var forholdHeight = img.Height / 500;
                var forholdWidth = img.Width / 500;
                var forhold = Math.Max(forholdHeight, forholdWidth);

                var newHeight = img.Height / forhold;
                var newWidth = img.Width / forhold;


                img.Mutate(i => i.Resize(newWidth, newHeight));
            }

            int widthImg = img.Width;
            int heightImg = img.Height;

            decimal segmentSizeWidtNotRounded = widthImg / NumbersofSegmentsHorizontal;
            decimal segmentSizeHeightNotRounded = heightImg / NumbersofSegmentsVertical;

            // want rounded numbers to decide how big the segments will be (both direction) 
            int segmentSizeWidt = Convert.ToInt32(Math.Floor(segmentSizeWidtNotRounded));
            int segmentSizeHeight = Convert.ToInt32(Math.Floor(segmentSizeHeightNotRounded));

            int restWidth = 0;
            int restHeight = 0;

            // will know how much pixels are left after dividing segments with length of whole numbers.
            if (widthImg % NumbersofSegmentsHorizontal != 0)
            {
                restWidth = widthImg % NumbersofSegmentsHorizontal;
            }
            if (heightImg % NumbersofSegmentsVertical != 0)
            {
                restHeight = heightImg % NumbersofSegmentsVertical;
            }

            // want to make segments from the image, and put the different segments ON a transparent imageBitmap. 
            // the segment should as well be put on the exactly same position as it has on the original image. 
            for (int i = 0; i < NumbersofSegmentsVertical; i++)
            {
                for (int j = 0; j < NumbersofSegmentsHorizontal; j++)
                {
                    //Use Bitmap to work with images defined by pixel data. 

                    // size of the segment/rectangle which should be drawn on the given image. 
                    SixLabors.ImageSharp.Rectangle segment;

                    if ((j == NumbersofSegmentsHorizontal - 1 & i == NumbersofSegmentsVertical - 1) | (j == NumbersofSegmentsHorizontal - 1) | (i == NumbersofSegmentsVertical - 1))
                    {
                        segment = new SixLabors.ImageSharp.Rectangle(j * segmentSizeWidt, i * segmentSizeHeight, segmentSizeWidt + restWidth, segmentSizeHeight + restHeight);
                    }
                    else
                    {
                        segment = new SixLabors.ImageSharp.Rectangle(j * segmentSizeWidt, i * segmentSizeHeight, segmentSizeWidt, segmentSizeHeight);
                    }
                    var SegmentImage = img.Clone(i => i.Crop(segment));
                    // creates a copy from the image defined by the size of the rectangle 


                    // Adds the image to the array
                    using (var EmptyImage = new Image<Rgba32>(img.Width, img.Height, Color.Transparent))
                    {
                        EmptyImage.Mutate(x => x.DrawImage(SegmentImage, new Point(segment.X, segment.Y), 1));
                        imgList.Add(EmptyImage.Clone());
                    }
                }
            }
            // converts the image array to byte 
            List<byte[]> imagesByteArray = new List<byte[]>();
            foreach (var entry in imgList)
            {
                MemoryStream ms = new MemoryStream();
                entry.Save(ms, new PngEncoder());
                var result = ms.ToArray();
                imagesByteArray.Add(result);
            }
            return imagesByteArray;
        }
    }
}

