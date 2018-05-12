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
    public partial class TvseriesGenre
    {
        public int TvseriesId { get; set; }
        public int GenreId { get; set; }

        public Genre Genre { get; set; }
        public Tvseries Tvseries { get; set; }
    }
}
