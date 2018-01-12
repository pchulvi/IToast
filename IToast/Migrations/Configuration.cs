namespace IToast.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Models.IToastContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Models.IToastContext context)
        {
            //  This method will be called after migrating to the latest version.

            context.Toasters.AddOrUpdate(
                new Models.Toaster { Status = Models.Status.Off, Time = 0, Profile = Models.Profile.NoProfile }
                
                );

            context.Pantries.AddOrUpdate(
                new Models.Pantry { Status = Models.PantryStatus.Full, NumberOfBreads = 100}
                );



            if(context.Toasters.Count() == 0)
            {
                context.Toasters.AddOrUpdate(
                new Models.Toaster
                {
                    Status = Models.Status.Off,
                    Time = 0,
                    Profile = Models.Profile.NoProfile,
                    TimeStart = new DateTime().ToShortTimeString(),
                    TimeEnd = new DateTime().ToShortTimeString(),
                    ToastsMade = 0
                }
                );
            }

            if(context.Pantry.Count() == 0)
            {
                context.Pantry.AddOrUpdate(
                    new Models.Pantry
                    {
                        NumberOfBreads = 100,
                        Status = Models.PantryStatus.Full
                    }
                    );
            }
            

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
