using System.Linq;

namespace AmazonPrimeVideoSearch.Model
{
    public class TVRepository : ITVRepository
    {
        readonly AWSPrimeStreamingContext _appDbContext;

        public TVRepository(AWSPrimeStreamingContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public TVMetadata GetTVMetadata()
        {
            TVMetadata metadata = new TVMetadata();
            metadata.Genres = _appDbContext.TVSeriesGenreCount.ToList();
            metadata.ReleasedMinimumYear = _appDbContext.Tvseries.Where(m => m.Released > 0).Min(a => a.Released);
            metadata.ReleasedMaximumYear = _appDbContext.Tvseries.Max(a => a.Released);
            metadata.Count = _appDbContext.Tvseries.Count();
            return metadata;
        }

        public TVSearchResults TVSearch(TVSearchCriteria criteria)
        {
            TVSearchResults results = new TVSearchResults();
            if (criteria.page < 0)
            {
                criteria.page = 0;
            }
            if (criteria.offset < 0)
            {
                criteria.offset = 0;
            }
            if (criteria.pageSize < 5 || criteria.pageSize > 250)
            {
                criteria.pageSize = 25;
            }
            if (criteria.yearMaximum < criteria.yearMinimum)
            {
                criteria.yearMinimum = -1;
                criteria.yearMaximum = -1;
            }
            if (criteria.yearMinimum < 1)
            {
                criteria.yearMinimum = -1;
            }
            if (criteria.yearMaximum < 1)
            {
                criteria.yearMaximum = -1;
            }

            if (criteria.imdbMaximum < criteria.imdbMinimum)
            {
                criteria.imdbMinimum = -1;
                criteria.imdbMaximum = -1;
            }
            if (criteria.imdbMinimum < 1)
            {
                criteria.imdbMinimum = -1;
            }
            if (criteria.imdbMaximum < 1)
            {
                criteria.imdbMaximum = -1;
            }

            var query = from tv in _appDbContext.Tvseries select tv;

            if (criteria.titleSearch.Length > 0)
            {
                if (criteria.wildcardSearch)
                {
                    query = query.Where(m => m.Title.Contains(criteria.titleSearch));
                }
                else
                {
                    query = query.Where(m => m.Title.StartsWith(criteria.titleSearch));
                }
            }
            if (criteria.yearMinimum != -1)
            {
                query = query.Where(m => m.Released >= criteria.yearMinimum);
            }
            if (criteria.yearMaximum != -1)
            {
                query = query.Where(m => m.Released <= criteria.yearMaximum);
            }
            if (criteria.imdbMinimum != -1)
            {
                query = query.Where(m => m.Imdbrating >= criteria.imdbMinimum);
            }
            if (criteria.imdbMaximum != -1)
            {
                query = query.Where(m => m.Imdbrating <= criteria.imdbMaximum);
            }
            foreach (string genre in criteria.genres)
            {
                string search = genre + ",";
                query = query.Where(m => m.Genres.Contains(search));
            }
            switch (criteria.sortOrder)
            {
                case "Title":
                    query = query.OrderBy(m => m.Title);
                    break;
                case "IMDB":
                    query = query.OrderByDescending(m => m.Imdbrating);
                    break;
            }
            if (criteria.returnCount)
            {
                results.Count = query.Count();
            }
            query = query.Skip(criteria.offset).Take(criteria.pageSize);
            results.Series = query.ToList();
            foreach (Tvseries tvSeries in results.Series)
            {
                if (tvSeries.Genres != null && tvSeries.Genres.Length > 0)
                {
                    tvSeries.Genres = tvSeries.Genres.Remove(tvSeries.Genres.Length - 2);
                }
            }
            results.Page = criteria.page;
            results.PageSize = criteria.pageSize;
            return results;
        }
    }
}