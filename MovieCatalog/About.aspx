<%@ Page Title="About Us" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="About.aspx.cs" Inherits="MovieCatalog.About" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        About
    </h2>

<asp:DropDownCheckBoxes ID="yearsDropDownCheckBoxes" runat="server" OnSelectedIndexChanged="yearsDropDownCheckBoxes_SelectedIndexChanged"
                AddJQueryReference="true" UseButtons="True" UseSelectAllNode="True">
                <Style SelectBoxWidth="160" DropDownBoxBoxWidth="160" DropDownBoxBoxHeight="80" />
                <Texts SelectBoxCaption="Year" />                      
            </asp:DropDownCheckBoxes>

<div style="padding-top: 10px;">
            <h1>
                Selected items (updated whenever postback from any of the controls occurs)</h1>
            <asp:Panel ID="selectedItemsPanel" runat="server" meta:resourcekey="selectedItemsPanelResource1">
            </asp:Panel>
        </div>

        <br /><br />
<!-- Movies DDCBox Control -->
<asp:DropDownCheckBoxes ID="MoviesDropDownCheckBoxes" runat="server" OnSelectedIndexChanged="MoviesDropDownCheckBoxes_SelectedIndexChanged"
                AddJQueryReference="true" UseButtons="True" UseSelectAllNode="True">
                <Style SelectBoxWidth="160" DropDownBoxBoxWidth="160" DropDownBoxBoxHeight="80" />
                <Texts SelectBoxCaption="Movies" />                      
            </asp:DropDownCheckBoxes>

<div style="padding-top: 10px;">
            <h1>
                Selected items (updated whenever postback from any of the controls occurs)</h1>
            <asp:Panel ID="selectedItemsPanel2" runat="server" meta:resourcekey="selectedItemsPanelResource2">
            </asp:Panel>
        </div>

       <asp:TextBox ID="movieTextBox" runat="server" ></asp:TextBox>

    
    <!-- string manipulation: string to array of words -->
    <br /><br />
    <asp:Button ID="btnKlik" runat="server" Text="Klik" onclick="btnKlik_Click" />
    <br />  
     <asp:Label ID="lblText" runat="server"></asp:Label>

     <br /><br /> 
     <!-- List Of Countries --> 
     <asp:Button ID="btnCountries" runat="server" Text="Fill DropDownCheckBox control Countries" 
        onclick="btnCountries_Click" /> 
        <br />
     <asp:DropDownCheckBoxes ID="countriesDropDownCheckBoxes" runat="server" 
                AddJQueryReference="true" UseButtons="True" UseSelectAllNode="True">
                <Style SelectBoxWidth="200" DropDownBoxBoxWidth="160" DropDownBoxBoxHeight="240" />
                <Texts SelectBoxCaption="Countries" />                      
            </asp:DropDownCheckBoxes>
    
         
    <br /><br />  
      <asp:DropDownList ID="ddlCountries" runat="server"></asp:DropDownList>
      <br />
      <asp:Button ID="btnCount" Text="Count Countries" runat="server" 
        onclick="btnCount_Click" />
        <asp:Label ID="lblBroj" runat="server"></asp:Label>

        <br /><br />  
        <asp:Label ID="lblText2" runat="server"></asp:Label>

        <br /><br />  
        <asp:Label ID="lblText3" runat="server"></asp:Label>
      
      <br /><br /> 
      <!-- Time DropDownList -->
      <asp:DropDownList ID="DropDownList1" runat="server">
</asp:DropDownList>
<asp:DropDownList ID="DropDownList2" runat="server">
</asp:DropDownList>
<asp:DropDownList ID="DropDownList3" runat="server">
 </asp:DropDownList>
 <br />
 <asp:Button ID="btnDuration" Text="Duration" runat="server" onclick="btnDuration_Click" />
 <br />
 <asp:Label ID="lblDuration" runat="server"></asp:Label>


 <br /><br /> 
      <!-- Calendar Control with Years and Months dropDownLists -->
      <div style="font-family:Arial">
      <asp:Label ID="lblYear" runat="server">Year:</asp:Label>

      <asp:DropDownList ID="DropDownListYear" runat="server" AutoPostBack="True" 
        onselectedindexchanged="DropDownListYear_SelectedIndexChanged">
    </asp:DropDownList>
    <asp:Label ID="lblMonth" runat="server">Month:</asp:Label>
    
    <asp:DropDownList ID="DropDownListMonth" runat="server" AutoPostBack="True" 
        onselectedindexchanged="DropDownListMonth_SelectedIndexChanged">
    </asp:DropDownList>
    <br />
    <asp:Calendar ID="CalendarXX" runat="server"></asp:Calendar>
    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
        Text="Get Selected Date" />
</div>
<asp:Label ID="lblDate1" runat="server"></asp:Label>


<!-- Must be if DropDownCheckBox control is used within WebForm that inherits MasterPage -->
<div>           
<asp:ScriptManager ID="ScriptManager1" runat="server" />   
<script type="text/javascript" language="javascript">
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler); 
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    function BeginRequestHandler(sender, args) { }
    function EndRequestHandler(sender, args) { }       
</script>
</div>

 </asp:Content>

