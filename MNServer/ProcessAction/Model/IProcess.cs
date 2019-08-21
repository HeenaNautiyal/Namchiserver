using System;
using MNServer.ProcessAction.Action;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MNServer.ProcessAction.Model
{
    public class IProcess
    {
        IProcessActionSeq ObjIProcessActionSeq { get; set; }
        string process_name { get; set; }
        string process_desc { get; set; }
        string createdBy { get; set; }
        DateTime creationTime { get; set; }
        string deactivateBy { get; set; }
        DateTime deactivatonTime { get; set; }
        Boolean isActive { get; set; }
        string initiatedBy { get; set; }
        DateTime initiationTime { get; set; }
        int processID { get; set; }
        string nextAction { get; set; }
    }
}