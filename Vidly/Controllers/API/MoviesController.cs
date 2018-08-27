using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Vidly.Dtos;
using Vidly.Models;
using System.Data.Entity;

namespace Vidly.Controllers.API
{
	public class MoviesController : ApiController
	{
		private ApplicationDbContext _context;

		public MoviesController()
		{
			_context = new ApplicationDbContext();
		}

		[HttpGet]
		public IHttpActionResult GetAllMovies(string query = null)
		{
			var moviesQuery = _context.Movies.Include(m => m.GenreTypes);

			if (!String.IsNullOrWhiteSpace(query))
				moviesQuery = moviesQuery.Where(m => m.Name.Contains(query));

			var moviesDtos = moviesQuery.Where(m => m.NumberAvaliable > 0).ToList().Select(Mapper.Map<Movies, MoviesDto>);

			return Ok(moviesDtos);
		}

		[HttpGet]
		public IHttpActionResult GetMoviesById(int Id)
		{
			var movie = _context.Movies.SingleOrDefault(m => m.Id == Id);

			if (movie == null)
				return NotFound();

			return Ok(Mapper.Map<Movies, MoviesDto>(movie));
		}


		[HttpPost]
		//[Authorize(Roles = RoleName.CanManageMovie)]
		public IHttpActionResult CreateMovie(MoviesDto movieDto)
		{
			if (!ModelState.IsValid)
				throw new HttpResponseException(HttpStatusCode.BadRequest);

			var movie = Mapper.Map<MoviesDto, Movies>(movieDto);
			_context.Movies.Add(movie);
			_context.SaveChanges();

			movieDto.Id = movie.Id;

			return Created(new Uri(Request.RequestUri + "/" + movie.Id), movieDto);
		}


		[HttpPut]
		[Authorize(Roles = RoleName.CanManageMovie)]
		public void EditMovie(int Id, MoviesDto movieDto)
		{
			if (!ModelState.IsValid)
				throw new HttpResponseException(HttpStatusCode.BadRequest);

			var movieInDb = _context.Movies.SingleOrDefault(c => c.Id == Id);

			if (movieInDb == null)
				throw new HttpResponseException(HttpStatusCode.NotFound);

			Mapper.Map(movieDto, movieInDb);

			_context.SaveChanges();
		}


		[HttpDelete]
		[Authorize(Roles = RoleName.CanManageMovie)]
		public void DeleteMovie(int Id)
		{
			var movieInDb = _context.Movies.SingleOrDefault(m => m.Id == Id);

			if (movieInDb == null)
				throw new HttpResponseException(HttpStatusCode.NotFound);

			_context.Movies.Remove(movieInDb);
			_context.SaveChanges();
		}
	}
}
