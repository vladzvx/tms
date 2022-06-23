using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using ProGaudi.Tarantool.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMS.Lib.Services;

namespace TMS.CommonService
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                //string mcnnstr = Environment.GetEnvironmentVariable("MONGO_DB_CNNSTR");
                string tcnnstr = "admin:secret-cluster-cookie@195.133.196.149:3301";// Environment.GetEnvironmentVariable("TARANTOOL_CNNSTR");
                
                ConventionRegistry.Register("IgnoreIfNullConvention", new ConventionPack { new IgnoreIfNullConvention(true) }, t => true);
                services.AddControllers();
                var box = Box.Connect(tcnnstr).Result;// new Box(new ProGaudi.Tarantool.Client.Model.ClientOptions(tcnnstr));
                box.Connect().Wait();
                //services.AddSingleton(new MongoClient(mcnnstr));
                services.AddSingleton<ComboWorker2>();
                services.AddSingleton(box);
            }
            catch (Exception ex) 
            { 
            
            }
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
