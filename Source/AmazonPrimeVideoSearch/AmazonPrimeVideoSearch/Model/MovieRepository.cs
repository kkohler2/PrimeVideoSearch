using System.Linq;

namespace AmazonPrimeVideoSearch.Model
{
    public class MovieRepository : IMovieRepository
    {
        readonly AWSPrimeStreamingContext _appDbContext;

        public MovieRepository(AWSPrimeStreamingContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public MovieMetadata GetMovieMetadata()
        {
            MovieMetadata metadata = new MovieMetadata();
            metadata.Genres = _appDbContext.MovieGenreCount.ToList();
            metadata.ReleasedMinimumYear = _appDbContext.Movie.Where(m => m.Released > 0).Min(a => a.Released);
            metadata.ReleasedMaximumYear = _appDbContext.Movie.Max(m => m.Released);
            metadata.RuntimeMinimum = _appDbContext.Movie.Where(x => x.Runtime > 0).Min(m => m.Runtime);
            metadata.RuntimeMaximum = _appDbContext.Movie.Max(m => m.Runtime);
            metadata.Count = _appDbContext.Movie.Count();
            return metadata;
        }

        public MovieSearchResults MovieSearch(MovieSearchCriteria criteria)
        {
            MovieSearchResults results = new MovieSearchResults();
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


            if (criteria.runtimeMaximum < criteria.runtimeMinimum)
            {
                criteria.runtimeMinimum = -1;
                criteria.runtimeMaximum = -1;
            }
            if (criteria.runtimeMinimum < 1)
            {
                criteria.runtimeMinimum = -1;
            }
            if (criteria.runtimeMaximum < 1)
            {
                criteria.runtimeMaximum = -1;
            }

            if (criteria.mPAArating != "G" && criteria.mPAArating != "PG" && criteria.mPAArating != "PG-13" && criteria.mPAArating != "R" && criteria.mPAArating != "NC-17" && criteria.mPAArating != "X" && criteria.mPAArating != "XXX")
            {
                criteria.mPAArating = string.Empty;
            }

            var query = from m in _appDbContext.Movie select m;

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
            if (criteria.runtimeMinimum != -1)
            {
                query = query.Where(m => m.Runtime >= criteria.runtimeMinimum);
            }
            if (criteria.runtimeMaximum != -1)
            {
                query = query.Where(m => m.Runtime <= criteria.runtimeMaximum);
            }
            if (criteria.mPAArating.Length > 0)
            {
                query = query.Where(m => m.Rating == criteria.mPAArating);
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
                case "Runtime":
                    query = query.OrderByDescending(m => m.Runtime);
                    break;
            }
            if (criteria.returnCount)
            {
                results.Count = query.Count();
            }
            query = query.Skip(criteria.offset).Take(criteria.pageSize);
            results.Movies = query.ToList();
            foreach (Movie movie in results.Movies)
            {
                if (movie.Genres != null && movie.Genres.Length > 0)
                {
                    movie.Genres = movie.Genres.Remove(movie.Genres.Length - 2);
                }
            }
            results.Page = criteria.page;
            results.PageSize = criteria.pageSize;
            return results;
        }
    }
}