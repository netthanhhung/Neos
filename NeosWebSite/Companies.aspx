<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Companies.aspx.cs" Inherits="Companies" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Company</title>

    <script src="script/utils.js" type="text/javascript"></script>
    <link href="Styles/Neos.css" rel="Stylesheet" />
    
    <telerik:RadScriptBlock runat="server" ID="scriptBlock">
        <script type="text/javascript">
            var isDoubleClick = false; 
            var clickHandler = null; 
            
            function companyContactRowSelected(sender, args)
            {
               var tableView = args.get_tableView(); 
               if(tableView.get_selectedItems().length == 1)
               {
                    var contactID = tableView.getCellByColumnUniqueName(tableView.get_selectedItems()[0], "ContactID").innerHTML;
                    var dataItem = $get(args.get_id());        
                    $find("CompanyAjaxManager").ajaxRequest("RebindContactInfoGrid/" + contactID + "/" + dataItem.rowIndex);
               }
               else
               {
                    
               }
            }
            
            function onCompanyGrid_RowSelected(sender, args)
            {
                isDoubleClick = false;
                if (clickHandler) 
                {
                    window.clearTimeout(clickHandler);
                    clickHandler = null;
                }
                clickHandler = window.setTimeout(function () { companyRowSelected(sender, args); }, 200);
                return false;
            }
            
            function companyRowSelected(sender, args)
            {
               var tableView = args.get_tableView(); 
               if(tableView.get_selectedItems().length == 1)
               {
                    var companyID = tableView.getCellByColumnUniqueName(tableView.get_selectedItems()[0], "CompanyID").innerHTML;
                    var dataItem = $get(args.get_id());        
                    $find("CompanyAjaxManager").ajaxRequest("RebindContactGrid/" + companyID + "/" + dataItem.rowIndex);
                    //update toolbar            
                    processCompanyToolBar("CompanyGridSelected");
               }
               else
               {
                    
               }
            }           
            
            
            function onCompanyGrid_Detroying(sender, Args)
            {
                processCompanyToolBar("CompanyGridDeSelected");
            } 
            function onAddContactClientClick(msg)
            {
                var grid = $find("<%=CompanyGrid.ClientID %>");
                var masterTable = grid.get_masterTableView();
                if(masterTable.get_selectedItems().length == 1)
                {
                    var companyID = masterTable.getCellByColumnUniqueName(masterTable.get_selectedItems()[0],"CompanyID").innerHTML;
                    $find("CompanyAjaxManager").ajaxRequest("AddCompanyContact/" + companyID);
                    
                }
                else
                {
                    alert(msg);
                } 
               
                return false;
            }
            function onAddContactInfoClientClick(msg)
            {
                var grid = $find("<%=CompanyContactGrid.ClientID %>");
                var masterTable = grid.get_masterTableView();
                if(masterTable != null)
                {
                    if(masterTable.get_selectedItems().length == 1)
                    {
                        var contactID = masterTable.getCellByColumnUniqueName(masterTable.get_selectedItems()[0],"ContactID").innerHTML;
                        $find("CompanyAjaxManager").ajaxRequest("AddContactInfo/" + contactID);
                    }
                    else
                    {
                        alert(msg);
                    }
                }
                else
                {
                    alert(msg);
                }                
                return false;
            }
            
            function OnDropDownContactInfo_ClientChanging(sender, eventArgs)
            {
                eventArgs.set_cancel(true);
            }
            function onCompanyGrid_ClientRowDblClick(sender, eventArgs)
            {
                isDoubleClick = true;
                if (clickHandler)
                { 
                    window.clearTimeout(clickHandler);
                    clickHandler = null;
                }
                clickHandler = window.setTimeout( function () { companyGridRowDblClick(sender, eventArgs); } , 200); 
                return false;
            }
            
            function companyGridRowDblClick(sender, eventArgs)
            {
                var grid = $find("<%= CompanyGrid.ClientID %>");
                var masterTable = grid.get_masterTableView();
                if(masterTable)
                {
                     var row = masterTable.get_selectedItems()[0];
                     var companyID = masterTable.getCellByColumnUniqueName(row, "CompanyID").innerHTML;
                     
                     var ajaxObj = $find('<%= CompanyAjaxManager.ClientID %>');
                     if(!ajaxObj) return;
                        ajaxObj.ajaxRequest("OpenSeletectedCompany");
                     
                 }
            }
            </script>
    </telerik:RadScriptBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="ScriptManager" runat="server" />
        <div class="rightpane_title"><asp:Literal runat="server" ID="lblCompanyTitle" Text="Companies"></asp:Literal></div>        
        <div style="margin-top:30px;">
            <telerik:RadAjaxManager  EnableAJAX="true" ID="CompanyAjaxManager" runat="server" OnAjaxRequest="OnMyAjaxManagerAjaxRequest">
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="CompanyGrid">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="CompanyGrid" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="CompanyContactGrid">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="CompanyContactGrid" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>                    
                </AjaxSettings>                
            </telerik:RadAjaxManager>
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" runat="server" Height="75px"
                Width="75px" Transparency="50">
                <img alt="Loading..." src='<%= RadAjaxLoadingPanel.GetWebResourceUrl(Page, "Telerik.Web.UI.Skins.Default.Ajax.loading.gif") %>'
                    style="border: 0;" />
            </telerik:RadAjaxLoadingPanel>
            
            <div>
                <telerik:RadGrid ID="CompanyGrid" GridLines="None" Skin="Office2007" AllowMultiRowSelection="False"
                    EnableAjaxSkinRendering="true" runat="server" AllowPaging="True" AllowSorting="True" AllowCustomPaging="True"
                    PageSize="10" Width="100%" AutoGenerateColumns="false" OnPageSizeChanged="OnCompanyGrid_PageSizeChanged"
                    OnNeedDataSource="OnCompanyGridNeedDataSource" OnItemDataBound="OnCompanyGridItemDataBound" 
                    OnPageIndexChanged="OnCompanyGridPageIndexChanged" OnSortCommand="OnCompanyGridSortCommand"
                    >
                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                    <MasterTableView DataKeyNames="CompanyID" DataMember="Company" AllowMultiColumnSorting="True"
                        Width="100%" EditMode="PopUp">
                        <%--<SortExpressions>
                            <telerik:GridSortExpression FieldName="CompanyName" SortOrder="Ascending" />
                        </SortExpressions>--%>
                        <Columns>
                            <telerik:GridBoundColumn UniqueName="CompanyID" DataField="CompanyID" Display="false">
                                <HeaderStyle></HeaderStyle>
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn UniqueName="CompanyName" SortExpression="CompanyName"
                                HeaderText="Name">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkCompanyName" runat="server" Text='<%# Eval("CompanyName") %>'
                                        NavigateUrl='<%# Eval("CompanyID","~/CompanyProfile.aspx?CompanyId={0}&mode=edit&backurl=visible") %>'>                                
                                    </asp:HyperLink>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <HeaderStyle></HeaderStyle>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn UniqueName="City" SortExpression="City" HeaderText="City"
                                DataField="City">
                                <HeaderStyle></HeaderStyle>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="StatusLabel" SortExpression="StatusLabel" HeaderText="StatusLabel"
                                DataField="StatusLabel">
                                <HeaderStyle></HeaderStyle>
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn UniqueName="ContactInfo" HeaderText="Contact Info">
                                <ItemTemplate>
                                    <telerik:RadComboBox runat="server" ID="ddlContactInfo" Skin="Office2007" Width="99%"
                                        BackColor="White" OnClientSelectedIndexChanging="OnDropDownContactInfo_ClientChanging">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="lblContactInfo"></asp:Literal>
                                        </ItemTemplate>
                                    </telerik:RadComboBox>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="center" />
                                <HeaderStyle></HeaderStyle>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn UniqueName="Responsible" HeaderText="Néos resp." DataField="ResponsibleLastName">
                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" />
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn UniqueName="Job" HeaderText="Job">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="lblJobCount"></asp:Literal>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="center" />
                                <HeaderStyle HorizontalAlign="center"></HeaderStyle>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="CreatedDate" HeaderText="Created Date" SortExpression="CreatedDate">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="lblCreatedDate"></asp:Literal>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <HeaderStyle></HeaderStyle>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings>
                        <Selecting AllowRowSelect="true" />
                        <ClientEvents OnRowSelected="onCompanyGrid_RowSelected" OnRowDblClick="onCompanyGrid_ClientRowDblClick"
                                        OnGridDestroying="onCompanyGrid_Detroying"/><%-- --%>
                    </ClientSettings>
                </telerik:RadGrid>
            </div>
            <div style="text-align: center;">
                <asp:LinkButton runat="server" ID="lnkAddCompany" OnClick="OnAddNewCompanyClick"></asp:LinkButton></div>
            <div style="margin-top: 10px; float: left; width: 100%">
                <div style="float: left; width: 49%;"  runat="server" id="divContact">
                    <asp:Literal runat="server" ID="lblContacts" Text=""></asp:Literal>
                    <div style=" height: 250px; overflow: -moz-scrollbars-vertical; overflow-y: scroll; overflow-x: hidden; text-align:left;">
                        <telerik:RadGrid ID="CompanyContactGrid" GridLines="None" Skin="Office2007" AllowMultiRowSelection="False"
                            runat="server" AllowPaging="False" AllowSorting="False" PageSize="10" HorizontalAlign="Right"
                            AutoGenerateColumns="false" OnNeedDataSource="OnCompanyContactGridNeedDataSource">
                            <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                            <MasterTableView DataKeyNames="ContactID" DataMember="CompanyContact" AllowMultiColumnSorting="False"
                                Width="100%" EditMode="PopUp">
                                <SortExpressions>
                                    <telerik:GridSortExpression FieldName="LastName" SortOrder="ascending" />
                                </SortExpressions>
                                <Columns>
                                    <telerik:GridBoundColumn UniqueName="ContactID" DataField="ContactID" Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="LastName" SortExpression="LastName" HeaderText=""
                                        DataField="LastName">
                                        <HeaderStyle Width="30%"></HeaderStyle>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="FirstName" SortExpression="FirstName" HeaderText=""
                                        DataField="FirstName">
                                        <HeaderStyle  Width="30%"></HeaderStyle>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="Position" SortExpression="Position" HeaderText=""
                                        DataField="Position">
                                        <HeaderStyle  Width="40%"></HeaderStyle>
                                    </telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings>
                                <Selecting AllowRowSelect="True" />
                                <ClientEvents OnRowSelected="companyContactRowSelected" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </div>
                    <div>
                    <asp:Panel runat="server" ID="pnlAddContact" DefaultButton="btnAddContact">                    
                        <telerik:RadTextBox runat="server" ID="txtLastName" Width="120" MaxLength="60"></telerik:RadTextBox>
                        <telerik:RadTextBox runat="server" ID="txtFirstName" Width="120" MaxLength="60"></telerik:RadTextBox>
                        <telerik:RadComboBox runat="server" ID="ddlFunction" Width="140" Height="200" Skin="Office2007" AllowCustomText="true"  ShowWhileLoading="true"
                        EnableLoadOnDemand="true" OnItemsRequested="OnDropdownFunctionItemRequested">
                        </telerik:RadComboBox>
                        <asp:Button runat="server" ID="btnAddContact" Text="Add" CssClass="flatButton" />
                     </asp:Panel>
                    </div>
                </div>
                <div style="float: left; width: 49%; margin-left:2%;" runat="server" id="divContactInfo">
                    <asp:Literal runat="server" ID="lblContactInfo" Text=""></asp:Literal>
                    <div style="height: 250px; overflow: -moz-scrollbars-vertical; overflow-y: scroll;  overflow-x: hidden;text-align:left">
                        <telerik:RadGrid ID="ContactInfoGrid" GridLines="None" Skin="Office2007" AllowMultiRowSelection="False"
                            runat="server" AllowPaging="False" AllowSorting="False" AutoGenerateColumns="false" HorizontalAlign="Right"
                            OnNeedDataSource="OnContactInfoGridNeedDataSource" OnItemDataBound="OnContactInfoGridItemDataBound">
                            <MasterTableView DataKeyNames="ContactID" DataMember="CompanyContact" AllowMultiColumnSorting="True"
                                EditMode="PopUp">
                                <Columns>
                                    <telerik:GridBoundColumn UniqueName="Type" HeaderText="" DataField="Type">
                                        <HeaderStyle Width="10%"></HeaderStyle>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="TelephoneZone" HeaderText="" DataField="TelephoneZone">
                                        <HeaderStyle Width="15%"></HeaderStyle>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="Tel" HeaderText="" DataField="Tel">
                                        <HeaderStyle Width="35%"></HeaderStyle>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="Location" HeaderText="" DataField="Location">
                                        <HeaderStyle Width="20%"></HeaderStyle>
                                    </telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>
                    <div>
                    <asp:Panel runat="server" ID="pnlAddContactInfo" DefaultButton="btnAddContactInfo">
                        <telerik:RadComboBox runat="server" ID="ddlType" Width="70" Skin="Office2007"></telerik:RadComboBox>&nbsp;
                        <telerik:RadTextBox runat="server" ID="txtPhoneZone" Width="60" MaxLength="6"></telerik:RadTextBox>
                        <telerik:RadTextBox runat="server" ID="txtInfo"  Width="110" MaxLength="60"></telerik:RadTextBox>
                        <telerik:RadTextBox runat="server" ID="txtPlace"  Width="110" MaxLength="20"></telerik:RadTextBox>
                        <asp:Button runat="server" ID="btnAddContactInfo" Text="Add" CssClass="flatButton"/>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
