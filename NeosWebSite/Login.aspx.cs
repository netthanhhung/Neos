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

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillLabels();
            ParamUser cookieUser =  GetUserFromCookie();
            if (cookieUser != null)
            {
                txtUserID.Text = cookieUser.UserID;
                txtPassword.Attributes.Add("value", "******");
            }
        }

        string script = "<script type='text/javascript'>";
        script += "centerLoginControl();";
        script += "</script>";

        if (!ClientScript.IsClientScriptBlockRegistered("centerlizeControl"))
            ClientScript.RegisterStartupScript(this.GetType(), "centerlizeControl", script);

    }

    private void FillLabels()
    {
        lblSignIn.Text = ResourceManager.GetString("lblSignIn");
        lblUserName.Text = ResourceManager.GetString("lblUserName");
        lblPassword.Text = ResourceManager.GetString("lblPassword");
        chkRemember.Text = ResourceManager.GetString("lblRememberMe");
        btnSignIn.Text = ResourceManager.GetString("lblSignIn");
        rfvUserID.ErrorMessage = ResourceManager.GetString("msgEnterUserName");
    }

    private ParamUser GetUserFromCookie()
    {
        HttpCookie loginCookie = Request.Cookies.Get("loginCookie");
        if (loginCookie != null)
        {
            string[] values = loginCookie.Values.ToString().Split('&');
            if (values.Length > 0)
            {
                string userName = values[0].Remove(0, values[0].LastIndexOf('=') + 1);
                string pass = values[1].Remove(0, values[1].LastIndexOf('=') + 1);
                if (!string.IsNullOrEmpty(userName))
                {
                    ParamUser user = new ParamUserRepository().GetUserById(userName);
                    if (user != null)
                    {
                        string hashedPwd = GetMD5Hash(user.Password);
                        if (pass == hashedPwd)
                        {
                            return user;
                        }
                    }
                }
            }
        }
        return null;
    }

    private void SaveUserToCookie(ParamUser user)
    {
        HttpCookie loginCookie = new HttpCookie("loginCookie");
        loginCookie.Expires = DateTime.Now.AddDays(7);
        loginCookie.Values.Add("uid", user.UserID);
        loginCookie.Values.Add("pwd", GetMD5Hash(user.Password));

        Response.Cookies.Add(loginCookie);
    }

    private void DeleteLoginCookie()
    {
        HttpCookie loginCookie = Request.Cookies.Get("loginCookie");
        if (loginCookie != null)
        {
            loginCookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(loginCookie);
        }
    }

    protected void OnButtonSignIn_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtUserID.Text.Trim()))
        {
            ParamUser user = new ParamUserRepository().GetUserById(txtUserID.Text);
            if (user != null)
            {
                string pwd = !string.IsNullOrEmpty(user.Password) ? user.Password : "";
                if (txtPassword.Text == "******")
                {
                    ParamUser cookieUser =  GetUserFromCookie();
                    if (cookieUser != null)
                    {
                        if(cookieUser.UserID == txtUserID.Text)
                            txtPassword.Text = cookieUser.Password;
                    }
                }
                if (pwd == txtPassword.Text)
                {
                    user.Permissions = new ParamUserPermissionRepository().GetPermissionsOfUser(user.UserID);
                    SessionManager.CurrentUser = user;
                    if (chkRemember.Checked)
                    {
                        SaveUserToCookie(user);
                    }
                    else
                    {
                        DeleteLoginCookie();
                    }
                    if (string.IsNullOrEmpty(Request.QueryString["CandidateId"]))
                    {
                        Response.Redirect("~/Home.aspx", true);
                    }
                    else
                    {
                        Response.Redirect("~/Home.aspx?CandidateId=" + Request.QueryString["CandidateId"], true);
                    }
                }              
                else
                {
                    string script = "<script type='text/javascript'>";
                    script += "centerLoginControl();";
                    script += string.Format("alert(\"{0}\");", ResourceManager.GetString("msgUserNotFound"));
                    script += "</script>";

                    if (!Page.ClientScript.IsClientScriptBlockRegistered("noticeInvalidUser"))
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "noticeInvalidUser", script.ToString());
                }
            }
            else
            {
                string script = "<script type='text/javascript'>";
                script += "centerLoginControl();";
                script += string.Format("alert(\"{0}\");", ResourceManager.GetString("msgUserNotFound"));
                script += "</script>";

                if (!Page.ClientScript.IsClientScriptBlockRegistered("noticeInvalidUser"))
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "noticeInvalidUser", script.ToString());
            }
        }
    }

    public string GetMD5Hash(string input)
    {
        if (string.IsNullOrEmpty(input))
            return "";
        System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
        bs = x.ComputeHash(bs);
        System.Text.StringBuilder s = new System.Text.StringBuilder();
        foreach (byte b in bs)
        {
            s.Append(b.ToString("x2").ToLower());
        }
        string password = s.ToString();
        return password;
    }
}
