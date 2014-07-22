<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminUserPopup.aspx.cs" Inherits="AdminUserPopup" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>User Popup</title>
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
    <form id="ediUserForm" runat="server">
    <telerik:RadScriptManager ID="ScriptManager" runat="server" /> 
    <asp:ValidationSummary runat="server" ID="sum" ShowMessageBox="true" ShowSummary="false" ValidationGroup="AdminUserValidation" />
    
    <asp:RequiredFieldValidator runat="server" ID="rfvAdminUser" ControlToValidate="txtUserID" 
        ValidationGroup="AdminUserValidation" Display="None" EnableClientScript="true"></asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator runat="server" ID="revAdminUser" ControlToValidate="txtEmail" 
        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
        ValidationGroup="AdminUserValidation" Display="None"></asp:RegularExpressionValidator>
    <asp:CompareValidator runat="server" ID="cvAdminUser" ControlToValidate="txtNewPassword" ControlToCompare="txtConfirmPassword" 
        ValidationGroup="AdminUserValidation" Display="None"></asp:CompareValidator>
    <div>
        <table>
            <tr>
                <td style="width:120px">
                    <asp:Label ID="lblUserID" runat="server" Text="UserID"></asp:Label>
                </td>
                <td style="width:200px">
                    <telerik:RadTextBox ID="txtUserID" runat="server" Width="200px" MaxLength="3"></telerik:RadTextBox>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblName" runat="server" Text="Name"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtName" runat="server" Width="200px" MaxLength="60"></telerik:RadTextBox>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblGenre" runat="server" Text="Genre" />
                </td>
                <td colspan="3">
                    <telerik:RadComboBox ID="ddlGenre" runat="server" Width="60" Skin="Office2007" />
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblEmail" runat="server" Text="Email"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtEmail" runat="server" Width="200px" MaxLength="60"></telerik:RadTextBox>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblTelephone" runat="server" Text="Telephone"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtTelephone" runat="server" Width="200px" MaxLength="100"></telerik:RadTextBox>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblActive" runat="server" Text="Active"></asp:Label>
                </td>
                <td>
                    <asp:CheckBox ID="chkActive" runat="server"></asp:CheckBox>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblPermission" runat="server" Text="Permission" />
                </td>                                
                <td>
                    <telerik:RadComboBox ID="ddlPermission" runat="server" Width="200px" Skin="Office2007"></telerik:RadComboBox>
                </td>
                <td>
                    <asp:Button ID="btnAddPermission" runat="server" CssClass="flatButton" Width="120px" 
                        Text="Add" OnClientClick="return OnButtonAddPermissionClientClicked();" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <asp:ListBox ID="listPermission" runat="server" Width="200px" Height="80px"></asp:ListBox>
                </td>
                <td valign="top">
                    <asp:Button ID="btnRemovePermission" runat="server" Width="120px" CssClass="flatButton" 
                        Text="Remove" OnClientClick="return OnButtonRemovePermissionClientClicked();" />
                </td>
            </tr>           
            <tr>
                <td>
                    <asp:Label ID="lblPassword" runat="server" Text="Password"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPassword" runat="server" Width="195px" CssClass="input" Enabled="false"
                        TextMode="Password" MaxLength="255"></asp:TextBox>
                </td>
                <td align="left">
                    <asp:Button ID="btnChangePassword" runat="server" Width="120px" Text="Change password" 
                        CssClass="flatButton" OnClick="OnBtnChangePasswordClicked"/>                    
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <div id="divChangePassword" runat="server">
                        <table width="100%">
                            <tr>
                                <td style="width:117px">
                                    <asp:Label ID="lblNewPassword" runat="server" Text="New password"></asp:Label>
                                </td>
                                <td style="width:200px">
                                    <asp:TextBox ID="txtNewPassword" runat="server" Width="195px" CssClass="input"
                                        TextMode="Password" MaxLength="255" EnableViewState="true"></asp:TextBox>                    
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblConfirmPassword" runat="server" Text="Confirm password"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtConfirmPassword" runat="server" Width="195px" CssClass="input"
                                        TextMode="Password" MaxLength="255" EnableViewState="true"></asp:TextBox>                    
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </div>
                </td>                
            </tr>            
        </table>  
        <asp:HiddenField ID="hiddenPermissionList" runat="server" />  
        <div style="text-align:center">
            <asp:Button runat="server" ID="btnSave" Text="Save" CausesValidation="true" ValidationGroup="AdminUserValidation" CssClass="flatButton" Width="60" OnClick="OnBtnSaveClicked"/>&nbsp;
            <asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="flatButton" Width="60" OnClientClick="OnBtnCancelClientClicked()"/>
        </div>    
    </div>
    </form>
</body>
</html>
