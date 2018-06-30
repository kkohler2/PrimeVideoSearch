using AmazonPrimeVideoSearch.Model;
using Microsoft.AspNetCore.Mvc;

namespace AmazonPrimeVideoSearch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : Controller
    {
        readonly IMovieRepository _movieRepository;

        public MovieController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        // GET: api/<controller>
        [HttpGet]
        public MovieMetadata Get()
        {
            return _movieRepository.GetMovieMetadata();
        }

        // POST: api/<controller>
        [HttpPost]
        public MovieSearchResults Post([FromBody] ReceivedMovieSearch received)
        {
            return _movieRepository.MovieSearch(received.movieSearchCriteria);
        }
    }
}