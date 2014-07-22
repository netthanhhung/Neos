<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ActionExportPopup.aspx.cs" Inherits="ActionExportPopup" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Action Export Page</title>
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
        function OnBtnExportClientClicked()         
        {
            var currentWindow = GetRadWindow();
            var isReload = "No";
            currentWindow.argument = isReload;
            currentWindow.close();
        }
     </script>
</head>
<body>
    <form id="actionExportPage" runat="server">
    <telerik:RadScriptManager ID="ScriptManager" runat="server" /> 
    <div>
        <telerik:RadAjaxManager ID="RadAjaxManager" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="btnAdd">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="listEmail"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>                
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblEmail" runat="server" Text="Attender's email"></asp:Label>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="200" Width="200px"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="OnBtnAddClicked"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:ListBox ID="listEmail" runat="server" Width="200px" Height="200px"></asp:ListBox>
                </td>
                <td></td>
            </tr>
            <tr>
                <td colspan="2" valign="middle">
                    <asp:Button ID="btnExport" runat="server" Text="Export" OnClick="OnBtnExportClicked"/>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
