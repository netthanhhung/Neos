<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminLocationPopup.aspx.cs" Inherits="AdminLocationPopup" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Add/Edit location Page</title>
    <script src="../script/utils.js" type="text/javascript"></script>
    <link href="../Styles/Neos.css" rel="Stylesheet" />
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
    <form id="addEditLocationForm" runat="server">
    <telerik:RadScriptManager ID="ScriptManager" runat="server" /> 
    <asp:ValidationSummary runat="server" ID="sum" ShowMessageBox="true" ShowSummary="false" ValidationGroup="AdminLocationValidation" />
    
    <asp:RequiredFieldValidator runat="server" ID="rfvAdminLocation1" ControlToValidate="txtLocation" 
        ValidationGroup="AdminLocationValidation" Display="None" EnableClientScript="true"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator runat="server" ID="rfvAdminLocation2" ControlToValidate="txtCode" 
        ValidationGroup="AdminLocationValidation" Display="None" EnableClientScript="true"></asp:RequiredFieldValidator>
    <div>
        <table>
            <tr>
                <td style="width:120px">
                    <asp:Label ID="lblLocation" runat="server" Text="Location"></asp:Label>
                </td>
                <td style="width:200px">
                    <telerik:RadTextBox ID="txtLocation" runat="server" Width="200px" MaxLength="60"></telerik:RadTextBox>
                </td>                
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblHierarchy" runat="server" Text="Hierarchy"></asp:Label>
                </td>
                <td>
                    <telerik:RadNumericTextBox ID="txtHierarchy" runat="server" Width="200px" 
                        Type="Number" Skin="Office2007" MaxLength="2"
                        NumberFormat-DecimalDigits="0" NumberFormat-PositivePattern="n"
                        NumberFormat-GroupSeparator="" />
                </td>                
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblLocationUk" runat="server" Text="LocationUk"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtLocationUk" runat="server" Width="200px" MaxLength="50"></telerik:RadTextBox>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblLocationNl" runat="server" Text="LocationNl"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtLocationNl" runat="server" Width="200px" MaxLength="60"></telerik:RadTextBox>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblCode" runat="server" Text="Code"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtCode" runat="server" Width="200px" MaxLength="10"></telerik:RadTextBox>
                </td>
                <td></td>
            </tr>
        </table>
        <div style="text-align:center">
            <asp:Button runat="server" ID="btnSave" Text="Save" CausesValidation="true" 
                ValidationGroup="AdminLocationValidation" CssClass="flatButton" Width="60" OnClick="OnBtnSaveClicked"/>&nbsp;
            <asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="flatButton" Width="60" 
                OnClientClick="OnBtnCancelClientClicked()"/>
        </div> 
    </div>
    </form>
</body>
</html>
