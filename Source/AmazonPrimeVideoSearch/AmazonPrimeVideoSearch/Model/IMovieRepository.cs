namespace AmazonPrimeVideoSearch.Model
{
    public interface IMovieRepository
    {
        MovieMetadata GetMovieMetadata();
        MovieSearchResults MovieSearch(MovieSearchCriteria criteria);
    }
}