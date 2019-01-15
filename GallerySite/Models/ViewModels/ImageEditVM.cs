using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GallerySite.Models.ViewModels
{
    public class ImageEditVM
    {
        public int ImageId { get; set; }

        [Display(Name = "Select a gallery")]
        [Required(ErrorMessage = "Please select a gallery")]
        public int GalleryId { get; set; }
        
        [Required(ErrorMessage = "Title can't be empty")]
        public string Title { get; set; }

        public string ImageUrl { get; set; }

        [Display(Name = "Comma-separated tags (optional)")]
        public string Tags { get; set; }

        public SelectListItem[] Galleries { get; set; }
    }
}
