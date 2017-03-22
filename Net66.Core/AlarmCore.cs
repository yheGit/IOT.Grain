using Net66.Comm;
using Net66.Core.Interface;
using Net66.Data.Interface;
using Net66.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Core
{
    public class AlarmCore: IAlarmCore
    {
        private static IGrainRepository<Alarm> aRepository;

        public AlarmCore(IGrainRepository<Alarm> _aRepository)
        {
            aRepository = _aRepository;
        }

        public bool Add(List<IAlarm> _list)
        {
            if (_list == null)
                return false;
            var datenow = Utils.GetServerDateTime();
            foreach (var model in _list)
            {
                aRepository.Add(new Alarm()
                {
                    CpuId = model.m_cpuid,
                    DateValue = model.data,
                    SensorId = model.tempid,
                    ShortCode = model.c_short,
                    StampTime = datenow,
                    Type = model.type2
                }, p => p.SensorId == model.tempid || p.ShortCode == model.c_short || p.CpuId == model.m_cpuid);
            }
            return true;
        }

        //取消报警，删除记录即可
    }
}
