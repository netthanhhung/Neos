<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InvoiceUnpaidPage.aspx.cs" aspCompat="True" Inherits="InvoiceUnpaidPage" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Unpaid invoices Page</title>
    <script src="script/utils.js" type="text/javascript"></script>
    <link href="Styles/Neos.css" rel="Stylesheet" />
    <telerik:RadScriptBlock runat="server" ID="scriptBlock">    
    </telerik:RadScriptBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="ScriptManager" runat="server" />
        <div class="rightpane_title">
                <asp:Literal runat="server" ID="lblInvoice" Text=""></asp:Literal></div>
            <div style="margin-top: 30px;">
            <telerik:RadAjaxManager ID="InvoiceUnpaidAjaxManager" runat="server">
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="gridInvoiceUnpaid">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridInvoiceUnpaid" />
                        </UpdatedControls>
                    </telerik:AjaxSetting> 
                </AjaxSettings>                              
            </telerik:RadAjaxManager>      
            <asp:MultiView runat="server" ID="TurnOverMView" ActiveViewIndex="0">
                <asp:View runat="server" ID="InvoiceTurnOverView">      
                <div>
                     <table width="500px">                        
                        <tr>
                            <td>                           
                                 <telerik:RadGrid ID="gridInvoiceUnpaid" GridLines="None" Skin="Office2007" AllowMultiRowSelection="false"
                                    EnableAjaxSkinRendering="true" runat="server" AllowPaging="True" AllowSorting="True" AllowCustomPaging="True"
                                    PageSize="20" Width="100%" AutoGenerateColumns="false" OnPageSizeChanged="OnInvoiceUnpaidGridPageSizeChanged"
                                    OnNeedDataSource="OnInvoiceUnpaidGridNeedDataSource" 
                                    OnPageIndexChanged="OnInvoiceUnpaidGridPageIndexChanged" OnSortCommand="OnInvoiceUnpaidGridSortCommand">
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                    <MasterTableView DataKeyNames="CompanyID" DataMember="InvoiceUnpaids" AllowMultiColumnSorting="True"
                                        Width="100%" EditMode="PopUp">                        
                                        <Columns>
                                            <telerik:GridBoundColumn UniqueName="CompanyID" DataField="CompanyID" Display="false">
                                                <HeaderStyle></HeaderStyle>
                                            </telerik:GridBoundColumn>
                                            <telerik:GridTemplateColumn UniqueName="CompanyName" SortExpression="CompanyName"
                                                HeaderText="Company">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="lnkCompanyName" runat="server" Text='<%# Eval("CompanyName") %>'
                                                        NavigateUrl='<%# Eval("CompanyID","~/InvoicesPage.aspx?CompanyId={0}") %>'>                                
                                                    </asp:HyperLink>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                                <HeaderStyle Width="40%"></HeaderStyle>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn UniqueName="DueAmount" SortExpression="DueAmount" HeaderText="Due amount"
                                                DataField="DueAmount">
                                                <HeaderStyle Width="30%"></HeaderStyle>
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn UniqueName="OldestDate" SortExpression="OldestDate" HeaderText="Oldest unpaid invoice"
                                                DataField="OldestDate" DataType="system.DateTime"
                                                DataFormatString="{0:dd/MM/yyyy}">
                                                <HeaderStyle Width="30%"></HeaderStyle>
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
                </asp:View>
                <asp:View runat="server" ID="NoPermissionView">
                        <div class="nopermission"><asp:Literal runat="server" ID="lblNoPermission"></asp:Literal></div>
                </asp:View>
            </asp:MultiView>
        </div>
    </form>
</body>
</html>
