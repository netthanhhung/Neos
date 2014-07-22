<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChooseCandidatePopup.aspx.cs" Inherits="ChooseCandidatePopup" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Choose candidate</title>
    <link href="Styles/Neos.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript">
        function GetRadWindow()
        {
          var oWindow = null;
          if (window.radWindow)
             oWindow = window.radWindow;
          else if (window.frameElement.radWindow)
             oWindow = window.frameElement.radWindow;
          return oWindow;
        }
        
        function OnBtnCancelClientClicked(sender, eventArgs)         
        {         
            var currentWindow = GetRadWindow();
            currentWindow.close();
        }
        
        function OnBtnOkClientClicked(argument)         
        {
            var currentWindow = GetRadWindow();
            currentWindow.argument = argument;
            currentWindow.close();
        }
    </script>
</head>
<body>
    <form id="chooseCandidatePopup" runat="server">
    <telerik:RadScriptManager ID="ScriptManager" runat="server" /> 
    <div>
        <telerik:RadAjaxManager ID="RadAjaxManager" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="btnSearchCandidates">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridCandidate"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridCandidate">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridCandidate"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

        <asp:Panel runat="server" ID="pnlSearchCandidates" DefaultButton="btnSearchCandidates">

         <table cellpadding="0" cellspacing="0" width="75%" style="margin-top: 0px;">
            <tr>
                <td>
                    <asp:Label ID="lblCanLastName" runat="server" Text="Last Name"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtCanLastName" runat="server" Width="145"></telerik:RadTextBox>
                </td>
                <td>
                    <asp:Label ID="lblCanFirstName" runat="server" Text="First Name"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtCanFirstName" runat="server" Width="145"></telerik:RadTextBox>
                </td>
                <td>
                    <asp:Button ID="btnSearchCandidates" runat="server" Text="Search" CssClass="flatButton" Width="60px"
                        OnClick="OnCandidateSearchClicked"></asp:Button>               
                </td>
            </tr>
         </table>
         </asp:Panel>
         <table cellpadding="0" cellspacing="0" width="100%" style="margin-top: 0px;">
            <tr>
                <td>
                    <div>                                                                          
                        <table style="width:100%">
                            <tr>
                                <td>
                                    <telerik:RadGrid ID="gridCandidate" GridLines="None" Skin="Office2007" AllowMultiRowSelection="false"
                                        runat="server" AllowPaging="True" AllowCustomPaging="true" AllowSorting="True"
                                        PageSize="10" Width="100%"  AutoGenerateColumns="false" EnableAjaxSkinRendering="true"
                                        OnPageIndexChanged="OnGridCandidatePageIndexChanged" 
                                        OnNeedDataSource="OnGridCandidateNeedDataSource"
                                        OnSortCommand="OnGridCandidateSortCommand">
                                        <PagerStyle Mode="NextPrevAndNumeric">
                                        </PagerStyle>
                                        <MasterTableView DataKeyNames="CandidateId" DataMember="Candidate" AllowMultiColumnSorting="True"
                                            Width="100%" EditMode="PopUp">                    
                                            <Columns>   
                                                <telerik:GridBoundColumn UniqueName="LastName" SortExpression="LastName" HeaderText="Last Name" DataField="LastName">
                                                    <HeaderStyle Width="20%"></HeaderStyle>
                                                </telerik:GridBoundColumn> 
                                                <telerik:GridBoundColumn UniqueName="FirstName" SortExpression="FirstName" HeaderText="First Name" DataField="FirstName">
                                                    <HeaderStyle Width="20%"></HeaderStyle>
                                                </telerik:GridBoundColumn> 
                                                
                                                <telerik:GridBoundColumn UniqueName="Status" SortExpression="Inactive" HeaderText="Status" DataField="InactiveString">
                                                    <HeaderStyle Width="10%"></HeaderStyle>
                                                </telerik:GridBoundColumn> 
                                                <telerik:GridBoundColumn UniqueName="ContactInfo" HeaderText="Contact info" DataField="ContactInfo" AllowSorting="false">
                                                    <HeaderStyle Width="35%"></HeaderStyle>
                                                </telerik:GridBoundColumn>                         
                                                
                                                <telerik:GridBoundColumn UniqueName="LastModifDate" SortExpression="LastModifDate" 
                                                    HeaderText="Last Modif." DataField="LastModifDate" DataType="system.DateTime" DataFormatString="{0:dd/MM/yyyy}">
                                                    <HeaderStyle Width="15%"></HeaderStyle>
                                                </telerik:GridBoundColumn>                       
                                            </Columns>

                                        </MasterTableView>
                                        <ClientSettings>
                                             <Selecting AllowRowSelect="true" />
                                        </ClientSettings>               
                                    </telerik:RadGrid>
                                </td>
                            </tr>                            
                        </table>                        
                    </div>
                </td>
            </tr>
        </table>
        <div style="text-align:center">
            <asp:Button runat="server" ID="btnOK" Text="OK" CssClass="flatButton" Width="60" OnClick="OnBtnOkClicked"/>&nbsp;
            <asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="flatButton" Width="60" OnClientClick="OnBtnCancelClientClicked()"/>
        </div>
    </div>
    </form>
</body>
</html>
