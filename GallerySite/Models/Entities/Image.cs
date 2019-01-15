using System;
using System.Collections.Generic;

namespace GallerySite.Models.Entities
{
    public partial class Image
    {
        public Image()
        {
            TagToImage = new HashSet<TagToImage>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public int GalleryId { get; set; }
        public DateTime Created { get; set; }
        public string Url { get; set; }

        public Gallery Gallery { get; set; }
        public ICollection<TagToImage> TagToImage { get; set; }
    }
}
