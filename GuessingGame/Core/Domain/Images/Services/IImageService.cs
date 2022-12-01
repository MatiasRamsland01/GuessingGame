namespace GuessingGame.Core.Domain.Images.Services
{
    public interface IImageService
    {
        public List<byte[]> SliceImageAutomatic(IFormFile image);
        public List<byte[]> ManualSliceImage(string drawnImage, IFormFile uploadedImage, string colors);

    }
}

