﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sales.Common.Models
{
    public class Claim
    {
       
        public int Id { get; set; }

        public string  UserId { get; set; }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }

    }
}