using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNServer.ProcessAction.Action
{
    interface IProcessAction
    {
       

        SortedDictionary<int, string> ProcessActionseq(int PID);

        #region insert
        string create(Model.IProcess process,int Id,int custid,string Type);
        #endregion
        #region Edit 
        string edit(Model.IProcess process, int Id, int custid, string Type);
        #endregion
        #region delete 
        string delete();
        #endregion
        #region View 
        List<Model.IProcess> view();
        #endregion
    }
}
