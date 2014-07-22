<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminStudy.aspx.cs" Inherits="AdminStudy" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Study Page</title>
    <script src="../script/utils.js" type="text/javascript"></script>
    <link href="../Styles/Neos.css" rel="Stylesheet" />
</head>
<body>
    <form id="studyForm" runat="server">
    <div>
        <telerik:RadScriptManager ID="ScriptManager" runat="server"/>
        <div>
            <telerik:RadAjaxManager ID="MyAjaxManager" runat="server" OnAjaxRequest="OnMyAjaxManagerAjaxRequest">
                <AjaxSettings> 
                    <telerik:AjaxSetting AjaxControlID="gridStudy">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridStudy" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>                                     
                </AjaxSettings>                
            </telerik:RadAjaxManager>     
            <div class="rightpane_title">
                <asp:Literal runat="server" ID="lblStudy" Text="Studies"></asp:Literal>
            </div>
            <div style="margin-top:30px;">
                <table>
                    <tr>
                        <td>
                            
                        </td>
                    </tr>
                     <tr>
                        <td>
                            <telerik:RadGrid ID="gridStudy" GridLines="None" Skin="Office2007" AllowMultiRowSelection="False"
                                EnableAjaxSkinRendering="true" runat="server" AllowPaging="True" AllowSorting="True"
                                PageSize="25" Width="400px" AutoGenerateColumns="false"
                                OnNeedDataSource="OnGridStudyNeedDataSource"
                                OnItemDataBound="OnGridStudyItemDataBound" 
                                OnPageIndexChanged="OnGridStudyPageIndexChanged">
                                <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                <MasterTableView DataKeyNames="FormationID" DataMember="ParamFormation" AllowMultiColumnSorting="True"
                                    Width="100%" EditMode="PopUp">                        
                                    <Columns>                                        
                                        <telerik:GridBoundColumn UniqueName="Label" SortExpression="Label" HeaderText="Study"
                                            DataField="Label">
                                            <HeaderStyle Width="70%"></HeaderStyle>
                                        </telerik:GridBoundColumn>                                                                                 
                                        <telerik:GridTemplateColumn UniqueName="TemplateEditStudyColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkStudyEdit" runat="server" Text="Edit" 
                                                    CommandArgument='<%# Eval("FormationID") %>'>  
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="15%"></HeaderStyle>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="TemplateDeleteStudyColumn">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkStudyDelete" runat="server" Text="Delete" CommandArgument='<%# Eval("FormationID") %>'
                                                    OnClientClick="return confirm('Are you sure to delete this item?')" OnClick="OnStudyDeleteClicked">
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="15%" HorizontalAlign="Center"></HeaderStyle>
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
                            <asp:LinkButton ID="lnkAddNewStudy" runat="server" Text="Add new study" 
                                OnClientClick="return OnAddNewStudyClientClicked()"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <telerik:RadWindow runat="server" ID="radWindowStudy" Skin="Office2007" VisibleOnPageLoad="false" VisibleStatusbar="false"
                            Modal="true" OffsetElementID="offsetElement" Top="30" Left="30" NavigateUrl="AdminStudyPopup.aspx"
                            Title="Action" Height="300px" Width="450px" OnClientClose="onClientStudyDetailWindowClosed">
                </telerik:RadWindow>
                
            </div>
        </div>
    </div>
    </form>
</body>
</html>
