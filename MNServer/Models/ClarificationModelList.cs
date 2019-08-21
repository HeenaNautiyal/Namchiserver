using MNServer.ProcessAction.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MNServer.Models
{
    public class ClarificationModelList
    {
        public string Clarification { get; set; }
        public List<string> CFlist { get; set; }
        public List<BirthProcess> RegistrationForm { get; set; }
    }
}