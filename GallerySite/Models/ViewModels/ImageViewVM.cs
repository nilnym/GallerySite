using GallerySite.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GallerySite.Models.ViewModels
{
    public class ImageViewVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public DateTime Created { get; set; }
        public Gallery Gallery { get; set; }
        public Tag[] Tags { get; set; }
    }
}
