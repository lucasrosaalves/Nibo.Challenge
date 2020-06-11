using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Nibo.API.ViewModels
{
    public class FilesViewModel
    {
        [Required(ErrorMessage = "Field {0} is required")]
        public IFormFile[] Files { get; set; }
    }
}
