<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Messages.aspx.cs" Inherits="Messages" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Messages</title>
    <link href="Styles/Neos.css" rel="stylesheet" />

    <script src="script/utils.js" type="text/javascript"></script>
    <telerik:RadScriptBlock runat="server" ID="scriptblock"></telerik:RadScriptBlock>

</head>
<body>

    <script type="text/javascript">
    function OnActionsClientSelectedIndexChanged(sender, eventArgs)
    {
       $find("MyAjaxManager").ajaxRequest("ExecuteAction");
    }
    function onMarkAsUnreadClick()
    {
        $find("MyAjaxManager").ajaxRequest("MarkAsUnread");
        return false;
    }
    function onMarkAsReadClick()
    {
        $find("MyAjaxManager").ajaxRequest("MarkAsRead");
        return false;
    }
    function onDeleteClientClick()
    {
        $find("MyAjaxManager").ajaxRequest("DeleteMessage");
        return false;
    }
    
    </script>

    <form id="form1" runat="server">
        <div>
            <telerik:RadScriptManager ID="ScriptManager" runat="server" />
            <div class="rightpane_title">
                <asp:Literal runat="server" ID="lblTitle" Text=""></asp:Literal></div>
            <div style="margin-top: 30px;">
                <telerik:RadAjaxManager  EnableAJAX="true" ID="MyAjaxManager" runat="server" OnAjaxRequest="OnMyAjaxManagerAjaxRequest">
                    <AjaxSettings>
                        <telerik:AjaxSetting AjaxControlID="MessageGrid">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="MessageGrid" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                    </AjaxSettings>
                </telerik:RadAjaxManager>
            </div>
            <div style="margin-bottom: 10px;">
                <asp:Button runat="server" ID="btnDelete" Text="Delete" CssClass="flatButton" OnClientClick="return onDeleteClientClick();" />&nbsp;
                <asp:Button runat="server" ID="btnMarkAsRead" Text="Mark as read"  CssClass="flatButton" OnClientClick="return onMarkAsReadClick();"></asp:Button>
                
                <telerik:RadComboBox runat="server" ID="ddlActions" Skin="Office2007" Width="120"
                    OnClientSelectedIndexChanged="OnActionsClientSelectedIndexChanged" Visible="false">
                    <ItemTemplate>                        
                        <asp:Label runat="server" ID="lblMoreActions" Text="More actions" ForeColor="gray"></asp:Label><br />
                        &nbsp;&nbsp;&nbsp;<asp:LinkButton runat="server" ID="lnkMarkAsRead" Text="Mark as read" CommandArgument="Read" Font-Underline="false" ForeColor="black" OnClientClick="return onMarkAsReadClick();"></asp:LinkButton><br />
                        &nbsp;&nbsp;&nbsp;<asp:LinkButton runat="server" ID="lnkMarkAsUnread" Text="Mark as unread" CommandArgument="Unread" Font-Underline="false" ForeColor="black" OnClientClick="return onMarkAsUnreadClick();"></asp:LinkButton><br />
                    </ItemTemplate>
                    <Items>
                    <telerik:RadComboBoxItem Text="More actions" />
                    </Items>
                </telerik:RadComboBox>
            </div>
            <div>
                <telerik:RadGrid ID="MessageGrid" GridLines="None" Skin="Office2007" AllowMultiRowSelection="true"
                    EnableAjaxSkinRendering="true" runat="server" AllowPaging="True" AllowSorting="True"
                    PageSize="22" Width="100%" AutoGenerateColumns="false"
                    OnPageSizeChanged="OnMessageGrid_PageSizeChanged" OnNeedDataSource="OnMessageGridNeedDataSource"
                    OnItemDataBound="OnMessageGridItemDataBound" OnPageIndexChanged="OnMessageGridPageIndexChanged">
                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                    <MasterTableView DataKeyNames="MessageID" DataMember="UserMessage" AllowMultiColumnSorting="False"
                        Width="100%" EditMode="PopUp">
                        <Columns>
                            <telerik:GridBoundColumn DataField="MessageID" UniqueName="MessageID" Display="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridClientSelectColumn ButtonType="PushButton" CommandArgument='MessageID'
                                CommandName="SelectMessage">
                                <HeaderStyle Width="5" />
                            </telerik:GridClientSelectColumn>
                            <telerik:GridBoundColumn UniqueName="Subject" SortExpression="Subject" HeaderText="Subject"
                                DataField="Subject">
                                <HeaderStyle></HeaderStyle>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="MessageContent" SortExpression="MessageContent"
                                HeaderText="Message Content" DataField="MessageContent">
                                <HeaderStyle></HeaderStyle>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="CreatedDate" DataField="CreatedDate" SortExpression="CreatedDate"
                                HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy hh:mm tt}">
                                <HeaderStyle HorizontalAlign="center" Width="180" />
                                <ItemStyle HorizontalAlign="Center" />
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings>
                        <Selecting AllowRowSelect="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </div>
        </div>
    </form>
</body>
</html>
