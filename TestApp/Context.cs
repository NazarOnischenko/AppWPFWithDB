using System.Data.Entity;

namespace TestApp
{
    //класс для подключения к БД
    public class Context : DbContext
    {
        public DbSet<Shipment> Shipments { get; set; }
        //DbConnection - это строка подключения, которая находиться в конфиг файле
        public Context() : base("DbConnection")
        {

        }
    }
}
