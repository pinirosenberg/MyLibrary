using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyLibrary.Models;

namespace MyLibrary.Data
{
    public class MyLibraryContext : DbContext
    {
        public MyLibraryContext (DbContextOptions<MyLibraryContext> options)
            : base(options)
        {
        }

        public DbSet<MyLibrary.Models.Book> Book { get; set; } = default!;
        public DbSet<MyLibrary.Models.Genre> Genre { get; set; } = default!;
        public DbSet<MyLibrary.Models.Serie> Serie { get; set; } = default!;
        public DbSet<MyLibrary.Models.Shelf> Shelf { get; set; } = default!;
    }
}
