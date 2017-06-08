using IOT.RightsSys.Entity;
using Net66.Comm;
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
                    var userRihts = dbEntity.UserGranaryRights.Where(w => hnumbers.Contains(w.GranaryNumber)).ToList();
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
                ////调试模式则输出SQL
                //if (Utils.DebugApp)
                //    dbEntity.Database.Log = new Action<string>(q => System.Diagnostics.Debug.WriteLine(q));

                var userRihts = dbEntity.UserGranaryRights.Where(w => hnumbers.Contains(w.GranaryNumber)).ToList();
                var userids = userRihts.Select(s => s.UserId).ToList();
                var users = dbEntity.UserInfos.Where(w => userids.Contains(w.Id)).ToList();
                List<PushAlarmMsg> pushList = new List<PushAlarmMsg>();
                foreach (var ur in userRihts)
                {
                    var uInfo = users.FirstOrDefault(f => f.Id == ur.UserId);
                    if (uInfo == null)
                        continue;
                    var msg = "您好，您负责的" + ur.GranaryNumber + "存在坏点，请及时核查。";

                    pushList.Add(new PushAlarmMsg()
                    {
                        GuidID = Utils.GetNewId(),
                        IsSend = 0,
                        LoginID = uInfo.LoginID,
                        MsgConn = msg,
                        Type = 1,
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
                Action<List<string>> pushMsg = AddMsgConn;
                pushMsg.BeginInvoke(sidlist, ar => pushMsg.EndInvoke(ar), null);
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
            var rId = 0;
            using (var db = new GrainContext())
            {
                var info = db.Receivers.Where(w =>w.Number == model.Number).FirstOrDefault();
                if (info != null && !info.CPUId.Equals(model.CPUId))
                {
                    info.CPUId = model.CPUId;
                    //{ "CPUId", "Humidity", "IsActive", "Temperature" }
                    rId = info.ID;
                    db.Receivers.Attach(info);
                    db.Entry(info).State = EntityState.Modified;
                    isup = true;                  
                }
                else
                {
                    var info2 = db.Receivers.Where(w => w.CPUId == model.CPUId).FirstOrDefault();
                    if (info2!=null&&!info2.Number.Equals(model.Number) )
                    {
                        info2.CPUId = ""; 
                        db.Receivers.Attach(info2);
                        db.Entry(info2).State = EntityState.Modified;
                    }
                    db.Receivers.Add(model);
                }
                reInt = db.SaveChanges();
                if (isup == true)
                    reInt = rId;
                return reInt;

            }

        }



    }
}
