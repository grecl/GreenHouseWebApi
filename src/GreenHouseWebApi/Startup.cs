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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GreenHouseWebApi
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //one instance per request
            services.AddScoped<IDeviceSetupRepository, DeviceSetupRepository>();

            //singleton only for demo, as the repo is not a real database
            services.AddSingleton<IFoodRepository, FoodRepository>();
            
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
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
            });

            app.UseMvc();
        }
    }
}
