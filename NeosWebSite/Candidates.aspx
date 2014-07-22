<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Candidates.aspx.cs" Inherits="Candidates" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Candidates Page</title>
    <link href="Styles/Neos.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    .absoluteposition
    {
        position: absolute;
    }
    </style>
    <script type="text/javascript" src="script/utils.js"></script>
    <telerik:RadScriptBlock runat="server" ID="scriptBlock">
     <script type="text/javascript">
        function candidategrid_RowSeleting(sender, Args)
        {
            var grid = $find('<%=RadCandidateGrid.ClientID %>');           
            if(grid.get_selectedItems().length >0)
            {
                processCandidateToolBar("CandidateGridSelected");
            }
            else
            {
                processCandidateToolBar("CandidateGridDeSelected");
            }
            
        }
        function onCandidateGrid_Detroying(sender, Args)
        {
           processCandidateToolBar("CandidateGridDeSelected");
        }
        
        function OnDropDownContactInfo_ClientChanging(sender, eventArgs)
        {
            eventArgs.set_cancel(true);
        }
        
        function onCandidateGrid_ClientRowDblClick(sender, eventArgs)
        {
            var grid = $find("<%= RadCandidateGrid.ClientID %>");
            var masterTable = grid.get_masterTableView();
            if(masterTable)
            {
                //CandidateId
                 var row = masterTable.get_selectedItems()[0];
                 var candidateId = masterTable.getCellByColumnUniqueName(row, "CandidateId").innerHTML;
                 
                 var ajaxObj = $find('<%=CandidateAjaxManager.ClientID %>');
                 if(!ajaxObj) return;
                    ajaxObj.ajaxRequest("OpenSelectedCandidate");
             }
        }
     </script>
    </telerik:RadScriptBlock>
</head>
<body>
    <form id="candidatesForm" runat="server">
        <telerik:RadScriptManager ID="ScriptManager" runat="server" />
        <div class="rightpane_title"><asp:Literal runat="server" ID="lblCandidateTitle" Text="Candidates"></asp:Literal></div>        
        <div style="margin-top:30px;">
            <telerik:RadAjaxManager  EnableAJAX="true" ID="CandidateAjaxManager" runat="server" OnAjaxRequest="OnMyAjaxManager_AjaxRequest">
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="RadCandidateGrid">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadCandidateGrid" LoadingPanelID="pnlRadAjaxLoading" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
            <telerik:RadAjaxLoadingPanel ID="pnlRadAjaxLoading" runat="server" Height="75px"
                Width="75px" Transparency="50">
                <img alt="Loading..." src='<%= RadAjaxLoadingPanel.GetWebResourceUrl(Page, "Telerik.Web.UI.Skins.Default.Ajax.loading.gif") %>'
                    style="border: 0;" />
            </telerik:RadAjaxLoadingPanel>
            <table style="width: 100%">
                <tr>
                    <td colspan="2">                       
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadGrid ID="RadCandidateGrid" GridLines="None" Skin="Office2007" AllowMultiRowSelection="False"
                            runat="server" AllowPaging="True" AllowSorting="True" AllowCustomPaging="True"
                            AutoGenerateColumns="false" EnableAjaxSkinRendering="true" PageSize="22" Width="100%"
                            OnItemDataBound="OnRadCandidateGridItemDataBound" 
                            OnNeedDataSource="OnRadCandidateGridNeedDataSource" OnPageSizeChanged="OnRadCandidateGrid_PageSizeChanged" 
                            OnPageIndexChanged="OnRadCandidateGridPageIndexChangeds" OnSortCommand="OnRadCandidateGridSortCommand"                              
                            >
                            <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                            <MasterTableView DataKeyNames="CandidateId" DataMember="Candidate" AllowMultiColumnSorting="True"
                                Width="100%" EditMode="PopUp">
                                <Columns>
                                    <telerik:GridTemplateColumn UniqueName="TemplateLastNameColumn" SortExpression="LastName"
                                        HeaderText="Last Name">
                                        <FooterTemplate>
                                            Template footer</FooterTemplate>
                                        <FooterStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkLastName" runat="server" Text='<%# Eval("LastName") %>' CommandArgument='<%# Eval("CandidateID") %>'
                                                OnClick="OnCandidateClicked">                                
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Width="50px"></HeaderStyle>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="TemplateFirstNameColumn" SortExpression="FirstName"
                                        HeaderText="First Name">
                                        <FooterTemplate>
                                            Template footer</FooterTemplate>
                                        <FooterStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkFirstName" runat="server" Text='<%# Eval("FirstName") %>'
                                                CommandArgument='<%# Eval("CandidateID") %>' OnClick="OnCandidateClicked">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Width="50px"></HeaderStyle>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn UniqueName="Status" SortExpression="Inactive" HeaderText="Status"
                                        DataField="InactiveString">
                                        <HeaderStyle Width="35px"></HeaderStyle>
                                    </telerik:GridBoundColumn>                                  
                                    <telerik:GridTemplateColumn UniqueName="ContactInfo"
                                        HeaderText="Contact info">
                                        <ItemTemplate>                                           
                                            <telerik:RadComboBox runat="server" ID="ddlContactInfo" Width="100%" Skin="Office2007" OnClientSelectedIndexChanging="OnDropDownContactInfo_ClientChanging">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="lblContactInfo"></asp:Literal>
                                                </ItemTemplate>
                                            </telerik:RadComboBox>
                                        </ItemTemplate>
                                        <HeaderStyle Width="100px"></HeaderStyle>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn UniqueName="LastModifDate" SortExpression="LastModifDate"
                                        HeaderText="Last Modif." DataField="LastModifDate" DataType="system.DateTime"
                                        DataFormatString="{0:dd/MM/yyyy}">
                                        <HeaderStyle Width="35px"></HeaderStyle>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="CandidateId"  DataField="CandidateId" Display="false">
                                    </telerik:GridBoundColumn>   
                                </Columns>
                            </MasterTableView>
                            <ClientSettings >
                                <Selecting AllowRowSelect="true"/>
                                <ClientEvents OnRowSelected="candidategrid_RowSeleting" OnRowDblClick="onCandidateGrid_ClientRowDblClick"  
                                                OnGridDestroying="onCandidateGrid_Detroying"/>
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                </tr>
               
                <tr align="center">
                    <td>
                        <asp:LinkButton ID="lnkAddNewCandidate" runat="server" Text="" OnClick="OnAddNewCandidateClicked"></asp:LinkButton>
                        
                    </td>
                    <td align="right">
                        <asp:LinkButton ID="lnkBackToAdvancedSearch" runat="server" Text="Back to advanced search" 
                        OnClick="OnBackToAdvancedSearchClicked" Visible="false"></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
