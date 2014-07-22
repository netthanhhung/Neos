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
using Telerik.Web.UI;
using System.Collections.Generic;

public partial class AdminUserAndPermission : System.Web.UI.Page
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
            FillLabelLanguage();
            BindGridData();
        }
    }

    private void BindGridData()
    {
        ParamUserRepository repo = new ParamUserRepository();
        gridUsers.DataSource = repo.GetAllUser(false);
    }

    private void FillLabelLanguage()
    {
        //lblUsers.Text = ResourceManager.GetString("lblUsersAdminUser");
        gridUsers.Columns[0].HeaderText = ResourceManager.GetString("columnUserUserID");
        gridUsers.Columns[1].HeaderText = ResourceManager.GetString("columnUserName");
        gridUsers.Columns[2].HeaderText = ResourceManager.GetString("columnUserGender");
        gridUsers.Columns[3].HeaderText = ResourceManager.GetString("columnUserEmail");
        gridUsers.Columns[4].HeaderText = ResourceManager.GetString("columnUserTelelphone");
        gridUsers.Columns[5].HeaderText = ResourceManager.GetString("columnUserActive");
        lnkAddNewUser.Text = ResourceManager.GetString("lblAddNewUser");

        lblPermission.Text = ResourceManager.GetString("lblUserPermissionAdminPermission");
        gridPermissions.Columns[0].HeaderText = ResourceManager.GetString("columnPermissionCode");
        gridPermissions.Columns[1].HeaderText = ResourceManager.GetString("columnPermissionDescription");        
    }
    #region event
    protected void OnUserDeleteClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        string userID = lnkItem.CommandArgument;

        //Check whether this user is being used
        //Delete user's permissions first.
        ParamUserPermissionRepository perRepo = new ParamUserPermissionRepository();
        IList<ParamUserPermission> oldPerList = perRepo.GetPermissionsOfUser(userID);
        foreach (ParamUserPermission deletePer in oldPerList)
        {
            perRepo.DeleteUserPermission(deletePer);
        }

        //Delete user        
        ParamUser deleteItem = new ParamUser(userID);
        ParamUserRepository repo = new ParamUserRepository();
        repo.Delete(deleteItem);

        BindGridData();
        gridUsers.DataBind();
    }

    protected void OnGridUserPageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        gridUsers.CurrentPageIndex = e.NewPageIndex;
        BindGridData();
        gridUsers.DataBind();
    }

    protected void OnGridUserNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        BindGridData();
    }

    protected void OnGridUserItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            LinkButton buttonDelete = dataItem["TemplateDeleteUserColumn"].Controls[1] as LinkButton;
            //buttonDelete.OnClientClick = "return confirm('" + ResourceManager.GetString("deleteConfirmText") + "')";
            buttonDelete.CommandArgument = ((ParamUser)e.Item.DataItem).UserID;
            buttonDelete.Text = ResourceManager.GetString("deleteText");
            int count = new ParamUserRepository().CountNumberBeingUsedOfUser(buttonDelete.CommandArgument);
            if (count > 0)
            {
                buttonDelete.OnClientClick = string.Format("javascript:alert('{0}'); return false;", ResourceManager.GetString("messageUserBeingUsed"));
            }
            else
            {
                buttonDelete.OnClientClick = "return confirm('" + ResourceManager.GetString("deleteConfirmText") + "')";
            }

            LinkButton buttonEdit = dataItem["TemplateEditUserColumn"].Controls[1] as LinkButton;
            buttonEdit.Text = ResourceManager.GetString("editText");
        }
        if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        {
            ParamUser user = e.Item.DataItem as ParamUser;
            if(user != null)
            {
                LinkButton lnkUserEdit = (LinkButton)e.Item.FindControl("lnkUserEdit");
                if (lnkUserEdit != null)
                {
                    lnkUserEdit.OnClientClick = string.Format("return OnUserEditClientClicked('{0}')", user.UserID);
                }
            }
        }
    }

    protected void OnGridPermissionNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        if (gridUsers.SelectedItems.Count == 1)
        {
            GridDataItem dataItem = ((GridDataItem)gridUsers.SelectedItems[0]);
            if (dataItem != null)
            {
                string userID = dataItem["UserID"].Text;
                IList<ParamUserPermission> perList =
                    new ParamUserPermissionRepository().GetPermissionsOfUser(userID);
                gridPermissions.DataSource = perList;
            }
        }
        else
        {
            gridPermissions.DataSource = new List<ParamUserPermission>();
        }
    }
    #endregion

    protected void OnMyAjaxManagerAjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.IndexOf("RebindPermissionGrid") != -1)
        {
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, gridPermissions);            
            string[] args = e.Argument.Split('/');
            if (args.Length == 3)
            {
                try
                {
                    //string userID = args[1];
                    int rowIndex = Int32.Parse(args[2]);
                    gridUsers.MasterTableView.ClearSelectedItems();
                    gridUsers.MasterTableView.Items[rowIndex - 1].Selected = true;
                }
                catch (Exception ex) { throw ex; }
                gridPermissions.Rebind();
            }
        }
        else if (e.Argument.IndexOf("RebindUserGrid") != -1)
        {
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, gridUsers);            
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, gridPermissions);
            gridUsers.Rebind();
            gridPermissions.Rebind();
        }
    }
}
