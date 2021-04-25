using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusPassApp.Controllers
{
    public class HomePageController : Controller
    {
        //
        // GET: /HomePage/
        BusPassApp.WSDL_Service.BusPassApp Ws_Service = new WSDL_Service.BusPassApp();
        public ActionResult HomePage()
        {
            try
            {
                DataSet ds_result = new DataSet();
                string strRegNo = HttpContext.Session["REGISTERED_NO"] != null ? HttpContext.Session["REGISTERED_NO"].ToString() : "";
                string strMobNo = HttpContext.Session["MOBILE_NO"] != null ? HttpContext.Session["MOBILE_NO"].ToString() : "";
                string strMailID = HttpContext.Session["USER_MAIL"] != null ? HttpContext.Session["USER_MAIL"].ToString() : "";
                string strFlag = "R";
                if (HttpContext.Session["USER_DATA"] != null && HttpContext.Session["USER_DATA"] != "")
                {
                    return View();
                }
                if (strRegNo != "" && strMobNo != "" && strMailID != "")
                {
                    ds_result = Ws_Service.Fetch_User_Details(strRegNo, strMobNo, strMailID, "",strFlag);
                   string str= JsonConvert.SerializeObject(ds_result);
                    if (ds_result != null && ds_result.Tables.Count > 0 && ds_result.Tables[0].Rows.Count > 0)
                    {
                        var dsRow = ds_result.Tables[0].Rows[0];
                        string strUserData = JsonConvert.SerializeObject(ds_result);
                        Session.Add("USER_DATA", strUserData);
                        Session.Add("REG_NO", dsRow["USD_REGISTERED_NO"].ToString());
                        Session.Add("USER_MAIL", dsRow["USD_USER_MAILID"].ToString());
                        Session.Add("USER_NAME", dsRow["USD_USER_NAME"].ToString());
                        Session.Add("USER_DOB", dsRow["USD_DOB"].ToString());
                        Session.Add("USER_SEX", dsRow["USD_GENDER"].ToString());
                        Session.Add("USER_ADDRESS", ((dsRow["USD_DOOR_NO"].ToString() != null ? (dsRow["USD_DOOR_NO"].ToString() + ",") : "") + (dsRow["USD_STREET_NAME"].ToString() != null ? (dsRow["USD_STREET_NAME"].ToString() + ",") : "") + (dsRow["USD_LOCATION"].ToString() != null ? (dsRow["USD_LOCATION"].ToString() + ",") : "") + (dsRow["USD_DISTRICT"].ToString() != null ? (dsRow["USD_DISTRICT"].ToString() + ",") : "")));
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }


        public ActionResult PassInfo()
        {
            try
            {
                DataSet ds_result = new DataSet();
                string strRegNo = HttpContext.Session["REGISTERED_NO"] != null ? HttpContext.Session["REGISTERED_NO"].ToString() : "";
                string strMobNo = HttpContext.Session["MOBILE_NO"] != null ? HttpContext.Session["MOBILE_NO"].ToString() : "";
                string strMailID = HttpContext.Session["USER_MAIL"] != null ? HttpContext.Session["USER_MAIL"].ToString() : "";
                string strFlag = "R";
                if (HttpContext.Session["USER_DATA"] != null && HttpContext.Session["USER_DATA"] != "")
                {
                    return View();
                }
                if (strRegNo != "" && strMobNo != "" && strMailID != "")
                {
                    ds_result = Ws_Service.Fetch_User_Details(strRegNo, strMobNo, strMailID, "", strFlag);
                    string str = JsonConvert.SerializeObject(ds_result);
                    if (ds_result != null && ds_result.Tables.Count > 0 && ds_result.Tables[0].Rows.Count > 0)
                    {
                        var dsRow = ds_result.Tables[0].Rows[0];
                        string strUserData = JsonConvert.SerializeObject(ds_result);
                        Session.Add("USER_DATA", strUserData);
                        Session.Add("REG_NO", dsRow["USD_REGISTERED_NO"].ToString());
                        Session.Add("USER_MAIL", dsRow["USD_USER_MAILID"].ToString());
                        Session.Add("USER_NAME", dsRow["USD_USER_NAME"].ToString());
                        Session.Add("USER_DOB", dsRow["USD_DOB"].ToString());
                        Session.Add("USER_PROFILE_PATH", dsRow["USD_PHOTO_PATH"].ToString());
                        Session.Add("USER_SEX", dsRow["USD_GENDER"].ToString());
                        Session.Add("USER_ADDRESS", ((dsRow["USD_DOOR_NO"].ToString() != null ? (dsRow["USD_DOOR_NO"].ToString() + ",") : "") + (dsRow["USD_STREET_NAME"].ToString() != null ? (dsRow["USD_STREET_NAME"].ToString() + ",") : "") + (dsRow["USD_LOCATION"].ToString() != null ? (dsRow["USD_LOCATION"].ToString() + ",") : "") + (dsRow["USD_DISTRICT"].ToString() != null ? (dsRow["USD_DISTRICT"].ToString() + ",") : "")));
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public ActionResult ExisitingUser()
        {
            return View();
        }

        public ActionResult IssueNewPass()
        {
            ViewBag.type=Request.QueryString["type"];
            Session.Add("Pass_type", Request.QueryString["type"]);
            return View();
        }
    }
}
