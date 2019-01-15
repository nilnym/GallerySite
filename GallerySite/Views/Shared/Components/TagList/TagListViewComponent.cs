using GallerySite.Models;
using GallerySite.Models.Entities;
using GallerySite.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GallerySite.Views.Shared.Components
{
    public class TagListViewComponent : ViewComponent
    {
        GalleryService service;

        public TagListViewComponent(GalleryService service)
        {
            this.service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await service.GetTagListVM());
        }
    }
}
