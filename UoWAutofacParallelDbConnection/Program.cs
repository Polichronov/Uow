using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data;
using Microsoft.Data.SqlClient;

namespace UoWAutofacParallelDbConnection
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = CreateHostBuilder(args).Build();
            builder.Run();
        }

        // Method to create and configure the host builder
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory()) // Use Autofac as DI container
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices((context, services) =>
                    {
                        // Add services to the container.
                        services.AddControllers();
                        services.AddEndpointsApiExplorer();
                        services.AddSwaggerGen();
                    });

                    webBuilder.Configure((context, app) =>
                    {
                        var env = context.HostingEnvironment;

                        // Configure the HTTP request pipeline.
                        if (env.IsDevelopment())
                        {
                            app.UseSwagger();
                            app.UseSwaggerUI();
                        }

                        app.UseRouting();  // Add routing to enable MapControllers

                        app.UseAuthorization();

                        // Enable endpoint routing
                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();  // Map controllers to routes
                        });
                    });
                })
                .ConfigureContainer<ContainerBuilder>(containerBuilder =>
                {
                    // Register UnitOfWork and other services with Autofac
                    containerBuilder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
                    containerBuilder.RegisterType<SampleService>().As<ISampleService>();
                    containerBuilder.RegisterType<SampleRepository>().As<ISampleRepository>();

                    // Register the DbConnection as IDbConnection
                    containerBuilder.Register<IDbConnection>(ctx =>
                    {
                        var connectionString = "Server=localhost\\MSSQLSERVER01;Database=testuom;Trusted_Connection=True;";
                        return new SqlConnection(connectionString);
                    }).InstancePerLifetimeScope();
                });
    }
}
