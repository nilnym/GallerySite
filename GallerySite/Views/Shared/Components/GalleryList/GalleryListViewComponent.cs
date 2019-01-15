using GallerySite.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GallerySite.Views.Shared.Components
{
    public class GalleryListViewComponent : ViewComponent
    {
        GalleryService service;

        public GalleryListViewComponent(GalleryService service)
        {
            this.service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await service.GetGalleryListVM());
        }
    }
}
