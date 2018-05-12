using System.Collections.Generic;

namespace AmazonPrimeVideoSearch.Model
{
    public class ReceivedTVSearch
    {
        public TVSearchCriteria tvSearchCriteria { get; set; }
    }
    public class TVSearchCriteria
    {
        public TVSearchCriteria()
        {
            genres = new List<string>();
            titleSearch = string.Empty;
            sortOrder = "Title";
        }
        public int page { get; set; }
        public int pageSize { get; set; }
        public int offset { get; set; }
        public bool wildcardSearch { get; set; } // If false, then search from start only
        public string titleSearch { get; set; }
        public string sortOrder { get; set; }
        public int yearMinimum { get; set; }
        public int yearMaximum { get; set; }
        public float imdbMinimum { get; set; }
        public float imdbMaximum { get; set; }
        public List<string> genres { get; set; }
        public bool returnCount { get; set; }
    }
}