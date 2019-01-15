using System;
using System.Collections.Generic;

namespace GallerySite.Models.Entities
{
    public partial class Tag
    {
        public Tag()
        {
            TagToImage = new HashSet<TagToImage>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<TagToImage> TagToImage { get; set; }
    }
}
