using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MovieCatalog.DAL;
using System.Data.Entity.Core;
using System.Data.SqlClient;

// The class implements the IDisposable interface to ensure that the database connection is released when the 
// object is disposed.

namespace MovieCatalog.BLL
{
    public class MovieCatalogBL : IDisposable
    {
        /*
         The class variable that holds a reference to the repository class is defined as an interface type, 
         * and the code that instantiates the repository class is contained in two constructors. 
         * The parameterless constructor is used by the ObjectDataSource control. It creates an instance 
         * of the MovieCatalogRepository class created earlier. 
         * The other constructor allows whatever code that instantiates the business-logic class to pass in
            any object that implements the repository interface.
         */
        private  IMovieCatalogRepository MovieRepository;

        public MovieCatalogBL()
        {
        this.MovieRepository = new MovieCatalogRepository();
        }

        public MovieCatalogBL(IMovieCatalogRepository MovieRepository)
        {
            this.MovieRepository = MovieRepository;
        }


        public IQueryable<Movie> GetMovieByID(int movieID)
        {
            try
            {
                return MovieRepository.GetMovieByID(movieID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<Movie> GetMovies()
        {
            try
            {
                return MovieRepository.GetMovies();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<Movie> GetMoviesExpiry()
        {
            try
            {
                var context = MovieRepository.GetMovies();
                context = context.Where(m => m.ExpireDate < DateTime.Now.AddDays(30)).ToList();
                return context;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertMovie(string contentProvider, string title, string genre, TimeSpan movieDuration, string country, string rightsIPTV, string rightsVOD, string svodRights, string ancillaryRights, DateTime startDate, DateTime expireDate, string comment, short year)
        {
            try
            {
                MovieRepository.InsertMovie(contentProvider, title, genre, movieDuration, country, rightsIPTV, rightsVOD, svodRights, ancillaryRights, startDate, expireDate, comment, year);
            }

            catch (UpdateException updateEx)
            {
                throw updateEx;
            }

            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }

            catch (Exception ex)
            {
                //Include catch blocks for specific exceptions first,
                //and handle or log the error as appropriate in each.
                //Include a generic catch block like this one last.

                throw ex;
            }
        }

        public void UpdateMovieByID(int movieID, string contentProvider, string title, string genre, TimeSpan movieDuration, string country, string rightsIPTV, string rightsVOD, string svodRights, string ancillaryRights, DateTime startDate, DateTime expireDate, string comment, short year)
        {
            try
            {
                MovieRepository.UpdateMovieByID(movieID, contentProvider, title, genre, movieDuration, country, rightsIPTV, rightsVOD, svodRights, ancillaryRights, startDate, expireDate, comment, year);
            }
            catch (OptimisticConcurrencyException ocex)
            {
                throw ocex;
            }

            catch (UpdateException uex)
            {
                throw uex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void DeleteMovieByID(int movieID)
        {
            try
            {
                MovieRepository.DeleteMovieByID(movieID);
            }
            catch (OptimisticConcurrencyException ocex)
            {
                throw ocex;
            }
            catch (ArgumentNullException argex)
            {
                throw argex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    MovieRepository.Dispose();
                }
            }
            this.disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}