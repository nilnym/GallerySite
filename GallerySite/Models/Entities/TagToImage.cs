using System;
using System.Collections.Generic;

namespace GallerySite.Models.Entities
{
    public partial class TagToImage
    {
        public int Id { get; set; }
        public int? ImageId { get; set; }
        public int? TagId { get; set; }

        public Image Image { get; set; }
        public Tag Tag { get; set; }
    }
}
