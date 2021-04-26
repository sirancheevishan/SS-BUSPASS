using BusPassApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BusPassApp.Controllers
{
    public class PaymentController : Controller
    {
        // GET: Payment
        BusPassApp.WSDL_Service.BusPassApp Ws_Service = new WSDL_Service.BusPassApp();
        public ActionResult Index(Models.PaymentInitiateModel _requestData)
        {
            Session.Add("PaymentString", JsonConvert.SerializeObject(_requestData));
            return View();
        }


        //[HttpPost]
        public ActionResult CreateOrder() //Models.PaymentInitiateModel _requestData
        {

            Models.PaymentInitiateModel _requestData=JsonConvert.DeserializeObject<Models.PaymentInitiateModel>(Convert.ToString(Session["PAYMENTRQRS"]));

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;


            OrderModel orderModel = new OrderModel();
            try
            {
                // Generate random receipt number for order
                Random randomObj = new Random();
                string transactionId = randomObj.Next(10000000, 100000000).ToString();

                Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient("rzp_test_PUIxVIZT1Ae6fR", "lvOoPdVuu9fmkaBG8bnF7qdy");
                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", Convert.ToInt32(_requestData.amount) * 100);  // Amount will in paise
                options.Add("receipt", transactionId);
                options.Add("currency", "INR");
                options.Add("payment_capture", "0"); // 1 - automatic  , 0 - manual
                                                     //options.Add("notes", "-- You can put any notes here --");
                Razorpay.Api.Order orderResponse = client.Order.Create(options);
                string orderId = orderResponse["id"].ToString();


                // Create order model for return on view
                 orderModel = new OrderModel
                {
                    orderId = orderResponse.Attributes["id"],
                    razorpayKey = "rzp_test_PUIxVIZT1Ae6fR",
                    amount = Convert.ToInt32(_requestData.amount) * 100,
                    currency = "INR",
                    name = _requestData.name,
                    email = _requestData.email,
                    contactNumber = _requestData.contactNumber,
                    address = _requestData.address,
                    description = (_requestData.strPassType != null ? _requestData.strPassType :"")
                 };
                string str = "";
                Session.Add("PaymentString", JsonConvert.SerializeObject(orderModel));
            }
            catch (Exception ex)
            {
                string str = ex.ToString();
            }
            // Return on PaymentPage with Order data
            //return View("PaymentPage", orderModel);

            return View("PaymentPage");
        }
        /// <summary>
        /// /
        /// </summary>
        public class OrderModel
        {
            public string orderId { get; set; }
            public string razorpayKey { get; set; }
            public int amount { get; set; }
            public string currency { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public string contactNumber { get; set; }
            public string address { get; set; }
            public string description { get; set; }
        }


        [HttpPost]
        public ActionResult Complete()
        {
            string strUserID = Convert.ToString(Session["REGISTERED_NO"]);
            string strPhnNo = Convert.ToString(Session["MOBILE_NO"]);
            string strMailID = Convert.ToString(Session["MAIL_ID"]);
            string strBookingReq = string.Empty;
            string strXMLData = string.Empty;
            string strtime = string.Empty;
            string strErrMSG = string.Empty;
            DataSet ds_result = new DataSet();

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                string paymentId = Request.Params["rzp_paymentid"];
                string orderId = Request.Params["rzp_orderid"];

                Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient("rzp_test_PUIxVIZT1Ae6fR", "lvOoPdVuu9fmkaBG8bnF7qdy");

                Razorpay.Api.Payment payment = client.Payment.Fetch(paymentId);

                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", payment.Attributes["amount"]);
                Razorpay.Api.Payment paymentCaptured = payment.Capture(options);
                string amt = paymentCaptured.Attributes["amount"];

                
                if (paymentCaptured.Attributes["status"] == "captured")
                {

                    #region Update Track
                    BaseClass.BookingRQRS BookingRQRS = new BaseClass.BookingRQRS();
                    string strCurrency = ConfigurationManager.AppSettings["Currency"] != null && ConfigurationManager.AppSettings["Currency"].ToString() != "" ? ConfigurationManager.AppSettings["Currency"].ToString() : "";
                    BookingRQRS.strPassType = "";
                    BookingRQRS.strUserID = strUserID;
                    BookingRQRS.stTrackID = "";
                    BookingRQRS.strPassNo = "";
                    BookingRQRS.strRemark = "";
                    BookingRQRS.strBookedDate = "";
                    BookingRQRS.strUpdateDate = "";
                    BookingRQRS.strRemark = "";
                    BookingRQRS.strFlag = "U";
                    BookingRQRS.strStatus = "S";
                    BookingRQRS.strMail = strMailID;
                    BookingRQRS.strPhnNo = strPhnNo;
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
                    strtime = DateTime.Now.ToString("DDMMYYYY HH:MM:SS:FFFF");
                    strXMLData = "<EVENT><RESPONSE>inserRegistrationDetails</RESPONSE>";
                    strXMLData += "<RESTTIME>" + strtime + "</RESTTIME>";
                    strXMLData += "<ERROMSG>" + strErrMSG + "</ERROMSG>";
                    strXMLData += "<RESDATA>" + JsonConvert.SerializeObject(ds_result) + "</RESDATA><EVENT>";

                    if (ds_result != null && ds_result.Tables.Count > 0 && ds_result.Tables[0].Rows.Count > 0)
                    {
                        if (ds_result.Tables[0].Rows[0]["Result"].ToString() == "1")
                        {

                        }
                    }
                    #endregion

                    return RedirectToAction("Success");
                }
                else
                {
                    return RedirectToAction("Failed");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Success");
            }
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult Failed()
        {
            return View();
        }
    }
}