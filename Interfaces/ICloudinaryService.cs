using System;

namespace WebApplication1.Interfaces;

public interface ICloudinaryService
{
Task<string> UploadImageAsync(IFormFile file);

}
