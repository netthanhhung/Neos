<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminProfiles.aspx.cs" Inherits="AdminProfiles" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Profiles Page</title>
    <script src="../script/utils.js" type="text/javascript"></script>
    <link href="../Styles/Neos.css" rel="Stylesheet" />
</head>
<body>
    <form id="profileForm" runat="server">
    <div>
        <telerik:RadScriptManager ID="ScriptManager" runat="server"/>
        <div>
            <telerik:RadAjaxManager ID="MyAjaxManager" runat="server" OnAjaxRequest="OnMyAjaxManagerAjaxRequest">
                <AjaxSettings> 
                    <telerik:AjaxSetting AjaxControlID="gridProfiles">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridProfiles" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>                                     
                </AjaxSettings>                
            </telerik:RadAjaxManager>     
            <div class="rightpane_title">
                <asp:Literal runat="server" ID="lblProfiles" Text="Profiles"></asp:Literal>
            </div>
            <div style="margin-top:30px;">
                <table>
                    <tr>
                        <td>
                            
                        </td>
                    </tr>
                     <tr>
                        <td>
                            <telerik:RadGrid ID="gridProfiles" GridLines="None" Skin="Office2007" AllowMultiRowSelection="False"
                                EnableAjaxSkinRendering="true" runat="server" AllowPaging="True" AllowSorting="True"
                                PageSize="30" Width="500px" AutoGenerateColumns="false"
                                OnNeedDataSource="OnGridProfileNeedDataSource"
                                OnItemDataBound="OnGridProfileItemDataBound" 
                                OnPageIndexChanged="OnGridProfilePageIndexChanged">
                                <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                <MasterTableView DataKeyNames="ProfileID" DataMember="ParamProfile" AllowMultiColumnSorting="True"
                                    Width="100%" EditMode="PopUp">                        
                                    <Columns> 
                                        <telerik:GridBoundColumn UniqueName="ProfileID" SortExpression="ProfileID" HeaderText="ProfileID"
                                            DataField="ProfileID" Display="false">
                                            <HeaderStyle Width="20%"></HeaderStyle>
                                        </telerik:GridBoundColumn>                                       
                                        <telerik:GridBoundColumn UniqueName="Profile" SortExpression="Profile" HeaderText="Profile"
                                            DataField="Profile">
                                            <HeaderStyle Width="40%"></HeaderStyle>
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="ProfileCode" SortExpression="ProfileCode" HeaderText="Code"
                                            DataField="ProfileCode">
                                            <HeaderStyle Width="40%"></HeaderStyle>
                                        </telerik:GridBoundColumn>                                        
                                        <telerik:GridTemplateColumn UniqueName="TemplateEditProfileColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkProfileEdit" runat="server" Text="Edit" 
                                                    CommandArgument='<%# Eval("ProfileID") %>'>  
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="10%"></HeaderStyle>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="TemplateDeleteProfileColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkProfileDelete" runat="server" Text="Delete" CommandArgument='<%# Eval("ProfileID") %>'
                                                    OnClientClick="return confirm('Are you sure to delete this item?')" OnClick="OnProfileDeleteClicked">
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
                            <asp:LinkButton ID="lnkAddNewProfile" runat="server" Text="Add new profile" 
                                OnClientClick="return OnAddNewProfileClientClicked()"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
               <telerik:RadWindow runat="server" ID="radWindowProfile" Skin="Office2007" VisibleOnPageLoad="false" VisibleStatusbar="false"
                            Modal="true" OffsetElementID="offsetElement" Top="30" Left="30" NavigateUrl="AdminProfilePopup.aspx"
                            Title="Action" Height="300px" Width="450px" OnClientClose="onClientProfileDetailWindowClosed">
                </telerik:RadWindow>
                
            </div>
        </div>
    </div>
    </form>
</body>
</html>
