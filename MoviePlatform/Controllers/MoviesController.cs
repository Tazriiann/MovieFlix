using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MoviePlatform.Controllers
{
    public class MoviesController : Controller
    {
        private readonly AppDbContext _context;

        public MoviesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Movies.ToListAsync());
        }

        // GET: Movies/Details/
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movie movie, IFormFile VideoFile, IFormFile PosterFile)
        {
            if (ModelState.IsValid)
            {
                // Upload video
                if (VideoFile != null && VideoFile.Length > 0)
                {
                    var videoFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/videos");
                    Directory.CreateDirectory(videoFolder);

                    var videoName = Guid.NewGuid() + Path.GetExtension(VideoFile.FileName);
                    var videoPath = Path.Combine(videoFolder, videoName);

                    using var stream = new FileStream(videoPath, FileMode.Create);
                    await VideoFile.CopyToAsync(stream);

                    movie.VideoPath = "/videos/" + videoName;
                }

                // Upload poster
                if (PosterFile != null && PosterFile.Length > 0)
                {
                    var posterFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/posters");
                    Directory.CreateDirectory(posterFolder);

                    var posterName = Guid.NewGuid() + Path.GetExtension(PosterFile.FileName);
                    var posterPath = Path.Combine(posterFolder, posterName);

                    using var stream = new FileStream(posterPath, FileMode.Create);
                    await PosterFile.CopyToAsync(stream);

                    movie.PosterPath = "/posters/" + posterName;
                }

                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(movie);
        }



        // GET: Movies/Edit/
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Movie movie, IFormFile VideoFile, IFormFile PosterFile)
        {
            if (id != movie.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingMovie = await _context.Movies.AsNoTracking().FirstOrDefaultAsync(m => m.Id == movie.Id);
                    if (existingMovie == null) return NotFound();

                    // Handle video
                    if (VideoFile != null && VideoFile.Length > 0)
                    {
                        var videoFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/videos");
                        Directory.CreateDirectory(videoFolder);

                        var videoName = Guid.NewGuid() + Path.GetExtension(VideoFile.FileName);
                        var videoPath = Path.Combine(videoFolder, videoName);

                        using var stream = new FileStream(videoPath, FileMode.Create);
                        await VideoFile.CopyToAsync(stream);

                        movie.VideoPath = "/videos/" + videoName;
                    }
                    else
                    {
                        movie.VideoPath = existingMovie.VideoPath;
                    }

                    // Handle poster
                    if (PosterFile != null && PosterFile.Length > 0)
                    {
                        var posterFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/posters");
                        Directory.CreateDirectory(posterFolder);

                        var posterName = Guid.NewGuid() + Path.GetExtension(PosterFile.FileName);
                        var posterPath = Path.Combine(posterFolder, posterName);

                        using var stream = new FileStream(posterPath, FileMode.Create);
                        await PosterFile.CopyToAsync(stream);

                        movie.PosterPath = "/posters/" + posterName;
                    }
                    else
                    {
                        movie.PosterPath = existingMovie.PosterPath;
                    }

                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Movies.Any(m => m.Id == movie.Id)) return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(movie);
        }




        public async Task<IActionResult> Download(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null || string.IsNullOrEmpty(movie.VideoPath))
                return NotFound();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", movie.VideoPath.TrimStart('/'));
            var contentType = "application/octet-stream";
            var fileName = Path.GetFileName(filePath);

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(fileBytes, contentType, fileName);
        }





        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
