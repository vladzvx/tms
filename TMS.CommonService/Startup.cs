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
            string mcnnstr = Environment.GetEnvironmentVariable("MONGO_DB_CNNSTR");
            string tcnnstr = Environment.GetEnvironmentVariable("TARANTOOL_CNNSTR");
            ConventionRegistry.Register("IgnoreIfNullConvention", new ConventionPack { new IgnoreIfNullConvention(true) }, t => true);
            services.AddControllers();
            services.AddSingleton(new MongoClient(mcnnstr));
            services.AddSingleton<ComboWorker>();
            services.AddSingleton(new Box(new ProGaudi.Tarantool.Client.Model.ClientOptions(tcnnstr)));
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
