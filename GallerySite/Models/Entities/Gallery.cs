using System;
using System.Collections.Generic;

namespace GallerySite.Models.Entities
{
    public partial class Gallery
    {
        public Gallery()
        {
            Image = new HashSet<Image>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Image> Image { get; set; }
    }
}
