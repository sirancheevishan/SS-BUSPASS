using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusPassApp.Models
{
    public class RQRS : Controller
    {
        //
        // GET: /RQRS/

        public ActionResult Index()
        {
            return View();
        }
        public class OTPcheck
        {
            public string Status { get; set; }
            public bool result { get; set; }
            public string Message { get; set; }
           
        }

    }
}
