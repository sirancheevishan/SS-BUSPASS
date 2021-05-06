using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BusPassApp.Models;

using System.Web.Mvc;
using System.Data;
using System.Configuration;

namespace BusPassApp.Controllers
{
    
    public class AfterBookingController : Controller
    {
        //
        // GET: /AfterBooking/

        BusPassApp.WSDL_Service.BusPassApp Ws_Service = new WSDL_Service.BusPassApp();
        public ActionResult ManagementBooking()
        {
            return View();
        }
        public ActionResult GetBookedHistory(string strTrackID)
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
                BookingRQRS.stTrackID = strTrackID;
                BookingRQRS.strFlag = "F";

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


        public ActionResult Test()
        {
            return View();
        }

    }
}
