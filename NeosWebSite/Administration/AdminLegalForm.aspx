<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminLegalForm.aspx.cs" Inherits="AdminLegalForm" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Legal form Page</title>
    <script src="../script/utils.js" type="text/javascript"></script>
    <link href="../Styles/Neos.css" rel="Stylesheet" />
</head>
<body>
    <form id="legalFormForm" runat="server">
    <div>
        <telerik:RadScriptManager ID="ScriptManager" runat="server"/>
        <div>
            <telerik:RadAjaxManager ID="MyAjaxManager" runat="server" OnAjaxRequest="OnMyAjaxManagerAjaxRequest">
                <AjaxSettings> 
                    <telerik:AjaxSetting AjaxControlID="gridLegalForm">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridLegalForm" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>                                     
                </AjaxSettings>                
            </telerik:RadAjaxManager>     
            <div class="rightpane_title">
                <asp:Literal runat="server" ID="lblLegalForm" Text="Legal Forms"></asp:Literal>
            </div>
            <div style="margin-top:30px;">
                <table>
                     <tr>
                        <td>
                            <telerik:RadGrid ID="gridLegalForm" GridLines="None" Skin="Office2007" AllowMultiRowSelection="False"
                                EnableAjaxSkinRendering="true" runat="server" AllowPaging="True" AllowSorting="True"
                                PageSize="25" Width="400px" AutoGenerateColumns="false"
                                OnNeedDataSource="OnGridLegalFormNeedDataSource"
                                OnItemDataBound="OnGridLegalFormItemDataBound" 
                                OnPageIndexChanged="OnGridLegalFormPageIndexChanged">
                                <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                <MasterTableView DataKeyNames="FormID" DataMember="ParamLegalForm" AllowMultiColumnSorting="True"
                                    Width="100%" EditMode="PopUp">                        
                                    <Columns>     
                                        <telerik:GridBoundColumn UniqueName="FormID" SortExpression="FormID" HeaderText="Form ID"
                                            DataField="FormID">
                                            <HeaderStyle Width="70%"></HeaderStyle>
                                        </telerik:GridBoundColumn>                                                                                                                                                      
                                        <telerik:GridTemplateColumn UniqueName="TemplateEditLegalFormColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkLegalFormEdit" runat="server" Text="Edit" 
                                                    CommandArgument='<%# Eval("FormID") %>'>  
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="15%"></HeaderStyle>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="TemplateDeleteLegalFormColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkLegalFormDelete" runat="server" Text="Delete" CommandArgument='<%# Eval("FormID") %>'
                                                    OnClientClick="return confirm('Are you sure to delete this item?')" OnClick="OnLegalFormDeleteClicked">
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="15%" HorizontalAlign="Center"></HeaderStyle>
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
                            <asp:LinkButton ID="lnkAddNewLegalForm" runat="server" Text="Add new legal form" 
                                OnClientClick="return OnAddNewLegalFormClientClicked()"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <telerik:RadWindow runat="server" ID="radWindowLegalForm" Skin="Office2007" VisibleOnPageLoad="false" VisibleStatusbar="false"
                            Modal="true" OffsetElementID="offsetElement" Top="30" Left="30" NavigateUrl="AdminLegalFormPopup.aspx"
                            Title="Action" Height="300px" Width="450px" OnClientClose="onClientLegalFormDetailWindowClosed">
                </telerik:RadWindow>
                
            </div>
        </div>
    </div>
    </form>
</body>
</html>
