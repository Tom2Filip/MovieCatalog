using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Entity.Core;
using MovieCatalog.DAL;
using MovieCatalog.BLL;
using System.Data;

using System.IO;
//www.microsoft.com/en-us/download/confirmation.aspx?id=20923
using Excel = Microsoft.Office.Interop.Excel;
using ExcelAutoFormat = Microsoft.Office.Interop.Excel.XlRangeAutoFormat;

namespace MovieCatalog
{
    public partial class _Default : System.Web.UI.Page
    {
        // counter for selected items in filterMoviesCheckBoxList
        public int selectedItems = 0;
        
        protected void Page_Load(object sender, EventArgs e)
        {
          
            if (!IsPostBack)
            {
                BindGridView1();
            }
        }

        // srediti filtriranje - uz sortiranje
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
               // MoviesDBEntities context2 = new MoviesDBEntities();
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
               { // ako je index 0 onda samo prvi rights (SVOD), inače samo drugi rights (Ancillary)                 
                   //var query = context2.Movies.Where(m => m.SVODRights == "Yes").ToList();
                   var query1 = context1.GetMovies().Where(m => m.SVODRights == "Yes").ToList();
                   return query1;
               }
               else // try this - (filterMoviesCheckBoxList.SelectedIndex == 1) -> NO!
               { // if index NOT 0 then only second rights (Ancillary) - (if index NOT 0 then it's 1, because there is only 2 items in list)
                   //var query = context2.Movies.Where(m => m.AncillaryRights == "Yes").ToList();
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
                       // dv.Sort = string.Concat(sortExpression, " ", sortDirection);
                       // Sort the query
                       // stackoverflow.com/questions/722868/sorting-a-list-using-lambda-linq-to-objects
                       // This is how I solved my problem:
                       if (sortDirection == "ASC")
                       {
                           //query = FilterMovies().OrderBy(mov => mov.GetType().GetProperty(sortExpression).GetValue(mov, null)).ToList();
                           query = query.OrderBy(mov => mov.GetType().GetProperty(sortExpression).GetValue(mov, null)).ToList();
                       }
                       else if (sortDirection == "DESC")
                       {
                           //query = FilterMovies().OrderByDescending(mov => mov.GetType().GetProperty(sortExpression).GetValue(mov, null)).ToList();
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
                       // dv.Sort = string.Concat(sortExpression, " ", sortDirection);
                       // Sort the query
                       // stackoverflow.com/questions/722868/sorting-a-list-using-lambda-linq-to-objects
                       // This is how I solved my problem:
                       if (sortDirection == "ASC")
                       {
                           query = FilterMovies().OrderBy(mov => mov.GetType().GetProperty(sortExpression).GetValue(mov, null)).ToList();
                           //query = query.OrderBy(mov => mov.GetType().GetProperty(sortExpression).GetValue(mov, null)).ToList();
                       }
                       else if (sortDirection == "DESC")
                       {
                           query = FilterMovies().OrderByDescending(mov => mov.GetType().GetProperty(sortExpression).GetValue(mov, null)).ToList();
                           //query = query.OrderByDescending(mov => mov.GetType().GetProperty(sortExpression).GetValue(mov, null)).ToList();
                       }

                   }
               } // If there is filter - END
                                          
               GridView1.DataSource = query;
               GridView1.DataBind();
           }
           catch (Exception)
           {
               lblMessage.ForeColor = System.Drawing.Color.Red;
               lblMessage.Text = "An error occurred while retrieving movies. Please try again.";
           }
       }

       protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
       {
           int pageIndex = e.NewPageIndex;         
           /*
           MovieCatalogBL context = new MovieCatalogBL();
           var query = context.GetMovies();
           GridView1.DataSource = query;
           */
           GridView1.PageIndex = pageIndex;
           BindGridView1();           
           //GridView1.DataBind();           
        }

       // !! Srediti filtriranje - uz sortiranje - ako se sortira nakon filtriranja!!
       //deepak-sharma.net/2012/10/25/gridview-paging-and-sorting-in-asp-net-without-using-a-datasource/
       //www.dotnetgallery.com/kb/resource12-How-to-implement-paging-and-sorting-in-aspnet-Gridview-control.aspx
       protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
       {
           //lblMessage.Text = e.SortExpression + " " + e.SortDirection;
           
           string sortExpression = e.SortExpression.ToString();
           ViewState["SortExpression"] = e.SortExpression;

           GetSortDirection(sortExpression);
           // ako je dva puta zaredom kliknuta !ISTA! kolumna onda mijenja iz ASC u DESC
           //string sortDirection = GetSortDirection(e.SortExpression);
           string sortDirection = ViewState["SortDirection"].ToString();
           lblMessage.Text = e.SortExpression + " " + sortDirection;

           BindGridView1();
           // 17.04.2015.  commented -> same code in "BindGridView1()"
           /*  BEGIN!
           MovieCatalogBL context = new MovieCatalogBL();
           var query = context.GetMovies();
           
           // stackoverflow.com/questions/722868/sorting-a-list-using-lambda-linq-to-objects
           // This is how I solved my problem:
           if (sortDirection == "ASC")
           {
               query = query.OrderBy(mov => mov.GetType().GetProperty(sortExpression).GetValue(mov, null)).ToList(); 
           }
           else if (sortDirection == "DESC")
           {
               query = query.OrderByDescending(mov => mov.GetType().GetProperty(sortExpression).GetValue(mov, null)).ToList();
           }
                      
           GridView1.DataSource = query;
           GridView1.DataBind(); 
            END!!
           */


        
           /*  !!!  !!!!     !!!!   !!!!    !!!!    !!!!    !!!!    !!!!    !!!!    !!!!
           /// KUDVENKAT -https://www.youtube.com/watch?v=dmyLe6bKdtg
           // string strSortDirection = e.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            var query = context.GetMovies();
            query = query.OrderByDescending(m => m.Year).ToList();
           //GridView1.DataSource = EmployeeDataAccessLayer.GetAllEmployees(e.SortExpression + " " + strSortDirection);
            GridView1.DataSource = query;
           GridView1.DataBind();

         

           // POGLEDATI PRVO !!
           //stackoverflow.com/questions/22541592/paging-and-sorting-entity-framework-on-a-field-from-partial-class
           //forums.asp.net/p/1976270/5655727.aspx?Paging+and+sorting+Entity+Framework+on+a+field+from+Partial+Class

           /*
            www.c-sharpcorner.com/UploadFile/8572ef/work-with-gridview-using-entity-framework/
            * 
            www.codewrecks.com/blog/index.php/2009/03/21/entity-framework-dynamic-sorting-and-pagination/
            social.msdn.microsoft.com/Forums/en-US/c4d03fd8-3890-49a8-92de-4a0b3bd68d59/how-do-i-use-linq-to-sort-items-using-a-variable-sortorderingsubordering-scheme?forum=adodotnetentityframework
            social.msdn.microsoft.com/Forums/en-US/86834aa4-c933-4ad0-bf9d-da1652d5e9dc/entity-framework-and-dynamic-order-by-statements?forum=adodotnetentityframework
            www.codeproject.com/Tips/666957/Dynamic-Sorting-in-LINQ-Part
           */      
                    
       }
        
       // msdn.microsoft.com/en-us/library/system.web.ui.webcontrols.gridview.sorting%28v=vs.110%29.aspx
       // msdn.microsoft.com/en-us/library/hwf94875%28v=vs.110%29.aspx
       // msdn.microsoft.com/en-us/library/system.web.ui.webcontrols.gridviewsorteventargs.sortdirection%28v=vs.100%29.aspx
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


       //dotnetawesome.blogspot.com/2013/11/how-to-import-export-database-data-from_18.html
       //www.encodedna.com/2013/01/asp.net-export-to-excel.htm
       protected void btnExportToExcel_Click(object sender, EventArgs e)
       {
           try
           {
               MovieCatalogBL context = new MovieCatalogBL();
               List<Movie> movieList = context.GetMovies().ToList();
               
               if (movieList.Count > 0)
               {
                   //forums.asp.net/t/1813648.aspx?Any+difference+between+Server+MapPath+and+Server+MapPath+
                   string path = Server.MapPath("exportedfiles\\");
                   //string path2 = Server.MapPath("~\\d:\\exportedfiles\\");
                   string path2 = Server.MapPath("~/d://exportedfiles/");
                   //relativepath = "~/Images/Users/" + FriendID + "/";

                   if (!Directory.Exists(path))   // CHECK IF THE FOLDER EXISTS. IF NOT, CREATE A NEW FOLDER.
                   {
                       Directory.CreateDirectory(path);
                   }
                   if (!Directory.Exists(path2))   // CHECK IF THE FOLDER EXISTS. IF NOT, CREATE A NEW FOLDER.
                   {
                       Directory.CreateDirectory(path2);
                   }


                   File.Delete(path + "Movies.xls"); // DELETE THE FILE BEFORE CREATING A NEW ONE.
                   // File.Delete(path + "MoviesToExpire.xlsx.xls"); // DELETE THE FILE BEFORE CREATING A NEW ONE.
                   File.Delete(path2 + "Movies.xls"); // DELETE THE FILE BEFORE CREATING A NEW ONE.
                   // File.Delete(path2 + "MoviesToExpire.xlsx.xls"); // DELETE THE FILE BEFORE CREATING A NEW ONE.

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
                   xlWorkSheetToExport.Columns[2].ColumnWidth = 50;
                   xlWorkSheetToExport.Columns[7].ColumnWidth = 50;
                   xlWorkSheetToExport.Columns[8].ColumnWidth = 50;
                                      

                   // SAVE THE FILE IN A FOLDER. (XLS or XLSX format)
                   xlWorkSheetToExport.SaveAs(path + "Movies.xls");
                   // xlWorkSheetToExport.SaveAs(path + "Movies.xlsx");
                   xlWorkSheetToExport.SaveAs(path2 + "Movies.xls");
                   // xlWorkSheetToExport.SaveAs(path2 + "Movies.xlsx");

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
           string path = Server.MapPath("exportedfiles\\");
           try
           {
               // CHECK IF THE FOLDER EXISTS.
               if (Directory.Exists(path))
               {
                   // CHECK IF THE FILE EXISTS.
                   if (File.Exists(path + "Movies.xls"))
                   {
                       // SHOW (NOT DOWNLOAD) THE EXCEL FILE.
                       Excel.Application xlAppToView = new Excel.Application();
                       xlAppToView.Workbooks.Open(path + "Movies.xls");
                       xlAppToView.Visible = true;
                   }
                   else
                   {
                       lblMessage.Text = "File with exported data does not exist.";
                       lblMessage.Attributes.Add("style", "color:red; font: bold 14px/16px Sans-Serif,Arial");
                   }
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
