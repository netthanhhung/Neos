<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InvoicesPage.aspx.cs" AspCompat="True"  ValidateRequest="false" EnableEventValidation="false"
    Inherits="InvoicesPage" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Invoices Page</title>

    <script src="script/utils.js" type="text/javascript"></script>

    <link href="Styles/Neos.css" rel="Stylesheet" />
    <telerik:RadScriptBlock runat="server" ID="scriptBlock">

        <script type="text/javascript">
            function InvoiceRowSelected(sender, args)
            {
               var grid = $find("<%=gridInvoice.ClientID %>");
               var MasterTable = grid.get_masterTableView();
               var selectedRows = MasterTable.get_selectedItems();
               if(selectedRows.length == 1)
               {
                    var invoiceIdPK = MasterTable.getCellByColumnUniqueName(selectedRows[0], "InvoiceIdPK").innerHTML;            
                    var dataItem = $get(args.get_id());        
                    $find("InvoiceAjaxManager").ajaxRequest("RebindInvoiceDetailGrid/" + invoiceIdPK + "/" + dataItem.rowIndex);

                    processInvoiceToolBar("InvoiceGridSelected");
                    if(document.getElementById('<%=btnPrintSelection.ClientID %>') != null)
                        document.getElementById('<%=btnPrintSelection.ClientID %>').style.display = "inline-block";
               }  
               else if(selectedRows.length > 1)
               {
                   var selectedInvoiceID = "";
                   for (var i = 0; i < selectedRows.length; i++)
                   {
                        var row = selectedRows[i];
                        var item = MasterTable.getCellByColumnUniqueName(row, "InvoiceIdPK").innerHTML;
                        selectedInvoiceID += item + ";";
                   }
                   //alert(selectedInvoiceID);
                   $find('<%=InvoiceAjaxManager.ClientID %>').ajaxRequest("InvoiceGridMultiSelected/" + selectedInvoiceID);
                   //processInvoiceToolBar("InvoiceGridMultiSelected");
                   if(document.getElementById('<%=btnPrintSelection.ClientID %>') != null)
                        document.getElementById('<%=btnPrintSelection.ClientID %>').style.display = "inline-block";
               }
               else
               {
                    if(document.getElementById('<%=btnPrintSelection.ClientID %>') != null)
                        document.getElementById('<%=btnPrintSelection.ClientID %>').style.display = "none";
               }
            }   
            function onInvoiceGridDetroying(sender)
            {
                processInvoiceToolBar("InvoiceGridDeSelected");
                if(document.getElementById('<%=btnPrintSelection.ClientID %>') != null)
                        document.getElementById('<%=btnPrintSelection.ClientID %>').style.display = "none";
            }
            function OnSendInvoiceByEmail(url)
            {
                var radWindow = $find('<%=radWindowSendInvoiceByEmail.ClientID %>');
                radWindow.setUrl(url);
                radWindow.show();
                
                return false;
            }
            
            function onInvoiceGrid_RowDblClick(sender, eventAgrs)
            {
                var grid = $find("<%= gridInvoice.ClientID %>");
                var masterTable = grid.get_masterTableView();
                if(masterTable)
                {
                     var ajaxObj = $find('<%=InvoiceAjaxManager.ClientID %>');
                     if(!ajaxObj) return;
                        ajaxObj.ajaxRequest("OpenSelectedInvoice");
                 }
            }
        </script>

    </telerik:RadScriptBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="ScriptManager" runat="server" />
        <div class="rightpane_title">
                <asp:Literal runat="server" ID="lblTitle" Text="Invoices"></asp:Literal></div>
        <div style="margin-top: 30px;">
            <telerik:RadAjaxManager ID="InvoiceAjaxManager" runat="server" OnAjaxRequest="OnMyAjaxManagerAjaxRequest">
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="gridInvoice">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridInvoice" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="gridInvoiceDetails">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridInvoiceDetails" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
            <asp:MultiView runat="server" ID="InvoicingMView" ActiveViewIndex="0">
                <asp:View runat="server" ID="InvoicingView">
                    <div>
                        <table width="100%">
                            <tr>
                                <td>
                                    <telerik:RadGrid ID="gridInvoice" GridLines="None" Skin="Office2007" AllowMultiRowSelection="true"
                                        EnableAjaxSkinRendering="true" runat="server" AllowPaging="True" AllowSorting="True"
                                        AllowCustomPaging="True" PageSize="15" Width="100%" AutoGenerateColumns="false"
                                        OnPageSizeChanged="OnInvoiceGridPageSizeChanged" OnNeedDataSource="OnInvoiceGridNeedDataSource"
                                        OnItemDataBound="OnInvoiceGridItemDataBound" OnPageIndexChanged="OnInvoiceGridPageIndexChanged"
                                        OnSortCommand="OnInvoiceGridSortCommand" OnExcelMLExportRowCreated="OnGridInvoiceExcelMLExportRowCreated"
                                        OnExcelMLExportStylesCreated="OnGridInvoiceExcelMLExportStylesCreated">
                                        <ExportSettings>
                                            <Pdf FontType="Subset" PaperSize="Letter" />
                                            <Excel Format="Html" />
                                            <Csv ColumnDelimiter="Tab" RowDelimiter="NewLine" />
                                        </ExportSettings>
                                        <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                        <MasterTableView DataKeyNames="InvoiceIdPK" DataMember="Invoices" AllowMultiColumnSorting="True"
                                            Width="100%" EditMode="PopUp">
                                            <Columns>
                                                <telerik:GridBoundColumn UniqueName="InvoiceIdPK" DataField="InvoiceIdPK" Display="false">
                                                    <HeaderStyle></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="CompanyName" SortExpression="CompanyName" HeaderText="Company"
                                                    DataField="CompanyName">
                                                    <HeaderStyle Width="10%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="CompanyId" SortExpression="CompanyId" HeaderText="Com. ref."
                                                    DataField="CompanyId" AllowSorting="false">
                                                    <HeaderStyle Width="7%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="IdFactNumber" SortExpression="IdFactNumber"
                                                    HeaderText="Inv/Cre Nbr." DataField="IdFactNumber">
                                                    <HeaderStyle Width="9%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="IdTypeInvoice" SortExpression="IdTypeInvoice"
                                                    HeaderText="Type" DataField="IdTypeInvoice">
                                                    <HeaderStyle Width="4%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="Date" SortExpression="Date" HeaderText="Date"
                                                    DataField="Date" DataType="system.DateTime" DataFormatString="{0:dd/MM/yyyy}">
                                                    <HeaderStyle Width="8%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="TotalHtvaEuro" SortExpression="TotalHtvaEuro"
                                                    HeaderText="Amount VAT Exclude (€)" DataField="TotalHtvaEuro" AllowSorting="false">
                                                    <HeaderStyle Width="10%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="AmountVatEuro" SortExpression="AmountVatEuro"
                                                    HeaderText="VAT amount (€)" DataField="AmountVatEuro" AllowSorting="false">
                                                    <HeaderStyle Width="10%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="TotalAmountIncludeVatEuro" SortExpression="TotalAmountIncludeVatEuro"
                                                    HeaderText="Total VAT (€)" DataField="TotalAmountIncludeVatEuro" AllowSorting="false">
                                                    <HeaderStyle Width="10%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="RemarkToShowed" SortExpression="RemarkToShowed" HeaderText="Remark"
                                                    AllowSorting="false" DataField="RemarkToShowed">
                                                    <HeaderStyle Width="12%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="Remark_Internal" SortExpression="Remark_Internal"
                                                    HeaderText="Internal remark" AllowSorting="false" DataField="Remark_Internal">
                                                    <HeaderStyle Width="12%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn UniqueName="TemplateEditInvoiceColumn">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="lnkInvoiceEdit" runat="server" Text="Edit" NavigateUrl='<%# Eval("InvoiceIdPK","~/InvoiceProfile.aspx?InvoiceIdPK={0}&mode=edit&backurl=visible") %>'>                                
                                                        </asp:HyperLink>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle></HeaderStyle>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="TemplateDeleteInvoiceColumn">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkInvoiceDelete" runat="server" Text="Delete" CommandArgument='<%# Eval("InvoiceIdPK") %>'
                                                            OnClientClick="return confirm('Are you sure to delete this item?')" OnClick="OnInvoiceDeleteClicked">
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle Width="4%" HorizontalAlign="Center"></HeaderStyle>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="TemplateInvoiceInvoiceColumn" Display="false">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="lnkInvoiceInvoice" runat="server" Text="Invoice" NavigateUrl='<%# Eval("InvoiceIdPK","~/InvoiceProfile.aspx?InvoiceIdPK={0}&type=future&mode=edit") %>'>                                
                                                        </asp:HyperLink>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle></HeaderStyle>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings>
                                            <Selecting AllowRowSelect="true" />
                                            <ClientEvents OnRowSelected="InvoiceRowSelected" OnRowDblClick="onInvoiceGrid_RowDblClick"
                                            OnGridDestroying="onInvoiceGridDetroying" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                    <telerik:RadWindow runat="server" ID="radWindowSendInvoiceByEmail" Skin="Office2007"
                                        VisibleOnPageLoad="false" VisibleStatusbar="false" Modal="true" OffsetElementID="offsetElement"
                                        Top="30" Left="30" NavigateUrl="SendEmail.aspx" Title="Send email" Width="750"
                                        Height="500">
                                    </telerik:RadWindow>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:LinkButton runat="server" ID="lnkAddInvoice" Text="Add new invoice" OnClick="OnAddNewInvoiceClick"></asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" ID="lblInvoiceDetail" Text="Invoice details"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadGrid ID="gridInvoiceDetails" GridLines="None" Skin="Office2007" AllowMultiRowSelection="False"
                                        EnableAjaxSkinRendering="true" runat="server" AllowPaging="True" AllowSorting="True"
                                        PageSize="10" Width="100%" AutoGenerateColumns="false" OnNeedDataSource="OnGridInvoiceDetailNeedDataSource">
                                        <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                        <MasterTableView DataKeyNames="InvoiceDetailsId" DataMember="InvoiceDetails" AllowMultiColumnSorting="True"
                                            Width="100%" EditMode="PopUp">
                                            <Columns>
                                                <telerik:GridBoundColumn UniqueName="Description" SortExpression="Description" HeaderText="Description"
                                                    DataField="Description">
                                                    <HeaderStyle Width="30%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="Quantity" SortExpression="Quantity" HeaderText="Quantity"
                                                    DataField="Quantity">
                                                    <HeaderStyle Width="10%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="UnitPriceEuro" SortExpression="UnitPriceEuro"
                                                    HeaderText="Unit price (€)" DataField="UnitPriceEuro">
                                                    <HeaderStyle Width="10%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="AmountEuro" SortExpression="AmountEuro" HeaderText="Amount (€)"
                                                    DataField="AmountEuro">
                                                    <HeaderStyle Width="10%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="VatCode" SortExpression="VatCode" HeaderText="Code VAT"
                                                    DataField="VatCode">
                                                    <HeaderStyle Width="8%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="VatRate" SortExpression="VatRate" HeaderText="% VAT"
                                                    DataField="VatRate">
                                                    <HeaderStyle Width="8%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="AmountVAT" SortExpression="AmountVAT" HeaderText="Amount VAT (€)"
                                                    DataField="AmountVAT">
                                                    <HeaderStyle Width="12%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="TotalAmountVAT" SortExpression="TotalAmountVAT"
                                                    HeaderText="Total Amount (€)" DataField="TotalAmountVAT">
                                                    <HeaderStyle Width="12%"></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings>
                                            <Selecting AllowRowSelect="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="text-align: center">
                        <table width="100%">
                            <tr>
                                <td align="center" nowrap="nowrap">
                                    <asp:Button runat="server" ID="btnPrintAll" Text="Print all" CssClass="flatButton"
                                        Width="100" OnClick="OnButtonInvoicePrintAllClicked" />&nbsp;
                                    <asp:Button runat="server" ID="btnPrintSelection" Text="Print selection" CssClass="flatButton"
                                        Width="100" OnClick="OnButtonInvoicePrintSelectionClicked" Style="display: none;" />
                                    <asp:Button runat="server" ID="btnEmail" Text="Email selection" CssClass="flatButton"
                                        Width="100" OnClick="OnButtonInvoiceEmailSelectionClicked" Visible="false" />
                                    <asp:Button runat="server" ID="btnExcelExport" Text="Excel export" CssClass="flatButton"
                                        Width="100" OnClick="OnButtonInvoiceExcelExportClicked" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:View>
                <asp:View runat="server" ID="DownloadInvoiceView">
                    <div>
                        <asp:Literal runat="server" ID="lblDownload" Text="Download PDF files"></asp:Literal>:</div>
                    <div>
                        <telerik:RadGrid ID="GridInvoiceFile" GridLines="None" Skin="Office2007" AllowMultiRowSelection="true"
                            EnableAjaxSkinRendering="true" runat="server" AutoGenerateColumns="false"
                            OnItemDataBound="OnGridInvoiceFile_ItemDataBound" OnItemCommand="OnGridInvoiceFile_ItemCommand"
                            >
                            <MasterTableView DataKeyNames="invoiceIdPK" DataMember="InvoicingFile" Width="100%">
                                <Columns>
                                    <telerik:GridBoundColumn UniqueName="FilePath" DataField="FilePath" Display="false">
                                    </telerik:GridBoundColumn>
                                              
                                    <telerik:GridBoundColumn UniqueName="InvoiceIdPK" DataField="invoiceIdPK" HeaderText="Invoice Number">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="File" DataField="FileName" HeaderText="File" >                                     
                                    </telerik:GridBoundColumn>
                                    <telerik:GridButtonColumn UniqueName="Download" ButtonType="ImageButton" CommandName="download" ImageUrl="images/24x24/download.png">
                                        <HeaderStyle Width="5%" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridButtonColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings>
                                <Selecting AllowRowSelect="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </div>
                    <div style="text-align:center; margin-top:10px;">
                        <asp:Button runat="server" ID="btnDownloadAllInvoices" Text="Download .Zip File" OnClick="OnButtonDownloadAllInvoices_Click" CssClass="download_zip" Height="40" Width="160" />
                    </div>
                </asp:View>
                <asp:View runat="server" ID="NoPermissionView">
                    <div class="nopermission"><asp:Literal runat="server" ID="lblNoPermission"></asp:Literal></div>
                </asp:View>
            </asp:MultiView>
        </div>
    </form>
</body>
</html>
