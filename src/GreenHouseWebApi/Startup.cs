using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenHouseWebApi.Dto;
using GreenHouseWebApi.Model;
using GreenHouseWebApi.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GreenHouseWebApi
{
    public class Startup
    {

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            //Todo: check what this is about
            /*
            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }
            */
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; private set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //one instance per request
            services.AddScoped<IDeviceSetupRepository, DeviceSetupRepository>();

            //singleton only for demo, as the repo is not a real database
            services.AddSingleton<IFoodRepository, FoodRepository>();
            
            services.AddDbContext<GreenHouseDatabaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            
            services.AddMvc();
        }

        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, GreenHouseDatabaseContext databaseContext)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            AutoMapper.Mapper.Initialize(mapper =>
            {
                mapper.CreateMap<FoodItem, FoodItemDto>().ReverseMap();
                mapper.CreateMap<DeviceSetup, DeviceSetupDto>().ReverseMap();
                mapper.CreateMap<WateringArea, WateringAreaDto>().ReverseMap();
            });

            app.UseMvc();

            DbInitializer.Initialize(databaseContext);
        }
    }
}
