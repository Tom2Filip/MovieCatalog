using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieCatalog.DAL
{
    public interface IMovieCatalogRepository : IDisposable
    {
        IQueryable<Movie> GetMovieByID(int movieID);
        IList<Movie> GetMovies();
        void InsertMovie(string contentProvider, string title, string genre, TimeSpan movieDuration, string country, string rightsIPTV, string rightsVOD, string svodRights, string ancillaryRights, DateTime startDate, DateTime expireDate, string comment, short year);
        void UpdateMovieByID(int movieID, string contentProvider, string title, string genre, TimeSpan movieDuration, string country, string rightsIPTV, string rightsVOD, string svodRights, string ancillaryRights, DateTime startDate, DateTime expireDate, string comment, short year);
        void DeleteMovieByID(int movieID);        
    }
}