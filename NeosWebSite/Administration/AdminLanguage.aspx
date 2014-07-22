<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminLanguage.aspx.cs" Inherits="AdminLanguage" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Languages Page</title>
    <script src="../script/utils.js" type="text/javascript"></script>
    <link href="../Styles/Neos.css" rel="Stylesheet" />
</head>
<body>
    <form id="languageForm" runat="server">
    <div>
        <telerik:RadScriptManager ID="ScriptManager" runat="server"/>
        <div>
            <telerik:RadAjaxManager ID="MyAjaxManager" runat="server" OnAjaxRequest="OnMyAjaxManagerAjaxRequest">
                <AjaxSettings> 
                    <telerik:AjaxSetting AjaxControlID="gridLanguage">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridLanguage" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>                                     
                </AjaxSettings>                
            </telerik:RadAjaxManager>     
            <div class="rightpane_title">
                <asp:Literal runat="server" ID="lblLanguage" Text="Languages"></asp:Literal>
            </div>
            <div style="margin-top:30px;">
                <table>
                     <tr>
                        <td>
                            <telerik:RadGrid ID="gridLanguage" GridLines="None" Skin="Office2007" AllowMultiRowSelection="False"
                                EnableAjaxSkinRendering="true" runat="server" AllowPaging="True" AllowSorting="True"
                                PageSize="30" Width="600px" AutoGenerateColumns="false"
                                OnNeedDataSource="OnGridLanguageNeedDataSource"
                                OnItemDataBound="OnGridLanguageItemDataBound" 
                                OnPageIndexChanged="OnGridLanguagePageIndexChanged">
                                <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                <MasterTableView DataKeyNames="LangueID" DataMember="ParamLangue" AllowMultiColumnSorting="True"
                                    Width="100%" EditMode="PopUp">                        
                                    <Columns>                                        
                                        <telerik:GridBoundColumn UniqueName="LangueID" SortExpression="LangueID" HeaderText="Langue ID"
                                            DataField="LangueID">
                                            <HeaderStyle Width="40%"></HeaderStyle>
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Label" SortExpression="Label" HeaderText="Label"
                                            DataField="Label">
                                            <HeaderStyle Width="40%"></HeaderStyle>
                                        </telerik:GridBoundColumn>                                          
                                        <telerik:GridTemplateColumn UniqueName="TemplateEditLanguageColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkLanguageEdit" runat="server" Text="Edit" 
                                                    CommandArgument='<%# Eval("LangueID") %>'>  
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="10%"></HeaderStyle>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="TemplateDeleteLanguageColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkLanguageDelete" runat="server" Text="Delete" CommandArgument='<%# Eval("LangueID") %>'
                                                    OnClientClick="return confirm('Are you sure to delete this item?')" OnClick="OnLanguageDeleteClicked">
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
                            <asp:LinkButton ID="lnkAddNewLanguage" runat="server" Text="Add new language" 
                                OnClientClick="return OnAddNewLanguageClientClicked()"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <telerik:RadWindow runat="server" ID="radWindowLanguage" Skin="Office2007" VisibleOnPageLoad="false" VisibleStatusbar="false"
                            Modal="true" OffsetElementID="offsetElement" Top="30" Left="30" NavigateUrl="AdminLanguagePopup.aspx"
                            Title="Action" Height="300px" Width="450px" OnClientClose="onClientLanguageDetailWindowClosed">
                </telerik:RadWindow>
                
            </div>
        </div>
    </div>
    </form>
</body>
</html>
