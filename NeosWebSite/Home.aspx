<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Home" %>

<%@ Register TagPrefix="telerik" Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head id="NeosMainHead" runat="server">
    <title>Neos main Page</title>
    <link href="Styles/Neos.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="script/utils.js"></script>

    <style type="text/css">
        html,body { overflow: hidden; }
        html>body { overflow: visible; }
        html { height: 100%; }
    </style>
    <telerik:RadScriptBlock runat="server" ID="scriptBlog">
        <script type="text/javascript">
        //<!--
            function SetSessionKeepAliveTimer() 
            {
                window.setInterval('sessionKeepAliveNoUrl()', 30000);
            }
            function OnClientItemClicking(sender, eventArgs)
            {
                var multipage=$find('<%=RadMultiPage1.ClientID %>');        
                multipage.set_selectedIndex(eventArgs.get_item().get_index());
                
                var splitter = $find('<%=RadSplitterBrowser.ClientID%>');
                var pane = splitter.GetPaneById('<%=radPaneContent.ClientID %>');
                if (!pane) return;
                
                var toolbar = $find('<%=ToolBarMView.ClientID %>');
                if(toolbar)
                    $find('<%=ToolBarMView.ClientID %>').set_selectedIndex(5);
                
                switch(eventArgs.get_item().get_index())
                {
                    case 0:
                        pane.set_contentUrl("Candidates.aspx");
                        if(toolbar)
                            $find('<%=ToolBarMView.ClientID %>').set_selectedIndex(0);
                     break;
                   case 1:
                        pane.set_contentUrl("Companies.aspx");
                        if(toolbar)
                            $find('<%=ToolBarMView.ClientID %>').set_selectedIndex(1);
                     break;
                   case 2:
                        pane.set_contentUrl("Actions.aspx");
                        if(toolbar)
                            $find('<%=ToolBarMView.ClientID %>').set_selectedIndex(2);
                     break;
                   case 3:
                        pane.set_contentUrl("Jobs.aspx");
                        if(toolbar)
                            $find('<%=ToolBarMView.ClientID %>').set_selectedIndex(3);
                     break;
                  case 4:
                     pane.set_contentUrl("StatisticsGeneral.aspx");
                     break;
                   case 5:
                        pane.set_contentUrl("Administration/AdminUserAndPermission.aspx");
                     break;
                     case 6:
                        pane.set_contentUrl("InvoicesPage.aspx");
                        if(toolbar)
                            $find('<%=ToolBarMView.ClientID %>').set_selectedIndex(4);
                     break;
                     case 7:
                        pane.set_contentUrl("Messages.aspx?type=unread");
                     break;
                }
                return false;
            }
            function OnCandidateSearchClicked()
            {
                
                var searchCriteria = document.getElementById('<%=txtLastNameSearch.ClientID %>').value;
                
                if(searchCriteria == "") return false;
                var splitter = $find('<%=RadSplitterBrowser.ClientID%>');
                var pane = splitter.GetPaneById('<%=radPaneContent.ClientID %>');
                if (!pane) return;
                    pane.set_contentUrl("Candidates.aspx?lastname=" + searchCriteria);
                  return false;
            }
            
            function OnCandidateCVSearchClicked()
            {
                var searchCriteria = document.getElementById('<%=txtCVSearchKeyWork.ClientID %>').value;
                if(searchCriteria == "") return false;
                var splitter = $find('<%=RadSplitterBrowser.ClientID%>');
                var pane = splitter.GetPaneById('<%=radPaneContent.ClientID %>');
                if (!pane) return;
                    pane.set_contentUrl("CandidateCVSearchResult.aspx?keyword=" + searchCriteria);
                  return false;
            }
            
            function OnCompanySearchClicked()
            {                                
                var cname = document.getElementById('<%=txtCompanySearch.ClientID %>').value;
                var ctype = getRadioButtonValue('<%=rdoListCompanyType.ClientID %>');
                
                var splitter = $find('<%=RadSplitterBrowser.ClientID%>');
                var pane = splitter.GetPaneById('<%=radPaneContent.ClientID %>');
                if (!pane) return;
                    pane.set_contentUrl("Companies.aspx?cname=" + cname + "&ctype=" + ctype);
                  return false;
            }  
            
            function getRadioButtonValue(id)
            {
                var radio = document.getElementsByName(id);
                for (var ii = 0; ii < radio.length; ii++)
                {
                    if (radio[ii].checked)
                        return radio[ii].value;
                }
            }
  
            function onFindJobs(msg)
            {
                var splitter = $find('<%=RadSplitterBrowser.ClientID%>');
                var pane = splitter.GetPaneById('<%=radPaneContent.ClientID %>');
                if (!pane) return;
                
                switch(msg)
                {
                    case "active":
                        pane.set_contentUrl("Jobs.aspx?mode=active");     
                     break;
                   case "inactive":
                        pane.set_contentUrl("Jobs.aspx?mode=inactive");
                     break;
                   case "all":
                        pane.set_contentUrl("Jobs.aspx?mode=all");
                     break;
                }
                return false;
            }    

            function onViewMessage_Click()
            {
                var splitter = $find('<%=RadSplitterBrowser.ClientID%>');
                var pane = splitter.GetPaneById('<%=radPaneContent.ClientID %>');
                if (!pane) return;
                
                pane.set_contentUrl("Messages.aspx");
                return false;
            }
           
            
            function OnButtonChooseCompanyClientClicked()
            {    
                var radWindow = $find('radWinInvoiceCompanyChoose');
                var url = "ChooseCompanyPopup.aspx?";
                radWindow.setUrl(url);
                radWindow.show();
                
                return false;
            }
                
            function OnButtonDeleteCompanyClientClicked() 
            {
                 var hiddenCompanyId = document.getElementById('hiddenCompanyId');
                 hiddenCompanyId.value = "";
                 
                 /*var txtInvoiceCompanyName = $find('txtInvoiceCompanyName');
                 txtInvoiceCompanyName.set_value("");*/ 
                 return false;
            }
            
            function onClientChooseCompanyWindowClosed(window)
            {            
                if (window.argument != undefined && window.argument != null && window.argument != "")
                {
                    var argument = window.argument;
                    var argument_array = argument.split("/");                
                    var hiddenCompanyId = document.getElementById('hiddenCompanyId');
                    hiddenCompanyId.value = argument;
                    
                    var ddlCompany = $find('ddlCompany');
                    ddlCompany.clearItems();                
                    ddlCompany.trackChanges();
                    
                    var companyItem = new Telerik.Web.UI.RadComboBoxItem();
                    companyItem.set_text(argument_array[1]);
                    companyItem.set_value(argument_array[0]);
                    ddlCompany.get_items().add(companyItem);
                    companyItem.select();
                    ddlCompany.commitChanges();
                    /*var txtInvoiceCompanyName = $find('txtInvoiceCompanyName');
                    txtInvoiceCompanyName.set_value(argument_array[1]);*/                                                        
                }
            }
            
            function onDropdownCompany_ClientItemsRequesting(sender, eventArgs)
            {
                var combo = $find("<%= ddlCompany.ClientID %>");
                var text = combo.get_text();
                if(text.length > 0)
                {                
                    eventArgs.set_cancel(false);
                }
                else
                {
                    eventArgs.set_cancel(true);
                }
            }
            
            //-->    
            </script>    
    </telerik:RadScriptBlock>
</head>
<body style="margin: 0px 0px 0px 0px; border: none; min-width:800px; min-height:600px" onresize="updateMapSize()">
    <form id="mainForm" method="post" runat="server">
        <div id="wrapper" style=" ">
            <telerik:RadScriptManager ID="ScriptManager" runat="server" />
            <telerik:RadAjaxManager EnableAJAX="true" runat="server" ID="homeAjaxManager" OnAjaxRequest="OnHomeAjaxManager_AjaxRequest">                
            </telerik:RadAjaxManager>
            <div class="toolbarBG">
            <telerik:RadMultiPage runat="server" ID="ToolBarMView" SelectedIndex="0">
                <telerik:RadPageView runat="server" ID="CandidateToolBarView" >
                    <telerik:RadToolBar ID="CandidateToolBar" Style="z-index: 90001" runat="server" Skin="Office2007" EnableAjaxSkinRendering="true"
                        Orientation="Horizontal" CssClass="toolbar_margin" OnClientButtonClicked="onToolBar_ClientClicking">
                        <Items>
                            <telerik:RadToolBarSplitButton EnableDefaultButton="false" AccessKey="N" Text="<u>N</u>ew"
                                 ImageUrl="images/16x16/new.png" Value="newcandidateDefault">
                                <Buttons>
                                    <telerik:RadToolBarButton Text="<u>C</u>andidate" ImageUrl="images/16x16/candidate.png"
                                        Width="180" Value="newcandidate"  NavigateUrl="javascript:;">
                                    </telerik:RadToolBarButton>
                                    <telerik:RadToolBarButton Text="Com<u>p</u>any" AccessKey="P" ImageUrl="images/16x16/company.png" Value="newcompany"  NavigateUrl="javascript:;">
                                    </telerik:RadToolBarButton>
                                    <telerik:RadToolBarButton Text="<u>A</u>ction" AccessKey="A" ImageUrl="images/16x16/action.png" Value="newaction"  NavigateUrl="javascript:;">
                                    </telerik:RadToolBarButton>
                                    <telerik:RadToolBarButton Text="<u>J</u>ob" AccessKey="J" ImageUrl="images/16x16/job.png" Value="newjob">
                                    </telerik:RadToolBarButton>
                                    <telerik:RadToolBarButton Text="<u>I</u>nvoice" AccessKey="I" ImageUrl="images/16x16/invoicing.png" Value="newinvoicing"  NavigateUrl="javascript:;">
                                    </telerik:RadToolBarButton>
                                </Buttons>
                            </telerik:RadToolBarSplitButton>
                            <telerik:RadToolBarButton IsSeparator="true">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarButton  AccessKey="O" Value="opencandidate" ImageUrl="images/16x16/open.png" Enabled="false" NavigateUrl="javascript:;">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarButton  AccessKey="D" Value="deletecandidate" ImageUrl="images/16x16/delete.png" Enabled="false"  NavigateUrl="javascript:;">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarButton Text="View/Edit" AccessKey="V" Value="vieweditcandidate" ImageUrl="images/16x16/candidate_view.png" Enabled="false"  NavigateUrl="javascript:;">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarButton AccessKey="S" Value="savecandidate" ImageUrl="images/16x16/save.png" Enabled="false"  NavigateUrl="javascript:;">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarButton AccessKey="F" Value="advancesearch" ImageUrl="images/16x16/find.png"  NavigateUrl="javascript:;">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarButton IsSeparator="true">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarSplitButton EnableDefaultButton="false" Text="Action" Enabled="false"
                                ImageUrl="images/16x16/action.png" Value="viewcandidateactions" NavigateUrl="javascript:;" >
                                <Buttons>
                                    <telerik:RadToolBarButton Text="Add action" Width="180" Value="addcandidateactions" NavigateUrl="javascript:;">
                                    </telerik:RadToolBarButton>
                                </Buttons>
                            </telerik:RadToolBarSplitButton>
                        </Items>
                    </telerik:RadToolBar>
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="CompanyToolBarView">
                    <telerik:RadToolBar ID="CompanyToolBar" Style="z-index: 90001" runat="server" Skin="Office2007"
                        Orientation="Horizontal" CssClass="toolbar_margin" OnClientButtonClicked="onToolBar_ClientClicking">
                        <Items>
                            <telerik:RadToolBarSplitButton EnableDefaultButton="false" AccessKey="N" Text="<u>N</u>ew"
                                 ImageUrl="images/16x16/new.png" Value="newcompanyDefault" NavigateUrl="javascript:;">
                                <Buttons>
                                    <telerik:RadToolBarButton Text="<u>C</u>andidate" AccessKey="C" ImageUrl="images/16x16/candidate.png" NavigateUrl="javascript:;"
                                        Width="180" Value="newcandidate">
                                    </telerik:RadToolBarButton>
                                    <telerik:RadToolBarButton Text="Com<u>p</u>any" AccessKey="P" ImageUrl="images/16x16/company.png" Value="newcompany" NavigateUrl="javascript:;">
                                    </telerik:RadToolBarButton >
                                    <telerik:RadToolBarButton Text="<u>A</u>ction" AccessKey="A" ImageUrl="images/16x16/action.png" Value="newaction" NavigateUrl="javascript:;">
                                    </telerik:RadToolBarButton>
                                    <telerik:RadToolBarButton Text="<u>J</u>ob" AccessKey="J" ImageUrl="images/16x16/job.png" Value="newjob" NavigateUrl="javascript:;">
                                    </telerik:RadToolBarButton>
                                    <telerik:RadToolBarButton Text="<u>I</u>nvoice" AccessKey="I" ImageUrl="images/16x16/invoicing.png" Value="newinvoicing" NavigateUrl="javascript:;">
                                    </telerik:RadToolBarButton>
                                </Buttons>
                            </telerik:RadToolBarSplitButton>
                            <telerik:RadToolBarButton IsSeparator="true">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarButton ToolTip="Open" Value="opencompany" Enabled="false" ImageUrl="images/16x16/open.png" NavigateUrl="javascript:;">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarButton ToolTip="Delete" Value="deletecompany" Enabled="false" ImageUrl="images/16x16/delete.png" NavigateUrl="javascript:;">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarButton Text="View/Edit" ToolTip="Change mode" Value="vieweditcompany" Enabled="false" ImageUrl="images/16x16/company_view.png" NavigateUrl="javascript:;">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarButton ToolTip="Save" Value="savecompany" Enabled="false" ImageUrl="images/16x16/save.png" NavigateUrl="javascript:;">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarButton IsSeparator="true">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarSplitButton EnableDefaultButton="false" Text="<u>A</u>ction"
                                ImageUrl="images/16x16/action.png" AccessKey="A" Value="viewcompanyactions" Enabled="false" NavigateUrl="javascript:;">
                                <Buttons>
                                    <telerik:RadToolBarButton Text="Add action" Width="180" Value="addcompanyaction" Enabled="false" NavigateUrl="javascript:;">
                                    </telerik:RadToolBarButton>
                                </Buttons>
                            </telerik:RadToolBarSplitButton>
                            <telerik:RadToolBarSplitButton EnableDefaultButton="false" Text="Job"
                                ImageUrl="images/16x16/job.png" AccessKey="J" Value="viewcompanyjobs" Enabled="false" NavigateUrl="javascript:;">
                                <Buttons>
                                    <telerik:RadToolBarButton Text="Add Job" Width="180" Value="addcompanyjobs" Enabled="false" NavigateUrl="javascript:;">
                                    </telerik:RadToolBarButton>
                                </Buttons>
                            </telerik:RadToolBarSplitButton>
                            <telerik:RadToolBarSplitButton EnableDefaultButton="false" Text="Invoice"
                                ImageUrl="images/16x16/invoicing.png" AccessKey="I" Value="viewcompanyinvoices" Enabled="false" NavigateUrl="javascript:;">
                                <Buttons>
                                    <telerik:RadToolBarButton Text="Add Invoice" Width="180" Value="addcompanyinvoice" Enabled="false" NavigateUrl="javascript:;">
                                    </telerik:RadToolBarButton>
                                </Buttons>
                            </telerik:RadToolBarSplitButton>
                        </Items>
                    </telerik:RadToolBar>
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="ActionToolBarView">
                    <telerik:RadToolBar ID="ActionToolBar" Style="z-index: 90001" runat="server" Skin="Office2007"
                        Orientation="Horizontal" CssClass="toolbar_margin" OnClientButtonClicked="onToolBar_ClientClicking">
                        <Items>
                            <telerik:RadToolBarSplitButton EnableDefaultButton="false" AccessKey="N" Text="<u>N</u>ew"
                                ImageUrl="images/16x16/new.png" Value="newactionDefault">
                                <Buttons>
                                    <telerik:RadToolBarButton Text="<u>C</u>andidate" ImageUrl="images/16x16/candidate.png"
                                        Width="180" Value="newcandidate" NavigateUrl="javascript:;">
                                    </telerik:RadToolBarButton>
                                    <telerik:RadToolBarButton Text="Com<u>p</u>any" ImageUrl="images/16x16/company.png" Value="newcompany" NavigateUrl="javascript:;">
                                    </telerik:RadToolBarButton>
                                    <telerik:RadToolBarButton Text="<u>A</u>ction" ImageUrl="images/16x16/action.png" Value="newaction" NavigateUrl="javascript:;">
                                    </telerik:RadToolBarButton>
                                    <telerik:RadToolBarButton Text="<u>J</u>ob" ImageUrl="images/16x16/job.png" Value="newjob" NavigateUrl="javascript:;">
                                    </telerik:RadToolBarButton>
                                    <telerik:RadToolBarButton Text="<u>I</u>nvoice" ImageUrl="images/16x16/invoicing.png" Value="newinvoicing" NavigateUrl="javascript:;">
                                    </telerik:RadToolBarButton>
                                </Buttons>
                            </telerik:RadToolBarSplitButton>
                            <telerik:RadToolBarButton IsSeparator="true">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarButton ToolTip="Open" Value="openaction" ImageUrl="images/16x16/open.png" Enabled="false" NavigateUrl="javascript:;">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarButton ToolTip="Delete" Value="deleteaction" ImageUrl="images/16x16/delete.png" Enabled="false" NavigateUrl="javascript:;">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarButton Text="View/Edit" ToolTip="Change mode" Value="vieweditaction" Enabled="false" ImageUrl="images/16x16/action_view.png" NavigateUrl="javascript:;">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarButton ToolTip="Save" Value="saveaction" Enabled="false" ImageUrl="images/16x16/save.png" NavigateUrl="javascript:;">
                            </telerik:RadToolBarButton>
                        </Items>
                    </telerik:RadToolBar>
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="JobToolBarView">
                    <telerik:RadToolBar ID="JobToolBar" Style="z-index: 90001" runat="server" Skin="Office2007"
                        Orientation="Horizontal" CssClass="toolbar_margin" OnClientButtonClicked="onToolBar_ClientClicking">
                        <Items>
                            <telerik:RadToolBarSplitButton EnableDefaultButton="false" AccessKey="N" Text="<u>N</u>ew"
                                ImageUrl="images/16x16/new.png"  Value="newjobDefault" NavigateUrl="javascript:;">
                                <Buttons>
                                    <telerik:RadToolBarButton Text="<u>C</u>andidate" ImageUrl="images/16x16/candidate.png" Value="newcandidate" NavigateUrl="javascript:;"
                                        Width="180">
                                    </telerik:RadToolBarButton>
                                    <telerik:RadToolBarButton Text="Com<u>p</u>any" ImageUrl="images/16x16/company.png" Value="newcompany" NavigateUrl="javascript:;">
                                    </telerik:RadToolBarButton>
                                    <telerik:RadToolBarButton Text="<u>A</u>ction" ImageUrl="images/16x16/action.png" Value="newaction" NavigateUrl="javascript:;">
                                    </telerik:RadToolBarButton>
                                    <telerik:RadToolBarButton Text="<u>J</u>ob" ImageUrl="images/16x16/job.png" Value="newjob" NavigateUrl="javascript:;">
                                    </telerik:RadToolBarButton>
                                    <telerik:RadToolBarButton Text="<u>I</u>nvoice" ImageUrl="images/16x16/invoicing.png" Value="newinvoicing" NavigateUrl="javascript:;">
                                    </telerik:RadToolBarButton>
                                </Buttons>
                            </telerik:RadToolBarSplitButton>
                            <telerik:RadToolBarButton IsSeparator="true">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarButton ToolTip="Open" Value="openjob" ImageUrl="images/16x16/open.png" Enabled="false" NavigateUrl="javascript:;">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarButton ToolTip="Delete" Value="deletejob" ImageUrl="images/16x16/delete.png" Enabled="false" NavigateUrl="javascript:;">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarButton ToolTip="View/Edit" Value="vieweditjob" ImageUrl="images/16x16/job_view.png" Enabled="false" NavigateUrl="javascript:;">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarButton ToolTip="Save" Value="savejob" ImageUrl="images/16x16/save.png" Enabled="false" NavigateUrl="javascript:;">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarButton ToolTip="Preview" Value="previewjob" ImageUrl="images/16x16/preview.png" Enabled="false" NavigateUrl="javascript:;">
                            </telerik:RadToolBarButton>
                        </Items>
                    </telerik:RadToolBar>
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="RadPageView1">
                    <telerik:RadToolBar ID="InvoicingToolBar" Style="z-index: 90001" runat="server" Skin="Office2007"
                        Orientation="Horizontal" CssClass="toolbar_margin"  OnClientButtonClicked="onToolBar_ClientClicking">
                        <Items>
                            <telerik:RadToolBarSplitButton EnableDefaultButton="false" AccessKey="N" Text="<u>N</u>ew" Value="newinvoicingDefault"
                                ImageUrl="images/16x16/new.png">
                                <Buttons>
                                    <telerik:RadToolBarButton Text="<u>C</u>andidate" ImageUrl="images/16x16/candidate.png" Value="newcandidate"
                                        Width="180">
                                    </telerik:RadToolBarButton>
                                    <telerik:RadToolBarButton Text="Com<u>p</u>any" ImageUrl="images/16x16/company.png" Value="newcompany">
                                    </telerik:RadToolBarButton>
                                    <telerik:RadToolBarButton Text="<u>A</u>ction" ImageUrl="images/16x16/action.png" Value="newaction">
                                    </telerik:RadToolBarButton>
                                    <telerik:RadToolBarButton Text="<u>J</u>ob" ImageUrl="images/16x16/job.png" Value="newjob">
                                    </telerik:RadToolBarButton>
                                    <telerik:RadToolBarButton Text="<u>I</u>nvoice" ImageUrl="images/16x16/invoicing.png" Value="newinvoicing"> 
                                    </telerik:RadToolBarButton>
                                </Buttons>
                            </telerik:RadToolBarSplitButton>
                            <telerik:RadToolBarButton IsSeparator="true">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarButton ToolTip="Open" Value="openinvoice" Enabled="false" NavigateUrl="javascript:;" ImageUrl="images/16x16/open.png">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarButton ToolTip="Delete" Value="deleteinvoice" Enabled="false" NavigateUrl="javascript:;" ImageUrl="images/16x16/delete.png">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarButton ToolTip="Change mode" Value="vieweditinvoice" Enabled="false" NavigateUrl="javascript:;" ImageUrl="images/16x16/invoice_view.png">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarButton ToolTip="Save" Value="saveinvoice" Enabled="false" NavigateUrl="javascript:;" ImageUrl="images/16x16/save.png">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarButton ToolTip="Print" Value="printinvoice" Enabled="false" NavigateUrl="javascript:;" ImageUrl="images/16x16/printer.png">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarButton ToolTip="Email" Value="emailinvoice" Enabled="false" NavigateUrl="javascript:;" ImageUrl="images/16x16/mail_new.png">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarButton ToolTip="Copy Invoice" Value="copyinvoice" Enabled="false" NavigateUrl="javascript:;" ImageUrl="images/16x16/copy.png">
                            </telerik:RadToolBarButton>
                        </Items>
                    </telerik:RadToolBar>
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="DefaultToolBarView">
                <telerik:RadToolBar ID="DefaultToolBar" Style="z-index: 90001" runat="server" Skin="Office2007"
                        Orientation="Horizontal" CssClass="toolbar_margin" OnClientButtonClicked="onToolBar_ClientClicking">
                        <Items>
                            <telerik:RadToolBarSplitButton EnableDefaultButton="false" AccessKey="N" Text="<u>N</u>ew"
                                ImageUrl="images/16x16/new.png" Value="new" >
                                <Buttons>
                                    <telerik:RadToolBarButton Text="<u>C</u>andidate" ImageUrl="images/16x16/candidate.png" Value="newcandidate"
                                        Width="180">
                                    </telerik:RadToolBarButton>
                                    <telerik:RadToolBarButton Text="Com<u>p</u>any" ImageUrl="images/16x16/company.png" Value="newcompany">
                                    </telerik:RadToolBarButton>
                                    <telerik:RadToolBarButton Text="<u>A</u>ction" ImageUrl="images/16x16/action.png" Value="newaction">
                                    </telerik:RadToolBarButton>
                                    <telerik:RadToolBarButton Text="<u>J</u>ob" ImageUrl="images/16x16/job.png" Value="newjob">
                                    </telerik:RadToolBarButton>
                                    <telerik:RadToolBarButton Text="<u>I</u>nvoice" ImageUrl="images/16x16/invoicing.png" Value="newinvoicing">
                                    </telerik:RadToolBarButton>
                                </Buttons>
                            </telerik:RadToolBarSplitButton>
                        </Items>
                    </telerik:RadToolBar>
                </telerik:RadPageView>
            </telerik:RadMultiPage>
        </div>       
            <telerik:RadSplitter ID="RadSplitterBrowser" runat="server" Height="100%" Width="100%"
                BorderSize="0" BorderColor="White" BorderStyle="None" Skin="WebBlue" CssClass="border_lbr">                
                <telerik:RadPane ID="RadPanePanelBar" runat="server"  Height="100%" Width="320" Scrolling="None">
                    <div id="multiPage">
                        <telerik:RadMultiPage ID="RadMultiPage1" runat="server" SelectedIndex="0" BackColor="White"
                            BorderColor="#f3f3f3" BorderStyle="Solid" BorderWidth="0px">                            
                            <telerik:RadPageView runat="server" ID="CandidateView">
                            <div>
                                <div class="qsfexHeader" style="text-align: left; overflow:hidden;">
                                    <asp:Label ID="lblCandidatesHeaderPageView" runat="server" Text="Candidates"></asp:Label>
                                </div>
                                <div style="overflow:auto; overflow-x: hidden" id="candidatePageContent">
                                    <table cellpadding="0" cellspacing="0" style="margin-top:20px;" width="100%">
                                        <tr>
                                            <td style="text-align: center; vertical-align: top;">
                                                <telerik:RadTextBox ID="txtLastNameSearch" runat="server" Width="145"
                                                    MaxLength="35">
                                                </telerik:RadTextBox>
                                                <asp:Button ID="btnSearchCandidates" runat="server" Text="Search" CssClass="flatButton"
                                                    OnClientClick="return OnCandidateSearchClicked();"></asp:Button>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center;">
                                                <asp:HyperLink ID="lnkCandidateAdvancedSearch" runat="server" Text="Advanced Search"
                                                    Font-Underline="true"  NavigateUrl="javascript:void();"
                                                    onclick="return OnCandidateAdvancedSearchClick();"></asp:HyperLink></td>
                                        </tr>
                                        <tr style="height: 4px;">
                                            <td>
                                            </td>
                                        </tr>
                                        <tr style="height: 1px; background: url(images/separator-line.gif) repeat-x">
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="litLastFiveCandidate" runat="server" Text="Last 5 viewed candidates :"></asp:Literal></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="divLast5Candidate" runat="server" style="width:100%">
                                                
                                                <asp:Repeater ID="lastFiveCandidateList" runat="server" OnItemDataBound="OnLastFiveListItemDataBound">
                                                    <HeaderTemplate>
                                                        <ul type="disc" style="">
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <li style="margin-top:3px;">
                                                            <asp:HyperLink ID="lnkLastFiveItem" runat="server"
                                                                Font-Underline="true" NavigateUrl="javascript:void();" />
                                                        </li>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        </ul>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr style="height: 1px; background: url(images/separator-line.gif) repeat-x">
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                               <asp:Literal runat="server" ID="lblSearchCV" Text="Search on candidate's CV"></asp:Literal>:
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align:center">
                                             <asp:Panel runat="server" ID="pnlCandidateCVSearch" DefaultButton="btnSearchCV">
                                                <asp:TextBox runat="server" ID="txtCVSearchKeyWork" Width="145"></asp:TextBox>
                                                <asp:Button ID="btnSearchCV" runat="server" Text="Search" CssClass="flatButton"
                                                        OnClientClick="return OnCandidateCVSearchClicked();"></asp:Button>
                                              </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            </telerik:RadPageView>
                            <telerik:RadPageView runat="server" ID="companyView">
                                <div class="qsfexHeader" style="width: 100%; text-align: left">
                                    <div style="text-align: left;">
                                        <asp:Label ID="lblCompaniesHeaderPageView" runat="server" Text="Companies"></asp:Label>
                                    </div>                                    
                                </div>
                               <div style="margin-top:0px; overflow:auto; overflow-x: hidden" id="companyPageContent">
                                <table width="100%" style="text-align: center; margin-top:20px;" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="center" colspan="2">
                                         <asp:Panel runat="server" ID="pnlCompanySearch" DefaultButton="btnCompanySearch">
                                            <telerik:RadTextBox runat="server" ID="txtCompanySearch" Width="145" MaxLength="100">
                                            </telerik:RadTextBox>&nbsp;
                                            <asp:Button runat="server" ID="btnCompanySearch" Text="Search" CssClass="flatButton"
                                                OnClientClick="return OnCompanySearchClicked();" />
                                          </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10%">
                                        </td>
                                        <td align="left" >
                                            <div style="margin-left:15px;" class="text">
                                                <asp:RadioButtonList runat="server" ID="rdoListCompanyType" RepeatDirection="Vertical"  CssClass="text" >
                                                    <asp:ListItem Text="All" Value="all" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Customers" Value="client"></asp:ListItem>
                                                    <asp:ListItem Text="Prospects" Value="prospect"></asp:ListItem>
                                                    <asp:ListItem Text="Disabled" Value="inactive"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr style="height: 1px; background: url(images/separator-line.gif) repeat-x">
                                        <td colspan="2">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left">
                                            <asp:Literal runat="server" ID="lblLastViewedCompanies" Text="Last 5 viewed companies:"></asp:Literal></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10%">
                                        </td>
                                        <td align="left">
                                            <div id="divLast5Company" runat="server" style="width:100%">
                                            <asp:Repeater runat="server" ID="rptLastViewedCompanies" OnItemDataBound="rptLastViewedCompanies_ItemDataBound">
                                                <HeaderTemplate>
                                                    <ul type="disc" style="">
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <li style="margin-top:3px;">
                                                        <asp:HyperLink ID="lnkLast5Company" runat="server"
                                                                Font-Underline="true"  NavigateUrl="javascript:void();"  />
                                                    </li>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </ul>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                </div>
                            </telerik:RadPageView>
                            <telerik:RadPageView runat="server" ID="actionView">
                                <div class="qsfexHeader" style="width: 100%; text-align: left">
                                    <div style="text-align: left;">
                                        <asp:Label ID="lblActionsHeaderPageView" runat="server" Text="Actions"></asp:Label>
                                    </div>                                    
                                </div>
                                <div style="margin-top:0px;overflow:auto; overflow-x: hidden" id="actionPageContent">
                                    <table width="100%" style=" margin-top:20px;" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <div id="divSearchPanel" runat="server">
                                                <asp:Panel runat="server" ID="pnlSearchAction" DefaultButton="btnActionSearch">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblActiveActionSearch" runat="server" Text="Active"></asp:Label>
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:RadioButton ID="radActiveActionYes" runat="server" GroupName="ActiveGroup" />
                                                                <asp:Label ID="lblActiveActionYes" runat="server" Text="Yes"></asp:Label>
                                                                <asp:RadioButton ID="radActiveActionNo" runat="server" GroupName="ActiveGroup" />
                                                                <asp:Label ID="lblActiveActionNo" runat="server" Text="No"></asp:Label>
                                                                <asp:RadioButton ID="radActiveActionBoth" runat="server" GroupName="ActiveGroup"
                                                                    Checked="true" />
                                                                <asp:Label ID="lblActiveActionBoth" runat="server" Text="Both"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblDateBetweenAction" runat="server" Text="Date between"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <telerik:RadDatePicker ID="datDateBetweenAction" runat="server" Width="90px" Skin="Office2007"  Calendar-CultureInfo="en-US">
                                                                    <DateInput ID="dateInputDateBetweenAction" runat="server" DateFormat="dd/MM/yyyy"
                                                                        DisplayDateFormat="dd/MM/yyyy">
                                                                    </DateInput>
                                                                </telerik:RadDatePicker>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblDateAndAction" runat="server" Text="and"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <telerik:RadDatePicker ID="datDateAndAction" runat="server" Width="90px" Skin="Office2007"  Calendar-CultureInfo="en-US">
                                                                    <DateInput ID="dateInputDateAndAction" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd/MM/yyyy">
                                                                    </DateInput>
                                                                </telerik:RadDatePicker>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblCandidateActionSearch" runat="server" Text="Candidate"></asp:Label>
                                                            </td>
                                                            <td colspan="3">
                                                                <telerik:RadTextBox ID="txtCandidateAction" runat="server" Width="207" MaxLength="35" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblCompanyActionSearch" runat="server" Text="Company"></asp:Label>
                                                            </td>
                                                            <td colspan="3">
                                                                <telerik:RadTextBox ID="txtCompanyAction" runat="server" Width="207" MaxLength="100" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblTypeActionSearch" runat="server" Text="Type"></asp:Label>
                                                            </td>
                                                            <td colspan="3">
                                                                <telerik:RadComboBox ID="ddlTypeAction" runat="server" Height="300" Width="211" Skin="Office2007">
                                                                </telerik:RadComboBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblDescriptionActionSearch" runat="server" Text="Description"></asp:Label>
                                                            </td>
                                                            <td colspan="3">
                                                                <telerik:RadTextBox ID="txtDescriptionAction" runat="server" Width="207" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblResponsibleActionSearch" runat="server" Text="Responsible"></asp:Label>
                                                            </td>
                                                            <td colspan="3">
                                                                <telerik:RadComboBox ID="ddlResponsibleAction" runat="server" Width="211" Skin="Office2007" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" align="center">
                                                                <asp:Button runat="server" ID="btnActionSearch" Text="Search" CssClass="flatButton"
                                                                    OnClientClick="return onActionSearch();" /><%-- OnClick="OnBtnActionSearchClicked"--%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    </asp:Panel>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr style="height: 1px; background: url(images/separator-line.gif) repeat-x">
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="divMyAction" runat="server">
                                                    <table>
                                                        <tr>
                                                            <td align="left" colspan="2">
                                                                <asp:Literal runat="server" ID="lblMyActions" Text="My actions:"></asp:Literal></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 5%">
                                                            </td>
                                                            <td align="left">
                                                                <ul type="circle" style="margin-top: 0px;">
                                                                    <li>
                                                                        <asp:HyperLink runat="server" ID="lnkMyActiveThisWeek" Text="Active this week" Font-Underline="true"
                                                                             NavigateUrl="javascript:void();" onclick="return OnSearchActionClick('MyActiveThisWeek');"></asp:HyperLink></li>
                                                                    <li>
                                                                        <asp:HyperLink runat="server" ID="lnkMyActive" Text="Active" Font-Underline="true"
                                                                             NavigateUrl="javascript:void();" onclick="return OnSearchActionClick('MyActive');"></asp:HyperLink></li>
                                                                    <li>
                                                                        <asp:HyperLink runat="server" ID="lnkMyInactive" Text="Inactive" Font-Underline="true"
                                                                             NavigateUrl="javascript:void();" onclick="return OnSearchActionClick('MyInactive');"></asp:HyperLink></li>
                                                                    <li>
                                                                        <asp:HyperLink runat="server" ID="lnkMyActions" Text="All my actions" Font-Underline="true"
                                                                             NavigateUrl="javascript:void();" onclick="return OnSearchActionClick('MyAllActions');"></asp:HyperLink></li>
                                                                </ul>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="divAllAction" runat="server">
                                                    <table>
                                                        <tr>
                                                            <td align="left" colspan="2">
                                                                <asp:Literal runat="server" ID="lblAllAction" Text="All actions:"></asp:Literal></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 5%">
                                                            </td>
                                                            <td align="left">
                                                                <ul type="circle" style="margin-top: 0px;">
                                                                    <li>
                                                                        <asp:HyperLink runat="server" ID="lnkActiveAction" Text="Active" Font-Underline="true"
                                                                             NavigateUrl="javascript:void();" onclick="return OnSearchActionClick('AllActive');"></asp:HyperLink></li>
                                                                    <li>
                                                                        <asp:HyperLink runat="server" ID="lnkInactiveAction" Text="Inactive" Font-Underline="true"
                                                                             NavigateUrl="javascript:void();" onclick="return OnSearchActionClick('AllInactive');"></asp:HyperLink></li>
                                                                    <li>
                                                                        <asp:HyperLink runat="server" ID="lnkAllActions" Text="All" Font-Underline="true"
                                                                             NavigateUrl="javascript:void();" onclick="return OnSearchActionClick('AllActions');"></asp:HyperLink></li>
                                                                </ul>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </telerik:RadPageView>
                            <telerik:RadPageView runat="server" ID="jobView">
                                <div class="qsfexHeader" style="width: 100%; text-align: left">
                                    <div style="text-align: left;">
                                        <asp:Label ID="lblJobsHeaderPageView" runat="server" Text="Jobs"></asp:Label>
                                    </div>                                    
                                </div>
                                <div style="margin-top:0px;overflow:auto; overflow-x: hidden" id="jobPageContent">                                  
                                <asp:Panel runat="server" ID="pnlJobSearch" DefaultButton="btnJobSearch">
                                    <table width="100%" style="margin-top: 20px;">
                                        <tr>
                                            <td>
                                                <asp:Literal runat="server" ID="lblTitle" Text="Title"></asp:Literal>
                                            </td>
                                            <td colspan="2">
                                                <telerik:RadTextBox ID="txtJobTitle" runat="server" Text="" Width="200">
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal runat="server" ID="lblActiveJob" Text="Active"></asp:Literal>
                                            </td>
                                            <td colspan="2">
                                                <asp:RadioButton ID="radActiveJobYes" runat="server" GroupName="ActiveJobGroup" />
                                                <asp:Label ID="lblActiveJobYes" runat="server" Text="Yes"></asp:Label>
                                                <asp:RadioButton ID="radActiveJobNo" runat="server" GroupName="ActiveJobGroup" />
                                                <asp:Label ID="lblActiveJobNo" runat="server" Text="No"></asp:Label>
                                                <asp:RadioButton ID="radActiveJobBoth" runat="server" GroupName="ActiveJobGroup"
                                                    Checked="true" />
                                                <asp:Label ID="lblActiveJobBoth" runat="server" Text="Both"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal runat="server" ID="lblCreatedDate" Text="Created"></asp:Literal>
                                            </td>
                                            <td>
                                                <telerik:RadDatePicker ID="calCreatedDate1" runat="server" MinDate="0001-01-01" Skin="Office2007"
                                                    Width="100"  Calendar-CultureInfo="en-US">
                                                    <DateInput ID="inputCreatedDate1" runat="server" Skin="Office2007" DateFormat="dd/MM/yyyy"
                                                        DisplayDateFormat="dd/MM/yyyy">
                                                    </DateInput>
                                                </telerik:RadDatePicker>
                                            </td>
                                            <td>
                                                <telerik:RadDatePicker ID="calCreatedDate2" runat="server" MinDate="0001-01-01" Skin="Office2007"
                                                    Width="100"  Calendar-CultureInfo="en-US">
                                                    <DateInput ID="inputCreatedDate2" runat="server" Skin="Office2007" DateFormat="dd/MM/yyyy"
                                                        DisplayDateFormat="dd/MM/yyyy">
                                                    </DateInput>
                                                </telerik:RadDatePicker>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal runat="server" ID="lblActivation" Text="Activate"></asp:Literal>
                                            </td>
                                            <td>
                                                <telerik:RadDatePicker ID="calActivationDate1" runat="server" MinDate="0001-01-01"
                                                    Skin="Office2007" Width="100"  Calendar-CultureInfo="en-US">
                                                    <DateInput ID="inputActivation1" runat="server" Skin="Office2007" DateFormat="dd/MM/yyyy"
                                                        DisplayDateFormat="dd/MM/yyyy">
                                                    </DateInput>
                                                </telerik:RadDatePicker>
                                            </td>
                                            <td>
                                                <telerik:RadDatePicker ID="calActivationDate2" runat="server" MinDate="0001-01-01"
                                                    Skin="Office2007" Width="100"  Calendar-CultureInfo="en-US">
                                                    <DateInput ID="inputActivation2" runat="server" Skin="Office2007" DateFormat="dd/MM/yyyy"
                                                        DisplayDateFormat="dd/MM/yyyy">
                                                    </DateInput>
                                                </telerik:RadDatePicker>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal runat="server" ID="lblExpiredDate" Text="Exprited"></asp:Literal>
                                            </td>
                                            <td>
                                                <telerik:RadDatePicker ID="calExpired1" runat="server" MinDate="0001-01-01" Skin="Office2007"
                                                    Width="100"  Calendar-CultureInfo="en-US">
                                                    <DateInput ID="inputExpired1" runat="server" Skin="Office2007" DateFormat="dd/MM/yyyy"
                                                        DisplayDateFormat="dd/MM/yyyy">
                                                    </DateInput>
                                                </telerik:RadDatePicker>
                                            </td>
                                            <td>
                                                <telerik:RadDatePicker ID="calExpired2" runat="server" MinDate="0001-01-01" Skin="Office2007"
                                                    Width="100"  Calendar-CultureInfo="en-US">
                                                    <DateInput ID="inputExpired2" runat="server" Skin="Office2007" DateFormat="dd/MM/yyyy"
                                                        DisplayDateFormat="dd/MM/yyyy">
                                                    </DateInput>
                                                </telerik:RadDatePicker>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal runat="server" ID="lblProfile" Text="Profile"></asp:Literal>
                                            </td>
                                            <td colspan="2">
                                                <telerik:RadComboBox ID="ddlProfile" runat="Server" Skin="Office2007" Width="205">
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal runat="server" ID="lblFunctionFam" Text="Function Fam"></asp:Literal>
                                            </td>
                                            <td colspan="2">
                                                <telerik:RadComboBox ID="ddlFunctionFam" runat="Server" Skin="Office2007" Width="205" Height="300">
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align:top;">
                                                <asp:Literal runat="server" ID="lblLocation" Text="Location"></asp:Literal>
                                            </td>
                                            <td colspan="2">
                                                <asp:ListBox runat="server" ID="lbxLocation" SelectionMode="Multiple" Width="205"
                                                    CssClass="noneborder_textbox"></asp:ListBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal runat="server" ID="lblResponsible" Text="Responsible"></asp:Literal>
                                            </td>
                                            <td colspan="2">
                                                <telerik:RadComboBox ID="ddlResponsible" runat="Server" Skin="Office2007" Width="205" Height="300">
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal runat="server" ID="lblCompany" Text="Company"></asp:Literal>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox runat="server" ID="txtCompany" Width="200"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="3">
                                                <asp:Button ID="btnJobSearch" runat="server" Text="" CssClass="flatButton" OnClientClick="return onJobSearch();">
                                                </asp:Button><%--OnClick="OnButonJobSearch_Click"--%>
                                            </td>
                                        </tr>                                        
                                    </table>
                                    </asp:Panel>
                                    <table width="100%">
                                        <tr style="height: 1px; background: url(images/separator-line.gif) repeat-x">
                                            <td colspan="3">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left" colspan="3">
                                                <ul type="circle" style="margin-top: 0px;">
                                                    <li>
                                                        <asp:HyperLink runat="server" ID="lnkActiveJobs" Text="Active ads" Font-Underline="true"
                                                            CssClass="" NavigateUrl="javascript:void();" onclick="return onFindJobs('active');"></asp:HyperLink></li>
                                                    <li>
                                                        <asp:HyperLink runat="server" ID="lnkInactiveJobs" Text="Inactive ads" Font-Underline="true"
                                                            CssClass="" NavigateUrl="javascript:void();" onclick="return onFindJobs('inactive');"></asp:HyperLink></li>
                                                    <li>
                                                        <asp:HyperLink runat="server" ID="lnkAllJobs" Text="All" Font-Underline="true" CssClass=""
                                                            NavigateUrl="javascript:void();" onclick="return onFindJobs('all');"></asp:HyperLink></li>
                                                </ul>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </telerik:RadPageView>
                            <telerik:RadPageView runat="server" ID="statisticsView">
                                <div class="qsfexHeader" style="width: 100%; text-align: left">
                                    <div style="text-align: left;">
                                        <asp:Label ID="lblstatisticsHeaderPageView" runat="server" Text="Statistics"></asp:Label>
                                    </div>                                    
                                </div>
                                <div style="margin-top:0px;overflow:auto; overflow-x: hidden" id="statisticPageContent">
                                    <table width="100%" style=" margin-top:20px;" cellpadding="0" cellspacing="0">
                                        <tr>
                                             <td align="left">
                                                <ul type="circle" style="margin-top: 0px;">
                                                    <li>
                                                        <asp:HyperLink runat="server" ID="hypGeneralStatistics" Text="General statistics" Font-Underline="true"
                                                             NavigateUrl="javascript:void();" 
                                                            onclick="return OnHypGeneralStatisticsClick();"></asp:HyperLink></li>                                                   
                                                   <li>
                                                        <asp:HyperLink runat="server" ID="hypStudyLevelStatistics" Text="Study level statistics" Font-Underline="true"
                                                             NavigateUrl="javascript:void();" 
                                                            onclick="return OnHypStudyLevelStatisticsClick();"></asp:HyperLink></li>
                                                   <li>
                                                        <asp:HyperLink runat="server" ID="hypCandidateInscription" Text="Candidate inscriptions statistics" Font-Underline="true"
                                                             NavigateUrl="javascript:void();" 
                                                            onclick="return OnHypCanInscriptionStatisticsClick();"></asp:HyperLink></li>
                                                   <li>
                                                        <asp:HyperLink runat="server" ID="hypCandidateLocation" Text="Candidate locations statistics" Font-Underline="true"
                                                             NavigateUrl="javascript:void();" 
                                                            onclick="return OnHypCanLocationStatisticsClick();"></asp:HyperLink></li>
                                                </ul>
                                            </td>
                                        </tr>
                                    </table>                                                 
                                </div>
                            </telerik:RadPageView>
                            <telerik:RadPageView runat="server" ID="administrationView">
                                <div class="qsfexHeader" style="width: 100%; text-align: left">
                                    <div style="text-align: left;">
                                        <asp:Label ID="lblAdministrationsHeaderPageView" runat="server" Text="Administrations"></asp:Label>
                                    </div>
                                </div>
                                <div style="margin-top:0px;overflow:auto; overflow-x: hidden" id="administrationPageContent">
                                    <table width="100%" style=" margin-top:20px;" cellpadding="0" cellspacing="0">
                                        <tr>
                                             <td align="left">
                                                <ul type="circle" style="margin-top: 0px;">
                                                   <li>
                                                        <asp:HyperLink runat="server" ID="hypUserAndPermission" Text="User and permissions" Font-Underline="true"
                                                             NavigateUrl="javascript:void();" 
                                                            onclick="return OnUserAndPermissionClick();"></asp:HyperLink></li>
                                                   <li>
                                                        <asp:HyperLink runat="server" ID="hypPermission" Text="Permissions" Font-Underline="true"
                                                             NavigateUrl="javascript:void();" 
                                                            onclick="return OnPermissionsClick();"></asp:HyperLink></li>
                                                  <li>
                                                        <asp:HyperLink runat="server" ID="hypLocation" Text="Locations" Font-Underline="true"
                                                             NavigateUrl="javascript:void();" 
                                                            onclick="return OnLocationsClick();"></asp:HyperLink></li>
                                                  <li>
                                                        <asp:HyperLink runat="server" ID="hypUnits" Text="Units" Font-Underline="true"
                                                             NavigateUrl="javascript:void();" 
                                                            onclick="return OnHypUnitsClick();"></asp:HyperLink></li>
                                                  <li>
                                                        <asp:HyperLink runat="server" ID="hypProfiles" Text="Profiles" Font-Underline="true"
                                                             NavigateUrl="javascript:void();" 
                                                            onclick="return OnHypProfilesClick();"></asp:HyperLink></li>
                                                  <li>
                                                        <asp:HyperLink runat="server" ID="hypFunctionFam" Text="Function categories" Font-Underline="true"
                                                             NavigateUrl="javascript:void();" 
                                                            onclick="return OnHypFunctionFamClick();"></asp:HyperLink></li>
                                                  <li>
                                                        <asp:HyperLink runat="server" ID="hypFunction" Text="Functions" Font-Underline="true"
                                                             NavigateUrl="javascript:void();" 
                                                            onclick="return OnHypFunctionClick();"></asp:HyperLink></li>
                                                  <li>
                                                        <asp:HyperLink runat="server" ID="hypKnowledgeFam" Text="Knowledge categories" Font-Underline="true"
                                                             NavigateUrl="javascript:void();" 
                                                            onclick="return OnHypKnowledgeFamClick();"></asp:HyperLink></li>                                                                                              
                                                  <li>
                                                        <asp:HyperLink runat="server" ID="hypKnowledge" Text="Knowledges" Font-Underline="true"
                                                             NavigateUrl="javascript:void();" 
                                                            onclick="return OnHypKnowledgeClick();"></asp:HyperLink></li>
                                                  <li>
                                                        <asp:HyperLink runat="server" ID="hypStudyLevel" Text="Study levels" Font-Underline="true"
                                                             NavigateUrl="javascript:void();" 
                                                            onclick="return OnHypStudyLevelClick();"></asp:HyperLink></li>
                                                  <li>
                                                        <asp:HyperLink runat="server" ID="hypStudy" Text="Studies" Font-Underline="true"
                                                             NavigateUrl="javascript:void();" 
                                                            onclick="return OnHypStudyClick();"></asp:HyperLink></li>  
                                                  <li>
                                                        <asp:HyperLink runat="server" ID="hypTypeAction" Text="Action types" Font-Underline="true"
                                                             NavigateUrl="javascript:void();" 
                                                            onclick="return OnHypTypeActionClick();"></asp:HyperLink></li> 
                                                  <li>
                                                        <asp:HyperLink runat="server" ID="hypLanguage" Text="Languages" Font-Underline="true"
                                                             NavigateUrl="javascript:void();" 
                                                            onclick="return OnHypLanguageClick();"></asp:HyperLink></li> 
                                                  <li>
                                                        <asp:HyperLink runat="server" ID="hypNationality" Text="Nationalities" Font-Underline="true"
                                                             NavigateUrl="javascript:void();" 
                                                            onclick="return OnHypNationalityClick();"></asp:HyperLink></li> 
                                                  <li>
                                                        <asp:HyperLink runat="server" ID="hypSituationCivil" Text="Civil situations" Font-Underline="true"
                                                             NavigateUrl="javascript:void();" 
                                                            onclick="return OnHypSituationCivilClick();"></asp:HyperLink></li> 
                                                  <li>
                                                        <asp:HyperLink runat="server" ID="hypClientStatus" Text="Client status" Font-Underline="true"
                                                             NavigateUrl="javascript:void();" 
                                                            onclick="return OnHypClientStatusClick();"></asp:HyperLink></li> 
                                                  <li>
                                                        <asp:HyperLink runat="server" ID="hypLegalForm" Text="Legal forms" Font-Underline="true"
                                                             NavigateUrl="javascript:void();" 
                                                            onclick="return OnHypLegalFormClick();"></asp:HyperLink></li> 
                                                  <li>
                                                        <asp:HyperLink runat="server" ID="hypContactFunction" Text="Contact functions" Font-Underline="true"
                                                             NavigateUrl="javascript:void();" 
                                                            onclick="return OnHypContactFunctionClick();"></asp:HyperLink></li> 
                                                </ul>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </telerik:RadPageView>
                            <telerik:RadPageView runat="server" ID="invoicingView">
                                <div class="qsfexHeader" style="width: 100%; text-align: left">
                                    <div style="text-align: left;">
                                        <asp:Label ID="lblInvoicingHeaderPageView" runat="server" Text="Invoicing"></asp:Label>
                                    </div>
                                </div>
                                <div style="margin-top:0px;overflow:auto; overflow-x: hidden" id="invoicingPageContent">
                                   
                                        <table width="100%" style=" margin-top:20px;" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <div id="divSearchInvoice" runat="server">
                                                 <asp:Panel runat="server" ID="pnlInvoiceSearch" DefaultButton="btnInvoiceSearch">
                                                    <table>                                                        
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblInvoiceNumber" runat="server" Text="Invoice number"></asp:Label>
                                                            </td>                
                                                            <%--<td>
                                                                <asp:Label ID="lblInvoiceNumberFrom" runat="server" Text="from"></asp:Label>
                                                            </td> --%>                                           
                                                            <td>                                                                
                                                                <telerik:RadNumericTextBox ID="txtInvoiceNumberFrom" runat="server" Width="88px" Type="Number" Skin="Office2007"
                                                                            NumberFormat-DecimalDigits="0" NumberFormat-PositivePattern="n"
                                                                            NumberFormat-GroupSeparator="" />
                                                            </td>
                                                             <td>
                                                                <asp:Label ID="lblInvoiceNumberTo" runat="server" Text="to"></asp:Label>
                                                            </td>                                            
                                                            <td>                                                                
                                                                <telerik:RadNumericTextBox ID="txtInvoiceNumberTo" runat="server" Width="88px" Type="Number" Skin="Office2007"
                                                                            NumberFormat-DecimalDigits="0" NumberFormat-PositivePattern="n"
                                                                            NumberFormat-GroupSeparator="" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblFiscalYear" runat="server" Text="Fiscal year"></asp:Label>
                                                            </td>
                                                            <td colspan="3">
                                                                <telerik:RadComboBox ID="ddlFiscalYear" runat="server" Width="204"  Skin="Office2007"/>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblInvoiceDate" runat="server" Text="Invoice date"></asp:Label>
                                                            </td>
                                                            <%--<td>
                                                                <asp:Label ID="lblInvoiceDateFrom" runat="server" Text="from"></asp:Label>
                                                            </td>--%>
                                                            <td>
                                                                <telerik:RadDatePicker ID="datInvoiceDateFrom" runat="server" Width="92px" Skin="Office2007"  Calendar-CultureInfo="en-US">
                                                                    <DateInput ID="datInputInvoiceDateFrom" runat="server" DateFormat="dd/MM/yyyy"
                                                                        DisplayDateFormat="dd/MM/yyyy">
                                                                    </DateInput>
                                                                </telerik:RadDatePicker>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblInvoiceDateTo" runat="server" Text="to"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <telerik:RadDatePicker ID="datInvoiceDateTo" runat="server" Width="92px" Skin="Office2007"  Calendar-CultureInfo="en-US">
                                                                    <DateInput ID="datInputInvoiceDateTo" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd/MM/yyyy">
                                                                    </DateInput>
                                                                </telerik:RadDatePicker>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblInvoiceType" runat="server" Text="Invoice type"></asp:Label>
                                                            </td>
                                                            <td colspan="3">
                                                                <telerik:RadComboBox ID="ddlInvoiceType" runat="server" Width="204"  Skin="Office2007"/>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                             <td>
                                                                <asp:Label ID="lblInvoiceCustomer" runat="server" Text="Customer"></asp:Label>
                                                            </td>
                                                            <td colspan="3" style=" vertical-align:bottom" nowrap="nowrap">
                                                              <div style="float:left">
                                                                <div style="float:left">
                                                                    <telerik:RadComboBox runat="server" ID="ddlCompany" EnableAjaxSkinRendering="true" Skin="Office2007" 
                                                                        Width="170" Height="250" AllowCustomText="true" EnableLoadOnDemand="True" 
                                                                         DataValueField="CompanyID" DataTextField="CompanyName" OnClientItemsRequesting="onDropdownCompany_ClientItemsRequesting"
                                                                        OnItemsRequested="OnDropdownCompany_ItemsRequested"></telerik:RadComboBox>
                    
                                                                    <telerik:RadTextBox ID="txtInvoiceCompanyName" runat="server" ReadOnly="true" BackColor="White"
                                                                    BorderWidth="1" BorderColor="#A8BEDA" Skin="Office2007" Width="136px" Visible="false">
                                                                    </telerik:RadTextBox>                                                                
                                                                    <asp:Button ID="btnInvoiceChooseCompany" runat="server" Width="30" Text="..." OnClientClick="return OnButtonChooseCompanyClientClicked();"/>
                                                                </div>
                                                                <%--<div style="float:left; text-align:right; margin-left:10px; vertical-align:bottom; margin-top:4px;">
                                                                    <asp:ImageButton ID="btnInvoiceDeleteCompany" ToolTip="remove" ImageAlign="AbsBottom" runat="server" ImageUrl="~/images/16x16/delete.png" OnClientClick="return OnButtonDeleteCompanyClientClicked();"/>
                                                                </div>--%>
                                                            </div>
                                                            </td>                                                            
                                                        </tr>   
                                                        <tr>
                                                            <td colspan="4" align="center">
                                                                <asp:Button runat="server" ID="btnInvoiceSearch" Text="Search" CssClass="flatButton" OnClientClick="return onInvoiceSearch();" /> <%--OnClick="OnBtnInvoiceSearchClicked" --%>
                                                            </td>
                                                        </tr>                                                     
                                                    </table>
                                                    </asp:Panel>
                                                    <telerik:RadWindow runat="server" ID="radWinInvoiceCompanyChoose" Skin="Office2007" VisibleOnPageLoad="false" VisibleStatusbar="false"
                                                        Modal="true" OffsetElementID="offsetElement" Top="30" Left="30" NavigateUrl="ChooseCompanyPopup.aspx"
                                                        Title="Choose company" Height="400px" Width="750px" OnClientClose="onClientChooseCompanyWindowClosed">
                                                    </telerik:RadWindow>
                                                    <asp:HiddenField ID="hiddenCompanyId" runat="server" Value="-1" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr style="height: 1px; background: url(images/separator-line.gif) repeat-x">
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                    
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                             <td align="left">
                                                <ul type="circle" style="margin-top: 0px;">
                                                   <li>
                                                        <asp:HyperLink runat="server" ID="hypUnpaidInvoice" Text="Unpaid invoices" Font-Underline="true"
                                                             NavigateUrl="javascript:void();" 
                                                            onclick="return OnUpaidInvoiceClick();"></asp:HyperLink></li>     
                                                   <li>
                                                        <asp:HyperLink runat="server" ID="hypFutureInvoice" Text="Future invoices" Font-Underline="true"
                                                             NavigateUrl="javascript:void();" 
                                                            onclick="return OnFutureInvoiceClick();"></asp:HyperLink></li>     
                                                   <li>
                                                        <asp:HyperLink runat="server" ID="hypTurnover" Text="Turnover" Font-Underline="true"
                                                             NavigateUrl="javascript:void();" 
                                                            onclick="return OnTurnoverInvoiceClick();"></asp:HyperLink></li>                                              
                                                </ul>
                                             </td>
                                        </tr>
                                    </table>
                                </div>
                            </telerik:RadPageView>
                            <telerik:RadPageView runat="server" ID="NotificationView">
                                <div class="qsfexHeader" style=" width: 100%; ">
                                    <div style="text-align: left;">
                                        <asp:Label ID="lblHomeView" runat="server" Text="Notification"></asp:Label>
                                    </div>
                                </div>
                                <div style="margin-top:0px;overflow:auto; overflow-x: hidden" id="notificationPageContent">
                                    <div style="margin-left: 15px; margin-top:20px;"><asp:HyperLink runat="server" ID="lnkUnreadMessage" NavigateUrl="javascript:void();" onclick="return onViewMessage_Click();" Font-Underline="false"></asp:HyperLink></div>
                                    <div style="margin-left: 15px;"><asp:LinkButton runat="server" ID="lbnSignOut"  OnClick="OnButtonLogout_Click" Font-Underline="false"></asp:LinkButton></div>
                                </div>
                            </telerik:RadPageView>
                        </telerik:RadMultiPage>
                    </div>
                    <div id="qsfexSeparator" style="text-align: center;">
                        <img src="images/separator-img.gif" style="border: none" />
                    </div>
                    <div  id="funcTab">
                        <telerik:RadPanelBar runat="server" ID="SectionPanel" Skin="Office2007" 
                            Width="100%" ExpandMode="SingleExpandedItem" OnClientItemClicking="OnClientItemClicking"
                            >
                            <Items>
                                <telerik:RadPanelItem Text="Candidates" Value="candidates" ImageUrl="images/24x24/candidate.png" >
                                </telerik:RadPanelItem>
                                <telerik:RadPanelItem Text="Companies" Value="companies" ImageUrl="images/24x24/company.png" >
                                </telerik:RadPanelItem>
                                <telerik:RadPanelItem Text="Actions" Value="actions" ImageUrl="images/24x24/action.png" >
                                </telerik:RadPanelItem>
                                <telerik:RadPanelItem Text="Jobs" Value="jobs" ImageUrl="images/24x24/job.png" >
                                </telerik:RadPanelItem>
                                <telerik:RadPanelItem Text="Statistics" Value="statistics" ImageUrl="images/24x24/statistic.png" >
                                </telerik:RadPanelItem>
                                <telerik:RadPanelItem Text="Administration" Value="administration" ImageUrl="images/24x24/administration.png" >
                                </telerik:RadPanelItem>
                                <telerik:RadPanelItem Text="Invoicing" Value="invoicing" ImageUrl="images/24x24/invoicing.png"  >
                                </telerik:RadPanelItem>
                                <telerik:RadPanelItem Text="Notification" Value="notification" ImageUrl="images/24x24/message.png" >
                                </telerik:RadPanelItem>                 
                            </Items>
                        </telerik:RadPanelBar>
                    </div>
                    <telerik:RadDock runat="server" ID="RadDockLogout" Skin="Office2007" Title="Welcome" Resizable="true" 
                    DockMode="Floating" Top="92%" Left="75%" Width="250" BorderWidth="0">
                        <ContentTemplate>
                            <div style="text-align:left; margin:0px 10px 0px 10px; float:left">
                                <asp:Literal runat="server" ID="lblCurrentUser"></asp:Literal>
                            </div>
                            <div style="float:right; margin-right:5px;">
                                <asp:LinkButton runat="server" ID="lbnLogout" Text="Log out" OnClick="OnButtonLogout_Click"></asp:LinkButton>
                            </div>
                        </ContentTemplate>
                        <Commands>
                            <telerik:DockPinUnpinCommand />
                            <telerik:DockExpandCollapseCommand />
                        </Commands>
                    </telerik:RadDock>                       
                </telerik:RadPane>
                <telerik:RadSplitBar ID="RadSplitBar" runat="server" CollapseMode="Forward" BorderStyle="None"></telerik:RadSplitBar>                
                <telerik:RadPane ID="radPaneContent" runat="server" Width="100%" EnableAjaxSkinRendering="true" >
                    
                </telerik:RadPane>
            </telerik:RadSplitter>
        </div>
        
        <div>
            <asp:HiddenField runat="server" ID="ConfirmDeleteCandidate" />
            <asp:HiddenField runat="server" ID="ConfirmDeleteCompany" />
            <asp:HiddenField runat="server" ID="ConfirmDeleteAction" />
            <asp:HiddenField runat="server" ID="ConfirmDeleteJob" />
            <asp:HiddenField runat="server" ID="ConfirmDeleteInvoice" />
            
            <asp:HiddenField runat="server" ID="flagUnreadMsg" Value="0"/>
        </div>
    </form>
</body>
</html>