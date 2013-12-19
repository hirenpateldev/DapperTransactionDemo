using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DapperDemo.Models
{
    public class PersonNote
    {
        public int ID { get; set; }
        public int PersonID { get; set; }
        public string Note { get; set; }
        public DateTime NoteCreatedOn { get; set; }

        public Person person { get; set; }

    }
}