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
using Telerik.Web.UI;

public partial class ChooseCompanyPopup : System.Web.UI.Page
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

            IList<Company> comList = new List<Company>();
            gridCompany.DataSource = comList;
            gridCompany.DataBind();
            txtName.Focus();            
        }
    }

    private void FillLabelLanguage()
    {
        this.Page.Title = ResourceManager.GetString("chooseCompanyPopupTitle");
        lblName.Text = ResourceManager.GetString("lblNameCom");
        btnSearchCompanies.Text = ResourceManager.GetString("btnCompanySearch");
        btnOK.Text = ResourceManager.GetString("okText");
        btnCancel.Text = ResourceManager.GetString("cancelText");

        gridCompany.Columns[0].HeaderText = ResourceManager.GetString("lblCompanyName");
        gridCompany.Columns[1].HeaderText = ResourceManager.GetString("lblCity");
        gridCompany.Columns[2].HeaderText = ResourceManager.GetString("lblType");
        gridCompany.Columns[3].HeaderText = ResourceManager.GetString("lblNeosResp");
        gridCompany.Columns[4].HeaderText = ResourceManager.GetString("columnCreatedDateAttachedDocPresent");
    }

    protected void OnCompanySearchClicked(object sender, EventArgs e)
    {
        //if (!string.IsNullOrEmpty(txtName.Text))
        //{
        //    IList<Company> comList = new CompanyRepository().FindByName(txtName.Text);
        //    //comList = FillContactInfo(canList);
        //    gridCompany.DataSource = comList;
        //    gridCompany.DataBind();
        //    if (comList.Count > 0)
        //        gridCompany.Items[0].Selected = true;
        //}
        BindCompanyGrid(null);
        gridCompany.DataBind();
        if (gridCompany.Items.Count > 0)
            gridCompany.Items[0].Selected = true;
    }

    
    protected void OnBtnOkClicked(object sender, EventArgs e)
    {
        if (gridCompany.SelectedValue != null)
        {
            int companyID = (int)gridCompany.SelectedValue;
            Company selectedCompany = new CompanyRepository().FindOne(companyID);
            string argument = selectedCompany.CompanyID.ToString() + "/"
                + selectedCompany.CompanyName;
            string script = "<script type=\"text/javascript\">";
            script += " OnBtnOkClientClicked(\"" + argument + "\");";
            script += " </script>";

            if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
                ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script);
        }
    }

    protected void OnGridCompanyPageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        gridCompany.CurrentPageIndex = e.NewPageIndex;
        BindCompanyGrid(null);
        gridCompany.DataBind();
    }

    private void BindCompanyGrid(GridSortCommandEventArgs sortEventArgs)
    {
        if (!string.IsNullOrEmpty(txtName.Text))
        {
            int pageSize = 10;
            int pageNumber = gridCompany.CurrentPageIndex + 1;
            string sortExpress = string.Empty;
            string sortExpressInvert = string.Empty;
            foreach (GridSortExpression item in gridCompany.MasterTableView.SortExpressions)
            {
                GridSortOrder newSortOrder = item.SortOrder;
                if (sortEventArgs != null && item.FieldName == sortEventArgs.SortExpression)
                {
                    newSortOrder = sortEventArgs.NewSortOrder;
                }

                if (!string.IsNullOrEmpty(sortExpress) && newSortOrder != GridSortOrder.None)
                {
                    sortExpress += ", ";
                    sortExpressInvert += ", ";
                }
                if (newSortOrder == GridSortOrder.Ascending)
                {
                    sortExpress += item.FieldName + " ASC";
                    sortExpressInvert += item.FieldName + " DESC";
                }
                else if (newSortOrder == GridSortOrder.Descending)
                {
                    sortExpress += item.FieldName + " DESC";
                    sortExpressInvert += item.FieldName + " ASC";
                }
            }

            if (sortEventArgs != null && !sortExpress.Contains(sortEventArgs.SortExpression))
            {
                if (!string.IsNullOrEmpty(sortExpress) && sortEventArgs.NewSortOrder != GridSortOrder.None)
                {
                    sortExpress += ", ";
                    sortExpressInvert += ", ";
                }
                if (sortEventArgs.NewSortOrder == GridSortOrder.Ascending)
                {
                    sortExpress += sortEventArgs.SortExpression + " ASC";
                    sortExpressInvert += sortEventArgs.SortExpression + " DESC";
                }
                else if (sortEventArgs.NewSortOrder == GridSortOrder.Descending)
                {
                    sortExpress += sortEventArgs.SortExpression + " DESC";
                    sortExpressInvert += sortEventArgs.SortExpression + " ASC";
                }
            }

            if (!string.IsNullOrEmpty(sortExpress))
            {
                if (sortExpress.Contains("CompanyName"))
                {
                    sortExpress = sortExpress.Replace("CompanyName", "SocNom");
                    sortExpressInvert = sortExpressInvert.Replace("CompanyName", "SocNom");
                }
                if (sortExpress.Contains("City"))
                {
                    sortExpress = sortExpress.Replace("City", "Commune");
                    sortExpressInvert = sortExpressInvert.Replace("City", "Commune");
                }
                if (sortExpress.Contains("StatusLabel"))
                {
                    sortExpress = sortExpress.Replace("StatusLabel", "StatusLabel");
                    sortExpressInvert = sortExpressInvert.Replace("StatusLabel", "StatusLabel");
                }
                if (sortExpress.Contains("Responsible"))
                {
                    sortExpress = sortExpress.Replace("Responsible", "Responsable");
                    sortExpressInvert = sortExpressInvert.Replace("Responsible", "Responsable");
                }
                if (sortExpress.Contains("CreatedDate"))
                {
                    sortExpress = sortExpress.Replace("CreatedDate", "DateCreation");
                    sortExpressInvert = sortExpressInvert.Replace("CreatedDate", "DateCreation");
                }
            }
            else
            {
                sortExpress = "SocNom ASC";
                sortExpressInvert = "SocNom DESC";
            }

            CompanyRepository companyRepo = new CompanyRepository();
            gridCompany.VirtualItemCount = companyRepo.CountSearchCompaniesByName(txtName.Text);

            gridCompany.DataSource = companyRepo.FindByName(txtName.Text,
                pageSize, pageNumber, sortExpress, sortExpressInvert);
        }
        else
        {
            IList<Company> comList = new List<Company>();
            gridCompany.DataSource = comList;
        }
    }

    protected void OnGridCompanyNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        BindCompanyGrid(null);
    }

    protected void OnGridCompanySortCommand(object source, GridSortCommandEventArgs e)
    {
        BindCompanyGrid(e);
    }
}
