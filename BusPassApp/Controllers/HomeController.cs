using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusPassApp.Models;
using BusPassApp.WSDL_Service;
using System.Collections;
using Newtonsoft.Json;
using System.Data;
using RestSharp;
using Newtonsoft.Json.Linq;


namespace BusPassApp.Controllers
{
    public class HomeController : Controller
    {

        BusPassApp.WSDL_Service.BusPassApp Ws_Service = new WSDL_Service.BusPassApp();

        public ActionResult LogIn()
        {
            return View();
        }
        public ActionResult Home()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
        public ActionResult ComingSoon()
        {
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFiles()
        {
            string strstatus = string.Empty;
            string Errmsg = string.Empty;
            string fname = string.Empty;
            string filename = string.Empty;
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {

                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];


                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            filename = "siran";//file.FileName;
                        }

                        // Get the complete folder path and store the file inside it.  
                        fname = Path.Combine(Server.MapPath("~/ProfilePic/"), filename);
                        file.SaveAs(fname);
                        strstatus = "00";
                        Errmsg = "";
                    }
                    // Returns message that successfully uploaded  
                    return Json(new { strPath = "~/ProfilePic/" + fname + "", status = strstatus, msg = Errmsg });
                }
                catch (Exception ex)
                {
                    strstatus = "01";
                    Errmsg = "Exception";
                    return Json(new { strPath = "", status = strstatus, msg = Errmsg });
                }
            }
            else
            {
                Errmsg = "No files selected.";
                return Json(new { strPath = "", status = strstatus, msg = Errmsg });
            }
        }

        public ActionResult Profilepicture()
        {
            ArrayList arylst = new ArrayList();
            arylst.Add("");
            arylst.Add("");
            int arr_error = 0;
            int arr_result = 1;
            string Exfilepath = string.Empty;

            try
            {
                HttpFileCollectionBase ProfilePicture = Request.Files;
                string ProfilepicName = Request.Form[0].ToString();
                string Mobileno = Request.Form[0].ToString();
                HttpPostedFileBase Imgprofilepicture = ProfilePicture[0];
                string fileExt1 = System.IO.Path.GetExtension(Imgprofilepicture.FileName);
                if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath("~/Images/ProfilePicture")))
                {
                    Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("~/Images/ProfilePicture"));
                }

                ProfilepicName = ProfilepicName + fileExt1;
                string filepath = Path.Combine(Server.MapPath("~/Images/ProfilePicture"), ProfilepicName);
                Imgprofilepicture.SaveAs(filepath);

                arylst[arr_result] = filepath;


            }
            catch (Exception ex)
            {
                //DatabaseLog.LogData("", "x", "HOMECONTROLLER", "status", ex.ToString(), "", ConfigurationManager.AppSettings["TerminalId"] != null ? ConfigurationManager.AppSettings["TerminalId"].ToString() : "", "0", "");
                arylst[arr_error] = "Unable to update profile picture. Please contact support team (#05)";
            }
            return Json(new { d = arylst });
        }
        public ActionResult UploadDocument()
        {
            ArrayList arylst = new ArrayList();
            arylst.Add("");
            arylst.Add("");
            int arr_error = 0;
            int arr_result = 1;
            string Exfilepath = string.Empty;

            try
            {
                HttpFileCollectionBase ProfilePicture = Request.Files;
                string ProfilepicName = Request.Form[0].ToString();
                string Mobileno = Request.Form[0].ToString();
                HttpPostedFileBase Imgprofilepicture = ProfilePicture[0];
                string fileExt1 = System.IO.Path.GetExtension(Imgprofilepicture.FileName);
                if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath("~/Documents/AdhaarImages")))
                {
                    Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("~/Documents/AdhaarImages"));
                }

                ProfilepicName = ProfilepicName + fileExt1;
                string filepath = Path.Combine(Server.MapPath("~/Images/ProfilePicture"), ProfilepicName);
                Imgprofilepicture.SaveAs(filepath);

                arylst[arr_result] = filepath;


            }
            catch (Exception ex)
            {
                //DatabaseLog.LogData("", "x", "HOMECONTROLLER", "status", ex.ToString(), "", ConfigurationManager.AppSettings["TerminalId"] != null ? ConfigurationManager.AppSettings["TerminalId"].ToString() : "", "0", "");
                arylst[arr_error] = "Unable to update profile picture. Please contact support team (#05)";
            }
            return Json(new { d = arylst });
        }

        public ActionResult Inser_Registration_Details(BaseClass.RegistrationDetails RegistrationDetails)

        {
            string strRegistrationDetails = string.Empty;
            string strStatus = string.Empty;
            string strMSG = string.Empty;
            string strXMLData = string.Empty;
            string strtime = string.Empty;
            string strErrMSG = string.Empty;
            string strRegisteredNo = string.Empty;
            DataSet ds_result = new DataSet();

            try
            {
                strRegistrationDetails = JsonConvert.SerializeObject(RegistrationDetails);

                //RequestLog
                strtime = DateTime.Now.ToString("DDMMYYYY HH:MM:SS:FFFF");
                strXMLData = "<EVENT><REQUEST>inserRegistrationDetails</REQUEST>";
                strXMLData += "<REQUESTTIME>" + strtime + "</REQUESTTIME><EVENT>";
                strXMLData += "<REQUESTDATA>" + strRegistrationDetails + "</REQUESTDATA><EVENT>";

                ds_result = Ws_Service.InsertRegistration(strRegistrationDetails, ref strErrMSG);

                //ResponseLog
                strtime = DateTime.Now.ToString("DDMMYYYY HH:MM:SS:FFFF");
                strXMLData = "<EVENT><RESPONSE>inserRegistrationDetails</RESPONSE>";
                strXMLData += "<RESTTIME>" + strtime + "</RESTTIME>";
                strXMLData += "<ERROMSG>" + strErrMSG + "</ERROMSG>";
                strXMLData += "<RESDATA>" + JsonConvert.SerializeObject(ds_result) + "</RESDATA><EVENT>";

                if (ds_result != null && ds_result.Tables.Count > 0 && ds_result.Tables[0].Rows.Count > 0)
                {
                    if (ds_result.Tables[0].Rows[0]["Result"].ToString() == "1")
                    {

                        strRegisteredNo = ds_result.Tables[0].Rows[0]["RegisteredNo"].ToString();
                        //JObject OTPResult = new JObject();
                        JsonResult OTPResult = new JsonResult();
                        OTPResult = SendOTP(RegistrationDetails.strName, strRegisteredNo, RegistrationDetails.strMbl, RegistrationDetails.strMail);
                        string strOTPResult = JsonConvert.SerializeObject(OTPResult.Data);
                        RQRS.OTPcheck OTPObj = JsonConvert.DeserializeObject<RQRS.OTPcheck>(strOTPResult);

                        if (OTPObj.result == true)
                        {
                            Session.Add("REGISTERED_NO", strRegisteredNo);
                            Session.Add("MOBILE_NO", RegistrationDetails.strMbl);
                            Session.Add("MAIL_ID", RegistrationDetails.strMail);
                            strStatus = "00";
                        }
                        else
                        {
                            strStatus = "01";
                            strMSG = OTPObj.Message;
                        }
                    }
                    else
                    {
                        strStatus = "01";
                        strMSG = ds_result.Tables[0].Rows[0]["RegisteredNo"] != null && ds_result.Tables[0].Rows[0]["RegisteredNo"].ToString() != "" ? ds_result.Tables[0].Rows[0]["RegisteredNo"].ToString() : "Unable to process your Request. Please try again later(#03)."; //#01 - exceptional;#02 - unable to get response from DB;#02 - unable to response error 
                    }
                }
                else
                {
                    strStatus = "01";
                    strMSG = "Unable to Register your Request. Please try again later(#02).";
                    return Json(new { status = strStatus, RegisteredNo = strRegisteredNo, Message = strMSG });
                }

            }
            catch (Exception ex)
            {
                strStatus = "01";
                strMSG = "Problem occured while Registration. Please try again later(#01).";
                strtime = DateTime.Now.ToString("DDMMYYYY HH:MM:SS:FFFF");
                strXMLData = "<EVENT>";
                strXMLData += "<RESTTIME>" + strtime + "</RESTTIME><EVENT>";
                strXMLData += "<DATA>" + ex.ToString() + "</DATA><EVENT>";
            }

            return Json(new { status = strStatus, RegisteredNo = strRegisteredNo, Message = strMSG, mobileNo = RegistrationDetails.strMbl, MailID = RegistrationDetails.strMail });
        }

        public JsonResult SendOTP(string strName, string strRegisteredNo, string strMbl, string strMail)
        {
            string GenerateMobileOTP = string.Empty;
            string MobileOTP = string.Empty;
            string GenerateEmailOTP = string.Empty;
            string EmailOTP = string.Empty;
            string strtime = string.Empty;
            string strStatus = string.Empty;
            string strMsg = string.Empty;
            string strMailContent = string.Empty;
            bool boolres = false;
            try
            {
                GenerateMobileOTP = Utilities.RandomOTPGeneration(ref MobileOTP);
                GenerateEmailOTP = Utilities.RandomOTPGeneration(ref EmailOTP);
                strtime = DateTime.Now.ToString("DDMMYYYY HH:MM:SS:FFFF");
                DataSet dsOTP = Ws_Service.Insert_Fetch_OTP(strRegisteredNo, strMbl, strMail, GenerateMobileOTP, GenerateEmailOTP, strtime, "I");

                strMailContent = OTP_Mail_Content_Formation(strName, GenerateEmailOTP, strRegisteredNo);
                string strMessageContent = "Dear " + strName + ", Your have initiated verfication process for online PBS with the Registered code of" + strRegisteredNo + ". " + GenerateMobileOTP + " is the OTP for your activate your acount. OTPs are SECRET. DO NOT disclose it to anyone.";
                bool SMSOTPResult = Ws_Service.SendOTP(strMbl, strMessageContent);
                bool MailOTPResult =  Ws_Service.SendMail(strMail, "BPS-Verfication", strMailContent);

                if (SMSOTPResult == true && MailOTPResult == true)
                {
                    strStatus = "00";
                    boolres = true;

                }
                else
                {
                    strStatus = "01";
                    boolres = false;
                    strMsg = "unable to sent OTP. Please try again later.";
                }
            }
            catch (Exception ex)
            {
                strStatus = "01";
                boolres = false;
                strMsg = "unable to process your request. Please try again later.";
            }
            //object jobj = new object();
            //jobj.strStatus = strStatus;
            //jobj.Add("status",strStatus);// .status=strStatus
            //jobj.Add("boolres", boolres);
            //jobj.Add("Message", strMsg);
            //return jobj;

            return Json(new { Status = strStatus, result = boolres, Message = strMsg });
        }

        public string OTP_Mail_Content_Formation(string strName, string strOTP, string strRegisteredNo)
        {

            string stringBuilder = string.Empty;

            #region MailContent

            stringBuilder += "<div class=''>";
            stringBuilder += "<div class='aHl'></div>";
            stringBuilder += "<div id=':16l' tabindex='-1'></div>";
            stringBuilder += "<div id=':17z' class='ii gt'>";
            stringBuilder += "<div id=':17y' class='a3s aXjCH '>";
            stringBuilder += "<hr><b>This is a system generated mail. Please do not reply to this email ID. If you have a query or need any clarification you may:<br> (1) Call our 24-hour Customer Care or<br> (2) Email Us <a href='sirancheevishan@gmail.com' target='_blank' >sirancheevishan@gmail.com</a></b>";
            stringBuilder += "<br>";
            stringBuilder += "<hr>";
            stringBuilder += "<br>Dear <b>" + strName + ",</b>";
            stringBuilder += "<br>";
            stringBuilder += "<br>You have initiated verification Process for online BusPass application with the registered number of " + strRegisteredNo + "";
            stringBuilder += "<br>";
            stringBuilder += "<br>Your One Time Password (<span class='il'>OTP</span>) is " + strOTP + " for activate Your BPS account.";
            stringBuilder += "<br><br>";
            stringBuilder += "<br>Please note, this <span class='il'>OTP</span> is valid only for mentioned transaction and cannot be used for any other transaction.";
            stringBuilder += "<br>Please do not share this One Time Password with anyone.";
            stringBuilder += "<br>For any problem please contact us at 24*7 Hrs. Customer Support at 9600591085, 8838978273 (Language: Hindi and English). or mail us at <a href='sirancheevishan@gmail.com' target='_blank' >sirancheevishan@gmail.com</a> ";
            stringBuilder += "<br>We solicit your continued patronage to our services.";
            stringBuilder += "<br>";
            stringBuilder += "<br>";
            stringBuilder += "<br>";
            stringBuilder += "<br>";
            stringBuilder += "<br>";
            stringBuilder += "<br>Warm Regards,";
            stringBuilder += "<br>Customer Care";
            stringBuilder += "<br>Internet Ticketing";
            stringBuilder += "<br>BPS";
            stringBuilder += "<div class='yj6qo'></div>";
            stringBuilder += "<div class='adL'>";
            stringBuilder += "<br>";
            stringBuilder += "</div>";
            stringBuilder += "</div>";
            stringBuilder += "</div>";
            stringBuilder += "<div id=':16q' class='ii gt' style='display:none'>";
            stringBuilder += "<div id=':16p' class='a3s aXjCH undefined'></div>";
            stringBuilder += "</div>";
            stringBuilder += "<div class='hi'></div>";
            stringBuilder += "</div>";
            #endregion

            return stringBuilder;
        }




        public ActionResult Verify_Registration(string strSMSOTP, string strMailOTP)
        {
            string strtime = string.Empty;
            string strStatus = string.Empty;
            string strMSG = string.Empty;
            string strXMLData = string.Empty;
            string strErrMSG = string.Empty;
            string strMobileNo = Session["MOBILE_NO"] != null ? Session["MOBILE_NO"].ToString() : "";
            string strRegisteredNo = Session["REGISTERED_NO"] != null ? Session["REGISTERED_NO"].ToString() : "";
            string strMailID = Session["MAIL_ID"] != null ? Session["MAIL_ID"].ToString() : "";
            try
            {
                strtime = DateTime.Now.ToString("DDMMYYYY HH:MM:SS:FFFF");
                Hashtable myparam = new Hashtable();
                myparam.Add("SMSOTP", strSMSOTP);
                myparam.Add("MailOTP", strMailOTP);
                myparam.Add("MobileNo", strMobileNo);
                myparam.Add("MailID", strMailID);

                //RequestLog
                strtime = DateTime.Now.ToString("dd/MM/YYYY HH:mm:ss");
                strXMLData = "<EVENT><REQUEST>inserRegistrationDetails</REQUEST>";
                strXMLData += "<REQUESTTIME>" + strtime + "</REQUESTTIME><EVENT>";
                strXMLData += "<REQUESTDATA>" + JsonConvert.SerializeObject(myparam) + "</REQUESTDATA><EVENT>";

                DataSet dsOTP = Ws_Service.Insert_Fetch_OTP(strRegisteredNo, strMobileNo, strMailID, strSMSOTP, strMailOTP, strtime, "F");
                //ResponseLog
                strtime = DateTime.Now.ToString("dd/MM/YYYY HH:mm:ss");
                strXMLData = "<EVENT><RESPONSE>inserRegistrationDetails</RESPONSE>";
                strXMLData += "<RESTTIME>" + strtime + "</RESTTIME>";
                strXMLData += "<ERROMSG>" + strErrMSG + "</ERROMSG>";
                strXMLData += "<RESDATA>" + JsonConvert.SerializeObject(dsOTP) + "</RESDATA><EVENT>";
                if (dsOTP != null && dsOTP.Tables.Count > 0 && dsOTP.Tables[0].Rows.Count > 0)
                {
                    DataSet dsVeriftResult = Ws_Service.Insert_Fetch_OTP(strRegisteredNo, strMobileNo, strMailID, strSMSOTP, strMailOTP, strtime, "V");
                    if (dsVeriftResult != null && dsVeriftResult.Tables.Count > 0 && dsVeriftResult.Tables[0].Rows.Count > 0)
                    {
                        strStatus = "00";
                        strMSG = "";
                    }
                    else
                    {
                        strStatus = "01";
                        strMSG = "Unable to verify your acoount. Please try again later.";
                    }

                }
                else
                {
                    strStatus = "01";
                    strMSG = "Enter Valid OTP";
                }

            }
            catch (Exception ex)
            {
                strStatus = "01";
                strMSG = "Problem occured while Vertify OTP. Please try again later(#01).";
                strtime = DateTime.Now.ToString("dd/MM/YYYY HH:mm:ss");
                strXMLData = "<EVENT>";
                strXMLData += "<RESTTIME>" + strtime + "</RESTTIME><EVENT>";
                strXMLData += "<DATA>" + ex.ToString() + "</DATA><EVENT>";
            }

            return Json(new { status = strStatus, RegisteredNo = strRegisteredNo, Message = strMSG });
        }





    }
}
