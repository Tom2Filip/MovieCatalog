using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MovieCatalog.DAL
{
    [MetadataType(typeof(MovieMetaData))]
    public partial class Movie
    {
    }

    public class MovieMetaData
    {
        [Display(Name = "Content Provider")]
        [StringLength(30, ErrorMessage = "Content Provider must be 30 characters or less in length.")]
        public String ContentProvider { get; set; }

        // Title == OriginalName
        [Display(Name = "Movie Title")]
        [StringLength(50, ErrorMessage = "Title must be 50 characters or less in length.")]
        [Required(ErrorMessage = "Movie Title is required.")]
        public String OriginalName { get; set; }

        [Display(Name = "Genre")]
        [StringLength(20, ErrorMessage = "Genre must be 20 characters or less in length.")]
        [Required(ErrorMessage = "Genre is required.")]
        public String Genre { get; set; }

        //msdn.microsoft.com/en-us/library/system.componentmodel.dataannotations.displayformatattribute%28v=vs.110%29.aspx
        //Duration
        [Display(Name = "Movie Duration" , Description="Movie Duration has to be in HH:MM:SS format")]
        [DisplayFormat(DataFormatString = "{0:HH:MM:SS}" )]
        // Also, apply format in edit mode.
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public TimeSpan  Duration { get; set; }

        [Display(Name = "Country")]
        [StringLength(40, ErrorMessage = "Country must be 40 characters or less in length.")]
        [Required(ErrorMessage = "Country is required.")]
        public String Country { get; set; }

        [Display(Name = "IPTV Rights")]
        [StringLength(300, ErrorMessage = "IPTV Rights must be 300 characters or less in length.")]
        public string RightsIPTV { get; set; }

        [Display(Name = "VOD Rights")]
        [StringLength(300, ErrorMessage = "VOD Rights  must be 300 characters or less in length.")]
        public string RightsVOD { get; set; }

        [Display(Name = "SVOD Rights")]
        [StringLength(3, ErrorMessage = "SVOD Rights must be 3 characters or less in length.")]
        public string SVODRights { get; set; }

        [Display(Name = "Ancillary Rights")]
        [StringLength(3, ErrorMessage = "Ancillary Rights must be 3 characters or less in length.")]
        public string AncillaryRights { get; set; }
                        
        [Display(Name = "StartDate")]
        // Display date data field in the format 11/12/2008. 
        // Also, apply format in edit mode
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "ExpireDate")]
        // Display date data field in the format 11/12/2008. 
        // Also, apply format in edit mode
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ExpireDate { get; set; }

        [Display(Name = "Year")]
        [Range(1800, UInt16.MaxValue, ErrorMessage = "Year must be greater than 1800")]
        [Required(ErrorMessage = "Year is required and in YYYY format")]
        public UInt16 Year { get; set; }

        [Display(Name = "Comment")]
        [StringLength(300, ErrorMessage = "Comment must be 300 characters or less in length.")]
        public String Comment { get; set; }

    }

}