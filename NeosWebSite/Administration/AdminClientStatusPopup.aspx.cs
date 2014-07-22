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
using Neos.Data;
using System.Collections.Generic;

public partial class AdminClientStatusPopup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (SessionManager.CurrentUser == null)
        {
            Common.RedirectToLoginPage(this);
            return;
        }
        else if (!IsPostBack)
        {
            FillLabelLanguage();
            if (!string.IsNullOrEmpty(Request.QueryString["StatusID"]))
            {
                int statusID = int.Parse(Request.QueryString["StatusID"]);
                ParamClientStatus clientStatus = new ParamClientStatusRepository().FindOne(new ParamClientStatus(statusID));
                txtClientStatus.Text = clientStatus.Status;                
            }
        }
    }

    private void FillLabelLanguage()
    {
        
    }

    protected void OnBtnSaveClicked(object sender, EventArgs e)
    {
        ParamClientStatusRepository repo = new ParamClientStatusRepository();

        ParamClientStatus saveItem = new ParamClientStatus();
        saveItem.Status = txtClientStatus.Text.Trim();        
                
        if (string.IsNullOrEmpty(Request.QueryString["StatusID"]))
        {
            IList<ParamClientStatus> oldList = repo.GetAllClientStatuses(saveItem.Status);
            if (oldList.Count > 0)
            {
                string message = ResourceManager.GetString("itemAlreadyExist");
                string script1 = "<script type=\"text/javascript\">";
                script1 += " alert(\"" + message + "\");";
                script1 += " </script>";

                if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
                    ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script1);
            }
            else
            {
                IList<ParamClientStatus> allItems = repo.GetAllClientStatuses();
                int statusID = 1;
                if (allItems.Count > 0)
                {
                    statusID = allItems[allItems.Count - 1].StatusID + 1;
                }
                saveItem.StatusID = statusID;
                repo.Insert(saveItem);
            }
        }
        else
        {
            saveItem.StatusID = int.Parse(Request.QueryString["StatusID"]);
            repo.Update(saveItem);
        }
        
        string script = "<script type=\"text/javascript\">";
        script += " OnBtnSaveClientClicked();";
        script += " </script>";

        if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
            ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script);
    }
}
