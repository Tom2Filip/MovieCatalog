using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MovieCatalog.DAL;
using MovieCatalog.BLL;
using System.Data;

using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;

using System.IO;
//www.microsoft.com/en-us/download/confirmation.aspx?id=20923
using Excel = Microsoft.Office.Interop.Excel;
using ExcelAutoFormat = Microsoft.Office.Interop.Excel.XlRangeAutoFormat;

using System.Reflection;

namespace MovieCatalog
{
    public partial class _Default : System.Web.UI.Page
    {
        // counter for selected items in filterMoviesCheckBoxList
        public int selectedItems = 0;

        protected void Page_Init(object sender, EventArgs e)
        {
            GridView1.EnableDynamicData(typeof(Movie));
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
          
            if (!IsPostBack)
            {
                BindGridView1();
            }
        }

        protected void FilterButton_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            // if any Item is selected
            if (filterMoviesCheckBoxList.SelectedIndex > -1)
            {
                selectedItems = 0;
                foreach (ListItem item in filterMoviesCheckBoxList.Items)
                {
                    if (item.Selected == true)
                    {
                     selectedItems++;
                     lblMessage.Text += "<br/>" + "Filtered by: " + item.ToString() + ", Value: " + item.Value;
                    }                  
                }
                BindFilteredMovies();
            }
            // if no Item is selected
            else
            {
                BindFilteredMovies();
                lblMessage.Text = "Filtered by: None";
            }            
        }
         
        // try catch block for exception handling is added where "FilterMovies()" method is called
       protected List<Movie> FilterMovies()
       {
               MovieCatalogBL context1 = new MovieCatalogBL();

               // if both Items are selected
               if (selectedItems == 2)
               { // select only those movies which have SVODRights and AncillaryRights set to "Yes"
                   // var query = context2.Movies.Where(m => m.SVODRights == "Yes" && m.AncillaryRights == "Yes").ToList();
                   var query1 = context1.GetMovies().Where(m => m.SVODRights == "Yes" && m.AncillaryRights == "Yes").ToList();
                   return query1;
               }
                       	       
		// if only one (first) Item is selected
               else if (filterMoviesCheckBoxList.SelectedIndex == 0)
               { // if index is 0 then only first rights (SVOD), else only second rights (Ancillary)                 
                   var query1 = context1.GetMovies().Where(m => m.SVODRights == "Yes").ToList();
                   return query1;
               }
               else // try this - (filterMoviesCheckBoxList.SelectedIndex == 1) -> NO!
               { // if index NOT 0 then only second rights (Ancillary) - (if index NOT 0 then it's 1, because there is only 2 items in list)
                   var query1 = context1.GetMovies().Where(m => m.AncillaryRights == "Yes").ToList();
                   return query1;
               }                                                       
       }

       protected void BindFilteredMovies()
       {    // if no Item selected then get all movies
           if (filterMoviesCheckBoxList.SelectedIndex == -1)
           {
               try
               {
                   BindGridView1();
               }
               catch (Exception)
               {
                   lblMessage.ForeColor = System.Drawing.Color.Red;
                   lblMessage.Text = "An error occurred while retrieving movies. Try again.";
               }               
           }
           else
           {
               try
               {  
                   GridView1.DataSource = FilterMovies();
                   GridView1.DataBind();                  
               }
               catch (Exception)
               {
                   lblMessage.ForeColor = System.Drawing.Color.Red;
                   lblMessage.Text = "An error occurred while retrieving movies. Try again.";    
               }               
           }           
       }
       
       private void BindGridView1()
       {
           //try - START
           try
           {
               // if "sortDirection" is not set then on first PageLoad there will be error
               string sortDirection = "ASC";
               string sortExpression = "";

               MovieCatalogBL context = new MovieCatalogBL();
               var query = context.GetMovies();
                
               // if there is no filter - get all movies
               if (filterMoviesCheckBoxList.SelectedIndex == -1)
               {
                   query = context.GetMovies();
                   if (ViewState["SortDirection"] != null)
                   {
                       sortDirection = ViewState["SortDirection"].ToString();
                   }
                   if (ViewState["SortExpression"] != null)
                   {
                       sortExpression = ViewState["SortExpression"].ToString();
                       if (sortDirection == "ASC")
                       {
                           query = query.OrderBy(mov => mov.GetType().GetProperty(sortExpression).GetValue(mov, null)).ToList();
                       }
                       else if (sortDirection == "DESC")
                       {
                           query = query.OrderByDescending(mov => mov.GetType().GetProperty(sortExpression).GetValue(mov, null)).ToList();
                       }
                   }
               } // END OF: if there is no filter - get all movies
                   
                 // If there is filter - Start
               else
               {
                   if (ViewState["SortDirection"] != null)
                   {
                       sortDirection = ViewState["SortDirection"].ToString();
                   }
                   if (ViewState["SortExpression"] != null)
                   {
                       sortExpression = ViewState["SortExpression"].ToString();
                       if (sortDirection == "ASC")
                       {
                           query = FilterMovies().OrderBy(mov => mov.GetType().GetProperty(sortExpression).GetValue(mov, null)).ToList();
                       }
                       else if (sortDirection == "DESC")
                       {
                           query = FilterMovies().OrderByDescending(mov => mov.GetType().GetProperty(sortExpression).GetValue(mov, null)).ToList();
                       }

                   }
               } // If there is filter - END
                                          
               GridView1.DataSource = query;
               GridView1.DataBind();
          // TRY -END 
       }
           catch (Exception ex)
           {
               lblMessage.ForeColor = System.Drawing.Color.Red;
               lblMessage.Text = "An error occurred while retrieving movies. Please try again." + "<br/>" + ex.ToString();
           }
           
       }

       protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
       {
           int pageIndex = e.NewPageIndex;         
           GridView1.PageIndex = pageIndex;
           BindGridView1();           
        }

       protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
       {
           //lblMessage.Text = e.SortExpression + " " + e.SortDirection;
           
           string sortExpression = e.SortExpression.ToString();
           ViewState["SortExpression"] = e.SortExpression;

           GetSortDirection(sortExpression);
           string sortDirection = ViewState["SortDirection"].ToString();
           lblMessage.Text = e.SortExpression + " " + sortDirection;

           BindGridView1();    
       }
        
       private void GetSortDirection(string column)
       {
           // By default, set the sort direction to ascending.
           string sortDirection = "ASC";
           // Retrieve the last column that was sorted.
           string sortExpression = ViewState["SortExpression"] as string;

           if (sortExpression != null)
           {
               // Check if the same column is being sorted.
               // Otherwise, the default value can be returned.
               if (sortExpression == column)
               {
                   string lastDirection = ViewState["SortDirection"] as string;
                   if ((lastDirection != null) && (lastDirection == "ASC"))
                   {
                       sortDirection = "DESC";
                   }
               }
           }

           // Save new values in ViewState.
           ViewState["SortDirection"] = sortDirection;
           ViewState["SortExpression"] = column;

          // return sortDirection;
       }


        // Add Client-Side confirmation when "Delete" button is clicked
       protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
       {
           if (e.Row.RowType == DataControlRowType.DataRow)
        {
        // reference the "Delete" LinkButton - first Control in first Cell
        LinkButton db = (LinkButton)e.Row.Cells[0].Controls[0];

        db.Attributes.Add("onclick", "return confirm('Are you sure you want to delete movie with Title - "
                            + DataBinder.Eval(e.Row.DataItem, "OriginalName") + "');");
        }
       }

       protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
       {
           try
           {
               int movieID = Convert.ToInt32(e.Keys[0]);
               string movieTitle = e.Values["OriginalName"].ToString();
               MovieCatalogBL context = new MovieCatalogBL();
               context.DeleteMovieByID(movieID);
               //FilterMovies();
               // after deletion rebind gridview with filter
               FilterButton_Click(sender, e);
               lblMessage.Text = "Movie: " + movieID + "-" + movieTitle + " deleted!";
               //Server.Transfer("~/Default.aspx");
               //Response.Redirect("~/Default.aspx");
           }
           catch (Exception)
           {
               lblMessage.ForeColor = System.Drawing.Color.Red;
               lblMessage.Text = "An error occurred while deleting movie record. Make sure that movie exists. " +
                                   "It is possible that other user deleted movie record.";
           }

       }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
       {
           
           try
           {
               MovieCatalogBL context = new MovieCatalogBL();
               List<Movie> movieList = context.GetMovies().ToList();
               
               if (movieList.Count > 0)
               {
                   string path = Server.MapPath("~/exportedfiles/");                   

                   if (!Directory.Exists(path))   // CHECK IF THE FOLDER EXISTS. IF NOT, CREATE A NEW FOLDER.
                   {
                       Directory.CreateDirectory(path);
                   }

                   // "File.Delete" method does not throw an exception when a file doesn't exist.
                   File.Delete(path + "Movies.xls"); // DELETE THE FILE BEFORE CREATING A NEW ONE.
                   
                   // ADD A WORKBOOK USING THE EXCEL APPLICATION.
                   Excel.Application xlAppToExport = new Excel.Application();
                   xlAppToExport.Workbooks.Add("");

                   // ADD A WORKSHEET.
                   Excel.Worksheet xlWorkSheetToExport = default(Excel.Worksheet);
                   xlWorkSheetToExport = (Excel.Worksheet)xlAppToExport.Sheets["Sheet1"];

                   // ROW ID FROM WHERE THE DATA STARTS SHOWING.
                   int iRowCnt = 4;

                   // SHOW THE HEADER.
                   xlWorkSheetToExport.Cells[1, 1] = "Movies list.";

                   Excel.Range range = xlWorkSheetToExport.Cells[1, 1] as Excel.Range;
                   range.EntireRow.Font.Name = "Arial";
                   range.EntireRow.Font.Bold = true;
                   range.EntireRow.Font.Size = 20;

                   // Depends how long the title is
                   xlWorkSheetToExport.Range["A1:F1"].MergeCells = true;       // MERGE CELLS OF THE HEADER.

                   // SHOW COLUMNS ON THE TOP.
                   xlWorkSheetToExport.Cells[iRowCnt - 1, 1] = "Title";
                   xlWorkSheetToExport.Cells[iRowCnt - 1, 2] = "Country";
                   xlWorkSheetToExport.Cells[iRowCnt - 1, 3] = "Genre";
                   xlWorkSheetToExport.Cells[iRowCnt - 1, 4] = "Year Of Production";
                   xlWorkSheetToExport.Cells[iRowCnt - 1, 5] = "Content Provider";
                   xlWorkSheetToExport.Cells[iRowCnt - 1, 6] = "Duration";
                   xlWorkSheetToExport.Cells[iRowCnt - 1, 7] = "IPTV Rights";
                   xlWorkSheetToExport.Cells[iRowCnt - 1, 8] = "VOD Rights";
                   xlWorkSheetToExport.Cells[iRowCnt - 1, 9] = "SVOD Rights";
                   xlWorkSheetToExport.Cells[iRowCnt - 1, 10] = "Ancillary Rights";
                   xlWorkSheetToExport.Cells[iRowCnt - 1, 11] = "Start Date";
                   xlWorkSheetToExport.Cells[iRowCnt - 1, 12] = "Expiry Date";
                   xlWorkSheetToExport.Cells[iRowCnt - 1, 13] = "Comment";


                   foreach (var item in movieList)
                   {
                       xlWorkSheetToExport.Cells[iRowCnt, 1] = item.OriginalName;
                       xlWorkSheetToExport.Cells[iRowCnt, 2] = item.Country;
                       xlWorkSheetToExport.Cells[iRowCnt, 3] = item.Genre;
                       xlWorkSheetToExport.Cells[iRowCnt, 4] = item.Year;
                       xlWorkSheetToExport.Cells[iRowCnt, 5] = item.ContentProvider;
                       // Duration has to be converted to string (Duration.ToString()), otherwise data can not be exported
                       xlWorkSheetToExport.Cells[iRowCnt, 6] = item.Duration.ToString();
                       xlWorkSheetToExport.Cells[iRowCnt, 7] = item.RightsIPTV;
                       xlWorkSheetToExport.Cells[iRowCnt, 8] = item.RightsVOD;
                       xlWorkSheetToExport.Cells[iRowCnt, 9] = item.SVODRights;
                       xlWorkSheetToExport.Cells[iRowCnt, 10] = item.AncillaryRights;
                       xlWorkSheetToExport.Cells[iRowCnt, 11] = item.StartDate;
                       xlWorkSheetToExport.Cells[iRowCnt, 12] = item.ExpireDate;
                       xlWorkSheetToExport.Cells[iRowCnt, 13] = item.Comment;
                                              

                       iRowCnt = iRowCnt + 1;
                   }

                   // FINALLY, FORMAT THE EXCEL SHEET USING EXCEL'S AUTOFORMAT FUNCTION.
                   Excel.Range range1 = xlAppToExport.ActiveCell.Worksheet.Cells[4, 1] as Excel.Range;
                   range1.AutoFormat(ExcelAutoFormat.xlRangeAutoFormatList3);


                   // Set width of Column                   
                   xlWorkSheetToExport.Columns[2].ColumnWidth = 25;                    
                   xlWorkSheetToExport.Columns[7].ColumnWidth = 25;
                   xlWorkSheetToExport.Columns[8].ColumnWidth = 25;
                                     

                   // SAVE THE FILE IN A FOLDER. (XLS or XLSX format)
                   xlWorkSheetToExport.SaveAs(path + "Movies.xls");
                   // xlWorkSheetToExport.SaveAs(path + "Movies.xlsx");
                   
                   // CLEAR.
                   xlAppToExport.Workbooks.Close();
                   xlAppToExport.Quit();
                   xlAppToExport = null;
                   xlWorkSheetToExport = null;

                   lblMessage.Text = "Data Exported.";
                   lblMessage.Attributes.Add("style", "color:green; font: bold 14px/16px Sans-Serif,Arial");
               }
      
           }
           catch (IOException)
           {
               lblMessage.Text = "There was an error while exporting data. Check if file is in use by another application (MS Excel).";
               lblMessage.Attributes.Add("style", "color:red; font: bold 14px/16px Sans-Serif,Arial");
           }
           catch (Exception)
           {
               lblMessage.Text = "There was an error while exporting data. Try again.";
               lblMessage.Attributes.Add("style", "color:red; font: bold 14px/16px Sans-Serif,Arial");
           }
    
       }
        
       // VIEW THE EXPORTED EXCEL DATA.
       protected void ViewData(object sender, System.EventArgs e)
       {
         // change path as needed
           string path = Server.MapPath("~/exportedfiles/");
            
           try
           {
               // CHECK IF THE FOLDER EXISTS.
               if (Directory.Exists(path))
               {
                   // CHECK IF THE FILE EXISTS.
                   if (File.Exists(path + "Movies.xls"))
                   {
                       //www.daniweb.com/web-development/aspnet/threads/209532/how-to-open-an-excel-file-with-in-the-asp-net-web-page
                       string strScript = "<script language=JavaScript>window.open('exportedfiles/" + "Movies" + ".xls','dn','width=1,height=1,toolbar=no,top=300,left=400,right=1, scrollbars=no,locaton=1,resizable=1');</script>";
                       if (!Page.ClientScript.IsStartupScriptRegistered("clientScript"))
                       {
                           ClientScript.RegisterStartupScript(this.GetType(), "clientScript", strScript);
                       }
                                          
                   }
                   else
                   {
                       lblMessage.Text = "File with exported data does not exist.";
                       lblMessage.Attributes.Add("style", "color:red; font: bold 14px/16px Sans-Serif,Arial");
                   }
               }
               else
               {
                   lblMessage.Text = "Directory with exported data does not exist.";
                   lblMessage.Attributes.Add("style", "color:red; font: bold 14px/16px Sans-Serif,Arial");
               }

           }
           catch (Exception)
           {
               lblMessage.Text = "There was an error while opening file. Try again.";
               lblMessage.Attributes.Add("style", "color:red; font: bold 14px/16px Sans-Serif,Arial");
           }
           
       }


    }
}
