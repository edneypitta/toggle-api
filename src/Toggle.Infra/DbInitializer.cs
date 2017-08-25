using System.Collections.Generic;
using System.Linq;
using Entities = Toggle.Domain.Entities;

namespace Toggle.Infra
{
    public class DbInitializer
    {
        public static void Initialize(ToggleContext context)
        {
            context.Database.EnsureCreated();

            if (context.Services.Any())
                return;

            var services = new List<Entities.Service>
            {
                new Entities.Service("Abc"), new Entities.Service("Def")
            };
            services.ForEach(s => context.Services.Add(s));
            context.SaveChanges();

            var toggles = new List<Entities.Toggle>
            {
                new Entities.Toggle("isButtonBlue", "true"),
                new Entities.Toggle(services[0].Id, "1.0", "isButtonGreen", "true"),
                new Entities.Toggle("isButtonRed", "true"),
                new Entities.Toggle(services[0].Id, "1.0", "isButtonRed", "false")
            };
            toggles.ForEach(t => context.Toggles.Add(t));
            context.SaveChanges();
        }
    }
}
