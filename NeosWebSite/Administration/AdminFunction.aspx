<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminFunction.aspx.cs" Inherits="AdminFunction" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Function Page</title>
    <script src="../script/utils.js" type="text/javascript"></script>
    <link href="../Styles/Neos.css" rel="Stylesheet" />
</head>
<body>
    <form id="functionForm" runat="server">
    <div>
        <telerik:RadScriptManager ID="ScriptManager" runat="server"/>
        <div class="rightpane_title">
            <asp:Literal runat="server" ID="lblFunction" Text="Functions"></asp:Literal>
        </div>        
        <div style="margin-top:30px;">
            <telerik:RadAjaxManager ID="MyAjaxManager" runat="server" OnAjaxRequest="OnMyAjaxManagerAjaxRequest">
                <AjaxSettings> 
                    <telerik:AjaxSetting AjaxControlID="gridFunction">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridFunction" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>                                     
                </AjaxSettings>                
            </telerik:RadAjaxManager>     
            <div>
                <table>                   
                     <tr>
                        <td>
                            <telerik:RadGrid ID="gridFunction" GridLines="None" Skin="Office2007" AllowMultiRowSelection="False"
                                EnableAjaxSkinRendering="true" runat="server" AllowPaging="True" AllowSorting="True"
                                PageSize="30" Width="600px" AutoGenerateColumns="false"
                                OnNeedDataSource="OnGridFunctionNeedDataSource"
                                OnItemDataBound="OnGridFunctionItemDataBound" 
                                OnPageIndexChanged="OnGridFunctionPageIndexChanged">
                                <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                <MasterTableView DataKeyNames="FunctionID" DataMember="ParamFunction" AllowMultiColumnSorting="True"
                                    Width="100%" EditMode="PopUp">                        
                                    <Columns>   
                                        <telerik:GridBoundColumn UniqueName="Label" SortExpression="Label" HeaderText="Label"
                                            DataField="Label">
                                            <HeaderStyle Width="40%"></HeaderStyle>
                                        </telerik:GridBoundColumn>                                     
                                        <telerik:GridBoundColumn UniqueName="FunctionFamID" SortExpression="FunctionFamID" HeaderText="Function category"
                                            DataField="FunctionFamID">
                                            <HeaderStyle Width="40%"></HeaderStyle>
                                        </telerik:GridBoundColumn>                                                                                  
                                        <telerik:GridTemplateColumn UniqueName="TemplateEditFunctionColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkFunctionEdit" runat="server" Text="Edit" 
                                                    CommandArgument='<%# Eval("FunctionID") %>'>  
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="10%"></HeaderStyle>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="TemplateDeleteFunctionColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkFunctionDelete" runat="server" Text="Delete" CommandArgument='<%# Eval("FunctionID") %>'
                                                    OnClientClick="return confirm('Are you sure to delete this item?')" OnClick="OnFunctionDeleteClicked">
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
                            <asp:LinkButton ID="lnkAddNewFunction" runat="server" Text="Add new function category" 
                                OnClientClick="return OnAddNewFunctionClientClicked()"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <telerik:RadWindow runat="server" ID="radWindowFunction" Skin="Office2007" VisibleOnPageLoad="false" VisibleStatusbar="false"
                            Modal="true" OffsetElementID="offsetElement" Top="30" Left="30" NavigateUrl="AdminFunctionPopup.aspx"
                            Title="Action" Height="300px" Width="450px" OnClientClose="onClientFunctionDetailWindowClosed">
                </telerik:RadWindow>
                
            </div>
        </div>
    </div>
    </form>
</body>
</html>
