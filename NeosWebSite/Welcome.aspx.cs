using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Welcome : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillLabelsText();
            InitControls();
        }
    }

    private void InitControls()
    {
        btnStartNeos.OnClientClick = string.Format("javascript:return openNeosWindow('{0}');", "Login.aspx");
    }

    private void FillLabelsText()
    {
        lblWelcomeMessage.Text = ResourceManager.GetString("lblWecomeMessage");
        lblNotificationMessage.Text = ResourceManager.GetString("lblWecomeNotificationMessage");
        btnStartNeos.Text = ResourceManager.GetString("lblStartNeosProject");
    }
}
