# PrimeVideoSearch

- Ken Kohler
- v 1.0 Apr 2018
- Licensed: MIT license

This project is an attempt to address the limited search capabilities of Amazon Prime Video's movie and tv library.
The intent of this project is to provide a cross-platform website that can be used to search a copy of Amazon's prime video libary.  Actual implementation differs from true cross platform implementation due to data storage choice and due to technical details with retrieval of AWS data.  With regard to data storage, SQL Server is used for data storage, thus it is limited in this regard to Windows and Linux, excluding Macintosh as a complete solution platform.  

The original intent for data retrieval was to use WebClient to download data, then screen scrape it. Due to Amazon limitations with regard to navigating their website to screen scrape the data, it is required to have an actual browser (or at least the guts of a browser) as part of the data retrieval.  Also, Amazon's use of JavaScript to retrieve movie/tv data after completion of page download and likely the need to have cookie support for paging through the library also force using a real browser.  Due to these limitations, a WinForms application with a webbrowser control is used for the screen scraping application.  This eliminates the possibility of either Linux or Macintosh from a complete cross platform solution.

The project is in two parts:
AWSMovieLister - the WinForms app for capturing the data.
AmazonPrimeVideoSearch - the website for searching the data.


Download SQL Server or SQL Express from Microsoft's website and install.  It is also suggested to download/install SQL Server Management Studio (SSMS) or SQL Operation Studio.
Execute script AmazonPrimeVideoSearchDatabase\database.sql to create the database and schema.  Note that script contains default paths for data/log files.  Edit as needed.

Build and execute AWSMovieLister.  Execution of this app is in 4 stages:
Retrieval of movie list not already in local database.
Retrieval of tv list not already in local database.
Retrieval of movie details.
Retrieval of tv details.

Initially, for both movies and tv, it will be necessary to choose All in the dropdown.  Afterwords, it is recommended to choose Last 7 days and to run app every week to retrieve new movie and tv listings.
After movie and tv lists have been retieved, it will be necessary to retrieve the details.  Detail retrieval navigates to the individual Amazon page for the movie/tv show, then screen scrapes it.  Due to Amazon limitations, after several hundred web pages have been downloaded, Amazon may think this app is a bot and prompt user to enter a code to continue.  If this happens, terminate application and re-run as some other time, possibly the next day to work around this limitation.  This should only really be an issue with the initial retrieval of the movie and tv lists, especially if it is then run weekly to update with new listings.

One limitation, new listings are added, but there is no method for identifying movie and tv listings that are no longer Prime selections.  These will need to be manually deleted as needed.

Once data has been downloaded, build/run AmazonPrimeVideoSearch and enjoy searching the Prime video catalog.