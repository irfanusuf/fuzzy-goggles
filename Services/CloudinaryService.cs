using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using WebApplication1.Interfaces;

public class CloudinaryService (Cloudinary cloudinary): ICloudinaryService
{

    public async Task<string> UploadImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is invalid.");

        using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            UseFilename = true,
            UniqueFilename = false,
            Overwrite = true
            // Transformation = new Transformation().Width(150).Height(150).Crop("fill")
        };

        var uploadResult = await cloudinary.UploadAsync(uploadParams);

        return uploadResult.SecureUrl.ToString();
    }
}
