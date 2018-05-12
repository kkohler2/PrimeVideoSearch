using System.Collections.Generic;

namespace AmazonPrimeVideoSearch.Model
{
    public class MovieSearchResults
    {
        public MovieSearchResults()
        {
            Count = -1;
            Movies = new List<Movie>();
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public List<Movie> Movies { get; set; }
    }
}