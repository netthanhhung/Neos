<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminNationality.aspx.cs" Inherits="AdminNationality" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Nationalities Page</title>
    <script src="../script/utils.js" type="text/javascript"></script>
    <link href="../Styles/Neos.css" rel="Stylesheet" />
</head>
<body>
    <form id="nationalityForm" runat="server">
    <div>
        <telerik:RadScriptManager ID="ScriptManager" runat="server"/>
        <div>
            <telerik:RadAjaxManager ID="MyAjaxManager" runat="server" OnAjaxRequest="OnMyAjaxManagerAjaxRequest">
                <AjaxSettings> 
                    <telerik:AjaxSetting AjaxControlID="gridNationality">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridNationality" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>                                     
                </AjaxSettings>                
            </telerik:RadAjaxManager>     
            <div class="rightpane_title">
                <asp:Literal runat="server" ID="lblNationality" Text="Nationalities"></asp:Literal>
            </div>
            <div style="margin-top:30px;">
                <table>
                     <tr>
                        <td>
                            <telerik:RadGrid ID="gridNationality" GridLines="None" Skin="Office2007" AllowMultiRowSelection="False"
                                EnableAjaxSkinRendering="true" runat="server" AllowPaging="True" AllowSorting="True"
                                PageSize="25" Width="600px" AutoGenerateColumns="false"
                                OnNeedDataSource="OnGridNationalityNeedDataSource"
                                OnItemDataBound="OnGridNationalityItemDataBound" 
                                OnPageIndexChanged="OnGridNationalityPageIndexChanged">
                                <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                <MasterTableView DataKeyNames="NationaliteID" DataMember="ParamNationalite" AllowMultiColumnSorting="True"
                                    Width="100%" EditMode="PopUp">                        
                                    <Columns>                                        
                                        <telerik:GridBoundColumn UniqueName="NationaliteID" SortExpression="NationaliteID" HeaderText="Nationality ID"
                                            DataField="NationaliteID">
                                            <HeaderStyle Width="40%"></HeaderStyle>
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Label" SortExpression="Label" HeaderText="Label"
                                            DataField="Label">
                                            <HeaderStyle Width="40%"></HeaderStyle>
                                        </telerik:GridBoundColumn>                                          
                                        <telerik:GridTemplateColumn UniqueName="TemplateEditNationalityColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkNationalityEdit" runat="server" Text="Edit" 
                                                    CommandArgument='<%# Eval("NationaliteID") %>'>  
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="10%"></HeaderStyle>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="TemplateDeleteNationalityColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkNationalityDelete" runat="server" Text="Delete" CommandArgument='<%# Eval("NationaliteID") %>'
                                                    OnClientClick="return confirm('Are you sure to delete this item?')" OnClick="OnNationalityDeleteClicked">
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
                            <asp:LinkButton ID="lnkAddNewNationality" runat="server" Text="Add new nationality" 
                                OnClientClick="return OnAddNewNationalityClientClicked()"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <telerik:RadWindow runat="server" ID="radWindowNationality" Skin="Office2007" VisibleOnPageLoad="false" VisibleStatusbar="false"
                            Modal="true" OffsetElementID="offsetElement" Top="30" Left="30" NavigateUrl="AdminNationalityPopup.aspx"
                            Title="Action" Height="300px" Width="450px" OnClientClose="onClientNationalityDetailWindowClosed">
                </telerik:RadWindow>
                
            </div>
        </div>
    </div>
    </form>
</body>
</html>
