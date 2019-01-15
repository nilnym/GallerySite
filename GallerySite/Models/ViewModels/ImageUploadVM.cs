using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GallerySite.Models.ViewModels
{
    public class ImageUploadVM
    {
        [Required(ErrorMessage = "Please enter a title")]
        public string Title { get; set; }

        [Display(Name = "Gallery")]
        [Required(ErrorMessage = "You must pick a gallery")]
        public int GalleryId { get; set; }

        public SelectListItem[] Galleries { get; set; }

        [Display(Name = "Upload from URL")]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        [Display(Name = "Upload from computer*")]
        [DataType(DataType.Upload)]
        public IFormFile File { get; set; }

        [Display(Name = "Comma-separated tags (optional)")]
        public string Tags { get; set; }

        public string FileName { get; set; }

        public bool SomethingWentWrong { get; set; }
    }
}
