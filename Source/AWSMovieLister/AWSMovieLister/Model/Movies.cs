using System;
using System.Collections.Generic;

namespace AWSMovieLister.Model
{
    public partial class Movies
    {
        public int Id { get; set; }
        public string DataSin { get; set; }
        public string Title { get; set; }
        public string SearchTitle { get; set; }
        public string Url { get; set; }
        public string Image { get; set; }
        public string Rating { get; set; }
        public bool ClosedCaptioned { get; set; }
        public int Released { get; set; }
        public string Runtime { get; set; }
        public string Stars { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string Metadata { get; set; }
        public bool MetadataRetrieved { get; set; }
    }
}
