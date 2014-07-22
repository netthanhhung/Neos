<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CandidateCVSearchResult.aspx.cs"
    ValidateRequest="false" Inherits="CandidateCVSearchResult" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Search</title>
    <link href="Styles/Neos.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ValidationSummary runat="server" ID="sumValidation" ShowMessageBox="true" ShowSummary="false"
            ValidationGroup="CandidateCVSearch" />
        <asp:RequiredFieldValidator runat="server" ID="rfvKeyWord" ControlToValidate="txtKeyword"
            Display="None" ValidationGroup="CandidateCVSearch" ErrorMessage="Enter a keyword"></asp:RequiredFieldValidator>
        <div>
            <div style="margin-bottom: 10px">
                <asp:Literal runat="server" ID="lblKeyword" Text="Keyword"></asp:Literal>&nbsp;
                <asp:TextBox runat="server" ID="txtKeyword" Width="300"></asp:TextBox>&nbsp;
                <asp:Button ID="btnSearchCandidateCV" runat="server" Text="Search" CssClass="flatButton"
                    ValidationGroup="CandidateCVSearch" OnClick="OnButtonSearchCandidateCV_Click"></asp:Button>
            </div>
            <div class="document_search_result_title">
                <table style="width: 100%">
                    <tr>
                        <td style="text-align: left; width: 20%">
                            <asp:Literal runat="server" ID="lblCandidateCV" Text="Candidate's curriculum vitae"></asp:Literal></td>
                        <td style="text-align: right; width: 80%">
                            <asp:Literal runat="server" ID="lblResultTitle" Text="Results {0} of {1} for <b>{2}</b>"></asp:Literal></td>
                    </tr>
                </table>
            </div>
            <div style="width: 100%;">
                <asp:GridView runat="server" ID="GridCandidateDocument" AllowPaging="true" PageSize="6"
                    AutoGenerateColumns="false" GridLines="None" OnRowDataBound="OnGridCandidateDocument_RowDataBound"
                    OnPageIndexChanging="OnGridCandidateDocument_PageIndexChanging">
                    <PagerStyle HorizontalAlign="Center" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <div class="document_search_result">
                                    <div>
                                        <asp:HyperLink runat="server" ID="lnkCandidateID"></asp:HyperLink></div>
                                    <div>
                                        <asp:Literal runat="server" ID="lblCandidateContent"></asp:Literal></div>
                                    <div>
                                        <asp:HyperLink runat="server" ID="lnkCandidateCV"></asp:HyperLink></div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </form>
</body>
</html>
