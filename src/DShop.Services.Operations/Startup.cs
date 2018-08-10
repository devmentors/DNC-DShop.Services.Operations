using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DShop.Common.AppMetrics;
using DShop.Common.Dispatchers;
using DShop.Common.Handlers;
using DShop.Common.Mongo;
using DShop.Common.Mvc;
using DShop.Common.RabbitMq;
using DShop.Services.Operations.Domain;
using DShop.Services.Operations.Handlers;
using DShop.Services.Operations.Subscriptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DShop.Services.Operations
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IContainer Container { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCustomMvc();
            services.AddAppMetrics();
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly())
                    .AsImplementedInterfaces();
            builder.Populate(services);
            builder.AddRabbitMq();
            builder.AddMongoDB();
            builder.AddMongoDBRepository<Operation>("Operations");
            builder.RegisterGeneric(typeof(GenericCommandHandler<>))
                .As(typeof(ICommandHandler<>));
            builder.RegisterGeneric(typeof(GenericEventHandler<>))
                .As(typeof(IEventHandler<>));

            Container = builder.Build();

            return new AutofacServiceProvider(Container);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
            IApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment() || env.EnvironmentName == "local")
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAppMetrics(applicationLifetime);
            app.UseErrorHandler();
            app.UseMvc();
            app.UseRabbitMq()
                .SubscribeAllCommands()
                .SubscribeAllEvents();
            applicationLifetime.ApplicationStopped.Register(() => Container.Dispose());
        }
    }
}
