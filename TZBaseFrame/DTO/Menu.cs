using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TZManageAPI.DTO
{
    public class Menu
    {
        public int FID { get; set; }
        public int FParentID { get; set; }
        public string FUrlPath { get; set; }
        public string FName { get; set; }
        public int? FBillTypeID { get; set; }
        public string FParameters { get; set; }

        public int? FSort { get; set; }
        public List<Menu> FChild { get; set; }
    }
}