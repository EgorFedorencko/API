using MiNET.Plugins;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace InquiryService.Model
{
    public class Inquiry
    {
        public Guid client_id { get; set; }
        public string department_address { get; set; }
        public decimal amout { get; set; }
        public string UAN { get; set; }
        public int status { get; set; }
    }
}
