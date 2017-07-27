using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Core.Interface
{
    public interface IReceiverCore
    {
        bool InstallReceiver(IReceiver _entity, out string c_short);

        string AddTemAndHum(IReceiver _entity);
    }
}
