<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminPermissions.aspx.cs" Inherits="AdminPermissions" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Permissions Page</title>
    <script src="../script/utils.js" type="text/javascript"></script>
    <link href="../Styles/Neos.css" rel="Stylesheet" />
</head>
<body>
    <form id="permissionForm" runat="server">
    <div>
        <telerik:RadScriptManager ID="ScriptManager" runat="server"/>
        <div>
            <telerik:RadAjaxManager ID="MyAjaxManager" runat="server" OnAjaxRequest="OnMyAjaxManagerAjaxRequest">
                <AjaxSettings> 
                    <telerik:AjaxSetting AjaxControlID="gridPermissions">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridPermissions" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>                                     
                </AjaxSettings>                
            </telerik:RadAjaxManager>     
            <div class="rightpane_title">
                <asp:Literal runat="server" ID="lblPermissions" Text="Permissions"></asp:Literal>
            </div>
            <div style="margin-top:30px;">
                <table>
                     <tr>
                        <td>
                            <telerik:RadGrid ID="gridPermissions" GridLines="None" Skin="Office2007" AllowMultiRowSelection="False"
                                EnableAjaxSkinRendering="true" runat="server" AllowPaging="True" AllowSorting="True"
                                PageSize="30" Width="600px" AutoGenerateColumns="false"
                                OnNeedDataSource="OnGridPermissionNeedDataSource"
                                OnItemDataBound="OnGridPermissionItemDataBound" 
                                OnPageIndexChanged="OnGridPermissionPageIndexChanged">
                                <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                <MasterTableView DataKeyNames="PermissionCode" DataMember="ParamPermission" AllowMultiColumnSorting="True"
                                    Width="100%" EditMode="PopUp">                        
                                    <Columns>                                        
                                        <telerik:GridBoundColumn UniqueName="Code" SortExpression="PermissionCode" HeaderText="Code"
                                            DataField="PermissionCode">
                                            <HeaderStyle Width="25%"></HeaderStyle>
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Description" SortExpression="PermissionDescription" HeaderText="Description"
                                            DataField="PermissionDescription">
                                            <HeaderStyle Width="65%"></HeaderStyle>
                                        </telerik:GridBoundColumn>    
                                        <telerik:GridTemplateColumn UniqueName="TemplateEditPermissionColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkPermissionEdit" runat="server" Text="Edit" 
                                                    CommandArgument='<%# Eval("PermissionCode") %>'>  
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="5%"></HeaderStyle>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="TemplateDeletePermissionColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkPermissionDelete" runat="server" Text="Delete" CommandArgument='<%# Eval("PermissionCode") %>'
                                                    OnClientClick="return confirm('Are you sure to delete this item?')" OnClick="OnPermissionDeleteClicked">
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="5%" HorizontalAlign="Center"></HeaderStyle>
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
                            <asp:LinkButton ID="lnkAddNewPermission" runat="server" Text="Add new permission" 
                                OnClientClick="return OnAddNewPermissionClientClicked()"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <telerik:RadWindow runat="server" ID="radWindowPermission" Skin="Office2007" VisibleOnPageLoad="false" VisibleStatusbar="false"
                            Modal="true" OffsetElementID="offsetElement" Top="30" Left="30" NavigateUrl="AdminPermissionPopup.aspx"
                            Title="Action" Height="300px" Width="450px" OnClientClose="onClientPermissionDetailWindowClosed">
                </telerik:RadWindow>
                
            </div>
        </div>
    </div>
    </form>
</body>
</html>
