<%@ Page Title="Movie Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MovieDetails.aspx.cs" Inherits="MovieCatalog.MovieDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Movie details</h2>

<!-- Dodati TemplateField kontrolu za 'Duration' s ID za TextBox + validation - RAZLOG: u slučajue da je trajanje '00:00:00' pretvoriti u empty string tj, da je textbox prazan -->
    <asp:DetailsView ID="MovieDetailsView" runat="server" DefaultMode="Edit" OnItemUpdating="MovieDetailsView_ItemUpdating" 
        AutoGenerateRows="false" onmodechanging="MovieDetailsView_ModeChanging" HeaderText="Movie Details"
        
        CssClass="DtView1"
        HeaderStyle-CssClass="header1"
        FieldHeaderStyle-CssClass="fieldHeader"
        AlternatingRowStyle-CssClass="alternating"
        CommandRowStyle-CssClass="command"
        PagerStyle-CssClass="pager" >
    <Fields>   
            <asp:DynamicField DataField="Id" HeaderText="ID" ReadOnly="true" />
            <asp:DynamicField DataField="ContentProvider" HeaderText="Content Provider"/>
            <asp:DynamicField DataField="OriginalName" HeaderText="Title" />
            <asp:DynamicField DataField="Genre" HeaderText="Genre" />
            
            <asp:TemplateField HeaderText="Duration">
            <EditItemTemplate>
            <asp:TextBox ID="txtBoxDuration" runat="server" Text='<%# Bind("Duration") %>' ReadOnly="true" ></asp:TextBox>                   
                   <asp:Label ID="lblHour" runat="server" AssociatedControlID="ddlHours">Hours:</asp:Label>
                    <asp:DropDownList ID="ddlHours" runat="server" OnSelectedIndexChanged="DurationChange" AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:Label ID="lblMinutes" runat="server" AssociatedControlID="ddlMinutes">Minutes:</asp:Label>
                    <asp:DropDownList ID="ddlMinutes" runat="server" OnSelectedIndexChanged="DurationChange" AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:Label ID="lblSeconds" runat="server" AssociatedControlID="ddlSeconds">Seconds:</asp:Label>
                    <asp:DropDownList ID="ddlSeconds" runat="server" OnSelectedIndexChanged="DurationChange" AutoPostBack="true">
                     </asp:DropDownList>                  
                 </EditItemTemplate>
             </asp:TemplateField>

            <asp:TemplateField HeaderText="Country">
                <EditItemTemplate>
                        <asp:TextBox ID="txtBoxCountry" runat="server" Text='<%# Bind("Country") %>' ReadOnly="true" ></asp:TextBox>
                         <asp:RequiredFieldValidator ID="CountryReqValidator" ControlToValidate="txtBoxCountry" 
                          runat="server" ErrorMessage="Country is required" Display="Dynamic" Text="*"></asp:RequiredFieldValidator>                                    
                       <asp:DropDownCheckBoxes ID="ddlCountry" runat="server" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" 
                        AddJQueryReference="true" UseButtons="True" UseSelectAllNode="True">
                        <Style SelectBoxWidth="200" DropDownBoxBoxWidth="160" DropDownBoxBoxHeight="240" />
                        <Texts SelectBoxCaption="Countries" />                      
                    </asp:DropDownCheckBoxes>
                </EditItemTemplate>
             </asp:TemplateField>

              <asp:TemplateField HeaderText="Year of Production">
                <EditItemTemplate>
                    <asp:TextBox ID="productionYearTextBox" runat="server" Text='<%# Bind("Year") %>' ReadOnly="true" ></asp:TextBox>
                    <asp:RequiredFieldValidator ID="yearReqValidator" ControlToValidate="productionYearTextBox" 
                          runat="server" ErrorMessage="Year is required and in YYYY format" Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
                    <asp:DropDownList ID="DropDownListProductionYear" runat="server" AutoPostBack="True" 
                      onselectedindexchanged="DropDownListProductionYear_SelectedIndexChanged"></asp:DropDownList>
                </EditItemTemplate>
             </asp:TemplateField>

             <asp:TemplateField HeaderText="IPTV Rights">
                   <EditItemTemplate>
                        <asp:TextBox ID="txtBoxIPTVRights" runat="server" Text='<%# Bind("RightsIPTV") %>' ReadOnly="true" ></asp:TextBox>
                        <asp:DropDownCheckBoxes ID="ddlCountriesIPTV" runat="server" OnSelectedIndexChanged="ddlCountriesIPTV_SelectedIndexChanged" 
                AddJQueryReference="true" UseButtons="True" UseSelectAllNode="True">
                <Style SelectBoxWidth="200" DropDownBoxBoxWidth="160" DropDownBoxBoxHeight="240" />
                <Texts SelectBoxCaption="Countries" />                      
            </asp:DropDownCheckBoxes>
                   </EditItemTemplate>
             </asp:TemplateField>

            <asp:TemplateField HeaderText="VOD Rights">
                   <EditItemTemplate>
                        <asp:TextBox ID="txtBoxVODRights" runat="server" Text='<%# Bind("RightsVOD") %>' ReadOnly="true" ></asp:TextBox>
                        <asp:DropDownCheckBoxes ID="ddlCountriesVOD" runat="server" OnSelectedIndexChanged="ddlCountriesVOD_SelectedIndexChanged" 
                AddJQueryReference="true" UseButtons="True" UseSelectAllNode="True">
                <Style SelectBoxWidth="200" DropDownBoxBoxWidth="160" DropDownBoxBoxHeight="240" />
                <Texts SelectBoxCaption="Countries" />                      
            </asp:DropDownCheckBoxes>
                   </EditItemTemplate>
             </asp:TemplateField>

            <asp:TemplateField HeaderText="SVOD Rights">
                   <EditItemTemplate>
                        <asp:TextBox ID="txtBoxSVODRights" runat="server" Text='<%# Bind("SVODRights") %>' ReadOnly="true" ></asp:TextBox>
                        <asp:DropDownList ID="ddlSVODRights" runat="server" 
                            DataTextField="SVODRights" 
                            onselectedindexchanged="ddlSVODRights_SelectedIndexChanged" AutoPostBack="True">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>Yes</asp:ListItem>
                        <asp:ListItem>No</asp:ListItem>
                        </asp:DropDownList>
                   </EditItemTemplate>
             </asp:TemplateField>
                        
            <asp:TemplateField HeaderText="Ancillary Rights">
                   <EditItemTemplate>
                        <asp:TextBox ID="txtBoxAncillaryRights" runat="server" Text='<%# Bind("AncillaryRights") %>' ReadOnly="true" ></asp:TextBox>
                        <asp:DropDownList ID="ddlAncillaryRights" runat="server" 
                            DataTextField="AncillaryRights" 
                            onselectedindexchanged="ddlAncillaryRights_SelectedIndexChanged" AutoPostBack="True">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>Yes</asp:ListItem>
                        <asp:ListItem>No</asp:ListItem>
                        </asp:DropDownList>
                   </EditItemTemplate>
             </asp:TemplateField>
             

            <asp:TemplateField HeaderText="Start Date">
                <EditItemTemplate>
                    <asp:TextBox ID="startDateTextBox" runat="server" Text='<%# Bind("StartDate", "{0:d}") %>' ReadOnly="true" ></asp:TextBox>
                    <asp:Label ID="lblYear" runat="server">Year:</asp:Label>
                         <asp:DropDownList ID="DropDownListYear" runat="server" AutoPostBack="True" 
                               onselectedindexchanged="DropDownListYear_SelectedIndexChanged"></asp:DropDownList>
    
                         <asp:Label ID="lblMonth" runat="server">Month:</asp:Label>
                         <asp:DropDownList ID="DropDownListMonth" runat="server" AutoPostBack="True" 
                           onselectedindexchanged="DropDownListMonth_SelectedIndexChanged"></asp:DropDownList>
                    <asp:ImageButton ID="calendarImage" runat="server" Height="17px" 
                          ImageUrl="~/images/calendar_icon.png" onclick="calendarImage_Click" />
                            <asp:Calendar ID="startDateCalendar" runat="server" Height="22px" Visible="false"
                                ImageUrl="~/images/calendar_icon.png" 
                                
                          onselectionchanged="startDateCalendar_SelectionChanged"></asp:Calendar>
                 </EditItemTemplate>                   
                </asp:TemplateField>

             <asp:TemplateField HeaderText="Expiry Date">
                <EditItemTemplate>             
                    <asp:TextBox ID="expireDateTextBox" runat="server" Text='<%# Bind("ExpireDate", "{0:d}") %>' ReadOnly="true" ></asp:TextBox>
                    
                        <asp:Label ID="lblExpYear" runat="server">Year:</asp:Label>
                        <asp:DropDownList ID="DropDownListExpireYear" runat="server" AutoPostBack="True" 
                               onselectedindexchanged="DropDownListExpireYear_SelectedIndexChanged"></asp:DropDownList>
    
                         <asp:Label ID="lblExpMonth" runat="server">Month:</asp:Label>
                         <asp:DropDownList ID="DropDownListExpireMonth" runat="server" AutoPostBack="True" 
                              onselectedindexchanged="DropDownListExpireMonth_SelectedIndexChanged"></asp:DropDownList>

                    <asp:ImageButton ID="calendarImage2" runat="server" Height="17px" 
                        ImageUrl="~/images/calendar_icon.png" onclick="calendarImage2_Click" />
                            <asp:Calendar ID="expireDateCalendar" runat="server" Height="22px" Visible="False"
                                ImageUrl="~/images/calendar_icon.png" 
                                
                          onselectionchanged="expireDateCalendar_SelectionChanged"></asp:Calendar>
                </EditItemTemplate>
             </asp:TemplateField>

            <asp:DynamicField DataField="Comment" HeaderText="Comment" />
            <asp:CommandField EditText="Edit" ShowEditButton="true" 
                                              UpdateText="Update Movie"
                                              CancelText="Cancel" ShowCancelButton="true" />
         </Fields>   
      </asp:DetailsView>
      
      <br />
      <asp:ValidationSummary ID="AddMovieDetailsViewValidationSummary" runat="server" ShowSummary="true" 
                                DisplayMode="BulletList"/>
      <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

      
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
