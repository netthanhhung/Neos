using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Neos.Data;
using System.Collections.Generic;

/// <summary>
/// Summary description for SessionManager
/// </summary>
public static class SessionManager
{
    #region Common
    private const string _BackUrl = "BackUrl";
    public static string BackUrl
    {
        get
        {
            return HttpContext.Current.Session[_BackUrl] as string;
        }
        set
        {
            HttpContext.Current.Session[_BackUrl] = value;
        }
    }
    #endregion

    #region Candidate
    private const string _CandidateSearchCriteria = "CandidateSearchCriteria";
    public static CandidateSearchCriteria CandidateSearchCriteria
    {
        get
        {
            return HttpContext.Current.Session[_CandidateSearchCriteria] as CandidateSearchCriteria;
        }
        set
        {
            HttpContext.Current.Session[_CandidateSearchCriteria] = value;
        }
    }

    private const string _CurrentCandidate = "CurrentCandidate";
    public static Candidate CurrentCandidate
    {
        get
        {
            return HttpContext.Current.Session[_CurrentCandidate] as Candidate;
        }
        set
        {
            HttpContext.Current.Session[_CurrentCandidate] = value;
        }
    }

    private const string _CandidateEvaluation = "CandidateEvaluation";
    public static CandidateEvaluation CandidateEvaluation
    {
        get
        {
            return HttpContext.Current.Session[_CandidateEvaluation] as CandidateEvaluation;
        }
        set
        {
            HttpContext.Current.Session[_CandidateEvaluation] = value;
        }
    }
    private const string _CandidateExpectation = "CandidateExpectation";
    public static CandidateExpectation CandidateExpectation
    {
        get
        {
            return HttpContext.Current.Session[_CandidateExpectation] as CandidateExpectation;
        }
        set
        {
            HttpContext.Current.Session[_CandidateExpectation] = value;
        }
    }

    //Session["LastNameSearchCriteria"]
    private const string _LastNameSearchCriteria = "LastNameSearchCriteria";
    public static string LastNameSearchCriteria
    {
        get
        {
            return HttpContext.Current.Session[_LastNameSearchCriteria] as string;
        }
        set
        {
            HttpContext.Current.Session[_LastNameSearchCriteria] = value;
        }
    }

    private const string _canKnowledgeOldList = "CanKnowledgeOldList";
    public static IList<CandidateKnowledge> CanKnowledgeOldList
    {
        get
        {
            IList<CandidateKnowledge> result = HttpContext.Current.Session[_canKnowledgeOldList] as IList<CandidateKnowledge>;
            if(result == null) 
                result = new List<CandidateKnowledge>();
            return result;
        }
        set
        {
            HttpContext.Current.Session[_canKnowledgeOldList] = value;
        }
    }
    private const string _canFunctionOldList = "CanFunctionOldList";
    public static IList<CandidateFunction> CanFunctionOldList
    {
        get
        {
            IList<CandidateFunction> result = HttpContext.Current.Session[_canFunctionOldList] as IList<CandidateFunction>;
            if(result == null) 
                result = new List<CandidateFunction>();
            return result;
        }
        set
        {
            HttpContext.Current.Session[_canFunctionOldList] = value;
        }
    }

    private const string _canKnowledgeDesList = "CanKnowledgeDesList";
    public static IList<CandidateKnowledge> CanKnowledgeDesList
    {
        get
        {
            IList<CandidateKnowledge> result = HttpContext.Current.Session[_canKnowledgeDesList] as IList<CandidateKnowledge>;
            if(result == null) 
                result = new List<CandidateKnowledge>();
            return result;
        }
        set
        {
            HttpContext.Current.Session[_canKnowledgeDesList] = value;
        }
    }

    private const string _canFunctionDesList = "CanFunctionDesList";
    public static IList<CandidateFunction> CanFunctionDesList
    {
        get
        {
            IList<CandidateFunction> result = HttpContext.Current.Session[_canFunctionDesList] as IList<CandidateFunction>;
            if(result == null) 
                result = new List<CandidateFunction>();
            return result;
        }
        set
        {
            HttpContext.Current.Session[_canFunctionDesList] = value;
        }
    }

    private const string _canExpectOldList = "CanExpectOldList";
    public static IList<CandidateExpectancy> CanExpectOldList
    {
        get
        {
            IList<CandidateExpectancy> result = HttpContext.Current.Session[_canExpectOldList] as IList<CandidateExpectancy>;
            if(result == null) 
                result = new List<CandidateExpectancy>();
            return result;
        }
        set
        {
            HttpContext.Current.Session[_canExpectOldList] = value;
        }
    }

    private const string _canExpectDesList = "CanExpectDesList";
    public static IList<CandidateExpectancy> CanExpectDesList
    {
        get
        {
            IList<CandidateExpectancy> result = HttpContext.Current.Session[_canExpectDesList] as IList<CandidateExpectancy>;
            if(result == null) 
                result = new List<CandidateExpectancy>();
            return result;
        }
        set
        {
            HttpContext.Current.Session[_canExpectDesList] = value;
        }
    }

    private const string _newCanTelephone = "NewCanTelephone";
    public static List<CandidateTelephone> NewCandidateTelephoneList
    {
        get
        {
            List<CandidateTelephone> result = HttpContext.Current.Session[_newCanTelephone] as List<CandidateTelephone>;
            if (result == null)
                result = new List<CandidateTelephone>();
            return result;
        }
        set
        {
            HttpContext.Current.Session[_newCanTelephone] = value;
        }
    }
    //#endregion
    #endregion

    #region company

    private const string _CurrentCompany = "CurrentCompany";
    public static Company CurrentCompany
    {
        get
        {
            return HttpContext.Current.Session[_CurrentCompany] as Company;
        }
        set
        {
            HttpContext.Current.Session[_CurrentCompany] = value;
        }
    } 
    private const string _newComContact = "NewComContact";
    public static List<CompanyContact> NewCompanyContactList
    {
        get
        {
            List<CompanyContact> result = HttpContext.Current.Session[_newComContact] as List<CompanyContact>;
            if (result == null)
                result = new List<CompanyContact>();
            return result;
        }
        set
        {
            HttpContext.Current.Session[_newComContact] = value;
        }
    }
    

    #endregion

    #region user
    private const string _CurrentUser = "CurrentUser";
    /// <summary>
    /// current logged in user
    /// </summary>
    public static ParamUser CurrentUser
    {
        get
        {
            return HttpContext.Current.Session[_CurrentUser] as ParamUser;
        }
        set
        {
            HttpContext.Current.Session[_CurrentUser] = value;
        }
    }

    #endregion

    #region job
    private const string _CurrentJob = "CurrentJob";

    public static Neos.Data.Job CurrentJob
    {
        get
        {
            return HttpContext.Current.Session[_CurrentJob] as Neos.Data.Job;
        }
        set
        {
            HttpContext.Current.Session[_CurrentJob] = value;
        }
    }

    #endregion

    #region invoice
    private const string _CurrentInvoice = "CurrentInvoice";
    public static Invoices CurrentInvoice
    {
        get
        {
            return HttpContext.Current.Session[_CurrentInvoice] as Invoices;
        }
        set
        {
            HttpContext.Current.Session[_CurrentInvoice] = value;
        }
    }
    #endregion

    #region send presentation
    private const string _PresentationEmailObject = "PresentationEmailObject";
    /// <summary>
    /// current logged in user
    /// </summary>
    public static PresentationEmailObject PresentationEmailObject
    {
        get
        {
            return HttpContext.Current.Session[_PresentationEmailObject] as PresentationEmailObject;
        }
        set
        {
            HttpContext.Current.Session[_PresentationEmailObject] = value;
        }
    }
    #endregion
}
