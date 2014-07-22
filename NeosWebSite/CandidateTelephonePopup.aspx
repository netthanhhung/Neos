<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CandidateTelephonePopup.aspx.cs" Inherits="CandidateTelephonePopup" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Candidate Telephone Popup</title>
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
    <form id="canTelephonePopup" runat="server">
    <telerik:RadScriptManager ID="ScriptManager" runat="server" /> 
    <div>
         <table>
            <tr>
                <td>
                    <asp:Label ID="lblType" runat="server" Text="Type"></asp:Label>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlType" runat="server" Width="153px" Skin="Office2007"></telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblZone" runat="server" Text="Zone"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtZone" runat="server" Width="150px" MaxLength="6"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblPhoneMail" runat="server" Text="Phone/Mail"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtPhoneMail" runat="server" Width="150px" MaxLength="50"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblPlace" runat="server" Text="Place"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtPlace" runat="server" Width="150px" MaxLength="20"></telerik:RadTextBox>
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
