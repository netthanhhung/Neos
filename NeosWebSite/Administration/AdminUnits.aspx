<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminUnits.aspx.cs" Inherits="AdminUnits" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Units Page</title>
    <script src="../script/utils.js" type="text/javascript"></script>
    <link href="../Styles/Neos.css" rel="Stylesheet" />
</head>
<body>
    <form id="unitForm" runat="server">
    <div>
        <telerik:RadScriptManager ID="ScriptManager" runat="server"/>
        <div>
            <telerik:RadAjaxManager ID="MyAjaxManager" runat="server" OnAjaxRequest="OnMyAjaxManagerAjaxRequest">
                <AjaxSettings> 
                    <telerik:AjaxSetting AjaxControlID="gridUnits">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridUnits" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>                                     
                </AjaxSettings>                
            </telerik:RadAjaxManager>     
            <div class="rightpane_title">
                <asp:Literal runat="server" ID="lblUnits" Text="Units"></asp:Literal>
            </div>
            <div style="margin-top:30px;">
                <table>
                     <tr>
                        <td>
                            <telerik:RadGrid ID="gridUnits" GridLines="None" Skin="Office2007" AllowMultiRowSelection="False"
                                EnableAjaxSkinRendering="true" runat="server" AllowPaging="True" AllowSorting="True"
                                PageSize="30" Width="600px" AutoGenerateColumns="false"
                                OnNeedDataSource="OnGridUnitNeedDataSource"
                                OnItemDataBound="OnGridUnitItemDataBound" 
                                OnPageIndexChanged="OnGridUnitPageIndexChanged">
                                <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                <MasterTableView DataKeyNames="TypeID" DataMember="ParamType" AllowMultiColumnSorting="True"
                                    Width="100%" EditMode="PopUp">                        
                                    <Columns>                                        
                                        <telerik:GridBoundColumn UniqueName="TypeID" SortExpression="TypeID" HeaderText="Type ID"
                                            DataField="TypeID">
                                            <HeaderStyle Width="30%"></HeaderStyle>
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Label" SortExpression="Label" HeaderText="Label"
                                            DataField="Label">
                                            <HeaderStyle Width="30%"></HeaderStyle>
                                        </telerik:GridBoundColumn>                                           
                                        <telerik:GridTemplateColumn UniqueName="TemplateEditUnitColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkUnitEdit" runat="server" Text="Edit" 
                                                    CommandArgument='<%# Eval("TypeID") %>'>  
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="20%"></HeaderStyle>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="TemplateDeleteUnitColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkUnitDelete" runat="server" Text="Delete" CommandArgument='<%# Eval("TypeID") %>'
                                                    OnClientClick="return confirm('Are you sure to delete this item?')" OnClick="OnUnitDeleteClicked">
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="20%" HorizontalAlign="Center"></HeaderStyle>
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
                            <asp:LinkButton ID="lnkAddNewUnit" runat="server" Text="Add new unit" 
                                OnClientClick="return OnAddNewUnitClientClicked()"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <telerik:RadWindow runat="server" ID="radWindowUnit" Skin="Office2007" VisibleOnPageLoad="false" VisibleStatusbar="false"
                            Modal="true" OffsetElementID="offsetElement" Top="30" Left="30" NavigateUrl="AdminUnitPopup.aspx"
                            Title="Action" Height="300px" Width="450px" OnClientClose="onClientUnitDetailWindowClosed">
                </telerik:RadWindow>
                
            </div>
        </div>
    </div>
    </form>
</body>
</html>
