interface IGenreCount {
    Name: string;
    Count: number;
}

interface IMovie {
    id: number;
    title: string;
    url: string;
    image: string;
    rating: string;
    closeCaptioned: boolean;
    released: number;
    runtimeDisplay: string;
    stars: string;
    ratings: string;
    imdbrating: number;
    plot: string;
    genres: string;
    director: string;
    starring: string;
    supportingActors: string;
}

interface IMovieMetadata {
    genres: IGenreCount[];
    releasedMinimumYear: number;
    releasedMaximumYear: number;
    count: number;
    runtimeMinimum: number;
    runtimeMaximum: number;
}

interface IMovieSearchCriteria {
    page: number;
    pageSize: number;
    offset: number;
    wildcardSearch: boolean;
    titleSearch: string;
    sortOrder: string;
    mPAARating: string;
    runtimeMinimum: number;
    runtimeMaximum: number;
    yearMinimum: number;
    yearMaximum: number;
    imdbMinimum: number;
    imdbMaximum: number;
    genres: string[];
    returnCount: boolean;
}


interface IMovieSearchResults {
    page: number;
    pageSize: number;
    count: number;
    movies: IMovie[];
}

interface ITVMetadata {
    genres: IGenreCount[];
    releasedMinimumYear: number;
    releasedMaximumYear: number;
    count: number;
}

interface ITVSearchCriteria {
    page: number;
    pageSize: number;
    offset: number;
    wildcardSearch: boolean;
    titleSearch: string;
    sortOrder: string;
    yearMinimum: number;
    yearMaximum: number;
    imdbMinimum: number;
    imdbMaximum: number;
    genres: string[];
    returnCount: boolean;
}

interface ITVSearchResults {
    page: number;
    pageSize: number;
    count: number;
    series: ITVSeries[];
}

interface ITVSeries {
    id: number;
    title: string;
    url: string;
    image: string;
    rating: string;
    closeCaptioned: boolean;
    released: number;
    stars: string;
    ratings: string;
    imdbrating: number;
    plot: string;
    genres: string;
    director: string;
    starring: string;
    supportingActors: string;
}

