﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusPassApp.Models
{
    public class PaymentInitiateModel
    {
        public string name { get; set; }
        public string email { get; set; }
        public string contactNumber { get; set; }
        public string address { get; set; }
        public string amount { get; set; }
        public string strTrackID { get; set; }
        public string strPassType { get; set; }
    }
}