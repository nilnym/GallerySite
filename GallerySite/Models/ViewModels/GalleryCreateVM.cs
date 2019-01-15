using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GallerySite.Models.ViewModels
{
    public class GalleryCreateVM
    {
        [Required(ErrorMessage = "Please enter a name")]
        public string Name { get; set; }
    }
}