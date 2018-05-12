namespace AWSTVSearch {
    var vm: ViewModel;
    export function onDeviceReady(): void {
        da.tvMetaData(function (metadata: IMovieMetadata): void {
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
        constructor(metadata: ITVMetadata) {
            var self = this;
            var pages = Math.floor(metadata.count / this.pageSize());
            if (metadata.count % this.pageSize()) {
                pages++;
            }
            this.enableNextButton(pages > 1);

            this.metadata = metadata;
            this.count = metadata.count;
            this.genres(metadata.genres);
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
        }

        public count: number;
        public enableBackButton: KnockoutObservable<boolean> = ko.observable(false);
        public enableNextButton: KnockoutObservable<boolean> = ko.observable(false);
        public genres: KnockoutObservableArray<IGenreCount> = ko.observableArray([]);
        public imdbRatingMinimum: KnockoutObservable<number> = ko.observable(0);
        public imdbRatingMaximum: KnockoutObservable<number> = ko.observable(10);
        public metadata: ITVMetadata;
        public page: KnockoutObservable<number> = ko.observable(1);
        public pages: KnockoutObservable<number> = ko.observable(0);
        public pageSize: KnockoutObservable<number> = ko.observable(5);
        public releasedMaximumYear: KnockoutObservable<number> = ko.observable(0);
        public releasedMinimumYear: KnockoutObservable<number> = ko.observable(0);
        private searchCount: number;
        private searchTimer: number;
        public selectedGenres: KnockoutObservableArray<string> = ko.observableArray([]);
        public sortOrder: KnockoutObservable<string> = ko.observable("Title");
        private timerValue = 500;
        public titleSearch: KnockoutObservable<string> = ko.observable("");
        public tvSeries: KnockoutObservableArray<ITVSeries> = ko.observableArray([]);
        public wildcardSearch: KnockoutObservable<boolean> = ko.observable(false);
        public yearMaximum: number;
        public yearMinimum: number;

        public backClick = function () {
            vm.search(false, vm.page() - 1);
        }

        public movieClick = function () {
            window.location.href = "./index.html";
        }

        public nextClick = function () {
            vm.search(false, vm.page() + 1);
        }

        private search = function (returnCount: boolean, searchPage: number): void {
            var tvSearchCriteria: ITVSearchCriteria = {
                page: searchPage,
                pageSize: vm.pageSize(),
                offset: (searchPage - 1) * vm.pageSize(),
                wildcardSearch: vm.wildcardSearch(),
                titleSearch: vm.titleSearch(),
                sortOrder: vm.sortOrder(),
                yearMinimum: vm.releasedMinimumYear(),
                yearMaximum: vm.releasedMaximumYear(),
                imdbMinimum: vm.imdbRatingMinimum(),
                imdbMaximum: vm.imdbRatingMaximum(),
                genres: vm.selectedGenres(),
                returnCount: returnCount
            };
            if (tvSearchCriteria.yearMinimum === vm.metadata.releasedMinimumYear) {
                tvSearchCriteria.yearMinimum = -1;
            }
            if (tvSearchCriteria.yearMaximum === vm.metadata.releasedMaximumYear) {
                tvSearchCriteria.yearMaximum = -1;
            }
            if (tvSearchCriteria.imdbMinimum === 0) {
                tvSearchCriteria.imdbMinimum = -1;
            }
            if (tvSearchCriteria.imdbMaximum === 10) {
                tvSearchCriteria.imdbMaximum = -1;
            }
            da.tvSearch(tvSearchCriteria, function (results: ITVSearchResults): void {
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
                vm.page(tvSearchCriteria.page);
                vm.tvSeries(results.series);
            });
        };

    }
}

AWSTVSearch.onDeviceReady();