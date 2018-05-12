namespace AWSMovieSearch {
    var vm: ViewModel;
    export function onDeviceReady(): void {
        da.movieMetaData(function (metadata: IMovieMetadata): void {
            if (metadata) {
                vm = new ViewModel(metadata);
                ko.applyBindings(vm);
            }
        });
    }

    function precisionRound(number, precision) {
        var factor = Math.pow(10, precision);
        return Math.round(number * factor) / factor;
    }

    class ViewModel {
        constructor(metadata: IMovieMetadata) {
            var self = this;
            var pages = Math.floor(metadata.count / this.pageSize());
            if (metadata.count % this.pageSize()) {
                pages++;
            }
            this.enableNextButton(pages > 1);

            this.metadata = metadata;
            this.count = metadata.count;
            this.genres(metadata.genres);
            this.mpaaRating.subscribe(function (e) {
                vm.search(true,1);
            });
            this.pages(pages);
            this.pageSize.subscribe(function (e) {
                vm.search(true, 1);
            });
            this.searchCount = metadata.count;
            this.selectedGenres.subscribe(function (e) {
                vm.search(true,1);
            });
            this.sortOrder.subscribe(function (e) {
                vm.search(true,1);
            });
            this.yearMinimum = metadata.releasedMinimumYear;
            this.yearMaximum = metadata.releasedMaximumYear;
            this.releasedMaximumYear(metadata.releasedMaximumYear);
            this.releasedMinimumYear(metadata.releasedMinimumYear);
            this.runtimeMaximum(metadata.runtimeMaximum);
            this.runtimeMinimum(metadata.runtimeMinimum);
            this.titleSearch.subscribe(function (e) {
                if (vm.searchTimer) {
                    clearTimeout(vm.searchTimer);
                    vm.searchTimer = null;
                }
                vm.searchTimer = setTimeout(function () {
                    vm.search(true,1);
                }, self.timerValue);
            });
            this.wildcardSearch.subscribe(function (e) {
                if (vm.titleSearch().length) {
                    vm.search(true,1);
                }
            });
            var yearSlider = $("#yearSlider")[0];
            noUiSlider.create(yearSlider, {
                start: [this.releasedMinimumYear(), this.releasedMaximumYear()],
                connect: true,
                range: {
                    'min': this.releasedMinimumYear(),
                    'max': this.releasedMaximumYear()
                }
            });
            (<any>yearSlider).noUiSlider.on('update', function (values, handle) {
                if (self.searchTimer) {
                    clearTimeout(self.searchTimer);
                    self.searchTimer = null;
                }
                self.releasedMinimumYear(Math.round(values[0]));
                self.releasedMaximumYear(Math.round(values[1]));
                self.searchTimer = setTimeout(function () {
                    self.search(true,1);
                }, self.timerValue);
            });

            var imdbSlider = $("#imdbSlider")[0];
            noUiSlider.create(imdbSlider, {
                start: [this.imdbRatingMinimum(), this.imdbRatingMaximum()],
                connect: true,
                range: {
                    'min': this.imdbRatingMinimum(),
                    'max': this.imdbRatingMaximum()
                }
            });
            (<any> imdbSlider).noUiSlider.on('update', function (values, handle) {
                if (self.searchTimer) {
                    clearTimeout(self.searchTimer);
                    self.searchTimer = null;
                }
                self.imdbRatingMinimum(precisionRound(values[0],1));
                self.imdbRatingMaximum(precisionRound(values[1],1));
                self.searchTimer = setTimeout(function () {
                    self.search(true,1);
                }, self.timerValue);
            });

            var runtimeSlider = $("#runtimeSlider")[0];
            noUiSlider.create(runtimeSlider, {
                start: [this.runtimeMinimum(), this.runtimeMaximum()],
                connect: true,
                range: {
                    'min': this.runtimeMinimum(),
                    'max': this.runtimeMaximum()
                }
            });
            (<any> runtimeSlider).noUiSlider.on('update', function (values, handle) {
                if (self.searchTimer) {
                    clearTimeout(self.searchTimer);
                    self.searchTimer = null;
                }
                self.runtimeMinimum(Math.round(values[0]));
                self.runtimeMaximum(Math.round(values[1]));
                self.searchTimer = setTimeout(function () {
                    self.search(true,1);
                }, self.timerValue);
            });
        }

        public count: number;
        public enableBackButton: KnockoutObservable<boolean> = ko.observable(false);
        public enableNextButton: KnockoutObservable<boolean> = ko.observable(false);
        public genres: KnockoutObservableArray<IGenreCount> = ko.observableArray([]);
        public imdbRatingMinimum: KnockoutObservable<number> = ko.observable(0);
        public imdbRatingMaximum: KnockoutObservable<number> = ko.observable(10);
        public metadata: IMovieMetadata;
        public movies: KnockoutObservableArray<IMovie> = ko.observableArray([]);
        public mpaaRating: KnockoutObservable<string> = ko.observable("");
        public page: KnockoutObservable<number> = ko.observable(1);
        public pages: KnockoutObservable<number> = ko.observable(0);
        public pageSize: KnockoutObservable<number> = ko.observable(5);
        public releasedMaximumYear: KnockoutObservable<number> = ko.observable(0);
        public releasedMinimumYear: KnockoutObservable<number> = ko.observable(0);
        public runtimeMinimum: KnockoutObservable<number> = ko.observable(0);
        private searchCount: number;
        private searchTimer: number;
        public runtimeMaximum: KnockoutObservable<number> = ko.observable(0);
        public selectedGenres: KnockoutObservableArray<string> = ko.observableArray([]);
        public sortOrder: KnockoutObservable<string> = ko.observable("Title");
        private timerValue = 500;
        public titleSearch: KnockoutObservable<string> = ko.observable("");
        public wildcardSearch: KnockoutObservable<boolean> = ko.observable(false);
        public yearMaximum: number;
        public yearMinimum: number;

        public backClick = function () {
            vm.search(false, vm.page() - 1);
        }
        public nextClick = function () {
            vm.search(false, vm.page() + 1);
        }

        private search = function (returnCount: boolean, searchPage: number): void {
            var movieSearchCriteria: IMovieSearchCriteria = {
                page: searchPage,
                pageSize: vm.pageSize(),
                offset: (searchPage - 1) * vm.pageSize(),
                wildcardSearch: vm.wildcardSearch(),
                titleSearch: vm.titleSearch(),
                sortOrder: vm.sortOrder(),
                mPAARating: vm.mpaaRating(),
                runtimeMinimum: vm.runtimeMinimum(),
                runtimeMaximum: vm.runtimeMaximum(),
                yearMinimum: vm.releasedMinimumYear(),
                yearMaximum: vm.releasedMaximumYear(),
                imdbMinimum: vm.imdbRatingMinimum(),
                imdbMaximum: vm.imdbRatingMaximum(),
                genres: vm.selectedGenres(),
                returnCount: returnCount
            };
            if (movieSearchCriteria.yearMinimum === vm.metadata.releasedMinimumYear) {
                movieSearchCriteria.yearMinimum = -1;
            }
            if (movieSearchCriteria.yearMaximum === vm.metadata.releasedMaximumYear) {
                movieSearchCriteria.yearMaximum = -1;
            }
            if (movieSearchCriteria.imdbMinimum === 0) {
                movieSearchCriteria.imdbMinimum = -1;
            }
            if (movieSearchCriteria.imdbMaximum === 10) {
                movieSearchCriteria.imdbMaximum = -1;
            }
            if (movieSearchCriteria.runtimeMinimum === vm.metadata.runtimeMinimum) {
                movieSearchCriteria.runtimeMinimum = -1;
            }
            if (movieSearchCriteria.runtimeMaximum === vm.metadata.runtimeMaximum) {
                movieSearchCriteria.runtimeMaximum = -1;
            }
            da.movieSearch(movieSearchCriteria, function (results: IMovieSearchResults): void {
                if (results.count !== -1) {
                    vm.count = results.count;
                    if (results.count === 0) {
                        vm.page(0);
                        vm.pages(0)
                        vm.enableBackButton(false);
                        vm.enableNextButton(false);
                    }
                    else {
                        var pages = Math.floor(results.count / vm.pageSize());
                        if (results.count % vm.pageSize()) {
                            pages++;
                        }
                        vm.page(1);
                        vm.pages(pages);
                        vm.enableBackButton(false);
                        vm.enableNextButton(pages > 1);
                    }
                }
                else {
                    vm.page(results.page);
                    vm.enableBackButton(vm.page() > 1);
                    vm.enableNextButton(results.page < vm.pages());
                }
                vm.page(movieSearchCriteria.page);
                vm.movies(results.movies);
            });
        };

        public tvClick = function () {
            window.location.href = "./tvseries.html";
        }

    }
}

AWSMovieSearch.onDeviceReady();