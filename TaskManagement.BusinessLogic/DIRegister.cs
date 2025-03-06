using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.BusinessLogic.BL;

namespace TaskManagement.BusinessLogic
{
    public static class DIRegister
    {

        /// <summary>
        /// Registers the BL into Dependency Injection
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddBusinessLogicDependencies(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ProjectBL>();
            serviceCollection.AddTransient<AuthBL>();
            serviceCollection.AddTransient<ProjectTaskBL>();
            return serviceCollection;
        }
    }
}
