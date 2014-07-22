<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register TagPrefix="telerik" Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head id="NeosMainHead" runat="server">
	<title>Neos main Page</title>	 
</head>
<body>
<form id="mainForm" method="post" runat="server">        
    <telerik:RadScriptManager ID="ScriptManager" runat="server" />          
        <telerik:RadSplitter ID="RadSplitterBrowser" runat="server" Height="100%" Width="100%" BorderSize="1">
            <telerik:RadPane ID="RadPanePanelBar" runat="server" Scrolling="None" Height="100%" Width="280px">
                <telerik:RadPanelBar runat="server" ID="RadPanelBar2" Skin="Outlook" Height="700px" Width="100%" 
                ExpandMode="SingleExpandedItem" OnItemClick="OnPanelBarItemClicked">
                    <Items>
                        <telerik:RadPanelItem Text="Candidates">
                            <Items>
                                <telerik:RadPanelItem>
                                    <ItemTemplate>                                        
                                         <div>                                                
                                            <asp:TextBox ID="txtLastNameSearch" runat="server" Text="Last Name"></asp:TextBox>
                                            <asp:Button ID="btnSearchCandidates" runat="server" Text="Search" OnClick="OnCandidateSearchClicked"></asp:Button>
                                            <br />
                                            <asp:Literal ID="litLastFive" runat="server" Text="Last 5 viewed candidates :"></asp:Literal>
                                            <br />
                                            <asp:Repeater ID="lastFiveList" runat="server" OnItemDataBound="OnLastFiveListItemDataBound">
                                                <HeaderTemplate>                                                            
                                                </HeaderTemplate>
                                                <ItemTemplate>                                                           
                                                            <img  src="Images/person.jpg" width="20" height="20" alt="text">                                                          
                                                            <asp:LinkButton ID="lnkLastFiveItem" runat="server" OnClick="OnLast5CandidateItemClicked"/>                                                            
                                                            <br />                                                                                                                 
                                                </ItemTemplate>
                                                <FooterTemplate>                                                    
                                                </FooterTemplate>
                                             </asp:Repeater>  
                                        </div>                                        
                                    </ItemTemplate>
                                </telerik:RadPanelItem>
                            </Items>
                        </telerik:RadPanelItem>
                        <telerik:RadPanelItem Text="Companies">
                            <Items>
                                <telerik:RadPanelItem>
                                    <ItemTemplate>
                                        <telerik:RadCalendar runat="server" ID="Calendar1" Skin="Outlook" style="margin: 30px auto 0" />
                                    </ItemTemplate>
                                </telerik:RadPanelItem>
                            </Items>
                        </telerik:RadPanelItem>
                        <telerik:RadPanelItem Text="Actions">
                            <Items>
                            <telerik:RadPanelItem Text="My Contacts"></telerik:RadPanelItem>
                            <telerik:RadPanelItem Text="Address Cards"></telerik:RadPanelItem>
                            <telerik:RadPanelItem Text="Phone List"></telerik:RadPanelItem>                        
                            </Items>
                        </telerik:RadPanelItem>
                        <telerik:RadPanelItem Text="Jobs">                                                          
                        </telerik:RadPanelItem>
                        <telerik:RadPanelItem Text="Statistics">
                        </telerik:RadPanelItem>
                        <telerik:RadPanelItem Text="Administration">
                        </telerik:RadPanelItem>
                        <telerik:RadPanelItem Text="Invoicing">
                        </telerik:RadPanelItem>
                    </Items>
                </telerik:RadPanelBar>                  				    				    
            </telerik:RadPane>
            <telerik:RadSplitBar ID="RadSplitBar" runat="server" CollapseMode="Forward"></telerik:RadSplitBar>
            <telerik:RadPane ID="radPaneContent" runat="server" Height="700px">					
            </telerik:RadPane>				                
        </telerik:RadSplitter>                       	
</form>
</body>
</html>
