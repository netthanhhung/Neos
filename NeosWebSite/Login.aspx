<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <link href="Styles/Neos.css" rel="stylesheet" />

    <script src="script/utils.js" type="text/javascript"></script>
    <script type="text/javascript">
    
    </script>
</head>
<body onresize="centerLoginControl()">
    <form id="form1" runat="server">
        <asp:ValidationSummary runat="server" ID="validSum" ShowMessageBox="true" ShowSummary="false" />
        <center>
            <table style="width: 335px; position: fixed" id="login" class="bordertable">
                <tr>
                    <td class="bottomborder" colspan="2" style="text-align: left;">
                        <p class="logintitle" style="text-align: left; margin: 2px 0px 2px 2px;">
                            <img src="images/signin.gif" border="0" alt="" />
                            <asp:Literal runat="server" ID="lblSignIn" Text="Sign in"></asp:Literal>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                    </td>
                </tr>
                <tr>
                    <td style="width: 40%; text-align: right" class="logintitle2">
                        <asp:Literal runat="server" ID="lblUserName" Text="User name"></asp:Literal>
                    </td>
                    <td style="text-align: left">
                        <asp:TextBox runat="server" ID="txtUserID" CssClass="input"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ID="rfvUserID" ControlToValidate="txtUserID"
                            ErrorMessage="" Display="None"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 40%; text-align: right" class="logintitle2">
                        <asp:Literal runat="server" ID="lblPassword" Text="Password"></asp:Literal>
                    </td>
                    <td style="text-align: left">
                        <asp:TextBox runat="server" ID="txtPassword" TextMode="Password" CssClass="input"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 40%; text-align: right">
                    </td>
                    <td style="text-align: left">
                        <asp:CheckBox runat="server" ID="chkRemember" Text="Remember me next time" Checked="true"
                            CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 40%; text-align: right">
                    </td>
                    <td style="text-align: right" class="submit">
                        <asp:Button runat="server" ID="btnSignIn" Text="Sign in" CssClass="loginsubmit"
                            OnClick="OnButtonSignIn_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                    </td>
                </tr>
            </table>
        </center>
    </form>
</body>
</html>

