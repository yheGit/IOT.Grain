using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Entity.IO_Model
{
    /// <summary>
    /// Iot设备发送的pack
    /// </summary>
    public class IPacks
    {
        public List<ICollector> Measurers { get; set; }

        public IReceiver Collector { get; set; }

        public List<IAlarm> Alarms { get; set; }

    }
}

/// <summary>
/// 收集器
/// </summary>
public class ICollector
{
    /// <summary>
    /// 收集器手工编号
    /// </summary>
    public string c_short { get; set; }
    /// <summary>
    /// 采集器cpuid
    /// </summary>
    public string m_cpuid { get; set; }
    /// <summary>
    /// 本采集器总共多少个温度id --改成一次传完
    /// </summary>
    public int sum { get; set; }
    /// <summary>
    /// 温度id的顺序,以后发的温度也是按照这个顺序排列
    /// </summary>
    public List<string> SensorId { get; set; }
    /// <summary>
    /// 温度值
    /// </summary>
    public string temp { get; set; }

    /// <summary>
    /// 1温度、 2ID、3线编号,
    /// </summary>
    public string type { get; set; }

    public string label { get; set; }

}

/// <summary>
/// 收集器/采集器安装
/// </summary>
public class IReceiver
{
    /// <summary>
    /// 0更新数据， 1是采集器安装，2是收集器安装,
    /// </summary>
    public string type { get; set; }

    /// <summary>
    /// 收集器短编号 ，与采集器安装相关
    ///  </summary>
    public string c_short { get; set; }
    /// <summary>
    /// 收集器/或者采集器CPUID--加c_
    /// </summary>
    public string c_cpuid { get; set; }
    /// <summary>
    /// L1栋楼 --这些参数可以由收集器自己发出,也可以由服务器修改
    /// </summary>
    public string building { get; set; }
    /// <summary>
    /// 第一层
    /// </summary>
    public string layer { get; set; }
    /// <summary>
    /// 第二的个廒间
    /// </summary>
    public string room { get; set; }
    /// <summary>
    /// 堆号 ，与采集器安装相关
    ///  </summary>
    public string heap { get; set; }
    /// <summary>
    /// 第二个采集器（码层）
    /// </summary>
    public string sublayer { get; set; }
    /// <summary>
    /// 收集器自己的湿度值无小数
    /// </summary>
    public string hum { get; set; }
    /// <summary>
    /// 收集器自己的温度值（仓外温度）
    /// </summary>
    public string temp { get; set; }
    /// <summary>
    /// 指定分机所在排
    /// </summary>
    public string column { get; set;  }

}

/// <summary>
/// 报警
/// </summary>
public class IAlarm
{
    /// <summary>
    ///电池低电报警
    /// </summary>
    public int type2 { get; set; }
    /// <summary>
    /// 湿度有关
    /// </summary>
    public string c_short { get; set; }
    /// <summary>
    /// 温度,电压有关
    /// </summary>
    public string m_cpuid { get; set; }
    /// <summary>
    /// 温度有关
    /// </summary>
    public string tempid { get; set; }
    /// <summary>
    /// 都有关
    /// </summary>
    public string data { get; set; }
}


