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
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        BusPassApp.WSDL_Service.BusPassApp Ws_Service = new WSDL_Service.BusPassApp();

        
        public ActionResult CheckLogin(string strUserMail, string strPassword, string struserDetails,string LoginFlag,string strMobileNo)
        {
            DataSet ds_result = new DataSet();
            string strStatus = string.Empty;
            string strMsg = string.Empty;
            string strBookingReq = string.Empty;
            string strTime = string.Empty;
            string strXMLData = string.Empty;
            string strErrMSG = string.Empty;
            string strResult = string.Empty;

            try
            {
                if (LoginFlag == "M")
                {
                    ds_result = Ws_Service.Fetch_User_Details("", "", strUserMail, strPassword, "M");

                    if (ds_result != null && ds_result.Tables.Count > 0 && ds_result.Tables[0].Rows.Count > 0)
                    {
                        var dsRow = ds_result.Tables[0].Rows[0];
                        string strUserData = JsonConvert.SerializeObject(ds_result);
                        if (dsRow["USD_ACCOUNT_VERIFICATION"].ToString() == "Y")
                        {
                            Session.Add("EXISITING_USER", "N");

                            string strRegistrationNo = dsRow["USD_REGISTERED_NO"].ToString();
                            Session.Add("USER_DATA", strUserData);
                            Session.Add("REGISTERED_NO", dsRow["USD_REGISTERED_NO"].ToString());
                            Session.Add("USER_MAIL", dsRow["USD_USER_MAILID"].ToString());
                            Session.Add("USER_NUM", dsRow["USD_Mobil_No"].ToString());
                            Session.Add("USD_Password", strPassword);
                            Session.Add("USD_AdharNo", dsRow["USD_ADHAAR_NO"].ToString());
                            Session.Add("USER_NAME", dsRow["USD_USER_NAME"].ToString());
                            Session.Add("USER_DOB", dsRow["USD_DOB"].ToString());
                            Session.Add("USER_SEX", dsRow["USD_GENDER"].ToString());
                            Session.Add("USER_ADDRESS", ((dsRow["USD_DOOR_NO"].ToString() != null ? (dsRow["USD_DOOR_NO"].ToString() + ",") : "") + (dsRow["USD_STREET_NAME"].ToString() != null ? (dsRow["USD_STREET_NAME"].ToString() + ",") : "") + (dsRow["USD_LOCATION"].ToString() != null ? (dsRow["USD_LOCATION"].ToString() + ",") : "") + (dsRow["USD_DISTRICT"].ToString() != null ? (dsRow["USD_DISTRICT"].ToString() + ",") : "")));


                            #region Get Pass Info
                            BaseClass.BookingRQRS BookingRQRS = new BaseClass.BookingRQRS();
                            string strCurrency = ConfigurationManager.AppSettings["Currency"] != null && ConfigurationManager.AppSettings["Currency"].ToString() != "" ? ConfigurationManager.AppSettings["Currency"].ToString() : "";

                            BookingRQRS.strUserID = strRegistrationNo;
                            BookingRQRS.stTrackID = "";
                            BookingRQRS.strFlag = "F";
                            BookingRQRS.strBookedDate =  "";
                            BookingRQRS.strStatus = "";
                            BookingRQRS.strPassCode = "";
                            strBookingReq = JsonConvert.SerializeObject(BookingRQRS);

                            Session.Add("BooingDetails", JsonConvert.SerializeObject(BookingRQRS));
                            //RequestLog
                            strTime = DateTime.Now.ToString("DDMMYYYY HH:MM:SS:FFFF");
                            strXMLData = "<EVENT><REQUEST>inserRegistrationDetails</REQUEST>";
                            strXMLData += "<REQUESTTIME>" + strTime + "</REQUESTTIME><EVENT>";
                            strXMLData += "<REQUESTDATA>" + strBookingReq + "</REQUESTDATA><EVENT>";

                            ds_result = Ws_Service.InertFetchBookingDetails(strBookingReq, ref strErrMSG);

                            if (ds_result != null && ds_result.Tables.Count > 0 && ds_result.Tables[0].Rows.Count>0)
                            {
                                strStatus = "00";
                                strResult = "Y";
                                Session["EXISITING_USER"] = "Y";
                            }
                            
                            //ResponseLog
                            strTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                            strXMLData = "<EVENT><RESPONSE>inserRegistrationDetails</RESPONSE>";
                            strXMLData += "<RESTTIME>" + strTime + "</RESTTIME>";
                            strXMLData += "<ERROMSG>" + strErrMSG + "</ERROMSG>";
                            strXMLData += "<RESDATA>" + JsonConvert.SerializeObject(ds_result) + "</RESDATA><EVENT>";
                            #endregion
                                
                        }
                        else {
                            strStatus = "02";
                            Session.Add("REGISTERED_NO", dsRow["USD_REGISTERED_NO"].ToString());
                            Session.Add("MAIL_ID", dsRow["USD_USER_MAILID"].ToString());
                            Session.Add("MOBILE_NO", dsRow["USD_Mobil_No"].ToString());
                            strMsg = "Your account is not verify. Please verify your account.";
                            string strOTPDet = dsRow["USD_USER_NAME"].ToString() + "~" + dsRow["USD_REGISTERED_NO"].ToString() + "~" + dsRow["USD_USER_MAILID"].ToString() + "~" + dsRow["USD_Mobil_No"].ToString();
                            return Json(new { Status = strStatus, OTPDet=strOTPDet, Message = strMsg });
                        }
                    }
                    else
                    {
                        strStatus = "01";
                        strMsg = "Incorrect Mail Id or Password";
                    }
                }
                else if (LoginFlag == "O")
                { 
                
                }
            }
            catch (Exception ex)
            {
                strStatus = "01";
                strMsg = "Problem occured while login. Please contact support team(#05).";
            }
            return Json(new {Status=strStatus,Message=strMsg,Result=strResult });
        }

    }
}
