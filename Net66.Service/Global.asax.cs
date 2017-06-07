using Net66.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Net66.Service
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            DatabaseInitializer.Initialize();
            IocConfig.RegisterDependencies();
            WebApiConfig.Register(GlobalConfiguration.Configuration);

            AreaRegistration.RegisterAllAreas();
            //GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            #region 执行定时任务
            try
            {
                //需要循环定时执行的程序
                AddCount(null, null);//需要立即执行
                System.Timers.Timer timer = new System.Timers.Timer(1500000);//十五分钟一次
                //System.Timers.Timer timer = new System.Timers.Timer(2000);
                timer.Elapsed += new System.Timers.ElapsedEventHandler(AddCount); //AddCount是一个方法，此方法就是每个6分钟而做的事情
                timer.AutoReset = true;
                //给Application["timer"]一个初始值
                Application.Lock();
                Application["timer"] = 1;
                Application.UnLock();
                timer.Enabled = true;
            }
            catch (Exception ex)
            {
                Comm.Utils.ExceptionLog(ex, "Application_Start-定时任务");
            }
            #endregion 

        }

        private void AddCount(object sender, System.Timers.ElapsedEventArgs e)
        {
            Application.Lock();
            Application["timer"] = Convert.ToInt32(Application["timer"]) + 1;
            #region 这里执行的任务

            Comm.Utils.PrintLog("定时任务最近依次执行时间：" + DateTime.Now.ToString());

            #endregion

            Application.UnLock();
        }


    }
}
