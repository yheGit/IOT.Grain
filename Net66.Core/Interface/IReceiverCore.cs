using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Core.Interface
{
    public interface IReceiverCore
    {
        bool Install(IReceiver _entity, out string c_short);

        bool AddHum(IReceiver _entity);
    }
}
