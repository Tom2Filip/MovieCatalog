using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MovieCatalog.DAL;
using System.Globalization;
using System.Data;

namespace MovieCatalog
{
    public partial class About : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                // for Calendar control -- Year & Month DropDownLists
                LoadYears();
                LoadMonths();

                // Time DropDownList
               for (int index = 0; index < 24; index++)
               {
                DropDownList1.Items.Add(index.ToString("00"));
               }
               for (int index = 0; index < 60; index++)
               {
                DropDownList2.Items.Add(index.ToString("00"));
                DropDownList3.Items.Add(index.ToString("00"));
               }
  
                
                // populate DropDownList ddlCounries
                ddlCountries.DataSource = CountriesList();
                ddlCountries.DataBind();

                // Movies List
                MoviesDBEntities context = new MoviesDBEntities();

                try
                {
                    List<string> movieTitle = new List<string>();
                    var moviesNameList = new string[context.Movies.Count()];
                    foreach (var item in context.Movies)
                    {
                        movieTitle.Add(item.OriginalName);     
                    }
                   
                    MoviesDropDownCheckBoxes.DataSource = movieTitle;
                    MoviesDropDownCheckBoxes.DataBind();
                }
                catch (Exception)
                {
                    //lblMessage.Text = "An error occurred. Please try again.";
                }


                // Years List
                var years = new int[20];
                var currentYear = DateTime.Now.Year;

                for (int i = 0; i < years.Length; i++)
                    years[i] = currentYear--;

                yearsDropDownCheckBoxes.DataSource = years;
                yearsDropDownCheckBoxes.DataBind();

            }
        }


        protected void yearsDropDownCheckBoxes_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedItemsPanel.Controls.Clear();

            foreach (ListItem item in (sender as ListControl).Items)
            {
                if (item.Selected)
                    selectedItemsPanel.Controls.Add(new Literal() { Text = item.Text + " : " + item.Value + "<br/>" });
            }
        }

        protected void MoviesDropDownCheckBoxes_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedItemsPanel2.Controls.Clear();
            // ako nema slijedeće linije koda onda nadodaje novi text na stari - ispada duplo
            movieTextBox.Text = "";
            
            foreach (ListItem item in (sender as ListControl).Items)
            {
                if (item.Selected)
                {
                    selectedItemsPanel2.Controls.Add(new Literal() { Text = item.Text + " : " + item.Value + "<br/>" });
                    if (movieTextBox.Text.Length > 0)
                    movieTextBox.Text += ", " + item.Text;
                    else
                        movieTextBox.Text += item.Text;

                }
                
            }
        }

        protected void btnKlik_Click(object sender, EventArgs e)
        {
            string text = "Historically, ?the!. ,world of. data, , . ! and the world of objects have ?not! .been well integrated.!";
            //Convert the string into an array of words 
            string[] wordArray = text.Split(new char[] { ' ', '.', '?', '!', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries);
            lblText.Text = text + "<br/>" + "<br/>";

            for (int i = 0; i < wordArray.Length ; i++)
            {
                lblText.Text += wordArray[i] + "<br/>";    
            }
            
        }

        protected void btnCountries_Click(object sender, EventArgs e)
        {
            //ddlCountries.DataSource = CountriesList();
            //ddlCountries.DataBind();

            countriesDropDownCheckBoxes.DataSource = CountriesList();
            countriesDropDownCheckBoxes.DataBind();
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

        protected void btnCount_Click(object sender, EventArgs e)
        {
            int i = 0;
            foreach (var item in ddlCountries.Items)
            {
                i++;
            }
          lblBroj.Text = "<br/>" + "i= " + i + "<br/> Number of Countries: " + ddlCountries.Items.Count.ToString();

          string text = "Croatia, .Germany!.Ukraine.Poland?Bosnia and Herzegovina. ,France; ,Italy.Spain ,Bosna;Srbija.Italija";
          //Convert the string into an array of words 
          string[] wordArray = text.Split(new char[] { '.', '?', '!', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries);

          lblText2.Text = "Countries in List: ";
          for (int j = 0; j < wordArray.Length; j++)
          {
              lblText2.Text += wordArray[j] + ", ";
          }

          lblText3.Text = "Duplicate Countries in List and DropDownList:" + "<br/>";   
       // foreach (var item in ddlCountries.Items)
          // has to be 'ListItem' "(ListItem item in countriesDropDownCheckBoxes.Items)" -> NOT (var item in ddlCountries.Items)
              foreach (ListItem item in countriesDropDownCheckBoxes.Items)
          {
              for (int k = 0; k < wordArray.Length; k++)
              {
                  if (item.ToString().ToUpperInvariant() == wordArray[k].ToUpperInvariant().Trim())
                  {
                      item.Selected = true;
                      lblText3.Text += wordArray[k] + "<br/>";   
                  }                
              }
          }   
        
        }

        protected void btnDuration_Click(object sender, EventArgs e)
        {
            string hours = DropDownList1.SelectedValue.ToString();
            string minutes = DropDownList2.SelectedValue.ToString();
            string seconds = DropDownList3.SelectedValue.ToString();
            
            string duration1 = "Movie Duration: " + hours + ":" + minutes + ":" + seconds;
            lblDuration.Text = duration1;
        }

        // www.youtube.com/watch?v=r4I-Pqvq4rA
        // csharp-video-tutorials.blogspot.com/2013/01/navigating-to-specific-month-and-year.html
        
        private void LoadMonths()
        {
            DataSet dsMonths = new DataSet();
            dsMonths.ReadXml(Server.MapPath("~/Data/Months.xml"));

            DropDownListMonth.DataTextField = "Name";
            DropDownListMonth.DataValueField = "Number";

            DropDownListMonth.DataSource = dsMonths;
            DropDownListMonth.DataBind();
        }
        // stackoverflow.com/questions/14379898/add-next-previous-year-button-to-asp-calendar-control
        private void LoadYears()
        {
            for (int intYear = 1900; intYear <= DateTime.Now.Year; intYear++)
            {
                DropDownListYear.Items.Add(intYear.ToString());                
            }

            //Make the current year selected item in the list
            DropDownListYear.Items.FindByValue(DateTime.Now.Year.ToString()).Selected = true;
   }

        protected void DropDownListYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            int year = Convert.ToInt16(DropDownListYear.SelectedValue);
            int month = Convert.ToInt16(DropDownListMonth.SelectedValue);
           
            CalendarXX.VisibleDate = new DateTime(year, month, 1);
            CalendarXX.SelectedDate = new DateTime(year, month, 1);
        }

        protected void DropDownListMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            int year = Convert.ToInt16(DropDownListYear.SelectedValue);
            int month = Convert.ToInt16(DropDownListMonth.SelectedValue);
            CalendarXX.VisibleDate = new DateTime(year, month, 1);
            CalendarXX.SelectedDate = new DateTime(year, month, 1);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //Response.Write(Calendar1.SelectedDate.ToShortDateString());
            lblDate1.Text = CalendarXX.SelectedDate.ToShortDateString();
        }


       
    }
}
