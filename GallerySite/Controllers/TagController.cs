using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GallerySite.Models;
using Microsoft.AspNetCore.Mvc;

namespace GallerySite.Controllers
{
    public class TagController : Controller
    {
        GalleryService service;

        public TagController(GalleryService service)
        {
            this.service = service;
        }

        [Route("tag/{name?}")]
        public async Task<IActionResult> Index(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                var viewModel = await service.GetImagesForTag(name);
                viewModel.Name = name;
                return View(viewModel);
            }
            else
                return RedirectToAction(nameof(GalleryController.Index), "Gallery");
        }
    }
}