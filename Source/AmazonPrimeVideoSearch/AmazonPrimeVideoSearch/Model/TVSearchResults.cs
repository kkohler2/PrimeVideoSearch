using System.Collections.Generic;

namespace AmazonPrimeVideoSearch.Model
{
    public class TVSearchResults
    {
        public TVSearchResults()
        {
            Count = -1;
            Series = new List<Tvseries>();
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public List<Tvseries> Series { get; set; }
    }
}