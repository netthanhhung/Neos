using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Web.Caching;
using System.Collections;


public class ResourceManager
{
    public static string GetString(string name)
    {

        return GetString(name, false);
    }

    public static string GetString(string name, bool defaultOnly)
    {
        string filePath = HttpContext.Current.Request.MapPath(WebConfig.DefaultLanguage);
        /*if (clsSessionVariable.language == "EN")
            filePath = HttpContext.Current.Request.MapPath(WebConfig.ENLanguageFile);
        else if (clsSessionVariable.language == "FR")
            filePath = HttpContext.Current.Request.MapPath(WebConfig.FRLanguageFile);
        else if ((clsSessionVariable.language == "NL"))
            filePath = HttpContext.Current.Request.MapPath(WebConfig.NLLanguageFile);*/
        
        //switch (clSessionVariable.language)
        //{
        //    case Language.ENGLISH:
                
        //        break;
        //    case Language.FRENCH:
        //        filePath = HttpContext.Current.Request.MapPath(WebConfig.FRLanguageFile);
        //        break;
        //    case Language.DUTCH:
        //        filePath = HttpContext.Current.Request.MapPath(WebConfig.NLLanguageFile);
        //        break;
        //    default:
        //        filePath = HttpContext.Current.Request.MapPath(WebConfig.ENLanguageFile);
        //        break;
        //}
        return GetString(name, filePath, defaultOnly);
        //return GetString(name, WebConfig.LanguageFile, defaultOnly);
    }

    public static string GetString(string name, string fileName)
    {
        return GetString(name, fileName, false);
    }

    public static string GetString(string name, string fileName, bool defaultOnly)
    {
        Hashtable resources = null;
        HttpContext cSContext = HttpContext.Current;

        if (fileName != null && fileName != "")
            resources = GetResource(ResourceManagerType.Text, fileName, defaultOnly);
        else
            resources = GetResource(ResourceManagerType.Text, WebConfig.DefaultLanguage, defaultOnly);

        string text = resources[name] as string;

        //try the standard file if we passed a file that didnt have the key we were looking for
        if (text == null && fileName != null && fileName != "")
        {
            resources = GetResource(ResourceManagerType.Text, WebConfig.DefaultLanguage, false);

            text = resources[name] as string;
        }

        if (text == null)
        {
            text = string.Empty;
        }
        return text;
    }

    private static Hashtable GetResource(ResourceManagerType resourceType, string fileName, bool defaultOnly)
    {
        string cacheKey = resourceType.ToString() + fileName;
        Hashtable resources = new Hashtable();
        resources = LoadResource(resourceType, resources, cacheKey, fileName);
        
        return resources;
    }

    private static Hashtable LoadResource(ResourceManagerType resourceType, Hashtable target, string cacheKey, string fileName)
    {
        string filePath = fileName;//HttpContext.Current.Request.MapPath("~/Languages/fr.xml");

        //CacheDependency dp = new CacheDependency(filePath);

        XmlDocument d = new XmlDocument();
        try
        {
            d.Load(filePath);
        }
        catch
        {
            return target;
        }

        foreach (XmlNode n in d.SelectSingleNode("root").ChildNodes)
        {
            if (n.NodeType == XmlNodeType.Element)
            {
                switch (resourceType)
                {
                    case ResourceManagerType.Text:                        
                        if (target[n.Attributes["name"].Value] == null)
                            target.Add(n.Attributes["name"].Value, n.InnerText);
                        else
                            target[n.Attributes["name"].Value] = n.InnerText;
                        
                        break;
                }
            }
        }
        //TCache.Max(cacheKey, target, dp);

        return target;

    }
}
enum ResourceManagerType
{
    Text,
}

