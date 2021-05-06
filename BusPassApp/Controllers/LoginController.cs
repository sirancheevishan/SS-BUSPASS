using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
                            Session.Add("USER_DATA", strUserData);
                            Session.Add("REGISTERED_NO", dsRow["USD_REGISTERED_NO"].ToString());
                            Session.Add("USER_MAIL", dsRow["USD_USER_MAILID"].ToString());
                            Session.Add("USER_NUM", dsRow["USD_Mobil_No"].ToString());
                            Session.Add("USD_Password", strPassword);
                            Session.Add("USD_AdharNo", dsRow["USD_ADHAAR_NO"].ToString());
                            Session.Add("USER_NAME", dsRow["USD_USER_NAME"].ToString());
                            Session.Add("USER_DOB", dsRow["USD_DOB"].ToString());
                            Session.Add("USER_SEX", dsRow["USD_GENDER"].ToString() == "M");
                            Session.Add("USER_ADDRESS", ((dsRow["USD_DOOR_NO"].ToString() != null ? (dsRow["USD_DOOR_NO"].ToString() + ",") : "") + (dsRow["USD_STREET_NAME"].ToString() != null ? (dsRow["USD_STREET_NAME"].ToString() + ",") : "") + (dsRow["USD_LOCATION"].ToString() != null ? (dsRow["USD_LOCATION"].ToString() + ",") : "") + (dsRow["USD_DISTRICT"].ToString() != null ? (dsRow["USD_DISTRICT"].ToString() + ",") : "")));
                            strStatus = "00";
                            strMsg = "";
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
            
            }
            return Json(new {Status=strStatus,Message=strMsg });
        }

    }
}
