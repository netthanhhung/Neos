<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Jobs.aspx.cs" Inherits="Jobs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Jobs</title>
    <link href="Styles/Neos.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="script/utils.js"></script>
    
</head>
<body>
<script type="text/javascript">
    function onJobGrid_RowSelected(sender, args)
    {    
       var tableView = args.get_tableView(); 
       if(tableView.get_selectedItems().length == 1)
       {
            //update action toolbar
            processJobToolBar("JobGridSelected");
       }
       if(tableView.get_selectedItems().length >= 1) 
       {
            if(document.getElementById('<%=divBatchUpdate.ClientID %>') != null)
                        document.getElementById('<%=divBatchUpdate.ClientID %>').style.display = "inline-block";
       } 
       else 
       {
            if(document.getElementById('<%=divBatchUpdate.ClientID %>') != null)
                        document.getElementById('<%=divBatchUpdate.ClientID %>').style.display = "none";
       }
       return false;
    }
    function onJobGrid_Detroying(sender, Args)
    {
        processJobToolBar("JobGridDeSelected");
    }
    
    function onJobGrid_RowDblClick(sender, eventAgrs)
    {        
        var grid = $find("<%= gridJobs.ClientID %>");
        var masterTable = grid.get_masterTableView();
        if(masterTable)
        {
             var ajaxObj = $find('<%=JobAjaxManager.ClientID %>');
             if(!ajaxObj) return;
                ajaxObj.ajaxRequest("OpenSelectedJob");
         }
    }
    
</script>
    <form id="form1" runat="server">
         <div class="rightpane_title"><asp:Literal runat="server" ID="lblJobTitle" Text="Jobs"></asp:Literal></div>        
        <div style="margin-top:30px;">
            <telerik:RadScriptManager ID="ScriptManager" runat="server">
            </telerik:RadScriptManager>
            <telerik:RadAjaxManager EnableAJAX="true" runat="server" ID="JobAjaxManager" OnAjaxRequest="JobAjaxManager_AjaxRequest">
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="JobAjaxManager">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl  ControlID="gridJobs" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
            <telerik:RadGrid ID="gridJobs" GridLines="None" Skin="Office2007" AllowMultiRowSelection="True"
                runat="server" AllowPaging="True" AllowSorting="true" PageSize="22" Width="100%"
                EnableAJAX="True" AutoGenerateColumns="false" EnableAjaxSkinRendering="true"
                OnDeleteCommand="OnGridJobs_DeleteCommand" OnNeedDataSource="OnGridJobs_NeedDataSource" 
                OnPageIndexChanged="OnGridJobs_PageIndexChanged" OnItemDataBound="OnGridJobs_ItemDataBound"
                OnPageSizeChanged="OnJobGrid_PageSizeChanged"
                >
                <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                <MasterTableView DataKeyNames="JobID" DataMember="Job" AllowMultiColumnSorting="False"
                    Width="100%">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="CreatedDate" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>             
                         <telerik:GridBoundColumn UniqueName="JobID" DataField="JobID" Display="false">
                            <HeaderStyle></HeaderStyle>
                        </telerik:GridBoundColumn>                            
                        <telerik:GridTemplateColumn UniqueName="Title" HeaderText="Title" SortExpression="Title">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="lnkJobTitle" Text='<%#Eval("Title") %>' 
                                    NavigateUrl='<%#Eval("JobID","~/JobProfile.aspx?JobId={0}&mode=edit&backurl=visible") %>'></asp:HyperLink>
                                    &nbsp;
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Left" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn UniqueName="CompanyName" DataField="CompanyName" SortExpression="CompanyName" HeaderText="CompanyName">                            
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </telerik:GridBoundColumn>
                        
                        <telerik:GridBoundColumn UniqueName="Location" DataField="Location" SortExpression="Location" HeaderText="Location">                            
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </telerik:GridBoundColumn>
                        
                        <telerik:GridBoundColumn UniqueName="NrOfVisites" DataField="NrOfVisites" SortExpression="NrOfVisites" HeaderText="Visits">                            
                            <HeaderStyle HorizontalAlign="center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        
                       <telerik:GridBoundColumn UniqueName="CreatedDate" DataField="CreatedDate" SortExpression="CreatedDate" HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy}">                            
                            <HeaderStyle HorizontalAlign="center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn UniqueName="ExpiredDate" DataField="ExpiredDate" SortExpression="ExpiredDate" HeaderText="Expired Date" DataFormatString="{0:dd/MM/yyyy}">                            
                            <HeaderStyle HorizontalAlign="center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>                        
                        <telerik:GridButtonColumn CommandName="Delete" CommandArgument="JobID" ButtonType="ImageButton" UniqueName="DeleteColumn">
                            <HeaderStyle Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>                        
                        <telerik:GridBoundColumn UniqueName="JobID" DataField="JobID" Display="false">
                        </telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings>
                    <Selecting AllowRowSelect="True"  />
                    <ClientEvents OnRowSelected="onJobGrid_RowSelected" OnGridDestroying="onJobGrid_Detroying" OnRowDblClick="onJobGrid_RowDblClick"/>
                </ClientSettings>
            </telerik:RadGrid>           
        </div>
        <div style="text-align: center">
        <%--<asp:LinkButton runat="server" ID="lnkAddJob" Text="Add New Job" OnClientClick="return OnAddNewJobClick();"></asp:LinkButton>--%>
        <asp:HyperLink runat="server" ID="lnkAddJob" Text="Add New Job" NavigateUrl="~/JobProfile.aspx?mode=edit&backurl=visible"></asp:HyperLink>
        </div>
        <div id="divBatchUpdate" runat="server" style="margin-top:30px; display: none;" >
            <table>
                <tr>
                    <td>
                        <asp:Literal runat="server" ID="lblExpiredDate" Text="Expired Date"></asp:Literal>
                    </td>
                    <td>
                        <telerik:RadDatePicker ID="calExpiredDate" runat="server" MinDate="0001-01-01" Skin="Office2007"
                            Width="122"  Calendar-CultureInfo="en-US">
                            <DateInput ID="inputExpiredDate" runat="server" Skin="Office2007" DateFormat="dd/MM/yyyy"
                                DisplayDateFormat="dd/MM/yyyy">
                            </DateInput>
                        </telerik:RadDatePicker>
                    </td>
                     <td>
                        <asp:Literal runat="server" ID="lblRemindDate" Text="Remind Date"></asp:Literal></td>
                    <td>
                        <telerik:RadDatePicker ID="calRemindDate" runat="server" MinDate="0001-01-01" Skin="Office2007"
                            Width="122"  Calendar-CultureInfo="en-US">
                            <DateInput ID="inputRemindDate" runat="server" Skin="Office2007" DateFormat="dd/MM/yyyy"
                                DisplayDateFormat="dd/MM/yyyy">
                            </DateInput>
                        </telerik:RadDatePicker>
                    </td>
                    <td>
                        <asp:Button ID="btnBatchUpdate" runat="server" Text="Batch update" Width="100px" OnClick="OnBtnBatchUpdateClicked" />
                    </td>
                     <td>
                        <asp:Button ID="btnActivate" runat="server" Text="Activate" Width="100px" OnClick="OnBtnBatchActivateClicked" />
                    </td>
                     <td>
                        <asp:Button ID="btnDeactivate" runat="server" Text="Deactivate" Width="100px" OnClick="OnBtnBatchDeactivateClicked" />
                    </td>
                </tr>
            </table>
            
        </div>
    </form>
</body>
</html>
