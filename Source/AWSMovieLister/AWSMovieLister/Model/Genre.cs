/*
	Ken Kohler
	Prime Video Search v 1.0
	v 1.0 Apr 2018
	Licensed: MIT license
*/
using System;
using System.Collections.Generic;

namespace AWSMovieLister.Model
{
    public partial class Genre
    {
        public Genre()
        {
            MovieGenre = new HashSet<MovieGenre>();
            TvseriesGenre = new HashSet<TvseriesGenre>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Created { get; set; }

        public ICollection<MovieGenre> MovieGenre { get; set; }
        public ICollection<TvseriesGenre> TvseriesGenre { get; set; }
    }
}
