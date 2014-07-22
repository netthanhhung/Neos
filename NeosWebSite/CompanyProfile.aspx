<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompanyProfile.aspx.cs" Inherits="CompanyProfile" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Company profile Page</title>
    <link href="Styles/Neos.css" rel="Stylesheet" />

    <script type="text/javascript" src="script/utils.js"></script>

    <telerik:RadScriptBlock runat="server" ID="scriptBlock">

        <script type="text/javascript">
        function companyContactRowSelected(sender, args)
        {    
           var tableView = args.get_tableView(); 
           if(tableView.get_selectedItems().length == 1)
           {
                var contactID = tableView.getCellByColumnUniqueName(tableView.get_selectedItems()[0], "ContactID").innerHTML;
                var dataItem = $get(args.get_id());        
                $find("CompanyProfileAjaxManager").ajaxRequest("RebindContactInfoGrid/" + contactID + "/" + dataItem.rowIndex);
                //document.getElementById("<%=lnkAddContactInfo.ClientID %>").style.display = "inline";
           }
           else
           {
                document.getElementById("<%=lnkAddContactInfo.ClientID %>").style.display = "none";
           }
           return false;
        }
        function onRadTabCompany_ClientLoad(sender)
        {
            var CompanyId = getQueryString("CompanyId");
            var mode = getQueryString("mode");
            if(CompanyId != null && CompanyId != "") //in edit mode
            {                       
                if(mode == "edit")
                    processCompanyToolBar("EditCompanyProfile");
                else if(mode == "view")
                    processCompanyToolBar("ViewCompanyProfile");
            } 
            else if(mode == "edit") 
            {
                processCompanyToolBar("AddCompanyProfile");
            }
           
            return false;
        }
        function onLoadCompanyProfilePage()
        {            
            var CompanyId = getQueryString("CompanyId");
            var mode = getQueryString("mode");
            if(CompanyId != null && CompanyId != "") //in edit mode
            {                        
                if(mode == "edit")
                    processCompanyToolBar("EditCompanyProfile");
                else if(mode == "view")
                    processCompanyToolBar("ViewCompanyProfile");
            }
            else if(mode == "edit") 
            {
                processCompanyToolBar("AddCompanyProfile");
            }
        }
        
        function onSaveOrLoadCompanyProfilePage() 
        {
            var homeAjaxManager = window.parent.$find("homeAjaxManager");
            if(!homeAjaxManager) return;
            homeAjaxManager.ajaxRequest("BindLast5ViewedCompany");
        }
            
        function onform_unload()
        {
            processCompanyToolBar("UnLoadCompanyProfilePage");
        }
        </script>

    </telerik:RadScriptBlock>
</head>
<body onbeforeunload="onform_unload()">
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="ScriptManager" runat="server" />
        <div class="rightpane_title" style="width:100%">
            <table style="width:98%">
                <tr>                                   
                    <td>
                        <asp:Literal runat="server" ID="lblCompanyProfileTitle" Text="Company Profile"></asp:Literal>
                    </td>
                    <td align="right">
                        <asp:LinkButton ID="lnkBack" runat="server"  Text="Back"
                                    OnClick="OnLinkBackClicked"></asp:LinkButton>     
                    </td> 
                </tr>
            </table>  
        </div>        
        <div style="margin-top:30px;">
            <telerik:RadAjaxManager EnableAJAX="true" runat="server" ID="CompanyProfileAjaxManager"
                OnAjaxRequest="OnMyAjaxManagerAjaxRequest">
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="gridActions">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridActions" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="CompanyProfileAjaxManager">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridActions" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                   <%-- <telerik:AjaxSetting AjaxControlID="CompanyProfileAjaxManager">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="ContactInfoGrid" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>--%>
                    <telerik:AjaxSetting AjaxControlID="CompanyProfileAjaxManager">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="lnkAddContactInfo" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    
                    <%--<telerik:AjaxSetting AjaxControlID="CompanyProfileAjaxManager">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="CompanyContactGrid" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>--%>
                    <telerik:AjaxSetting AjaxControlID="CompanyContactGrid">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="CompanyContactGrid" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="CompanyContactGrid">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="ContactInfoGrid" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="CompanyContactGrid">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="lnkAddContactInfo" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                     <telerik:AjaxSetting AjaxControlID="ContactInfoGrid">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="ContactInfoGrid" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="CompanyProfileAjaxManager">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridInvoiceCoordinate" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="gridInvoiceCoordinate">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridInvoiceCoordinate" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="CompanyJobGrid">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="CompanyJobGrid" LoadingPanelID="RadAjaxLoadingPanel" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="grdDocuments">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="grdDocuments" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" runat="server" Height="75px"
                Width="75px" Transparency="50">
                <img alt="Loading..." src='<%= RadAjaxLoadingPanel.GetWebResourceUrl(Page, "Telerik.Web.UI.Skins.Default.Ajax.loading.gif") %>'
                    style="border: 0;" />
            </telerik:RadAjaxLoadingPanel>
            <table width="100%" style="border: none; margin-bottom: 5px;" cellpadding="0" cellspacing="1">
                <tr style="height: 17px;">
                    <td style="font-weight: bold; text-align: left;">
                        <asp:Literal runat="server" ID="lblNameCom" Text="Name"></asp:Literal>:
                        <asp:TextBox runat="server" ID="txtCompanyName" MaxLength="100" BackColor="White"
                            BorderWidth="1" BorderColor="#A8BEDA"></asp:TextBox>
                        <asp:TextBox runat="server" ID="txtCompanyID" ReadOnly="true" Width="30" BackColor="White"
                            BorderWidth="1" BorderColor="#A8BEDA"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="lblNeosResp" Text="Néos responsible"></asp:Literal>:
                        <telerik:RadComboBox runat="server" ID="ddlNeosResp" Skin="Office2007" Width="100">
                        </telerik:RadComboBox>
                    </td>
                    <td style="">
                        <asp:Literal runat="server" ID="lblWebsite" Text="Website"></asp:Literal>
                        <asp:HyperLink runat="server" ID="lnkWebsiteTitle" Text="Website" Visible="false" Target="_blank"></asp:HyperLink>:
                        <asp:HyperLink runat="server" ID="lnkWebsite" Visible="True" Target="_blank"></asp:HyperLink>
                        <asp:TextBox runat="server" ID="txtWebsite" Width="100" MaxLength="200" Visible="false"
                            BackColor="White" BorderWidth="1" BorderColor="#A8BEDA"></asp:TextBox></td>
                    <td nowrap="nowrap" style="">
                        <asp:Literal runat="server" ID="lblLogo" Text="Logo"></asp:Literal>:
                        <asp:MultiView runat="server" ID="MViewLogo" ActiveViewIndex="0">
                            <asp:View runat="server" ID="ViewLogo">
                                <telerik:RadToolTip runat="server" ID="logoTooltip" TargetControlID="lnkLogo" Animation="Resize">
                                    <asp:Image runat="server" ID="imgCompanyLogo" BorderStyle="None" />
                                </telerik:RadToolTip>
                                <asp:HyperLink runat="server" ID="lnkLogo" Text="n/a">
                                </asp:HyperLink>
                            </asp:View>
                            <asp:View runat="server" ID="EditLogo">
                                <asp:FileUpload runat="server" ID="fileCompanyLogo" CssClass="fileupload"></asp:FileUpload>
                                <asp:CheckBox runat="server" ID="chkRemoveLogo" Text="Remove" />
                            </asp:View>
                        </asp:MultiView>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                    </td>
                    <td style="text-align: left">
                    </td>
                </tr>
            </table>
            <telerik:RadTabStrip ID="radTabStripCompany" runat="server" Skin="Web20" BackColor="#A0B8DB" MultiPageID="CompanyMultiPage"
                SelectedIndex="0" CssClass="tabStrip">
                <%--OnClientLoad="onRadTabCompany_ClientLoad"--%>
                <Tabs>
                    <telerik:RadTab Text="Contact Info" Value="ContactInfo">
                    </telerik:RadTab>
                    <telerik:RadTab Text="Client Info" Value="ClientInfo">
                    </telerik:RadTab>
                    <telerik:RadTab Text="Actions" Value="Actions">
                    </telerik:RadTab>
                    <telerik:RadTab Text="Invoice Coordinates" Value="Invoice">
                    </telerik:RadTab>
                    <telerik:RadTab Text="Jobs" Value="Job">
                    </telerik:RadTab>
                    <telerik:RadTab Text="Documents" Value="DocumentView" >
                    </telerik:RadTab>
                </Tabs>
            </telerik:RadTabStrip>
            <telerik:RadMultiPage ID="CompanyMultiPage" runat="server" SelectedIndex="0" CssClass="multiPage">
                <telerik:RadPageView ID="ContactInfoView" runat="server">
                    <table width="99%">
                        <tr>
                            <td style="text-align: left; width: 7%">
                                <asp:Literal runat="server" ID="lblAddress"></asp:Literal></td>
                            <td style="text-align: left; width: 25%" colspan="3">
                                <asp:TextBox runat="server" ID="txtCompanyAddress" Width="99%" MaxLength="255" BackColor="White"
                                    BorderWidth="1" BorderColor="#A8BEDA"></asp:TextBox></td>
                            <td style="text-align: right; width: 7%">
                                <asp:Literal runat="server" ID="lblLegalForm"></asp:Literal></td>
                            <td style="text-align: left; width: 15%">
                                <telerik:RadComboBox runat="server" ID="ddlCompanyLegalForm" Width="99%" Skin="Office2007">
                                </telerik:RadComboBox>
                            </td>
                            <td style="text-align: right; width: 10%">
                                <asp:Literal runat="server" ID="lblGroup"></asp:Literal></td>
                            <td style="text-align: left; width: 15%">
                                <asp:TextBox runat="server" ID="txtCompanyGroup" Width="98%" MaxLength="60" BackColor="White"
                                    BorderWidth="1" BorderColor="#A8BEDA"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:Literal runat="server" ID="lblZipcode"></asp:Literal></td>
                            <td style="text-align: left;">
                                <asp:TextBox runat="server" ID="txtCompanyZipCode" Width="99%" MaxLength="7" BackColor="White"
                                    BorderWidth="1" BorderColor="#A8BEDA"></asp:TextBox></td>
                            <td style="text-align: right; width: 6%">
                                <asp:Literal runat="server" ID="lblLocality"></asp:Literal></td>
                            <td style="text-align: left;">
                                <asp:TextBox runat="server" ID="txtCompanyCity" Width="98%" MaxLength="70" BackColor="White"
                                    BorderWidth="1" BorderColor="#A8BEDA"></asp:TextBox></td>
                            <td style="text-align: right;">
                                <asp:Literal runat="server" ID="lblPhoneArea"></asp:Literal></td>
                            <td style="text-align: left;">
                                <asp:TextBox runat="server" ID="txtCompanyPhoneArea" Width="97%" MaxLength="6" BackColor="White"
                                    BorderWidth="1" BorderColor="#A8BEDA"></asp:TextBox></td>
                            <td style="text-align: right;">
                                <asp:Literal runat="server" ID="lblPhone"></asp:Literal></td>
                            <td style="text-align: left;">
                                <asp:TextBox runat="server" ID="txtCompanyPhone" Width="98%" MaxLength="20" BackColor="White"
                                    BorderWidth="1" BorderColor="#A8BEDA"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:Literal runat="server" ID="lblEmail"></asp:Literal></td>
                            <td style="text-align: left;" colspan="3">
                                <asp:TextBox runat="server" ID="txtCompanyEmail" Width="99%" MaxLength="100" BackColor="White"
                                    BorderWidth="1" BorderColor="#A8BEDA"></asp:TextBox></td>
                            <td style="text-align: right;">
                                <asp:Literal runat="server" ID="lblType"></asp:Literal></td>
                            <td style="text-align: left;">
                                <telerik:RadComboBox runat="server" ID="ddlParamClientStatus" Width="99%" Skin="Office2007">
                                </telerik:RadComboBox>
                            </td>
                            <td style="text-align: right;">
                                <asp:Literal runat="server" ID="lblFax"></asp:Literal></td>
                            <td style="text-align: left;">
                                <asp:TextBox runat="server" ID="txtCompanyFax" Width="98%" MaxLength="20" BackColor="White"
                                    BorderWidth="1" BorderColor="#A8BEDA"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="text-align: right;" colspan="7">
                                <asp:Literal runat="server" ID="lblVATNumber"></asp:Literal></td>
                            <td style="text-align: left;">
                                <asp:TextBox runat="server" ID="txtCompanyVAT" Width="98%" MaxLength="14" BackColor="White"
                                    BorderWidth="1" BorderColor="#A8BEDA"></asp:TextBox></td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td>
                                <div>
                                    <strong>
                                        <asp:Literal runat="server" ID="lblContacts"></asp:Literal></strong></div>
                                <div>
                                    <telerik:RadGrid ID="CompanyContactGrid" GridLines="None" Skin="Office2007" AllowMultiRowSelection="False"
                                        runat="server" AllowPaging="True" AllowSorting="True" PageSize="10" Width="100%"
                                        AutoGenerateColumns="false" OnNeedDataSource="OnCompanyContactGridNeedDataSource"
                                        OnItemDataBound="OnCompanyContactGridItemDataBound" OnPageIndexChanged="OnCompanyContactGridPageIndexChanged"
                                        OnDeleteCommand="OnCompanyContactGridDeleteCommand">
                                        <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                        <MasterTableView DataKeyNames="ContactID" DataMember="CompanyContact" AllowMultiColumnSorting="False"
                                            Width="100%" EditMode="PopUp">
                                            <SortExpressions>
                                                <telerik:GridSortExpression FieldName="LastName" SortOrder="ascending" />
                                            </SortExpressions>
                                            <Columns>
                                                <telerik:GridBoundColumn UniqueName="ContactID" DataField="ContactID" Display="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="LastName" SortExpression="LastName" HeaderText=""
                                                    DataField="LastName">
                                                    <HeaderStyle></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="FirstName" SortExpression="FirstName" HeaderText=""
                                                    DataField="FirstName">
                                                    <HeaderStyle></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="Position" SortExpression="Position" HeaderText=""
                                                    DataField="Position">
                                                    <HeaderStyle></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="Gender" HeaderText="" DataField="Gender">
                                                    <ItemStyle HorizontalAlign="center" />
                                                    <HeaderStyle Width="75" HorizontalAlign="center"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="Language" HeaderText="Language" DataField="Language">
                                                    <HeaderStyle Width="75"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn UniqueName="TemplateEditContactColumn">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkContactEdit" runat="server" Text="Edit">  
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle Width="5%"></HeaderStyle>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridButtonColumn CommandName="Delete" CommandArgument="ContactID" ButtonType="ImageButton"
                                                    UniqueName="DeleteColumn">
                                                    <HeaderStyle Width="5%"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridButtonColumn>                                                
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings>
                                            <Selecting AllowRowSelect="True" />
                                            <ClientEvents OnRowSelected="companyContactRowSelected" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </div>
                                <div style="text-align: center">
                                    <telerik:RadWindow runat="server" ID="radWindowCompanyContact" Skin="Office2007"
                                        VisibleStatusbar="false" ShowContentDuringLoad="false" VisibleOnPageLoad="false"
                                        Modal="true" OffsetElementID="offsetElement" Top="30" Left="30" Height="400px"
                                        Width="400px" NavigateUrl="CompanyContactPopup.aspx" OnClientClose="onClientComContactWindowClosed">
                                    </telerik:RadWindow>
                                    <asp:LinkButton runat="server" ID="lnkAddContact" Text=""></asp:LinkButton></div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div>
                                    <strong>
                                        <asp:Literal runat="server" ID="lblContactInfo"></asp:Literal></strong></div>
                                <div>
                                    <telerik:RadGrid ID="ContactInfoGrid" GridLines="None" Skin="Office2007" AllowMultiRowSelection="False"
                                        runat="server" AllowPaging="False" AllowSorting="False" Width="50%" AutoGenerateColumns="false"
                                        OnNeedDataSource="OnContactInfoGridNeedDataSource" OnItemDataBound="OnContactInfoGridItemDataBound"
                                        OnDeleteCommand="OnContactInfoGridDeleteCommand">
                                        <MasterTableView DataKeyNames="ContactID" DataMember="CompanyContact" AllowMultiColumnSorting="True"
                                            EditMode="PopUp">
                                            <Columns>
                                                <telerik:GridBoundColumn UniqueName="Type" HeaderText="" DataField="Type">
                                                    <HeaderStyle Width="10%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="TelephoneZone" HeaderText="" DataField="TelephoneZone">
                                                    <HeaderStyle Width="15%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="Tel" HeaderText="" DataField="Tel">
                                                    <HeaderStyle Width="35%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="Location" HeaderText="" DataField="Location">
                                                    <HeaderStyle Width="20%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn UniqueName="TemplateEditContactColumn">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkContactEdit" runat="server" Text="Edit"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle Width="5%"></HeaderStyle>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridButtonColumn CommandName="Delete" CommandArgument="ContactID" ButtonType="ImageButton"
                                                    UniqueName="DeleteColumn">
                                                    <HeaderStyle Width="5%"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridButtonColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings>
                                            <Selecting AllowRowSelect="True" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left">
                                <telerik:RadWindow runat="server" ID="radWindowContactInfo" Skin="Office2007" VisibleStatusbar="false"
                                    ShowContentDuringLoad="false" VisibleOnPageLoad="false" Modal="true" OffsetElementID="offsetElement"
                                    Top="30" Left="30" Height="240" Width="310" NavigateUrl="ContactInfoPopup.aspx"
                                    OnClientClose="onClientContactInfoWindowClosed">
                                </telerik:RadWindow>
                                <asp:LinkButton runat="server" ID="lnkAddContactInfo" Visible="false"></asp:LinkButton></td>
                        </tr>
                    </table>
                </telerik:RadPageView>
                <telerik:RadPageView ID="ClientInfoView" runat="server">
                    <fieldset style="margin: 5px 2px 5px 2px;">
                        <table width="100%">
                            <tr style="vertical-align: top;">
                                <td style="width: 5%; text-align: left">
                                    <asp:Literal runat="server" ID="lblActivity" Text="Activity"></asp:Literal>:</td>
                                <td style="text-align: right; width: 60%">
                                    <asp:TextBox runat="server" ID="txtActivity" TextMode="MultiLine" Width="98%" Height="250"
                                        BackColor="White" BorderWidth="1" BorderColor="#A8BEDA"></asp:TextBox>
                                </td>
                                <td style="width: 5%">
                                </td>
                                <td style="width: 30%; text-align: left;">
                                    <div style="float: left; width: 100%">
                                        <div style="float: left; width: 40%; text-align: left">
                                            <asp:Literal runat="server" ID="lblCreatedDate" Text="Created date"></asp:Literal>:</div>
                                        <div style="float: left; width: 60%; text-align: left">
                                            <telerik:RadDatePicker ID="datCreatedDate" runat="server" Width="100px" Skin="Office2007"  Calendar-CultureInfo="en-US">
                                                <DateInput ID="dateInputCreatedDate" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd/MM/yyyy">
                                                </DateInput>
                                            </telerik:RadDatePicker>
                                        </div>
                                    </div>
                                    <br />
                                    <div style="float: left; width: 100%;">
                                        <div style="float: left; width: 40%; text-align: left">
                                            <asp:Literal runat="server" ID="lblUnit" Text="Unit"></asp:Literal>:</div>
                                        <div style="float: left; width: 60%; text-align: left">
                                            <telerik:RadComboBox runat="server" ID="ddlUnit" Skin="Office2007" Width="99%">
                                            </telerik:RadComboBox>
                                        </div>
                                    </div>
                                    <br />
                                    <div style="float: left; width: 100%;">
                                        <div style="float: left; width: 40%; text-align: left">
                                            <asp:Literal runat="server" ID="lblSponsor" Text="Sponsor"></asp:Literal>:</div>
                                        <div style="float: left; width: 60%; text-align: left">
                                            <asp:CheckBox runat="server" ID="chkSponsor" /></div>
                                    </div>
                                </td>
                            </tr>
                            <tr style="vertical-align: top">
                                <td style="width: 7%; text-align: left">
                                    <asp:Literal runat="server" ID="lblRemark" Text="Remark"></asp:Literal>:</td>
                                <td style="text-align: right; width: 53%">
                                    <asp:TextBox runat="server" ID="txtRemark" TextMode="MultiLine" Width="98%" Height="250"
                                        BackColor="White" BorderWidth="1" BorderColor="#A8BEDA"></asp:TextBox>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </telerik:RadPageView>
                <telerik:RadPageView ID="ActionView" runat="server">
                    <div>
                        <table width="99%">
                            <tr>
                                <td>
                                    <telerik:RadGrid ID="gridActions" GridLines="None" Skin="Office2007" AllowMultiRowSelection="True"
                                        runat="server" AllowPaging="True" AllowSorting="True" PageSize="5" Width="100%"
                                        AutoGenerateColumns="false" OnItemDataBound="OnGridActionItemDataBound" OnPageIndexChanged="OnGridActionPageIndexChanged"
                                        OnNeedDataSource="OnGridActionNeedDataSource">
                                        <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                        <MasterTableView DataKeyNames="ActionId" DataMember="Action" AllowMultiColumnSorting="True"
                                            Width="100%">
                                            <Columns>
                                                <telerik:GridBoundColumn UniqueName="Active" SortExpression="Actif" HeaderText="Active"
                                                    DataField="Actif">
                                                    <HeaderStyle Width="5%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="TaskNbr" SortExpression="ActionId" HeaderText="Task nbr"
                                                    DataField="ActionId">
                                                    <HeaderStyle Width="7%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="DateAction" SortExpression="DateAction" HeaderText="Date"
                                                    DataField="DateAction" DataType="system.datetime" DataFormatString="{0:dd/MM/yyyy}">
                                                    <HeaderStyle Width="8%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="Hour" SortExpression="Hour" HeaderText="Hour"
                                                    DataField="Hour" DataType="system.datetime" DataFormatString="{0:hh:mm tt}">
                                                    <HeaderStyle Width="13%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="TypeAction" SortExpression="TypeActionLabel"
                                                    HeaderText="Type" DataField="TypeActionLabel">
                                                    <HeaderStyle Width="10%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="Candidate" SortExpression="CandidateFullName"
                                                    HeaderText="Candidate" DataField="CandidateFullName">
                                                    <HeaderStyle Width="12%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="Descripton" SortExpression="DescrAction" HeaderText="Descripton"
                                                    DataField="DescrAction">
                                                    <HeaderStyle Width="25%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="ResponsableName" SortExpression="ResponsableName"
                                                    HeaderText="Responsible" DataField="ResponsableName">
                                                    <HeaderStyle Width="10%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn UniqueName="TemplateEditComActionColumn">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkComActionEdit" runat="server" Text="Edit" CommandArgument='<%# Eval("ActionId") %>'
                                                            OnClientClick='<%# Eval("ActionId", "return OnCompanyActionEditClientClicked({0});") %>'>  
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle Width="5%"></HeaderStyle>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="TemplateDeleteComActionColumn">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkComActionDelete" runat="server" Text="Delete" CommandArgument='<%# Eval("ActionId") %>'
                                                            OnClientClick="return confirm('Are you sure to delete this item?')" OnClick="OnCompanyActionDeleteClicked">
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle Width="5%"></HeaderStyle>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="TemplateExportActionColumn">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkExportAction" runat="server" Text="Export" CommandArgument='<%# Eval("ActionId") %>'
                                                            OnClientClick="return confirm('Are you sure to export this action to Outlook ?')" OnClick="OnActionExportClicked">
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle Width="5%"></HeaderStyle>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings Selecting-AllowRowSelect="true">
                                            <Selecting AllowRowSelect="false" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                    <telerik:RadWindow runat="server" ID="radWindowComAction" Skin="Office2007" VisibleOnPageLoad="false"
                                        VisibleStatusbar="false" Modal="true" OffsetElementID="offsetElement" Top="30"
                                        Left="30" NavigateUrl="ComCanActionPopup.aspx" Title="Company Action" Height="500px"
                                        Width="850px" OnClientClose="onClientComActionDetailWindowClosed">
                                    </telerik:RadWindow>
                                </td>
                            </tr>
                            <tr align="center">
                                <td>
                                    <asp:LinkButton ID="lnkAddNewAction" runat="server" Text="Add new action" OnClientClick="return OnAddNewComActionClientClicked();"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                </telerik:RadPageView>
                <telerik:RadPageView ID="InvoiceCoordinatesView" runat="server">
                    <div>
                        <table width="99%">
                            <tr>
                                <td>
                                    <telerik:RadGrid ID="gridInvoiceCoordinate" GridLines="None" Skin="Office2007" AllowMultiRowSelection="True"
                                        runat="server" AllowPaging="True" AllowSorting="True" PageSize="20" Width="100%"
                                        AutoGenerateColumns="false" OnItemDataBound="OnGridInvoiceCoordinateItemDataBound"
                                        OnPageIndexChanged="OnGridInvoiceCoordinatePageIndexChanged" OnNeedDataSource="OnGridInvoiceCoordinateNeedDataSource">
                                        <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                        <MasterTableView DataKeyNames="AddressID" DataMember="CompanyAddress" AllowMultiColumnSorting="True"
                                            Width="100%">
                                            <Columns>
                                                <telerik:GridBoundColumn UniqueName="Name" SortExpression="Name" HeaderText="Name"
                                                    DataField="Name">
                                                    <HeaderStyle Width="15%"></HeaderStyle>
                                                </telerik:GridBoundColumn>                                               
                                                <telerik:GridBoundColumn UniqueName="Address" SortExpression="Address" HeaderText="Address"
                                                    DataField="Address">
                                                    <HeaderStyle Width="16%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="ZipCode" SortExpression="ZipCode" HeaderText="Zip code"
                                                    DataField="ZipCode">
                                                    <HeaderStyle Width="7%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="City" SortExpression="City" HeaderText="City"
                                                    DataField="City">
                                                    <HeaderStyle Width="10%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="VatNumber" SortExpression="VatNumber" HeaderText="VAT Number"
                                                    DataField="VatNumber">
                                                    <HeaderStyle Width="10%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="Telephone" SortExpression="Telephone" HeaderText="Telephone"
                                                    DataField="Telephone">
                                                    <HeaderStyle Width="8%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="Fax" SortExpression="Fax" HeaderText="Fax" DataField="Fax">
                                                    <HeaderStyle Width="8%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="Email" SortExpression="Email" HeaderText="Email"
                                                    DataField="Email">
                                                    <HeaderStyle Width="13%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="IsDefault" SortExpression="IsDefault" HeaderText="Default"
                                                    DataField="IsDefault">
                                                    <HeaderStyle Width="5%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn UniqueName="TemplateEditInvoiceCoordinateColumn">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkInvoiceCoordinateEdit" runat="server" Text="Edit" CommandArgument='<%# Eval("AddressID") %>'
                                                            OnClientClick='<%# Eval("AddressID", "return OnInvoiceCoordinateEditClientClicked({0});") %>'>  
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle Width="4%"></HeaderStyle>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="TemplateDeleteInvoiceCoordinateColumn">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkInvoiceCoordinateDelete" runat="server" Text="Delete" CommandArgument='<%# Eval("AddressID") %>'
                                                            OnClientClick="return confirm('Are you sure to delete this item?')" OnClick="OnInvoiceCoordinateDeleteClicked">
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle Width="4%"></HeaderStyle>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings Selecting-AllowRowSelect="true">
                                            <Selecting AllowRowSelect="false" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                    <telerik:RadWindow runat="server" ID="radWindowInvoiceCoordinate" Skin="Office2007"
                                        VisibleOnPageLoad="false" VisibleStatusbar="false" Modal="true" OffsetElementID="offsetElement"
                                        Top="30" Left="30" NavigateUrl="InvoiceCoordinatePopup.aspx" Title="Invoice Coordinates"
                                        Height="500px" Width="700px" OnClientClose="onClientInvoiceCoordinateDetailWindowClosed">
                                    </telerik:RadWindow>
                                </td>
                            </tr>
                            <tr align="center">
                                <td>
                                    <asp:LinkButton ID="lnkAddNewInvoiceCoordinate" runat="server" Text="Add New Invoice Coordinate"
                                        OnClientClick="return OnAddNewInvoiceCoordinateClientClicked();"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="JobView">
                    <telerik:RadGrid ID="CompanyJobGrid" GridLines="None" Skin="Office2007" AllowMultiRowSelection="True"
                        runat="server" AllowPaging="True" AllowSorting="true" PageSize="25" Width="100%"
                        AutoGenerateColumns="false" EnableAjaxSkinRendering="true" OnNeedDataSource="OnCompanyJobGridNeedDataSource"
                        OnDeleteCommand="OnCompanyJobGrid_DeleteCommand" OnPageIndexChanged="OnCompanyJobGrid_PageIndexChanged"
                        OnItemDataBound="OnCompanyJobGrid_ItemDataBound">
                        <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                        <MasterTableView DataKeyNames="JobID" DataMember="Job" AllowMultiColumnSorting="False"
                            Width="100%">
                            <SortExpressions>
                                <telerik:GridSortExpression FieldName="Title" SortOrder="Ascending" />
                            </SortExpressions>
                            <Columns>
                                <telerik:GridTemplateColumn UniqueName="Title" HeaderText="Title" SortExpression="Title">
                                    <ItemTemplate>
                                        <asp:HyperLink runat="server" ID="lnkJobTitle" Text='<%#Eval("Title") %>' ></asp:HyperLink>
                                        &nbsp;
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn UniqueName="CompanyName" DataField="CompanyName" SortExpression="CompanyName"
                                    HeaderText="CompanyName">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="Location" DataField="Location" SortExpression="Location"
                                    HeaderText="Location">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="NrOfVisites" DataField="NrOfVisites" SortExpression="NrOfVisites"
                                    HeaderText="Visits">
                                    <HeaderStyle HorizontalAlign="center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="CreatedDate" DataField="CreatedDate" SortExpression="CreatedDate"
                                    HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy}">
                                    <HeaderStyle HorizontalAlign="center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="ExpiredDate" DataField="ExpiredDate" SortExpression="ExpiredDate"
                                    HeaderText="Expired Date" DataFormatString="{0:dd/MM/yyyy}">
                                    <HeaderStyle HorizontalAlign="center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridButtonColumn CommandName="Delete" CommandArgument="JobID" ButtonType="ImageButton"
                                    UniqueName="DeleteColumn">
                                    <HeaderStyle Width="5%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridButtonColumn>
                                <telerik:GridBoundColumn UniqueName="JobID" DataField="JobID" Display="false">
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                        <ClientSettings>
                            <Selecting AllowRowSelect="True" />
                        </ClientSettings>
                    </telerik:RadGrid>
                    <div style="text-align: center;">
                        <table width="100%">
                            <tr>
                                <td align="center">
                                    <asp:HyperLink ID="lnkAddNewJob" runat="server" Text="Add New Job"></asp:HyperLink></td>
                            </tr>
                        </table>
                    </div>
                </telerik:RadPageView>
                <telerik:RadPageView ID="DocumentView" runat="server">
                    <telerik:RadGrid ID="grdDocuments" GridLines="None" Skin="Office2007" AllowMultiRowSelection="True" EnableAjaxSkinRendering="true"
                        runat="server" AllowPaging="True" AllowSorting="true" PageSize="20" Width="100%" 
                        AutoGenerateColumns="false" OnItemDataBound="OnGridDocumentsItemDataBound"
                        OnDeleteCommand="OnGridDocumentsDeleteCommand" OnPageIndexChanged="OnGridDocumentsPageIndexChanged"
                        OnNeedDataSource="OnGridDocumentsNeedDataSource"
                        ><%--   OnItemCommand="OnGridDocumentsItemCommand"--%>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                        <MasterTableView DataKeyNames="DocumentID" DataMember="CompanyDocument" AllowMultiColumnSorting="False"
                            Width="100%">
                            <Columns>                                        
                                <telerik:GridTemplateColumn UniqueName="DocumentName">
                                    <ItemTemplate>
                                        <%--<asp:HyperLink runat="server" ID="lnkDocumentName" Text='<%# Eval("DocumentName") %>' NavigateUrl='<%# Eval("AbsoluteURL") %>'></asp:HyperLink>--%>
                                        <%--<asp:LinkButton runat="server" ID="lnkDocumentName" Text='<%# Eval("DocumentName") %>' CommandName="viewdocument"></asp:LinkButton>--%>
                                        <asp:LinkButton runat="server" ID="lnkDocumentName" Text='<%# Eval("DocumentName") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle Width="30%"></HeaderStyle>
                                </telerik:GridTemplateColumn>
                                
                                <telerik:GridBoundColumn UniqueName="DocumentLegend" SortExpression="DocumentLegend"
                                    HeaderText="Legend" DataField="DocumentLegend">
                                    <HeaderStyle Width="30%"></HeaderStyle>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="CreatedDate" SortExpression="CreatedDate" HeaderText="Created Date">
                                    <HeaderStyle Width="20%"></HeaderStyle>
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn UniqueName="TemplateEditComContactColumn">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkComContactEdit" runat="server" Text="Edit" CommandArgument='<%# Eval("DocumentID") %>'
                                            OnClientClick='<%# Eval("DocumentID", "return OnComDocumentEditClientClicked(\"\",{0});") %>'> 
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                    <HeaderStyle Width="10%"></HeaderStyle>
                                </telerik:GridTemplateColumn>
                                <telerik:GridButtonColumn CommandName="Delete" ButtonType="ImageButton" UniqueName="DeleteColumn">
                                    <HeaderStyle Width="10%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridButtonColumn>
                            </Columns>
                        </MasterTableView>
                        <ClientSettings>
                            <Selecting AllowRowSelect="false" />
                        </ClientSettings>
                    </telerik:RadGrid>
                    <telerik:RadWindow runat="server" ID="radWindowComDocument" Skin="Office2007" VisibleStatusbar="false"
                        ShowContentDuringLoad="true" VisibleOnPageLoad="false" Modal="true" OffsetElementID="offsetElement"
                        Top="30" NavigateUrl="CompanyDocumentPopup.aspx" Title="Company documents"
                        Height="250px" Width="420px" OnClientClose="onClientRadComDocumentWindowClosed">
                    </telerik:RadWindow>
                    <div style="text-align: center;">
                        <asp:HyperLink runat="server" ID="lnkAddNewDocument" NavigateUrl="javascript:;"
                            Visible="false"></asp:HyperLink></div>
                </telerik:RadPageView>
            </telerik:RadMultiPage>
                <table width="100%">
                    <tr>                        
                        <td align="center">
                            <asp:Button runat="server" ID="btnEditSave" Text="Edit" CssClass="flatButton" Width="60"
                                OnClick="onButtonCompanyEditSaveClicked" />&nbsp;
                            <asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="flatButton" Width="60"
                                OnClick="OnButtonCompanyCancelClicked" />
                        </td>
                        <td align="right" style="width: 150px">
                            <asp:LinkButton ID="lnkBackToAction" runat="server" Text="Back to action list" Visible="false"
                                OnClick="OnLinkBackToActionClicked"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hidActionUrl" runat="server" />
                <asp:HiddenField ID="hidMode" runat="server" />           
        </div>
    </form>
</body>
</html>
