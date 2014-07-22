<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminUserAndPermission.aspx.cs" Inherits="AdminUserAndPermission" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>User and permission Page</title>
    <script src="../script/utils.js" type="text/javascript"></script>
    <link href="../Styles/Neos.css" rel="Stylesheet" />
</head>
<body>
    <script type="text/javascript">

    function userRowSelected(sender, args)
    {       
       var tableView = args.get_tableView(); 
       if(tableView.get_selectedItems().length == 1)
       {
            var userID = tableView.getCellByColumnUniqueName(tableView.get_selectedItems()[0], "UserID").innerHTML;
            var dataItem = $get(args.get_id());        
            $find("MyAjaxManager").ajaxRequest("RebindPermissionGrid/" + userID + "/" + dataItem.rowIndex);
       }
       else
       {
            
       }
    }
    </script>
    <form id="userAndPermissionForm" runat="server">
    <div>
        <telerik:RadScriptManager ID="ScriptManager" runat="server" />
        <div>
            <telerik:RadAjaxManager ID="MyAjaxManager" runat="server" OnAjaxRequest="OnMyAjaxManagerAjaxRequest">
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="gridUsers">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridUsers" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>    
                    <telerik:AjaxSetting AjaxControlID="gridPermissions">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridPermissions" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>                                     
                </AjaxSettings>                
            </telerik:RadAjaxManager>     
            <div class="rightpane_title">
                <asp:Literal runat="server" ID="lblUsers" Text="Users & Permissions"></asp:Literal>
            </div>
            <div style="margin-top:30px;">
                <table width="100%">
                    <tr>
                        <td>
                            <telerik:RadGrid ID="gridUsers" GridLines="None" Skin="Office2007" AllowMultiRowSelection="False"
                                EnableAjaxSkinRendering="true" runat="server" AllowPaging="True" AllowSorting="True"
                                PageSize="20" Width="100%" AutoGenerateColumns="false"
                                OnNeedDataSource="OnGridUserNeedDataSource"
                                OnItemDataBound="OnGridUserItemDataBound" 
                                OnPageIndexChanged="OnGridUserPageIndexChanged">
                                <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                <MasterTableView DataKeyNames="UserID" DataMember="ParamUser" AllowMultiColumnSorting="True"
                                    Width="100%" EditMode="PopUp">                        
                                    <Columns>
                                        <telerik:GridBoundColumn UniqueName="UserID" SortExpression="UserID" HeaderText="User ID"
                                             DataField="UserID">
                                            <HeaderStyle Width="10%"></HeaderStyle>
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Name" SortExpression="LastName" HeaderText="Name"
                                            DataField="LastName">
                                            <HeaderStyle Width="20%"></HeaderStyle>
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Gender" SortExpression="Gender" HeaderText="Gender"
                                            DataField="Gender">
                                            <HeaderStyle Width="5%"></HeaderStyle>
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Email" SortExpression="Email" HeaderText="Email"
                                            DataField="Email">
                                            <HeaderStyle Width="30%"></HeaderStyle>
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Telephone" SortExpression="Telephone" HeaderText="Telephone"
                                            DataField="Telephone">
                                            <HeaderStyle Width="20%"></HeaderStyle>
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Active" SortExpression="Active" HeaderText="Active"
                                            DataField="Active">
                                            <HeaderStyle Width="5%"></HeaderStyle>
                                        </telerik:GridBoundColumn>                            
                                        <telerik:GridTemplateColumn UniqueName="TemplateEditUserColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkUserEdit" runat="server" Text="Edit" CommandArgument='<%# Eval("UserID") %>'
                                                  >  
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="5%"></HeaderStyle>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="TemplateDeleteUserColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkUserDelete" runat="server" Text="Delete" CommandArgument='<%# Eval("UserID") %>'
                                                    OnClientClick="return confirm('Are you sure to delete this item?')" OnClick="OnUserDeleteClicked">
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="5%" HorizontalAlign="Center"></HeaderStyle>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                                <ClientSettings>
                                    <Selecting AllowRowSelect="true" />
                                    <ClientEvents OnRowSelected="userRowSelected" />
                                </ClientSettings>
                            </telerik:RadGrid>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:LinkButton ID="lnkAddNewUser" runat="server" Text="Add new user" OnClientClick="return OnAddNewUserClientClicked()"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal runat="server" ID="lblPermission" Text="Permissions"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadGrid ID="gridPermissions" GridLines="None" Skin="Office2007" AllowMultiRowSelection="False"
                                EnableAjaxSkinRendering="true" runat="server" AllowPaging="True" AllowSorting="True"
                                PageSize="5" Width="500px" AutoGenerateColumns="false"
                                OnNeedDataSource="OnGridPermissionNeedDataSource">
                                <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                <MasterTableView DataKeyNames="PermissionCode" DataMember="ParamPermission" AllowMultiColumnSorting="True"
                                    Width="100%" EditMode="PopUp">                        
                                    <Columns>                                        
                                        <telerik:GridBoundColumn UniqueName="Code" SortExpression="PermissionCode" HeaderText="Code"
                                            DataField="PermissionCode">
                                            <HeaderStyle Width="30%"></HeaderStyle>
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Description" SortExpression="PermissionDescription" HeaderText="Description"
                                            DataField="PermissionDescription">
                                            <HeaderStyle Width="70%"></HeaderStyle>
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
                <telerik:RadWindow runat="server" ID="radWindowUser" Skin="Office2007" VisibleOnPageLoad="false" VisibleStatusbar="false"
                            Modal="true" OffsetElementID="offsetElement" Top="30" Left="30" NavigateUrl="AdminUserPopup.aspx"
                            Title="Action" Height="450px" Width="550px" OnClientClose="onClientUserDetailWindowClosed">
                </telerik:RadWindow>
            </div>                                           
        </div>
    </div>
    </form>
</body>
</html>
