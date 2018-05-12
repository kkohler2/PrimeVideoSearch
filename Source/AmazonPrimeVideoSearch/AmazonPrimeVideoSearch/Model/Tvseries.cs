using System;
using System.Collections.Generic;

namespace AmazonPrimeVideoSearch.Model
{
    public partial class Tvseries
    {
        public Tvseries()
        {
            TvseriesGenre = new HashSet<TvseriesGenre>();
        }

        public int Id { get; set; }
        public string DataSin { get; set; }
        public string Title { get; set; }
        public string SearchTitle { get; set; }
        public string Url { get; set; }
        public string Image { get; set; }
        public bool ClosedCaptioned { get; set; }
        public int Released { get; set; }
        public string Stars { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string Ratings { get; set; }
        public float Imdbrating { get; set; }
        public string Plot { get; set; }
        public string Genres { get; set; }
        public string Director { get; set; }
        public string Starring { get; set; }
        public string SupportingActors { get; set; }
        public bool IsPrime { get; set; }

        public ICollection<TvseriesGenre> TvseriesGenre { get; set; }
    }
}