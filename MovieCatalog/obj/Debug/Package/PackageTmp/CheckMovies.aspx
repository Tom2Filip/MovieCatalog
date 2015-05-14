<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CheckMovies.aspx.cs" Inherits="MovieCatalog.CheckMovies" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   
    <h2>Movies to expire in next 30 days</h2>
    
    <asp:ListView runat="server" ID="ListView2" onitemdatabound="ListView2_ItemDataBound">
  <LayoutTemplate>
     <table class="example" ID="tbl1" runat="server">
      <tr>
        <th>Nr.</th>      <th>ID</th><th>Movie Title</th>   <th>Expiry Date</th> <th>Days to Expiration</th>
      </tr>
      <tr runat="server" id="itemPlaceholder" ></tr>
    </table>
  </LayoutTemplate>
  
    <ItemTemplate>
        <%# Container.DataItemIndex + 1 %>
    </ItemTemplate>
    
  <ItemTemplate>       
          <tr id="Tr1" runat="server">
            <td>
               <%# Container.DataItemIndex + 1 %>
            </td>
            <td>
              <asp:Label ID="Label1" runat="server" Text='<%# Eval("Id") %>' />
            </td>
            <td>
             <asp:Label ID="Label2" runat="server" Text='<%# Eval("OriginalName") %>' />
              </td>
            <td>                       
              <asp:Label ID="lblExpireDate" runat="server" Text='<%# Eval("ExpireDate" , "{0:d}") %>' />
              </td>
              <td>
              <asp:Label ID="lblDaysToExpiry" runat="server"/>
              </td>
           </tr>       
  </ItemTemplate>
  <EmptyDataTemplate>
  <span>There is no Movies</span>
  </EmptyDataTemplate>
</asp:ListView>


<br />
<asp:Button ID="btnCheck" runat="server" onclick="btnCheck_Click" Text="Check Movies" />
<br />
<p><asp:Button ID="btnExportToExcel" Text="Export to Excel" runat="server" onclick="btnExportToExcel_Click" /></p>

<p><asp:Button ID="btnViewExportedData" Text="View Exported Data" runat="server" onclick="ViewData" /></p>

    <asp:ListView ID="ListView1" runat="server" DataKeyNames="Id"> 
    <LayoutTemplate>
    <table id="ListViewMovies" class="example">
    <tr id="itemPlaceholder" runat="server" />                    
                       <tr>
                           <th>
                             <a href="#">Title</a>
                            </th>
                           <th>
                             <a href="#">Date</a>
                           </th>
                           <th>
                            <a href="#">Day to Expiry</a>
                           </th>                          
                         </tr>                             
       </table>
    </LayoutTemplate>                         
    <ItemTemplate>  
        <div style="width:400px; padding:2px; color:Gray; border-left:1px solid #e5eff8; background-color:#007ACC; background-color:#ebecda"> 
        <asp:Literal ID="Literal1" runat="server">Movie:</asp:Literal>
        <asp:Label ID="lbl" runat="server" Text='<%#Eval("OriginalName")%>'></asp:Label>
        <span>expires on</span>
        <asp:Label ID="lblExpireDate" runat="server" Text='<%#Eval("ExpireDate", "{0:d}")%>'/>
        <span>, expires in</span>
        <asp:Label ID="lblDaysToExpiry" runat="server"></asp:Label>
        <span>days.</span>
        </div>
    </ItemTemplate>
    <AlternatingItemTemplate>
        <div style="width:500px; padding:2px; color:Gray;  border-top:none; background-color:White" >
        <asp:Literal ID="Literal1" runat="server">Movie:</asp:Literal>
        <asp:Label ID="lbl" runat="server" Text='<%#Eval("OriginalName")%>'></asp:Label>
        <span>expires on</span>
        <asp:Label ID="lblExpireDate" runat="server" Text='<%#Eval("ExpireDate", "{0:d}")%>'/>
        <span>, expires in</span>
        <asp:Label ID="lblDaysToExpiry" runat="server"></asp:Label>
        <span>days.</span>
        </div>
    </AlternatingItemTemplate>
    </asp:ListView>
    
    <asp:Label ID="lblMessage2" runat="server"></asp:Label>
   
</asp:Content>
