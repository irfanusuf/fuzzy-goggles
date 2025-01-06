using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using WebApplication1.Interfaces;

public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(Cloudinary cloudinary)
    {
        _cloudinary = cloudinary;
    }

    public async Task<string> UploadImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is invalid.");

        using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Transformation = new Transformation().Width(150).Height(150).Crop("fill")
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        return uploadResult.Url.ToString();
    }
}
