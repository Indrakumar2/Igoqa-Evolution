using Evolution.Common.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Admin.Domain.Models.Admins
{
    public class User : BaseModel
    {
        public string CompanyCode { get; set; }

        public string Username { get; set; }

        public string DisplayName { get; set; }

        public bool? IsActive { get; set; }

        public string Culture { get; set; }

        public string Email { get; set; }

        public string CompanyOffice { get; set; }

        public string CompanyName { get; set; }

        public string UserType { get; set; }

        public string Status { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public int? userId { get; set; }
    }
}
