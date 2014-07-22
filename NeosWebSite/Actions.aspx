<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Actions.aspx.cs" Inherits="Actions" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="ew" Namespace="eWorld.UI" Assembly="eWorld.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Actions Page</title>
    <link href="Styles/Neos.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="script/utils.js"></script>
    
</head>
<body>
<script type="text/javascript">
    function onActionGrid_RowSelected(sender, args)
    {    
       var tableView = args.get_tableView(); 
       if(tableView.get_selectedItems().length == 1)
       {
            //update action toolbar
            processActionToolBar("ActionGridSelected");
       }
       else
       {
            
       }
       return false;
    }
    function onActionGrid_Detroying(sender, Args)
    {
        processActionToolBar("ActionGridDeSelected");
    }
    
    function onActionGrid_RowDblClick(sender, eventAgrs)
    {        
        var grid = $find("<%= gridActions.ClientID %>");
        var masterTable = grid.get_masterTableView();
        if(masterTable)
        {
            //TaskNbr
             var ajaxObj = $find('<%=ActionAjaxManager.ClientID %>');
             if(!ajaxObj) return;
                ajaxObj.ajaxRequest("OpenSelectedAction");
         }
    }
</script>
    <form id="actionsForm" runat="server">
        <telerik:RadScriptManager ID="ScriptManager" runat="server" />
        <div class="rightpane_title"><asp:Literal runat="server" ID="lblActionTitle" Text="Actions"></asp:Literal></div>        
        <div style="margin-top:30px;">
            <telerik:RadAjaxManager  EnableAJAX="true" ID="ActionAjaxManager" runat="server" OnAjaxRequest="OnMyAjaxManagerAjaxRequest">
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="gridActions">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="gridActions"/>
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
            
            <table width="99%">
                <tr>
                    <td>
                        <telerik:RadGrid ID="gridActions" GridLines="None" Skin="Office2007" AllowMultiRowSelection="False"
                            runat="server" AllowPaging="True" AllowSorting="True" AllowCustomPaging="true" PageSize="20" Width="100%"
                            AutoGenerateColumns="false" 
                            OnItemDataBound="OnGridActionItemDataBound" OnPageIndexChanged="OnGridActionPageIndexChanged"
                            OnSortCommand="OnRadActionGridSortCommand" OnPageSizeChanged="OnActionGrid_PageSizeChanged"
                            OnNeedDataSource="OnGridActionNeedDataSource" EnableAjaxSkinRendering="true">
                            <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                            <MasterTableView DataKeyNames="ActionId" DataMember="Action" AllowMultiColumnSorting="True"
                                Width="100%">
                                <Columns>
                                    <telerik:GridBoundColumn UniqueName="Active" SortExpression="Actif" HeaderText="Active"
                                        DataField="Actif">
                                        <HeaderStyle Width="5%"></HeaderStyle>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="TaskNbr" SortExpression="ActionId" HeaderText="Task nbr" Visible="false"
                                        DataField="ActionId">
                                        <%--<HeaderStyle Width="8%"></HeaderStyle>--%>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="DateAction" SortExpression="DateAction" HeaderText="Date"
                                        DataField="DateAction" DataType="system.datetime" DataFormatString="{0:dd/MM/yyyy}">
                                        <%--<HeaderStyle Width="8%"></HeaderStyle>--%>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="Hour" SortExpression="Hour" HeaderText="Hour" Visible="false" Display="false"
                                        DataField="Hour" DataType="system.datetime" DataFormatString="{0:dd/MM/yyyy hh:mm tt}">
                                        <%--<HeaderStyle Width="9%"></HeaderStyle>--%>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="TypeAction" SortExpression="TypeActionLabel"
                                        HeaderText="Type" DataField="TypeActionLabel">
                                        <%--<HeaderStyle Width="5%"></HeaderStyle>--%>
                                    </telerik:GridBoundColumn>
                                     <telerik:GridTemplateColumn UniqueName="TemplateCandidateColumn" SortExpression="CandidateFullName"
                                        HeaderText="Candidate">
                                        <FooterTemplate>Template footer</FooterTemplate>
                                        <FooterStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkCandidateName" runat="server" Text='<%# Eval("CandidateFullName") %>'
                                                CommandArgument='<%# Eval("CandidateID") %>' CommandName="test" OnClick="OnCandidateClicked">
                                            </asp:LinkButton>&nbsp;
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <%--<HeaderStyle Width="9%"></HeaderStyle>--%>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="TemplateCompanyColumn" SortExpression="CompanyName"
                                        HeaderText="Company">
                                        <FooterTemplate>Template footer</FooterTemplate>
                                        <FooterStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkCompanyName" runat="server" Text='<%# Eval("CompanyName") %>'
                                                CommandArgument='<%# Eval("CompanyID") %>' OnClick="OnCompanyClicked">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <%--<HeaderStyle Width="15%"></HeaderStyle>--%>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn UniqueName="Descripton" HeaderText="Descripton" AllowSorting="false"
                                        DataField="DescrAction">
                                        <%--<HeaderStyle Width="20%"></HeaderStyle>--%>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="ResponsableName" SortExpression="ResponsableName"
                                        HeaderText="Responsible" DataField="ResponsableName">
                                        <%--<HeaderStyle Width="8%"></HeaderStyle>--%>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn UniqueName="TemplateEditActionColumn">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lnkActionEdit" runat="server" Text="Edit" 
                                            NavigateUrl='<%# Eval("ActionId","ActionDetails.aspx?ActionID={0}&type=action&mode=edit&backurl=visible") %>'>
                                            </asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle Width="4%"></HeaderStyle>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="TemplateDeleteActionColumn">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkActionDelete" runat="server" Text="Delete" CommandArgument='<%# Eval("ActionId") %>'
                                                OnClientClick="return confirm('Are you sure to delete this item?')" OnClick="OnActionDeleteClicked">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle Width="4%" HorizontalAlign="Center"></HeaderStyle>
                                    </telerik:GridTemplateColumn>  
                                    <telerik:GridTemplateColumn UniqueName="TemplateExportActionColumn">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkExportAction" runat="server" Text="Export" CommandArgument='<%# Eval("ActionId") %>'
                                                OnClientClick="return confirm('Are you sure to export this action to Outlook ?')" OnClick="OnActionExportClicked">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Width="5%"></HeaderStyle>
                                    </telerik:GridTemplateColumn>                                  
                                </Columns>
                            </MasterTableView>
                            <ClientSettings >
                                <Selecting AllowRowSelect="true" />
                                <ClientEvents OnRowSelected="onActionGrid_RowSelected" OnRowDblClick="onActionGrid_RowDblClick"
                                 OnGridDestroying="onActionGrid_Detroying"/>
                            </ClientSettings>
                        </telerik:RadGrid>
                        <telerik:RadWindow runat="server" ID="radWindowAction" Skin="Office2007" VisibleOnPageLoad="false" VisibleStatusbar="false"
                            Modal="true" OffsetElementID="offsetElement" Top="30" Left="30" NavigateUrl="ActionDetails.aspx"
                            Title="Action" Height="500px" Width="850px" OnClientClose="onClientActionDetailWindowClosed">
                        </telerik:RadWindow>                        
                    </td>
                </tr>
                <tr align="center">
                    <td>
                        <asp:HyperLink ID="lnkAddNewAction" runat="server" Text="Add new action" NavigateUrl="~/ActionDetails.aspx?type=action&mode=edit&backurl=visible"></asp:HyperLink>
                    </td>                    
                </tr>                
            </table>
        </div>
    </form>
</body>
</html>
