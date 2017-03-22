using IOT.Grain.Entity.DR_Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;


/******************************************
*Creater:yhw[96160]
*CreatTime:2017-2-6 10:03:58
*Description:
*==========================================
*Modifyier:sxf[]
*MidifyTime:
*Description:
*******************************************/
namespace Net66.Data.Context
{
    public class DbGrain : DbContext
    {
        public DbGrain()
            : base("DB_SEC")
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        public DbGrain(string name)
            : base(name)
        {
        }
        public DbSet<Collector> Collectors { get; set; }
        public DbSet<Floor> Floors { get; set; }
        public DbSet<Granary> Granaries { get; set; }
        public DbSet<Heap> Heaps { get; set; }
        public DbSet<Humidity> Humidities { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Receiver> Receivers { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<Temperature> Temperatures { get; set; }
        public DbSet<UserInfo> UserInfoes { get; set; }
        public DbSet<WareHouse> WareHouses { get; set; }
        public DbSet<WareHouse_Log> WareHouse_Log { get; set; }

    }
}
