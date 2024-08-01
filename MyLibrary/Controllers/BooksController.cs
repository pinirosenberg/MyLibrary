using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using MyLibrary.Data;
using MyLibrary.Models;

namespace MyLibrary.Controllers
{
    public class BooksController : Controller
    {
        private readonly MyLibraryContext _context;

        public BooksController(MyLibraryContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var myLibraryContext = _context.Book.Include(b => b.Serie);
            return View(await myLibraryContext.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Serie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["SerieId"] = new SelectList(_context.Serie, "Id", "Id");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,SerieName,Height,Width,GenreName")] Book book)
        {
            if (ModelState.IsValid)
            {
                var genre = await _context.Genre.
                    Where(b => b.Name == book.GenreName).FirstOrDefaultAsync();
                //first we need to get a serie to push it into the shelf
                book.Serie = _context.Serie.FirstOrDefault(s => s.Name == book.SerieName);
                //if we have not a serie we can to push it to the serie
                
                var genreId = genre.Id;
                if(book.Serie == null)
                {
                    //we search for a shelf
                    //We are looking for a shelf that satisfies the shelf
                    //condition of max(sheleftwidth - book.width > 0)
                   
                    var shelf = _context.Shelf
                        .Where(s => s.LeftWidth - book.Width >= 0 && s.Height >= book.Height && genreId == s.Genre.Id)
                        .OrderByDescending(s => s.Height - book.Height)
                        .FirstOrDefault();
                    if (shelf == null)
                        return NotFound();
                    if (shelf.Height - book.Height > 10)
                    {
                        //לא מיושם עדיין
                        Console.WriteLine("FGHNJ");
                    }
                    var serie = new Serie();
                    serie.Name = book.Name;
                    serie.Shelf = shelf; 
                    serie.WidthOfAll = book.Width;
                    serie.MaxHeight = book.Height;
                    book.Serie = serie;
                    book.SerieId = serie.Id;
                    shelf.LeftWidth = shelf.LeftWidth - book.Width;
                    
                    _context.Update(shelf);
                    _context.Serie.Add(serie);
                    _context.Book.Add(book);
                    //_context.Update(book);
                    //_context.Update(serie);
                    await _context.SaveChangesAsync();

                }
                //_context.Add(book);
                //await _context.SaveChangesAsync();
                //_context.Add(book);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SerieId"] = new SelectList(_context.Serie, "Id", "Id", book.SerieId);
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["SerieId"] = new SelectList(_context.Serie, "Id", "Id", book.SerieId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,SerieId,Height,Width")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SerieId"] = new SelectList(_context.Serie, "Id", "Id", book.SerieId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Serie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book != null)
            {
                _context.Book.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}
