namespace AmazonPrimeVideoSearch.Model
{
    public interface ITVRepository
    {
        TVMetadata GetTVMetadata();
        TVSearchResults TVSearch(TVSearchCriteria criteria);
    }
}