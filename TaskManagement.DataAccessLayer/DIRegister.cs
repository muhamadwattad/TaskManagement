using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.DataAccessLayer.Db;
using TaskManagement.DataAccessLayer.Repositories.Interfaces;

namespace TaskManagement.DataAccessLayer
{
    public static class DIRegister
    {
        public static IServiceCollection AddDataAccessLayerDependencies(this IServiceCollection serviceCollection)
        {

            serviceCollection.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("TaskManagement");
            });

            serviceCollection.AddTransient<IUnitOfWork, UnitOfWork>();

            return serviceCollection;
        }
    }
}
