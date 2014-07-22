<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminLocations.aspx.cs" Inherits="AdminLocations" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Locations Page</title>
    <script src="../script/utils.js" type="text/javascript"></script>
    <link href="../Styles/Neos.css" rel="Stylesheet" />
</head>
<body>
    <form id="locationForm" runat="server">
    <div>
        <telerik:RadScriptManager ID="ScriptManager" runat="server"/>
        <div>
            <telerik:RadAjaxManager ID="MyAjaxManager" runat="server" OnAjaxRequest="OnMyAjaxManagerAjaxRequest">
                <AjaxSettings> 
                    <telerik:AjaxSetting AjaxControlID="gridLocations">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridLocations" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>                                     
                </AjaxSettings>                
            </telerik:RadAjaxManager>     
            <div class="rightpane_title">
                <asp:Literal runat="server" ID="lblLocations" Text="Locations"></asp:Literal>
            </div>
            <div style="margin-top:30px;">
                <table>
                     <tr>
                        <td>
                            <telerik:RadGrid ID="gridLocations" GridLines="None" Skin="Office2007" AllowMultiRowSelection="False"
                                EnableAjaxSkinRendering="true" runat="server" AllowPaging="True" AllowSorting="True"
                                PageSize="30" Width="600px" AutoGenerateColumns="false"
                                OnNeedDataSource="OnGridLocationNeedDataSource"
                                OnItemDataBound="OnGridLocationItemDataBound" 
                                OnPageIndexChanged="OnGridLocationPageIndexChanged">
                                <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                <MasterTableView DataKeyNames="Location" DataMember="ParamLocations" AllowMultiColumnSorting="True"
                                    Width="100%" EditMode="PopUp">                        
                                    <Columns>                                        
                                        <telerik:GridBoundColumn UniqueName="Location" SortExpression="Location" HeaderText="Location"
                                            DataField="Location">
                                            <HeaderStyle Width="20%"></HeaderStyle>
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Hierarchy" SortExpression="Hierarchie" HeaderText="Hierarchy"
                                            DataField="Hierarchie">
                                            <HeaderStyle Width="10%"></HeaderStyle>
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="LocationUk" SortExpression="LocationUk" HeaderText="LocationUk"
                                            DataField="LocationUk">
                                            <HeaderStyle Width="20%"></HeaderStyle>
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="LocationNl" SortExpression="LocationNl" HeaderText="LocationNl"
                                            DataField="LocationNl">
                                            <HeaderStyle Width="20%"></HeaderStyle>
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="CodeLocation" SortExpression="CodeLocation" HeaderText="Code"
                                            DataField="CodeLocation">
                                            <HeaderStyle Width="20%"></HeaderStyle>
                                        </telerik:GridBoundColumn>    
                                        <telerik:GridTemplateColumn UniqueName="TemplateEditLocationColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkLocationEdit" runat="server" Text="Edit" 
                                                    CommandArgument='<%# Eval("Location") %>'>  
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="5%"></HeaderStyle>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="TemplateDeleteLocationColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkLocationDelete" runat="server" Text="Delete" CommandArgument='<%# Eval("Location") %>'
                                                    OnClientClick="return confirm('Are you sure to delete this item?')" OnClick="OnLocationDeleteClicked">
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
                            <asp:LinkButton ID="lnkAddNewLocation" runat="server" Text="Add new location" 
                                OnClientClick="return OnAddNewLocationClientClicked()"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <telerik:RadWindow runat="server" ID="radWindowLocation" Skin="Office2007" VisibleOnPageLoad="false" VisibleStatusbar="false"
                            Modal="true" OffsetElementID="offsetElement" Top="30" Left="30" NavigateUrl="AdminLocationPopup.aspx"
                            Title="Action" Height="300px" Width="450px" OnClientClose="onClientLocationDetailWindowClosed">
                </telerik:RadWindow>
                
            </div>
        </div>
    </div>
    </form>
</body>
</html>
