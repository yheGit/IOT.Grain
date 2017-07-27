using IOT.RightsSys.Entity;
using Net66.Comm;
using Net66.Comm.vxin;
using Net66.Data.Base;
using Net66.Entity.IO_Model;
using Net66.Entity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Data.Context
{
    public class DbEntity
    {
        /// <summary>
        /// 年月日初始时间
        /// </summary>
        private static string ymdTest = string.Empty;
        /// <summary>
        /// 本周最后一天
        /// </summary>
        private static string lastdayTest = string.Empty;

        #region  温度堆位、仓内、仓外(粮堆)
        /// <summary>
        /// 有效取温度的变化值，以便渲染温度变化 2017-06-27 01:54:32
        /// </summary>
        public int TempsChartDataDay(string wh_number, string g_number, string h_number, string sensorGuid, int type = 1)
        {
            var datenow = Utils.GetServerDateTime();
            var updatetime = datenow;
            var datenowStr = updatetime.ToString();
            var msg = "wh_number:" + wh_number + "&&&g_number:" + g_number + "&&&h_number:" + h_number + "&&&type:" + type;
            Utils.PrintLog(msg, "TempsChartData", "DebugLog");
            var timeflag = "DAY";
            var tels = "";
            using (GrainContext db = new GrainContext())
            {
                #region (24H)客户端自定义 每8小时一次
                datenow = datenow.AddHours(-8);
                datenowStr = updatetime.ToString();
                List<Temperature> addDayTemp = new List<Temperature>();

                var lasttime = updatetime.AddHours(-1);

                if (type == 1)
                {
                    #region 粮堆温
                    var templist1 = db.Temperatures.Where(f => f.H_Number.Equals(h_number) && f.UpdateTime >= datenow
                    && (f.Type == 0 || f.Type == 1) && (f.TimeFlag.Equals(timeflag) || f.TimeFlag.Equals("-"))).ToList();
                    var info1 = templist1.FirstOrDefault(f => f.Type == 1 && f.RealHeart == 1 && f.UpdateTime >= lasttime);
                    if (info1 != null)
                    {//密度太大，删除上一次的，以最后一次为准
                        info1.Type = -4;
                        db.Temperatures.Attach(info1);
                        db.Entry(info1).State = EntityState.Modified;
                    }

                    var averTepms = templist1.Where(f => f.RealHeart == 0).Average(a => a.Temp) ?? 0;
                    addDayTemp.Add(new Temperature()
                    {
                        PId = sensorGuid,//MONTH,YEAR
                        StampTime = datenowStr,
                        UpdateTime = updatetime,
                        Temp = averTepms,
                        RealHeart = 1,
                        Type = 1,
                        WH_Number = wh_number,
                        G_Number = g_number,
                        H_Number = h_number,
                        TimeFlag = timeflag
                    });

                    #endregion //粮堆温
                }

                if (type == 2)
                {
                    #region 仓内温
                    var templist2 = db.Temperatures.Where(f => f.G_Number.Equals(g_number) && f.UpdateTime >= datenow
                    && f.Type == 2 && (f.TimeFlag.Equals(timeflag) || f.TimeFlag.Equals("-"))).ToList();
                    decimal insidemax = templist2.Where(f => f.RealHeart == 0).Max(s => s.Temp) ?? -1000;
                    var info2 = templist2.FirstOrDefault(f => f.RealHeart == 1 && f.UpdateTime >= lasttime);
                    if (info2 != null)
                    {
                        info2.Type = -4;
                        db.Temperatures.Attach(info2);
                        db.Entry(info2).State = EntityState.Modified;
                    }
                        
                    if (insidemax != -1000)
                    {
                        addDayTemp.Add(new Temperature()
                        {
                            PId = sensorGuid,//MONTH,YEAR
                            StampTime = datenowStr,
                            UpdateTime = updatetime,
                            Temp = insidemax,
                            RealHeart = 1,
                            Type = 2,
                            WH_Number = wh_number,
                            G_Number = g_number,
                            H_Number = h_number,
                            TimeFlag = timeflag
                        });
                    }
                    #endregion //仓内温
                }

                if (type == 3)
                {
                    #region 仓外温
                    var templist3 = db.Temperatures.Where(f => f.WH_Number.Equals(wh_number) && f.G_Number.Equals("0") && f.UpdateTime >= datenow
                    && f.Type == 3 && (f.TimeFlag.Equals(timeflag) || f.TimeFlag.Equals("-"))).ToList();
                    decimal outsidemax = templist3.Where(f => f.RealHeart == 0).Max(s => s.Temp) ?? -1000;
                    var info3 = templist3.FirstOrDefault(f => f.RealHeart == 1 && f.UpdateTime >= lasttime);
                    if (info3 != null)
                    {
                        info3.Type = -4;
                        db.Temperatures.Attach(info3);
                        db.Entry(info3).State = EntityState.Modified;
                    }
                    if (outsidemax != -1000)
                    {
                        addDayTemp.Add(new Temperature()
                        {
                            PId = sensorGuid,//MONTH,YEAR
                            StampTime = datenowStr,
                            UpdateTime = updatetime,
                            Temp = outsidemax,
                            RealHeart = 1,
                            Type = 3,
                            WH_Number = wh_number,
                            G_Number = g_number,
                            H_Number = h_number,
                            TimeFlag = timeflag
                        });
                    }
                    #endregion //仓外温
                }

                db.Temperatures.AddRange(addDayTemp);
                var json = JsonConvertHelper.SerializeObject(addDayTemp);
                Utils.PrintLog(json, "TempsChartData-addDayTemp", "OutParamLog");

                #region SMS 2017-6-28 22:33:11
                #endregion

                #endregion 客户端自定义      
                try
                {
                    return db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Utils.ExceptionLog(ex, "TempsChartData");
                    return -22;
                }
            }

        }

        public int TempsChartDataMonth(string wh_number, string g_number, string h_number, string sensorGuid, int type = 1)
        {
            var datenow = Utils.GetServerDateTime();
            var updatetime = datenow;
            var datenowStr = updatetime.ToString();
            var msg = "wh_number:" + wh_number + "&&&g_number:" + g_number + "&&&h_number:" + h_number + "&&&type:" + type;
            Utils.PrintLog(msg, "TempsChartData", "DebugLog");
            var timeflag = "MONTH";
            var tels = "";
            using (GrainContext db = new GrainContext())
            {

                #region 1Month最近一个月(每天取一次，今天计算昨天的)              
                List<Temperature> addMonthTemp = null;
                string ymd = datenow.Year + "-" + datenow.Month + "-" + datenow.Day;
                var ymdInfo = db.Temperatures.OrderByDescending(d => d.UpdateTime)
                    .FirstOrDefault(f => f.H_Number == h_number && f.TimeFlag == timeflag && f.Type == type);
                ymdTest = "";
                if (ymdInfo != null)
                {
                    var ymgTime = ymdInfo.UpdateTime ?? datenow;
                    ymdTest = ymgTime.Year + "-" + ymgTime.Month + "-" + ymgTime.Day;
                }
                if (!ymdTest.Equals(ymd))
                {
                    addMonthTemp = new List<Temperature>();
                    DateTime starttime = Convert.ToDateTime(datenow.ToString("yyyy-MM-dd ") + "00:01:00");
                    datenow = starttime.AddHours(-24);
                    datenowStr = updatetime.ToString();
                    ymdTest = ymd;

                    if (type == 1)
                    {
                        #region 粮堆温
                        var m_average = db.Temperatures.Where(f => f.H_Number.Equals(h_number) && f.UpdateTime >= datenow && f.Type == 1).Average(s => s.Temp) ?? -1000;
                        if (m_average != -1000)
                        {
                            addMonthTemp.Add(new Temperature()
                            {
                                PId = sensorGuid,//MONTH,YEAR
                                StampTime = datenowStr,
                                UpdateTime = updatetime,
                                Temp = m_average,
                                RealHeart = 1,
                                Type = 1,
                                WH_Number = wh_number,
                                G_Number = g_number,
                                H_Number = h_number,
                                TimeFlag = timeflag
                            });
                        }
                        #endregion //粮堆温

                    }
                    if (type == 2)
                    {
                        #region 仓内温
                        var m_insidemax = db.Temperatures.Where(f => f.G_Number.Equals(g_number) && f.UpdateTime >= datenow && f.Type == 2).Max(s => s.Temp) ?? -1000;
                        if (m_insidemax != -1000)
                        {
                            addMonthTemp.Add(new Temperature()
                            {
                                PId = sensorGuid,//MONTH,YEAR
                                StampTime = datenowStr,
                                UpdateTime = updatetime,
                                Temp = m_insidemax,
                                RealHeart = 1,
                                Type = 2,
                                WH_Number = wh_number,
                                G_Number = g_number,
                                H_Number = h_number,
                                TimeFlag = timeflag
                            });
                        }
                        #endregion //仓内温
                    }
                    if (type == 3)
                    {
                        #region 仓外温
                        var m_outsidemax = db.Temperatures.Where(f => f.WH_Number.Equals(wh_number) && f.G_Number.Equals("0") && f.UpdateTime >= datenow && f.Type == 3).Max(s => s.Temp) ?? -1000;
                        if (m_outsidemax != -1000)
                        {
                            addMonthTemp.Add(new Temperature()
                            {
                                PId = sensorGuid,//MONTH,YEAR
                                StampTime = datenowStr,
                                UpdateTime = updatetime,
                                Temp = m_outsidemax,
                                RealHeart = 1,
                                Type = 3,
                                WH_Number = wh_number,
                                G_Number = g_number,
                                H_Number = h_number,
                                TimeFlag = timeflag
                            });
                        }
                        #endregion //仓外温
                    }

                    db.Temperatures.AddRange(addMonthTemp);
                    var monthjson = JsonConvertHelper.SerializeObject(addMonthTemp);
                    Utils.PrintLog(monthjson, "TempsChartData-addMonthTemp", "OutParamLog");

                    #region SMS 2017-6-28 22:33:11
                    #endregion
                }
                #endregion (1Month)最近一个月

                try
                {
                    return db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Utils.ExceptionLog(ex, "TempsChartData");
                    return -22;
                }
            }

        }

        public int TempsChartDataYear(string wh_number, string g_number, string h_number, string sensorGuid, int type = 1)
        {
            var datenow = Utils.GetServerDateTime();
            var updatetime = datenow;
            var datenowStr = updatetime.ToString();
            var msg = "wh_number:" + wh_number + "&&&g_number:" + g_number + "&&&h_number:" + h_number + "&&&type:" + type;
            Utils.PrintLog(msg, "TempsChartData", "DebugLog");
            var timeflag = "YEAR";
            var tels = "";
            using (GrainContext db = new GrainContext())
            {
                #region 1YEAR最近一年(每周取一次，每周日凌晨计算上周数据)              
                List<Temperature> addYearTemp = null;
                DateTime lastdaytime = Utils.GetWeekLastDaySun(datenow);
                string lastday = lastdaytime.ToString();
                var ymgInfo = db.Temperatures.OrderByDescending(d => d.UpdateTime)
                    .FirstOrDefault(f => f.H_Number == h_number && f.TimeFlag == timeflag && f.Type == type);
                lastdayTest = "";
                if (ymgInfo != null)
                {
                    var ymgTime = ymgInfo.UpdateTime ?? datenow;
                    lastdayTest = Utils.GetWeekLastDaySun(ymgTime).ToString();
                }

                if (!lastdayTest.Equals(lastday))
                {
                    addYearTemp = new List<Temperature>();
                    DateTime starttime = lastdaytime;
                    datenow = starttime.AddDays(-7);
                    datenowStr = updatetime.ToString();
                    lastdayTest = lastday;

                    if (type == 1)
                    {
                        #region 粮堆温
                        var y_average = db.Temperatures.Where(f => f.H_Number.Equals(h_number) && f.UpdateTime >= datenow && f.Type == 1).Average(s => s.Temp) ?? -1000;
                        if (y_average != -1000)
                        {
                            addYearTemp.Add(new Temperature()
                            {
                                PId = sensorGuid,//MONTH,YEAR
                                StampTime = datenowStr,
                                UpdateTime = updatetime,
                                Temp = y_average,
                                RealHeart = 1,
                                Type = 1,
                                WH_Number = wh_number,
                                G_Number = g_number,
                                H_Number = h_number,
                                TimeFlag = timeflag
                            });
                        }
                        #endregion //粮堆温
                    }
                    if (type == 2)
                    {
                        #region 仓内温
                        var y_insidemax = db.Temperatures.Where(f => f.G_Number.Equals(g_number) && f.UpdateTime >= datenow && f.Type == 2).Max(s => s.Temp) ?? -1000;
                        if (y_insidemax != -1000)
                        {
                            addYearTemp.Add(new Temperature()
                            {
                                PId = sensorGuid,//MONTH,YEAR
                                StampTime = datenowStr,
                                UpdateTime = updatetime,
                                Temp = y_insidemax,
                                RealHeart = 1,
                                Type = 2,
                                WH_Number = wh_number,
                                G_Number = g_number,
                                H_Number = h_number,
                                TimeFlag = timeflag
                            });
                        }
                        #endregion //仓内温
                    }
                    if (type == 3)
                    {
                        #region 仓外温
                        var y_outsidemax = db.Temperatures.Where(f => f.WH_Number.Equals(wh_number) && f.G_Number.Equals("0") && f.UpdateTime >= datenow && f.Type == 3).Max(s => s.Temp) ?? -1000;
                        if (y_outsidemax != -1000)
                        {
                            addYearTemp.Add(new Temperature()
                            {
                                PId = sensorGuid,//MONTH,YEAR
                                StampTime = datenowStr,
                                UpdateTime = updatetime,
                                Temp = y_outsidemax,
                                RealHeart = 1,
                                Type = 3,
                                WH_Number = wh_number,
                                G_Number = g_number,
                                H_Number = h_number,
                                TimeFlag = timeflag
                            });
                        }
                        #endregion //仓外温
                    }

                    db.Temperatures.AddRange(addYearTemp);
                    var yearjson = JsonConvertHelper.SerializeObject(addYearTemp);
                    Utils.PrintLog(yearjson, "TempsChartData-addYearTemp", "OutParamLog");

                    #region SMS 2017-6-28 22:33:11
                    #endregion
                }
                #endregion 1YEAR最近一年


                try
                {
                    return db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Utils.ExceptionLog(ex, "TempsChartData");
                    return -22;
                }
            }

        }

        /// <summary>
        /// 2017-06-27 06:56:42
        /// </summary>
        public void AddTempsChartData(string wh_number, string g_number, string h_number, string pid, int type = 1)
        {
            try
            {
                TempsChartDataDay(wh_number, g_number, h_number, pid, type);
                TempsChartDataMonth(wh_number, g_number, h_number, pid, type);
                TempsChartDataYear(wh_number, g_number, h_number, pid, type);
            }
            catch (Exception ex)
            {
                Utils.ExceptionLog(ex, "AddTempsChartData");
            }
        }
        #endregion

        #region  温度堆位、仓内、仓外(传感器)

        /// <summary>
        /// 有效取温度的变化值，以便渲染温度变化 2017-06-27 01:54:32
        /// </summary>
        public int TempsChartData_SensorDay(string wh_number, string g_number, string h_number, string sensorGuid, decimal temp, int type = 1)
        {
            var datenow = Utils.GetServerDateTime();
            var updatetime = datenow;
            var datenowStr = updatetime.ToString();
            var msg = "wh_number:" + wh_number + "&&&g_number:" + g_number + "&&&h_number:" + h_number + "&&&temp:" + temp + "&&&type:" + type;
            Utils.PrintLog(msg, "TempsChartData", "DebugLog");
            var timeflag = "DAY";
            var tels = "";
            using (GrainContext db = new GrainContext())
            {
                #region (24H)客户端自定义 每8小时一次
                datenow = datenow.AddHours(-8);
                datenowStr = updatetime.ToString();
                List<Temperature> addDayTemp = new List<Temperature>();
                if (type == 1)
                {
                    //传感器
                    addDayTemp.Add(new Temperature()
                    {
                        PId = sensorGuid,//MONTH,YEAR
                        StampTime = datenowStr,
                        UpdateTime = updatetime,
                        Temp = temp,
                        RealHeart = 1,
                        Type = 0,
                        WH_Number = wh_number,
                        G_Number = g_number,
                        H_Number = h_number,
                        TimeFlag = timeflag
                    });
                }

                db.Temperatures.AddRange(addDayTemp);
                var json = JsonConvertHelper.SerializeObject(addDayTemp);
                Utils.PrintLog(json, "TempsChartData-addDayTemp", "OutParamLog");

                #region SMS 2017-6-28 22:33:11
                #endregion

                #endregion 客户端自定义

                try
                {
                    return db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Utils.ExceptionLog(ex, "TempsChartData");
                    return -22;
                }
            }

        }

        /// <summary>
        /// 有效取温度的变化值，以便渲染温度变化 2017-06-27 01:54:32
        /// </summary>
        public int TempsChartData_SensorMonth(string wh_number, string g_number, string h_number, string sensorGuid, decimal temp, int type = 1)
        {
            var datenow = Utils.GetServerDateTime();
            var updatetime = datenow;
            var datenowStr = updatetime.ToString();
            var msg = "wh_number:" + wh_number + "&&&g_number:" + g_number + "&&&h_number:" + h_number + "&&&temp:" + temp + "&&&type:" + type;
            Utils.PrintLog(msg, "TempsChartData", "DebugLog");
            var timeflag = "MONTH";
            var tels = "";
            using (GrainContext db = new GrainContext())
            {
                #region 1Month最近一个月(每天取一次，今天计算昨天的)              
                List<Temperature> addMonthTemp = null;
                string ymd = datenow.Year + "-" + datenow.Month + "-" + datenow.Day;
                var ymdInfo = db.Temperatures.OrderByDescending(d => d.UpdateTime).FirstOrDefault(f => f.PId == sensorGuid && f.TimeFlag == timeflag);
                ymdTest = "";
                if (ymdInfo != null)
                {
                    var ymgTime = ymdInfo.UpdateTime ?? datenow;
                    ymdTest = ymgTime.Year + "-" + ymgTime.Month + "-" + ymgTime.Day;
                }
                if (!ymdTest.Equals(ymd))
                {
                    addMonthTemp = new List<Temperature>();
                    DateTime starttime = Convert.ToDateTime(datenow.ToString("yyyy-MM-dd ") + "00:01:00");
                    datenow = starttime.AddHours(-24);
                    datenowStr = updatetime.ToString();
                    ymdTest = ymd;

                    if (type == 1)
                    {
                        var monthaverage = db.Temperatures.Where(f => f.H_Number == h_number && f.PId == sensorGuid && f.UpdateTime >= datenow && f.Type == 0).Average(s => s.Temp) ?? -1000;
                        if (monthaverage != -1000)
                        {
                            addMonthTemp.Add(new Temperature()
                            {
                                PId = sensorGuid,//MONTH,YEAR
                                StampTime = datenowStr,
                                UpdateTime = updatetime,
                                Temp = monthaverage,
                                RealHeart = 1,
                                Type = 0,
                                WH_Number = wh_number,
                                G_Number = g_number,
                                H_Number = h_number,
                                TimeFlag = timeflag
                            });
                        }

                    }

                    db.Temperatures.AddRange(addMonthTemp);
                    var monthjson = JsonConvertHelper.SerializeObject(addMonthTemp);
                    Utils.PrintLog(monthjson, "TempsChartData-addMonthTemp", "OutParamLog");

                    #region SMS 2017-6-28 22:33:11                
                    #endregion
                }
                #endregion (1Month)最近一个月

                try
                {
                    return db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Utils.ExceptionLog(ex, "TempsChartData");
                    return -22;
                }
            }

        }

        /// <summary>
        /// 有效取温度的变化值，以便渲染温度变化 2017-06-27 01:54:32
        /// </summary>
        public int TempsChartData_SensorYear(string wh_number, string g_number, string h_number, string sensorGuid, decimal temp, int type = 1)
        {
            var datenow = Utils.GetServerDateTime();
            var updatetime = datenow;
            var datenowStr = updatetime.ToString();
            var msg = "wh_number:" + wh_number + "&&&g_number:" + g_number + "&&&h_number:" + h_number + "&&&temp:" + temp + "&&&type:" + type;
            Utils.PrintLog(msg, "TempsChartData", "DebugLog");
            var timeflag = "YEAR";
            var tels = "";
            using (GrainContext db = new GrainContext())
            {
                #region 1YEAR最近一年(每周取一次，每周日凌晨计算上周数据)              
                List<Temperature> addYearTemp = null;
                DateTime lastdaytime = Utils.GetWeekLastDaySun(datenow);
                string lastday = lastdaytime.ToString();
                var ymgInfo = db.Temperatures.OrderByDescending(d => d.UpdateTime).FirstOrDefault(f => f.PId == sensorGuid && f.TimeFlag == timeflag);
                lastdayTest = "";
                if (ymgInfo != null)
                {
                    var ymgTime = ymgInfo.UpdateTime ?? datenow;
                    lastdayTest = Utils.GetWeekLastDaySun(ymgTime).ToString();
                }
                if (!lastdayTest.Equals(lastday))
                {
                    addYearTemp = new List<Temperature>();
                    DateTime starttime = lastdaytime;
                    datenow = starttime.AddDays(-7);
                    datenowStr = updatetime.ToString();
                    lastdayTest = lastday;

                    if (type == 1)
                    {
                        var yearaverage = db.Temperatures.Where(f => f.H_Number == h_number && f.PId == sensorGuid && f.UpdateTime >= datenow && f.Type == 0).Average(s => s.Temp) ?? -1000;
                        if (yearaverage != -1000)
                        {
                            addYearTemp.Add(new Temperature()
                            {
                                PId = sensorGuid,//MONTH,YEAR
                                StampTime = datenowStr,
                                UpdateTime = updatetime,
                                Temp = yearaverage,
                                RealHeart = 1,
                                Type = 0,
                                WH_Number = wh_number,
                                G_Number = g_number,
                                H_Number = h_number,
                                TimeFlag = timeflag
                            });
                        }

                    }
                    db.Temperatures.AddRange(addYearTemp);
                    var yearjson = JsonConvertHelper.SerializeObject(addYearTemp);
                    Utils.PrintLog(yearjson, "TempsChartData-addYearTemp", "OutParamLog");

                    #region SMS 2017-6-28 22:33:11
                    #endregion
                }
                #endregion 1YEAR最近一年

                try
                {
                    return db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Utils.ExceptionLog(ex, "TempsChartData");
                    return -22;
                }
            }

        }

        /// <summary>
        /// 2017-06-27 06:56:42
        /// </summary>
        public void AddTempsChartData2(string wh_number, string g_number, string h_number, string sensorGuid, decimal temp, int type = 1)
        {
            try
            {
                TempsChartData_SensorDay(wh_number, g_number, h_number, sensorGuid, temp, type);
                TempsChartData_SensorMonth(wh_number, g_number, h_number, sensorGuid, temp, type);
                TempsChartData_SensorYear(wh_number, g_number, h_number, sensorGuid, temp, type);
            }
            catch (Exception ex)
            {
                Utils.ExceptionLog(ex, "AddTempsChartData2");
            }
        }
        #endregion

        #region  湿度堆位、仓内、仓外
        /// <summary>
        /// 有效取湿度的变化值，以便渲染湿度变化 2017-06-27 01:54:32
        /// </summary>
        public int HumsChartData(string wh_number, string g_number, string h_number, decimal temp, int type = 0)
        {
            var datenow = Utils.GetServerDateTime();
            var datenowStr = datenow.ToString();
            var updateStr = datenowStr;

            using (GrainContext db = new GrainContext())
            {
                #region (24H)客户端自定义 每8小时一次
                datenow = datenow.AddHours(-8);
                datenowStr = datenow.ToString();
                List<Humidity> addDayHum = new List<Humidity>();

                if (type == 0)
                {
                    var insidemax = db.Humidities.Where(f => f.G_Number == g_number && string.Compare(f.StampTime, datenowStr) >= 0 && f.Type == 0).Max(s => s.Humility);
                    addDayHum.Add(new Humidity()
                    {
                        ReceiverId = -1,//MONTH,YEAR
                        StampTime = updateStr,
                        //UpdateTime = datenow,
                        Temp = insidemax,
                        RealHeart = 1,
                        Type = 0,
                        WH_Number = wh_number,
                        G_Number = g_number,
                        H_Number = h_number,
                        TimeFlag = "DAY"
                    });
                }
                if (type == 1)
                {
                    var outsidemax = db.Humidities.Where(f => f.G_Number == g_number && string.Compare(f.StampTime, datenowStr) >= 0 && f.Type == 1).Max(s => s.Humility);
                    addDayHum.Add(new Humidity()
                    {
                        ReceiverId = -1,//MONTH,YEAR
                        StampTime = updateStr,
                        //UpdateTime = datenow,
                        Temp = outsidemax,
                        RealHeart = 1,
                        Type = 1,
                        WH_Number = wh_number,
                        G_Number = g_number,
                        H_Number = h_number,
                        TimeFlag = "DAY"
                    });
                }

                if (Utils.DebugApp)
                {
                    var msg = JsonConvertHelper.SerializeObject(addDayHum);
                    Utils.PrintLog(msg, "HumsChartData", "DebugLog");
                }

                db.Humidities.AddRange(addDayHum);
                #endregion 客户端自定义

                #region 1Month最近一个月(每天取一次，今天计算昨天的)              
                List<Humidity> addMonthHum = null;
                string ymd = datenow.Year + "-" + datenow.Month + "-" + datenow.Day;
                if (!ymdTest.Equals(ymd))
                {
                    addMonthHum = new List<Humidity>();
                    DateTime starttime = Convert.ToDateTime(datenow.ToString("yyyy-MM-dd ") + "00:01:00");
                    datenow = starttime.AddHours(-24);
                    datenowStr = datenow.ToString();
                    ymdTest = ymd;
                    if (type == 0)
                    {
                        var m_insidemax = db.Humidities.Where(f => f.G_Number == g_number && string.Compare(f.StampTime, datenowStr) >= 0 && f.Type == 0).Max(s => s.Humility);
                        addMonthHum.Add(new Humidity()
                        {
                            ReceiverId = -1,//MONTH,YEAR
                            StampTime = updateStr,
                            //UpdateTime = datenow,
                            Temp = m_insidemax,
                            RealHeart = 1,
                            Type = 0,
                            WH_Number = wh_number,
                            G_Number = g_number,
                            H_Number = h_number,
                            TimeFlag = "DAY"
                        });
                    }
                    if (type == 1)
                    {
                        var m_outsidemax = db.Humidities.Where(f => f.G_Number == g_number && string.Compare(f.StampTime, datenowStr) >= 0 && f.Type == 1).Max(s => s.Humility);
                        addMonthHum.Add(new Humidity()
                        {
                            ReceiverId = -1,//MONTH,YEAR
                            StampTime = updateStr,
                            //UpdateTime = datenow,
                            Temp = m_outsidemax,
                            RealHeart = 1,
                            Type = 1,
                            WH_Number = wh_number,
                            G_Number = g_number,
                            H_Number = h_number,
                            TimeFlag = "DAY"
                        });
                    }
                }
                db.Humidities.AddRange(addMonthHum);
                #endregion (1Month)最近一个月

                #region 1YEAR最近一年(每周取一次，每周日凌晨计算上周数据)              
                List<Humidity> addYearHum = null;
                DateTime lastdaytime = Utils.GetWeekLastDaySun(datenow);
                string lastday = lastdaytime.ToString();
                if (!lastdayTest.Equals(lastday))
                {
                    addYearHum = new List<Humidity>();
                    DateTime starttime = lastdaytime;
                    datenow = starttime.AddDays(-7);
                    datenowStr = datenow.ToString();
                    lastdayTest = lastday;

                    if (type == 0)
                    {
                        var y_insidemax = db.Humidities.Where(f => f.G_Number == g_number && string.Compare(f.StampTime, datenowStr) >= 0 && f.Type == 0).Max(s => s.Humility);
                        addYearHum.Add(new Humidity()
                        {
                            ReceiverId = -1,//MONTH,YEAR
                            StampTime = updateStr,
                            //UpdateTime = datenow,
                            Temp = y_insidemax,
                            RealHeart = 1,
                            Type = 0,
                            WH_Number = wh_number,
                            G_Number = g_number,
                            H_Number = h_number,
                            TimeFlag = "DAY"
                        });
                    }
                    if (type == 1)
                    {
                        var y_outsidemax = db.Humidities.Where(f => f.G_Number == g_number && string.Compare(f.StampTime, datenowStr) >= 0 && f.Type == 1).Max(s => s.Humility);
                        addYearHum.Add(new Humidity()
                        {
                            ReceiverId = -1,//MONTH,YEAR
                            StampTime = updateStr,
                            //UpdateTime = datenow,
                            Temp = y_outsidemax,
                            RealHeart = 1,
                            Type = 1,
                            WH_Number = wh_number,
                            G_Number = g_number,
                            H_Number = h_number,
                            TimeFlag = "DAY"
                        });
                    }
                }
                db.Humidities.AddRange(addYearHum);
                #endregion (1Month)最近一个月

                try
                {
                    return db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Utils.ExceptionLog(ex, "HumsChartData");
                    return -22;
                }
            }

        }

        /// <summary>
        /// 2017-06-27 06:56:42
        /// </summary>
        public void AddHumsChartData(string wh_number, string g_number, string h_number, decimal temp, int type = 0)
        {
            try
            {
                HumsChartData(wh_number, g_number, h_number, temp, type);
            }
            catch (Exception ex)
            {
                Utils.ExceptionLog(ex, "AddHumsChartData");
            }
        }
        #endregion

        #region  delete 废弃

        #region  堆位
        /// <summary>
        /// 有效取温度的变化值，以便渲染温度变化 2017-06-27 01:54:32
        /// </summary>
        public int TempsChartData_Heap(string wh_number, string g_number, string h_number, decimal temp)
        {
            var datenow = Utils.GetServerDateTime();
            var datenowStr = datenow.ToString();

            using (GrainContext db = new GrainContext())
            {
                #region (24H)客户端自定义 每8小时一次
                datenow = datenow.AddHours(-8);
                datenowStr = datenow.ToString();
                List<Temperature> addDayTemp = new List<Temperature>();
                addDayTemp.Add(new Temperature()
                {
                    PId = "DAY",//MONTH,YEAR
                    StampTime = datenowStr,
                    UpdateTime = datenow,
                    Temp = temp,
                    RealHeart = 1,
                    Type = 1,
                    WH_Number = wh_number,
                    G_Number = g_number,
                    H_Number = h_number
                });
                var insidemax = db.Temperatures.Where(f => f.H_Number == h_number && f.UpdateTime >= datenow && f.Type == 2).Max(s => s.Temp);
                addDayTemp.Add(new Temperature()
                {
                    PId = "DAY",//MONTH,YEAR
                    StampTime = datenowStr,
                    UpdateTime = datenow,
                    Temp = insidemax,
                    RealHeart = 1,
                    Type = 2,
                    WH_Number = wh_number,
                    G_Number = g_number,
                    H_Number = h_number
                });
                var outsidemax = db.Temperatures.Where(f => f.H_Number == h_number && f.UpdateTime >= datenow && f.Type == 3).Max(s => s.Temp);
                addDayTemp.Add(new Temperature()
                {
                    PId = "DAY",//MONTH,YEAR
                    StampTime = datenowStr,
                    UpdateTime = datenow,
                    Temp = outsidemax,
                    RealHeart = 1,
                    Type = 3,
                    WH_Number = wh_number,
                    G_Number = g_number,
                    H_Number = h_number
                });

                db.Temperatures.AddRange(addDayTemp);
                #endregion 客户端自定义

                #region 1Month最近一个月(每天取一次，今天计算昨天的)              
                List<Temperature> addMonthTemp = null;
                string ymd = datenow.Year + "-" + datenow.Month + "-" + datenow.Day;
                if (!ymdTest.Equals(ymd))
                {
                    addMonthTemp = new List<Temperature>();
                    DateTime starttime = Convert.ToDateTime(datenow.ToString("yyyy-MM-dd ") + "00:01:00");
                    datenow = starttime.AddHours(-24);
                    datenowStr = datenow.ToString();
                    ymdTest = ymd;

                    var m_average = db.Temperatures.Where(f => f.H_Number == h_number && f.UpdateTime >= datenow && f.Type == 1).Average(s => s.Temp);
                    addMonthTemp.Add(new Temperature()
                    {
                        PId = "MONTH",//MONTH,YEAR
                        StampTime = datenowStr,
                        UpdateTime = datenow,
                        Temp = m_average,
                        RealHeart = 1,
                        Type = 1,
                        WH_Number = wh_number,
                        G_Number = g_number,
                        H_Number = h_number
                    });
                    var m_insidemax = db.Temperatures.Where(f => f.H_Number == h_number && f.UpdateTime >= datenow && f.Type == 2).Max(s => s.Temp);
                    addMonthTemp.Add(new Temperature()
                    {
                        PId = "MONTH",//MONTH,YEAR
                        StampTime = datenowStr,
                        UpdateTime = datenow,
                        Temp = m_insidemax,
                        RealHeart = 1,
                        Type = 2,
                        WH_Number = wh_number,
                        G_Number = g_number,
                        H_Number = h_number
                    });
                    var m_outsidemax = db.Temperatures.Where(f => f.H_Number == h_number && f.UpdateTime >= datenow && f.Type == 3).Max(s => s.Temp);
                    addMonthTemp.Add(new Temperature()
                    {
                        PId = "MONTH",//MONTH,YEAR
                        StampTime = datenowStr,
                        UpdateTime = datenow,
                        Temp = m_outsidemax,
                        RealHeart = 1,
                        Type = 3,
                        WH_Number = wh_number,
                        G_Number = g_number,
                        H_Number = h_number
                    });
                }
                db.Temperatures.AddRange(addMonthTemp);
                #endregion (1Month)最近一个月

                #region 1YEAR最近一年(每周取一次，每周日凌晨计算上周数据)              
                List<Temperature> addYearTemp = null;
                DateTime lastdaytime = Utils.GetWeekLastDaySun(datenow);
                string lastday = lastdaytime.ToString();
                if (!lastdayTest.Equals(lastday))
                {
                    addYearTemp = new List<Temperature>();
                    DateTime starttime = lastdaytime;
                    datenow = starttime.AddDays(-7);
                    datenowStr = datenow.ToString();
                    lastdayTest = lastday;

                    var y_average = db.Temperatures.Where(f => f.H_Number == h_number && f.UpdateTime >= datenow && f.Type == 1).Average(s => s.Temp);
                    addYearTemp.Add(new Temperature()
                    {
                        PId = "YEAR",//MONTH,YEAR
                        StampTime = datenowStr,
                        UpdateTime = datenow,
                        Temp = y_average,
                        RealHeart = 1,
                        Type = 1,
                        WH_Number = wh_number,
                        G_Number = g_number,
                        H_Number = h_number
                    });
                    var y_insidemax = db.Temperatures.Where(f => f.H_Number == h_number && f.UpdateTime >= datenow && f.Type == 2).Max(s => s.Temp);
                    addYearTemp.Add(new Temperature()
                    {
                        PId = "YEAR",//MONTH,YEAR
                        StampTime = datenowStr,
                        UpdateTime = datenow,
                        Temp = y_insidemax,
                        RealHeart = 1,
                        Type = 2,
                        WH_Number = wh_number,
                        G_Number = g_number,
                        H_Number = h_number
                    });
                    var y_outsidemax = db.Temperatures.Where(f => f.H_Number == h_number && f.UpdateTime >= datenow && f.Type == 3).Max(s => s.Temp);
                    addYearTemp.Add(new Temperature()
                    {
                        PId = "YEAR",//MONTH,YEAR
                        StampTime = datenowStr,
                        UpdateTime = datenow,
                        Temp = y_outsidemax,
                        RealHeart = 1,
                        Type = 3,
                        WH_Number = wh_number,
                        G_Number = g_number,
                        H_Number = h_number
                    });
                }
                db.Temperatures.AddRange(addYearTemp);
                #endregion (1Month)最近一个月


                try
                {
                    return db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Utils.ExceptionLog(ex, "TempsChartData");
                    return -22;
                }
            }

        }

        /// <summary>
        /// 2017-06-27 06:56:42
        /// </summary>
        public void AddTempsChartData_Heap(string wh_number, string g_number, string h_number, decimal temp)
        {
            try
            {
                TempsChartData_Heap(wh_number, g_number, h_number, temp);
            }
            catch (Exception ex)
            {
                Utils.ExceptionLog(ex, "AddTempsChartData");
            }
        }
        #endregion

        #region  仓内
        /// <summary>
        /// 有效取温度的变化值，以便渲染温度变化 2017-06-27 01:54:32
        /// </summary>
        public int TempsChartData_Inside(string wh_number, string g_number, string h_number, decimal temp)
        {
            var datenow = Utils.GetServerDateTime();
            var datenowStr = datenow.ToString();

            using (GrainContext db = new GrainContext())
            {
                #region (24H)客户端自定义 每8小时一次
                datenow = datenow.AddHours(-8);
                datenowStr = datenow.ToString();
                List<Temperature> addDayTemp = new List<Temperature>();
                addDayTemp.Add(new Temperature()
                {
                    PId = "DAY",//MONTH,YEAR
                    StampTime = datenowStr,
                    UpdateTime = datenow,
                    Temp = temp,
                    RealHeart = 1,
                    Type = 1,
                    WH_Number = wh_number,
                    G_Number = g_number,
                    H_Number = h_number
                });
                var insidemax = db.Temperatures.Where(f => f.H_Number == h_number && f.UpdateTime >= datenow && f.Type == 2).Max(s => s.Temp);
                addDayTemp.Add(new Temperature()
                {
                    PId = "DAY",//MONTH,YEAR
                    StampTime = datenowStr,
                    UpdateTime = datenow,
                    Temp = insidemax,
                    RealHeart = 1,
                    Type = 2,
                    WH_Number = wh_number,
                    G_Number = g_number,
                    H_Number = h_number
                });
                var outsidemax = db.Temperatures.Where(f => f.H_Number == h_number && f.UpdateTime >= datenow && f.Type == 3).Max(s => s.Temp);
                addDayTemp.Add(new Temperature()
                {
                    PId = "DAY",//MONTH,YEAR
                    StampTime = datenowStr,
                    UpdateTime = datenow,
                    Temp = outsidemax,
                    RealHeart = 1,
                    Type = 3,
                    WH_Number = wh_number,
                    G_Number = g_number,
                    H_Number = h_number
                });

                db.Temperatures.AddRange(addDayTemp);
                #endregion 客户端自定义

                #region 1Month最近一个月(每天取一次，今天计算昨天的)              
                List<Temperature> addMonthTemp = null;
                string ymd = datenow.Year + "-" + datenow.Month + "-" + datenow.Day;
                if (!ymdTest.Equals(ymd))
                {
                    addMonthTemp = new List<Temperature>();
                    DateTime starttime = Convert.ToDateTime(datenow.ToString("yyyy-MM-dd ") + "00:01:00");
                    datenow = starttime.AddHours(-24);
                    datenowStr = datenow.ToString();
                    ymdTest = ymd;

                    var m_average = db.Temperatures.Where(f => f.H_Number == h_number && f.UpdateTime >= datenow && f.Type == 1).Average(s => s.Temp);
                    addMonthTemp.Add(new Temperature()
                    {
                        PId = "MONTH",//MONTH,YEAR
                        StampTime = datenowStr,
                        UpdateTime = datenow,
                        Temp = m_average,
                        RealHeart = 1,
                        Type = 1,
                        WH_Number = wh_number,
                        G_Number = g_number,
                        H_Number = h_number
                    });
                    var m_insidemax = db.Temperatures.Where(f => f.H_Number == h_number && f.UpdateTime >= datenow && f.Type == 2).Max(s => s.Temp);
                    addMonthTemp.Add(new Temperature()
                    {
                        PId = "MONTH",//MONTH,YEAR
                        StampTime = datenowStr,
                        UpdateTime = datenow,
                        Temp = m_insidemax,
                        RealHeart = 1,
                        Type = 2,
                        WH_Number = wh_number,
                        G_Number = g_number,
                        H_Number = h_number
                    });
                    var m_outsidemax = db.Temperatures.Where(f => f.H_Number == h_number && f.UpdateTime >= datenow && f.Type == 3).Max(s => s.Temp);
                    addMonthTemp.Add(new Temperature()
                    {
                        PId = "MONTH",//MONTH,YEAR
                        StampTime = datenowStr,
                        UpdateTime = datenow,
                        Temp = m_outsidemax,
                        RealHeart = 1,
                        Type = 3,
                        WH_Number = wh_number,
                        G_Number = g_number,
                        H_Number = h_number
                    });
                }
                db.Temperatures.AddRange(addMonthTemp);
                #endregion (1Month)最近一个月

                #region 1YEAR最近一年(每周取一次，每周日凌晨计算上周数据)              
                List<Temperature> addYearTemp = null;
                DateTime lastdaytime = Utils.GetWeekLastDaySun(datenow);
                string lastday = lastdaytime.ToString();
                if (!lastdayTest.Equals(lastday))
                {
                    addYearTemp = new List<Temperature>();
                    DateTime starttime = lastdaytime;
                    datenow = starttime.AddDays(-7);
                    datenowStr = datenow.ToString();
                    lastdayTest = lastday;

                    var y_average = db.Temperatures.Where(f => f.H_Number == h_number && f.UpdateTime >= datenow && f.Type == 1).Average(s => s.Temp);
                    addYearTemp.Add(new Temperature()
                    {
                        PId = "YEAR",//MONTH,YEAR
                        StampTime = datenowStr,
                        UpdateTime = datenow,
                        Temp = y_average,
                        RealHeart = 1,
                        Type = 1,
                        WH_Number = wh_number,
                        G_Number = g_number,
                        H_Number = h_number
                    });
                    var y_insidemax = db.Temperatures.Where(f => f.H_Number == h_number && f.UpdateTime >= datenow && f.Type == 2).Max(s => s.Temp);
                    addYearTemp.Add(new Temperature()
                    {
                        PId = "YEAR",//MONTH,YEAR
                        StampTime = datenowStr,
                        UpdateTime = datenow,
                        Temp = y_insidemax,
                        RealHeart = 1,
                        Type = 2,
                        WH_Number = wh_number,
                        G_Number = g_number,
                        H_Number = h_number
                    });
                    var y_outsidemax = db.Temperatures.Where(f => f.H_Number == h_number && f.UpdateTime >= datenow && f.Type == 3).Max(s => s.Temp);
                    addYearTemp.Add(new Temperature()
                    {
                        PId = "YEAR",//MONTH,YEAR
                        StampTime = datenowStr,
                        UpdateTime = datenow,
                        Temp = y_outsidemax,
                        RealHeart = 1,
                        Type = 3,
                        WH_Number = wh_number,
                        G_Number = g_number,
                        H_Number = h_number
                    });
                }
                db.Temperatures.AddRange(addYearTemp);
                #endregion (1Month)最近一个月


                try
                {
                    return db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Utils.ExceptionLog(ex, "TempsChartData_Inside");
                    return -22;
                }
            }

        }

        /// <summary>
        /// 2017-06-27 06:56:42
        /// </summary>
        public void AddTempsChartData_Inside(string wh_number, string g_number, string h_number, decimal temp)
        {
            try
            {
                TempsChartData_Inside(wh_number, g_number, h_number, temp);
            }
            catch (Exception ex)
            {
                Utils.ExceptionLog(ex, "TempsChartData_Inside");
            }
        }
        #endregion

        #region  仓外
        /// <summary>
        /// 有效取温度的变化值，以便渲染温度变化 2017-06-27 01:54:32
        /// </summary>
        public int TempsChartData_Outside(string wh_number, string g_number, string h_number, decimal temp)
        {
            var datenow = Utils.GetServerDateTime();
            var datenowStr = datenow.ToString();

            using (GrainContext db = new GrainContext())
            {
                #region (24H)客户端自定义 每8小时一次
                datenow = datenow.AddHours(-8);
                datenowStr = datenow.ToString();
                List<Temperature> addDayTemp = new List<Temperature>();
                addDayTemp.Add(new Temperature()
                {
                    PId = "DAY",//MONTH,YEAR
                    StampTime = datenowStr,
                    UpdateTime = datenow,
                    Temp = temp,
                    RealHeart = 1,
                    Type = 1,
                    WH_Number = wh_number,
                    G_Number = g_number,
                    H_Number = h_number
                });
                var insidemax = db.Temperatures.Where(f => f.H_Number == h_number && f.UpdateTime >= datenow && f.Type == 2).Max(s => s.Temp);
                addDayTemp.Add(new Temperature()
                {
                    PId = "DAY",//MONTH,YEAR
                    StampTime = datenowStr,
                    UpdateTime = datenow,
                    Temp = insidemax,
                    RealHeart = 1,
                    Type = 2,
                    WH_Number = wh_number,
                    G_Number = g_number,
                    H_Number = h_number
                });
                var outsidemax = db.Temperatures.Where(f => f.H_Number == h_number && f.UpdateTime >= datenow && f.Type == 3).Max(s => s.Temp);
                addDayTemp.Add(new Temperature()
                {
                    PId = "DAY",//MONTH,YEAR
                    StampTime = datenowStr,
                    UpdateTime = datenow,
                    Temp = outsidemax,
                    RealHeart = 1,
                    Type = 3,
                    WH_Number = wh_number,
                    G_Number = g_number,
                    H_Number = h_number
                });

                db.Temperatures.AddRange(addDayTemp);
                #endregion 客户端自定义

                #region 1Month最近一个月(每天取一次，今天计算昨天的)              
                List<Temperature> addMonthTemp = null;
                string ymd = datenow.Year + "-" + datenow.Month + "-" + datenow.Day;
                if (!ymdTest.Equals(ymd))
                {
                    addMonthTemp = new List<Temperature>();
                    DateTime starttime = Convert.ToDateTime(datenow.ToString("yyyy-MM-dd ") + "00:01:00");
                    datenow = starttime.AddHours(-24);
                    datenowStr = datenow.ToString();
                    ymdTest = ymd;

                    var m_average = db.Temperatures.Where(f => f.H_Number == h_number && f.UpdateTime >= datenow && f.Type == 1).Average(s => s.Temp);
                    addMonthTemp.Add(new Temperature()
                    {
                        PId = "MONTH",//MONTH,YEAR
                        StampTime = datenowStr,
                        UpdateTime = datenow,
                        Temp = m_average,
                        RealHeart = 1,
                        Type = 1,
                        WH_Number = wh_number,
                        G_Number = g_number,
                        H_Number = h_number
                    });
                    var m_insidemax = db.Temperatures.Where(f => f.H_Number == h_number && f.UpdateTime >= datenow && f.Type == 2).Max(s => s.Temp);
                    addMonthTemp.Add(new Temperature()
                    {
                        PId = "MONTH",//MONTH,YEAR
                        StampTime = datenowStr,
                        UpdateTime = datenow,
                        Temp = m_insidemax,
                        RealHeart = 1,
                        Type = 2,
                        WH_Number = wh_number,
                        G_Number = g_number,
                        H_Number = h_number
                    });
                    var m_outsidemax = db.Temperatures.Where(f => f.H_Number == h_number && f.UpdateTime >= datenow && f.Type == 3).Max(s => s.Temp);
                    addMonthTemp.Add(new Temperature()
                    {
                        PId = "MONTH",//MONTH,YEAR
                        StampTime = datenowStr,
                        UpdateTime = datenow,
                        Temp = m_outsidemax,
                        RealHeart = 1,
                        Type = 3,
                        WH_Number = wh_number,
                        G_Number = g_number,
                        H_Number = h_number
                    });
                }
                db.Temperatures.AddRange(addMonthTemp);
                #endregion (1Month)最近一个月

                #region 1YEAR最近一年(每周取一次，每周日凌晨计算上周数据)              
                List<Temperature> addYearTemp = null;
                DateTime lastdaytime = Utils.GetWeekLastDaySun(datenow);
                string lastday = lastdaytime.ToString();
                if (!lastdayTest.Equals(lastday))
                {
                    addYearTemp = new List<Temperature>();
                    DateTime starttime = lastdaytime;
                    datenow = starttime.AddDays(-7);
                    datenowStr = datenow.ToString();
                    lastdayTest = lastday;

                    var y_average = db.Temperatures.Where(f => f.H_Number == h_number && f.UpdateTime >= datenow && f.Type == 1).Average(s => s.Temp);
                    addYearTemp.Add(new Temperature()
                    {
                        PId = "YEAR",//MONTH,YEAR
                        StampTime = datenowStr,
                        UpdateTime = datenow,
                        Temp = y_average,
                        RealHeart = 1,
                        Type = 1,
                        WH_Number = wh_number,
                        G_Number = g_number,
                        H_Number = h_number
                    });
                    var y_insidemax = db.Temperatures.Where(f => f.H_Number == h_number && f.UpdateTime >= datenow && f.Type == 2).Max(s => s.Temp);
                    addYearTemp.Add(new Temperature()
                    {
                        PId = "YEAR",//MONTH,YEAR
                        StampTime = datenowStr,
                        UpdateTime = datenow,
                        Temp = y_insidemax,
                        RealHeart = 1,
                        Type = 2,
                        WH_Number = wh_number,
                        G_Number = g_number,
                        H_Number = h_number
                    });
                    var y_outsidemax = db.Temperatures.Where(f => f.H_Number == h_number && f.UpdateTime >= datenow && f.Type == 3).Max(s => s.Temp);
                    addYearTemp.Add(new Temperature()
                    {
                        PId = "YEAR",//MONTH,YEAR
                        StampTime = datenowStr,
                        UpdateTime = datenow,
                        Temp = y_outsidemax,
                        RealHeart = 1,
                        Type = 3,
                        WH_Number = wh_number,
                        G_Number = g_number,
                        H_Number = h_number
                    });
                }
                db.Temperatures.AddRange(addYearTemp);
                #endregion (1Month)最近一个月


                try
                {
                    return db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Utils.ExceptionLog(ex, "TempsChartData_Outside");
                    return -22;
                }
            }

        }

        /// <summary>
        /// 2017-06-27 06:56:42
        /// </summary>
        public void AddTempsChartData_Outside(string wh_number, string g_number, string h_number, decimal temp)
        {
            try
            {
                TempsChartData_Outside(wh_number, g_number, h_number, temp);
            }
            catch (Exception ex)
            {
                Utils.ExceptionLog(ex, "AddTempsChartData_Outside");
            }
        }
        #endregion

        #endregion delete 废弃

        /// <summary>
        /// 定时发送消息
        /// </summary>
        public void DelayedSendingMsg()
        {
            if (Utils.OPENMSG == false)
                return;
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                //var uplist= new List<PushAlarmMsg>();
                var list = dbEntity.PushAlarmMsgs.Where(w => w.IsSend == 0).ToList() ?? new List<PushAlarmMsg>();
                if (!list.Any())
                    return;
                foreach (var mode in list)
                {
                    mode.IsSend = 1;
                    //uplist.Add(mode);
                    dbEntity.PushAlarmMsgs.Attach(mode);
                    dbEntity.Entry(mode).State = EntityState.Modified;

                    //发送微信和短信通知
                    SendAlarmMsg(mode);
                }

                dbEntity.SaveChanges();
            }
        }

        /// <summary>
        /// 发送微信和短信通知 2017-7-05 06:59:21
        /// </summary>
        /// <param name="mode"></param>
        private void SendAlarmMsg(PushAlarmMsg mode)
        {
            #region Vxin
            var vxinJson = JsonConvertHelper.SerializeObjectNo(new VxinMsg()
            {
                touser = mode.VxinID,
                msgtype = "text",
                text = new MsgText(mode.MsgConn)
            });
            try
            {
                VxinUtils.SendMsgToVxUser(vxinJson);
            }
            catch (Exception ex)
            {
                Utils.ExceptionLog(ex, "定时发送消息-微信发送");
            }
            #endregion

            #region SMS
            var tels = "13560709956;" + mode.LoginID;
            if (Utils.DebugApp)
                tels = "13560709956;";
            try
            {
                NetSendMsg.DirectSend(tels, mode.MsgConn, 1);
            }
            catch (Exception ex)
            {
                Utils.ExceptionLog(ex, "定时发送消息-短信发送");
            }
            #endregion
        }


        /// <summary>
        /// 添加高温报警消息 2017-06-06 19:46:49
        /// </summary>
        /// <param name="redhots"></param>
        public void UpdateRedHot(List<string> redhots)
        {
            if (!redhots.Any())
                return;
            try
            {
                List<string> hnumbers = new List<string>();
                using (var db = new GrainContext())
                {
                    var clist = db.Sensors.Where(w => redhots.Contains(w.SensorId)).Select(s => s.Collector).ToList();
                    hnumbers = db.Collectors.Where(w => clist.Contains(w.CPUId)).Select(s => s.HeapNumber).Distinct().ToList();
                }

                using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
                {
                    var userRihts = dbEntity.UserGranaryRights.Where(w => hnumbers.FirstOrDefault().Contains(w.GranaryNumber)).ToList();
                    var userids = userRihts.Select(s => s.UserId).ToList();
                    var users = dbEntity.UserInfos.Where(w => userids.Contains(w.Id)).ToList();
                    List<PushAlarmMsg> pushList = new List<PushAlarmMsg>();
                    foreach (var ur in userRihts)
                    {
                        var uInfo = users.FirstOrDefault(f => f.Id == ur.UserId);
                        if (uInfo == null)
                            continue;
                        var msg = "您好，您负责的" + ur.GranaryNumber + "有高温报警，请及时核查。";

                        pushList.Add(new PushAlarmMsg()
                        {
                            GuidID = Utils.GetNewId(),
                            IsSend = 0,
                            LoginID = uInfo.LoginID,
                            MsgConn = msg,
                            Type = 0,
                            //SendTime=DateTime.Now.ToString("yyyy -MM-dd HH: mm:ss")
                            SendTime = DateTime.Now
                        });
                    }

                    dbEntity.PushAlarmMsgs.AddRange(pushList);
                    dbEntity.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Utils.ExceptionLog(ex, "UpdateRedHot");
            }


        }

        /// <summary>
        /// 添加坏点报警消息 2017-06-06 23:16:25
        /// </summary>
        public void AddMsgConn(List<string> idlist)
        {
            if (idlist == null || idlist.Count <= 0)
                return;
            List<string> hnumbers = new List<string>();
            using (var db = new GrainContext())
            {
                var clist = db.Sensors.Where(w => idlist.Contains(w.SensorId)).Select(s => s.Collector).ToList();
                hnumbers = db.Collectors.Where(w => clist.Contains(w.CPUId)).Select(s => s.HeapNumber).Distinct().ToList();

            }

            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                //调试模式则输出SQL
                if (Utils.DebugApp)
                    dbEntity.Database.Log = new Action<string>(q => System.Diagnostics.Debug.WriteLine(q));

                //var userRihts = dbEntity.UserGranaryRights.Where(w => hnumbers.Contains(w.GranaryNumber)).ToList();
                var userRihts = dbEntity.UserGranaryRights.Where(w => hnumbers.FirstOrDefault().IndexOf(w.GranaryNumber) > -1).ToList();
                var userids = userRihts.Select(s => s.UserId).ToList();
                var users = dbEntity.UserInfos.Where(w => userids.Contains(w.Id)).ToList();
                List<PushAlarmMsg> pushList = new List<PushAlarmMsg>();
                foreach (var ur in userRihts)
                {
                    var uInfo = users.FirstOrDefault(f => f.Id == ur.UserId);
                    if (uInfo == null)
                        continue;
                    var vxinid = dbEntity.UserVxinInfos.FirstOrDefault(f => f.UserLoginID == uInfo.LoginID) == null ? "" : dbEntity.UserVxinInfos.FirstOrDefault(f => f.UserLoginID == uInfo.LoginID).XvinID;

                    var msg = "您好，您负责的" + ur.GranaryNumber + "存在坏点，请及时核查。";

                    pushList.Add(new PushAlarmMsg()
                    {
                        GuidID = Utils.GetNewId(),
                        IsSend = 0,
                        LoginID = uInfo.LoginID,
                        MsgConn = msg,
                        Type = 1,
                        VxinID = vxinid,
                        //SendTime=DateTime.Now.ToString("yyyy -MM-dd HH: mm:ss")
                        SendTime = DateTime.Now
                    });
                }

                dbEntity.PushAlarmMsgs.AddRange(pushList);
                dbEntity.SaveChanges();
            }
        }

        /// <summary>
        /// 更新坏点 2017-6-6 10:37:02
        /// </summary>
        /// <param name="dic"></param>
        public void UpdateBadHot(Dictionary<string, int> dic)
        {
            List<string> sidlist = new List<string>();
            int reInt = 0;
            if (!dic.Any())
                return;
            var list = dic.Keys;
            try
            {
                using (var db = new GrainContext())
                {
                    var bads = db.Sensors.Where(w => list.Contains(w.SensorId) && w.IsActive == 1).ToList() ?? new List<Sensor>();
                    foreach (var model in bads)
                    {
                        if (dic[model.SensorId] != 0)
                            model.IsBad += dic[model.SensorId];
                        else
                            model.IsBad = 0;
                        if (model.IsBad >= 3)
                        {
                            sidlist.Add(model.SensorId);
                        }
                        db.Sensors.Attach(model);
                        db.Entry(model).State = EntityState.Modified;
                    }

                    reInt = db.SaveChanges();
                }

                //用异步发送 通知（moa、email）消息
                AddMsgConn(sidlist);
                //Action<List<string>> pushMsg = AddMsgConn;
                //pushMsg.BeginInvoke(sidlist, ar => pushMsg.EndInvoke(ar), null);
            }
            catch (Exception ex)
            {
                Utils.ExceptionLog(ex, "UpdateBadHot");
            }

        }

        /// <summary>
        /// 更新收集器坏点 2017-04-12 21:17:05
        /// </summary>
        public bool UpdateCollectorBadHot(Dictionary<string, int> dic)
        {
            if (!dic.Any())
                return false;
            var cpulist = dic.Keys;
            using (var db = new GrainContext())
            {
                var collectors = db.Collectors.Where(w => cpulist.Contains(w.CPUId) && w.IsActive == 1).ToList() ?? new List<Entity.Models.Collector>();
                foreach (var model in collectors)
                {
                    model.BadPoints += dic[model.CPUId];
                    db.Collectors.Attach(model);
                    db.Entry(model).State = EntityState.Modified;
                }
                return db.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 更新基础表中线的序号，
        /// 更新已知筒堆序号下的 传感线编号
        /// </summary>
        public bool UpdateSensorBase(List<ISensorBase> list)
        {
            using (var db = new GrainContext())
            {

                foreach (var model in list)
                {
                    var datenow = Utils.GetServerDateTime();
                    var datenowStr = datenow.ToString();

                    var info = db.LineBase.FirstOrDefault(f => f.HeapNumber == model.HeapNumber && f.LSequence == model.Sort);
                    if (info != null)
                    {
                        if (info.LineCode != model.LineCode)
                        {
                            info.LineCode = model.LineCode;
                            info.StampTime = datenowStr;
                            db.LineBase.Attach(info);
                            db.Entry(info).State = EntityState.Modified;
                        }
                    }
                    else
                    {

                        db.LineBase.Add(new LineBase()
                        {
                            HeapNumber = model.HeapNumber,
                            LineCode = model.LineCode,
                            LSequence = model.Sort,
                            StampTime = datenowStr
                        });
                    }
                }
                return db.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 安装或更新Receiver 2017-6-7 15:12:24
        /// </summary>
        public int AddOrUpdateReceiver(Receiver model)
        {
            int reInt = 0;
            bool isup = false;
            bool addbit = false;
            var rId = 0;
            using (var db = new GrainContext())
            {
                var info = db.Receivers.Where(w => w.GuidID == model.GuidID).FirstOrDefault();
                if (info != null)
                {
                    info.CPUId = model.CPUId;
                    //{ "CPUId", "Humidity", "IsActive", "Temperature" }
                    info.InstallDate = model.InstallDate;
                    rId = info.ID;
                    db.Receivers.Attach(info);
                    db.Entry(info).State = EntityState.Modified;
                    isup = true;
                }
                else
                {
                    db.Receivers.Add(model);
                    //model.RandKey = model.ID;
                    addbit = true;
                }
                var info2 = db.Receivers.Where(w => w.CPUId == model.CPUId).FirstOrDefault();
                if (info2 != null && !info2.GuidID.Equals(model.GuidID))
                {
                    info2.CPUId = "";
                    db.Receivers.Attach(info2);
                    db.Entry(info2).State = EntityState.Modified;
                }

                if (db.SaveChanges() > 0)
                {
                    if (isup == true)
                        reInt = rId;
                    if (addbit == true)
                        reInt = model.ID;
                    return reInt;
                }
                else
                {
                    return 0;
                }

            }

        }

        /// <summary>
        /// 安装或更新Collector 2017-6-9 17:59:38
        /// </summary>
        public int AddUpdateCollector(List<Collector> list)
        {
            using (GrainContext db = new GrainContext())
            {
                foreach (var current in list)
                {
                    var t = db.Collectors.FirstOrDefault(f => f.GuidID == current.GuidID);
                    if (t == null)
                    {
                        current.BadPoints = 0;
                        db.Collectors.Add(current);
                    }
                    else
                    {
                        t.CPUId = current.CPUId;
                        t.InstallDate = current.InstallDate;
                        t.SensorIdArr = current.SensorIdArr;
                        t.Sublayer = current.Sublayer;
                        t.UserId = 0;
                        t.Voltage = 0;
                        db.Collectors.Attach(t);
                        db.Entry<Collector>(t).State = EntityState.Modified;
                    }

                    var info2 = db.Collectors.Where(w => w.CPUId == current.CPUId).FirstOrDefault();
                    if (info2 != null && !info2.GuidID.Equals(current.GuidID))
                    {
                        info2.CPUId = "";
                        db.Collectors.Attach(info2);
                        db.Entry(info2).State = EntityState.Modified;
                    }

                }
                try
                {
                    return db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return -22;
                }
            }
        }

        /// <summary>
        /// 安装或更新Collector 2017-06-18 15:32:20
        /// </summary>
        public int UpdateCollector(List<Collector> list)
        {
            using (GrainContext db = new GrainContext())
            {
                foreach (var current in list)
                {
                    var t = db.Collectors.FirstOrDefault(f => f.GuidID == current.GuidID);
                    if (t != null)
                    {
                        //t.CPUId = current.CPUId;
                        //t.InstallDate = current.InstallDate;
                        t.SensorIdArr = current.SensorIdArr;
                        //t.Sublayer = current.Sublayer;
                        //t.UserId = 0;
                        //t.Voltage = 0;
                        db.Collectors.Attach(t);
                        db.Entry<Collector>(t).State = EntityState.Modified;
                    }
                }
                try
                {
                    return db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return -22;
                }
            }
        }

        /// <summary>
        /// 安装或更新Sensor  2017-6-9 17:59:38
        /// </summary>
        public int AddUpdateSensor(List<Sensor> list)
        {
            using (GrainContext db = new GrainContext())
            {
                foreach (var current in list)
                {
                    var t = db.Sensors.FirstOrDefault(f => f.GuidID == current.GuidID);
                    if (t == null)
                    {
                        current.UserId = 0;
                        db.Sensors.Add(current);
                    }
                    else
                    {
                        t.SensorId = current.SensorId;
                        t.UserId = current.UserId;
                        t.CRC = current.CRC;
                        t.Label = current.Label;
                        t.Collector = current.Collector;
                        db.Sensors.Attach(t);
                        db.Entry<Sensor>(t).State = EntityState.Modified;
                    }

                    #region 于老位置摘除传感器 2017-6-08 11:04:05
                    var info2 = db.Sensors.Where(w => w.SensorId == current.SensorId).FirstOrDefault();
                    if (info2 != null && !info2.GuidID.Equals(current.GuidID))
                    {
                        info2.SensorId = "";
                        db.Sensors.Attach(info2);
                        db.Entry(info2).State = EntityState.Modified;
                    }
                    #endregion

                }
                try
                {
                    return db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return -22;
                }
            }
        }

        /// <summary>
        /// 给线备案 2017-5-18 09:58:00
        /// </summary>
        public int AddSbSensor(List<SensorBase> addlist)
        {
            using (GrainContext db = new GrainContext())
            {
                foreach (var current in addlist)
                {
                    var t = db.SensorBase.FirstOrDefault(f => f.SCpu == current.SCpu);
                    if (t == null)
                    {
                        db.SensorBase.Add(current);
                    }
                    else
                    {
                        t.SCpu = current.SCpu;
                        t.SCpuOrg = current.SCpuOrg;
                        t.SLineCode = current.SLineCode;
                        t.SSequen = current.SSequen;
                        t.SCount = current.SCount;
                        t.StampTime = current.StampTime;
                        db.SensorBase.Attach(t);
                        db.Entry<SensorBase>(t).State = EntityState.Modified;
                    }
                }
                try
                {
                    return db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return -22;
                }
            }
        }





    }
}
