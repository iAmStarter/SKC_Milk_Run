using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SKC_Milk_Run
{
    public class PTTModel
    {
        public string PRODUCT { get; set; }
        public string PRICE { get; set; }
    }
    public class PTTModelList : List<PTTModel>
    {
    }
}