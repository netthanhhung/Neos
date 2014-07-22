<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CandidateAdvancedSearch.aspx.cs"
    Inherits="CandidateAdvancedSearch" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Candidate Advanced Search Page</title>
    <link href="Styles/Neos.css" rel="Stylesheet" />

    <script type="text/javascript" src="script/utils.js"></script>

</head>
<body>

    <script type="text/javascript">
    function ddlUnitClientChanged(sender, eventArgs)
    {
        $find("MyAjaxManager").ajaxRequest("ddlUnitClientChanged");
    }
    function ddlKnowledgeFamClientChanged(sender, eventArgs)
    {
        $find("MyAjaxManager").ajaxRequest("ddlKnowledgeFamClientChanged");
    }
    function ddlFunctionFamClientChanged(sender, eventArgs)
    {
        $find("MyAjaxManager").ajaxRequest("ddlFunctionFamClientChanged");
    }
    
    </script>

    <form id="candidateAdvancedSearchForm" runat="server">
        <telerik:RadScriptManager ID="ScriptManager" runat="server">
        </telerik:RadScriptManager>
        <div>
            <telerik:RadAjaxManager runat="server" ID="MyAjaxManager" OnAjaxRequest="OnMyAjaxManager_AjaxRequest">
                <AjaxSettings>
                    <%--<telerik:AjaxSetting AjaxControlID="ddlUnit">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="ddlFunctionFam" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>   
                    <telerik:AjaxSetting AjaxControlID="ddlUnit">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="ddlKnowledgeFam" />
                        </UpdatedControls>
                    </telerik:AjaxSetting> 
                    
                    <telerik:AjaxSetting AjaxControlID="ddlUnit">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="listFunctionOriginal" />
                        </UpdatedControls>
                    </telerik:AjaxSetting> 
                    <telerik:AjaxSetting AjaxControlID="ddlUnit">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="listKnowledgeOriginal" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>  --%>
                    
                    <telerik:AjaxSetting AjaxControlID="ddlFunctionFam">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="listFunctionOriginal" />
                        </UpdatedControls>
                    </telerik:AjaxSetting> 
                    <telerik:AjaxSetting AjaxControlID="ddlKnowledgeFam">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="listKnowledgeOriginal" />
                        </UpdatedControls>
                    </telerik:AjaxSetting> 
                    <telerik:AjaxSetting AjaxControlID="btnStudyAdd">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="listStudyDestination" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="btnStudyRemove">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="listStudyDestination" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="btnKnowledgeAdd">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="listKnowledgeDestination" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="btnKnowledgeRemove">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="listKnowledgeDestination" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="btnFunctionAdd">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="listFunctionDestination" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="btnFunctionRemove">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="listFunctionDestination" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
            <div runat="server" id="contentPanel">
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr>                       
                        <td style="width:50%"></td>
                        <td style="text-align:left;">
                            <asp:Label ID="lblUnit" runat="server" Text="Unit"></asp:Label>
                            <telerik:RadComboBox ID="ddlUnit" runat="server" Width="150px" Skin="Office2007"
                                OnClientSelectedIndexChanged="ddlUnitClientChanged">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" colspan="2">
                            <table width="100%" style="margin-top: 0px;">
                                <tr style="vertical-align: top">
                                    <td>
                                        <fieldset>
                                            <legend>
                                                <asp:Label runat="server" ID="lblGeneralField" Text="General"></asp:Label>
                                            </legend>
                                            <table>
                                                <tr>
                                                    <td style="width: 100px">
                                                        <asp:Label ID="lblLastName" runat="server" Text="Last name"></asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <telerik:RadTextBox ID="txtLastName" runat="server" Width="210px">
                                                        </telerik:RadTextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblFirstName" runat="server" Text="First name"></asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <telerik:RadTextBox ID="txtFirstName" runat="server" Width="210px">
                                                        </telerik:RadTextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAgeFrom" runat="server" Text="Age"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <telerik:RadComboBox ID="ddlAgeFrom" runat="server" Skin="Office2007" Height="150px"
                                                            Width="85px">
                                                        </telerik:RadComboBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblAgeTo" runat="server" Text="to"></asp:Label>
                                                        <telerik:RadComboBox ID="ddlAgeTo" runat="server" Skin="Office2007" Height="150px"
                                                            Width="85px">
                                                        </telerik:RadComboBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblActiveCan" runat="server" Text="Active"></asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:RadioButton ID="radActiveCanYes" runat="server" GroupName="ActiveGroup" />
                                                        <asp:Label ID="lblActiveCanYes" runat="server" Text="Yes"></asp:Label>
                                                        <asp:RadioButton ID="radActiveCanNo" runat="server" GroupName="ActiveGroup" />
                                                        <asp:Label ID="lblActiveCanNo" runat="server" Text="No"></asp:Label>
                                                        <asp:RadioButton ID="radActiveCanBoth" runat="server" GroupName="ActiveGroup" Checked="true" />
                                                        <asp:Label ID="lblActiveCanBoth" runat="server" Text="Both"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblInterviewer" runat="server" Text="Interviewer"></asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <telerik:RadComboBox ID="ddlInterviewer" runat="server" Skin="Office2007" Width="214px">
                                                        </telerik:RadComboBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDateInterviewFrom" runat="server" Text="Date interview"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <telerik:RadDatePicker ID="datInterviewFrom" runat="server" Width="90px" Skin="Office2007"  Calendar-CultureInfo="en-US">
                                                            <DateInput ID="dateInputDatInterviewFrom" runat="server" DateFormat="dd/MM/yyyy"
                                                                DisplayDateFormat="dd/MM/yyyy">
                                                            </DateInput>
                                                        </telerik:RadDatePicker>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblDateInterTo" runat="server" Text="to"></asp:Label>
                                                        <telerik:RadDatePicker ID="datInterviewTo" runat="server" Width="90px" Skin="Office2007"  Calendar-CultureInfo="en-US">
                                                            <DateInput ID="dateInputDatInterviewTo" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd/MM/yyyy">
                                                            </DateInput>
                                                        </telerik:RadDatePicker>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                    <td rowspan="2">
                                        <fieldset style="height:100%">
                                            <legend>
                                                <asp:Label ID="lblKnowlegde" runat="server" Text="Knowledge"></asp:Label>
                                            </legend>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblFrenchLang" runat="server" Text="French" />
                                                    </td>
                                                    <td style="width: 150px">
                                                        <telerik:RadComboBox ID="ddlFrenchLangLevel" runat="server" Width="130px" Skin="Office2007"
                                                            EnableViewState="true" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblGermanLang" runat="server" Text="German" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadComboBox ID="ddlGermanLangLevel" runat="server" Width="130px" Skin="Office2007" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDutchLang" runat="server" Text="Dutch" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadComboBox ID="ddlDutchLangLevel" runat="server" Width="130px" Skin="Office2007" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblSpanishLange" runat="server" Text="Spanish" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadComboBox ID="ddlSpanishLangeLevel" runat="server" Width="130px" Skin="Office2007" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblEnglishLang" runat="server" Text="English" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadComboBox ID="ddlEnglishLangLevel" runat="server" Width="130px" Skin="Office2007" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblItalianLang" runat="server" Text="Italian" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadComboBox ID="ddlItalianLangLevel" runat="server" Width="130px" Skin="Office2007" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <telerik:RadComboBox ID="ddlOtherLang" runat="server" Width="80px" Height="300" Skin="Office2007" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadComboBox ID="ddlOtherLangLevel" runat="server" Width="130px" Height="150px"
                                                            Skin="Office2007" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" style="width: 100px;">
                                                        <table>
                                                            <tr>
                                                                <td valign="top">
                                                                    <telerik:RadComboBox ID="ddlKnowledgeFam" runat="server" Width="170px" Height="150px"
                                                                        Skin="Office2007" 
                                                                        OnClientSelectedIndexChanged="ddlKnowledgeFamClientChanged"/>
                                                                </td>
                                                                <td rowspan="3">
                                                                    <asp:Button ID="btnKnowledgeAdd" runat="server" Text="Add" CssClass="flatButton"
                                                                        Width="75px" OnClick="OnBtnKnowledgeAddClicked" />
                                                                   <div style="margin-top:10px;">
                                                                    <asp:Button ID="btnKnowledgeRemove" runat="server" Text="Remove" CssClass="flatButton"
                                                                        Width="75px" OnClick="OnBtnKnowledgeRemoveClicked" />
                                                                        </div>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblKnowledgeSelection" runat="server" Text="Knowledge selection"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:ListBox ID="listKnowledgeOriginal" runat="server" Height="157px" Width="170px"
                                                                        SelectionMode="Multiple"></asp:ListBox>
                                                                </td>
                                                                <td>
                                                                    <asp:ListBox ID="listKnowledgeDestination" runat="server" Height="157px" Width="170px"
                                                                        SelectionMode="Multiple"></asp:ListBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="radKnowledgeOne" runat="server" GroupName="KnowledgeSearch"
                                                                        Checked="true" />
                                                                    <asp:Label ID="lblKnowlegdeHaveOne" runat="server" Text="Must have at least one"></asp:Label>
                                                                    <br />
                                                                    <asp:RadioButton ID="radKnowledgeAll" runat="server" GroupName="KnowledgeSearch" />
                                                                    <asp:Label ID="lblKnowlegdeHaveAll" runat="server" Text="Must have all"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align:bottom;">
                                        <fieldset style="height:100%">
                                            <legend>
                                                <asp:Label ID="lblSignage" runat="server" Text="Location"></asp:Label>
                                            </legend>
                                            <table cellpadding="0" cellspacing="0" style="width: 350px; border: none; ">
                                                <tr style="vertical-align:top;">
                                                    <td style="width: 106px;">
                                                        <asp:Literal runat="server" ID="lblLocation" Text="Location"></asp:Literal>
                                                    </td>
                                                    <td style="padding-top: 4px;">
                                                        <asp:ListBox runat="server" ID="listLocation" SelectionMode="Multiple" Height="139"
                                                            Width="215"></asp:ListBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>                                        
                                    </td>
                                </tr>                                
                                
                                <tr>
                                    <td>
                                        <fieldset>
                                            <legend>
                                                <asp:Label ID="lblFormation" runat="server" Text="Formation"></asp:Label>
                                            </legend>
                                            <table>
                                                <tr>
                                                    <td colspan="3" style="width: 100px">
                                                        <table>
                                                            <tr>
                                                                <td valign="top">
                                                                    <asp:Label ID="lblStudyBackgroundLevel" runat="server" Text="Background level"></asp:Label>
                                                                </td>
                                                                <td></td>
                                                                <td>
                                                                    <asp:Label ID="lblStudySelection" runat="server" Text="Study selection"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top">
                                                                    <telerik:RadComboBox ID="ddlStudyBackgroundLevel" runat="server" Skin="Office2007"
                                                                        Height="150px">
                                                                    </telerik:RadComboBox>
                                                                    <br />
                                                                    <asp:Label ID="lblStudyOrentation" runat="server" Text="Orentation"></asp:Label>
                                                                    <br />
                                                                    <telerik:RadComboBox ID="ddlStudyOrentation" runat="server" Skin="Office2007" Height="150px">
                                                                    </telerik:RadComboBox>
                                                                </td>
                                                                <td style="vertical-align:middle;">
                                                                    <div><asp:Button ID="btnStudyAdd" runat="server" Text="Add" CssClass="flatButton" Width="75px"
                                                                        OnClick="OnBtnStudyAddClicked" /></div>
                                                                    <div style="margin-top:10px;">
                                                                    <asp:Button ID="btnStudyRemove" runat="server" Text="Remove" CssClass="flatButton"
                                                                        Width="75px" OnClick="OnBtnStudyRemoveClicked" /></div>
                                                                </td>
                                                                <td>
                                                                    <asp:ListBox ID="listStudyDestination" runat="server" Height="150px" Width="180px"
                                                                        SelectionMode="Multiple"></asp:ListBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td></td>
                                                                <td>
                                                                    <asp:RadioButton ID="radStudyOne" runat="server" GroupName="StudySearch" Checked="true" />
                                                                    <asp:Label ID="lblStudyHaveOne" runat="server" Text="Must have at least one"></asp:Label>
                                                                    <br />
                                                                    <asp:RadioButton ID="radStudyAll" runat="server" GroupName="StudySearch" />
                                                                    <asp:Label ID="lblStudyHaveAll" runat="server" Text="Must have all"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <fieldset>
                                            <legend>
                                                <asp:Label ID="lblFunction" runat="server" Text="Function"></asp:Label>
                                            </legend>
                                            <table>
                                                <tr>
                                                    <td colspan="3" style="width: 100px;">
                                                        <table>
                                                            <tr>
                                                                <td valign="top">
                                                                    <telerik:RadComboBox ID="ddlFunctionFam" runat="server" Width="170px" Height="150px"
                                                                        Skin="Office2007" OnClientSelectedIndexChanged="ddlFunctionFamClientChanged" />
                                                                </td>
                                                                <td rowspan="3">
                                                                    <asp:Button ID="btnFunctionAdd" runat="server" Text="Add" CssClass="flatButton" Width="75px"
                                                                        OnClick="OnBtnFunctionAddClicked" />
                                                                    <div style="margin-top:10px;">
                                                                    <asp:Button ID="btnFunctionRemove" runat="server" Text="Remove" CssClass="flatButton"
                                                                        Width="75px" OnClick="OnBtnFunctionRemoveClicked" />
                                                                        </div>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblFunctionSelection" runat="server" Text="Function selection"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:ListBox ID="listFunctionOriginal" runat="server" Height="150px" Width="170px"
                                                                        SelectionMode="Multiple"></asp:ListBox>
                                                                </td>
                                                                <td>
                                                                    <asp:ListBox ID="listFunctionDestination" runat="server" Height="150px" Width="170px"
                                                                        SelectionMode="Multiple"></asp:ListBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="radFunctionOne" runat="server" GroupName="FunctionSearch" Checked="true" />
                                                                    <asp:Label ID="lblFunctionHaveOne" runat="server" Text="Must have at least one"></asp:Label>
                                                                    <br />
                                                                    <asp:RadioButton ID="radFunctionAll" runat="server" GroupName="FunctionSearch" />
                                                                    <asp:Label ID="lblFunctionHaveAll" runat="server" Text="Must have all"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <%--<td style="width: 20px">
                        </td>
                        <td valign="top">
                            <table width="100%" style="margin-top: 0px;">
                                <tr style="vertical-align: top">
                                    <td>
                                        
                                        
                                    </td>
                                </tr>
                            </table>
                        </td>--%>
                    </tr>
                    <tr align="center">
                        <td colspan="2">
                            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="flatButton" Width="95px"
                                OnClick="OnBtnSearchClicked" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
