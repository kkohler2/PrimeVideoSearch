/*
	Ken Kohler
	Prime Video Search v 1.0
	v 1.0 Apr 2018
	Licensed: MIT license
*/
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AWSMovieLister.Model
{
    public partial class AWSPrimeStreamingContext : DbContext
    {
        public virtual DbSet<Genre> Genre { get; set; }
        public virtual DbSet<Movie> Movie { get; set; }
        public virtual DbSet<MovieGenre> MovieGenre { get; set; }
        public virtual DbSet<Tvseries> Tvseries { get; set; }
        public virtual DbSet<TvseriesGenre> TvseriesGenre { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=.\;Database=AWSPrimeStreaming;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>(entity =>
            {
                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DataSin)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Director).HasMaxLength(100);

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.Imdbrating)
                    .HasColumnName("IMDBRating")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Rating)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Ratings)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Released).HasDefaultValueSql("((0))");

                entity.Property(e => e.Runtime).HasDefaultValueSql("((0))");

                entity.Property(e => e.RuntimeDisplay)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SearchTitle)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.Stars)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("URL")
                    .HasMaxLength(512)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MovieGenre>(entity =>
            {
                entity.HasKey(e => new { e.MovieId, e.GenreId });

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.MovieGenre)
                    .HasForeignKey(d => d.GenreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MovieGenre_Genre");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.MovieGenre)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MovieGenre_Movie");
            });

            modelBuilder.Entity<Tvseries>(entity =>
            {
                entity.ToTable("TVSeries");

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DataSin)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Director).HasMaxLength(100);

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.Imdbrating)
                    .HasColumnName("IMDBRating")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Ratings)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Released).HasDefaultValueSql("((0))");

                entity.Property(e => e.SearchTitle)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.Stars)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("URL")
                    .HasMaxLength(512)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TvseriesGenre>(entity =>
            {
                entity.HasKey(e => new { e.TvseriesId, e.GenreId });

                entity.ToTable("TVSeriesGenre");

                entity.Property(e => e.TvseriesId).HasColumnName("TVSeriesId");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.TvseriesGenre)
                    .HasForeignKey(d => d.GenreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TVSeriesGenre_Genre");

                entity.HasOne(d => d.Tvseries)
                    .WithMany(p => p.TvseriesGenre)
                    .HasForeignKey(d => d.TvseriesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TVSeriesGenre_TVSeries");
            });
        }
    }
}
