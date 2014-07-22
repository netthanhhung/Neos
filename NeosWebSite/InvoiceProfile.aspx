<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InvoiceProfile.aspx.cs" aspCompat="True" Inherits="InvoiceProfile" ValidateRequest="false" EnableEventValidation="false" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Invoice profile Page</title>
    <link href="Styles/Neos.css" rel="Stylesheet" />

    <script src="script/utils.js" type="text/javascript"></script>
    <telerik:RadScriptBlock runat="server" ID="scriptBlock">
    <script type="text/javascript">
    function OnButtonChooseCompanyClientClicked()
    {    
        var radWindow = $find('radWinCompanyChoose');
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
            var txtIdCustomer = $find('txtIdCustomer');
            txtIdCustomer.set_value(argument_array[0]);  
            
            var ddlCustomer = $find('<%=ddlCustomer.ClientID %>');
            ddlCustomer.clearItems();                
            ddlCustomer.trackChanges();
            
            var customerItem = new Telerik.Web.UI.RadComboBoxItem();
            customerItem.set_text(argument_array[1]);
            customerItem.set_value(argument_array[0]);
            ddlCustomer.get_items().add(customerItem);
            customerItem.select();
            ddlCustomer.commitChanges();
                     
//            var txtCustomerName = $find('txtCustomerName');
//            txtCustomerName.set_value(argument_array[1]); 
            $find("invoiceProfileAjaxManager").ajaxRequest("DataBindCompanyAddress-" + argument_array[0]);                                                                    
        }
    }
    
     function OnButtonChooseCompanyAddressClientClicked()
    {    
        var txtIdCustomer = $find('txtIdCustomer');
        var companyId = txtIdCustomer.get_value();
        var radWindow = $find('radWinCompanyAddressChoose');
        var url = "ChooseCompanyAddressPopup.aspx?CompanyId=" + companyId;
        radWindow.setUrl(url);
        radWindow.show();
        
        return false;
    }
    
    function onClientChooseCompanyAddressWindowClosed(window)
    {                            
        if (window.argument != undefined && window.argument != null && window.argument != "")
        {
            var argument = window.argument;
            var argument_array = argument.split("/");                
            var hiddenCompanyAddressId = document.getElementById('hiddenCompanyAddressId');
            hiddenCompanyAddressId.value = argument_array[0];
            
            var txtAddressName = $find('txtAddressName');
            txtAddressName.set_value(argument_array[1]);
            
            var txtAddress = $find('txtAddress');
            txtAddress.set_value(argument_array[2]);    
             
            var txtCity = $find('txtCity');
            txtCity.set_value(argument_array[3]);   
            
            var txtVatNumber = $find('txtVatNumber');
            txtVatNumber.set_value(argument_array[4]);                                                                   
        }
    }
    
    function onClientInvoiceDetailWindowClosed(window) 
    {
        if (window.argument != undefined && window.argument != null && window.argument != "")
        {
            var isReload = window.argument;
            if(isReload == "Yes") 
            {                                  
                $find("invoiceProfileAjaxManager").ajaxRequest("RebindInvoiceDetailData");
            }
        }
    }
    
    function onClientInvoicePaymentWindowClosed(window) 
    {
        if (window.argument != undefined && window.argument != null && window.argument != "")
        {
            var isReload = window.argument;
            if(isReload == "Yes") 
            {                                  
                $find("invoiceProfileAjaxManager").ajaxRequest("RebindInvoicePaymentData");
            }
        }
    }
    
    function onBtnSaveClientClicked(message) 
    {        
        var hiddenCompanyAddressId = document.getElementById('hiddenCompanyAddressId');
        if(hiddenCompanyAddressId.value == null || hiddenCompanyAddressId.value == '') 
        {
            alert(message);
            return false;
        }
        return true;
    }
    
    function onLoadInvoiceProfilePage()
    {
        var invoiceIdPK = getQueryString("InvoiceIdPK");
        var mode = getQueryString("mode");
        if(invoiceIdPK != null && invoiceIdPK != "") //in edit mode
        {                    
            if(mode == "edit")
                processInvoiceToolBar("EditInvoiceProfile");
            else if(mode == "view")
                processInvoiceToolBar("ViewInvoiceProfile");
        } 
        else if(mode == "edit") 
        {
            processInvoiceToolBar("AddInvoiceProfile");
        }
    }
    function onform_unload()
    {
        processInvoiceToolBar("UnLoadInvoiceProfilePage");
    }            
    
    function OnSendInvoiceByEmail(url)
    {
        var radWindow = $find('<%=radWindowSendInvoiceByEmail.ClientID %>');
        radWindow.setUrl(url);
        radWindow.show();
        
        return false;
    }
    function onDropdownCustomer_ClientRequesting(sender, eventArgs)
    {
        var combo = $find("<%= ddlCustomer.ClientID %>");
        var text = combo.get_text();
        if(text.length > 0)
        {
            eventArgs.set_cancel(false);
        }
        else
        {
            eventArgs.set_cancel(true);
        }
    }
    function onDropdownCustomer_ClientChanged(sender, eventArgs)
    {
        var item = eventArgs.get_item();
//        var txtCustomerName = $find('txtCustomerName');
//        txtCustomerName.set_value(item.get_text());
        $find("invoiceProfileAjaxManager").ajaxRequest("DataBindCompanyAddress-" + item.get_value());
    }
    </script>
    </telerik:RadScriptBlock>
</head>
<body onbeforeunload="onform_unload()">    
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="invoiceProfileScriptManager" runat="server" />        
        <div class="rightpane_title" style="width:100%">
            <table style="width:98%">
                <tr>       
                    <td>
                        <asp:Literal runat="server" ID="lblInvoiceTitle" Text="Invoice"></asp:Literal>
                    </td>                                                
                    <td align="right">
                        <asp:LinkButton ID="lnkBack" runat="server"  Text="Back"
                                    OnClick="OnLinkBackClicked"></asp:LinkButton>     
                    </td> 
                </tr>
            </table>  
        </div>
        <div style="margin-top:30px;">
        <telerik:RadAjaxManager EnableAJAX="true" runat="server" ID="invoiceProfileAjaxManager" OnAjaxRequest="OnInvoiceProfileAjaxManagerAjaxRequest">
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="gridInvoiceDetails">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridInvoiceDetails" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>    
                    <telerik:AjaxSetting AjaxControlID="gridInvoicePayments">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridInvoicePayments" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>                
                </AjaxSettings>
            </telerik:RadAjaxManager>
        <asp:MultiView runat="server" ID="InvoicingMView" ActiveViewIndex="0">
            <asp:View runat="server" ID="InvoiceProfileView">
            <div>            
                <div>
                    <table width="99%">
                        <tr>
                            <td>
                                <asp:RadioButton ID="radInvoice" runat="server" Checked="true" GroupName="TypeGroup" />
                                <asp:Label ID="lblTypeInvoice" runat="server" Text="Invoice"></asp:Label>
                                <asp:RadioButton ID="radCreditNote" runat="server" GroupName="TypeGroup" />
                                <asp:Label ID="lblTypeCreditNote" runat="server" Text="Credite note"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="divHeader" runat="server">
                                 <fieldset style="width:750px">
                                    <legend>
                                        <asp:Label runat="server" ID="lblInvoiceHeader" Text="Header"></asp:Label></legend>
                                    <table width="700px">
                                        <tr>
                                            <td style="width:90px">
                                                <asp:Label ID="lblCustomer" runat="server" Text="Customer"></asp:Label>
                                            </td>
                                            <td style="width:200px">
                                                <telerik:RadComboBox runat="server" ID="ddlCustomer" Skin="Office2007" Width="203" Height="300"
                                                EnableAjaxSkinRendering="true"  AllowCustomText="true" EnableLoadOnDemand="True" DataValueField="CompanyID" DataTextField="CompanyName"
                                                OnItemsRequested="OnDropdownCustomer_ItemsRequested" OnClientItemsRequesting="onDropdownCustomer_ClientRequesting"
                                                OnClientSelectedIndexChanged="onDropdownCustomer_ClientChanged"
                                                >
                                                </telerik:RadComboBox>
                                                <telerik:RadTextBox ID="txtCustomerName" runat="server" Width="200px" ReadOnly="true" Skin="Office2007"
                                                BackColor="White" BorderWidth="1" BorderColor="#A8BEDA" Visible="false"></telerik:RadTextBox>
                                            </td>
                                            <td style="width:70px" align="left">
                                                <asp:Button ID="btnChooseCustomer" runat="server" Width="120px" CssClass="flatButton"
                                                    Text="Choose customer" OnClientClick="return OnButtonChooseCompanyClientClicked();"/>
                                            </td>
                                             <td style="width:95px">
                                                <asp:Label ID="lblDate" runat="server" Text="Date"></asp:Label>
                                            </td>
                                            <td>                                            
                                                <telerik:RadDatePicker ID="datInvoiceDate" runat="server" Width="104px" Skin="Office2007" Calendar-CultureInfo="en-US">
                                                    <DateInput ID="datInputInvoiceDate" runat="server" DateFormat="dd/MM/yyyy"
                                                        DisplayDateFormat="dd/MM/yyyy">
                                                    </DateInput>
                                                </telerik:RadDatePicker>
                                            </td>                                          
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblIdCustomer" runat="server" Text="Id customer"></asp:Label>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtIdCustomer" runat="server" Width="200px" ReadOnly="true"
                                                BackColor="White" BorderWidth="1" BorderColor="#A8BEDA" Skin="Office2007"></telerik:RadTextBox>
                                            </td>
                                            <td></td>
                                            <td style="width:100px">
                                                <asp:Label ID="lblInvoiceNumber" runat="server" Text="Invoice number"></asp:Label>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtInvoiceNumber" runat="server" Width="100px" ReadOnly="true"
                                                BackColor="White" BorderWidth="1" BorderColor="#A8BEDA" Skin="Office2007"></telerik:RadTextBox>
                                            </td>  
                                            <td></td> 
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblAddressName" runat="server" Text="Name"></asp:Label>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtAddressName" runat="server" Width="200px" Skin="Office2007"
                                                  ReadOnly="true" BackColor="White" BorderWidth="1" BorderColor="#A8BEDA"></telerik:RadTextBox>
                                            </td>
                                            <td style="width:170px" align="left">
                                                <asp:Button ID="btnChooseAddress" runat="server" Width="120px" 
                                                    Text="Choose address" CssClass="flatButton"
                                                    OnClientClick="return OnButtonChooseCompanyAddressClientClicked();"/>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblFactoring" runat="server" Text="Factoring"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkFactoring" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblVatNumber" runat="server" Text="VAT number"></asp:Label>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtVatNumber" runat="server" Width="200px" ReadOnly="true"
                                                 BackColor="White" BorderWidth="1" BorderColor="#A8BEDA" Skin="Office2007"></telerik:RadTextBox>
                                            </td>
                                            <td colspan="3"></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblAddress" runat="server" Text="Address"></asp:Label>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtAddress" runat="server" Width="200px" 
                                                ReadOnly="true" BackColor="White" BorderWidth="1" BorderColor="#A8BEDA" Skin="Office2007"></telerik:RadTextBox>
                                            </td>                                        
                                            <td colspan="3"></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblZipCode" runat="server" Text="Zip code"></asp:Label>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtZipCode" runat="server" Width="200px" ReadOnly="true"
                                                 BackColor="White" BorderWidth="1" BorderColor="#A8BEDA" Skin="Office2007"></telerik:RadTextBox>
                                            </td>    
                                            <td colspan="3"></td>                                    
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblCity" runat="server" Text="City"></asp:Label>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtCity" runat="server" Width="200px" ReadOnly="true" 
                                                BackColor="White" BorderWidth="1" BorderColor="#A8BEDA" Skin="Office2007"></telerik:RadTextBox>
                                            </td>  
                                            <td colspan="3"></td>                                      
                                        </tr>                                    
                                    </table>                                
                                </fieldset>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                 <fieldset style="width:99%">
                                    <legend>
                                        <asp:Label runat="server" ID="lblInvoice" Text="Invoice"></asp:Label></legend>
                                    <telerik:RadTabStrip ID="radTabStripInvoice" runat="server" Skin="Web20" MultiPageID="radDetailMultiPage" BackColor="#A0B8DB"
                                        SelectedIndex="0" CssClass="tabStrip">
                                        <Tabs>
                                            <telerik:RadTab Text="Details">
                                            </telerik:RadTab>
                                            <telerik:RadTab Text="Payments">
                                            </telerik:RadTab>   
                                            <telerik:RadTab Text="Internal remark">
                                            </telerik:RadTab>                                      
                                        </Tabs>
                                    </telerik:RadTabStrip>
                                    <telerik:RadMultiPage ID="radDetailMultiPage" runat="server" SelectedIndex="0" CssClass="multiPage">
                                        <telerik:RadPageView ID="invoiceDetailPageView" runat="server">
                                            <table width="99%">
                                                <tr>
                                                    <td>
                                                        <telerik:RadGrid ID="gridInvoiceDetails" GridLines="None" Skin="Office2007" AllowMultiRowSelection="True"
                                                        EnableAjaxSkinRendering="true" runat="server" AllowPaging="False" AllowSorting="True"
                                                        PageSize="10" Width="100%" AutoGenerateColumns="false"
                                                        OnItemDataBound="OnGridInvoiceDetailItemDataBound" OnRowDrop="OnGridInvoiceDetails_RowDrop"
                                                        OnNeedDataSource="OnGridInvoiceDetailNeedDataSource">                                                        
                                                        <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <MasterTableView DataKeyNames="InvoiceDetailsId" DataMember="InvoiceDetails" AllowMultiColumnSorting="True"
                                                            Width="100%" EditMode="PopUp">                        
                                                            <SortExpressions>
                                                                <telerik:GridSortExpression FieldName="IdLigneNumber" SortOrder="Ascending" />
                                                            </SortExpressions>
                                                            <Columns>
                                                                <telerik:GridBoundColumn UniqueName="Description" SortExpression="Description" HeaderText="Description"
                                                                    DataField="Description" AllowSorting="false">
                                                                    <HeaderStyle Width="20%"></HeaderStyle>
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn UniqueName="Quantity" SortExpression="Quantity" HeaderText="Quantity"
                                                                    DataField="Quantity" AllowSorting="false">
                                                                    <HeaderStyle Width="8%"></HeaderStyle>
                                                                </telerik:GridBoundColumn> 
                                                                <telerik:GridBoundColumn UniqueName="UnitPriceEuro" SortExpression="UnitPriceEuro" HeaderText="Unit price (€)"
                                                                    DataField="UnitPriceEuro" AllowSorting="false">
                                                                    <HeaderStyle Width="10%"></HeaderStyle>
                                                                </telerik:GridBoundColumn> 
                                                                <telerik:GridBoundColumn UniqueName="AmountEuro" SortExpression="AmountEuro" HeaderText="Amount (€)"
                                                                    DataField="AmountEuro" AllowSorting="false">
                                                                    <HeaderStyle Width="16%"></HeaderStyle>
                                                                </telerik:GridBoundColumn> 
                                                                <telerik:GridBoundColumn UniqueName="VatCode" SortExpression="VatCode" HeaderText="Code VAT"
                                                                    DataField="VatCode" AllowSorting="false">
                                                                    <HeaderStyle Width="8%"></HeaderStyle>
                                                                </telerik:GridBoundColumn> 
                                                                <telerik:GridBoundColumn UniqueName="VatRate" SortExpression="VatRate" HeaderText="% VAT"
                                                                    DataField="VatRate" AllowSorting="false">
                                                                    <HeaderStyle Width="8%"></HeaderStyle>
                                                                </telerik:GridBoundColumn> 
                                                                <telerik:GridBoundColumn UniqueName="AmountVAT" SortExpression="AmountVAT" HeaderText="Amount VAT (€)"
                                                                    DataField="AmountVAT" AllowSorting="false">
                                                                    <HeaderStyle Width="10%"></HeaderStyle>
                                                                </telerik:GridBoundColumn>    
                                                                 <telerik:GridBoundColumn UniqueName="TotalAmountVAT" SortExpression="TotalAmountVAT" 
                                                                    HeaderText="Total Amount (€)" DataField="TotalAmountVAT" AllowSorting="false">
                                                                    <HeaderStyle Width="10%"></HeaderStyle>
                                                                </telerik:GridBoundColumn>  
                                                                <telerik:GridTemplateColumn UniqueName="TemplateEditInvoiceDetailColumn">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkInvoiceDetailEdit" runat="server" Text="Edit" CommandArgument='<%# Eval("InvoiceDetailsId") %>'
                                                                        OnClientClick='<%# Eval("InvoiceDetailsId", "return OnInvoiceDetailEditClientClicked({0});") %>'>  
                                                                    </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle Width="5%"></HeaderStyle>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn UniqueName="TemplateDeleteInvoiceDetailColumn">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkInvoiceDetailDelete" runat="server" Text="Delete" CommandArgument='<%# Eval("InvoiceDetailsId") %>'
                                                                            OnClientClick="return confirm('Are you sure to delete this item?')" OnClick="OnInvoiceDetailDeleteClicked">
                                                                        </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle Width="5%" HorizontalAlign="Center"></HeaderStyle>
                                                                </telerik:GridTemplateColumn> 
                                                            </Columns>
                                                        </MasterTableView>
                                                        <ClientSettings  AllowRowsDragDrop="true">
                                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" />        
                                                            <ClientEvents />
                                                        </ClientSettings>
                                                    </telerik:RadGrid>
                                                    </td>
                                                </tr>
                                                
                                                <tr>
                                                    <td align="center">
                                                        <asp:LinkButton runat="server" ID="lnkAddInvoiceDetail" Text="Add new invoice detail" 
                                                            OnClientClick="return OnAddNewInvoiceDetailClientClicked()"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div id="divTotal" runat="server">
                                                        <table>
                                                            <tr>                                                    
                                                                <td colspan="2" rowspan="3" style="width:600px">
                                                                    <telerik:RadTextBox ID="txtRemark" runat="server" Rows="4" TextMode="multiLine"
                                                                        Width="600px" BackColor="White" BorderWidth="1" BorderColor="#A8BEDA" Skin="Office2007"
                                                                        />
                                                                </td>     
                                                                <td align="right">
                                                                    <asp:Label ID="lblTotalHTVA" runat="server" Text="Total H.T.V.A (€)"></asp:Label>
                                                                </td>                                                                 
                                                                <td align="right">
                                                                    <telerik:RadNumericTextBox ID="txtTotalHTVA" runat="server" Width="150px" Type="Number"
                                                                        NumberFormat-DecimalDigits="2" NumberFormat-PositivePattern="n" 
                                                                        NumberFormat-GroupSeparator="." ReadOnly="true">
                                                                    </telerik:RadNumericTextBox>
                                                                </td>                                            
                                                            </tr>
                                                            <tr>
                                                                <td align="right">
                                                                    <asp:Label ID="lblTotalVAT" runat="server" Text="Total V.A.T (€)"></asp:Label>
                                                                </td>                                                                  
                                                                <td align="right">
                                                                    <%--<asp:TextBox runat="server" ID="txtTotalVAT" Width="150px"></asp:TextBox>--%>
                                                                    <telerik:RadNumericTextBox ID="txtTotalVAT" runat="server" Width="150px" Type="Number"
                                                                        NumberFormat-DecimalDigits="2" NumberFormat-PositivePattern="n"
                                                                        NumberFormat-GroupSeparator="." ReadOnly="true">                                
                                                                    </telerik:RadNumericTextBox>
                                                                </td> 
                                                            </tr>
                                                            <tr>
                                                                <td align="right">
                                                                    <asp:Label ID="lblTotal" runat="server" Text="Total (€)"></asp:Label>
                                                                </td>                                                                  
                                                                <td align="right">
                                                                    <telerik:RadNumericTextBox ID="txtTotal" runat="server" Width="150px" Type="Number"
                                                                        NumberFormat-DecimalDigits="2" NumberFormat-PositivePattern="n" 
                                                                        NumberFormat-GroupSeparator="." ReadOnly="true">                                
                                                                    </telerik:RadNumericTextBox>
                                                                </td> 
                                                            </tr>
                                                            <tr>
                                                                <td align="right" style="width:500px; text-align:right">
                                                                    <asp:Label ID="lblPaymentDate" runat="server" Text="Payment date"></asp:Label>
                                                                </td>
                                                                <td align="right" style="width:100px; text-align:right">
                                                                    <asp:TextBox ID="txtPaymentDate" runat="server" Width="89px" 
                                                                    ReadOnly="true" BackColor="White" BorderWidth="1" BorderColor="#A8BEDA"></asp:TextBox>
                                                                </td>                                                
                                                                <td align="right">
                                                                    <asp:Label ID="lblPayment" runat="server" Text="Payment"></asp:Label>
                                                                    <asp:CheckBox ID="chkPayment" runat="server" Enabled="false"/>
                                                                </td>                                                                
                                                                 <td align="right">
                                                                    <telerik:RadNumericTextBox ID="txtPayment" runat="server" Width="150px" Type="Number"
                                                                        NumberFormat-DecimalDigits="2" NumberFormat-PositivePattern="n" 
                                                                        NumberFormat-GroupSeparator="." ReadOnly="true">                                
                                                                    </telerik:RadNumericTextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </telerik:RadPageView>
                                        <telerik:RadPageView ID="invoicePaymentPageView" runat="server">
                                            <table width="600px">
                                                <tr>
                                                    <td>
                                                        <telerik:RadGrid ID="gridInvoicePayments" GridLines="None" Skin="Office2007" AllowMultiRowSelection="False"
                                                        EnableAjaxSkinRendering="true" runat="server" AllowPaging="False" AllowSorting="True"
                                                        PageSize="10" Width="600px" AutoGenerateColumns="false"
                                                        OnItemDataBound="OnGridInvoicePaymentItemDataBound" 
                                                        OnNeedDataSource="OnGridInvoicePaymentNeedDataSource">
                                                        <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <MasterTableView DataKeyNames="IdPayment" DataMember="InvoicePayments" AllowMultiColumnSorting="True"
                                                            Width="100%" EditMode="PopUp">                        
                                                            <Columns>                                        
                                                                <telerik:GridBoundColumn UniqueName="DatePayment" SortExpression="DatePayment" HeaderText="Date"
                                                                    DataField="DatePayment" DataType="system.DateTime" AllowSorting="false"
                                                                    DataFormatString="{0:dd/MM/yyyy}">
                                                                    <HeaderStyle Width="20%"></HeaderStyle>
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn UniqueName="Amount" SortExpression="Amount" HeaderText="Amount (€)"
                                                                    DataField="Amount" AllowSorting="false">
                                                                    <HeaderStyle Width="20%"></HeaderStyle>
                                                                </telerik:GridBoundColumn> 
                                                                <telerik:GridBoundColumn UniqueName="Remark" SortExpression="Remark" HeaderText="Remark"
                                                                    DataField="Remark" AllowSorting="false">
                                                                    <HeaderStyle Width="40%"></HeaderStyle>
                                                                </telerik:GridBoundColumn> 
                                                                 
                                                                <telerik:GridTemplateColumn UniqueName="TemplateEditInvoicePaymentColumn">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkInvoicePaymentEdit" runat="server" Text="Edit" CommandArgument='<%# Eval("IdPayment") %>'
                                                                        OnClientClick='<%# Eval("IdPayment", "return OnInvoicePaymentEditClientClicked({0});") %>'>  
                                                                    </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle Width="10%"></HeaderStyle>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn UniqueName="TemplateDeleteInvoicePaymentColumn">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkInvoicePaymentDelete" runat="server" Text="Delete" CommandArgument='<%# Eval("IdPayment") %>'
                                                                            OnClientClick="return confirm('Are you sure to delete this item?')" OnClick="OnInvoicePaymentDeleteClicked">
                                                                        </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle Width="10%" HorizontalAlign="Center"></HeaderStyle>
                                                                </telerik:GridTemplateColumn>                                          
                                                            </Columns>
                                                        </MasterTableView>
                                                        <ClientSettings>
                                                            <Selecting AllowRowSelect="true" />                                    
                                                        </ClientSettings>
                                                    </telerik:RadGrid>
                                                    </td>
                                                </tr>
                                                
                                                <tr>
                                                    <td align="center">
                                                        <asp:LinkButton runat="server" ID="lnkAddNewPayment" Text="Add new payment" 
                                                            OnClientClick="return OnAddNewInvoicePaymentClientClicked()"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </telerik:RadPageView>
                                        <telerik:RadPageView ID="radInternalRemardView" runat="server">
                                            <table width="600px">
                                                <tr>
                                                    <td>                                                        
                                                            <telerik:RadTextBox ID="txtInternalRemark" runat="server" Rows="4" TextMode="multiLine"
                                                                Width="600px" BackColor="White" BorderWidth="1" BorderColor="#A8BEDA" Skin="Office2007" >
                                                             </telerik:RadTextBox>
                                                                                                                               
                                                    </td>
                                                </tr>
                                            </table>
                                        </telerik:RadPageView>
                                    </telerik:RadMultiPage>                                
                                 </fieldset>    
                            </td>
                         </tr>       
                                    
                    </table>
                    <telerik:RadWindow runat="server" ID="radWinCompanyChoose" Skin="Office2007" VisibleOnPageLoad="false" VisibleStatusbar="false"
                        Modal="true" OffsetElementID="offsetElement" Top="30" Left="30" NavigateUrl="ChooseCompanyPopup.aspx" 
                        Title="Choose company" Height="400px" Width="750px" OnClientClose="onClientChooseCompanyWindowClosed">
                    </telerik:RadWindow>
                    <asp:HiddenField ID="hiddenCompanyId" runat="server" Value="-1" />
                    <telerik:RadWindow runat="server" ID="radWinCompanyAddressChoose" Skin="Office2007" VisibleOnPageLoad="false" VisibleStatusbar="false"
                        Modal="true" OffsetElementID="offsetElement" Top="30" Left="30" NavigateUrl="ChooseCompanyPopup.aspx"
                        Title="Choose company" Height="400px" Width="750px" OnClientClose="onClientChooseCompanyAddressWindowClosed">
                    </telerik:RadWindow>
                      <asp:HiddenField ID="hiddenCompanyAddressId" runat="server" />
                    <telerik:RadWindow runat="server" ID="radWinInvoiceDetail" Skin="Office2007" VisibleOnPageLoad="false" VisibleStatusbar="false"
                        Modal="true" OffsetElementID="offsetElement" Top="30" Left="30" NavigateUrl="InvoiceDetailPopup.aspx"
                        Title="Invoice detail" Height="300px" Width="650px" OnClientClose="onClientInvoiceDetailWindowClosed">
                    </telerik:RadWindow> 
                    <telerik:RadWindow runat="server" ID="radWinInvoicePayment" Skin="Office2007" VisibleOnPageLoad="false" VisibleStatusbar="false"
                        Modal="true" OffsetElementID="offsetElement" Top="30" Left="30" NavigateUrl="InvoicePaymentPopup.aspx"
                        Title="Invoice payment" Height="300px" Width="550px" OnClientClose="onClientInvoicePaymentWindowClosed">
                    </telerik:RadWindow> 
                    <telerik:RadWindow runat="server" ID="radWindowSendInvoiceByEmail" Skin="Office2007"
                    VisibleOnPageLoad="false" VisibleStatusbar="false" Modal="true" OffsetElementID="offsetElement"
                    Top="30" Left="30" NavigateUrl="SendEmail.aspx" Title="Send email" Width="750"
                    Height="500">
                    </telerik:RadWindow>
                </div>
                <div style="text-align: center">
                    <table width="100%">
                        <tr>                            
                            <td align="center">
                                <asp:Button runat="server" ID="btnExport" Text="Print" CssClass="flatButton" Width="60"
                                    OnClick="OnButtonInvoiceExportClicked" Visible="false"/>&nbsp;
                                <asp:Button runat="server" ID="btnEditSave" Text="Edit" CssClass="flatButton" Width="60"
                                    OnClick="OnButtonInvoiceEditSaveClicked"/>&nbsp;
                                <asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="flatButton" Width="60"
                                    OnClick="OnButtonInvoiceCancelClicked" />
                            </td>                        
                        </tr>
                    </table>                
                </div>
            </div>
            </asp:View>
             <asp:View runat="server" ID="NoPermissionView">
                    <div class="nopermission"><asp:Literal runat="server" ID="lblNoPermission"></asp:Literal></div>
            </asp:View>
        </asp:MultiView>
        <asp:HiddenField ID="hidMode" runat="server" /> 
        </div> 
    </form>
</body>
</html>
