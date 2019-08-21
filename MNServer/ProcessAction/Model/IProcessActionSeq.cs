using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNServer.ProcessAction.Model
{
    interface IProcessActionSeq
    {
        SortedDictionary<int, String> sortedDict { set; get; }
    }
}
