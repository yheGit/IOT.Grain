﻿using Net66.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Core.Interface
{
    public interface ICollectorCore
    {
        string CollectorInstall(IReceiver _entity);
        bool AddTemp(List<ICollector> _list);

        string UploadMeasurers(List<ICollector> _list);
    }
}
