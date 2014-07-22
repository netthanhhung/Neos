<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminFunctionFam.aspx.cs" Inherits="AdminFunctionFam" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Function categories Page</title>
    <script src="../script/utils.js" type="text/javascript"></script>
    <link href="../Styles/Neos.css" rel="Stylesheet" />
</head>
<body>
    <form id="functionFamForm" runat="server">
    <div>
        <telerik:RadScriptManager ID="ScriptManager" runat="server"/>
        <telerik:RadAjaxManager ID="MyAjaxManager" runat="server" OnAjaxRequest="OnMyAjaxManagerAjaxRequest">
                <AjaxSettings> 
                    <telerik:AjaxSetting AjaxControlID="gridFunctionFam">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridFunctionFam" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>                                     
                </AjaxSettings>                
            </telerik:RadAjaxManager>     
            <div class="rightpane_title">
                <asp:Literal runat="server" ID="lblFunctionFam" Text="Function Categories"></asp:Literal>
            </div>
            <div style="margin-top:30px;">
                <table>
                     <tr>
                        <td>
                            <telerik:RadGrid ID="gridFunctionFam" GridLines="None" Skin="Office2007" AllowMultiRowSelection="False"
                                EnableAjaxSkinRendering="true" runat="server" AllowPaging="True" AllowSorting="True"
                                PageSize="30" Width="600px" AutoGenerateColumns="false"
                                OnNeedDataSource="OnGridFunctionFamNeedDataSource"
                                OnItemDataBound="OnGridFunctionFamItemDataBound" 
                                OnPageIndexChanged="OnGridFunctionFamPageIndexChanged">
                                <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                <MasterTableView DataKeyNames="FonctionFamID" DataMember="ParamFunctionFam" AllowMultiColumnSorting="True"
                                    Width="100%" EditMode="PopUp">                        
                                    <Columns>                                        
                                        <telerik:GridBoundColumn UniqueName="FonctionFamID" SortExpression="FonctionFamID" HeaderText="Function category"
                                            DataField="FonctionFamID">
                                            <HeaderStyle Width="40%"></HeaderStyle>
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Genre" SortExpression="Genre" HeaderText="Genre"
                                            DataField="Genre">
                                            <HeaderStyle Width="40%"></HeaderStyle>
                                        </telerik:GridBoundColumn>                                          
                                        <telerik:GridTemplateColumn UniqueName="TemplateEditFunctionFamColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkFunctionFamEdit" runat="server" Text="Edit" 
                                                    CommandArgument='<%# Eval("FonctionFamID") %>'>  
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="10%"></HeaderStyle>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="TemplateDeleteFunctionFamColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkFunctionFamDelete" runat="server" Text="Delete" CommandArgument='<%# Eval("FonctionFamID") %>'
                                                    OnClientClick="return confirm('Are you sure to delete this item?')" OnClick="OnFunctionFamDeleteClicked">
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
                            <asp:LinkButton ID="lnkAddNewFunctionFam" runat="server" Text="Add new function category" 
                                OnClientClick="return OnAddNewFunctionFamClientClicked()"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
               <telerik:RadWindow runat="server" ID="radWindowFunctionFam" Skin="Office2007" VisibleOnPageLoad="false" VisibleStatusbar="false"
                            Modal="true" OffsetElementID="offsetElement" Top="30" Left="30" NavigateUrl="AdminFunctionFamPopup.aspx"
                            Title="Action" Height="300px" Width="450px" OnClientClose="onClientFunctionFamDetailWindowClosed">
                </telerik:RadWindow>
                
            </div>
        </div>
    </div>
    </form>
</body>
</html>
