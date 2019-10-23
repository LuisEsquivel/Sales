using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sales.Common.Models
{
    public class MyUserASP
    {

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public object LockoutEndDateUtd { get; set; }

        public bool AccessFailledCount { get; set; }

        public List<Claim> Claims { get; set; }

        public string Id { get; set; }

        public string UserName { get; set; }

    }
}