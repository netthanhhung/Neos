<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CandidateStudyPopup.aspx.cs" Inherits="CandidateStudyPopup" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Candidate Study Popup</title>
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
    <form id="canStudyPopup" runat="server">
    <telerik:RadScriptManager ID="ScriptManager" runat="server" />         
    <div>
         <table width="100%">
            <tr>
                <td>
                    <asp:Label ID="lblPeriode" runat="server" Text="Periode"></asp:Label>
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblPeriodeFrom" runat="server" Text="From"></asp:Label>
                                <telerik:RadComboBox ID="ddlPeriodeFrom" runat="server" Width="55px" Skin="Office2007"></telerik:RadComboBox>                    
                            </td>
                            <td style="padding-left:30px">
                                <asp:Label ID="lblPeriodeTo" runat="server" Text="To"></asp:Label>
                                <telerik:RadComboBox ID="ddlPeriodeTo" runat="server" Width="55px" Skin="Office2007"></telerik:RadComboBox>                            
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
                    <asp:Label ID="lblTraining" runat="server" Text="Training"></asp:Label>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlTraining" runat="server" Width="204px" Skin="Office2007"></telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblDiploma" runat="server" Text="Diploma"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtDiploma" runat="server" Width="200px" MaxLength="255"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblLevel" runat="server" Text="Level"></asp:Label>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlLevel" runat="server" Width="204px" Skin="Office2007"></telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblSchool" runat="server" Text="School"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtSchool" runat="server" Width="200px" MaxLength="100"></telerik:RadTextBox>
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
