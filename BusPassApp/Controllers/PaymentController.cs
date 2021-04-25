using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BusPassApp.Controllers
{
    public class PaymentController : Controller
    {
        // GET: Payment
           
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
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            
            string paymentId = Request.Params["rzp_paymentid"];

            // This is orderId
            string orderId = Request.Params["rzp_orderid"];

            Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient("rzp_test_PUIxVIZT1Ae6fR", "lvOoPdVuu9fmkaBG8bnF7qdy");

            Razorpay.Api.Payment payment = client.Payment.Fetch(paymentId);

            // This code is for capture the payment 
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", payment.Attributes["amount"]);
            Razorpay.Api.Payment paymentCaptured = payment.Capture(options);
            string amt = paymentCaptured.Attributes["amount"];

            //// Check payment made successfully

            if (paymentCaptured.Attributes["status"] == "captured")
            {
                // Create these action method
                return RedirectToAction("Success");
            }
            else
            {
                return RedirectToAction("Failed");
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