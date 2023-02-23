using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Reado.Models
{
    public partial class BooksContext : DbContext
    {
        public BooksContext()
        {
        }

        public BooksContext(DbContextOptions<BooksContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BooksList> BooksLists { get; set; } = null!;
        public virtual DbSet<TblList> TblLists { get; set; } = null!;
        public virtual DbSet<TblUser> TblUsers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Books;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BooksList>(entity =>
            {
                entity.HasKey(e => e.Isbn);

                entity.ToTable("books_list");

                entity.Property(e => e.Isbn)
                    .HasMaxLength(300)
                    .HasColumnName("ISBN");

                entity.Property(e => e.BookAuthor)
                    .HasMaxLength(300)
                    .HasColumnName("Book_Author");

                entity.Property(e => e.BookTitle)
                    .HasMaxLength(300)
                    .HasColumnName("Book_Title");

                entity.Property(e => e.Column10)
                    .HasMaxLength(300)
                    .HasColumnName("column10");

                entity.Property(e => e.Column11)
                    .HasMaxLength(300)
                    .HasColumnName("column11");

                entity.Property(e => e.Column9)
                    .HasMaxLength(300)
                    .HasColumnName("column9");

                entity.Property(e => e.ImageUrlL)
                    .HasMaxLength(300)
                    .HasColumnName("Image_URL_L");

                entity.Property(e => e.ImageUrlM)
                    .HasMaxLength(300)
                    .HasColumnName("Image_URL_M");

                entity.Property(e => e.ImageUrlS)
                    .HasMaxLength(300)
                    .HasColumnName("Image_URL_S");

                entity.Property(e => e.Publisher).HasMaxLength(300);

                entity.Property(e => e.YearOfPublication).HasColumnName("Year_Of_Publication");
            });

            modelBuilder.Entity<TblList>(entity =>
            {
                entity.HasKey(e => e.ListId)
                    .HasName("PK_List_ID");

                entity.ToTable("Tbl_List");

                entity.Property(e => e.ListId).HasColumnName("List_Id");

                entity.Property(e => e.ListIsbn)
                    .HasMaxLength(300)
                    .HasColumnName("List_Isbn");

                entity.Property(e => e.ListRead).HasColumnName("List_Read");

                entity.Property(e => e.ListScore).HasColumnName("List_Score");

                entity.Property(e => e.ListUsrId).HasColumnName("List_Usr_Id");

                entity.HasOne(d => d.ListIsbnNavigation)
                    .WithMany(p => p.TblLists)
                    .HasForeignKey(d => d.ListIsbn)
                    .HasConstraintName("FK_List_Isbn");

                entity.HasOne(d => d.ListUsr)
                    .WithMany(p => p.TblLists)
                    .HasForeignKey(d => d.ListUsrId)
                    .HasConstraintName("FK_List_Usr_Id");
            });

            modelBuilder.Entity<TblUser>(entity =>
            {
                entity.HasKey(e => e.UsrId);

                entity.ToTable("Tbl_User");

                entity.Property(e => e.UsrId).HasColumnName("Usr_ID");

                entity.Property(e => e.UsrEmail)
                    .HasMaxLength(150)
                    .HasColumnName("Usr_Email");

                entity.Property(e => e.UsrName)
                    .HasMaxLength(30)
                    .HasColumnName("Usr_Name");

                entity.Property(e => e.UsrPassword)
                    .HasMaxLength(150)
                    .HasColumnName("Usr_Password");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
