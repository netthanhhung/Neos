<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InvoiceCoordinatePopup.aspx.cs" Inherits="InvoiceCoordinatePopup" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Invoice Coordinate Popup</title>
    <link href="Styles/Neos.css" rel="stylesheet" type="text/css" />
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
        
        function OnButtonChooseCompanyClientClicked()
        {    
            var radWindow = $find('radWinActionCompanyChoose');
            var url = "ChooseCompanyPopup.aspx?";
            radWindow.setUrl(url);
            radWindow.show();
            
            return false;
        }        
             
        function onClientChooseCompanyWindowClosed(window)
        {                        
            if (window.argument != undefined && window.argument != null && window.argument != "")
            {                
                var argument = window.argument;
                var argument_array = argument.split("/");                
                var hiddenCompanyId = document.getElementById('hiddenCompanyId');
                hiddenCompanyId.value = argument_array[0];                                                    
                  
                var ddlFactoringCode = $find('ddlFactoringCode');
                ddlFactoringCode.clearItems();                
                ddlFactoringCode.trackChanges();
                
                var companyItem = new Telerik.Web.UI.RadComboBoxItem();
                companyItem.set_text(argument_array[1]);
                companyItem.set_value(argument_array[0]);
                ddlFactoringCode.get_items().add(companyItem);
                companyItem.select();
                ddlFactoringCode.commitChanges();                                
            }
        }
    </script>
    </telerik:RadScriptBlock>
</head>
<body>
    <form id="invoiceCoordinatePopup" runat="server">    
    <telerik:RadScriptManager ID="ScriptManager" runat="server" /> 
    <asp:ValidationSummary runat="server" ID="sum" ShowMessageBox="true" ShowSummary="false" ValidationGroup="InvoiceCoordinate" />
    
    <asp:RegularExpressionValidator runat="server" ID="revInvCoordinate" ControlToValidate="txtEmail" 
        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
        ValidationGroup="InvoiceCoordinate" Display="None"></asp:RegularExpressionValidator>
    <div>              
        <table width="500px">
            <tr>
                <td style="width:100px">
                    <asp:Label runat="server" ID="lblNbrCustomer" Text="Nbr. customer"></asp:Label>
                </td>
                <td style="width:160px" align="left">
                    <telerik:RadTextBox ID="txtNbrCustomer" runat="server" Enabled="false" Width="100%" Skin="Office2007" />
                </td>    
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblName" Text="Name"></asp:Label>
                </td>
                <td colspan="3">
                    <telerik:RadTextBox ID="txtName" runat="server" Width="100%" Skin="Office2007" MaxLength="150" />
                </td>                                                                              
            </tr>
            <tr>
                 <td>
                    <asp:Label runat="server" ID="lblCo" Text="c/o"></asp:Label>
                </td>
                <td colspan="3">
                    <telerik:RadTextBox ID="txtCO" runat="server" Width="100%" Skin="Office2007" MaxLength="100" />
                </td>  
            </tr>    
            <tr>
                 <td>
                    <asp:Label runat="server" ID="lblAddress" Text="Address"></asp:Label>
                </td>
                <td colspan="3">
                    <telerik:RadTextBox ID="txtAddress" runat="server" Width="100%" Skin="Office2007" MaxLength="200"/>
                </td>  
            </tr>  
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblZipCode" Text="Zip code"></asp:Label>
                </td>
                <td colspan="3">
                    <telerik:RadTextBox ID="txtZipCode" runat="server" Width="100%" Skin="Office2007" MaxLength="10" />
                </td>  
            </tr>  
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblCity" Text="City"></asp:Label>
                </td>
                <td colspan="3">
                    <telerik:RadTextBox ID="txtCity" runat="server" Width="100%" Skin="Office2007" MaxLength="50" />
                </td>  
            </tr>   
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblVatNumber" Text="VAT number"></asp:Label>
                </td>
                <td colspan="3">
                    <telerik:RadTextBox ID="txtVatNumber" runat="server" Width="100%" Skin="Office2007" MaxLength="15" />
                </td>  
            </tr>         
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblTelephone" Text="Telephone"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtTelephone" runat="server" Width="100%" Skin="Office2007" MaxLength="15" />
                </td>  
                <td style="width:100px" align="right">
                    <asp:Label runat="server" ID="lblFax" Text="Fax"></asp:Label>
                </td>
                <td style="width:120px" align="left">
                    <telerik:RadTextBox ID="txtFax" runat="server" Width="100%" Skin="Office2007" MaxLength="15" />
                </td> 
            </tr>    
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblEmail" Text="Email"></asp:Label>
                </td>
                <td colspan="3">
                    <telerik:RadTextBox ID="txtEmail" runat="server" Width="100%" Skin="Office2007" MaxLength="50"/>
                </td>                 
            </tr>       
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblDefault" Text="Default"></asp:Label>
                </td>
                <td>
                    <asp:CheckBox ID="chkDefault" runat="server" Checked="false"/>
                </td>  
                 <td></td>
                  <td></td>
            </tr>   
            <tr>
                <td>
                    <asp:Label ID="lblFactoringCode" runat="server" Text="Factoring Code"></asp:Label>
                </td>
                <td colspan="3">                    
                    <%--<telerik:RadComboBox runat="server" ID="ddlFactoringCode" EnableAjaxSkinRendering="true" Skin="Office2007" 
                        Width="100%" Height="300" AllowCustomText="true" EnableLoadOnDemand="True" 
                         DataValueField="CompanyID" DataTextField="CompanyName" 
                        OnItemsRequested="OnDropdownCompany_ItemsRequested" 
                        OnClientItemsRequesting="onDropdownCompany_ClientRequesting"></telerik:RadComboBox>--%>
                    <telerik:RadTextBox ID="txtFactoringCode" runat="server" Width="100%" Skin="Office2007" MaxLength="1000"></telerik:RadTextBox>
                </td>
                <%--<td style="text-align:left;">                    
                    <asp:Button ID="btnFactoringCode" runat="server" Width="30px" Text="..." OnClientClick="return OnButtonChooseCompanyClientClicked();"/>
                </td>--%>                
            </tr>    
        </table>
        <br />
        <%--<asp:HiddenField ID="hiddenCompanyId" runat="server" Value="-1" />--%>
        <telerik:RadWindow runat="server" ID="radWinActionCompanyChoose" Skin="Office2007" VisibleOnPageLoad="false" VisibleStatusbar="false"
            Modal="true" OffsetElementID="offsetElement" Top="30" Left="30" NavigateUrl="ChooseCompanyPopup.aspx"
            Title="Choose company" Height="400px" Width="700px" OnClientClose="onClientChooseCompanyWindowClosed">
        </telerik:RadWindow>
        <div style="text-align:center">
            <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="flatButton" Width="60" 
                CausesValidation="true" ValidationGroup="InvoiceCoordinate" OnClick="OnBtnSaveClicked"/>&nbsp;
            <asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="flatButton" Width="60" OnClientClick="OnBtnCancelClientClicked()"/>
        </div>
    </div>
    </form>
</body>
</html>
