namespace AmazonPrimeVideoSearch.Model
{
    public partial class TvseriesGenre
    {
        public int TvseriesId { get; set; }
        public int GenreId { get; set; }

        public Genre Genre { get; set; }
        public Tvseries Tvseries { get; set; }
    }
}