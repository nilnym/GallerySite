using GallerySite.Models.Entities;
using GallerySite.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GallerySite.Models
{
    public class GalleryService
    {
        GallerySiteContext context;

        public GalleryService(GallerySiteContext context)
        {
            this.context = context;
        }

        public async Task CreateGallery(GalleryCreateVM viewModel)
        {
            context.Gallery.Add(new Gallery { Name = viewModel.Name });
            await context.SaveChangesAsync();
        }

        public async Task CreateImage(ImageUploadVM viewModel)
        {
            var image = new Image
            {
                Created = DateTime.Now,
                GalleryId = viewModel.GalleryId,
                Title = viewModel.Title,
                Url = viewModel.FileName,
            };


            if (!string.IsNullOrWhiteSpace(viewModel.Tags))
            {
                var tags = context.Tag.ToArray();

                foreach (var tag in viewModel.Tags.Split(","))
                {
                    var tagName = tag.Trim();
                    var tagToBeInserted = tags.SingleOrDefault(t => t.Name == tagName);

                    if (tagToBeInserted == null)
                        tagToBeInserted = new Tag { Name = tagName };

                    image.TagToImage.Add(new TagToImage
                    {
                        Image = image,
                        Tag = tagToBeInserted,
                    });
                }
            }

            context.Image.Add(image);
            await context.SaveChangesAsync();
        }

        public async Task<Image> GetSingleImage(int id)
        {
            return await context.Image.Include(o => o.Gallery).Where(o => o.Id == id).SingleAsync();
        }

        public async Task UpdateImage(ImageEditVM viewModel)
        {
            Image image = context.Image.Include(o => o.TagToImage).Single(o => o.Id == viewModel.ImageId);

            image.Title = viewModel.Title;
            image.GalleryId = viewModel.GalleryId;

            if (!string.IsNullOrWhiteSpace(viewModel.Tags))
            {
                var tags = await context.Tag.ToListAsync();

                foreach (var tag in viewModel.Tags.Split(","))
                {
                    var tagName = tag.Trim();
                    var tagToBeInserted = tags.SingleOrDefault(t => t.Name == tagName);
                    var tagToImage = new TagToImage();

                    if (tagToBeInserted == null)
                    {
                        tagToImage.Tag = new Tag { Name = tagName };
                        tagToImage.Image = image;

                        image.TagToImage.Add(tagToImage);
                    }
                    else
                    {
                        if (image.TagToImage.Count(o => o.ImageId == image.Id && o.TagId == tagToBeInserted.Id) == 0)
                        {
                            tagToImage.Tag = tagToBeInserted;
                            tagToImage.Image = image;

                            image.TagToImage.Add(tagToImage);
                        }
                        tags.Remove(tagToBeInserted);
                    }
                }

                var tagsToRemove = tags.Where(o => o.TagToImage.Any(t => t.ImageId == image.Id)).SelectMany(a => a.TagToImage);
                if (tagsToRemove != null)
                    context.TagToImage.RemoveRange(tagsToRemove);
            }

            await context.SaveChangesAsync();

            await RemoveUnusedTags();
        }

        public async Task DeleteImage(int id)
        {
            var tags = await context.TagToImage.Where(t => t.ImageId == id).ToArrayAsync();
            context.TagToImage.RemoveRange(tags);

            var image = await GetSingleImage(id);
            context.Image.Remove(image);

            await context.SaveChangesAsync();

            await RemoveUnusedTags();
        }

        public GalleryIndexVM GetAllImages()
        {
            var viewModel = new GalleryIndexVM
            {
                Images = context.Image.Include(o => o.Gallery).OrderBy(x => Guid.NewGuid()).ToArray(),
            };

            return viewModel;
        }

        public GalleryViewVM GetImagesForGallery(int id)
        {
            var viewModel = new GalleryViewVM
            {
                Name = context.Gallery.Where(g => g.Id == id).Select(o => o.Name).Single(),
                Images = context.Image.Where(i => i.GalleryId == id).ToArray(),
            };

            return viewModel;
        }

        public SelectListItem[] GetAllGalleries()
        {
            return context.Gallery
                .Select(g => new SelectListItem
                {
                    Value = g.Id.ToString(),
                    Text = g.Name,
                })
                .ToArray();
        }

        public async Task<GalleryListVM> GetGalleryListVM()
        {
            var viewModel = new GalleryListVM
            {
                Galleries = await context.Gallery.ToArrayAsync(),
            };

            return viewModel;
        }

        public async Task<Tag[]> GetTagsForImage(int id)
        {
            return await context.Tag.Where(t => t.TagToImage.Any(w => w.ImageId == id)).ToArrayAsync();
        }

        public async Task<TagGalleryVM> GetImagesForTag(string name)
        {
            return new TagGalleryVM
            {
                Images = await context.Image
                    .Include(i => i.Gallery)
                    .Where(o => o.TagToImage
                        .Any(e => e.Tag.Name == name)
                    ).ToArrayAsync(),
            };
        }

        public async Task<TagListVM> GetTagListVM()
        {
            var viewModel = new TagListVM
            {
                Tags = await context.Tag.ToArrayAsync(),
            };

            return viewModel;
        }

        async Task RemoveUnusedTags()
        {
            var tags = await context.Tag
                .Where(s => s.TagToImage.Count(a => a.TagId == s.Id) == 0)
                .ToArrayAsync();

            context.Tag.RemoveRange(tags);

            await context.SaveChangesAsync();
        }
    }
}
