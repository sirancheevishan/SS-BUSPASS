using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BusPassApp.Models
{
    public class BaseClass : Controller
    {
        //
        // GET: /BaseClass/

        public class RegistrationDetails{
           public string strName{get;set;}
            public string strFName{get;set;}
            public string strGender{get;set;}
            public string strDob{get;set;}
            public string strAdhaar{get;set;}
            public string strMbl{get;set;}
            public string strDoorno{get;set;}
            public string strStreet{get;set;}
            public string strVillage{get;set;}
            public string strDistrict{get;set;}
            public string strState{get;set;}
            public string strPin{get;set;}
            public string strMail{get;set;}
            public string strPass{get;set;}
            public string strImage{get;set;}
            public string strpdf{get;set;}
        }
        public class BookingRQRS
        {
            public string strUserID { get; set; }
            public string strTitle { get; set; }
            public string strFName { get; set; }
            public string strLName { get; set; }
            public string strDOB { get; set; }
            public string strGender { get; set; }
            public string strPhnNo { get; set; }
            public string strMail { get; set; }
            public string strPassCode { get; set; }
            public string strPassType { get; set; }
            public string strPassName { get; set; }
            public string stTrackID { get; set; }
            public string strPassNo { get; set; }
            public string strStatus { get; set; }
            public string strValildFrom { get; set; }
            public string strValildTo { get; set; }
            public string strCurrency { get; set; }
            public string strAmount { get; set; }
            public string strRemark { get; set; }
            public string strBookedDate { get; set; }
            public string strUpdateDate { get; set; }
            public string strFlag { get; set; }
            public string strAdhaarNo { get; set; }

        }

        public class PGRQRS
        {

            public string strUserID { get; set; }
            public string stTrackID { get; set; }
            public string strUserName { get; set; }
            public string strMail { get; set; }
            public string strContactNo { get; set; }
            public string strIpAddress { get; set; }
            public string strAdhaarNo { get; set; }
            public string strPassNo { get; set; }
            public string strTrackID { get; set; }
            public string strPaymentID { get; set; }
            public string strOrderID { get; set; }
            public string strCurrency { get; set; }
            public string strAmount { get; set; }
            public string strTrackStatusCode { get; set; }
            public string strTrackStatus { get; set; }
            public string strRemark { get; set; }
            public string strCreatedDate { get; set; }
            public string strUpdateDate { get; set; }
            public string strFlag { get; set; }

        }
       
    }
    public class Utilities {
        public static string LoadServerdatetime()
        {
            double GMT = ConfigurationManager.AppSettings["GMTTIME"].ToString() == "" ? 0 : Convert.ToDouble(ConfigurationManager.AppSettings["GMTTIME"].ToString());
            DateTime dateTime = DateTime.Now.AddMinutes(GMT);//.Now.Date;

            //DatabaseLog.LogData(HttpContext.Current.Session["Loginusermailid"] != null ? HttpContext.Current.Session["Loginusermailid"].ToString() : "", "E", "FLIGHTCONTROLLER", "FLIGHTS ONLOAD GET SERVER DATE TIME", dateTime.ToString("dd/MM/yyyy"), HttpContext.Current.Session["CompanyID"] != null ? HttpContext.Current.Session["CompanyID"].ToString() : "", HttpContext.Current.Session["UserID"] != null ? HttpContext.Current.Session["UserID"].ToString() : "", HttpContext.Current.Session["sequenceid"] != null ? HttpContext.Current.Session["sequenceid"].ToString() : "0", HttpContext.Current.Session["IPAddress"] != null ? HttpContext.Current.Session["IPAddress"].ToString() : "");

            return dateTime.ToString("dd/MM/yyyy");
        }
        public static string RandomOTPGeneration(ref string GenerateOTP)
        {
            GenerateOTP = string.Empty;
            try
            {
                string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
                string numbers = "1234567890";

                string characters = numbers;// alphabets + small_alphabets + numbers;

                string otp = string.Empty;
                int length = 5;
                for (int i = 0; i < length; i++)
                {
                    string character = string.Empty;
                    do
                    {
                        int index = new Random().Next(0, characters.Length);
                        character = characters.ToCharArray()[index].ToString();
                    } while (otp.IndexOf(character) != -1);
                    otp += character;
                }
                GenerateOTP = otp;
            }
            catch (Exception ex)
            {


            }

            return GenerateOTP;
        }
        public static string GetPublicIpAddress()
        {
            var request = (HttpWebRequest)WebRequest.Create("http://ifconfig.me");

            request.UserAgent = "curl"; // this will tell the server to return the information as if the request was made by the linux "curl" command

            string publicIPAddress;

            request.Method = "GET";
            using (WebResponse response = request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    publicIPAddress = reader.ReadToEnd();
                }
            }

            return publicIPAddress.Replace("\n", "");
        }
        public static bool SendSMS(string strMobNo,string strContent)
        {
            
            bool result = false;
            

            

            return result;
        }
        public static bool SendMail(string strMailID, string strMailContent)
        {

            bool result = false;




            return result;
        }
   }

}
