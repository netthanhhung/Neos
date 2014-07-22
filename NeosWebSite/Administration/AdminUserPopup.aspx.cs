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

public partial class AdminUserPopup : System.Web.UI.Page
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

            divChangePassword.Visible = false;
            ddlGenre.Items.Add(new RadComboBoxItem("M", "M"));
            ddlGenre.Items.Add(new RadComboBoxItem("F", "F"));

            ddlPermission.DataTextField = "PermissionCode";
            ddlPermission.DataValueField = "PermissionCode";
            ddlPermission.DataSource = new ParamPermissionRepository().FindAll();
            ddlPermission.DataBind();

            if (!string.IsNullOrEmpty(Request.QueryString["UserID"]))
            {
                btnChangePassword.Visible = true;
                txtUserID.Enabled = false;
                txtPassword.Enabled = false;
                string userID = Request.QueryString["UserID"];
                ParamUser user = new ParamUserRepository().FindOne(new ParamUser(userID));
                txtUserID.Text = user.UserID;
                txtName.Text = user.LastName;
                ddlGenre.SelectedValue = user.Gender;
                txtEmail.Text = user.Email;
                txtTelephone.Text = user.Telephone;
                chkActive.Checked = user.Active;
                txtPassword.Attributes.Add("value", user.Password);
                txtPassword.Text = user.Password;
                //txtNewPassword.Attributes.Add("value", user.Password);
                //txtConfirmPassword.Attributes.Add("value", user.Password);                

                IList<ParamUserPermission> permissionList = 
                    new ParamUserPermissionRepository().GetPermissionsOfUser(userID);
                string perString = string.Empty;
                foreach (ParamUserPermission item in permissionList)
                {
                    listPermission.Items.Add(new ListItem(item.PermissionCode, item.PermissionCode));
                    perString += item.PermissionCode + ";";
                }
                if (!string.IsNullOrEmpty(perString))
                    perString = perString.TrimEnd(';');
                hiddenPermissionList.Value = perString;
            }
            else
            {
                txtUserID.Enabled = true;
                btnChangePassword.Visible = false;
                txtPassword.Enabled = true;
            }

            bool haveChangeUserPermission = false;
            if (SessionManager.CurrentUser != null && SessionManager.CurrentUser.Permissions != null)
            {               
                foreach (ParamUserPermission item in SessionManager.CurrentUser.Permissions)
                {
                    if (item.PermissionCode == "CHANGEUSERPERMISSIONS")
                    {
                        haveChangeUserPermission = true;
                        break;
                    }
                    
                }                
            }
            btnAddPermission.Enabled = haveChangeUserPermission;
            btnRemovePermission.Enabled = haveChangeUserPermission;
        }
    }

    private void FillLabelLanguage()
    {        
        lblUserID.Text = ResourceManager.GetString("columnUserUserID");
        lblName.Text = ResourceManager.GetString("columnUserName");
        lblGenre.Text = ResourceManager.GetString("columnUserGender");
        lblEmail.Text = ResourceManager.GetString("columnUserEmail");
        lblTelephone.Text = ResourceManager.GetString("columnUserTelelphone");
        lblActive.Text = ResourceManager.GetString("columnUserActive");        
        lblPermission.Text = ResourceManager.GetString("lblPermissionAdminPermission");
        btnAddPermission.Text = ResourceManager.GetString("addText");
        btnRemovePermission.Text = ResourceManager.GetString("removeText");
        btnSave.Text = ResourceManager.GetString("saveText");
        btnCancel.Text = ResourceManager.GetString("cancelText");  
        btnChangePassword.Text = ResourceManager.GetString("btnChangePassword");
        lblPassword.Text = ResourceManager.GetString("lblPassword");
        lblNewPassword.Text = ResourceManager.GetString("lblNewPassword");  
        lblConfirmPassword.Text = ResourceManager.GetString("lblConfirmPassword");          

        rfvAdminUser.ErrorMessage = ResourceManager.GetString("messageUserIDMustNotBeEmpty");
        revAdminUser.ErrorMessage = ResourceManager.GetString("messageUserEmailNotValid");
        cvAdminUser.ErrorMessage = ResourceManager.GetString("messageConfirmPassworNotValid");
    }

    protected void OnBtnChangePasswordClicked(object sender, EventArgs e)
    {
        divChangePassword.Visible = true;
    }

    protected void OnBtnSaveClicked(object sender, EventArgs e)
    {
        ParamUserRepository repo = new ParamUserRepository();
        if (string.IsNullOrEmpty(Request.QueryString["UserID"]))
        {
            ParamUser oldUser = repo.FindOne(new ParamUser(txtUserID.Text.Trim()));
            if (oldUser != null)
            {
                string message = ResourceManager.GetString("messageUserIDAlreadyExist");
                string scriptMes = "<script type=\"text/javascript\">";
                scriptMes += " alert(\"" + message + "\")";
                scriptMes += " </script>";

                if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
                    ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", scriptMes);
                return;
            }
        }        

        ParamUser saveItem = GetUser();
        if (string.IsNullOrEmpty(Request.QueryString["UserID"]))
        {
            //Insert new record
            repo.InserNewUser(saveItem);
        }
        else
        {
            //Update the record.
            //saveItem.UserID = Request.QueryString["UserID"];
            repo.Update(saveItem);
        }
        //Save permission.
        SavePermission(saveItem);
        string script = "<script type=\"text/javascript\">";
        script += " OnBtnSaveClientClicked();";
        script += " </script>";

        if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
            ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script);
    }

    private void SavePermission(ParamUser user)
    {
        ParamUserPermissionRepository perRepo = new ParamUserPermissionRepository();
        IList<ParamUserPermission> oldPerList = perRepo.GetPermissionsOfUser(user.UserID);

        string[] newPerArray = hiddenPermissionList.Value.Split(';');
        if (newPerArray != null && newPerArray.Length > 0)
        {
            for (int i = 0; i < newPerArray.Length; i++)
            {
                if (!string.IsNullOrEmpty(newPerArray[i]))
                {
                    bool isNew = true;
                    foreach (ParamUserPermission oldItem in oldPerList)
                    {
                        if (oldItem.PermissionCode.Trim() == newPerArray[i].Trim())
                        {
                            isNew = false;
                            break;
                        }
                    }
                    if (isNew)
                    {
                        ParamUserPermission newItem = new ParamUserPermission();
                        newItem.UserID = user.UserID;
                        newItem.PermissionCode = newPerArray[i].Trim();
                        perRepo.Insert(newItem);
                    }
                }
            }
        }

        foreach (ParamUserPermission oldItem in oldPerList)
        {
            bool isDelete = true;
            if (newPerArray != null && newPerArray.Length > 0)
            {
                for (int i = 0; i < newPerArray.Length; i++)
                {
                    if (oldItem.PermissionCode.Trim() == newPerArray[i].Trim())
                    {
                        isDelete = false;
                        break;
                    }
                }
            }
            if (isDelete)
                perRepo.DeleteUserPermission(oldItem);
        }
    }

    private ParamUser GetUser()
    {
        ParamUser saveItem = new ParamUser();
        saveItem.UserID = txtUserID.Text;
        saveItem.LastName = txtName.Text;
        saveItem.Gender = ddlGenre.SelectedValue;
        saveItem.Email = txtEmail.Text;
        saveItem.Telephone = txtTelephone.Text;
        saveItem.Active = chkActive.Checked;
        if (divChangePassword.Visible)
            saveItem.Password = txtNewPassword.Text;
        else
        {
            if (string.IsNullOrEmpty(Request.QueryString["UserID"]))
                saveItem.Password = txtPassword.Text;
            else
                saveItem.Password = txtPassword.Attributes["value"];
        }
        return saveItem;
    }
}
