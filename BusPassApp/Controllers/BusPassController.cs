using BusPassApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace BusPassApp.Controllers
{
    public class BusPassController : PaymentController
    {
        //
        // GET: /BusPass/



        BusPassApp.WSDL_Service.BusPassApp Ws_Service = new WSDL_Service.BusPassApp();
        public ActionResult NewPass()
        {
            return View();
        }

        public ActionResult GetUserDetails()
        {
            DataSet ds_result = new DataSet();
            string strStatus = string.Empty;
            string strError = string.Empty;
            string strResult = string.Empty;
            string strLogData = string.Empty;

            string strRegNo = HttpContext.Session["REGISTERED_NO"] != null ? HttpContext.Session["REGISTERED_NO"].ToString() : "";
            string strMobNo = HttpContext.Session["USER_NUM"] != null ? HttpContext.Session["USER_NUM"].ToString() : "";
            string strMailID = HttpContext.Session["USER_MAIL"] != null ? HttpContext.Session["USER_MAIL"].ToString() : "";
            string strPassword = HttpContext.Session["USD_Password"] != null ? HttpContext.Session["USD_Password"].ToString() : "";

            string strFlag = "R";

            if (string.IsNullOrEmpty(strMobNo))
            {
                return Json(new { Status = "-1", Message = "Session Expired.", Result = "" });

            }

            try
            {

                strLogData += "<EVENT>";
                strLogData += "<REQUEST><REQUESTMETHOD>GetUserDetails</REQUESTMETHOD><REQUESTTIME>" + DateTime.Now.ToString() + "</REQUESTTIME><REQUESTDATA><REGISTERNO>" + strRegNo + "</REGISTERNO><MOBILENO>" + strMobNo + "</MOBILENO><MAILID>" + strMailID + "</MAILID></REQUESTDATA></REQUEST>";

                ds_result = Ws_Service.Fetch_User_Details(strRegNo, strMobNo, strMailID, "", strFlag);

                if (ds_result != null && ds_result.Tables.Count > 0 && ds_result.Tables[0].Rows.Count > 0)
                {
                    strResult = JsonConvert.SerializeObject(ds_result);
                }
                else
                {
                    strStatus = "01";
                    strError = "Unable to get user details.";
                }

                strLogData += "<RESPONSE><RESTIME>" + DateTime.Now.ToString() + "</RESTIME><RESDATA>" + JsonConvert.SerializeObject(ds_result) + "</RESDATA></RESPONSE>";
                strLogData += "</EVENT>";

            }
            catch (Exception ex)
            {
                strStatus = "05";
                strError = "Problemt occured while get user details(#05).";
                strLogData += "<ERROR>" + ex.ToString() + "</ERROR>";
                strLogData += "</EVENT>";
            }

            return Json(new { Status = strStatus, Message = strError, Result = strResult });
        }


        public ActionResult BookingRequest(BaseClass.BookingRQRS BookingRQRS)
        {

            string strBookingReq = string.Empty;
            string strStatus = string.Empty;
            string strMSG = string.Empty;
            string strResult = string.Empty;
            string strXMLData = string.Empty;
            string strtime = string.Empty;
            string strErrMSG = string.Empty;
            string strRegisteredNo = string.Empty;
            string strTrackID = string.Empty;
            String strPassType = string.Empty;
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
                string strCurrency = ConfigurationManager.AppSettings["Currency"] != null && ConfigurationManager.AppSettings["Currency"].ToString() != "" ? ConfigurationManager.AppSettings["Currency"].ToString() : "";
                BookingRQRS.strPassType= strPassType;
                BookingRQRS.strUserID = strUserID;
                strTrackID = "BPSBKT" + strPassType + DateTime.Now.ToString("yyyyMMddHHmmss") + randomObj.Next(10000000, 100000000).ToString();
                BookingRQRS.stTrackID = strTrackID;
                BookingRQRS.strPassNo = randomObj.Next(10000000, 100000000).ToString();
                BookingRQRS.strRemark = "Pending";
                BookingRQRS.strBookedDate = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                BookingRQRS.strFlag = "I";
                BookingRQRS.strStatus="P";
                BookingRQRS.strAdhaarNo = Convert.ToString(Session["USD_AdharNo"]);
                BookingRQRS.strCurrency = strCurrency;

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
                    if (ds_result.Tables[0].Rows[0]["Result"].ToString() == "1")
                    {
                        strTrackID = "PGRAZOR" + strPassType + DateTime.Now.ToString("yyyyMMddHHmmss") + randomObj.Next(10000000, 100000000).ToString();
                        BaseClass.PGRQRS PGRQRS = new BaseClass.PGRQRS();
                        PGRQRS.strUserID = strUserID;
                        PGRQRS.strUserName = BookingRQRS.strTitle + " " + BookingRQRS.strFName + " " + BookingRQRS.strLName;
                        PGRQRS.strAdhaarNo = Convert.ToString(Session["USD_AdharNo"]);
                        PGRQRS.strCurrency = strCurrency;
                        PGRQRS.strAmount = BookingRQRS.strAmount;
                        PGRQRS.strContactNo = BookingRQRS.strPhnNo;
                        PGRQRS.strCreatedDate = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                        PGRQRS.strFlag = "I";
                        PGRQRS.strIpAddress = Utilities.GetPublicIpAddress();
                        PGRQRS.strMail = BookingRQRS.strMail;
                        PGRQRS.strPassNo = BookingRQRS.strPassNo;
                        PGRQRS.strRemark = "Pending";
                        PGRQRS.strTrackID = strTrackID;
                        PGRQRS.stTrackID = strTrackID;
                        PGRQRS.strTrackStatus = "P";
                        PGRQRS.strTrackStatusCode = "00";
                        PGRQRS.strUpdateDate = DateTime.Now.ToString(("yyyyMMddHHmmss"));

                        //RequestLog
                        string strPGReq = JsonConvert.SerializeObject(PGRQRS);
                        strtime = DateTime.Now.ToString("DDMMYYYY HH:MM:SS:FFFF");
                        strXMLData = "<EVENT><REQUEST>PGRequest</REQUEST>";
                        strXMLData += "<REQUESTTIME>" + strtime + "</REQUESTTIME><EVENT>";
                        strXMLData += "<REQUESTDATA>" + strPGReq + "</REQUESTDATA><EVENT>";

                        ds_result = Ws_Service.ManagePGTrack(strPGReq, ref strErrMSG);
                        if (ds_result != null && ds_result.Tables.Count > 0 && ds_result.Tables[0].Rows.Count > 0)
                        {
                            if (ds_result.Tables[0].Rows[0]["Result"].ToString() == "1")
                            {
                                Models.PaymentInitiateModel _requestData = new Models.PaymentInitiateModel();
                                _requestData.amount = BookingRQRS.strAmount;
                                _requestData.address = "Chennai";
                                _requestData.contactNumber = BookingRQRS.strPhnNo;
                                _requestData.email = BookingRQRS.strMail;
                                _requestData.strTrackID = strTrackID;
                                _requestData.name = BookingRQRS.strTitle + " " + BookingRQRS.strFName + " " + BookingRQRS.strLName; ;
                                _requestData.strPassType = strPassType;
                                Session.Add("PAYMENTRQRS", JsonConvert.SerializeObject(_requestData));
                                strResult = JsonConvert.SerializeObject(_requestData);
                                strStatus = "01";

                            }
                        }
                        else
                        {
                            strStatus = "00";
                            strMSG = "Unable to initiate Payment.";
                        }

                        //ResponseLog
                        strtime = DateTime.Now.ToString("DDMMYYYY HH:MM:SS:FFFF");
                        strXMLData = "<EVENT><RESPONSE>inserRegistrationDetails</RESPONSE>";
                        strXMLData += "<RESTTIME>" + strtime + "</RESTTIME>";
                        strXMLData += "<ERROMSG>" + strErrMSG + "</ERROMSG>";
                        strXMLData += "<RESDATA>" + JsonConvert.SerializeObject(ds_result) + "</RESDATA><EVENT>";

                    }
                    else
                    {
                        strStatus = "00";
                        strMSG = "Unable to initiate booking.";
                    }
                }
                else
                {
                    strStatus = "00";
                    strMSG = "Unable to initiate booking."; 
                }

            }
            catch (Exception ex)
            {
                strStatus = "01";
                strMSG = "Problem occured while Booking. Please try again later(#05).";
                strtime = DateTime.Now.ToString("DDMMYYYY HH:MM:SS:FFFF");
                strXMLData = "<EVENT>";
                strXMLData += "<RESTTIME>" + strtime + "</RESTTIME><EVENT>";
                strXMLData += "<DATA>" + ex.ToString() + "</DATA><EVENT>";
            }

            return Json(new { status = strStatus,Message=strMSG,Result= strResult });
        }


        #region Pass Renewal

        public ActionResult GetSelectedPassDetails(string strPassNumber)
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
                BookingRQRS.strFlag = "R";
                BookingRQRS.strPassNo = strPassNumber;
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
                    string[] strName = ds_result.Tables[0].Rows[0]["PI_USER_NAME"].ToString().Split(' ');
                    Session.Add("RE_USER_NAME", strName.Length > 1 ? strName[1] : "");
                    Session.Add("RE_USER_LNAME", strName.Length>1 ? strName[2] :"");
                    Session.Add("RE_USER_DOB", ds_result.Tables[0].Rows[0]["PI_EFFECTIVE_TO"].ToString());
                    Session.Add("RE_USER_SEX", ds_result.Tables[0].Rows[0]["PI_USER_NAME"].ToString());
                    Session.Add("RE_USER_NUM", ds_result.Tables[0].Rows[0]["PI_USER_CONTACT_NO"].ToString());
                    Session.Add("RE_USER_SEX", strName.Length > 0 ? strName[0] : "");
                    Session.Add("RE_USD_USER_MAILID", ds_result.Tables[0].Rows[0]["PI_USER_MAIL"].ToString());
                    Session.Add("RE_Amount", ds_result.Tables[0].Rows[0]["PI_AMOUNT"].ToString());
                    Session.Add("RE_Effective_From", ds_result.Tables[0].Rows[0]["PI_EFFECTIVE_FROM"].ToString());
                    Session.Add("RE_Effective_To", ds_result.Tables[0].Rows[0]["PI_EFFECTIVE_TO"].ToString());

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
                strMSG = "Problem occured while geting details. Please try again later(#05).";
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
