using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MovieCatalog.DAL;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Objects;

namespace MovieCatalog.DAL
{   // The class implements the IDisposable interface to ensure that the database connection is released 
    // when the object is disposed.
    public class MovieCatalogRepository : IDisposable, IMovieCatalogRepository
    {
        private MoviesDBEntities context = new MoviesDBEntities();

        // If there is "MergeOption.NoTracking" update statement doesn't work (movie can't be updated)
        /*
        public MovieCatalogRepository()
        {
            context.Movies.MergeOption = MergeOption.NoTracking;
        }                        
        */
        public IQueryable<Movie> GetMovieByID(int movieID)
        {
            try
            {
                return context.Movies.Where(m => m.Id == movieID);
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
                return context.Movies.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertMovie(string contentProvider, string title, string genre, TimeSpan movieDuration, string country, string rightsIPTV, string rightsVOD, string svodRights, string ancillaryRights, DateTime startDate, DateTime expireDate, string comment, short year)
        {
            try
            {
                Movie newMovie = new Movie();
                newMovie.ContentProvider = contentProvider;
                newMovie.OriginalName = title;
                newMovie.Genre = genre;
                newMovie.Duration = movieDuration;
                newMovie.Country = country;

                newMovie.RightsIPTV = rightsIPTV;
                newMovie.RightsVOD = rightsVOD;
                newMovie.SVODRights = svodRights;
                newMovie.AncillaryRights = ancillaryRights;

                newMovie.StartDate = startDate;
                newMovie.ExpireDate = expireDate;

                newMovie.Comment = comment;
                newMovie.Year = year;

                context.Movies.AddObject(newMovie);
                context.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            } 
        }

        public void UpdateMovieByID(int movieID, string contentProvider, string title, string genre, TimeSpan movieDuration, string country, string rightsIPTV, string rightsVOD, string svodRights, string ancillaryRights, DateTime startDate, DateTime expireDate, string comment, short year)
        {
            try
            {
                var movieToUpdate = context.Movies.Where(m => m.Id == movieID).FirstOrDefault();

                movieToUpdate.ContentProvider = contentProvider;
                movieToUpdate.OriginalName = title;
                movieToUpdate.Genre = genre;
                movieToUpdate.Duration = movieDuration;
                movieToUpdate.Country = country;

                movieToUpdate.RightsIPTV = rightsIPTV;
                movieToUpdate.RightsVOD = rightsVOD;
                movieToUpdate.SVODRights = svodRights;
                movieToUpdate.AncillaryRights = ancillaryRights;

                movieToUpdate.StartDate = startDate;
                movieToUpdate.ExpireDate = expireDate;

                movieToUpdate.Comment = comment;
                movieToUpdate.Year = year;
                                
                context.SaveChanges();
                
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

        public void DeleteMovieByID(int MovieID)
        {
            try
            {
                var movieToDelete = context.Movies.Where(m => m.Id == MovieID).First();
                context.Movies.DeleteObject(movieToDelete);
                context.SaveChanges();
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
                    context.Dispose();
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