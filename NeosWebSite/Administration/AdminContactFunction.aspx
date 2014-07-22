<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminContactFunction.aspx.cs" Inherits="AdminContactFunction" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Contact function Page</title>
    <script src="../script/utils.js" type="text/javascript"></script>
    <link href="../Styles/Neos.css" rel="Stylesheet" />
</head>
<body>
    <form id="contactFunctionForm" runat="server">
    <div>
        <telerik:RadScriptManager ID="ScriptManager" runat="server"/>
        <div class="rightpane_title">
            <asp:Literal runat="server" ID="lblContactFunction" Text="Contact Functions"></asp:Literal>
        </div>        
        <div style="margin-top:30px;">
            <telerik:RadAjaxManager ID="MyAjaxManager" runat="server" OnAjaxRequest="OnMyAjaxManagerAjaxRequest">
                <AjaxSettings> 
                    <telerik:AjaxSetting AjaxControlID="gridContactFunction">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridContactFunction" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>                                     
                </AjaxSettings>                
            </telerik:RadAjaxManager>     
            <div>
                <table>                   
                     <tr>
                        <td>
                            <telerik:RadGrid ID="gridContactFunction" GridLines="None" Skin="Office2007" AllowMultiRowSelection="False"
                                EnableAjaxSkinRendering="true" runat="server" AllowPaging="True" AllowSorting="True"
                                PageSize="25" Width="500px" AutoGenerateColumns="false"
                                OnNeedDataSource="OnGridContactFunctionNeedDataSource"
                                OnItemDataBound="OnGridContactFunctionItemDataBound" 
                                OnPageIndexChanged="OnGridContactFunctionPageIndexChanged">
                                <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                <MasterTableView DataKeyNames="ContactFunctionID" DataMember="ParamContactFunction" AllowMultiColumnSorting="True"
                                    Width="100%" EditMode="PopUp">                        
                                    <Columns>                                        
                                        <telerik:GridBoundColumn UniqueName="FunctionName" SortExpression="FunctionName" HeaderText="Function"
                                            DataField="FunctionName">
                                            <HeaderStyle Width="40%"></HeaderStyle>
                                        </telerik:GridBoundColumn>       
                                        <telerik:GridBoundColumn UniqueName="LogicalOrder" SortExpression="LogicalOrder" HeaderText="Logical order"
                                            DataField="LogicalOrder">
                                            <HeaderStyle Width="40%"></HeaderStyle>
                                        </telerik:GridBoundColumn>                                                                           
                                        <telerik:GridTemplateColumn UniqueName="TemplateEditContactFunctionColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkContactFunctionEdit" runat="server" Text="Edit" 
                                                    CommandArgument='<%# Eval("ContactFunctionID") %>'>  
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="10%"></HeaderStyle>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="TemplateDeleteContactFunctionColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkContactFunctionDelete" runat="server" Text="Delete" CommandArgument='<%# Eval("ContactFunctionID") %>'
                                                    OnClientClick="return confirm('Are you sure to delete this item?')" OnClick="OnContactFunctionDeleteClicked">
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
                            <asp:LinkButton ID="lnkAddNewContactFunction" runat="server" Text="Add new contact function" 
                                OnClientClick="return OnAddNewContactFunctionClientClicked()"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <telerik:RadWindow runat="server" ID="radWindowContactFunction" Skin="Office2007" VisibleOnPageLoad="false" VisibleStatusbar="false"
                            Modal="true" OffsetElementID="offsetElement" Top="30" Left="30" NavigateUrl="AdminContactFunctionPopup.aspx"
                            Title="Action" Height="300px" Width="450px" OnClientClose="onClientContactFunctionDetailWindowClosed">
                </telerik:RadWindow>
                
            </div>
        </div>
    </div>
    </form>
</body>
</html>
