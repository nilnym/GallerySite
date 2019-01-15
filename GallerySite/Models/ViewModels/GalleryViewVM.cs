using GallerySite.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GallerySite.Models.ViewModels
{
    public class GalleryViewVM
    {
        public string Name { get; set; }
        public Image[] Images { get; set; }
    }
}
