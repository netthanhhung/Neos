<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JobProfile.aspx.cs" Inherits="JobPopup" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Job</title>
    <link href="Styles/Neos.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="script/utils.js"></script>
    <telerik:RadScriptBlock runat="server" ID="scriptBlock">
          <script type="text/javascript">
    function onButtonPreview_ClientClick(previewURL)
    {
        return openPopUp(previewURL);
    }
    function OnDropdownProfile_ClientSelectionChanged(sender, eventArgs)
    {
        $find("JobProfileAjaxManager").ajaxRequest("ddlProfileClientChanged");
    }
    function enableInputControl(control, inputID1, inputID2)
    {
        if(document.getElementById(control) != null)
        {
            if(document.getElementById(control) != null)
            {
                document.getElementById(inputID1).disabled = document.getElementById(control).selected;
                document.getElementById(inputID2).disabled = !document.getElementById(control).selected;
            }
        }   
    }
    function onTabStripJobProfile_ClientLoad(sender)
    {
        var jobID = getQueryString("JobId");
        var mode = getQueryString("mode");
        if(jobID != null && jobID != "") //in edit mode
        {                                  
            if(mode == "edit")
                processJobToolBar("EditJobProfile");
            else if(mode == "view")
                processJobToolBar("ViewJobProfile");
        } 
        else if(mode == "edit") 
        {
            processJobToolBar("EditJobProfile");
        }
        return false;
    }
    function onLoadJobProfilePage()
    {
        var jobID = getQueryString("JobId");      
        var mode = getQueryString("mode");  
        if(jobID != null && jobID != "") //in edit mode
        {           
            if(mode == "edit")
                processJobToolBar("EditJobProfile");
            else if(mode == "view")
                processJobToolBar("ViewJobProfile");
        }
        else if(mode == "edit") 
        {
            processJobToolBar("EditJobProfile");
        }
        return false;
    }
    
    function onBeforeFormUnload()
    {
        processJobToolBar("UnloadJobProfilePage");
    }
    </script>
    </telerik:RadScriptBlock>

</head>
<body onbeforeunload="onBeforeFormUnload()">
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="ScriptManager" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadAjaxManager runat="server" ID="JobProfileAjaxManager"  DefaultLoadingPanelID="RadAjaxLoadingPanel1"
        OnAjaxRequest="OnMyAjaxManager_AjaxRequest">
        </telerik:RadAjaxManager>
        
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Height="75px"
                Width="75px" Transparency="50">
                <img alt="Loading..." src='<%= RadAjaxLoadingPanel.GetWebResourceUrl(Page, "Telerik.Web.UI.Skins.Default.Ajax.loading.gif") %>'
                    style="border: 0;" />
            </telerik:RadAjaxLoadingPanel>
            
            
        <asp:ValidationSummary runat="server" ID="sumValid" ShowMessageBox="true" ShowSummary="false"  ValidationGroup="JobProfileValidation"/>
        <div class="rightpane_title" style="width:100%">
             <table style="width:98%">
                <tr>                                   
                    <td>
                        <asp:Literal runat="server" ID="lblJobProfileTitle" Text="Job Profile"></asp:Literal>
                    </td>
                    <td align="right">
                        <asp:LinkButton ID="lnkBack" runat="server"  Text="Back"
                                    OnClick="OnLinkBackClicked"></asp:LinkButton>     
                    </td> 
                </tr>
            </table> 
        </div>        
        <div style="margin-top:30px;">
        <div>
            <table width="100%">                
                <tr>
                    <td><asp:Literal runat="server" ID="lblJobRef"></asp:Literal></td>
                    <td><asp:Literal runat="server" ID="txtJobRef"></asp:Literal></td>
                    <td><asp:Literal runat="server" ID="lblNumberOfVisits"></asp:Literal></td>
                    <td><asp:Literal runat="server" ID="txtNumberOfVisits"></asp:Literal></td>
                    <td><asp:Literal runat="server" ID="lblNumberApplications"></asp:Literal></td>
                    <td><asp:Literal runat="server" ID="txtNumberOfApplications"></asp:Literal></td>
                    <td><asp:Literal runat="server" ID="lblLastModifDate"></asp:Literal></td>
                    <td>
                        <telerik:RadDatePicker ID="calLastModif" runat="server" MinDate="0001-01-01" Skin="Office2007"
                            Width="122" Enabled="false"  Calendar-CultureInfo="en-US">
                            <DateInput ID="inputLastModif" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd/MM/yyyy"
                                Skin="Office2007">
                            </DateInput>
                        </telerik:RadDatePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal runat="server" ID="lblProfile" Text="Profile"></asp:Literal></td>
                    <td>
                        <telerik:RadComboBox runat="server" ID="ddlProfile" Width="140" Skin="Office2007"
                            DataMember="ParamProfile" DataTextField="Profile" DataValueField="ProfileID"
                            OnClientSelectedIndexChanged="OnDropdownProfile_ClientSelectionChanged">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="lblLocation" Text="Location"></asp:Literal>:</td>
                    <td>
                        <telerik:RadComboBox runat="server" ID="ddlLocation" Width="120" Skin="Office2007"
                            DataMember="ParamLocations" DataTextField="Location" DataValueField="Location">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="lblResponsible" Text="Responsible"></asp:Literal></td>
                    <td>
                        <telerik:RadComboBox runat="server" ID="ddlResponsible" Width="120" Skin="Office2007"
                            DataMember="ParamUser" DataTextField="LastName" DataValueField="UserID">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="lblActivatedDate" Text="Activated Date"></asp:Literal></td>
                    <td>
                        <telerik:RadDatePicker ID="calActivatedDate" runat="server" MinDate="0001-01-01"
                            Skin="Office2007" Width="122"  Calendar-CultureInfo="en-US">
                            <DateInput ID="inputActivatedDate" runat="server" Skin="Office2007" DateFormat="dd/MM/yyyy"
                                DisplayDateFormat="dd/MM/yyyy">
                            </DateInput>
                        </telerik:RadDatePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal runat="server" ID="lblFunction" Text="Function"></asp:Literal></td>
                    <td>
                        <telerik:RadComboBox runat="server" ID="ddlFunction" Width="140" Skin="Office2007"
                            DataMember="ParamFunctionFam" DataTextField="FonctionFamID" DataValueField="FonctionFamID">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="lblConfidential" Text="Confidential"></asp:Literal></td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkIsConfidential" Text="" />
                    </td>
                    <td>
                        <asp:RadioButton runat="server" ID="rdoSelectEmail" Text="Email" GroupName="EmailUrl" Checked="true" /></td>
                    <td>
                        <asp:TextBox runat="server" ID="txtEmail" Width="115" MaxLength="60"></asp:TextBox>
                        <asp:RegularExpressionValidator runat="server" ID="revEmail" ControlToValidate="txtEmail" 
                        Display="none" ErrorMessage="Invalid email format" ValidationGroup="JobProfileValidation" 
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                     </td>
                    <td>
                        <asp:Literal runat="server" ID="lblExpiredDate" Text="Expired Date"></asp:Literal></td>
                    <td>
                        <telerik:RadDatePicker ID="calExpiredDate" runat="server" MinDate="0001-01-01" Skin="Office2007"
                            Width="122"  Calendar-CultureInfo="en-US">
                            <DateInput ID="inputExpiredDate" runat="server" Skin="Office2007" DateFormat="dd/MM/yyyy"
                                DisplayDateFormat="dd/MM/yyyy">
                            </DateInput>
                        </telerik:RadDatePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal runat="server" ID="lblCompany" Text="Company"></asp:Literal></td>
                    <td>
                        <telerik:RadComboBox runat="server" ID="ddlCompany" Width="140" DataMember="Company"
                            DataTextField="CompanyName" DataValueField="CompanyID" Height="300" ChangeTextOnKeyBoardNavigation="true"
                            MarkFirstMatch="true" Skin="Office2007">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="lblActive" Text="Active"></asp:Literal></td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkIsActive" Text="" />
                    </td>
                    <td>
                        <asp:RadioButton runat="server" ID="rdoSelectURL" Text="Url" GroupName="EmailUrl" /></td>
                    <td>
                        <asp:TextBox runat="server" ID="txtURL" MaxLength="200" Width="115"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="lblRemindDate" Text="Remind Date"></asp:Literal></td>
                    <td>
                        <telerik:RadDatePicker ID="calRemindDate" runat="server" MinDate="0001-01-01" Skin="Office2007"
                            Width="122"  Calendar-CultureInfo="en-US">
                            <DateInput ID="inputRemindDate" runat="server" Skin="Office2007" DateFormat="dd/MM/yyyy"
                                DisplayDateFormat="dd/MM/yyyy">
                            </DateInput>
                        </telerik:RadDatePicker>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <telerik:RadTabStrip ID="radTabStripJobProfile" runat="server" Skin="Web20" BackColor="#A0B8DB" MultiPageID="JobProfileMultiPage"
                SelectedIndex="0" CssClass="tabStrip" CausesValidation="false">
                <Tabs>
                    <telerik:RadTab Text="FR" Font-Bold="true">
                    </telerik:RadTab>
                    <telerik:RadTab Text="NL" Font-Bold="true">
                    </telerik:RadTab>
                    <telerik:RadTab Text="EN" Font-Bold="true">
                    </telerik:RadTab>
                </Tabs>
            </telerik:RadTabStrip>
            <telerik:RadMultiPage ID="JobProfileMultiPage" runat="server" SelectedIndex="0" CssClass="multiPage">
                <telerik:RadPageView ID="FRView" runat="server">
                    <table width="100%">
                        <tr>
                            <td style="width: 20%; text-align: right">
                                <strong>
                                    <asp:Literal runat="server" ID="lblTitle" Text="Title"></asp:Literal></strong></td>
                            <td style="text-align: left;">
                                <asp:TextBox runat="server" ID="txtTitle" Width="300" MaxLength="128"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top; text-align: right">
                                <strong>
                                    <asp:Literal runat="server" ID="lblCompanyDesc" Text="Company Description"></asp:Literal></strong>:</td>
                            <td>
                                <telerik:RadEditor runat="server" ID="txtCompanyDesc" Height="220" Skin="Telerik"
                                    ToolsFile="~/Config/RadEditor.xml">
                                </telerik:RadEditor>
                                <asp:Literal runat="server" ID="txtCompanyDescView" Visible="false"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top; text-align: right">
                                <strong>
                                    <asp:Literal runat="server" ID="lblJobDesc" Text="Job Description"></asp:Literal></strong>:</td>
                            <td>
                                <telerik:RadEditor runat="server" ID="txtJobDesc" Height="220" Skin="Telerik" ToolsFile="~/Config/RadEditor.xml">
                                </telerik:RadEditor>
                                <asp:Literal runat="server" ID="txtJobDescView" Visible="false"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top; text-align: right">
                                <strong>
                                    <asp:Literal runat="server" ID="lblPersonalDesc" Text="Personal Description"></asp:Literal></strong>:</td>
                            <td>
                                <telerik:RadEditor runat="server" ID="txtPersonalDesc" Height="220" Skin="Telerik"
                                    ToolsFile="~/Config/RadEditor.xml">
                                </telerik:RadEditor>
                                <asp:Literal runat="server" ID="txtPersonalDescView" Visible="false"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top; text-align: right">
                                <strong>
                                    <asp:Literal runat="server" ID="lblPackageDesc" Text="Package Description"></asp:Literal></strong>:</td>
                            <td>
                                <telerik:RadEditor runat="server" ID="txtPackageDesc" Height="220" Skin="Telerik"
                                    ToolsFile="~/Config/RadEditor.xml">
                                </telerik:RadEditor>
                                <asp:Literal runat="server" ID="txtPackageDescView" Visible="false"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </telerik:RadPageView>
                <telerik:RadPageView ID="NLView" runat="server">
                    <table width="100%">
                        <tr>
                            <td style="width: 20%; text-align: right">
                                <strong>
                                    <asp:Literal runat="server" ID="lblTitleNL" Text="Title (NL)"></asp:Literal></strong>:</td>
                            <td style="text-align: left;">
                                <asp:TextBox runat="server" ID="txtTitleNL" Width="300" MaxLength="128"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top; text-align: right">
                                <strong>
                                    <asp:Literal runat="server" ID="lblCompanyDescNL" Text="Company Description (NL)"></asp:Literal></strong>:</td>
                            <td>
                                <telerik:RadEditor runat="server" ID="txtCompanyDescNL" Height="220" Skin="Telerik"
                                    ToolsFile="~/Config/RadEditor.xml">
                                </telerik:RadEditor>
                                <asp:Literal runat="server" ID="txtCompanyDescNLView" Visible="false"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top; text-align: right">
                                <strong>
                                    <asp:Literal runat="server" ID="lblJobDescNL" Text="Job Description (NL)"></asp:Literal></strong>:</td>
                            <td>
                                <telerik:RadEditor runat="server" ID="txtJobDescNL" Height="220" Skin="Telerik" ToolsFile="~/Config/RadEditor.xml">
                                </telerik:RadEditor>
                                <asp:Literal runat="server" ID="txtJobDescNLView" Visible="false"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top; text-align: right">
                                <strong>
                                    <asp:Literal runat="server" ID="lblPersonalDescNL" Text="Personal Description (NL)"></asp:Literal></strong>:</td>
                            <td>
                                <telerik:RadEditor runat="server" ID="txtPersonalDescNL" Height="220" Skin="Telerik"
                                    ToolsFile="~/Config/RadEditor.xml">
                                </telerik:RadEditor>
                                <asp:Literal runat="server" ID="txtPersonalDescNLView" Visible="false"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top; text-align: right">
                                <strong>
                                    <asp:Literal runat="server" ID="lblPackageDescNL" Text="Package Description (NL)"></asp:Literal></strong>:</td>
                            <td>
                                <telerik:RadEditor runat="server" ID="txtPackageDescNL" Height="220" Skin="Telerik"
                                    ToolsFile="~/Config/RadEditor.xml">
                                </telerik:RadEditor>
                                <asp:Literal runat="server" ID="txtPackageDescNLView" Visible="false"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </telerik:RadPageView>
                <telerik:RadPageView ID="ENView" runat="server">
                    <table width="100%">
                        <tr>
                            <td style="width: 20%; text-align: right">
                                <strong>
                                    <asp:Literal runat="server" ID="lblTitleEN" Text="Title(EN)"></asp:Literal></strong>:</td>
                            <td style="text-align: left;">
                                <asp:TextBox runat="server" ID="txtTitleEN" Width="300" MaxLength="128"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top; text-align: right">
                                <strong>
                                    <asp:Literal runat="server" ID="lblCompanyDescEN" Text="Company Description (EN)"></asp:Literal></strong>:</td>
                            <td>
                                <telerik:RadEditor runat="server" ID="txtCompanyDescEN" Height="220" Skin="Telerik"
                                    ToolsFile="~/Config/RadEditor.xml">
                                </telerik:RadEditor>
                                <asp:Literal runat="server" ID="txtCompanyDescENView" Visible="false"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top; text-align: right">
                                <strong>
                                    <asp:Literal runat="server" ID="lblJobDescEN" Text="Job Description (EN)"></asp:Literal></strong>:</td>
                            <td>
                                <telerik:RadEditor runat="server" ID="txtJobDescEN" Height="220" Skin="Telerik" ToolsFile="~/Config/RadEditor.xml">
                                </telerik:RadEditor>
                                <asp:Literal runat="server" ID="txtJobDescENView" Visible="false"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top; text-align: right">
                                <strong>
                                    <asp:Literal runat="server" ID="lblPersonalDescEN" Text="Personal Description (EN)"></asp:Literal></strong>:</td>
                            <td>
                                <telerik:RadEditor runat="server" ID="txtPersonalDescEN" Height="220" Skin="Telerik"
                                    ToolsFile="~/Config/RadEditor.xml">
                                </telerik:RadEditor>
                                <asp:Literal runat="server" ID="txtPersonalDescENView" Visible="false"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top; text-align: right">
                                <strong>
                                    <asp:Literal runat="server" ID="lblPackageDescEN" Text="Package Description (EN)"></asp:Literal></strong>:</td>
                            <td>
                                <telerik:RadEditor runat="server" ID="txtPackageDescEN" Height="220" Skin="Telerik"
                                    ToolsFile="~/Config/RadEditor.xml">
                                </telerik:RadEditor>
                                <asp:Literal runat="server" ID="txtPackageDescENView" Visible="false"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </telerik:RadPageView>
            </telerik:RadMultiPage>
        </div>
        <table width="100%">
            <tr>                
                <td style="width:60%; text-align:right">
                    <asp:Button runat="server" ID="btnEdit" CssClass="flatButton" Text="Edit" OnClick="OnButtonEdit_Click"
                        Visible="true" CausesValidation="false" />&nbsp;
                    <asp:Button runat="server" ID="btnSave" CssClass="flatButton" Text="Save" OnClick="OnButtonSave_Click"
                        Visible="false" CausesValidation="true" ValidationGroup="JobProfileValidation"/>&nbsp;
                    <asp:Button runat="server" ID="btnCancel" CssClass="flatButton" Text="Cancel" OnClick="OnButtonCancel_Click"
                        Visible="false" CausesValidation="false"/>
                    <asp:Button runat="server" ID="btnPreview" CssClass="flatButton" Text="Preview" CausesValidation="false" />
                </td>
                <td style="width:40%; text-align:right">
                    <asp:HyperLink runat="server" ID="lnkBackToCompany" Text="Back to company"></asp:HyperLink>
                </td>
            </tr>
        </table>            
        <div>
            <asp:HiddenField ID="hidMode" runat="server" />
        </div>
        </div>
    </form>
</body>
</html>
