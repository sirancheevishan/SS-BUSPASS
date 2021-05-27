using BusPassApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
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

            string strRegistrationNo = Convert.ToString(Session["REGISTERED_NO"]);
            string strBookingReq = string.Empty;
            string strXMLData = string.Empty;
            string strErrMSG = string.Empty;
            string strStatus = string.Empty;
            string strResult = string.Empty;
            #region Get Pass Info
            BaseClass.BookingRQRS BookingRQRS = new BaseClass.BookingRQRS();
            string strCurrency = ConfigurationManager.AppSettings["Currency"] != null && ConfigurationManager.AppSettings["Currency"].ToString() != "" ? ConfigurationManager.AppSettings["Currency"].ToString() : "";

            BookingRQRS.strUserID = strRegistrationNo;
            BookingRQRS.stTrackID = "";
            BookingRQRS.strFlag = "F";
            BookingRQRS.strBookedDate = "";
            BookingRQRS.strStatus = "";
            BookingRQRS.strPassCode = "";
            strBookingReq = JsonConvert.SerializeObject(BookingRQRS);

            Session.Add("BooingDetails", JsonConvert.SerializeObject(BookingRQRS));
            //RequestLog
            strXMLData = "<EVENT><REQUEST>inserRegistrationDetails</REQUEST>";
            strXMLData += "<REQUESTTIME>" + DateTime.Now.ToString("DDMMYYYY HH:MM:SS:FFFF") + "</REQUESTTIME><EVENT>";
            strXMLData += "<REQUESTDATA>" + strBookingReq + "</REQUESTDATA><EVENT>";

            DataSet ds_result = Ws_Service.InertFetchBookingDetails(strBookingReq, ref strErrMSG);

            if (ds_result != null && ds_result.Tables.Count > 0 && ds_result.Tables[0].Rows.Count > 0)
            {
                strStatus = "00";
                strResult = "Y";
            }

            //ResponseLog
            strXMLData = "<EVENT><RESPONSE>inserRegistrationDetails</RESPONSE>";
            strXMLData += "<RESTTIME>" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</RESTTIME>";
            strXMLData += "<ERROMSG>" + strErrMSG + "</ERROMSG>";
            strXMLData += "<RESDATA>" + JsonConvert.SerializeObject(ds_result) + "</RESDATA><EVENT>";
            #endregion
            return View();
        }
        public ActionResult IssueNewPass()
        {
            ViewBag.type = Request.QueryString["type"];
            Session.Add("Pass_type", Request.QueryString["type"]);
            return View();
        }


        #region Renewal
        public ActionResult GetPassDetails()
        {
            string strBookingReq = string.Empty;
            string strStatus = string.Empty;
            string strMSG = string.Empty;
            string strResult = string.Empty;
            string strXMLData = string.Empty;
            string strtime = string.Empty;
            string strErrMSG = string.Empty;
            string strRegisteredNo = string.Empty;
            string RegistrationDetails = string.Empty;
            string strUserID = Convert.ToString(Session["REGISTERED_NO"]);

            if (string.IsNullOrEmpty(strUserID))
            {
                return Json(new { Status = "-1", Message = "Session Expired!", Result = "" });
            }


            try
            {
                DataSet ds_result = new DataSet();
                Random randomObj = new Random();
                BaseClass.BookingRQRS BookingRQRS = new BaseClass.BookingRQRS();
                string strCurrency = ConfigurationManager.AppSettings["Currency"] != null && ConfigurationManager.AppSettings["Currency"].ToString() != "" ? ConfigurationManager.AppSettings["Currency"].ToString() : "";

                BookingRQRS.strUserID = strUserID;
                BookingRQRS.strPassNo = "";
                BookingRQRS.strFlag = "R";
                strBookingReq = JsonConvert.SerializeObject(BookingRQRS);

                Session.Add("BooingDetails", JsonConvert.SerializeObject(BookingRQRS));
                //RequestLog
                strtime = DateTime.Now.ToString("DDMMYYYY HH:MM:SS:FFFF");
                strXMLData = "<EVENT><REQUEST>inserRegistrationDetails</REQUEST>";
                strXMLData += "<REQUESTTIME>" + strtime + "</REQUESTTIME><EVENT>";
                strXMLData += "<REQUESTDATA>" + strBookingReq + "</REQUESTDATA><EVENT>";

                ds_result = Ws_Service.InertFetchBookingDetails(strBookingReq, ref strErrMSG);

                //ResponseLog
                strtime = DateTime.Now.ToString("yyyyMMddHHmmss");
                strXMLData = "<EVENT><RESPONSE>inserRegistrationDetails</RESPONSE>";
                strXMLData += "<RESTTIME>" + strtime + "</RESTTIME>";
                strXMLData += "<ERROMSG>" + strErrMSG + "</ERROMSG>";
                strXMLData += "<RESDATA>" + JsonConvert.SerializeObject(ds_result) + "</RESDATA><EVENT>";
                string str= JsonConvert.SerializeObject(ds_result);
                if (ds_result != null && ds_result.Tables.Count > 0 && ds_result.Tables[0].Rows.Count > 0)
                {
                    strStatus = "01";
                    strResult = JsonConvert.SerializeObject(ds_result);
                }
                else
                {
                    strStatus = "00";
                    strMSG = "Unable to Get booked history.";
                }

            }
            catch (Exception ex)
            {
                strStatus = "00";
                strMSG = "Problem occured while geting history. Please try again later(#05).";
                strtime = DateTime.Now.ToString("DDMMYYYY HH:MM:SS:FFFF");
                strXMLData = "<EVENT>";
                strXMLData += "<RESTTIME>" + strtime + "</RESTTIME><EVENT>";
                strXMLData += "<DATA>" + ex.ToString() + "</DATA><EVENT>";
            }

            return Json(new { status = strStatus, Message = strMSG, Result = strResult });
        }
        #endregion
    }
}
