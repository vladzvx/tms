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

        public async Task WriteArray(TestModel2[] testModels)
        {
            var schema = box.GetSchema();

            TarantoolTuple<TarantoolTuple<long,long>,TarantoolTuple<long, long>[]> data = TarantoolTuple.Create(TarantoolTuple.Create((long)1, (long)2), testModels.Select(item=> 
            {
                return TarantoolTuple.Create<long, long>(item.Time.Ticks, item.Data);
            }).ToArray());
            var q = await box.Call<TarantoolTuple<TarantoolTuple<long, long>, TarantoolTuple<long, long>[]>,long>("test",data);
        }

        public async Task Write(((ulong,ulong,ulong),double)[] datas)
        {
            List<TarantoolTuple<TarantoolTuple<ulong, ulong, ulong>, double>> tuples = new List<TarantoolTuple<TarantoolTuple<ulong, ulong, ulong>, double>>();
            foreach(var d in datas)
            {
                tuples.Add(TarantoolTuple.Create(TarantoolTuple.Create(d.Item1.Item1, d.Item1.Item2, d.Item1.Item3),d.Item2));
            }
            await box.Call<TarantoolTuple<TarantoolTuple<ulong, ulong, ulong>,double>[]>("add_records", tuples.ToArray());
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
                var res = await space.Insert(TarantoolTuple.Create<long, long, ulong, ulong, long>(data.Int64, data.Int32, testModel.Entity, testModel.Type, testModel.Time.Ticks));
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<TestModel> ReadTarantool(TestModel testModel)
        {
            try
            {
                var schema = box.GetSchema();
                var space = await schema.GetSpace("mongos");
                var ind1 = await space.GetIndex("entity");
                var res = await ind1.Select<TarantoolTuple<ulong>, TarantoolTuple<long, long, ulong, ulong, long>>(TarantoolTuple.Create(testModel.Entity), new SelectOptions { Iterator = Iterator.Eq });
                return new TestModel() {MongoId=IdConverter.Convert((int)res.Data[0].Item2, res.Data[0].Item1), Entity =res.Data[0].Item3,Time=new DateTime( res.Data[0].Item5,DateTimeKind.Utc),Type= res.Data[0].Item4 };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TestModel> ReadMongo(TestModel testModel)
        {
            try
            {
                var coll = database.GetCollection<TestModel>("TestModelsCollection");
                var res = coll.Find<TestModel>(Builders<TestModel>.Filter.Eq(item => item.MongoId, testModel.MongoId)).FirstOrDefault();
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }

    public class ComboWorker2
    {
        private readonly Box box;
        public ComboWorker2(Box box)
        {
            this.box = box;
            //box.Connect().Wait();
        }

        public async Task Write(((ulong, ulong, ulong), double)[] datas)
        {
            List<TarantoolTuple<TarantoolTuple<ulong, ulong, ulong>, double>> tuples = new List<TarantoolTuple<TarantoolTuple<ulong, ulong, ulong>, double>>();
            foreach (var d in datas)
            {
                tuples.Add(TarantoolTuple.Create(TarantoolTuple.Create(d.Item1.Item1, d.Item1.Item2, d.Item1.Item3), d.Item2));
            }
            await box.Call<TarantoolTuple<TarantoolTuple<TarantoolTuple<ulong, ulong, ulong>, double>[]>>("add_records", TarantoolTuple.Create(tuples.ToArray()));
        }
    }
}
