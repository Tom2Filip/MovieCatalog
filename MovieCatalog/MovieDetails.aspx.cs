using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MovieCatalog.DAL;
using System.Web.DynamicData;
using System.Globalization;
using System.Data.Entity.Core;
using MovieCatalog.BLL;
//using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data;

namespace MovieCatalog
{
    public partial class MovieDetails : System.Web.UI.Page
    {
        public int movieID;

        protected void Page_Init(object sender, EventArgs e)
        {
            MovieDetailsView.EnableDynamicData(typeof(Movie));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // check QueryString not null OR empty - if null OR empty redirect to Previous page
            if (Request.QueryString["Id"] == null || Request.QueryString["Id"] == "")
            {
                Response.Redirect("Default.aspx");
            }

            movieID = Convert.ToInt32(Request.QueryString["Id"]);
            
            if (!IsPostBack)
            {
                MovieCatalogBL contextBL = new MovieCatalogBL();

                try
                {   
                    var queryMovieByID2 = contextBL.GetMovieByID(movieID);
                    // needs to be List
                    MovieDetailsView.DataSource = queryMovieByID2.ToList();
                    MovieDetailsView.DataBind();
                                                         
                   // find DropDownList control inside DetailsView
                   DropDownList ddlHours = (DropDownList)MovieDetailsView.FindControl("ddlHours");
                   DropDownList ddlMinutes = (DropDownList)MovieDetailsView.FindControl("ddlMinutes");
                   DropDownList ddlSeconds = (DropDownList)MovieDetailsView.FindControl("ddlSeconds");
                   // Time DropDownList
                   for (int index = 0; index < 24; index++)
                   {
                       ddlHours.Items.Add(index.ToString("00"));
                   }
                   for (int index = 0; index < 60; index++)
                   {

                       ddlMinutes.Items.Add(index.ToString("00"));
                       ddlSeconds.Items.Add(index.ToString("00"));
                   }

                   // for Calendar control -- Year & Month DropDownLists
                   LoadYears();
                   LoadMonths();
             
                   // for movie production (make) year
                   LoadProductionYears();
                   DropDownList ddlProductionYear = (DropDownList)MovieDetailsView.FindControl("DropDownListProductionYear");
                   // set year in DropDownListProductionYear
                   ddlProductionYear.Text = queryMovieByID2.FirstOrDefault().Year.ToString();
                   
                   // get Duration of movie
                   TimeSpan duration = queryMovieByID2.FirstOrDefault().Duration.Value;
                   // set the text of ddl's for hours, minutes and seconds
                   ddlHours.Text = duration.Hours.ToString("00");
                   ddlMinutes.Text = duration.Minutes.ToString("00");
                   ddlSeconds.Text = duration.Seconds.ToString("00");

                   // Fill CheckBoxLists with countries
                   CheckBoxList ddlCountry = (CheckBoxList)MovieDetailsView.FindControl("ddlCountry");
                   ddlCountry.DataSource = CountriesList();
                   ddlCountry.DataBind();
                    
                   CheckBoxList ddlCountriesIPTV = (CheckBoxList)MovieDetailsView.FindControl("ddlCountriesIPTV");
                   ddlCountriesIPTV.DataSource = CountriesList();
                   ddlCountriesIPTV.DataBind();

                   CheckBoxList ddlCountriesVOD = (CheckBoxList)MovieDetailsView.FindControl("ddlCountriesVOD");
                   ddlCountriesVOD.DataSource = CountriesList();
                   ddlCountriesVOD.DataBind();
                    

                   // select countries in ddlCountry
                   string countriesMovie = queryMovieByID2.FirstOrDefault().Country;
                   //Convert the string into an array of words 
                   string[] countryArray = countriesMovie.Split(new char[] { '?', '!', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries);
                                                        
                   // has to be 'ListItem' "(ListItem item in ddlCountry.Items)" -> NOT "var"(var item in ddlCountries.Items)
                   foreach (ListItem item in ddlCountry.Items)
                   {
                       for (int k = 0; k < countryArray.Length; k++)
                       {
                           if (item.ToString().ToUpperInvariant() == countryArray[k].ToUpperInvariant().Trim())
                           {
                               item.Selected = true;
                           }
                       }
                   }

                   // select countries in ddlIPTV
                    string countriesIPTV = queryMovieByID2.FirstOrDefault().RightsIPTV;
                   //Convert the string into an array of words 
                   string[] iptvArray = countriesIPTV.Split(new char[] { '?', '!', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries);
                   // has to be 'ListItem' "(ListItem item in ddlcountriesIPTV.Items)" -> NOT "var"(var item in ddlCountries.Items)
                   foreach (ListItem item in ddlCountriesIPTV.Items)
                   {
                       for (int k = 0; k < iptvArray.Length; k++)
                       {
                           if (item.ToString().ToUpperInvariant() == iptvArray[k].ToUpperInvariant().Trim())
                           {
                               item.Selected = true;
                           }
                       }
                   }


                   // select countries in ddlVOD
                    string countriesVOD = queryMovieByID2.FirstOrDefault().RightsVOD;
                   //Convert the string into an array of words 
                    string[] VODArray = countriesVOD.Split(new char[] { '?', '!', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries);
                   // has to be 'ListItem' "(ListItem item in ddlCountriesVOD.Items)" -> NOT "var"(var item in ddlCountriesVOD.Items)
                   foreach (ListItem item in ddlCountriesVOD.Items)
                   {
                       for (int k = 0; k < VODArray.Length; k++)
                       {
                           if (item.ToString().ToUpperInvariant() == VODArray[k].ToUpperInvariant().Trim())
                           {
                               item.Selected = true;
                           }
                       }
                   }

                   // set text/value in DropdDownCheckBox Controls: ddlAncillaryRights and ddlSVODRights
                   string svodRights = queryMovieByID2.FirstOrDefault().SVODRights.ToString();
                   string ancillaryRights = queryMovieByID2.FirstOrDefault().AncillaryRights.ToString();
                   DropDownList ddlAncillaryRights = (DropDownList)MovieDetailsView.FindControl("ddlAncillaryRights");
                   DropDownList ddlSVODRights = (DropDownList)MovieDetailsView.FindControl("ddlSVODRights");
                   ddlAncillaryRights.Text = ancillaryRights;
                   ddlSVODRights.Text = svodRights;

                   // set the date in startDateCalendar and make it visible in control
                   DateTime startDate = (DateTime)queryMovieByID2.First().StartDate;
                   System.Web.UI.WebControls.Calendar startDateCalendar = (System.Web.UI.WebControls.Calendar)MovieDetailsView.FindControl("startDateCalendar");
                   startDateCalendar.SelectedDate = startDate;
                   startDateCalendar.VisibleDate = startDate;
                   // set the Year and Month in ddlStartYear and ddlStartMonth
                   DropDownList ddlStartYear = (DropDownList)MovieDetailsView.FindControl("DropDownListYear");
                   DropDownList ddlStartMonth = (DropDownList)MovieDetailsView.FindControl("DropDownListMonth");
                   ddlStartYear.Text = startDate.Year.ToString();
                   ddlStartMonth.Text = startDate.Month.ToString();

                   
                    
                   // set the date in expiryDateCalendar and make it visible in control
                   DateTime expiryDate = (DateTime)queryMovieByID2.First().ExpireDate;
                   System.Web.UI.WebControls.Calendar expiryDateCalendar = (System.Web.UI.WebControls.Calendar)MovieDetailsView.FindControl("expireDateCalendar");
                   expiryDateCalendar.SelectedDate = expiryDate;
                   expiryDateCalendar.VisibleDate = expiryDate;
                   // set the Year and Month in ddlExpiryYear and ddlExpiryMonth
                    DropDownList ddlExpiryYear = (DropDownList)MovieDetailsView.FindControl("DropDownListExpireYear");
                   DropDownList ddlExpiryMonth = (DropDownList)MovieDetailsView.FindControl("DropDownListExpireMonth");
                   ddlExpiryYear.Text = expiryDate.Year.ToString();
                   ddlExpiryMonth.Text = expiryDate.Month.ToString();
                }
                catch (Exception)
                {
                    lblMessage.Text = "An error occurred. Please try again.";
                }
                                                               
            }
        }

        // code for StartDate
        protected void calendarImage_Click(object sender, EventArgs e)
        {   
            System.Web.UI.WebControls.Calendar cal = (System.Web.UI.WebControls.Calendar)MovieDetailsView.FindControl("startDateCalendar");
            cal.Visible = true;
        }

        protected void startDateCalendar_SelectionChanged(object sender, EventArgs e)
        {
            System.Web.UI.WebControls.Calendar calendar = (System.Web.UI.WebControls.Calendar)MovieDetailsView.FindControl("startDateCalendar");
            TextBox startDateTextBox = (TextBox)MovieDetailsView.FindControl("startDateTextBox");
            startDateTextBox.Text = calendar.SelectedDate.ToShortDateString();
            calendar.Visible = false;

            // get Year and Month on Calendar selectionChanged and change it in DropDownLists (Year and Month ddl's)
            string year1 = calendar.SelectedDate.Year.ToString();
            string month1 = calendar.SelectedDate.Month.ToString();

            DropDownList DropDownListYear = (DropDownList)MovieDetailsView.FindControl("DropDownListYear");
            DropDownList DropDownListMonth = (DropDownList)MovieDetailsView.FindControl("DropDownListMonth");

            //string year2 = DropDownListYear.Items.FindByValue(year1).ToString();
            DropDownListYear.Text = year1;
            DropDownListMonth.Text = month1;
        }

        // code for ExpireDate
        protected void calendarImage2_Click(object sender, EventArgs e)
        {
            System.Web.UI.WebControls.Calendar cal = (System.Web.UI.WebControls.Calendar)MovieDetailsView.FindControl("expireDateCalendar");
            cal.Visible = true;
        }

        protected void expireDateCalendar_SelectionChanged(object sender, EventArgs e)
        {
            System.Web.UI.WebControls.Calendar calendar = (System.Web.UI.WebControls.Calendar)MovieDetailsView.FindControl("expireDateCalendar");
            TextBox expireDateTextBox = (TextBox)MovieDetailsView.FindControl("expireDateTextBox");
            expireDateTextBox.Text = calendar.SelectedDate.ToShortDateString();
            calendar.Visible = false;

            // get Year and Month on Calendar selectionChanged and change it in DropDownLists (Year and Month ddl's)
            string year2 = calendar.SelectedDate.Year.ToString();
            string month2 = calendar.SelectedDate.Month.ToString();

            DropDownList DropDownListExpYear = (DropDownList)MovieDetailsView.FindControl("DropDownListExpireYear");
            DropDownList DropDownListExpMonth = (DropDownList)MovieDetailsView.FindControl("DropDownListExpireMonth");

            //string year = DropDownListYear.Items.FindByValue(year2).ToString();
            DropDownListExpYear.Text = year2;
            DropDownListExpMonth.Text = month2;
        }

        protected void MovieDetailsView_ModeChanging(object sender, DetailsViewModeEventArgs e)
        {
            if (e.CancelingEdit)
            {
                Response.Redirect("~/Default.aspx");
            }
        }

        protected void DurationChange(object sender, EventArgs e)
        {
            DropDownList ddlHours = (DropDownList)MovieDetailsView.FindControl("ddlHours");
            DropDownList ddlMinutes = (DropDownList)MovieDetailsView.FindControl("ddlMinutes");
            DropDownList ddlSeconds = (DropDownList)MovieDetailsView.FindControl("ddlSeconds");

            string hours = ddlHours.SelectedValue.ToString();
            string minutes = ddlMinutes.SelectedValue.ToString();
            string seconds = ddlSeconds.SelectedValue.ToString();

            TextBox durationTextBox = (TextBox)MovieDetailsView.FindControl("txtBoxDuration");
            durationTextBox.Text = hours + ":" + minutes + ":" + seconds;
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBoxList ddlCountry = (CheckBoxList)MovieDetailsView.FindControl("ddlCountry");
            TextBox txtBoxCountry = (TextBox)MovieDetailsView.FindControl("txtBoxCountry");

            txtBoxCountry.Text = "";
            // for counting selected items
            int i = 0;
            foreach (ListItem item in (sender as ListControl).Items)
            {
                if (item.Selected)
                {
                    i++;
                    txtBoxCountry.Text += item.Text + ", ";
                    // Label1.Text += ddlCountries2.SelectedItem.Value + ", ";
                }

            }
            // "i" indicates if any item is selected, without "if" statement exception is thrown on button OK click when no item is selected
            if (i > 0)
            {
                // get the index of last occurence of coma "," in a txtBoxVODRights string
                int indexLastComa = txtBoxCountry.Text.LastIndexOf(",");
                // Remove last coma "," in a txtBoxVODRights string
                string newtextVOD = txtBoxCountry.Text.ToString().Remove(indexLastComa);
                //newtextVOD =  newtextVOD.Remove(indexLastComa);
                txtBoxCountry.Text = newtextVOD;
            }

        }

        public static List<string> CountriesList()
        {
            // creating List
            List<string> countriesList = new List<string>();
            // getting the specific CultureInfo from CultureInfo class
            CultureInfo[] getCultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (CultureInfo culture in getCultureInfo)
            {
                // creating the object of RegionInfo class
                RegionInfo regionInfo = new RegionInfo(culture.LCID);
                // adding each country name into the array list
                if (!(countriesList.Contains(regionInfo.EnglishName)))
                    countriesList.Add(regionInfo.EnglishName);
            }

            // sort the list of countries
            countriesList.Sort();

            return countriesList;
        }

        protected void DropDownListProductionYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList DropDownListProdYear = (DropDownList)MovieDetailsView.FindControl("DropDownListProductionYear");
            TextBox productionYearTextBox = (TextBox)MovieDetailsView.FindControl("productionYearTextBox");

            string movieProductionYear = DropDownListProdYear.SelectedValue;
            productionYearTextBox.Text = movieProductionYear;
        }

        private void LoadProductionYears()
        {
            DropDownList DropDownListProdYear = (DropDownList)MovieDetailsView.FindControl("DropDownListProductionYear");

            for (int intYear = 1900; intYear <= DateTime.Now.Year; intYear++)
            {
                DropDownListProdYear.Items.Add(intYear.ToString());
            }

            //Make the current year selected item in the list
            DropDownListProdYear.Items.FindByValue(DateTime.Now.Year.ToString()).Selected = true;
        }

        protected void ddlCountriesIPTV_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ddlCountriesIPTV_SelectedIndexChanged -= new System.EventHandler(this.ddlCountriesIPTV_SelectedIndexChanged);
            CheckBoxList ddlCountriesIPTV = (CheckBoxList)MovieDetailsView.FindControl("ddlCountriesIPTV");
            TextBox txtBoxIPTVRights = (TextBox)MovieDetailsView.FindControl("txtBoxIPTVRights");

           txtBoxIPTVRights.Text = "";
           
            // for counting selected items
            int i = 0;

            foreach (ListItem item in (sender as ListControl).Items)
            {
                if (item.Selected)
                {
                    i++;
                    txtBoxIPTVRights.Text += item.Text + ", ";
                    // Label1.Text += ddlCountries2.SelectedItem.Value + ", ";
                }

            }

            // "i" indicates if any item is selected, without "if" statement exception is thrown on button OK click when no item is selected
            if (i > 0)
            {
                // get the index of last occurence of coma "," in a txtBoxIPTVRights string
                int indexLastComa = txtBoxIPTVRights.Text.LastIndexOf(",");
                // Remove last coma "," in a txtBoxIPTVRights string
                string newtextIPTV = txtBoxIPTVRights.Text.ToString().Remove(indexLastComa);
                txtBoxIPTVRights.Text = newtextIPTV;
             }

        }

        protected void ddlCountriesVOD_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ddlCountriesIPTV_SelectedIndexChanged -= new System.EventHandler(this.ddlCountriesIPTV_SelectedIndexChanged);
            CheckBoxList ddlCountriesVOD = (CheckBoxList)MovieDetailsView.FindControl("ddlCountriesVOD");
            TextBox txtBoxVODRights = (TextBox)MovieDetailsView.FindControl("txtBoxVODRights");

            txtBoxVODRights.Text = "";
           
            // for counting selected items
            int i = 0;

            foreach (ListItem item in (sender as ListControl).Items)
            {
                if (item.Selected)
                {
                    i++;
                    txtBoxVODRights.Text += item.Text + ", ";
                 }

            }

            // "i" indicates if any item is selected, without "if" statement exception is thrown on button OK click when no item is selected
            if (i > 0)
            {
                // get the index of last occurence of coma "," in a txtBoxIPTVRights string
                int indexLastComa = txtBoxVODRights.Text.LastIndexOf(",");
                // Remove last coma "," in a txtBoxIPTVRights string
                string newtextVOD = txtBoxVODRights.Text.ToString().Remove(indexLastComa);
                txtBoxVODRights.Text = newtextVOD;
             }

        }

        protected void ddlSVODRights_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl2 = (DropDownList)MovieDetailsView.FindControl("ddlSVODRights");
            TextBox SVODRightsTextBox = (TextBox)MovieDetailsView.FindControl("txtBoxSVODRights");
            SVODRightsTextBox.Text = ddl2.SelectedItem.Text.ToString();
        }

        protected void ddlAncillaryRights_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)MovieDetailsView.FindControl("ddlAncillaryRights");
            TextBox ancRightsTextBox = (TextBox)MovieDetailsView.FindControl("txtBoxAncillaryRights");
            ancRightsTextBox.Text = ddl.SelectedItem.Text.ToString();
        }

        private void LoadMonths()
        {
            DataSet dsMonths = new DataSet();
            dsMonths.ReadXml(Server.MapPath("~/Data/Months.xml"));

            DropDownList DropDownListMonth = (DropDownList)MovieDetailsView.FindControl("DropDownListMonth");
            DropDownList DropDownListExpMonth = (DropDownList)MovieDetailsView.FindControl("DropDownListExpireMonth");

            DropDownListMonth.DataTextField = "Name";
            DropDownListMonth.DataValueField = "Number";
            DropDownListExpMonth.DataTextField = "Name";
            DropDownListExpMonth.DataValueField = "Number";

            DropDownListMonth.DataSource = dsMonths;
            DropDownListExpMonth.DataSource = dsMonths;
            DropDownListMonth.DataBind();
            DropDownListExpMonth.DataBind();
        }
        
        private void LoadYears()
        {
            DropDownList DropDownListYear = (DropDownList)MovieDetailsView.FindControl("DropDownListYear");
            DropDownList DropDownListExpYear = (DropDownList)MovieDetailsView.FindControl("DropDownListExpireYear");
            for (int intYear = 1900; intYear <= 2100; intYear++)
            {
                DropDownListYear.Items.Add(intYear.ToString());
                DropDownListExpYear.Items.Add(intYear.ToString());
            }

            //Make the current year selected item in the list
            DropDownListYear.Items.FindByValue(DateTime.Now.Year.ToString()).Selected = true;
            DropDownListExpYear.Items.FindByValue(DateTime.Now.Year.ToString()).Selected = true;
        }

        protected void DropDownListYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList DropDownListYear = (DropDownList)MovieDetailsView.FindControl("DropDownListYear");
            DropDownList DropDownListMonth = (DropDownList)MovieDetailsView.FindControl("DropDownListMonth");

            int year = Convert.ToInt16(DropDownListYear.SelectedValue);
            int month = Convert.ToInt16(DropDownListMonth.SelectedValue);

            System.Web.UI.WebControls.Calendar startDateCalendar = (System.Web.UI.WebControls.Calendar)MovieDetailsView.FindControl("startDateCalendar");
            startDateCalendar.VisibleDate = new DateTime(year, month, 1);
            startDateCalendar.SelectedDate = new DateTime(year, month, 1);

            // show selected Year in TextBox - startDateTextBox
            startDateCalendar_SelectionChanged(sender, e);
        }

        protected void DropDownListMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList DropDownListYear = (DropDownList)MovieDetailsView.FindControl("DropDownListYear");
            DropDownList DropDownListMonth = (DropDownList)MovieDetailsView.FindControl("DropDownListMonth");

            int year = Convert.ToInt16(DropDownListYear.SelectedValue);
            int month = Convert.ToInt16(DropDownListMonth.SelectedValue);

            System.Web.UI.WebControls.Calendar startDateCalendar = (System.Web.UI.WebControls.Calendar)MovieDetailsView.FindControl("startDateCalendar");
            startDateCalendar.VisibleDate = new DateTime(year, month, 1);
            startDateCalendar.SelectedDate = new DateTime(year, month, 1);

            // show selected Month in TextBox - startDateTextBox
            startDateCalendar_SelectionChanged(sender, e);
        }

        protected void DropDownListExpireYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList DropDownListExpYear = (DropDownList)MovieDetailsView.FindControl("DropDownListExpireYear");
            DropDownList DropDownListExpMonth = (DropDownList)MovieDetailsView.FindControl("DropDownListExpireMonth");

            int year = Convert.ToInt16(DropDownListExpYear.SelectedValue);
            int month = Convert.ToInt16(DropDownListExpMonth.SelectedValue);

            System.Web.UI.WebControls.Calendar expDateCalendar = (System.Web.UI.WebControls.Calendar)MovieDetailsView.FindControl("expireDateCalendar");
            expDateCalendar.VisibleDate = new DateTime(year, month, 1);
            expDateCalendar.SelectedDate = new DateTime(year, month, 1);

            // show selected Year in TextBox - expireDateTextBox
            expireDateCalendar_SelectionChanged(sender, e);
        }
        
        protected void DropDownListExpireMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList DropDownListExpYear = (DropDownList)MovieDetailsView.FindControl("DropDownListExpireYear");
            DropDownList DropDownListExpMonth = (DropDownList)MovieDetailsView.FindControl("DropDownListExpireMonth");

            int year = Convert.ToInt16(DropDownListExpYear.SelectedValue);
            int month = Convert.ToInt16(DropDownListExpMonth.SelectedValue);

            System.Web.UI.WebControls.Calendar expDateCalendar = (System.Web.UI.WebControls.Calendar)MovieDetailsView.FindControl("expireDateCalendar");
            expDateCalendar.VisibleDate = new DateTime(year, month, 1);
            expDateCalendar.SelectedDate = new DateTime(year, month, 1);

            // show selected Month in TextBox - startDateTextBox
            expireDateCalendar_SelectionChanged(sender, e);
        }


        protected void MovieDetailsView_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            try
            {
                MovieCatalogBL contextBL = new MovieCatalogBL();
                var movieToUpdate = contextBL.GetMovieByID(movieID);

                string contentProvider = null;
                if (e.NewValues["ContentProvider"] == null || e.NewValues["ContentProvider"].ToString() == "")
                {
                    contentProvider = "";
                }
                else
                {
                    contentProvider = e.NewValues["ContentProvider"].ToString();
                }

                string title = e.NewValues["OriginalName"].ToString();
                string genre = e.NewValues["Genre"].ToString();

                //msdn.microsoft.com/en-us/library/system.timespan%28v=vs.110%29.aspx
                //string value = "12:12:15";
                //TimeSpan ts = TimeSpan.Parse(value);
                TimeSpan movieDuration = TimeSpan.Zero;

                // find DropDownList control inside DetailsView
                DropDownList ddlHours = (DropDownList)MovieDetailsView.FindControl("ddlHours");
                DropDownList ddlMinutes = (DropDownList)MovieDetailsView.FindControl("ddlMinutes");
                DropDownList ddlSeconds = (DropDownList)MovieDetailsView.FindControl("ddlSeconds");

                string hours = ddlHours.SelectedValue.ToString().Trim();
                string minutes = ddlMinutes.SelectedValue.ToString().Trim();
                string seconds = ddlSeconds.SelectedValue.ToString().Trim();

                //string value = "12:12:15";
                //TimeSpan ts = TimeSpan.Parse(value);
                string duration1 = hours + ":" + minutes + ":" + seconds;
                movieDuration = TimeSpan.Parse(duration1);


                string country = e.NewValues["Country"].ToString();

                string rightsIPTV = null;
                if (e.NewValues["RightsIPTV"] == null || e.NewValues["RightsIPTV"].ToString() == "")
                {
                    rightsIPTV = string.Empty;
                }
                else
                {
                    rightsIPTV = e.NewValues["RightsIPTV"].ToString();
                }

                string rightsVOD = null;
                if (e.NewValues["RightsVOD"] == null || e.NewValues["RightsVOD"].ToString() == "")
                {
                    rightsVOD = string.Empty;
                }
                else
                {
                    rightsVOD = e.NewValues["RightsVOD"].ToString();
                }

                string svodRights = null;
                if (e.NewValues["SVODRights"] == null || e.NewValues["SVODRights"].ToString() == "")
                {
                    svodRights = string.Empty;
                }
                else
                {
                    svodRights = e.NewValues["SVODRights"].ToString();
                }

                string ancillaryRights = null;
                if (e.NewValues["AncillaryRights"] == null || e.NewValues["AncillaryRights"].ToString() == "")
                {
                    ancillaryRights = string.Empty;
                }
                else
                {
                    ancillaryRights = e.NewValues["AncillaryRights"].ToString();
                }


                DateTime startDate = Convert.ToDateTime(e.NewValues["StartDate"]);
                DateTime expireDate = Convert.ToDateTime(e.NewValues["ExpireDate"]);
                int result = DateTime.Compare(startDate, expireDate);
                string relationship = "Expiry Date is earlier than Start Date !";
                //msdn.microsoft.com/en-us/library/5ata5aya%28v=vs.110%29.aspx
                // if startDate is later than expireDate result is (1) greater than 0 (zero)
                if (result > 0)
                {
                    // instead of "e.Cancel = true;" can be -> ((DetailsViewInsertEventArgs)e).Cancel = true;
                    // www.noordam.it/validating-detailsview-field-NewValues-during-insert-and-update/
                    // has to be 'return' after 'e.Cancel = true;'  -> at least in some cases
                    e.Cancel = true;
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + relationship + "');", true);
                    return;
                }


                string comment = null;
                if (e.NewValues["Comment"] == null || e.NewValues["Comment"].ToString() == "")
                {
                    comment = string.Empty;
                }
                else
                {
                    comment = e.NewValues["Comment"].ToString();
                }

                Int16 year = Convert.ToInt16(e.NewValues["Year"]);

                contextBL.UpdateMovieByID(movieID, contentProvider, title, genre, movieDuration, country, rightsIPTV, rightsVOD, svodRights, ancillaryRights, startDate, expireDate, comment, year);
                lblMessage.ForeColor = System.Drawing.Color.Black;
                lblMessage.Text = "Movie " + title + " updated.";
             }
            catch (DbUpdateException)
            {
                lblMessage.Text = "Update Exception. Error while updating movie data. Please try again.";
            } 
            catch(NullReferenceException)
            {
                lblMessage.Text = "An error occurred while updating movie. Make sure that movie exists.";
            }
            catch(ArgumentNullException)
            {
                lblMessage.Text = "ArgumentNullException: An error occurred while updating movie. Make sure that movie exists.";
            }
            catch (Exception)
            {
                lblMessage.Text = "An error occurred while updating movie. Please try again.";
            }
                        
        }


    }
} 