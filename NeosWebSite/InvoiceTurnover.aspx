<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InvoiceTurnover.aspx.cs" aspCompat="True" Inherits="InvoiceTurnover" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Turnover Page</title>
    <link href="Styles/Neos.css" rel="Stylesheet" />

    <script type="text/javascript" src="script/utils.js"></script>
</head>
<body>
    <form id="turnoverForm" runat="server">
    <telerik:RadScriptManager ID="turnoverScriptManager" runat="server" />   
    <asp:MultiView runat="server" ID="TurnOverMView" ActiveViewIndex="0">
    <asp:View runat="server" ID="InvoiceTurnOverView">
    <div class="rightpane_title">
                <asp:Literal runat="server" ID="lblTitle" Text="Turnover"></asp:Literal></div>
    <div style="margin-top: 30px;">
        <fieldset style="width:300px">
            <legend>
                <asp:Label runat="server" ID="lblTurnover" Text="Turnover"></asp:Label></legend>
            <table width="300px">
                <tr>
                    <td style="width:150px">
                        <asp:Label runat="server" ID="lblStartDate" Text="Start date :"></asp:Label>        
                    </td>
                    <td  style="width:150px">
                        <asp:Label runat="server" ID="lblEndDate" Text="End date :"></asp:Label>        
                    </td>
                </tr>
                 <tr>
                    <td>
                        <telerik:RadDatePicker ID="datStartDate" runat="server" Width="104px" Skin="Office2007"  Calendar-CultureInfo="en-US">
                            <DateInput ID="datInputStartDate" runat="server" DateFormat="dd/MM/yyyy"
                                DisplayDateFormat="dd/MM/yyyy">
                            </DateInput>
                        </telerik:RadDatePicker>
                    </td>
                    <td>
                         <telerik:RadDatePicker ID="datEndDate" runat="server" Width="104px" Skin="Office2007"  Calendar-CultureInfo="en-US">
                            <DateInput ID="DateInput1" runat="server" DateFormat="dd/MM/yyyy"
                                DisplayDateFormat="dd/MM/yyyy">
                            </DateInput>
                        </telerik:RadDatePicker>       
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Button ID="btnGo" runat="server" Text="Go" CssClass="flatButton" Width="100px" OnClick="OnBtnGoClicked" />
                    </td>                    
                </tr>
                 <tr>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblResult" Text="Turn over (€)"></asp:Label>                                      
                        <asp:TextBox runat="server" ID="txtResult" width="100px"></asp:TextBox>        
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    </asp:View>
    <asp:View runat="server" ID="NoPermissionView">
            <div class="nopermission"><asp:Literal runat="server" ID="lblNoPermission"></asp:Literal></div>
    </asp:View>
    </asp:MultiView>
    </form>
</body>
</html>
