using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;

namespace TonaWebApp.Controllers;

public class CloudinaryController : Controller
{
  private readonly Cloudinary _cloudinary;

  public CloudinaryController(IConfiguration configuration)
  {
    Account account = new(
        "djke8yehz",
        "721588136165575",
        "qmHTMF5R8t3yw8MG0nERQ0hVxvY"
    );

    _cloudinary = new Cloudinary(account);
  }

  public async Task<string?> UploadImageAsync(IFormFile file)
  {
    if (file == null || file.Length == 0)
    {
      return null;
    }

    using (var stream = file.OpenReadStream())
    {
      var uploadParams = new ImageUploadParams
      {
        File = new FileDescription(file.FileName, stream),
      };

      var uploadResult = await _cloudinary.UploadAsync(uploadParams);

      return uploadResult.SecureUrl.AbsoluteUri;
    }
  }

  public async Task<string?> DeleteImageAsync(string publicUrl)
  {
    var uri = new Uri(publicUrl);
    var publicId = Path.GetFileNameWithoutExtension(uri.Segments.Last());

    var deleteParams = new DeletionParams(publicId);

    var result = await _cloudinary.DestroyAsync(deleteParams);

    return result.Result == "ok" ? publicId : null;
  }
}