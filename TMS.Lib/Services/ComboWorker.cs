using MongoDB.Driver;
using ProGaudi.Tarantool.Client;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Lib.Models;
using TMS.Lib.Utils;

namespace TMS.Lib.Services
{
    public class ComboWorker
    {
        private readonly IMongoDatabase database;
        private readonly Box box;
        public ComboWorker(MongoClient mongoClient, Box box)
        {
            database = mongoClient.GetDatabase("test");
            this.box=box;
            box.Connect().Wait();
        }

        public async Task Write(TestModel testModel)
        {
            try
            {
                var coll = database.GetCollection<TestModel>("TestModelsCollection");
                await coll.InsertOneAsync(testModel);
  
                var schema = box.GetSchema();
                var data = IdConverter.Convert(testModel.MongoId);
                var space = await schema.GetSpace("mongos");
                var res = await space.Insert(TarantoolTuple.Create<long,long, ulong, ulong,long>(data.Int64,data.Int32,testModel.Entity,testModel.Type,testModel.Time.Ticks));
            }
            catch (Exception ex)
            {

            }

        }

        public async Task Read(TestModel testModel)
        {
            try
            {
                var schema = box.GetSchema();
                var space = await schema.GetSpace("mongos");
                var ind1 = await space.GetIndex("entity");
                var res = await ind1.Select<TarantoolTuple<ulong>, TarantoolTuple<long, long, ulong, ulong, long>>(TarantoolTuple.Create(testModel.Entity), new SelectOptions { Iterator = Iterator.Eq });
            }
            catch (Exception ex)
            {

            }

        }
    }
}
