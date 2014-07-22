<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminKnowledgeFam.aspx.cs" Inherits="AdminKnowledgeFam" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Knowledge categories Page</title>
    <script src="../script/utils.js" type="text/javascript"></script>
    <link href="../Styles/Neos.css" rel="Stylesheet" />
</head>
<body>
    <form id="knowledgeFamForm" runat="server">
    <div>
        <telerik:RadScriptManager ID="ScriptManager" runat="server"/>
        <div>
            <telerik:RadAjaxManager ID="MyAjaxManager" runat="server" OnAjaxRequest="OnMyAjaxManagerAjaxRequest">
                <AjaxSettings> 
                    <telerik:AjaxSetting AjaxControlID="gridKnowledgeFam">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridKnowledgeFam" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>                                     
                </AjaxSettings>                
            </telerik:RadAjaxManager>     
            <div class="rightpane_title">
                <asp:Literal runat="server" ID="lblKnowledgeFam" Text="Knowledge Categories"></asp:Literal>
            </div>
            <div style="margin-top:30px;">
                <table>
                     <tr>
                        <td>
                            <telerik:RadGrid ID="gridKnowledgeFam" GridLines="None" Skin="Office2007" AllowMultiRowSelection="False"
                                EnableAjaxSkinRendering="true" runat="server" AllowPaging="True" AllowSorting="True"
                                PageSize="30" Width="600px" AutoGenerateColumns="false"
                                OnNeedDataSource="OnGridKnowledgeFamNeedDataSource"
                                OnItemDataBound="OnGridKnowledgeFamItemDataBound" 
                                OnPageIndexChanged="OnGridKnowledgeFamPageIndexChanged">
                                <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                <MasterTableView DataKeyNames="ConFamilleID" DataMember="ParamKnowledgeFam" AllowMultiColumnSorting="True"
                                    Width="100%" EditMode="PopUp">                        
                                    <Columns>                                        
                                        <telerik:GridBoundColumn UniqueName="ConFamilleID" SortExpression="ConFamilleID" HeaderText="Knowledge category"
                                            DataField="ConFamilleID">
                                            <HeaderStyle Width="40%"></HeaderStyle>
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Genre" SortExpression="Genre" HeaderText="Genre"
                                            DataField="Genre">
                                            <HeaderStyle Width="40%"></HeaderStyle>
                                        </telerik:GridBoundColumn>                                          
                                        <telerik:GridTemplateColumn UniqueName="TemplateEditKnowledgeFamColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkKnowledgeFamEdit" runat="server" Text="Edit" 
                                                    CommandArgument='<%# Eval("ConFamilleID") %>'>  
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="10%"></HeaderStyle>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="TemplateDeleteKnowledgeFamColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkKnowledgeFamDelete" runat="server" Text="Delete" CommandArgument='<%# Eval("ConFamilleID") %>'
                                                    OnClientClick="return confirm('Are you sure to delete this item?')" OnClick="OnKnowledgeFamDeleteClicked">
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
                            <asp:LinkButton ID="lnkAddNewKnowledgeFam" runat="server" Text="Add new knowledge category" 
                                OnClientClick="return OnAddNewKnowledgeFamClientClicked()"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <telerik:RadWindow runat="server" ID="radWindowKnowledgeFam" Skin="Office2007" VisibleOnPageLoad="false" VisibleStatusbar="false"
                            Modal="true" OffsetElementID="offsetElement" Top="30" Left="30" NavigateUrl="AdminKnowledgeFamPopup.aspx"
                            Title="Action" Height="300px" Width="450px" OnClientClose="onClientKnowledgeFamDetailWindowClosed">
                </telerik:RadWindow>
                
            </div>
        </div>
    </div>
    </form>
</body>
</html>
