using Net66.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Core.Interface
{
    public interface IAlarmCore
    {
        bool Add(List<IAlarm> _list);

    }
}
