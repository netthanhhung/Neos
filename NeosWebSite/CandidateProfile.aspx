<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CandidateProfile.aspx.cs"  EnableEventValidation="false" Inherits="CandidateProfile" %>
<%@ Register TagPrefix="ew" Namespace="eWorld.UI" Assembly="eWorld.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Candidate Profile Page</title>
    <link href="Styles/Neos.css" rel="Stylesheet" />
    <script type="text/javascript" src="script/utils.js"></script>
    <telerik:RadScriptBlock runat="server" ID="scriptBlock">
            <script type="text/javascript">
            function OnCompanyClientSelectedIndexChanged(sender, eventArgs)
            {                                        
               $find("MyAjaxManager").ajaxRequest("RebindPresentationContactGrid");
            }
            function onCandidateProfile_ClientLoad(sender)
            {                
                var candidateId = getQueryString("CandidateID");
                var mode = getQueryString("mode");
                if(candidateId != null && candidateId != "") //in edit mode
                {                    
                    if(mode == "edit")
                    {
                        processCandidateToolBar("EditCandidateProfile");
                    }                    
                    else if(mode == "view")
                    {
                        processCandidateToolBar("ViewCandidateProfile");
                    }                
                }
                else if(mode == "edit")
                {
                    processCandidateToolBar("AddCandidateProfile");
                }
               
                return false;
            }
            
            function onLoadCandidateProfilePage()
            {                           
                var candidateId = getQueryString("CandidateID");
                var mode = getQueryString("mode");
                if(candidateId != null && candidateId != "") //in edit mode
                {                    
                    if(mode == "edit")
                    {
                        processCandidateToolBar("EditCandidateProfile");
                    }                    
                    else if(mode == "view")
                    {
                        processCandidateToolBar("ViewCandidateProfile");
                    }
                }
                else if(mode == "edit")
                {
                    processCandidateToolBar("AddCandidateProfile");
                }
                return false;
            }
            
            function onSaveOrLoadCandidateProfilePage() 
            {
                var homeAjaxManager = window.parent.$find("homeAjaxManager");
                if(!homeAjaxManager) return;
                homeAjaxManager.ajaxRequest("BindLast5ViewedCandidate");
            }
            
            function onform_unload()
            {
                processCandidateToolBar("UnLoadCandidateProfilePage");
            }
            function processToolBar(str)
            {
                var parentWindow = window.parent;
                var thisWindow = window;
                var radAjaxManagerObject = parentWindow["HomeAjaxManager"];
                if(!radAjaxManagerObject) return null;
                radAjaxManagerObject.ajaxRequest(str);
            }
            function OnTabStripCandidateProfile_TabSelecting(sender, args)
            {
                
                if (args.get_tab().get_pageViewID())
                {
                    args.get_tab().set_postBack(false);
                }
                else
                {
                    var tab = args.get_tab();
                    args.get_tab().set_pageViewID(tab.get_value());
                    var ajaxObj = $find("MyAjaxManager");
                    if(!ajaxObj) return;
                        ajaxObj.ajaxRequest("radTabClick_" + tab.get_value());
                }
                
            }
                
            function onDropdownCompany_ClientItemRequesting(sender, eventArgs)
            {
                var combo = $find("<%= ddlCompany.ClientID %>");
                var text = combo.get_text();
                if(text.length > 0)
                {                
                    eventArgs.set_cancel(false);
                }
                else
                {
                    eventArgs.set_cancel(true);
                }
            }
                        
            </script>
    </telerik:RadScriptBlock>
</head>
<body onbeforeunload="onform_unload()">
    <form id="CandidateProfileForm" runat="server">        
        <telerik:RadScriptManager ID="ScriptManager" runat="server"/>        
        <div class="rightpane_title" style="width:100%">
            <table style="width:98%">
                <tr>                                   
                    <td>
                        <asp:Literal runat="server" ID="lblCandidateProfileTitle" Text="Candidate Profile"></asp:Literal>
                    </td>
                    <td align="right">
                        <asp:LinkButton ID="lnkBack" runat="server"  Text="Back"
                                    OnClick="OnLinkBackClicked"></asp:LinkButton>     
                    </td> 
                </tr>
            </table>            
        </div>        
        <div style="margin-top:50px;">
            <telerik:RadAjaxManager EnableAJAX="true" runat="server" ID="MyAjaxManager" OnAjaxRequest="OnMyAjaxManagerAjaxRequest">
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="ddlCanExpectUnit">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="ddlCanExpectFam" />
                            <telerik:AjaxUpdatedControl ControlID="listExpectOriginal" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>                    
                    <telerik:AjaxSetting AjaxControlID="ddlCanExpectFam">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="listExpectOriginal" />
                        </UpdatedControls>                   
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="btnCanExpectAdd">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridCanExpectDestination" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="btnCanExpectRemove">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridCanExpectDestination" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>   
                    
                    <telerik:AjaxSetting AjaxControlID="ddlKnowFuncUnit">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="ddlKnowFuncFam" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="ddlKnowFuncUnit">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="listKnowFuncOriginal" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="ddlKnowFuncFam">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="listKnowFuncOriginal" />
                        </UpdatedControls>                   
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="btnKnowFuncAdd">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridKnowFuncDestination" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="btnKnowFuncRemove">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridKnowFuncDestination" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>   
                    
                    <telerik:ajaxsetting ajaxcontrolid="gridcontact">
                        <updatedcontrols>
                            <telerik:ajaxupdatedcontrol controlid="gridcontact" />
                        </updatedcontrols>
                    </telerik:ajaxsetting>
                    <telerik:AjaxSetting AjaxControlID="expectancyGrid">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="expectancyGrid" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="gridStudies">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridStudies" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="gridExperience">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridExperience" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="grdPresentationContacts">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="grdPresentationContacts" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                     <telerik:AjaxSetting AjaxControlID="grdComDocuments">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="grdComDocuments" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="grdPresentationAttachedDocs">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="grdPresentationAttachedDocs" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="gridActions">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridActions" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="grdDocuments">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="grdDocuments" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>            
                    <%--<telerik:AjaxSetting AjaxControlID="MyAjaxManager">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadMultiPage1" LoadingPanelID="RadAjaxLoadingPanel"/>
                        </UpdatedControls>
                    </telerik:AjaxSetting> --%>           
                </AjaxSettings>
            </telerik:RadAjaxManager>
            <telerik:RadAjaxLoadingPanel ID="pnlRadAjaxLoading" runat="server"  Height="100">
                <img alt="Loading..." src='<%= RadAjaxLoadingPanel.GetWebResourceUrl(Page, "Telerik.Web.UI.Skins.Default.Ajax.loading7.gif") %>'
                    style="border: 0;margin-bottom:10px;" />
            </telerik:RadAjaxLoadingPanel>
            <div runat="server" id="contentPanel">
                <div>
                    <table cellpadding="0" cellspacing="0" width="100%" style="border: none; margin-bottom: 5px;">
                        <tr style="font-size: 10pt;">
                            <td align="left" style="">
                                <asp:Literal runat="server" ID="lblCanLastName" Text="Last name"></asp:Literal>:</td>
                            <td align="left" style="">
                                <strong>
                                    <telerik:RadTextBox runat="server" ID="txtLastName" Width="90" BackColor="White">
                                    </telerik:RadTextBox></strong></td>
                            <td align="left" style="">
                                <asp:Literal runat="server" ID="lblCanFirstName" Text="First name"></asp:Literal>:</td>
                            <td align="left" style="">
                                <strong>
                                    <telerik:RadTextBox runat="server" ID="txtFirstName" Width="90" BackColor="White">
                                    </telerik:RadTextBox></strong></td>
                            <td align="left" style="">
                                <asp:Literal runat="server" ID="lblCanUnit" Text="Unit"></asp:Literal></td>
                            <td align="left" style="">
                                <telerik:RadComboBox ID="ddlUnit" runat="Server" Skin="Office2007" Width="130">
                                    <Items>
                                        <telerik:RadComboBoxItem runat="server" Value="1" Text="Engineering" />
                                        <telerik:RadComboBoxItem runat="server" Value="1" Text="Longer Infomation" />
                                        <telerik:RadComboBoxItem runat="server" Value="1" Text="Director" />
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                            <td align="left" style="">
                                <asp:Literal runat="server" ID="lblCanInterviewer" Text="Interviewer"></asp:Literal></td>
                            <td align="left" style="">
                                <telerik:RadComboBox ID="ddlInterviewer" runat="Server" Skin="Office2007" Width="140">
                                </telerik:RadComboBox>
                            </td>
                            <td align="left" style="">
                                <asp:Literal runat="server" ID="lblDateInterview" Text="Date Interview"></asp:Literal></td>
                            <td align="left" style="">
                                <telerik:RadDatePicker ID="datDateInterview" runat="server" MinDate="0001-01-01"   Calendar-CultureInfo="en-US"
                                    Skin="Office2007" Width="100">
                                    <DateInput ID="dateInputDateInterview" runat="server" Skin="Office2007" DateFormat="dd/MM/yyyy"  BackColor="White"
                                        DisplayDateFormat="dd/MM/yyyy">
                                    </DateInput>
                                </telerik:RadDatePicker>
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <telerik:RadTabStrip ID="radTabStripCandidateProfile" runat="server" Skin="Web20" BackColor="#A0B8DB" MultiPageID="CandidateProfileMultiPage"
                        SelectedIndex="0" CssClass="tabStrip" OnClientTabSelecting="OnTabStripCandidateProfile_TabSelecting" 
                         
                        ><%--OnClientLoad="onCandidateProfile_ClientLoad" --%>
                        <Tabs>
                            <telerik:RadTab Text="General" Value="GeneralView">
                            </telerik:RadTab>
                            <telerik:RadTab Text="Expectancies" Value="ExpectancyView" >
                            </telerik:RadTab>
                            <telerik:RadTab Text="Studies & Experience" Value="StudyExperienceView" >
                            </telerik:RadTab>
                            <telerik:RadTab Text="Evaluation" Value="EvaluationView" >
                            </telerik:RadTab>
                            <telerik:RadTab Text="Knowledge & Functions" Value="KnowledgeView" >
                            </telerik:RadTab>
                            <telerik:RadTab Text="Presentation" Value="PresentationView" >
                            </telerik:RadTab>
                            <telerik:RadTab Text="Actions" Value="ActionView" >
                            </telerik:RadTab>
                            <telerik:RadTab Text="Documents" Value="DocumentView" >
                            </telerik:RadTab>
                        </Tabs>
                    </telerik:RadTabStrip>
                    <telerik:RadMultiPage ID="CandidateProfileMultiPage" runat="server" SelectedIndex="0" CssClass="multiPage">
                        <telerik:RadPageView ID="GeneralView" runat="server">
                            <div>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <div>
                                                            <table width="100%" cellpadding="3">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblCanAddress" runat="server" Text="Address" />
                                                                    </td>
                                                                    <td colspan="3">
                                                                        <asp:TextBox ID="txtAddress" runat="server" Width="99%" MaxLength="255" BackColor="White" BorderStyle="Solid" BorderColor="#A8BEDA" BorderWidth="1"/>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblCanZip" runat="server" Text="Zip" />
                                                                    </td>
                                                                    <td>
                                                                        <%--<telerik:RadNumericTextBox ID="txtZip" runat="server" Type="Number" MaxLength="10"
                                                                            Width="150" Skin="Office2007" NumberFormat-DecimalDigits="0" NumberFormat-PositivePattern="n"
                                                                            NumberFormat-GroupSeparator="" BackColor="White"  BorderColor="#A8BEDA" />--%>
                                                                            <ew:NumericBox ID="txtZip" runat="server" BackColor="White" BorderWidth="1" BorderColor="#A8BEDA" ></ew:NumericBox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblCanCity" runat="server" Text="City" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtCity" runat="server" MaxLength="50" BackColor="White"  BorderStyle="Solid" BorderColor="#A8BEDA" BorderWidth="1"/>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblCanCountry" runat="server" Text="Country" />
                                                                    </td>
                                                                    <td colspan="3">
                                                                        <telerik:RadComboBox ID="ddlCountry" runat="server" Width="150" Skin="Office2007" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblCanGenre" runat="server" Text="Genre" />
                                                                    </td>
                                                                    <td colspan="3">
                                                                        <telerik:RadComboBox ID="ddlGenre" runat="server" Width="150" Skin="Office2007" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblCanNationality" runat="server" Text="Nationality" />
                                                                    </td>
                                                                    <td colspan="3">
                                                                        <telerik:RadComboBox ID="ddlNationality" runat="server" Width="150" Skin="Office2007" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblCanDateOfBirth" runat="server" Text="Date of birth" />
                                                                    </td>
                                                                    <td>
                                                                       <div runat="server" id="divDateOfBirthDatePicker">
                                                                       <telerik:RadDatePicker ID="datDateOfBirth" runat="server" MinDate="0001-01-01" Skin="Office2007"  Calendar-CultureInfo="en-US">
                                                                            <DateInput ID="dateInputDateOfBirth" runat="server" Skin="Office2007" DateFormat="dd/MM/yyyy" BackColor="White"
                                                                                DisplayDateFormat="dd/MM/yyyy" OnClientDateChanged="OnBirthDateClientDataChanged">
                                                                            </DateInput>
                                                                            <Calendar runat="server" ID="calBirthDate"></Calendar>
                                                                            <DatePopupButton runat="server" />
                                                                        </telerik:RadDatePicker>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblCanAge" runat="server" Text="Age" />
                                                                    </td>
                                                                    <td>
                                                                        <telerik:RadNumericTextBox ID="txtAge" runat="server" Width="30" Type="Number" Skin="Office2007"
                                                                            Enabled="false" NumberFormat-DecimalDigits="0" NumberFormat-PositivePattern="n"
                                                                            NumberFormat-GroupSeparator="" BorderStyle="Solid" BorderColor="#A8BEDA" BorderWidth="1"/>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <telerik:RadGrid ID="gridContact" GridLines="None" 
                                                            Skin="Office2007" runat="server" AllowPaging="True" PageSize="5"
                                                            Width="100%" AutoGenerateColumns="false" OnItemDataBound="OnGridContactItemDataBound"
                                                            OnPageIndexChanged="OnGridContactPageIndexChanged" 
                                                            OnNeedDataSource="OnGridContactNeedDataSource" EnableAjaxSkinRendering="true">
                                                            <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                            <MasterTableView DataKeyNames="TelePhoneId" DataMember="CandidateTelephone" AllowMultiColumnSorting="True"
                                                                Width="100%" EditMode="PopUp">
                                                                <Columns>
                                                                    <telerik:GridBoundColumn UniqueName="Type" SortExpression="TypeLabel" HeaderText="Type"
                                                                        DataField="TypeLabel">
                                                                        <HeaderStyle Width="10%"></HeaderStyle>
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn UniqueName="Zone" SortExpression="PhoneArea" HeaderText="Zone"
                                                                        DataField="PhoneArea">
                                                                        <HeaderStyle Width="10%"></HeaderStyle>
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn UniqueName="PhoneMail" SortExpression="PhoneMail" HeaderText="Phone/Mail"
                                                                        DataField="PhoneMail">
                                                                        <HeaderStyle Width="30%"></HeaderStyle>
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn UniqueName="Place" SortExpression="Location" HeaderText="Place"
                                                                        DataField="Location">
                                                                        <HeaderStyle Width="30%"></HeaderStyle>
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridTemplateColumn UniqueName="TemplateEditCanContactColumn">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkCanContactEdit" runat="server" Text="Edit" CommandArgument='<%# Eval("TelePhoneId") %>'
                                                                                OnClientClick='<%# Eval("TelePhoneId", "return OnCandidateContactEditClientClicked({0});") %>'>  
                                                                            </asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                        <HeaderStyle Width="10%"></HeaderStyle>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn UniqueName="TemplateDeleteCanContactColumn">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkCanContactDelete" runat="server" Text="Delete" CommandArgument='<%# Eval("TelePhoneId") %>'
                                                                                OnClientClick="return confirm('Are you sure to delete this item?')" OnClick="OnCandidateContactDeleteClicked">
                                                                            </asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                        <HeaderStyle Width="10%"></HeaderStyle>
                                                                    </telerik:GridTemplateColumn>
                                                                </Columns>
                                                            </MasterTableView>
                                                            <ClientSettings>
                                                                <Selecting AllowRowSelect="true" />
                                                            </ClientSettings>
                                                        </telerik:RadGrid>
                                                        <telerik:RadWindow runat="server" ID="radWindowCanContact" Skin="Office2007" VisibleStatusbar="false"
                                                            ShowContentDuringLoad="true" VisibleOnPageLoad="false" Modal="true" OffsetElementID="offsetElement"
                                                            Top="30" Left="30" NavigateUrl="CandidateTelephonePopup.aspx" Title="Candidate Telephone"
                                                            Height="200px" Width="300px" OnClientClose="onClientCanContactDetailWindowClosed">
                                                        </telerik:RadWindow>
                                                    </td>
                                                </tr>
                                                <tr align="center">
                                                    <td>
                                                        <asp:LinkButton ID="lnkCanContactAdd" runat="server" Text="Add Candidate Telephone"
                                                             />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="vertical-align: top;">
                                            <table width="100%">
                                                <tr>
                                                    <td nowrap="nowrap">
                                                    <div runat='server' id='divCreateDatePicker' nowrap="nowrap">
                                                        <asp:Label ID="lblCanCreationDate" runat="server" Text="Creation Date" />
                                                        <telerik:RadDatePicker ID="datCreationDate" runat="server" Width="100px" Skin="Office2007"  Calendar-CultureInfo="en-US">
                                                            <DateInput ID="dateInputCreationDate" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd/MM/yyyy"  BackColor="White">
                                                            </DateInput>
                                                            <Calendar runat="server" ID="calCreationDate"></Calendar>
                                                           <DatePopupButton runat="server"/>
                                                        </telerik:RadDatePicker>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblCanStatus" runat="server" Text="Status" />
                                                        <telerik:RadComboBox ID="ddlStatus" runat="server" Skin="Office2007" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblCanInactivityReason" runat="server" Text="Inactivity reason" /><br />
                                                        <asp:TextBox ID="txtInactivityReason" runat="server" Rows="4" TextMode="multiLine" BackColor="White"
                                                            Width="99%"  BorderStyle="Solid" BorderColor="#A8BEDA" BorderWidth="1"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblCanRemarkGeneral" runat="server" Text="Remark" /><br />
                                                        <asp:TextBox ID="txtRemarkGeneral" runat="server" Rows="4" TextMode="multiLine" BackColor="White"
                                                            Width="99%"  BorderStyle="Solid" BorderColor="#A8BEDA" BorderWidth="1"/>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div>
                                                <fieldset>
                                                    <legend>
                                                        <asp:Label runat="server" ID="lblCandidateWishes" Text="Candidate Wishes"></asp:Label></legend>
                                                    <table width="99%">
                                                        <tr>
                                                            <td style="width: 50%" valign="top" rowspan="4">
                                                                <asp:Label ID="lblCanArea" runat="server" Text="Area" /><br />
                                                                <table width="99%">
                                                                    <tr>
                                                                        <td>
                                                                            <telerik:RadComboBox ID="ddlCanArea" runat="server" Width="300px" Skin="Office2007"></telerik:RadComboBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnAddCanArea" runat="server" CssClass="flatButton" Width="70px" Text="Add" OnClientClick="return OnButtonAddCanAreaClientClicked();" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:ListBox ID="listCanArea" runat="server" Width="300px" Height="100px"></asp:ListBox>
                                                                        </td>
                                                                        <td valign="top">
                                                                            <asp:Button ID="btnRemoveCanArea" runat="server" Width="70px" CssClass="flatButton" Text="Remove" OnClientClick="return OnButtonRemoveCanAreaClientClicked();" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:Label ID="lblCanSalary" runat="server" Text="Salary" /><br />
                                                                <telerik:RadComboBox ID="ddlSalaryWish" runat="server" Width="99%" AllowCustomText="true"
                                                                    Skin="Office2007" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:Label ID="lblCanCompanyGeneral" runat="server" Text="Company" /><br />
                                                                <telerik:RadComboBox ID="ddlCompanyWish" runat="server" AllowCustomText="true" Width="99%"
                                                                    Skin="Office2007" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:Label ID="lblCanContractType" runat="server" Text="Contract type" /><br />
                                                                <telerik:RadComboBox ID="ddlContractTypeWish" runat="server" AllowCustomText="true"
                                                                    Width="99%" Skin="Office2007" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:Label ID="lblCanFunction" runat="server" Text="Function" /><br />
                                                                <asp:TextBox ID="txtFunction" runat="server" Rows="4" TextMode="multiLine" BackColor="White"
                                                                    Width="99%" BorderStyle="Solid" BorderColor="#A8BEDA" BorderWidth="1"/>
                                                            </td>
                                                            <td valign="top">
                                                                <asp:Label ID="lblCanMotivation" runat="server" Text="Motivation" /><br />
                                                                <asp:TextBox ID="txtMotivation" runat="server" Rows="4" TextMode="multiLine" BackColor="White"
                                                                    Width="99%" BorderStyle="Solid" BorderColor="#A8BEDA" BorderWidth="1"/>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:HiddenField ID="hiddenCanAreaList" runat="server" />
                                                </fieldset>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </telerik:RadPageView>
                        <telerik:RadPageView ID="ExpectancyView" runat="server" CssClass="pageViewEducation">
                            <div>
                                <table>
                                    <tr>
                                        <td>
                                            <div style="height: 257px; width: 500px; overflow: -moz-scrollbars-vertical; overflow-y: scroll; overflow-x: hidden">                                
                                                <telerik:RadGrid ID="expectancyGrid" GridLines="None" AllowMultiRowSelection="false"
                                                Skin="Office2007" runat="server" AllowPaging="False" AllowSorting="False"
                                                AutoGenerateColumns="false" EnableAjaxSkinRendering="true" Visible="true">
                                                    <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                    <MasterTableView DataKeyNames="FunctionID" DataMember="CandidateExpectancy"
                                                        AllowMultiColumnSorting="False" Width="100%">
                                                        <Columns>
                                                            <telerik:GridBoundColumn UniqueName="Type" SortExpression="Type" HeaderText="Type"
                                                                DataField="Type">
                                                                <HeaderStyle Width=""></HeaderStyle>
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn UniqueName="Group" SortExpression="Group" HeaderText="Group"
                                                                DataField="Group">
                                                                <HeaderStyle Width=""></HeaderStyle>
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn UniqueName="FunctionFam" SortExpression="FunctionFam" HeaderText="Function"
                                                                DataField="FunctionFam">
                                                                <HeaderStyle Width=""></HeaderStyle>
                                                            </telerik:GridBoundColumn>                                                                                                                     
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings>
                                                        <Selecting AllowRowSelect="true" />
                                                    </ClientSettings>
                                                </telerik:RadGrid>
                                            </div> 
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Button ID="btnCanExpectEdit" runat="server" Text="Edit" CssClass="flatButton" Width="60px" OnClick="OnBtnExpectEditClicked" />
                                        </td>                                        
                                    </tr>
                                </table>
                            </div>
                                                       
                            <div id="divAddRemoveExpectancy" runat="server" visible="false">
                                <fieldset>
                                    <legend>
                                        <asp:Label runat="server" ID="lblAddRemoveExpectancy" Text="Add Expectancy"></asp:Label></legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblCanExpectUnit" runat="server" Text="Unit"></asp:Label>
                                            </td>
                                            <td>
                                                <telerik:RadComboBox ID="ddlCanExpectUnit" runat="server" AutoPostBack="true" Width="200"
                                                    OnSelectedIndexChanged="OnExpectUnitItemChanged" Skin="Office2007">
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblCanExpectFam" runat="server" Text="Family"></asp:Label>
                                            </td>
                                            <td>
                                                <telerik:RadComboBox ID="ddlCanExpectFam" runat="server" AutoPostBack="true" Width="200px"
                                                    OnSelectedIndexChanged="OnExpectFamItemChanged" Skin="Office2007">
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table>                                        
                                        <tr>
                                            <td>
                                                <asp:ListBox ID="listExpectOriginal" runat="server" Height="250px" Width="300px"
                                                    SelectionMode="Multiple"></asp:ListBox>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnCanExpectAdd" runat="server" Text="Add" CssClass="flatButton" Width="95px" OnClick="OnBtnExpectAddClicked" /><br />
                                                <asp:Button ID="btnCanExpectRemove" runat="server" Text="Remove" CssClass="flatButton" Width="95px" OnClick="OnBtnExpectRemoveClicked"/>
                                            </td>
                                            <td>
                                                <div style="height: 257px; width: 400px; overflow: -moz-scrollbars-vertical; overflow-y: scroll; overflow-x: hidden">
                                                    <telerik:RadGrid ID="gridCanExpectDestination" GridLines="None" AllowMultiRowSelection="false"
                                                        Skin="Office2007" runat="server" AllowPaging="True" AllowSorting="True" PageSize="100"
                                                        AutoGenerateColumns="false" EnableAjaxSkinRendering="true">
                                                        <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <MasterTableView DataKeyNames="FunctionID" DataMember="CandidateExpectancy"
                                                            AllowMultiColumnSorting="False" Width="100%">
                                                            <Columns>
                                                                <telerik:GridBoundColumn UniqueName="Type" SortExpression="Type" HeaderText="Type"
                                                                    DataField="Type">
                                                                    <HeaderStyle Width=""></HeaderStyle>
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn UniqueName="Group" SortExpression="Group" HeaderText="Group"
                                                                    DataField="Group">
                                                                    <HeaderStyle Width=""></HeaderStyle>
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn UniqueName="FunctionFam" SortExpression="FunctionFam" HeaderText="Function"
                                                                    DataField="FunctionFam">
                                                                    <HeaderStyle Width=""></HeaderStyle>
                                                                </telerik:GridBoundColumn>                                                                                                                      
                                                            </Columns>
                                                        </MasterTableView>
                                                        <ClientSettings>
                                                            <Selecting AllowRowSelect="true" />
                                                        </ClientSettings>
                                                    </telerik:RadGrid>                                                     
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" align="center">
                                                <asp:Button ID="btnExpectOK" runat="server" Text="OK" CssClass="flatButton" Width="95px" OnClick="OnBtnExpectOKClicked" />&nbsp;
                                                <asp:Button ID="btnExpectCancel" runat="server" Text="Cancel" CssClass="flatButton" Width="95px" OnClick="OnBtnExpectCancelClicked" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>
                        </telerik:RadPageView>
                        <telerik:RadPageView ID="StudyExperienceView" runat="server" CssClass="pageViewEducation">
                            <div>
                                <table width="99%">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCanStudies" runat="server" Text="Studies"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadGrid ID="gridStudies" GridLines="None" AllowMultiRowSelection="False"
                                                runat="server" AllowPaging="True" AllowSorting="True" PageSize="10" Width="100%"
                                                AutoGenerateColumns="false" Skin="Office2007" OnItemDataBound="OnGridStudyItemDataBound"
                                                OnPageIndexChanged="OnGridStudyPageIndexChanged" OnPageSizeChanged="OnGridStudy_PageSizeChanged"
                                                OnNeedDataSource="OnGridStudyNeedDataSource" EnableAjaxSkinRendering="true" Visible="false">
                                                <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                <MasterTableView DataKeyNames="CandidateFormationID" DataMember="CandidateTraining"
                                                    AllowMultiColumnSorting="True" Width="100%">
                                                    <Columns>
                                                        <telerik:GridBoundColumn UniqueName="Period" SortExpression="Period" HeaderText="Period"
                                                            DataField="Period">
                                                            <HeaderStyle Width="10%"></HeaderStyle>
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn UniqueName="Training" SortExpression="FormationString" HeaderText="Training"
                                                            DataField="FormationString">
                                                            <HeaderStyle Width="15%"></HeaderStyle>
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn UniqueName="Diploma" SortExpression="Diplome" HeaderText="Diploma"
                                                            DataField="Diplome">
                                                            <HeaderStyle Width="15%"></HeaderStyle>
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn UniqueName="Level" SortExpression="StudyLevelString" HeaderText="Level"
                                                            DataField="StudyLevelString">
                                                            <HeaderStyle Width="25%"></HeaderStyle>
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn UniqueName="School" SortExpression="School" HeaderText="School"
                                                            DataField="School">
                                                            <HeaderStyle Width="25%"></HeaderStyle>
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridTemplateColumn UniqueName="TemplateEditCanStudyColumn">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkCanStudyEdit" runat="server" Text="Edit" CommandArgument='<%# Eval("CandidateFormationID") %>'
                                                                    OnClientClick='<%# Eval("CandidateFormationID", "return OnCandidateStudyEditClientClicked({0});") %>'>  
                                                                </asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle Width="5%"></HeaderStyle>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="TemplateDeleteCanStudyColumn">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkCanStudyDelete" runat="server" Text="Delete" CommandArgument='<%# Eval("CandidateFormationID") %>'
                                                                    OnClientClick="return confirm('Are you sure to delete this item?')" OnClick="OnCandidateStudyDeleteClicked">
                                                                </asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle Width="5%"></HeaderStyle>
                                                        </telerik:GridTemplateColumn>
                                                    </Columns>
                                                </MasterTableView>
                                                <ClientSettings>
                                                    <Selecting AllowRowSelect="true" />
                                                </ClientSettings>
                                            </telerik:RadGrid>
                                            <telerik:RadWindow runat="server" ID="radWindowCanStudy" VisibleOnPageLoad="false"
                                                Skin="Office2007" VisibleStatusbar="false" Modal="true" OffsetElementID="offsetElement"
                                                Top="30" Left="30" NavigateUrl="CandidateStudyPopup.aspx" Title="Candidate Study"
                                                Height="250px" Width="450px" OnClientClose="onClientCanStudyDetailWindowClosed">
                                            </telerik:RadWindow>
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkAddNewStudy" runat="server" Text="Add new study" OnClientClick="return OnAddNewCanStudyClientClicked()"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCanExperience" runat="server" Text="Experience"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadGrid ID="gridExperience" GridLines="None" Skin="Office2007" AllowMultiRowSelection="False"
                                                runat="server" AllowPaging="True" AllowSorting="True" PageSize="10" Width="100%"
                                                AutoGenerateColumns="false" OnItemDataBound="OnGridExperienceItemDataBound"
                                                OnPageIndexChanged="OnGridExperiencePageIndexChanged" OnPageSizeChanged="OnGridExperience_PageSizeChanged"
                                                OnNeedDataSource="OnGridExperienceNeedDataSource" EnableAjaxSkinRendering="true" Visible="false">
                                                <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                <MasterTableView DataKeyNames="ExperienceID" DataMember="CandidateExperience" AllowMultiColumnSorting="True"
                                                    Width="100%">
                                                    <Columns>
                                                        <telerik:GridBoundColumn UniqueName="Period" SortExpression="Period" HeaderText="Period"
                                                            DataField="Period">
                                                            <HeaderStyle Width="12%"></HeaderStyle>
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn UniqueName="Company" SortExpression="Company" HeaderText="Company"
                                                            DataField="Company">
                                                            <HeaderStyle Width="15%"></HeaderStyle>
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn UniqueName="Salary" SortExpression="Salary" HeaderText="Salary"
                                                            DataField="Salary">
                                                            <HeaderStyle Width="10%"></HeaderStyle>
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn UniqueName="SalaryPackage" SortExpression="ExtraAdvantage"
                                                            HeaderText="Salary package" DataField="ExtraAdvantage">
                                                            <HeaderStyle Width="12%"></HeaderStyle>
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn UniqueName="JobTitle" SortExpression="FunctionString" HeaderText="Job title "
                                                            DataField="FunctionString">
                                                            <HeaderStyle Width="16%"></HeaderStyle>
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn UniqueName="QuitReason" SortExpression="LeftReason" HeaderText="Quit reason"
                                                            DataField="LeftReason">
                                                            <HeaderStyle Width="25%"></HeaderStyle>
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridTemplateColumn UniqueName="TemplateEditCanExperienceColumn">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkCanExperieceEdit" runat="server" Text="Edit" CommandArgument='<%# Eval("ExperienceID") %>'
                                                                    OnClientClick='<%# Eval("ExperienceID", "return OnCandidateExperieceEditClientClicked({0});") %>'>  
                                                                </asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle Width="5%"></HeaderStyle>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="TemplateDeleteCanExperieceColumn">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkCanExperieceDelete" runat="server" Text="Delete" CommandArgument='<%# Eval("ExperienceID") %>'
                                                                    OnClientClick="return confirm('Are you sure to delete this item?')" OnClick="OnCandidateExperieceDeleteClicked">
                                                                </asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle Width="5%"></HeaderStyle>
                                                        </telerik:GridTemplateColumn>
                                                    </Columns>
                                                </MasterTableView>
                                                <ClientSettings>
                                                    <Selecting AllowRowSelect="true" />
                                                </ClientSettings>
                                            </telerik:RadGrid>
                                            <telerik:RadWindow runat="server" ID="radWindowCanExperience" Skin="Office2007" VisibleOnPageLoad="false"
                                                VisibleStatusbar="false" Modal="true" OffsetElementID="offsetElement" Top="30"
                                                Left="30" NavigateUrl="CandidateExperiencePopup.aspx" Title="Candidate Experience"
                                                Height="350px" Width="600px" OnClientClose="onClientCanExperienceDetailWindowClosed">
                                            </telerik:RadWindow>
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkAddNewExperience" runat="server" Text="Add new experience"
                                                OnClientClick="return OnAddNewCanExperienceClientClicked()"></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </telerik:RadPageView>
                        <telerik:RadPageView ID="EvaluationView" runat="server">
                            <div>
                                <table width="99%">
                                    <tr>
                                        <td style="width: 50%">
                                            <asp:Label ID="lblCanPresentationEval" runat="server" Text="Presentation" /><br />
                                            <asp:TextBox ID="txtPresentationEval" runat="server" Rows="4" TextMode="multiLine"
                                                Width="99%" BackColor="White" BorderStyle="Solid" BorderColor="#A8BEDA" BorderWidth="1"/>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCanGlobal" runat="server" Text="Global" /><br />
                                            <asp:TextBox ID="txtGlobal" runat="server" Rows="4" TextMode="multiLine" Width="99%"
                                                 BackColor="White" BorderStyle="Solid" BorderColor="#A8BEDA" BorderWidth="1"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCanVerbal" runat="server" Text="Verbal" /><br />
                                            <asp:TextBox ID="txtVerbal" runat="server" Rows="4" TextMode="multiLine" Width="99%"
                                                 BackColor="White" BorderStyle="Solid" BorderColor="#A8BEDA" BorderWidth="1"/>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCanOtherEval" runat="server" Text="Other" /><br />
                                            <asp:TextBox ID="txtOtherEval" runat="server" Rows="4" TextMode="multiLine"
                                                Width="99%"  BackColor="White" BorderStyle="Solid" BorderColor="#A8BEDA" BorderWidth="1"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCanAutonomy" runat="server" Text="Autonomy" /><br />
                                            <asp:TextBox ID="txtAutonomy" runat="server" Rows="4" TextMode="multiLine"
                                                Width="99%" BackColor="White" BorderStyle="Solid" BorderColor="#A8BEDA" BorderWidth="1"/>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCanMotivationEval" runat="server" Text="Motivation" /><br />
                                            <asp:TextBox ID="txtMotivationEval" runat="server" Rows="4" TextMode="multiLine"
                                                Width="99%" BackColor="White"  BorderStyle="Solid" BorderColor="#A8BEDA" BorderWidth="1"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCanPersonality" runat="server" Text="Personality" /><br />
                                            <asp:TextBox ID="txtPersonality" runat="server" Rows="4" TextMode="multiLine"
                                                Width="99%" BackColor="White" BorderStyle="Solid" BorderColor="#A8BEDA" BorderWidth="1"/>
                                        </td>
                                        <td>
                                            <div>
                                                <fieldset>
                                                    <legend>
                                                        <asp:Label runat="server" ID="lblCanLanguage" Text="Language"></asp:Label></legend>
                                                    <table width="99%">
                                                        <tr>
                                                            <td style="width: 30px">
                                                                <asp:Label ID="lblCanFrenchLang" runat="server" Text="French" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadComboBox ID="ddlFrenchLang" runat="server" Width="150px" Skin="Office2007" />
                                                            </td>
                                                            <td style="width: 30px">
                                                                <asp:Label ID="lblCanGermanLang" runat="server" Text="German" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadComboBox ID="ddlGermanLang" runat="server" Width="150px" Skin="Office2007" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblCanDutchLang" runat="server" Text="Dutch" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadComboBox ID="ddlDutchLang" runat="server" Width="150px" Skin="Office2007" />
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblCanSpanishLang" runat="server" Text="Spainish" />
                                                            </td>
                                                            <td>                                                                
                                                                <telerik:RadComboBox ID="ddlSpainishLang" runat="server" Width="150px" Skin="Office2007" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblCanEnglishLang" runat="server" Text="English" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadComboBox ID="ddlEnglishLang" runat="server" Width="150px" Skin="Office2007" />
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblCanItalianLang" runat="server" Text="Italian" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadComboBox ID="ddlItalianLang" runat="server" Width="150px" Skin="Office2007" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblCanOtherLang" runat="server" Text="Other" />
                                                            </td>
                                                            <td colspan="2">
                                                                <telerik:RadComboBox ID="ddlOtherLang" runat="server" Width="99%" Skin="Office2007" />
                                                            </td>
                                                            <td>                                                                
                                                                <telerik:RadComboBox ID="ddlOtherLangSkill" runat="server" Width="150px" Skin="Office2007" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </telerik:RadPageView>
                        <telerik:RadPageView ID="KnowledgeView" runat="server">
                            <div>
                                <table>
                                    <tr>
                                        <td style="width: 400px">
                                            <asp:Label ID="lblCanKnowledgeGrid" runat="server" Text="Knowledge"></asp:Label>
                                        </td>
                                        <td style="width: 400px">
                                            <asp:Label ID="lblCanFunctionGrid" runat="server" Text="Funtion"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <div style="height: 257px; width: 400px; overflow: -moz-scrollbars-vertical; overflow-y: scroll; overflow-x: hidden">
                                                <telerik:RadGrid ID="gridKnowledgeOld" GridLines="None" AllowMultiRowSelection="false"
                                                Skin="Office2007" runat="server" AutoGenerateColumns="false" EnableAjaxSkinRendering="true">
                                                <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                <MasterTableView DataKeyNames="KnowledgeID" DataMember="CandidateKnowledge" AllowMultiColumnSorting="True"
                                                      Width="100%" EditMode="PopUp">
                                                    <Columns>
                                                        <telerik:GridBoundColumn UniqueName="Type" SortExpression="Type" HeaderText="Type"
                                                            DataField="Type">
                                                            <HeaderStyle Width="20%"></HeaderStyle>
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn UniqueName="Group" SortExpression="Group" HeaderText="Group"
                                                            DataField="Group">
                                                            <HeaderStyle Width="40%"></HeaderStyle>
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn UniqueName="Knowledge" SortExpression="Code" HeaderText="Knowledge"
                                                            DataField="Code">
                                                            <HeaderStyle Width="40%"></HeaderStyle>
                                                        </telerik:GridBoundColumn>                                                                   
                                                    </Columns>
                                                </MasterTableView>
                                                <ClientSettings>
                                                    <Selecting AllowRowSelect="true" />
                                                </ClientSettings>
                                            </telerik:RadGrid>                                                        
                                            </div>
                                        </td>
                                        <td valign="top">
                                            <div style="height: 257px; width: 400px; overflow: -moz-scrollbars-vertical; overflow-y: scroll; overflow-x: hidden">
                                                <telerik:RadGrid ID="gridFunctionOld" GridLines="None" AllowMultiRowSelection="false"
                                                Skin="Office2007" runat="server" 
                                                AutoGenerateColumns="false" EnableAjaxSkinRendering="true">
                                                <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                <MasterTableView DataKeyNames="FunctionID" DataMember="CandidateFunction" AllowMultiColumnSorting="True"
                                                     Width="100%" EditMode="PopUp">
                                                    <Columns>
                                                        <telerik:GridBoundColumn UniqueName="Type" SortExpression="Type" HeaderText="Type"
                                                            DataField="Type">
                                                            <HeaderStyle Width="20%"></HeaderStyle>
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn UniqueName="Group" SortExpression="Group" HeaderText="Group"
                                                            DataField="Group">
                                                            <HeaderStyle Width="40%"></HeaderStyle>
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn UniqueName="Function" SortExpression="Code" HeaderText="Function"
                                                            DataField="Code">
                                                            <HeaderStyle Width="40%"></HeaderStyle>
                                                        </telerik:GridBoundColumn>                                                                   
                                                    </Columns>
                                                </MasterTableView>
                                                <ClientSettings>
                                                    <Selecting AllowRowSelect="true" />
                                                </ClientSettings>
                                            </telerik:RadGrid>    
                                            </div>         
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Button ID="btnKnowledgeEdit" runat="server" Text="Edit" CssClass="flatButton" Width="60px" OnClick="OnKnowledgeEditClicked" />
                                        </td>
                                        <td align="left">
                                            <asp:Button ID="btnFunctionEdit" runat="server" Text="Edit" CssClass="flatButton" Width="60px" OnClick="OnFunctionEditClicked" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <br />
                            <div id="divKnowledgeFunctionEdit" runat="server" visible="false">
                                <fieldset>
                                    <legend>
                                        <asp:Label runat="server" ID="lblFieldSetKnowFunc" Text="Add Knowledge/Function"></asp:Label></legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblKnowFuncUnit" runat="server" Text="Unit"></asp:Label>
                                            </td>
                                            <td>
                                                <telerik:RadComboBox ID="ddlKnowFuncUnit" runat="server" AutoPostBack="true" Width="200"
                                                    OnSelectedIndexChanged="OnKnowFuncUnitItemChanged" Skin="Office2007">
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblKnowFuncFam" runat="server" Text="Family"></asp:Label>
                                            </td>
                                            <td>
                                                <telerik:RadComboBox ID="ddlKnowFuncFam" runat="server" AutoPostBack="true" Width="200px"
                                                    OnSelectedIndexChanged="OnKnowFuncFamItemChanged" Skin="Office2007">
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblKnowFuncDestination" runat="server" Text="Knowleage/Function"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:ListBox ID="listKnowFuncOriginal" runat="server" Height="250px" Width="300px"
                                                    SelectionMode="Multiple"></asp:ListBox>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnKnowFuncAdd" runat="server" Text="Add" CssClass="flatButton" Width="95px" OnClick="OnBtnKnowFuncAddClicked" /><br />
                                                <asp:Button ID="btnKnowFuncRemove" runat="server" Text="Remove" CssClass="flatButton" Width="95px" OnClick="OnBtnKnowFuncRemoveClicked"/>
                                            </td>
                                            <td>
                                                <div style="height: 257px; width: 400px; overflow: -moz-scrollbars-vertical; overflow-y: scroll; overflow-x: hidden">
                                                    <telerik:RadGrid ID="gridKnowFuncDestination" GridLines="None" AllowMultiRowSelection="true"
                                                    Skin="Office2007" runat="server"
                                                    AutoGenerateColumns="false" EnableAjaxSkinRendering="true">
                                                    <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                    <MasterTableView DataKeyNames="KnowledgeID" DataMember="CandidateKnowledge" AllowMultiColumnSorting="True"
                                                          Width="100%" EditMode="PopUp">
                                                        <Columns>
                                                            <telerik:GridBoundColumn UniqueName="Type" SortExpression="Type" HeaderText="Type"
                                                                DataField="Type">
                                                                <HeaderStyle Width="20%"></HeaderStyle>
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn UniqueName="Group" SortExpression="Group" HeaderText="Group"
                                                                DataField="Group">
                                                                <HeaderStyle Width="40%"></HeaderStyle>
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn UniqueName="Knowledge" SortExpression="Code" HeaderText="Knowledge"
                                                                DataField="Code">
                                                                <HeaderStyle Width="40%"></HeaderStyle>
                                                            </telerik:GridBoundColumn>                                                                   
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings>
                                                        <Selecting AllowRowSelect="true" />
                                                    </ClientSettings>
                                                </telerik:RadGrid>                                                        
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" align="center">
                                                <asp:Button ID="btnKnowFuncOK" runat="server" Text="OK" CssClass="flatButton" Width="95px" OnClick="OnBtnKnowFuncOKClicked" />&nbsp;
                                                <asp:Button ID="btnKnowFuncCancel" runat="server" Text="Cancel" CssClass="flatButton" Width="95px" OnClick="OnBtnKnowFuncCancelClicked" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>
                        </telerik:RadPageView>
                        <telerik:RadPageView ID="PresentationView" runat="server">
                            <div style="text-align: center; margin-top: 5px;">
                                <asp:TextBox runat="server" ID="txtPresentationText" Width="99%" Height="250"
                                    TextMode="MultiLine"  BackColor="White" BorderWidth="1" BorderColor="#A8BEDA">
                                </asp:TextBox>
                            </div>
                            <div style="text-align: center;">
                                <asp:Button runat="server" ID="btnGeneratePresentation" Text="Generate Presentation"
                                    CssClass="flatButton" />
                            </div>
                            <div>
                                <fieldset>
                                    <legend>
                                        <asp:Label runat="server" ID="lblSendPresentation" Text="Send the presentation"></asp:Label></legend>
                                    <table width="100%">
                                        <tr>
                                            <td align="right" style="width: 20%">
                                                <asp:Literal runat="server" ID="lblCompanyPresent" Text="Company"></asp:Literal></td>
                                            <td align="left">
                                                <telerik:RadComboBox runat="server" ID="ddlCompany" Width="220"  EnableLoadOnDemand="true" Skin="Office2007" ShowWhileLoading="true" MaxHeight="300"
                                                     OnClientSelectedIndexChanged="OnCompanyClientSelectedIndexChanged" AllowCustomText="true"
                                                     OnItemsRequested="OnCompanyDropdownItemRequested" OnClientItemsRequesting="onDropdownCompany_ClientItemRequesting" >
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="width: 10%; vertical-align: top">
                                                <asp:Literal runat="server" ID="lblContactsPresent" Text="Contacts"></asp:Literal></td>
                                            <td align="left">
                                                <telerik:RadGrid ID="grdPresentationContacts" GridLines="None" Skin="Office2007"
                                                    PageSize="5" AllowMultiRowSelection="true" runat="server" AllowPaging="True"
                                                    AllowSorting="False" Width="500" AutoGenerateColumns="false" OnItemDataBound="OnGridPresentationContactsItemDataBound"
                                                    OnPageIndexChanged="OnGridPresentationContactsPageIndexChanged" 
                                                    OnNeedDataSource="OnPresentationContactGridNeedDataSource" EnableAjaxSkinRendering="true">
                                                    <PagerStyle Mode="NextPrevAndNumeric" />
                                                    <MasterTableView AllowMultiColumnSorting="False" Width="100%" DataKeyNames="ContactID" DataMember="CompanyContact">
                                                        <Columns>
                                                             <telerik:GridBoundColumn UniqueName="ContactID" DataField="ContactID" Display="false">
                                                                <HeaderStyle></HeaderStyle>
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridTemplateColumn UniqueName="TemplateContactNameColumn" HeaderText="Name"
                                                                Resizable="true">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="lblContactName" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="50%" />
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="TemplateContactEmailColumn" HeaderText="e-mail"
                                                                Resizable="true">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="lblContactEmail" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="40%" />
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridClientSelectColumn Resizable="true">
                                                                <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="center" />
                                                            </telerik:GridClientSelectColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings>
                                                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" />
                                                    </ClientSettings>
                                                </telerik:RadGrid>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="width: 10%; vertical-align: top">
                                                <asp:Literal runat="server" ID="lblAttachedDocumentsPresent" Text="Attached Documents"></asp:Literal></td>
                                            <td align="left">
                                                <telerik:RadGrid ID="grdPresentationAttachedDocs" GridLines="None" Skin="Office2007" 
                                                    AllowMultiRowSelection="true" runat="server" AllowPaging="True" PageSize="5"
                                                    AllowSorting="False" OnItemDataBound="OnGridPresentationAttachedDocsItemDataBound"
                                                    Width="500" AutoGenerateColumns="false" OnPageIndexChanged="OnGridPresentationAttachedDocsPageIndexChanged"
                                                    OnNeedDataSource="OnGridPresentationAttachedDocsNeedDataSource">
                                                    <PagerStyle Mode="NextPrevAndNumeric" />
                                                    <MasterTableView DataKeyNames="DocumentID" DataMember="CandidateDocument" AllowMultiColumnSorting="False" Width="100%">
                                                        <Columns>
                                                            <telerik:GridBoundColumn UniqueName="DocumentID" DataField="DocumentID" Display="false">
                                                                <HeaderStyle></HeaderStyle>
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridTemplateColumn UniqueName="TemplateContactNameColumn" HeaderText="Document"
                                                                Resizable="true">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="lblDocName" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="50%" />
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="TemplateContactEmailColumn" HeaderText="Created date"
                                                                Resizable="true">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="lbCreationDate" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="40%" />
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridClientSelectColumn Resizable="true">
                                                                <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="center" />
                                                            </telerik:GridClientSelectColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings>
                                                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" />
                                                    </ClientSettings>
                                                </telerik:RadGrid>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="width: 10%; vertical-align: top">
                                                <asp:Literal runat="server" ID="lblComDocuments" Text="Company documents"></asp:Literal></td>                                            
                                            <td>
                                                <div id="divPresentDocuments" runat="Server">
                                                    <telerik:RadGrid ID="grdComDocuments" GridLines="None" Skin="Office2007" AllowMultiRowSelection="True" 
                                                        EnableAjaxSkinRendering="true" AllowSorting="False"
                                                        runat="server" AllowPaging="True" PageSize="5" Width="500" 
                                                        AutoGenerateColumns="false"                                                        
                                                        OnNeedDataSource="OnGridComDocumentsNeedDataSource"
                                                        OnPageIndexChanged="OnGridComDocumentsPageIndexChanged">
                                                        <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                        <MasterTableView DataKeyNames="DocumentID" DataMember="CompanyDocument" AllowMultiColumnSorting="False"
                                                            Width="100%">
                                                            <Columns>        
                                                                 <telerik:GridBoundColumn UniqueName="DocumentID" DataField="DocumentID" Display="false">
                                                                    <HeaderStyle></HeaderStyle>
                                                                </telerik:GridBoundColumn>                                
                                                                <telerik:GridBoundColumn UniqueName="DocumentName" SortExpression="DocumentName" 
                                                                    HeaderText="Name" DataField="DocumentName">                                                                   
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                    <HeaderStyle Width="50%"></HeaderStyle>
                                                                </telerik:GridBoundColumn>                                                                
                                                                <telerik:GridBoundColumn UniqueName="CreatedDate" SortExpression="CreatedDate" 
                                                                    HeaderText="Created Date" DataField="CreatedDate" DataType="system.datetime" DataFormatString="{0:dd/MM/yyyy HH:mm tt}">
                                                                    <HeaderStyle Width="40%"></HeaderStyle>
                                                                </telerik:GridBoundColumn>                                                                
                                                                <telerik:GridClientSelectColumn Resizable="true">
                                                                <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="center" />
                                                            </telerik:GridClientSelectColumn>
                                                            </Columns>
                                                        </MasterTableView>
                                                        <ClientSettings>
                                                            <Selecting AllowRowSelect="true" />
                                                        </ClientSettings>
                                                    </telerik:RadGrid>
                                                    <telerik:RadWindow runat="server" ID="radWindowComDocument" Skin="Office2007" VisibleStatusbar="false"
                                                        ShowContentDuringLoad="true" VisibleOnPageLoad="false" Modal="true" OffsetElementID="offsetElement"
                                                        Top="30" NavigateUrl="CompanyDocumentPopup.aspx" Title="Company documents"
                                                        Height="250px" Width="420px" OnClientClose="onClientRadPreComDocumentWindowClosed">
                                                    </telerik:RadWindow>
                                                    <div style="text-align: center;">
                                                        <asp:HyperLink runat="server" ID="lnkAddNewComDocument" Text="Add new documents" NavigateUrl="javascript:;"
                                                            Visible="false"></asp:HyperLink></div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:Button runat="server" ID="btnSendPresentation" Text="Send the presentation now"
                                                    CssClass="flatButton" OnClick="OnBtnSendPresentationClicked"/>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>
                             <telerik:RadWindow runat="server" ID="radWinSendPresentation" Skin="Office2007" VisibleOnPageLoad="false" VisibleStatusbar="false"
                                Modal="true" OffsetElementID="offsetElement" Top="30" Left="30" NavigateUrl="SendPresentationEmail.aspx"
                                Title="Send Presentation" Height="500px" Width="750px" OnClientClose="onSendPresentationEmailWindowClosed">
                            </telerik:RadWindow>
                        </telerik:RadPageView>
                        <telerik:RadPageView ID="ActionView" runat="server">
                            <div>
                                <table width="99%">
                                    <tr>
                                        <td>
                                            <telerik:RadGrid ID="gridActions" GridLines="None" Skin="Office2007" AllowMultiRowSelection="True"
                                                runat="server" AllowPaging="True" AllowSorting="True" PageSize="20" Width="100%"
                                                AutoGenerateColumns="false" OnItemDataBound="OnGridActionItemDataBound" OnPageIndexChanged="OnGridActionPageIndexChanged"
                                                OnNeedDataSource="OnGridActionNeedDataSource" OnPageSizeChanged="OnActionGrid_PageSizeChanged"
                                                EnableAjaxSkinRendering="true" Visible="false">
                                                <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                <MasterTableView DataKeyNames="ActionId" DataMember="Action" AllowMultiColumnSorting="True"
                                                    Width="100%">
                                                    <Columns>
                                                        <telerik:GridBoundColumn UniqueName="Active" SortExpression="Actif" HeaderText="Active"
                                                            DataField="Actif">
                                                            <HeaderStyle Width="5%"></HeaderStyle>
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn UniqueName="TaskNbr" SortExpression="ActionId" HeaderText="Task nbr"
                                                            DataField="ActionId">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn UniqueName="DateAction" SortExpression="DateAction" HeaderText="Date"
                                                            DataField="DateAction" DataType="system.datetime" DataFormatString="{0:dd/MM/yyyy}">
                                                            <HeaderStyle Width="10%"></HeaderStyle>
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn UniqueName="Hour" SortExpression="Hour" HeaderText="Hour"
                                                            DataField="Hour" DataType="system.datetime" DataFormatString="{0:hh:mm tt}">
                                                            <HeaderStyle Width="10%"></HeaderStyle>
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn UniqueName="TypeAction" SortExpression="TypeActionLabel"
                                                            HeaderText="Type" DataField="TypeActionLabel">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn UniqueName="Company" SortExpression="CompanyName" HeaderText="Company"
                                                            DataField="CompanyName">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn UniqueName="Descripton" SortExpression="DescrAction" HeaderText="Descripton"
                                                            DataField="DescrAction">
                                                            <HeaderStyle Width="25%"></HeaderStyle>
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn UniqueName="ResponsableName" SortExpression="ResponsableName"
                                                            HeaderText="Responsible" DataField="ResponsableName">
                                                            <HeaderStyle Width="10%"></HeaderStyle>
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridTemplateColumn UniqueName="TemplateEditCanActionColumn">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkCanActionEdit" runat="server" Text="Edit" CommandArgument='<%# Eval("ActionId") %>'
                                                                    OnClientClick='<%# Eval("ActionId", "return OnCandidateActionEditClientClicked({0});") %>'>  
                                                                </asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle Width="5%"></HeaderStyle>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="TemplateDeleteCanActionColumn">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkCanActionDelete" runat="server" Text="Delete" CommandArgument='<%# Eval("ActionId") %>'
                                                                    OnClientClick="return confirm('Are you sure to delete this item?')" OnClick="OnCandidateActionDeleteClicked">
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
                                                <ClientSettings>
                                                    <Selecting AllowRowSelect="false" />
                                                </ClientSettings>
                                            </telerik:RadGrid>
                                            <telerik:RadWindow runat="server" ID="radWindowCanAction" Skin="Office2007" VisibleOnPageLoad="false" VisibleStatusbar="false"
                                                Modal="true" OffsetElementID="offsetElement" Top="30" Left="30" NavigateUrl="ComCanActionPopup.aspx"
                                                Title="Candidate Action" Height="500px" Width="850px" OnClientClose="onClientCanActionDetailWindowClosed">
                                            </telerik:RadWindow>
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkAddNewAction" runat="server" Text="Add new action" OnClientClick="return OnAddNewCanActionClientClicked()"></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </telerik:RadPageView>
                        <telerik:RadPageView ID="DocumentView" runat="server">
                            <telerik:RadGrid ID="grdDocuments" GridLines="None" Skin="Office2007" AllowMultiRowSelection="True" EnableAjaxSkinRendering="true"
                                runat="server" AllowPaging="True" AllowSorting="true" PageSize="20" Width="100%" Visible="false" 
                                AutoGenerateColumns="false" OnItemDataBound="OnGridDocumentsItemDataBound"
                                OnDeleteCommand="OnGridDocumentsDeleteCommand" OnPageIndexChanged="OnGridDocumentsPageIndexChanged"
                                OnNeedDataSource="OnGridDocumentsNeedDataSource" OnPageSizeChanged="OnDocumentGrid_PageSizeChanged"
                                ><%--OnItemCommand="OnGridDocumentsItemCommand"--%>
                                <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                <MasterTableView DataKeyNames="DocumentID" DataMember="CandidateDocument" AllowMultiColumnSorting="False"
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
                                        <telerik:GridTemplateColumn UniqueName="TemplateEditCanContactColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkCanContactEdit" runat="server" Text="Edit" CommandArgument='<%# Eval("DocumentID") %>'
                                                    OnClientClick='<%# Eval("DocumentID", "return OnDocumentEditClientClicked(\"\",{0});") %>'> 
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
                            <telerik:RadWindow runat="server" ID="radWindowDocument" Skin="Office2007" VisibleStatusbar="false"
                                ShowContentDuringLoad="true" VisibleOnPageLoad="false" Modal="true" OffsetElementID="offsetElement"
                                Top="30" NavigateUrl="DocumentPopup.aspx" Title="Candidate documents"
                                Height="250px" Width="420px" OnClientClose="onClientRadDocumentWindowClosed">
                            </telerik:RadWindow>
                            <div style="text-align: center;">
                                <asp:HyperLink runat="server" ID="lnkAddNewDocument" NavigateUrl="javascript:;"
                                    Visible="false"></asp:HyperLink></div>
                        </telerik:RadPageView>
                    </telerik:RadMultiPage>
                </div>
                <div style="text-align: center; margin-top:10px;">
                    <table width="100%">
                        <tr>
                            
                            <td align="center">
                                <asp:Button runat="server" ID="btnEditSave" Text="Edit" CssClass="flatButton" Width="60"
                                    OnClick="OnButtonCandidateEditSaveClicked" />&nbsp;
                                <asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="flatButton" Width="60"
                                    OnClick="OnButtonCandidateCancelClicked" />
                            </td>
                            <td align="right" style="width:150px">
                                <asp:LinkButton ID="lnkBackToAction" runat="server"  Text="Back to action list"
                                    OnClick="OnLinkBackToActionClicked" Visible="false"></asp:LinkButton>                                    
                            </td>                            
                        </tr>
                    </table>
                    <asp:HiddenField ID="hidActionUrl" runat="server" />
                    <asp:HiddenField ID="hidMode" runat="server" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
