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
using System.Data.SqlTypes;
using Telerik.Web.UI;

public partial class Jobs : System.Web.UI.Page
{
    PageStatePersister _pers;
    protected override PageStatePersister PageStatePersister
    {
        get
        {
            if (_pers == null)
            {
                _pers = new SessionPageStatePersister(this);
            }
            return _pers;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (SessionManager.CurrentUser == null)
        {
            Common.RedirectToLoginPage(this);
            return;
        }
        else if (!IsPostBack)
        {
            InitControls();
            FillLabelLanguage();
            BindData();
            SessionManager.BackUrl = Request.Url.ToString();
        }
    }

    private void InitControls()
    {
        HttpCookie jobGridPageSizeCookie = Request.Cookies.Get("jobgrdps");
        if (jobGridPageSizeCookie != null)
        {
            if (!string.IsNullOrEmpty(jobGridPageSizeCookie.Value))
                gridJobs.PageSize = Convert.ToInt32(jobGridPageSizeCookie.Value) > 0 ? Convert.ToInt32(jobGridPageSizeCookie.Value) : gridJobs.PageSize;
        }
    }



    private void FillLabelLanguage()
    {
        lblJobTitle.Text = ResourceManager.GetString("lblRightPaneJobTitle");
        lnkAddJob.Text = ResourceManager.GetString("lblAddNewJob");

        gridJobs.Columns[1].HeaderText = ResourceManager.GetString("lblJobTitle");
        gridJobs.Columns[2].HeaderText = ResourceManager.GetString("lblJobCompanyName");
        gridJobs.Columns[3].HeaderText = ResourceManager.GetString("lblJobLocation");
        gridJobs.Columns[4].HeaderText = ResourceManager.GetString("lblJobVisits");
        gridJobs.Columns[5].HeaderText = ResourceManager.GetString("lblJobCreatedDate");
        gridJobs.Columns[6].HeaderText = ResourceManager.GetString("lblJobExpiredDate");

        lblExpiredDate.Text = ResourceManager.GetString("lblJobExpiredDate");
        lblRemindDate.Text = ResourceManager.GetString("lblJobRemindDate");
        btnActivate.Text = ResourceManager.GetString("btnJobActivate");
        btnDeactivate.Text = ResourceManager.GetString("btnJobDeactivate");
        btnBatchUpdate.Text = ResourceManager.GetString("btnJobBatchUpdate");
    }

    private void BindData()
    {
        gridJobs.DataSource = DoSearch();
        gridJobs.DataBind();
    }


    private List<Job> DoSearch()
    {
        List<Job> list = new List<Job>();
        string mode = "recent";
        if (!string.IsNullOrEmpty(Request.QueryString["mode"]))
        {
            mode = Request.QueryString["mode"];
        }
        if (mode == "recent")
        {
            list = new JobRepository().GetTopJobs(gridJobs.PageSize);
            return list;
        }
        JobSearchCriteria criteria = new JobSearchCriteria();
        switch (mode)
        {
            case "active":
                criteria.Active = "Yes";
                break;
            case "inactive":
                criteria.Active = "No";
                break;
            case "all":
                break;
            case "search":
                if (!string.IsNullOrEmpty(Request.QueryString["title"]))
                    criteria.Title = Request.QueryString["title"];
                if (!string.IsNullOrEmpty(Request.QueryString["active"]))
                    criteria.Active = Request.QueryString["active"];
                if (!string.IsNullOrEmpty(Request.QueryString["createdMin"]))
                    criteria.CreatedDateFrom = DateTime.ParseExact(Request.QueryString["createdMin"], "dd/MM/yyyy",
                                System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat);
                if (!string.IsNullOrEmpty(Request.QueryString["createdMax"]))
                    criteria.CreatedDateTo = DateTime.ParseExact(Request.QueryString["createdMax"], "dd/MM/yyyy",
                                System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat);
                if (!string.IsNullOrEmpty(Request.QueryString["activatedMin"]))
                    criteria.ActivatedDateFrom = DateTime.ParseExact(Request.QueryString["activatedMin"], "dd/MM/yyyy",
                                System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat);
                if (!string.IsNullOrEmpty(Request.QueryString["activatedMax"]))
                    criteria.ActivatedDateTo = DateTime.ParseExact(Request.QueryString["activatedMax"], "dd/MM/yyyy",
                                System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat);
                if (!string.IsNullOrEmpty(Request.QueryString["expiredMin"]))
                    criteria.ExpiredDateFrom = DateTime.ParseExact(Request.QueryString["expiredMin"], "dd/MM/yyyy",
                                System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat);
                if (!string.IsNullOrEmpty(Request.QueryString["expiredMax"]))
                    criteria.ExpiredDateTo = DateTime.ParseExact(Request.QueryString["expiredMax"], "dd/MM/yyyy",
                                System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat);

                if (!string.IsNullOrEmpty(Request.QueryString["profile"]))
                    criteria.ProfileID = int.Parse(Request.QueryString["profile"]);
                if (!string.IsNullOrEmpty(Request.QueryString["functionFam"]))
                    criteria.FunctionFam = Request.QueryString["functionFam"];
                if (!string.IsNullOrEmpty(Request.QueryString["location"]))
                    criteria.Locations = Request.QueryString["location"].Split(';');
                if (!string.IsNullOrEmpty(Request.QueryString["responsible"]))
                    criteria.Responsible = Request.QueryString["responsible"];
                if (!string.IsNullOrEmpty(Request.QueryString["company"]))
                    criteria.ComName = Request.QueryString["company"];
                break;
        }
        list = new JobRepository().SearchJobs(criteria);

        return list;
    }



    #region Grid Jobs events
    protected void OnJobGrid_PageSizeChanged(object source, GridPageSizeChangedEventArgs e)
    {
        HttpCookie jobGridPageSizeCookie = new HttpCookie("jobgrdps");
        jobGridPageSizeCookie.Expires.AddDays(30);
        jobGridPageSizeCookie.Value = e.NewPageSize.ToString();
        Response.Cookies.Add(jobGridPageSizeCookie);
    }

    protected void OnGridJobs_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "delete")
        {
            int jobID = Int32.Parse(e.CommandArgument.ToString());
            JobRepository jobRepo = new JobRepository();
            jobRepo.Delete(new Job(jobID));

            gridJobs.Rebind();
        }
    }

    protected void OnGridJobs_PageIndexChanged(object source, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        gridJobs.CurrentPageIndex = e.NewPageIndex;
        BindData();
    }

    protected void OnGridJobs_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        gridJobs.DataSource = DoSearch();
    }

    protected void OnGridJobs_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            if (dataItem != null)
            {
                ImageButton delButton = dataItem["DeleteColumn"].Controls[0] as ImageButton;
                delButton.Attributes["onclick"] = string.Format("return confirm(\"{0}\")", ResourceManager.GetString("confirmDeleteJob"));
                delButton.CommandArgument = ((Job)e.Item.DataItem).JobID.ToString();
            }
        }
    }
    #endregion

    #region Batch update event
    protected void OnBtnBatchUpdateClicked(object sender, EventArgs e)
    {
        GridItemCollection col = gridJobs.SelectedItems;
        JobRepository repo = new JobRepository();
        bool updated = false;
        foreach (GridDataItem item in col)
        {
            TableCell cell = item["JobID"];
            if (!string.IsNullOrEmpty(cell.Text))
            {
                int jobId = int.Parse(cell.Text);
                Job job = repo.FindOne(new Job(jobId));
                if (job != null)
                {
                    job.ExpiredDate = calExpiredDate.SelectedDate;
                    job.RemindDate = calRemindDate.SelectedDate;
                    repo.Update(job);
                    updated = true;
                }
            }
        }
        if (updated)
        {
            calExpiredDate.SelectedDate = null;
            calRemindDate.SelectedDate = null;
            gridJobs.Rebind();
        }
    }

    protected void OnBtnBatchActivateClicked(object sender, EventArgs e)
    {
        GridItemCollection col = gridJobs.SelectedItems;
        JobRepository repo = new JobRepository();
        bool updated = false;
        foreach (GridDataItem item in col)
        {
            TableCell cell = item["JobID"];
            if (!string.IsNullOrEmpty(cell.Text))
            {
                int jobId = int.Parse(cell.Text);
                Job job = repo.FindOne(new Job(jobId));
                if (job != null)
                {
                    job.IsActive = true;
                    repo.Update(job);
                    updated = true;
                }
            }
        }
        if (updated)
        {
            //calExpiredDate.SelectedDate = null;
            //calRemindDate.SelectedDate = null;
            gridJobs.Rebind();
        }
    }

    protected void OnBtnBatchDeactivateClicked(object sender, EventArgs e)
    {
        GridItemCollection col = gridJobs.SelectedItems;
        JobRepository repo = new JobRepository();
        bool updated = false;
        foreach (GridDataItem item in col)
        {
            TableCell cell = item["JobID"];
            if (!string.IsNullOrEmpty(cell.Text))
            {
                int jobId = int.Parse(cell.Text);
                Job job = repo.FindOne(new Job(jobId));
                if (job != null)
                {
                    job.IsActive = false;
                    repo.Update(job);
                    updated = true;
                }
            }
        }
        if (updated)
        {
            //calExpiredDate.SelectedDate = null;
            //calRemindDate.SelectedDate = null;
            gridJobs.Rebind();
        }
    }
    #endregion
    #region ajax event
    protected void JobAjaxManager_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.IndexOf("OpenSelectedJob") > -1)
        {
            if (gridJobs.SelectedItems.Count == 1)
            {
                Response.Redirect(string.Format("~/JobProfile.aspx?JobId={0}&mode=edit", GetSelectedJobID()), true);
            }
        }
        else if (e.Argument.IndexOf("DeleteSelectedJob") > -1)
        {
            if (gridJobs.SelectedItems.Count == 1)
            {
                JobAjaxManager.AjaxSettings.AddAjaxSetting(JobAjaxManager, gridJobs);
                JobRepository jobRepo = new JobRepository();
                jobRepo.Delete(new Job(GetSelectedJobID()));
                gridJobs.Rebind();
            }
        }
        else if (e.Argument.IndexOf("PreviewJob") > -1)
        {
            if (gridJobs.SelectedItems.Count == 1)
            {
                string script = string.Format("openPopUp('{0}')", WebConfig.NeosJobDetailURL + GetSelectedJobID());
                JobAjaxManager.ResponseScripts.Add(script);
                JobAjaxManager.ResponseScripts.Add("processJobToolBar(\"JobGridSelected\");");
            }
        }
    }

    private int GetSelectedJobID()
    {
        //
        GridDataItem selectedItem = gridJobs.SelectedItems[0] as GridDataItem;
        TableCell jobIDCell = selectedItem["JobID"];
        if (!string.IsNullOrEmpty(jobIDCell.Text))
            return Convert.ToInt32(jobIDCell.Text);
        return 0;
    }
    #endregion
}
