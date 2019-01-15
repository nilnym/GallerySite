using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GallerySite.Models.Entities
{
    public partial class GallerySiteContext : DbContext
    {
        public GallerySiteContext()
        {
        }

        public GallerySiteContext(DbContextOptions<GallerySiteContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Gallery> Gallery { get; set; }
        public virtual DbSet<Image> Image { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }
        public virtual DbSet<TagToImage> TagToImage { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gallery>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.GalleryId).HasColumnName("GalleryID");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("URL")
                    .IsUnicode(false);

                entity.HasOne(d => d.Gallery)
                    .WithMany(p => p.Image)
                    .HasForeignKey(d => d.GalleryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Image__GalleryID__4D94879B");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ__Tag__737584F640191B4A")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(16)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TagToImage>(entity =>
            {
                entity.HasIndex(e => new { e.ImageId, e.TagId })
                    .HasName("UQ__TagToIma__A34138970F231413")
                    .IsUnique();

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.TagToImage)
                    .HasForeignKey(d => d.ImageId)
                    .HasConstraintName("FK__TagToImag__Image__5535A963");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.TagToImage)
                    .HasForeignKey(d => d.TagId)
                    .HasConstraintName("FK__TagToImag__TagId__5629CD9C");
            });
        }
    }
}
