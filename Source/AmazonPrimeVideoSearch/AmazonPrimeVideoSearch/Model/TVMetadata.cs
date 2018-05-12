using System.Collections.Generic;

namespace AmazonPrimeVideoSearch.Model
{
    public class TVMetadata
    {
        public List<TVSeriesGenreCount> Genres { get; set; }
        public int ReleasedMinimumYear { get; set; }
        public int ReleasedMaximumYear { get; set; }
        public int Count { get; set; }
    }
}