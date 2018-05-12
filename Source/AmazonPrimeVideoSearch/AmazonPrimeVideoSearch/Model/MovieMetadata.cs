using System.Collections.Generic;

namespace AmazonPrimeVideoSearch.Model
{
    public class MovieMetadata
    {
        public List<MovieGenreCount> Genres { get; set; }
        public int ReleasedMinimumYear { get; set; }
        public int ReleasedMaximumYear { get; set; }
        public int Count { get; set; }
        public int RuntimeMinimum { get; set; }
        public int RuntimeMaximum { get; set; }
    }
}