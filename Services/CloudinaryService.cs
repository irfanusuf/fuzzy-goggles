using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dotenv.net;
using WebApplication1.Interfaces;

public class CloudinaryService  : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService()
    {
        // Load the .env file
        try
        {
            DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to load .env file.", ex);
        }


        // connect to cloudinary 
      
        var cloudinaryUrl = Environment.GetEnvironmentVariable("CLOUDINARY_URL");
        
        if (string.IsNullOrEmpty(cloudinaryUrl))
        {
            throw new InvalidOperationException("CLOUDINARY_URL environment variable is not set.");
        }

        // Initialize Cloudinary instance
        _cloudinary = new Cloudinary(cloudinaryUrl) { Api = { Secure = true } };
    }

    public async Task<string> UploadImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("File is invalid.");
        }

        using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            UseFilename = true,
            UniqueFilename = false,
            Overwrite = true,
            Folder = "Fuzzy Goggles"
            // Transformation = new Transformation().Width(150).Height(150).Crop("fill")
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        if (uploadResult.Error != null)
        {
            throw new InvalidOperationException($"Cloudinary upload failed: {uploadResult.Error.Message}");
        }

        return uploadResult.SecureUrl.ToString();
    }
}
