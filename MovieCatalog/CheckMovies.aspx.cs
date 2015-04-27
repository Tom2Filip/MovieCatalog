using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MovieCatalog.DAL;
using MovieCatalog.BLL;

using System.Data;
using System.Data.SqlClient;
using System.IO;

//www.microsoft.com/en-us/download/confirmation.aspx?id=20923
using Excel = Microsoft.Office.Interop.Excel;
using ExcelAutoFormat = Microsoft.Office.Interop.Excel.XlRangeAutoFormat;

namespace MovieCatalog
{
    public partial class CheckMovies : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MovieCatalogBL context = new MovieCatalogBL();
                var query = context.GetMoviesExpiry();
                ListView2.DataSource = query;
                ListView2.DataBind();
            }

        }

        // msdn.microsoft.com/en-us/library/bb398790%28v=vs.140%29.aspx#CreatingTemplatesForTheListViewControl
        // Days to Expiration - fill the column fields
        protected void ListView2_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                // Display the Expiry Date in italics.
              Label expDateLabel = (Label)e.Item.FindControl("lblExpireDate");
              Label daysToExpiryLabel = (Label)e.Item.FindControl("lblDaysToExpiry");

              // Convert to Date lblExpireDate Text
              DateTime expDate = Convert.ToDateTime(expDateLabel.Text);
              // calculate the days to expiration
              int daysToExpiry = (Int32)((expDate - DateTime.Now).Days);
              daysToExpiryLabel.Text = daysToExpiry.ToString();

              daysToExpiryLabel.Font.Bold = true;
              daysToExpiryLabel.ForeColor = System.Drawing.Color.Red;

            }
        }


        protected void btnCheck_Click(object sender, EventArgs e)
        {
            MovieCatalogBL context = new MovieCatalogBL();
            // check if it's expired or expires in less than 30 days
            var query2 = context.GetMoviesExpiry();
            ListView1.DataSource = query2;
            ListView1.DataBind();

            AddExpiryDays();
            lblMessage2.Text = "Number of movies that will expire in next 30 days: " + ListView1.Items.Count.ToString();
        }

       protected void AddExpiryDays()
        {
            foreach (ListViewItem item in ListView1.Items)
            {
                Label labelDaysToExpiry = (Label)item.FindControl("lblDaysToExpiry");
                Label labelExpiryDate = (Label)item.FindControl("lblExpireDate");
                DateTime expiryDate = Convert.ToDateTime(labelExpiryDate.Text);
                int daysToExpiry = (Int32)((expiryDate - DateTime.Now).Days);
                labelDaysToExpiry.Text = " " + daysToExpiry;// + totalDays2.ToString();
            }
        }

       //dotnetawesome.blogspot.com/2013/11/how-to-import-export-database-data-from_18.html
       //www.encodedna.com/2013/01/asp.net-export-to-excel.htm
       protected void btnExportToExcel_Click(object sender, EventArgs e)
       {
          try
          {
           MovieCatalogBL context = new MovieCatalogBL();
           List<Movie> movieList = context.GetMoviesExpiry().ToList();
           //var query = context.GetMoviesExpiry();
           
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


               File.Delete(path + "MoviesToExpire.xls"); // DELETE THE FILE BEFORE CREATING A NEW ONE.
              // File.Delete(path + "MoviesToExpire.xlsx.xls"); // DELETE THE FILE BEFORE CREATING A NEW ONE.
               File.Delete(path2 + "MoviesToExpire.xls"); // DELETE THE FILE BEFORE CREATING A NEW ONE.
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
               xlWorkSheetToExport.Cells[1, 1] = "Movies To Expire in next 30 days.";

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
               xlWorkSheetToExport.Cells[iRowCnt - 1, 5] = "Expiry Date";
                   
              
               foreach (var item in movieList)
               {
                   xlWorkSheetToExport.Cells[iRowCnt, 1] = item.OriginalName;
                   xlWorkSheetToExport.Cells[iRowCnt, 2] = item.Country;
                   xlWorkSheetToExport.Cells[iRowCnt, 3] = item.Genre;
                   xlWorkSheetToExport.Cells[iRowCnt, 4] = item.Year;
                   xlWorkSheetToExport.Cells[iRowCnt, 5] = item.ExpireDate;

                   iRowCnt = iRowCnt + 1;
               }
          
               // FINALLY, FORMAT THE EXCEL SHEET USING EXCEL'S AUTOFORMAT FUNCTION.
               Excel.Range range1 = xlAppToExport.ActiveCell.Worksheet.Cells[4, 1] as Excel.Range;
               range1.AutoFormat(ExcelAutoFormat.xlRangeAutoFormatList3);

               // SAVE THE FILE IN A FOLDER. (XLS or XLSX format)
               xlWorkSheetToExport.SaveAs(path + "MoviesToExpire.xls");
              // xlWorkSheetToExport.SaveAs(path + "MoviesToExpire.xlsx");
               xlWorkSheetToExport.SaveAs(path2 + "MoviesToExpire.xls");
              // xlWorkSheetToExport.SaveAs(path2 + "MoviesToExpire.xlsx");

               // CLEAR.
               xlAppToExport.Workbooks.Close();
               xlAppToExport.Quit();
               xlAppToExport = null;
               xlWorkSheetToExport = null;

               
               lblMessage2.Text = "Data Exported.";
               lblMessage2.Attributes.Add("style", "color:green; font: bold 14px/16px Sans-Serif,Arial");                              
             }
           
           }
          catch (IOException)
          {
              lblMessage2.Text = "There was an error while exporting data. Check if file is in use by another application (MS Excel).";
              lblMessage2.Attributes.Add("style", "color:red; font: bold 14px/16px Sans-Serif,Arial");
          }
           catch (Exception)
           {
               lblMessage2.Text = "There was an error while exporting data. Try again.";
               lblMessage2.Attributes.Add("style", "color:red; font: bold 14px/16px Sans-Serif,Arial");
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
                   if (File.Exists(path + "MoviesToExpire.xls"))
                   {
                       // SHOW (NOT DOWNLOAD) THE EXCEL FILE.
                       Excel.Application xlAppToView = new Excel.Application();
                       xlAppToView.Workbooks.Open(path + "MoviesToExpire.xls");
                       xlAppToView.Visible = true;
                   }
                   else
                   {
                       lblMessage2.Text = "File with exported data does not exist.";
                       lblMessage2.Attributes.Add("style", "color:red; font: bold 14px/16px Sans-Serif,Arial");
                   }
               }
               
           }
           catch (Exception)
           {
               lblMessage2.Text = "There was an error while opening file. Try again.";
               lblMessage2.Attributes.Add("style", "color:red; font: bold 14px/16px Sans-Serif,Arial");
           }
       }

    }
}