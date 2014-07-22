<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CandidateExperiencePopup.aspx.cs" Inherits="CandidateExperiencePopup" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Candidate Experience Popup</title>
    <link href="Styles/Neos.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript">
        function GetRadWindow()
        {
          var oWindow = null;
          if (window.radWindow)
             oWindow = window.radWindow;
          else if (window.frameElement.radWindow)
             oWindow = window.frameElement.radWindow;
          return oWindow;
        }
        
        function OnBtnCancelClientClicked(sender, eventArgs)         
        {         
            var currentWindow = GetRadWindow();
            var isReload = "No";
            currentWindow.argument = isReload;
            currentWindow.close();
        }
        
        function OnBtnSaveClientClicked()         
        {
            var currentWindow = GetRadWindow();
            var isReload = "Yes";
            currentWindow.argument = isReload;
            currentWindow.close();
        }
    </script>
</head>
<body>
    <form id="canExperiencePopup" runat="server">
    <telerik:RadScriptManager ID="ScriptManager" runat="server" /> 
    <div>
         <telerik:RadAjaxManager EnableAJAX="true" runat="server" ID="MyAjaxManager">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="ddlFuncUnit">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlFuncFam" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ddlFuncUnit">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlFunction" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ddlFuncFam">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlFunction" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>                        
         </telerik:RadAjaxManager>
         <table>
            <tr>
                <td style="width:130px">
                    <asp:Label ID="lblPeriode" runat="server" Text="Periode"></asp:Label>
                </td>
                <td style="width:400px">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblPeriodeFrom" runat="server" Text="From"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlPeriodeMonthFrom" runat="server" Width="40px" Skin="Office2007"></telerik:RadComboBox>
                            </td>
                            <td style="padding-left:2px">
                                <telerik:RadComboBox ID="ddlPeriodeYearFrom" runat="server" Width="55px" Skin="Office2007"></telerik:RadComboBox>
                            </td>
                            <td style="padding-left:15px">
                                <asp:Label ID="lblPeriodeTo" runat="server" Text="To"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlPeriodeMonthTo" runat="server" Width="40px" Skin="Office2007"></telerik:RadComboBox>
                            </td>
                            <td style="padding-left:2px">
                                <telerik:RadComboBox ID="ddlPeriodeYearTo" runat="server" Width="55px" Skin="Office2007"></telerik:RadComboBox>
                            </td>
                            <td style="padding-left:10px">
                                <telerik:RadTextBox ID="txtPeriodeString" runat="server" Width="100px" Enabled="false"></telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
             <tr>
                <td>
                    <asp:Label ID="lblCompany" runat="server" Text="Company"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtCompany" runat="server" Width="150px" MaxLength="90"></telerik:RadTextBox>
                </td>
            </tr>
             <tr>
                <td>
                    <asp:Label ID="lblSalary" runat="server" Text="Salary"></asp:Label>
                </td>
                <td>
                     <telerik:RadComboBox ID="ddlSalary" runat="server" Width="154px" AllowCustomText="true" Skin="Office2007" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblSalaryPackage" runat="server" Text="Salary package"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtSalaryPackage" runat="server" Width="150px" MaxLength="255"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblJobTitle" runat="server" Text="Job title"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtJobTitle" runat="server" Width="220px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblQuitReason" runat="server" Text="Quit reason"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtQuitReason" runat="server" Width="220px" MaxLength="255"></telerik:RadTextBox>
                </td>
            </tr>                                               
            <tr>
                <td>
                    <asp:Label ID="lblFunction" runat="server" Text="Function"></asp:Label>
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblFuncUnit" runat="server" Text="Unit:"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlFuncUnit" runat="server" AutoPostBack="true" Width="162"
                                     OnSelectedIndexChanged="OnFuncUnitItemChanged" Skin="Office2007">
                                 </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblFuncFam" runat="server" Text="Family:"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlFuncFam" runat="server" AutoPostBack="true" Width="162px"
                                     OnSelectedIndexChanged="OnFuncFamItemChanged" Skin="Office2007">
                                 </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblFunctionChild" runat="server" Text="Function:"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlFunction" runat="server" Width="162px" Skin="Office2007"></telerik:RadComboBox>
                            </td>
                        </tr>
                    </table>                                         
                </td>
            </tr>            
        </table>
        <div style="text-align:center">
            <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="flatButton" Width="60" OnClick="OnBtnSaveClicked"/>&nbsp;
            <asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="flatButton" Width="60" OnClientClick="OnBtnCancelClientClicked()"/>
        </div>
    </div>
    </form>
</body>
</html>
