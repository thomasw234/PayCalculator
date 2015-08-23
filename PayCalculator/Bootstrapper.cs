using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using PayCalculator.Repository;
using Unity.Mvc3;

namespace PayCalculator
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            var payCalculatorDb = new SqlConnection(ConfigurationManager.ConnectionStrings["PayCalculator"].ConnectionString);

            container.RegisterInstance(typeof (IDbConnection), payCalculatorDb)
                .RegisterType<ISalaryRepository, SqlSalaryRepository>()
                .RegisterType<ISessionManager, SessionManager>()
                .RegisterType<IRealtimeSalaryCalculator, RealtimeSalaryCalculator>();

            return container;
        }
    }
}