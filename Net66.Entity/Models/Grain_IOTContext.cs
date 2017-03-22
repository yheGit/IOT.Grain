using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Net66.Entity.Models.Mapping;

namespace Net66.Entity.Models
{
    public partial class Grain_IOTContext : DbContext
    {
        static Grain_IOTContext()
        {
            Database.SetInitializer<Grain_IOTContext>(null);
        }

        public Grain_IOTContext()
            : base("Name=Grain_IOTContext")
        {
        }

        public DbSet<Alarm> Alarms { get; set; }
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AlarmMap());
            modelBuilder.Configurations.Add(new CollectorMap());
            modelBuilder.Configurations.Add(new FloorMap());
            modelBuilder.Configurations.Add(new GranaryMap());
            modelBuilder.Configurations.Add(new HeapMap());
            modelBuilder.Configurations.Add(new HumidityMap());
            modelBuilder.Configurations.Add(new OrganizationMap());
            modelBuilder.Configurations.Add(new ReceiverMap());
            modelBuilder.Configurations.Add(new SensorMap());
            modelBuilder.Configurations.Add(new TemperatureMap());
            modelBuilder.Configurations.Add(new UserInfoMap());
            modelBuilder.Configurations.Add(new WareHouseMap());
            modelBuilder.Configurations.Add(new WareHouse_LogMap());
        }
    }
}
