<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="MovieCatalog._Default" %> 

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<!-- Pretvoriti u dll -> Deploy -->

    <h2>Welcome to Movie Catalog</h2>
               
    <asp:Label ID="lblFilter" Text="Filter by:" runat="server" AssociatedControlID="filterMoviesCheckBoxList"></asp:Label>
    <asp:CheckBoxList ID="filterMoviesCheckBoxList" runat="server" RepeatDirection="Horizontal" CssClass="sameLine">
    <asp:ListItem Value="0">SVOD rights</asp:ListItem>
    <asp:ListItem Value="1">Ancillary rights</asp:ListItem>
    </asp:CheckBoxList>
    
    <asp:Button ID="FilterButton" runat="server" Text="Filter" CssClass="btnFilter" 
        onclick="FilterButton_Click" BorderStyle="Outset"/>
    <br /><br />
    
      <asp:GridView ID="GridView1" runat="server" AllowPaging="True" CssClass="GridViewStyle"
        AllowSorting="True" DataKeyNames="Id" AutoGenerateColumns="False" onrowdeleting="GridView1_RowDeleting" 
        PageSize="5" onpageindexchanging="GridView1_PageIndexChanging" 
        onsorting="GridView1_Sorting" EnableSortingAndPagingCallbacks="False"
        onrowdatabound="GridView1_RowDataBound">
         
         <HeaderStyle CssClass="GridViewHeaderStyle" />
         <FooterStyle CssClass="GridViewFooterStyle" />
         <RowStyle CssClass="GridViewRowStyle" />   
         <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
         <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
         <PagerStyle CssClass="GridViewPagerStyle" />
         
        <Columns>
            <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
            <asp:BoundField DataField="Id" HeaderText="ID" SortExpression="Id" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField DataField="OriginalName" HeaderText="Title" SortExpression="OriginalName" />
            <asp:BoundField DataField="Genre" HeaderText="Genre" SortExpression="Genre" />
            <asp:BoundField DataField="Country" HeaderText="Country" SortExpression="Country" />
            <asp:BoundField DataField="Year" HeaderText="Year" SortExpression="Year" />
            <asp:HyperLinkField HeaderText="Details" Text="View Details" DataNavigateUrlFields="Id" 
                                DataNavigateUrlFormatString="~\MovieDetails.aspx?Id={0}" />
        </Columns>
    </asp:GridView>
    
    <p>
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
    </p>

    <p><asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/MovieAdd.aspx">Add New Movie</asp:HyperLink></p>

    <p><asp:Button ID="btnExcel" Text="Export to Excel" runat="server" OnClick="btnExportToExcel_Click" /></p>
    <p><asp:Button ID="btnViewExportedData" Text="View Exported Data" runat="server" onclick="ViewData" /></p>

</asp:Content>
