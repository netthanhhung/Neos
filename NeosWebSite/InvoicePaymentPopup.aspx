<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InvoicePaymentPopup.aspx.cs"
    Inherits="InvoicePaymentPopup" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Invoice payment</title>
    <link href="Styles/Neos.css" rel="Stylesheet" />

    <script type="text/javascript" src="script/utils.js"></script>
<telerik:RadScriptBlock runat="server" ID="scriptBlock">
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
</telerik:RadScriptBlock>
</head>
<body>
    <form id="invoicePaymentForm" runat="server">
        <telerik:RadScriptManager ID="ScriptManager" runat="server" />
        <div>            
            <center>
                <table width="500px">
                    <tr>
                        <td style="width:100px">
                            <asp:Label runat="server" ID="lblPaymentDate" Text="Payment date"></asp:Label>
                        </td>
                        <td style="width:400px" align="left">
                            <telerik:RadDatePicker ID="datPaymentDate" runat="server" Width="120px" Skin="Office2007"  Calendar-CultureInfo="en-US">
                                <DateInput ID="dateInputPaymentDate" runat="server" DateFormat="dd/MM/yyyy"
                                    DisplayDateFormat="dd/MM/yyyy">
                                </DateInput>
                            </telerik:RadDatePicker>
                        </td>                                   
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblAmount" Text="Amount"></asp:Label>
                        </td>
                        <td align="left">
                            <telerik:RadNumericTextBox ID="txtAmount" runat="server" Width="115px" Type="Number"
                                NumberFormat-DecimalDigits="2" NumberFormat-PositivePattern="n" 
                                NumberFormat-GroupSeparator=".">                                
                            </telerik:RadNumericTextBox>
                        </td>                                                                             
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblRemark" Text="Remark"></asp:Label>
                        </td>
                        <td align="left">
                            <telerik:RadTextBox ID="txtRemark" runat="server" Rows="4" TextMode="multiLine"
                                                                    Width="400px" Skin="Office2007" />
                        </td> 
                    </tr>                    
                </table>
                <div style="text-align:center">
                    <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="flatButton" Width="60" OnClick="OnBtnSaveClicked"/>&nbsp;
                    <asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="flatButton" Width="60" OnClientClick="OnBtnCancelClientClicked()"/>
                </div>
            </center>
        </div>
    </form>
</body>
</html>
