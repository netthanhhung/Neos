<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InvoiceDetailPopup.aspx.cs"
    Inherits="InvoiceDetailPopup" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Invoice detail</title>
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
        
        function OnQuantityOrUnitPriceLostFocus(sender, args)
        {            
            var txtAmount = $find('<%=txtAmount.ClientID %>');
            var txtUnitPrice = $find('<%=txtUnitPrice.ClientID %>');
            var txtQuantity = $find('<%=txtQuantity.ClientID %>');
            txtAmount.SetValue(txtUnitPrice.GetValue() * txtQuantity.GetValue());
        }
    </script>
</telerik:RadScriptBlock>
</head>
<body>
    <form id="invoiceDetailForm" runat="server">
        <telerik:RadScriptManager ID="ScriptManager" runat="server" />
        <div>
            <telerik:RadAjaxManager EnableAJAX="true" runat="server" ID="invoiceDetailAjaxManager" OnAjaxRequest="OnInvoiceDetailAjaxManagerAjaxRequest">
               <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="ddlVAT">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="txtVATCode" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>                    
                </AjaxSettings>
            </telerik:RadAjaxManager>
            <center>                
                <table width="610px">
                    <tr>
                        <td style="width:110px">
                            <asp:Label runat="server" ID="lblDescription" Text="Description"></asp:Label>
                        </td>
                        <td colspan="2" style="width:500px">
                            <telerik:RadTextBox ID="txtDescription" runat="server" MaxLength="255" Width="500px" TabIndex="0" />
                        </td>                                   
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblQuantity" Text="Quantity"></asp:Label>
                        </td>
                        <td style="width:150px">
                            <telerik:RadNumericTextBox ID="txtQuantity" runat="server" Width="150px" Type="Number" TabIndex="1"
                                NumberFormat-DecimalDigits="2" NumberFormat-PositivePattern="n" 
                                NumberFormat-GroupSeparator=".">
                                <ClientEvents OnBlur="OnQuantityOrUnitPriceLostFocus" />
                            </telerik:RadNumericTextBox>
                        </td>   
                        <td align="right" style="width:250px" rowspan="3">
                            <fieldset style="width:250px">
                                <legend>
                                    <asp:Label runat="server" ID="lblVATHeader" Text="VAT"></asp:Label></legend>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="lblVAT" Text="V.A.T"></asp:Label>
                                        </td>
                                         <td>
                                            <asp:Label runat="server" ID="lblVATCode" Text="V.A.T Code"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:150px">
                                            <telerik:RadComboBox ID="ddlVAT" runat="server" Width="84px" AutoPostBack="true" TabIndex="4"
                                                Skin="Office2007" OnSelectedIndexChanged="OnVATSelectedIndexChanged"></telerik:RadComboBox>
                                        </td>
                                        <td style="width:80px">
                                             <telerik:RadTextBox ID="txtVATCode" runat="server" Width="80px"  ReadOnly="true" TabIndex="5"/>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>                                                        
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblUnitPrice" Text="Unit Price (€)"></asp:Label>
                        </td>
                        <td style="width:150px">
                            <telerik:RadNumericTextBox ID="txtUnitPrice" runat="server" Width="150px" Type="Number" TabIndex="2"
                                NumberFormat-DecimalDigits="2" NumberFormat-PositivePattern="n" 
                                NumberFormat-GroupSeparator=".">
                                <ClientEvents OnBlur="OnQuantityOrUnitPriceLostFocus" />                                
                            </telerik:RadNumericTextBox>
                        </td> 
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblAmount" Text="Amount (€)"></asp:Label>
                        </td>
                        <td style="width:150px">
                            <telerik:RadNumericTextBox ID="txtAmount" runat="server" Width="150px" Type="Number" TabIndex="3"
                                NumberFormat-DecimalDigits="2" NumberFormat-PositivePattern="n" ReadOnly="true"
                                NumberFormat-GroupSeparator="." /> 
                        </td> 
                        <td></td>
                    </tr>
                </table>
                <div style="text-align:center">
                    <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="flatButton" Width="60" OnClick="OnBtnSaveClicked" TabIndex="6"/>&nbsp;
                    <asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="flatButton" Width="60" OnClientClick="OnBtnCancelClientClicked()" TabIndex="7"/>
                </div>                
            </center>
        </div>
    </form>
</body>
</html>
