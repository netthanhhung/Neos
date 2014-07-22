<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompanyContactPopup.aspx.cs"
    Inherits="CompanyContactPopup" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Contact</title>
    <link href="Styles/Neos.css" rel="Stylesheet" />

    <script type="text/javascript" src="script/utils.js"></script>

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
            return false;
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
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="ScriptManager" runat="server" />
        <div>
            <center>
                <table width="100%">
                    <tr>
                        <td style="text-align: left; width: 30%;">
                            <asp:Literal runat="server" ID="lblLastName"></asp:Literal>:</td>
                        <td style="text-align: left;">
                            <telerik:RadTextBox runat="server" ID="txtLastName" Width="98%" MaxLength="60"></telerik:RadTextBox></td>
                    </tr>
                    <tr>
                        <td style="text-align: left;">
                            <asp:Literal runat="server" ID="lblFirstName"></asp:Literal>:</td>
                        <td style="text-align: left;">
                            <telerik:RadTextBox runat="server" ID="txtFirstName" Width="98%" MaxLength="60"></telerik:RadTextBox></td>
                    </tr>
                    <tr>
                        <td style="text-align: left;">
                            <asp:Literal runat="server" ID="lblFunction"></asp:Literal>:</td>
                        <td style="text-align: left;">
                            <telerik:RadComboBox runat="server" ID="ddlFunction" Skin="Office2007" AllowCustomText="true"  Width="100%"
                            DataValueField="ContactFunctionID" DataTextField="FunctionName">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left;">
                            <asp:Literal runat="server" ID="lblGender"></asp:Literal>:</td>
                        <td style="text-align: left;">
                            <telerik:RadComboBox runat="server" ID="ddlGender" Skin="Office2007"  Width="100">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left;">
                            <asp:Literal runat="server" ID="lblLanguage"></asp:Literal>:</td>
                        <td style="text-align: left;">
                            <telerik:RadTextBox runat="server" ID="txtLanguage"  Width="96" MaxLength="1"></telerik:RadTextBox></td>
                    </tr>
                    <tr style="vertical-align:top;">
                        <td style="text-align: left;">
                            <asp:Literal runat="server" ID="lblRemark"></asp:Literal>:</td>
                        <td style="text-align: left; ">
                            <telerik:RadTextBox runat="server" ID="txtRemark"  Width="98%" TextMode="MultiLine" Height="100"></telerik:RadTextBox></td>
                    </tr>
                    <tr>
                        <td style="text-align: left;"></td>
                        <td style="text-align: left;">
                            <asp:CheckBox runat="server" ID="chkReceiveNeosNewsletter" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left;">
                        </td>
                        <td style="text-align: left;">
                            <asp:Button runat="server" ID="btnSave" CssClass="flatButton" OnClick="OnButtonSaveClick" />&nbsp;
                            <asp:Button runat="server" ID="btnCancel" CssClass="flatButton"  OnClientClick="return OnBtnCancelClientClicked();" />
                        </td>
                    </tr>
                </table>
            </center>
        </div>
    </form>
</body>
</html>
