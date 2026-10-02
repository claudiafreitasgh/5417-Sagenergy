using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace _5417_Sagenergy.Helpers
{
    public interface IImageHelper
    {
        Task<string> UploadImageAsync(IFormFile imageFile, string folder);
    }
}