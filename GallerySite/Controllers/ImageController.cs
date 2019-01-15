using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GallerySite.Models;
using GallerySite.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GallerySite.Controllers
{
    public class ImageController : Controller
    {
        GalleryService service;
        FileService fileService;
        string Gallery = "Gallery";

        public ImageController(GalleryService service, FileService fileService)
        {
            this.service = service;
            this.fileService = fileService;
        }

        [ActionName("View")]
        public async Task<IActionResult> ViewImage(int id)
        {
            var image = await service.GetSingleImage(id);
            var tags = await service.GetTagsForImage(id);

            ImageViewVM viewModel = new ImageViewVM
            {
                Id = image.Id,
                Created = image.Created,
                Gallery = image.Gallery,
                ImageUrl = image.Url,
                Title = image.Title,
                Tags = tags,
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var image = await service.GetSingleImage(id);
            var tags = await service.GetTagsForImage(id);

            ImageEditVM viewModel = new ImageEditVM
            {
                Title = image.Title,
                ImageId = image.Id,
                GalleryId = image.GalleryId,
                ImageUrl = image.Url,
                Tags = string.Join(", ", tags.Select(t => t.Name)),
                Galleries = service.GetAllGalleries(),
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ImageEditVM viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            await service.UpdateImage(viewModel);
            return RedirectToAction(nameof(GalleryController.Index), Gallery);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await service.DeleteImage(id);

            return RedirectToAction(nameof(GalleryController.Index), Gallery);
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View(new ImageUploadVM { Galleries = service.GetAllGalleries() });
        }

        [HttpPost]
        public async Task<IActionResult> Upload(ImageUploadVM viewModel)
        {
            bool success = false;
            string fileName = string.Empty;

            if (viewModel.File != null)
            {
                fileName = fileService.GetFileName(viewModel.File.FileName);

                if (!string.IsNullOrEmpty(fileName))
                    success = await fileService.SaveFileFromFormAsync(viewModel.File, fileName);
            }
            else if (viewModel.ImageUrl != null)
            {
                fileName = fileService.GetFileName(viewModel.ImageUrl);

                if (!string.IsNullOrEmpty(fileName))
                    success = await fileService.SaveFileFromUrlAsync(viewModel.ImageUrl, fileName);
            }
            else
            {
                ModelState.AddModelError(nameof(viewModel.ImageUrl),
                    "At least one file need to be specified.");
                ModelState.AddModelError(nameof(viewModel.File),
                    "At least one file need to be specified.");
            }

            if (!success)
            {
                ModelState.AddModelError(nameof(viewModel.SomethingWentWrong),
                    "Something went wrong");
                viewModel.SomethingWentWrong = true;
            }

            if (!ModelState.IsValid)
            {
                viewModel.Galleries = service.GetAllGalleries();
                return View(viewModel);
            }

            viewModel.FileName = fileName;
            await service.CreateImage(viewModel);

            return RedirectToAction(nameof(GalleryController.Index), Gallery);
        }
    }
}