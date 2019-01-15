using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GallerySite.Models;
using GallerySite.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GallerySite.Controllers
{
    public class GalleryController : Controller
    {
        GalleryService service;

        public GalleryController(GalleryService service)
        {
            this.service = service;
        }

        [Route("/")]
        [Route("gallery/index")]
        public IActionResult Index()
        {
            return View(service.GetAllImages());
        }

        [ActionName("View")]
        public IActionResult ViewGallery(int id = 0)
        {
            return View(service.GetImagesForGallery(id));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GalleryCreateVM viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            await service.CreateGallery(viewModel);
            return RedirectToAction(nameof(Index));
        }
    }
}