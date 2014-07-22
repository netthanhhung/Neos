<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Welcome.aspx.cs" Inherits="Welcome" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Welcome</title>
    <link href="Styles/Neos.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="script/utils.js"></script>

</head>
<body>
    <form id="form1" runat="server">
    <div class="welcome_text"><asp:Literal runat="server" ID="lblWelcomeMessage"></asp:Literal></div>
    <div style="margin-top:10px;"><asp:Literal runat="server" ID="lblNotificationMessage"></asp:Literal></div>
    <div style="margin-top:10px;"><asp:Button runat="server" ID="btnStartNeos" Text="Start Néos Project beta" Width="250"  /></div>
    </form>
</body>
</html>
