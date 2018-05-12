module da {
    "use strict";

    export function movieMetaData(callback: (results: IMovieMetadata) => void): void {
        try {
            var ajaxSettings: JQueryAjaxSettings = {
                type: "GET",
                url: settings.url + "/Movie",
                headers: { "Accept": "application/json", "Content-Type": "application/json" },
                contentType: "application/json; charset=utf-8;",
                dataType: "json",
                success: function (results: any): void {
                    if (results) {
                        callback(<IMovieMetadata> results);
                    }
                },
                error: function (serverErr: JQueryXHR): void {
                    var err = new Error("Error " + serverErr.status + ":" + serverErr.statusText);
                    // log error
                    if (typeof callback === "function") { callback(null); }
                }
            };
            $.ajax(ajaxSettings);
        }
        catch (err) {
            // log error
            if (typeof callback === "function") { callback(null); }
        }
    }

    /// Why doesn't passing data in the body work???
    export function movieSearch(movieSearchCriteria: IMovieSearchCriteria, callback?: (results: IMovieSearchResults) => void): void {
        try {
            var ajaxSettings: JQueryAjaxSettings = {
                type: "POST",
                url: settings.url + "/Movie",
                headers: { "Accept": "application/json", "Content-Type": "application/json" },
                contentType: "application/json; charset=utf-8;",
                dataType: 'json',
                data: JSON.stringify({ movieSearchCriteria: movieSearchCriteria }),
                success: function (results: any): void {
                    if (results) {
                        callback(results);
                    }
                    else {
                        callback(null);
                    }
                },
                error: function (serverErr: JQueryXHR): void {
                    var err = new Error("Error " + serverErr.status + ":" + serverErr.statusText);
                    // log error
                    if (typeof callback === "function") { callback(null); }
                }
            };
            $.ajax(ajaxSettings);
        }
        catch (err) {
            // log error
            if (typeof callback === "function") { callback(null); }
        }
    }

    export function tvMetaData(callback: (results: ITVMetadata) => void): void {
        try {
            var ajaxSettings: JQueryAjaxSettings = {
                type: "GET",
                url: settings.url + "/TV",
                headers: { "Accept": "application/json", "Content-Type": "application/json" },
                contentType: "application/json; charset=utf-8;",
                dataType: "json",
                success: function (results: any): void {
                    if (results) {
                        callback(<ITVMetadata>results);
                    }
                },
                error: function (serverErr: JQueryXHR): void {
                    var err = new Error("Error " + serverErr.status + ":" + serverErr.statusText);
                    // log error
                    if (typeof callback === "function") { callback(null); }
                }
            };
            $.ajax(ajaxSettings);
        }
        catch (err) {
            // log error
            if (typeof callback === "function") { callback(null); }
        }
    }

    /// Why doesn't passing data in the body work???
    export function tvSearch(tvSearchCriteria: ITVSearchCriteria, callback?: (results: ITVSearchResults) => void): void {
        try {
            var ajaxSettings: JQueryAjaxSettings = {
                type: "POST",
                url: settings.url + "/TV",
                headers: { "Accept": "application/json", "Content-Type": "application/json" },
                contentType: "application/json; charset=utf-8;",
                dataType: 'json',
                data: JSON.stringify({ tvSearchCriteria: tvSearchCriteria }),
                success: function (results: any): void {
                    if (results) {
                        callback(<ITVSearchResults> results);
                    }
                    else {
                        callback(null);
                    }
                },
                error: function (serverErr: JQueryXHR): void {
                    var err = new Error("Error " + serverErr.status + ":" + serverErr.statusText);
                    // log error
                    if (typeof callback === "function") { callback(null); }
                }
            };
            $.ajax(ajaxSettings);
        }
        catch (err) {
            // log error
            if (typeof callback === "function") { callback(null); }
        }
    }
}