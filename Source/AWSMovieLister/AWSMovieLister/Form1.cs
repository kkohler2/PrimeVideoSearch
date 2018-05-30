/*
	Ken Kohler
	Prime Video Search v 1.0
	v 1.0 Apr 2018
	Licensed: MIT license
*/

// dotnet ef dbcontext scaffold "Server=.\;Database=AWSPrimeStreaming;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -o Model
using AWSMovieLister.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Windows.Forms;

namespace AWSMovieLister
{
    public partial class Form1 : Form
    {
        string nextURL = string.Empty;
        public int page = 0;
        public int foundItems = 0;
        public int newItems = 0;
        public AWSPrimeStreamingContext context;
        public int itemsToProcess = 0;
        public int itemsProcessed = 0;
        public int badURLs = 0;
        public List<Movie> moviesForDetails;
        public List<Tvseries> tvForDetails;

        public Form1()
        {
            InitializeComponent();
            context = new AWSPrimeStreamingContext();
            cbDownloadScope.SelectedIndex = 0;
        }

        private void DocumentCompleted()
        {
            string text = "";
            try
            {
                text = webBrowser1.Document.Body.OuterHtml;
            }
            catch (Exception)
            {
            }

            if (cbDetails.Checked)
            {
                int pos = text.IndexOf("Sorry! We couldn't find that page.");
                if (rbMovies.Checked)
                {
                    if (pos == -1)
                    {
                        ParseMovieDetails(text);
                    }
                    else
                    {
                        badURLs++;
                        itemsProcessed++;
                        if (itemsProcessed < moviesForDetails.Count())
                        {
                            lblDetailProgress.Text = $"{itemsProcessed} of {itemsToProcess}  Bad URLs: {badURLs}";
                            nextURL = moviesForDetails[itemsProcessed].Url;
                            webBrowser1.Navigate(nextURL);
                            while (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                            {
                                Application.DoEvents();
                            }
                            timer1.Start();
                        }
                        else
                        {
                            MessageBox.Show("Update complete");
                        }
                    }
                }
                else
                {
                    if (pos == -1)
                    {
                        ParseTVDetails(text);
                    }
                    else
                    {
                        badURLs++;
                        itemsProcessed++;
                        if (itemsProcessed < tvForDetails.Count())
                        {
                            lblDetailProgress.Text = $"{itemsProcessed} of {itemsToProcess}  Bad URLs: {badURLs}";
                            nextURL = tvForDetails[itemsProcessed].Url;
                            webBrowser1.Navigate(nextURL);
                            while (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                            {
                                Application.DoEvents();
                            }
                            timer1.Start();
                        }
                        else
                        {
                            MessageBox.Show("Update complete");
                        }
                    }
                }
            }
            else
            {
                page++;
                tbPage.Text = page.ToString();
                if (rbMovies.Checked)
                {
                    ParseMoviePage(text, webBrowser1);
                }
                else
                {
                    ParseTVPage(text, webBrowser1);
                }
            }
        }

        public void ParseMovieDetails(string result)
        {
            try
            {
                Movie movie = moviesForDetails[itemsProcessed];
                if (result.IndexOf("To discuss automated access to Amazon") != -1)
                {
                    MessageBox.Show("Update complete");
                    return;
                }
                using (AWSPrimeStreamingContext context = new AWSPrimeStreamingContext())
                {
                    List<string> genres = new List<string>();
                    context.Update(movie);
                    int pos = result.IndexOf("Watch for 0.00 with a Prime");
                    movie.IsPrime = pos != -1;

                    pos = result.IndexOf("av-icon--amazon_rating");
                    int pos2;
                    int pos3;
                    if (pos != -1)
                    {
                        pos = result.IndexOf("(", pos);
                        pos2 = result.IndexOf(")", pos);
                        movie.Ratings = result.Substring(pos + 1, pos2 - pos - 1);
                    }

                    pos = result.IndexOf("\"imdb-rating-badge\"");
                    if (pos != -1)
                    {
                        pos = result.IndexOf(">", pos);
                        pos2 = result.IndexOf("<", pos);
                        try
                        {
                            movie.Imdbrating = Convert.ToSingle(result.Substring(pos + 1, pos2 - pos - 1));
                        }
                        catch (Exception)
                        {
                        }
                    }

                    pos = result.IndexOf("\"synopsis\"");
                    if (pos != -1)
                    {
                        pos = result.IndexOf("<p", pos);
                        if (pos > -1)
                        {
                            pos = result.IndexOf(">", pos);
                            pos2 = result.IndexOf("</p>", pos);
                            try
                            {
                                movie.Plot = HttpUtility.HtmlDecode(result.Substring(pos + 1, pos2 - pos - 1).Replace("Now included with Prime.", "").Replace("Now on Prime.  ", "").Trim());
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }

                    int posProductDetails = result.IndexOf("Product details");
                    if (posProductDetails != -1)
                    {
                        int posOtherFormats = result.IndexOf("Other formats", posProductDetails);
                        int posTR = -1;
                        pos = result.IndexOf("Genres", posProductDetails);
                        if (pos > posOtherFormats)
                        {
                            pos = -1;
                        }
                        if (pos != -1)
                        {
                            posTR = result.IndexOf("<tr", pos);
                            pos3 = result.IndexOf("</td>", pos);
                            pos = result.IndexOf("<a", pos);
                            movie.Genres = string.Empty;
                            while (pos < pos3 && pos < posTR && pos != -1)
                            {
                                if (pos >= posOtherFormats)
                                {
                                    break;
                                }
                                pos = result.IndexOf(">", pos);
                                pos2 = result.IndexOf("</a>", pos);
                                try
                                {
                                    string genre = HttpUtility.HtmlDecode(result.Substring(pos + 1, pos2 - pos - 1));
                                    genres.Add(genre);
                                    genre += ", ";
                                    movie.Genres += genre;
                                    pos = result.IndexOf("<a", pos);
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }

                        pos = result.IndexOf("Director", posProductDetails);
                        if (pos != -1)
                        {
                            posTR = result.IndexOf("<tr", pos);
                            pos3 = result.IndexOf("</td>", pos);
                            pos = result.IndexOf("<a", pos);
                            if (pos < pos3 && pos < posTR && pos != -1)
                            {
                                pos = result.IndexOf(">", pos);
                                pos2 = result.IndexOf("</a>", pos);
                                try
                                {
                                    movie.Director = HttpUtility.HtmlDecode(result.Substring(pos + 1, pos2 - pos - 1));
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                        pos = result.IndexOf("Starring", posProductDetails);
                        if (pos != -1)
                        {
                            posTR = result.IndexOf("<tr", pos);
                            pos3 = result.IndexOf("</td>", pos);
                            pos = result.IndexOf("<a", pos);
                            movie.Starring = string.Empty;
                            while (pos < pos3 && pos < posTR && pos != -1)
                            {
                                if (pos >= posOtherFormats)
                                {
                                    break;
                                }
                                pos = result.IndexOf(">", pos);
                                pos2 = result.IndexOf("</a>", pos);
                                try
                                {
                                    string data = HttpUtility.HtmlDecode(result.Substring(pos + 1, pos2 - pos - 1)) + ", ";
                                    movie.Starring += data;
                                    pos = result.IndexOf("<a", pos);
                                }
                                catch (Exception)
                                {
                                }
                            }
                            if (movie.Starring != null && movie.Starring.Length > 0)
                            {
                                movie.Starring = movie.Starring.Replace("  ", " ");
                                movie.Starring = movie.Starring.Substring(0, movie.Starring.Length - 2);
                            }
                        }

                        pos = result.IndexOf("Supporting actors", posProductDetails);
                        if (pos != -1)
                        {
                            posTR = result.IndexOf("<tr", pos);
                            pos3 = result.IndexOf("</td>", pos);
                            pos = result.IndexOf("<a", pos);
                            movie.SupportingActors = string.Empty;
                            while (pos < pos3 && pos < posTR && pos != -1)
                            {
                                if (pos >= posOtherFormats)
                                {
                                    break;
                                }
                                pos = result.IndexOf(">", pos);
                                pos2 = result.IndexOf("</a>", pos);
                                try
                                {
                                    string data = HttpUtility.HtmlDecode(result.Substring(pos + 1, pos2 - pos - 1)) + ", ";
                                    movie.SupportingActors += data;
                                    pos = result.IndexOf("<a", pos);
                                }
                                catch (Exception)
                                {
                                }
                            }
                            if (movie.SupportingActors != null && movie.SupportingActors.Length > 0)
                            {
                                movie.SupportingActors = movie.SupportingActors.Replace("  ", " ");
                                movie.SupportingActors = movie.SupportingActors.Substring(0, movie.SupportingActors.Length - 2);
                            }
                        }
                    }

                    context.SaveChanges();
                    foreach (string _genre in genres)
                    {
                        Genre genre = context.Genre.Where(g => g.Name == _genre).SingleOrDefault();
                        if (genre == null)
                        {
                            genre = new Genre();
                            genre.Name = _genre;
                            context.Add(genre);
                            context.SaveChanges();
                        }
                        MovieGenre movieGenre = new MovieGenre();
                        movieGenre.Genre = genre;
                        movieGenre.Movie = movie;
                        context.Add(movieGenre);
                        try
                        {
                            context.SaveChanges();
                        }
                        catch (Exception)
                        {

                        }
                    }
                }

            }
            catch (Exception)
            {
            }

            itemsProcessed++;
            lblDetailProgress.Text = $"{itemsProcessed} of {itemsToProcess}  Bad URLs: {badURLs}";
            if (itemsProcessed < moviesForDetails.Count())
            {
                nextURL = moviesForDetails[itemsProcessed].Url;
                webBrowser1.Navigate(nextURL);
                while (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                {
                    Application.DoEvents();
                }
                timer1.Start();
            }
            else
            {
                MessageBox.Show("Update complete");
            }
        }

        public void ParseTVDetails(string result)
        {
            try
            {
                List<string> genres = new List<string>();
                Tvseries tv = tvForDetails[itemsProcessed];
                context.Tvseries.Update(tv);
                int pos = result.IndexOf("Watch for 0.00 with Prime");
                tv.IsPrime = pos != -1;

                pos = result.IndexOf("av-icon--amazon_rating");
                int pos2;
                int pos3;
                if (pos != -1)
                {
                    pos = result.IndexOf("(", pos);
                    pos2 = result.IndexOf(")", pos);
                    tv.Ratings = result.Substring(pos + 1, pos2 - pos - 1);
                }

                pos = result.IndexOf("\"imdb-rating-badge\"");
                if (pos != -1)
                {
                    pos = result.IndexOf(">", pos);
                    pos2 = result.IndexOf("<", pos);
                    try
                    {
                        tv.Imdbrating = Convert.ToSingle(result.Substring(pos + 1, pos2 - pos - 1));
                    }
                    catch (Exception)
                    {
                    }
                }

                pos = result.IndexOf("\"synopsis\"");
                if (pos != -1)
                {
                    pos = result.IndexOf("<p", pos);
                    if (pos > -1)
                    {
                        pos = result.IndexOf(">", pos);
                        pos2 = result.IndexOf("</p>", pos);
                        try
                        {
                            tv.Plot = result.Substring(pos + 1, pos2 - pos - 1).Replace("Now included with Prime.", "").Trim();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

                int posProductDetails = result.IndexOf("Product details");
                if (posProductDetails != -1)
                {
                    int posOtherFormats = result.IndexOf("Customer Reviews", posProductDetails);
                    int posTR = -1;
                    pos = result.IndexOf("Genres", posProductDetails);
                    if (pos > posOtherFormats)
                    {
                        pos = -1;
                    }
                    if (pos != -1)
                    {
                        posTR = result.IndexOf("<tr", pos);
                        pos3 = result.IndexOf("</td>", pos);
                        pos = result.IndexOf("<a", pos);
                        tv.Genres = string.Empty;
                        while (pos < pos3 && pos < posTR && pos != -1)
                        {
                            if (pos >= posOtherFormats)
                            {
                                break;
                            }
                            pos = result.IndexOf(">", pos);
                            pos2 = result.IndexOf("</a>", pos);
                            try
                            {
                                string genre = HttpUtility.HtmlDecode(result.Substring(pos + 1, pos2 - pos - 1));
                                genres.Add(genre);
                                genre += ", ";
                                tv.Genres += genre;
                                pos = result.IndexOf("<a", pos);
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }

                    pos = result.IndexOf("Director", posProductDetails);
                    if (pos != -1)
                    {
                        posTR = result.IndexOf("<tr", pos);
                        pos3 = result.IndexOf("</td>", pos);
                        pos = result.IndexOf("<a", pos);
                        if (pos < pos3 && pos < posTR && pos != -1)
                        {
                            pos = result.IndexOf(">", pos);
                            pos2 = result.IndexOf("</a>", pos);
                            try
                            {
                                tv.Director = HttpUtility.HtmlDecode(result.Substring(pos + 1, pos2 - pos - 1));
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    pos = result.IndexOf("Starring", posProductDetails);
                    if (pos != -1)
                    {
                        posTR = result.IndexOf("<tr", pos);
                        pos3 = result.IndexOf("</td>", pos);
                        pos = result.IndexOf("<a", pos);
                        tv.Starring = string.Empty;
                        while (pos < pos3 && pos < posTR && pos != -1)
                        {
                            if (pos >= posOtherFormats)
                            {
                                break;
                            }
                            pos = result.IndexOf(">", pos);
                            pos2 = result.IndexOf("</a>", pos);
                            try
                            {
                                string data = HttpUtility.HtmlDecode(result.Substring(pos + 1, pos2 - pos - 1)) + ", ";
                                tv.Starring += data;
                                pos = result.IndexOf("<a", pos);
                            }
                            catch (Exception)
                            {
                            }
                        }
                        if (tv.Starring != null && tv.Starring.Length > 0)
                        {
                            tv.Starring = tv.Starring.Replace("  ", " ");
                            tv.Starring = tv.Starring.Substring(0, tv.Starring.Length - 2);
                        }
                    }

                    pos = result.IndexOf("Supporting actors", posProductDetails);
                    if (pos != -1)
                    {
                        posTR = result.IndexOf("<tr", pos);
                        pos3 = result.IndexOf("</td>", pos);
                        pos = result.IndexOf("<a", pos);
                        tv.SupportingActors = string.Empty;
                        while (pos < pos3 && pos < posTR && pos != -1)
                        {
                            if (pos >= posOtherFormats)
                            {
                                break;
                            }
                            pos = result.IndexOf(">", pos);
                            pos2 = result.IndexOf("</a>", pos);
                            try
                            {
                                string data = HttpUtility.HtmlDecode(result.Substring(pos + 1, pos2 - pos - 1)) + ", ";
                                tv.SupportingActors += data;
                                pos = result.IndexOf("<a", pos);
                            }
                            catch (Exception)
                            {
                            }
                        }
                        if (tv.SupportingActors != null && tv.SupportingActors.Length > 0)
                        {
                            tv.SupportingActors = tv.SupportingActors.Replace("  ", " ");
                            tv.SupportingActors = tv.SupportingActors.Substring(0, tv.SupportingActors.Length - 2);
                        }
                    }
                }

                context.SaveChanges();
                foreach (string _genre in genres)
                {
                    Genre genre = context.Genre.Where(g => g.Name == _genre).SingleOrDefault();
                    if (genre == null)
                    {
                        genre = new Genre();
                        genre.Name = _genre;
                        context.Add(genre);
                        context.SaveChanges();
                    }
                    TvseriesGenre tvseriesGenre = new TvseriesGenre();
                    tvseriesGenre.Genre = genre;
                    tvseriesGenre.Tvseries = tv;
                    context.Add(tvseriesGenre);
                    context.SaveChanges();
                }

                itemsProcessed++;
                lblDetailProgress.Text = $"{itemsProcessed} of {itemsToProcess}  Bad URLs: {badURLs}";
                if (itemsProcessed < tvForDetails.Count())
                {
                    nextURL = tvForDetails[itemsProcessed].Url;
                    webBrowser1.Navigate(nextURL);
                    while (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                    {
                        Application.DoEvents();
                    }
                    timer1.Start();
                }
                else
                {
                    MessageBox.Show("Update complete");
                }
            }
            catch (Exception ex)
            {
                var error = ex;
            }
        }

        public void ParseMoviePage(string fileContent, WebBrowser browser)
        {
            List<string> movieContent = new List<string>();
            int pos;
            while (true)
            {
                pos = fileContent.IndexOf("<!--");
                if (pos == -1)
                    break;
                int pos2 = fileContent.IndexOf("-->", pos);
                if (pos2 == -1)
                    break;
                fileContent = fileContent.Remove(pos, pos2 - pos + 3);
            }

            string searchValue = "data-asin=\"";

            pos = fileContent.IndexOf(searchValue, 0);
            string tempContent = fileContent.Substring(pos);
            while (true)
            {
                int pos2 = tempContent.IndexOf(searchValue, 0);
                int pos3 = tempContent.IndexOf("</li>", pos2 + 1);
                string movie = tempContent.Substring(pos2, pos3 - pos2 + 5);
                movieContent.Add(movie);
                tempContent = tempContent.Substring(pos3 + 5);
                if (tempContent.IndexOf(searchValue) == -1)
                {
                    break;
                }
            }
            foundItems += movieContent.Count;
            tbFound.Text = foundItems.ToString();
            foreach (string _movie in movieContent)
            {
                pos = _movie.IndexOf("data-asin=\"");
                int pos2 = _movie.IndexOf("\"", pos + 12);
                string dataSin = _movie.Substring(pos + 11);
                pos2 = dataSin.IndexOf("\"", 0);
                dataSin = dataSin.Substring(0, pos2);

                Movie movie = new Movie();
                movie.DataSin = dataSin;

                pos = _movie.IndexOf("out of 5 stars");
                if (pos != -1)
                {
                    pos2 = _movie.IndexOf("<", pos);
                    while (pos > 0 && _movie[pos] != '>')
                    {
                        pos--;
                    }
                    if (_movie[pos] == '>')
                    {
                        movie.Stars = _movie.Substring(pos + 1, pos2 - pos - 1);
                    }
                }
                pos = _movie.IndexOf("src=\"");
                pos2 = _movie.IndexOf("\"", pos + 6);
                movie.Image = _movie.Substring(pos + 5);
                pos2 = movie.Image.IndexOf("\"", 0);
                movie.Image = movie.Image.Substring(0, pos2);

                pos = _movie.IndexOf("href=\"");
                pos2 = _movie.IndexOf("\"", pos + 7);
                movie.Url = _movie.Substring(pos + 6);
                pos2 = movie.Url.IndexOf("\"", 0);
                movie.Url = movie.Url.Substring(0, pos2);

                pos = _movie.IndexOf("title=\"");
                pos2 = _movie.IndexOf("\"", pos + 8);
                movie.Title = _movie.Substring(pos + 7);
                pos2 = movie.Title.IndexOf("\"", 0);
                movie.SearchTitle = movie.Title = HttpUtility.HtmlDecode(movie.Title.Substring(0, pos2));
                if (movie.Title.ToUpper().IndexOf("THE ") == 0)
                {
                    movie.SearchTitle = movie.Title.Substring(4) + ", " + movie.Title.Substring(0, 3);
                }

                pos = _movie.IndexOf("a-icon-alt\">");
                if (pos != -1)
                {
                    pos2 = _movie.IndexOf("\"", pos + 11);
                    movie.Stars = _movie.Substring(pos + 12);
                    pos2 = movie.Stars.IndexOf("<", 0);
                    movie.Stars = movie.Stars.Substring(0, pos2);
                }

                pos = 0;
                bool nextIsRuntime = false;
                while (pos > -1)
                {
                    pos = _movie.IndexOf("class=\"a-size-small a-color-secondary\">", pos + 1);
                    if (pos != -1)
                    {
                        pos2 = _movie.IndexOf("<", pos + 1);
                        string value = _movie.Substring(pos + 39, pos2 - pos - 39);
                        if (nextIsRuntime)
                        {
                            nextIsRuntime = false;
                            movie.RuntimeDisplay = value;
                            if (movie.RuntimeDisplay != null)
                            {
                                context.Movie.Update(movie);
                                int pos3 = movie.RuntimeDisplay.IndexOf(" hr");
                                if (pos3 != -1)
                                {
                                    movie.Runtime = Convert.ToInt32(movie.RuntimeDisplay.Substring(0, pos3)) * 60;
                                    if (movie.RuntimeDisplay.IndexOf(" mins") != -1)
                                    {
                                        int pos4 = movie.RuntimeDisplay.IndexOf(' ', pos3 + 3);
                                        movie.Runtime += Convert.ToInt32(movie.RuntimeDisplay.Substring(pos4 + 1).Replace(" mins", ""));
                                    }
                                }
                                else if (movie.RuntimeDisplay.IndexOf(" mins") != -1)
                                {
                                    movie.Runtime = Convert.ToInt32(movie.RuntimeDisplay.Replace(" mins", ""));
                                }
                                else if (movie.RuntimeDisplay.IndexOf(" min") != -1)
                                {
                                    movie.Runtime = Convert.ToInt32(movie.RuntimeDisplay.Replace(" min", ""));
                                }
                            }
                        }
                        else if (movie.Released == 0)
                        {
                            int year = -1;
                            if (int.TryParse(value, out year))
                            {
                                movie.Released = year;
                            }
                        }
                        else
                        {
                            switch (value)
                            {
                                case "G":
                                case "PG":
                                case "PG-13":
                                case "R":
                                case "NC-17":
                                case "X":
                                case "XXX":
                                    movie.Rating = value;
                                    break;
                                case "CC":
                                    movie.ClosedCaptioned = true;
                                    break;
                                case "Runtime:":
                                    nextIsRuntime = true;
                                    break;
                            }
                        }
                    }
                }
                var existingRecord = context.Movie.Where(m => m.Released == movie.Released && m.Title == movie.Title).FirstOrDefault<Movie>();
                if (existingRecord != null)
                {
                    try
                    {
                        context.Remove(movie);
                    }
                    catch (Exception)
                    {

                    }
                    movie = null;
                    continue;
                }

                context.Add(movie);
                newItems += 1;
                tbNew.Text = newItems.ToString();
                context.SaveChanges();
            }
            var nextButton = browser.Document.GetElementById("pagnNextLink");
            if (nextButton == null)
            {
                MessageBox.Show("Update complete");
                rbMovies.Enabled = true;
                rbTV.Enabled = true;
                btnSearch.Enabled = true;
                cbDownloadScope.Enabled = true;
            }
            else
            {
                nextButton.InvokeMember("click");
                while (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                {
                    Application.DoEvents();
                }
                timer1.Start();
            }
        }

        public void ParseTVPage(string fileContent, WebBrowser browser)
        {
            List<string> tvContent = new List<string>();
            int pos;
            while (true)
            {
                pos = fileContent.IndexOf("<!--");
                if (pos == -1)
                    break;
                int pos2 = fileContent.IndexOf("-->", pos);
                if (pos2 == -1)
                    break;
                fileContent = fileContent.Remove(pos, pos2 - pos + 3);
            }

            string searchValue = "data-asin=\"";

            pos = fileContent.IndexOf(searchValue, 0);
            string tempContent = fileContent.Substring(pos);
            while (true)
            {
                int pos2 = tempContent.IndexOf(searchValue, 0);
                int pos3 = tempContent.IndexOf("</li>", pos2 + 1);
                string tvseries = tempContent.Substring(pos2, pos3 - pos2 + 5);
                tvContent.Add(tvseries);
                tempContent = tempContent.Substring(pos3 + 5);
                if (tempContent.IndexOf(searchValue) == -1)
                {
                    break;
                }
            }
            foundItems += tvContent.Count;
            tbFound.Text = foundItems.ToString();
            foreach (string _tvshow in tvContent)
            {
                pos = _tvshow.IndexOf("data-asin=\"");
                int pos2 = _tvshow.IndexOf("\"", pos + 12);
                string dataSin = _tvshow.Substring(pos + 11);
                pos2 = dataSin.IndexOf("\"", 0);
                dataSin = dataSin.Substring(0, pos2);

                Tvseries tvSeries = new Tvseries();
                tvSeries.DataSin = dataSin;

                pos = _tvshow.IndexOf("out of 5 stars");
                if (pos != -1)
                {
                    pos2 = _tvshow.IndexOf("<", pos);
                    while (pos > 0 && _tvshow[pos] != '>')
                    {
                        pos--;
                    }
                    if (_tvshow[pos] == '>')
                    {
                        tvSeries.Stars = _tvshow.Substring(pos + 1, pos2 - pos - 1);
                    }
                }
                pos = _tvshow.IndexOf("src=\"");
                pos2 = _tvshow.IndexOf("\"", pos + 6);
                tvSeries.Image = _tvshow.Substring(pos + 5);
                pos2 = tvSeries.Image.IndexOf("\"", 0);
                tvSeries.Image = tvSeries.Image.Substring(0, pos2);

                pos = _tvshow.IndexOf("href=\"");
                pos2 = _tvshow.IndexOf("\"", pos + 7);
                tvSeries.Url = _tvshow.Substring(pos + 6);
                pos2 = tvSeries.Url.IndexOf("\"", 0);
                tvSeries.Url = tvSeries.Url.Substring(0, pos2);

                pos = _tvshow.IndexOf("title=\"");
                pos2 = _tvshow.IndexOf("\"", pos + 8);
                tvSeries.Title = _tvshow.Substring(pos + 7);
                pos2 = tvSeries.Title.IndexOf("\"", 0);
                tvSeries.SearchTitle = tvSeries.Title = HttpUtility.HtmlDecode(tvSeries.Title.Substring(0, pos2));
                if (tvSeries.Title.ToUpper().IndexOf("THE ") == 0)
                {
                    tvSeries.SearchTitle = tvSeries.Title.Substring(4) + ", " + tvSeries.Title.Substring(0, 3);
                }

                pos = _tvshow.IndexOf("a-icon-alt\">");
                if (pos != -1)
                {
                    pos2 = _tvshow.IndexOf("\"", pos + 11);
                    tvSeries.Stars = _tvshow.Substring(pos + 12);
                    pos2 = tvSeries.Stars.IndexOf("<", 0);
                    tvSeries.Stars = tvSeries.Stars.Substring(0, pos2);
                }

                pos = 0;
                while (pos > -1)
                {
                    pos = _tvshow.IndexOf("class=\"a-size-small a-color-secondary\">", pos + 1);
                    if (pos != -1)
                    {
                        pos2 = _tvshow.IndexOf("<", pos + 1);
                        string value = _tvshow.Substring(pos + 39, pos2 - pos - 39);
                        if (tvSeries.Released == 0)
                        {
                            int year = -1;
                            if (int.TryParse(value, out year))
                            {
                                tvSeries.Released = year;
                            }
                        }
                        else
                        {
                            switch (value)
                            {
                                case "CC":
                                    tvSeries.ClosedCaptioned = true;
                                    break;
                            }
                        }
                    }
                }
                object existingRecord = context.Tvseries.Where(t => t.Released == tvSeries.Released && t.Title == tvSeries.Title).FirstOrDefault<Tvseries>();
                if (existingRecord != null)
                {
                    try
                    {
                        context.Remove(tvSeries);
                    }
                    catch (Exception)
                    {

                    }
                    tvSeries = null;
                    continue;
                }
                context.Add(tvSeries);
                newItems += 1;
                tbNew.Text = newItems.ToString();
                context.SaveChanges();
            }
            var nextButton = browser.Document.GetElementById("pagnNextLink");
            if (nextButton == null)
            {
                MessageBox.Show("Update complete");
                rbMovies.Enabled = true;
                rbTV.Enabled = true;
                btnSearch.Enabled = true;
            }
            else
            {
                nextButton.InvokeMember("click");
                while (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                {
                    Application.DoEvents();
                }
                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            DocumentCompleted();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (cbDetails.Checked)
            {
                searhForDetails();
            }
            else
            {
                searhForNewItems();
            }
        }

        private void searhForDetails()
        {
            if (rbMovies.Checked)
            {
                moviesForDetails = context.Movie.Where(m => m.Plot == null).ToList();
                itemsToProcess = moviesForDetails.Count;
                nextURL = moviesForDetails[itemsProcessed].Url;
            }
            else
            {
                tvForDetails = context.Tvseries.Where(m => m.Plot == null).ToList();
                nextURL = tvForDetails[itemsProcessed].Url;
            }
            webBrowser1.Navigate(nextURL);
            while (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }
            timer1.Start();
        }

        private void searhForNewItems()
        {
            page = 0;
            foundItems = 0;
            newItems = 0;
            tbPage.Text = string.Empty;
            tbFound.Text = string.Empty;
            tbNew.Text = string.Empty;


            rbMovies.Enabled = false;
            rbTV.Enabled = false;
            btnSearch.Enabled = false;
            cbDownloadScope.Enabled = false;

            if (rbMovies.Checked)
            {
                switch (cbDownloadScope.SelectedIndex)
                {
                    case 0: // All
                        nextURL = "https://www.amazon.com/s/ref=sr_rot_p_n_ways_to_watch_2?fst=as%3Aoff&rh=n%3A2858778011%2Cp_85%3A2470955011%2Cn%3A2858905011%2Cp_n_ways_to_watch%3A12007865011&bbn=2858905011&ie=UTF8&qid=1514940132&rnid=12007862011&lo=none";
                        break;
                    case 1: // Last 7 days
                        nextURL = "https://www.amazon.com/s/ref=sr_nr_p_n_date_0?fst=as%3Aoff&rh=n%3A2858778011%2Cp_85%3A2470955011%2Cn%3A2858905011%2Cp_n_ways_to_watch%3A12007865011%2Cp_n_date%3A2693526011&bbn=2858905011&ie=UTF8&qid=1517092161&rnid=2693522011&lo=none";
                        break;
                    case 2: // Last 30 days
                        nextURL = "https://www.amazon.com/s/ref=sr_nr_p_n_date_1?fst=as%3Aoff&rh=n%3A2858778011%2Cp_85%3A2470955011%2Cn%3A2858905011%2Cp_n_ways_to_watch%3A12007865011%2Cp_n_date%3A2693527011&bbn=2858905011&ie=UTF8&qid=1517092222&rnid=2693522011&lo=none";
                        break;
                    case 3:
                        nextURL = "https://www.amazon.com/s/ref=sr_nr_p_n_date_2?fst=as%3Aoff&rh=n%3A2858778011%2Cp_85%3A2470955011%2Cn%3A2858905011%2Cp_n_ways_to_watch%3A12007865011%2Cp_n_date%3A2693528011&bbn=2858905011&ie=UTF8&qid=1517091778&rnid=2693522011&lo=none";
                        break;
                }
            }
            else // TV Shows
            {
                switch (cbDownloadScope.SelectedIndex)
                {
                    case 0: // All
                        nextURL = "https://www.amazon.com/s/gp/search/ref=sr_nr_p_n_entity_type_1?fst=as%3Aoff&rh=n%3A2858778011%2Cp_85%3A2470955011%2Cp_n_ways_to_watch%3A12007865011%2Cp_n_entity_type%3A14069185011&bbn=2858778011&ie=UTF8&qid=1515018457&rnid=14069183011&lo=none";
                        break;
                    case 1: // Last 7 days
                        nextURL = "https://www.amazon.com/s/ref=sr_nr_p_n_date_0?fst=as%3Aoff&rh=n%3A2858778011%2Cp_85%3A2470955011%2Cp_n_ways_to_watch%3A12007865011%2Cp_n_entity_type%3A14069185011%2Cp_n_date%3A2693526011&bbn=2858778011&ie=UTF8&qid=1517092368&rnid=2693522011&lo=none";
                        break;
                    case 2: // Last 30 days
                        nextURL = "https://www.amazon.com/s/ref=sr_nr_p_n_date_1?fst=as%3Aoff&rh=n%3A2858778011%2Cp_85%3A2470955011%2Cp_n_ways_to_watch%3A12007865011%2Cp_n_entity_type%3A14069185011%2Cp_n_date%3A2693527011&bbn=2858778011&ie=UTF8&qid=1517092411&rnid=2693522011&lo=none";
                        break;
                    case 3:
                        nextURL = "https://www.amazon.com/s/ref=sr_nr_p_n_date_2?fst=as%3Aoff&rh=n%3A2858778011%2Cp_85%3A2470955011%2Cp_n_ways_to_watch%3A12007865011%2Cp_n_entity_type%3A14069185011%2Cp_n_date%3A2693528011&bbn=2858778011&ie=UTF8&qid=1517092411&rnid=2693522011&lo=none";
                        break;
                }
            }
            webBrowser1.Navigate(nextURL);
            while (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }
            timer1.Start();
        }

        private void rbMovies_CheckedChanged(object sender, EventArgs e)
        {
            lblFound.Text = "Movies Found";
            lblNew.Text = "New Movies";
            if (cbDetails.Checked)
            {
                itemsToProcess = context.Movie.Where(m => m.Plot == null).Count();
                itemsProcessed = 0;
                lblDetailProgress.Text = $"{itemsProcessed} of {itemsToProcess}";
            }
        }

        private void rbTV_CheckedChanged(object sender, EventArgs e)
        {
            lblFound.Text = "TV Shows Found";
            lblNew.Text = "New TV Shows";
            if (cbDetails.Checked)
            {
                itemsToProcess = context.Tvseries.Where(m => m.Plot == null).Count();
                itemsProcessed = 0;
                lblDetailProgress.Text = $"{itemsProcessed} of {itemsToProcess}";
            }
        }

        private void cbDetails_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDetails.Checked)
            {
                panel2.Location = panel1.Location;
                panel1.Visible = false;
                panel2.Visible = true;
                if (rbMovies.Checked)
                {
                    itemsToProcess = context.Movie.Where(m => m.Plot == null).Count();
                    itemsProcessed = 0;
                }
                else
                {
                    itemsToProcess = context.Tvseries.Where(m => m.Plot == null).Count();
                    itemsProcessed = 0;
                }
                lblDetailProgress.Text = $"{itemsProcessed} of {itemsToProcess}";
            }
            else
            {
                panel1.Visible = true;
                panel2.Visible = false;
            }
        }

    }
}
