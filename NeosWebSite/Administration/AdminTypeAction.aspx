<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminTypeAction.aspx.cs" Inherits="AdminTypeAction" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Type Action Page</title>
    <script src="../script/utils.js" type="text/javascript"></script>
    <link href="../Styles/Neos.css" rel="Stylesheet" />
</head>
<body>
    <form id="TypeActionForm" runat="server">
    <div>
        <telerik:RadScriptManager ID="ScriptManager" runat="server"/>
        <div>
            <telerik:RadAjaxManager ID="MyAjaxManager" runat="server" OnAjaxRequest="OnMyAjaxManagerAjaxRequest">
                <AjaxSettings> 
                    <telerik:AjaxSetting AjaxControlID="gridTypeAction">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridTypeAction" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>                                     
                </AjaxSettings>                
            </telerik:RadAjaxManager>     
            <div class="rightpane_title">
                <asp:Literal runat="server" ID="lblTypeAction" Text="Action Types"></asp:Literal>
            </div>
            <div style="margin-top:30px;">
                <table>
                     <tr>
                        <td>
                            <telerik:RadGrid ID="gridTypeAction" GridLines="None" Skin="Office2007" AllowMultiRowSelection="False"
                                EnableAjaxSkinRendering="true" runat="server" AllowPaging="True" AllowSorting="True"
                                PageSize="25" Width="500px" AutoGenerateColumns="false"
                                OnNeedDataSource="OnGridTypeActionNeedDataSource"
                                OnItemDataBound="OnGridTypeActionItemDataBound" 
                                OnPageIndexChanged="OnGridTypeActionPageIndexChanged">
                                <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                <MasterTableView DataKeyNames="ParamActionID" DataMember="ParamTypeAction" AllowMultiColumnSorting="True"
                                    Width="100%" EditMode="PopUp">                        
                                    <Columns>                                        
                                        <telerik:GridBoundColumn UniqueName="Label" SortExpression="Label" HeaderText="Label"
                                            DataField="Label">
                                            <HeaderStyle Width="50%"></HeaderStyle>
                                        </telerik:GridBoundColumn>  
                                         <telerik:GridBoundColumn UniqueName="UnitCode" SortExpression="UnitCode" HeaderText="Unit Code"
                                            DataField="UnitCode">
                                            <HeaderStyle Width="20%"></HeaderStyle>
                                        </telerik:GridBoundColumn>                                                                                
                                        <telerik:GridTemplateColumn UniqueName="TemplateEditTypeActionColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkTypeActionEdit" runat="server" Text="Edit" 
                                                    CommandArgument='<%# Eval("ParamActionID") %>'>  
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="15%"></HeaderStyle>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="TemplateDeleteTypeActionColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkTypeActionDelete" runat="server" Text="Delete" CommandArgument='<%# Eval("ParamActionID") %>'
                                                    OnClientClick="return confirm('Are you sure to delete this item?')" OnClick="OnTypeActionDeleteClicked">
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
                            <asp:LinkButton ID="lnkAddNewTypeAction" runat="server" Text="Add new action type" 
                                OnClientClick="return OnAddNewTypeActionClientClicked()"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <telerik:RadWindow runat="server" ID="radWindowTypeAction" Skin="Office2007" VisibleOnPageLoad="false" VisibleStatusbar="false"
                            Modal="true" OffsetElementID="offsetElement" Top="30" Left="30" NavigateUrl="AdminTypeActionPopup.aspx"
                            Title="Action" Height="300px" Width="450px" OnClientClose="onClientTypeActionDetailWindowClosed">
                </telerik:RadWindow>
                
            </div>
        </div>
    </div>
    </form>
</body>
</html>
