<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminKnowledge.aspx.cs" Inherits="AdminKnowledge" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Knowledge Page</title>
    <script src="../script/utils.js" type="text/javascript"></script>
    <link href="../Styles/Neos.css" rel="Stylesheet" />
</head>
<body>
    <form id="knowledgeForm" runat="server">
    <div>
        <telerik:RadScriptManager ID="ScriptManager" runat="server"/>
        <div>
            <telerik:RadAjaxManager ID="MyAjaxManager" runat="server" OnAjaxRequest="OnMyAjaxManagerAjaxRequest">
                <AjaxSettings> 
                    <telerik:AjaxSetting AjaxControlID="gridKnowledge">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridKnowledge" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>                                     
                </AjaxSettings>                
            </telerik:RadAjaxManager>     
            <div class="rightpane_title">
                <asp:Literal runat="server" ID="lblKnowledge" Text="Knowledges"></asp:Literal>
            </div>
            <div style="margin-top:30px;">
                <table>                   
                     <tr>
                        <td>
                            <telerik:RadGrid ID="gridKnowledge" GridLines="None" Skin="Office2007" AllowMultiRowSelection="False"
                                EnableAjaxSkinRendering="true" runat="server" AllowPaging="True" AllowSorting="True"
                                PageSize="30" Width="700px" AutoGenerateColumns="false"
                                OnNeedDataSource="OnGridKnowledgeNeedDataSource"
                                OnItemDataBound="OnGridKnowledgeItemDataBound" 
                                OnPageIndexChanged="OnGridKnowledgePageIndexChanged">
                                <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                <MasterTableView DataKeyNames="KnowledgeID" DataMember="ParamKnowledge" AllowMultiColumnSorting="True"
                                    Width="100%" EditMode="PopUp">                        
                                    <Columns>   
                                        <telerik:GridBoundColumn UniqueName="Code" SortExpression="Code" HeaderText="Code"
                                            DataField="Code">
                                            <HeaderStyle Width="30%"></HeaderStyle>
                                        </telerik:GridBoundColumn>                                     
                                        <telerik:GridBoundColumn UniqueName="KnowledgeFamID" SortExpression="KnowledgeFamID" HeaderText="Knowledge category"
                                            DataField="KnowledgeFamID">
                                            <HeaderStyle Width="30%"></HeaderStyle>
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Definition" SortExpression="Definition" HeaderText="Definition"
                                            DataField="Definition">
                                            <HeaderStyle Width="30%"></HeaderStyle>
                                        </telerik:GridBoundColumn>                                          
                                        <telerik:GridTemplateColumn UniqueName="TemplateEditKnowledgeColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkKnowledgeEdit" runat="server" Text="Edit" 
                                                    CommandArgument='<%# Eval("KnowledgeID") %>'>  
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="5%"></HeaderStyle>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="TemplateDeleteKnowledgeColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkKnowledgeDelete" runat="server" Text="Delete" CommandArgument='<%# Eval("KnowledgeID") %>'
                                                    OnClientClick="return confirm('Are you sure to delete this item?')" OnClick="OnKnowledgeDeleteClicked">
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
                            <asp:LinkButton ID="lnkAddNewKnowledge" runat="server" Text="Add new knowledge category" 
                                OnClientClick="return OnAddNewKnowledgeClientClicked()"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <telerik:RadWindow runat="server" ID="radWindowKnowledge" Skin="Office2007" VisibleOnPageLoad="false" VisibleStatusbar="false"
                            Modal="true" OffsetElementID="offsetElement" Top="30" Left="30" NavigateUrl="AdminKnowledgePopup.aspx"
                            Title="Action" Height="300px" Width="450px" OnClientClose="onClientKnowledgeDetailWindowClosed">
                </telerik:RadWindow>
                
            </div>
        </div>
    </div>
    </form>
</body>
</html>
