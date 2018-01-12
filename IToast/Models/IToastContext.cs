using System.Data.Entity;

namespace IToast.Models
{
    public class IToastContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
            /// <summary>
            /// 
            /// </summary>
        public IToastContext() : base("name=IToastContext")
        {
        }

        public DbSet<Toaster> Toasters { get; set; }
        public DbSet<Pantry> Pantries { get; set; }
        public DbSet<SuperMarket> SuperMarkets { get; set; }
    }
}
