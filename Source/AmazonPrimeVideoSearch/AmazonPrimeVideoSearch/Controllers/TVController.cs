using AmazonPrimeVideoSearch.Model;
using Microsoft.AspNetCore.Mvc;

namespace AmazonPrimeVideoSearch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TVController : Controller
    {
        readonly ITVRepository _tvRepository;

        public TVController(ITVRepository tvRepository)
        {
            _tvRepository = tvRepository;
        }

        // GET: api/<controller>
        [HttpGet]
        public TVMetadata Get()
        {
            return _tvRepository.GetTVMetadata();
        }

        // POST: api/MovieSearch
        [HttpPost]
        public TVSearchResults Post([FromBody] ReceivedTVSearch received)
        {
            return _tvRepository.TVSearch(received.tvSearchCriteria);
        }
    }
}