<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminClientStatus.aspx.cs" Inherits="AdminClientStatus" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Client Status Page</title>
    <script src="../script/utils.js" type="text/javascript"></script>
    <link href="../Styles/Neos.css" rel="Stylesheet" />
</head>
<body>
    <form id="ClientStatusForm" runat="server">
    <div>
        <telerik:RadScriptManager ID="ScriptManager" runat="server"/>
        <div class="rightpane_title">
            <asp:Literal runat="server" ID="lblClientStatus" Text="Client Status"></asp:Literal>
        </div>        
        <div style="margin-top:30px;">
            <telerik:RadAjaxManager ID="MyAjaxManager" runat="server" OnAjaxRequest="OnMyAjaxManagerAjaxRequest">
                <AjaxSettings> 
                    <telerik:AjaxSetting AjaxControlID="gridClientStatus">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridClientStatus" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>                                     
                </AjaxSettings>                
            </telerik:RadAjaxManager>     
            <div>
                <table>                    
                     <tr>
                        <td>
                            <telerik:RadGrid ID="gridClientStatus" GridLines="None" Skin="Office2007" AllowMultiRowSelection="False"
                                EnableAjaxSkinRendering="true" runat="server" AllowPaging="True" AllowSorting="True"
                                PageSize="25" Width="400px" AutoGenerateColumns="false"
                                OnNeedDataSource="OnGridClientStatusNeedDataSource"
                                OnItemDataBound="OnGridClientStatusItemDataBound" 
                                OnPageIndexChanged="OnGridClientStatusPageIndexChanged">
                                <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                <MasterTableView DataKeyNames="StatusID" DataMember="ParamClientStatus" AllowMultiColumnSorting="True"
                                    Width="100%" EditMode="PopUp">                        
                                    <Columns>     
                                        <telerik:GridBoundColumn UniqueName="Status" SortExpression="Status" HeaderText="Status"
                                            DataField="Status">
                                            <HeaderStyle Width="40%"></HeaderStyle>
                                        </telerik:GridBoundColumn>                                                                                                                                                      
                                        <telerik:GridTemplateColumn UniqueName="TemplateEditClientStatusColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkClientStatusEdit" runat="server" Text="Edit" 
                                                    CommandArgument='<%# Eval("StatusID") %>'>  
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="10%"></HeaderStyle>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="TemplateDeleteClientStatusColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkClientStatusDelete" runat="server" Text="Delete" CommandArgument='<%# Eval("StatusID") %>'
                                                    OnClientClick="return confirm('Are you sure to delete this item?')" OnClick="OnClientStatusDeleteClicked">
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
                            <asp:LinkButton ID="lnkAddNewClientStatus" runat="server" Text="Add new client status" 
                                OnClientClick="return OnAddNewClientStatusClientClicked()"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <telerik:RadWindow runat="server" ID="radWindowClientStatus" Skin="Office2007" VisibleOnPageLoad="false" VisibleStatusbar="false"
                            Modal="true" OffsetElementID="offsetElement" Top="30" Left="30" NavigateUrl="AdminClientStatusPopup.aspx"
                            Title="Action" Height="300px" Width="450px" OnClientClose="onClientClientStatusDetailWindowClosed">
                </telerik:RadWindow>
                
            </div>
        </div>
    </div>
    </form>
</body>
</html>
