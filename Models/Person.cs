using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DapperDemo.Models
{
    public class Person
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public DateTime CreatedDate { get; set; }
    }


}