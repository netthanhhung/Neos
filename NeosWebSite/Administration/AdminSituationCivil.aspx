<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminSituationCivil.aspx.cs" Inherits="AdminSituationCivil" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Civil Situations Page</title>
    <script src="../script/utils.js" type="text/javascript"></script>
    <link href="../Styles/Neos.css" rel="Stylesheet" />
</head>
<body>
    <form id="SituationCivilForm" runat="server">
    <div>
        <telerik:RadScriptManager ID="ScriptManager" runat="server"/>
        <div>
            <telerik:RadAjaxManager ID="MyAjaxManager" runat="server" OnAjaxRequest="OnMyAjaxManagerAjaxRequest">
                <AjaxSettings> 
                    <telerik:AjaxSetting AjaxControlID="gridSituationCivil">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridSituationCivil" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>                                     
                </AjaxSettings>                
            </telerik:RadAjaxManager>     
            <div class="rightpane_title">
                <asp:Literal runat="server" ID="lblSituationCivil" Text="Civil Situations"></asp:Literal>
            </div>
            <div style="margin-top:30px;">
                <table>
                     <tr>
                        <td>
                            <telerik:RadGrid ID="gridSituationCivil" GridLines="None" Skin="Office2007" AllowMultiRowSelection="False"
                                EnableAjaxSkinRendering="true" runat="server" AllowPaging="True" AllowSorting="True"
                                PageSize="25" Width="600px" AutoGenerateColumns="false"
                                OnNeedDataSource="OnGridSituationCivilNeedDataSource"
                                OnItemDataBound="OnGridSituationCivilItemDataBound" 
                                OnPageIndexChanged="OnGridSituationCivilPageIndexChanged">
                                <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                <MasterTableView DataKeyNames="Code" DataMember="ParamSituationCivil" AllowMultiColumnSorting="True"
                                    Width="100%" EditMode="PopUp">                        
                                    <Columns>      
                                        <telerik:GridBoundColumn UniqueName="Code" SortExpression="Code" HeaderText="Code"
                                            DataField="Code">
                                            <HeaderStyle Width="20%"></HeaderStyle>
                                        </telerik:GridBoundColumn>                                  
                                        <telerik:GridBoundColumn UniqueName="CodeType" SortExpression="CodeType" HeaderText="Code type"
                                            DataField="CodeType">
                                            <HeaderStyle Width="30%"></HeaderStyle>
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Label" SortExpression="Label" HeaderText="Label"
                                            DataField="Label">
                                            <HeaderStyle Width="30%"></HeaderStyle>
                                        </telerik:GridBoundColumn>                                          
                                        <telerik:GridTemplateColumn UniqueName="TemplateEditSituationCivilColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkSituationCivilEdit" runat="server" Text="Edit" 
                                                    CommandArgument='<%# Eval("Code") %>'>  
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="10%"></HeaderStyle>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="TemplateDeleteSituationCivilColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkSituationCivilDelete" runat="server" Text="Delete" CommandArgument='<%# Eval("Code") %>'
                                                    OnClientClick="return confirm('Are you sure to delete this item?')" OnClick="OnSituationCivilDeleteClicked">
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
                            <asp:LinkButton ID="lnkAddNewSituationCivil" runat="server" Text="Add new civil situation" 
                                OnClientClick="return OnAddNewSituationCivilClientClicked()"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <telerik:RadWindow runat="server" ID="radWindowSituationCivil" Skin="Office2007" VisibleOnPageLoad="false" VisibleStatusbar="false"
                            Modal="true" OffsetElementID="offsetElement" Top="30" Left="30" NavigateUrl="AdminSituationCivilPopup.aspx"
                            Title="Action" Height="300px" Width="450px" OnClientClose="onClientSituationCivilDetailWindowClosed">
                </telerik:RadWindow>
                
            </div>
        </div>
    </div>
    </form>
</body>
</html>
