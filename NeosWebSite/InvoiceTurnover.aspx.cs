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

public partial class InvoiceTurnover : System.Web.UI.Page
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
            if (!HavePermission())
            {
                TurnOverMView.ActiveViewIndex = 1;
                return;
            }
        }
    }

    private bool HavePermission()
    {
        bool viewInvoicingPermission = false;
        if (SessionManager.CurrentUser != null && SessionManager.CurrentUser.Permissions != null)
        {
            foreach (ParamUserPermission item in SessionManager.CurrentUser.Permissions)
            {
                if (item.PermissionCode == "INVOICING")
                {
                    viewInvoicingPermission = true;
                    break;
                }
            }
        }

        return viewInvoicingPermission;
    }

    private void FillLabelLanguage()
    {
        lblTitle.Text = ResourceManager.GetString("hypTurnover");
        lblTurnover.Text = ResourceManager.GetString("hypTurnover");
        lblStartDate.Text = ResourceManager.GetString("lblTurnoverStartDate");
        lblEndDate.Text = ResourceManager.GetString("lblTurnoverEndDate");
        lblResult.Text = ResourceManager.GetString("lblTurnoverAmount");
        btnGo.Text = ResourceManager.GetString("goText");

        lblNoPermission.Text = ResourceManager.GetString("lblNoPermissionToViewInvoices"); 
  //      <string name="">Start date :</string>
  //<string name="">Start date :</string>
  //<string name="">Turnover (€) :</string>
    }

    protected void OnBtnGoClicked(object sender, EventArgs e)
    {
        if (!datStartDate.SelectedDate.HasValue || !datEndDate.SelectedDate.HasValue)
        {
            string message = ResourceManager.GetString("messageInvalidDateTurnover");
            string script = "<script type=\"text/javascript\">";
            script += " alert(\"" + message + "\");";
            script += " </script>";

            if (!ClientScript.IsClientScriptBlockRegistered("ttt"))
                ClientScript.RegisterStartupScript(this.GetType(), "ttt", script);
        }
        else
        {
            InvoicesRepository repo = new InvoicesRepository();
            double? iAmount = repo.GetSumAmountNotVAT("I", datStartDate.SelectedDate, datEndDate.SelectedDate);
            double? cAmount = repo.GetSumAmountNotVAT("C", datStartDate.SelectedDate, datEndDate.SelectedDate);
            double result = (iAmount.HasValue ? iAmount.Value : 0) - (cAmount.HasValue ? cAmount.Value : 0);
            txtResult.Text = result.ToString();
        }
    }
}
