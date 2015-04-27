using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MovieCatalog.DAL;
using MovieCatalog.BLL;
using System.Globalization;
using System.Data;

namespace MovieCatalog
{
    public partial class MovieAdd : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            MovieDetailsView.EnableDynamicData(typeof(Movie));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // for Calendar control -- Year & Month DropDownLists
                LoadYears();
                LoadMonths();
                // for movie production (make) year
                LoadProductionYears();

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


                CheckBoxList ddlCountry = (CheckBoxList)MovieDetailsView.FindControl("ddlCountry");
                ddlCountry.DataSource = CountriesList();
                ddlCountry.DataBind();

                CheckBoxList ddlCountriesIPTV = (CheckBoxList)MovieDetailsView.FindControl("ddlCountriesIPTV");             
                ddlCountriesIPTV.DataSource = CountriesList();
                ddlCountriesIPTV.DataBind();

                CheckBoxList ddlCountriesVOD = (CheckBoxList)MovieDetailsView.FindControl("ddlCountriesVOD");                             
                ddlCountriesVOD.DataSource = CountriesList();
                ddlCountriesVOD.DataBind();

            }
        }

        protected void MovieDetailsView_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {      
            try
            {
                //MoviesDBEntities context = new MoviesDBEntities();
                //Movie newMovie = new Movie();
                 MovieCatalogRepository context2 = new MovieCatalogRepository();
                 MovieCatalogBL contextBL = new MovieCatalogBL();
                
                 

                string contentProvider = null;
                if (e.Values["ContentProvider"] == null || e.Values["ContentProvider"].ToString() == "")
                {
                    contentProvider = "";
                }
                else
                {
                    contentProvider = e.Values["ContentProvider"].ToString();
                }
                
                string title = e.Values["OriginalName"].ToString();
                string genre = e.Values["Genre"].ToString();

                //msdn.microsoft.com/en-us/library/system.timespan%28v=vs.110%29.aspx
                //string value = "12:12:15";
                //TimeSpan ts = TimeSpan.Parse(value);
                TimeSpan movieDuration = TimeSpan.Zero;

               /*
                if (e.Values["Duration"] == null || e.Values["Duration"].ToString().Trim() == "")
                {
                    //movieDuration = TimeSpan.Zero;
                }
                else
                {
                    /*
                    // gets the value of "Duration" field and converts it to string
                    string durationString = e.Values["Duration"].ToString().Trim();
                    // converts 'durationString' to TimeSpan
                    movieDuration = TimeSpan.Parse(durationString);
                    */
                /*
                    // find DropDownList control inside DetailsView
                    DropDownList ddlHours = (DropDownList)MovieDetailsView.FindControl("ddlHours");
                    DropDownList ddlMinutes = (DropDownList)MovieDetailsView.FindControl("ddlMinutes");
                    DropDownList ddlSeconds = (DropDownList)MovieDetailsView.FindControl("ddlSeconds");

                    string hours = ddlHours.SelectedValue.ToString();
                    string minutes = ddlMinutes.SelectedValue.ToString();
                    string seconds = ddlSeconds.SelectedValue.ToString();

                    string duration1 = "Movie Duration: " + hours + ":" + minutes + ":" + seconds;
                    movieDuration = TimeSpan.Parse(duration1);
                                       
                 }
                */
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
               

                string country = e.Values["Country"].ToString();

                string rightsIPTV = null;
                if (e.Values["RightsIPTV"] == null || e.Values["RightsIPTV"].ToString() == "")
                {
                    rightsIPTV = string.Empty;
                }
                else
                {
                    rightsIPTV = e.Values["RightsIPTV"].ToString();
                }

                string rightsVOD = null;
                if (e.Values["RightsVOD"] == null || e.Values["RightsVOD"].ToString() == "")
                {
                    rightsVOD = string.Empty;
                }
                else
                {
                    rightsVOD = e.Values["RightsVOD"].ToString();
                }

                string svodRights = null;
                if (e.Values["SVODRights"] == null || e.Values["SVODRights"].ToString() == "")
                {
                    svodRights = string.Empty;
                }
                else
                {
                    svodRights = e.Values["SVODRights"].ToString();
                }

                string ancillaryRights = null;
                if (e.Values["AncillaryRights"] == null || e.Values["AncillaryRights"].ToString() == "")
                {
                    ancillaryRights = string.Empty;
                }
                else
                {
                    ancillaryRights = e.Values["AncillaryRights"].ToString();
                }

                
                DateTime startDate = Convert.ToDateTime(e.Values["StartDate"]);
                DateTime expireDate = Convert.ToDateTime(e.Values["ExpireDate"]);
                int result = DateTime.Compare(startDate, expireDate);
                string relationship = "Expiry Date is earlier than Start Date !";
                //msdn.microsoft.com/en-us/library/5ata5aya%28v=vs.110%29.aspx
                // if startDate is later than expireDate result is (1) greater than 0 (zero)
                if (result > 0)
                {
                    // instead of "e.Cancel = true;" can be -> ((DetailsViewInsertEventArgs)e).Cancel = true;
                    // www.noordam.it/validating-detailsview-field-values-during-insert-and-update/
                    // has to be 'return' after 'e.Cancel = true;'  -> at least in some cases
                    e.Cancel = true;
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + relationship + "');", true);
                    return;
                }
                           
                     
                string comment = null;
                if (e.Values["Comment"] == null || e.Values["Comment"].ToString() == "")
                {
                    comment = string.Empty; 
                }
                else
                {
                    comment = e.Values["Comment"].ToString();
                }
                
                Int16 year = Convert.ToInt16(e.Values["Year"]);

                /*
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
                */
                //context.Movies.AddObject(newMovie);
                // context.SaveChanges();
                
                contextBL.InsertMovie(contentProvider, title, genre, movieDuration, country, rightsIPTV, rightsVOD, svodRights, ancillaryRights, startDate, expireDate, comment, year);
                lblMessage.ForeColor = System.Drawing.Color.Black;
                lblMessage.Text = "Movie" + title + " added.";
                //contextBL.InsertMovie(contentProvider, title, genre, movieDuration, country, rightsIPTV, rightsVOD, svodRights, ancillaryRights, startDate, expireDate, comment, year);
                
            }
            // If there's a need to check if the same movie already exists in database
            //msdn.microsoft.com/en-us/library/system.data.objects.objectcontext.addobject%28v=vs.110%29.aspx
            /*
            catch (UpdateException ex)
            {
                throw new InvalidOperationException(string.Format(
                    "The object could not be added. Make sure that a "
                    + "product with a product number '{0}' does not aleady exist.\n",
                    newProduct.ProductNumber), ex);
            } 
            */
            catch(InvalidOperationException)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Error while adding new movie. Invalid data provided. Please try again.";
            }
            catch (Exception)
            {
               lblMessage.ForeColor = System.Drawing.Color.Red;
               lblMessage.Text = "Error while adding new movie. Please try again.";
            }
                        
        }


        protected void MovieDetailsView_ModeChanging(object sender, DetailsViewModeEventArgs e)
        {
            if (e.CancelingEdit)
            {
                Response.Redirect("~/Default.aspx");
            }
        }

        protected void btnDate_Click(object sender, EventArgs e)
        {
            DateTime dt;
            //if (DateTime.TryParse(Textbox1.Text, out dt))
                if (DateTime.TryParseExact( Textbox1.Text.Trim(), "d.M.yyyy", CultureInfo.CreateSpecificCulture("hr-HR"), DateTimeStyles.AllowWhiteSpaces, out dt))
            {
                Label1.Text = "Valid date format";
            }
            else
            {
                Label1.Text = "Invalid date format";
            }
               string relationship = "Expire Date is earlier than Start Date";
               ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + relationship + "');", true);
        }


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

        protected void ddlAncillaryRights_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)MovieDetailsView.FindControl("ddlAncillaryRights");
            TextBox ancRightsTextBox = (TextBox)MovieDetailsView.FindControl("txtBoxAncillaryRights");
            ancRightsTextBox.Text = ddl.SelectedItem.Text.ToString();
            Label1.Text = ddl.SelectedItem.Text.ToString();         
        }

        protected void ddlSVODRights_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl2 = (DropDownList)MovieDetailsView.FindControl("ddlSVODRights");
            TextBox SVODRightsTextBox = (TextBox)MovieDetailsView.FindControl("txtBoxSVODRights");
            SVODRightsTextBox.Text = ddl2.SelectedItem.Text.ToString();
            Label1.Text = ddl2.SelectedItem.Text.ToString();
        }


        public static List<string> CountriesList()
        {
            // creating List
            List<string> countriesList = new List<string>();
            // getting the specific CultureInfo from CultureInfo class
            CultureInfo[] getCultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (CultureInfo culture in getCultureInfo)
            {
                // cretaing the object of RegionInfo class
                RegionInfo regionInfo = new RegionInfo(culture.LCID);
                // adding each country name into the array list
                if (!(countriesList.Contains(regionInfo.EnglishName)))
                    countriesList.Add(regionInfo.EnglishName);
            }

            // sort the list of countries
            countriesList.Sort();

            return countriesList;
        }


        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            // this event (ddlCountry_SelectedIndexChanged) occurs twice - probably because this Control - DropDownCheckBoxes - consists of two controls (DropDownList and CheckBoxList)
            //ddlCountry_SelectedIndexChanged -= new System.EventHandler(this.ddlCountry_SelectedIndexChanged);

            CheckBoxList ddlCountry = (CheckBoxList)MovieDetailsView.FindControl("ddlCountry");
            TextBox txtBoxCountry = (TextBox)MovieDetailsView.FindControl("txtBoxCountry");

            // double Postback! -> wrights countries twice inside TextBox if there's no next line of code
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
                Label1.Text = newtextVOD;    
            }
            
        }


        protected void ddlCountriesIPTV_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ddlCountriesIPTV_SelectedIndexChanged -= new System.EventHandler(this.ddlCountriesIPTV_SelectedIndexChanged);
            CheckBoxList ddlCountriesIPTV = (CheckBoxList)MovieDetailsView.FindControl("ddlCountriesIPTV");
            TextBox txtBoxIPTVRights = (TextBox)MovieDetailsView.FindControl("txtBoxIPTVRights");

            // double Postback! -> wrights countries twice inside TextBox if there's no next line of code
            txtBoxIPTVRights.Text = "";
            // txtBoxVODRights.Text = ddlCountries2.SelectedItem.Text.ToString();
            //Label1.Text = ddlCountries2.SelectedItem.Text.ToString();

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
                Label1.Text = newtextIPTV;    
            }
            
        }


        protected void ddlCountriesVOD_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ddlCountriesIPTV_SelectedIndexChanged -= new System.EventHandler(this.ddlCountriesIPTV_SelectedIndexChanged);
            CheckBoxList ddlCountriesVOD = (CheckBoxList)MovieDetailsView.FindControl("ddlCountriesVOD");
            TextBox txtBoxVODRights = (TextBox)MovieDetailsView.FindControl("txtBoxVODRights");

            // double Postback! -> wrights countries twice inside TextBox if there's no next line of code
            txtBoxVODRights.Text = "";
            // txtBoxVODRights.Text = ddlCountries2.SelectedItem.Text.ToString();
            //Label1.Text = ddlCountries2.SelectedItem.Text.ToString();

            // for counting selected items
            int i = 0;

            foreach (ListItem item in (sender as ListControl).Items)
            {
                if (item.Selected)
                {
                    i++;
                    txtBoxVODRights.Text += item.Text + ", ";
                    // Label1.Text += ddlCountries2.SelectedItem.Value + ", ";
                }

            }

            // "i" indicates if any item is selected, without "if" statement exception is thrown on button OK click when no item is selected
            if (i > 0)
            {
                // get the index of last occurence of coma "," in a txtBoxIPTVRights string
                int indexLastComa = txtBoxVODRights.Text.LastIndexOf(",");
                // Remove last coma "," in a txtBoxIPTVRights string
                string newtextIPTV = txtBoxVODRights.Text.ToString().Remove(indexLastComa);
                txtBoxVODRights.Text = newtextIPTV;
                Label1.Text = newtextIPTV;    
            }
            
        }

        

        // www.youtube.com/watch?v=r4I-Pqvq4rA
        // csharp-video-tutorials.blogspot.com/2013/01/navigating-to-specific-month-and-year.html

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
        // stackoverflow.com/questions/14379898/add-next-previous-year-button-to-asp-calendar-control
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
            startDateCalendar_SelectionChanged(sender , e);
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

            // Case: if selected Day in Month is 30 or 31 - if month is changed to February then exception will be thrown - System.ArgumentOutOfRangeException
            // System.ArgumentOutOfRangeException: Year, Month, and Day parameters describe an un-representable DateTime.
            /*
            startDateCalendar.VisibleDate = new DateTime(year, month, startDateCalendar.SelectedDate.Day);
            startDateCalendar.SelectedDate = new DateTime(year, month, startDateCalendar.SelectedDate.Day);
            */
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

            // Case: if selected Day in Month is 30 or 31 - if month is changed to February then exception will be thrown - System.ArgumentOutOfRangeException
            // System.ArgumentOutOfRangeException: Year, Month, and Day parameters describe an un-representable DateTime.
            /*
            startDateCalendar.VisibleDate = new DateTime(year, month, startDateCalendar.SelectedDate.Day);
            startDateCalendar.SelectedDate = new DateTime(year, month, startDateCalendar.SelectedDate.Day);
            */
            // show selected Month in TextBox - startDateTextBox
            expireDateCalendar_SelectionChanged(sender, e);
        }


        protected void DropDownListProductionYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList DropDownListProdYear = (DropDownList)MovieDetailsView.FindControl("DropDownListProductionYear");
            TextBox productionYearTextBox = (TextBox)MovieDetailsView.FindControl("productionYearTextBox");

            string movieProductionYear = DropDownListProdYear.SelectedValue;
            productionYearTextBox.Text = movieProductionYear;                     
        }
        

    }
}