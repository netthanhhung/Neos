using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using Neos.Data;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void OnPanelBarItemClicked(object sender, Telerik.Web.UI.RadPanelBarEventArgs e)
    {
        RadPanelItem itemClicked = e.Item;
        if (itemClicked.Selected)
        {
            if (itemClicked.Level == 0 && itemClicked.Index == 0)
            {
                IList<Candidate> last5Candidates = NeosDAO.GetLastModifCandidates(5);
                Repeater lastFiveList = (Repeater)RadPanelBar2.Items[0].Items[0].FindControl("lastFiveList");                
                lastFiveList.DataSource = last5Candidates;
                lastFiveList.DataBind();
            }
        }
    }

    protected void OnCandidateSearchClicked(object sender, EventArgs e)
    {                
        string lastname = ((TextBox) RadPanelBar2.Items[0].Items[0].FindControl("txtLastNameSearch")).Text;         
        radPaneContent.ContentUrl = "~/Candidates.aspx?lastname=" + lastname;
    }
    protected void OnLastFiveListItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            Candidate dataItem = e.Item.DataItem as Candidate;
            if (dataItem != null)
            {
                LinkButton lnkItem = (LinkButton)e.Item.FindControl("lnkLastFiveItem");
                if (lnkItem != null)
                {
                    lnkItem.Text = dataItem.FirstName + " " + dataItem.LastName;
                    lnkItem.CommandArgument = dataItem.CandidateId.ToString();
                }

            }
        }
    }

    public void OnLast5CandidateItemClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        string id = lnkItem.CommandArgument;
        SessionManager.LastNameSearchCriteria = null;
        radPaneContent.ContentUrl = "~/CandidateProfile.aspx?CandidateID=" + id;
    }
 

}
