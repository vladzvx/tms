using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Tests.Common;
using TMS.Lib;
using TMS.Lib.Services;
using MongoDB.Bson.Serialization.Conventions;
using TMS.Lib.Models;
using MongoDB.Bson;
using System.Net.Http;
using System.Threading;

namespace TMS.FuncTests
{
    [TestClass]
    public class ComboWorkerTests
    {
        static ComboWorker worker;


        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            ConventionRegistry.Register("IgnoreIfNullConvention", new ConventionPack { new IgnoreIfNullConvention(true) }, t => true);

            Utils.EnvFileReader("cw.env");
            string MONGO_DB_USER = Environment.GetEnvironmentVariable(Constants.MongoUser_VariableName);
            string MONGO_DB_PWD = Environment.GetEnvironmentVariable(Constants.MongoPWD_VariableName);
            string MONGO_DB_HOST = Environment.GetEnvironmentVariable(Constants.MongoHost_VariableName);
            string MONGO_DB_INTERNAL_PORT = Environment.GetEnvironmentVariable(Constants.MongoInternalPort_VariableName);
            string MONGO_DB_EXTERNAL_PORT = Environment.GetEnvironmentVariable(Constants.MongoExternalPort_VariableName);
            string MONGO_DB_CNNSTR = string.Format("mongodb://{0}:{1}@{2}:{3}", MONGO_DB_USER, MONGO_DB_PWD, MONGO_DB_HOST, MONGO_DB_EXTERNAL_PORT);
            Environment.SetEnvironmentVariable(Constants.MongoConnectionString_VariableName, MONGO_DB_CNNSTR);

            string TARANTOOL_USER = Environment.GetEnvironmentVariable(Constants.TarantoolUser_VariableName);
            string TARANTOOL_PWD = Environment.GetEnvironmentVariable(Constants.TarantoolPWD_VariableName);
            string TARANTOOL_HOST = Environment.GetEnvironmentVariable(Constants.TarantoolHost_VariableName);
            string TARANTOOL_PORT = Environment.GetEnvironmentVariable(Constants.TarantoolExternalPort_VariableName);
            string TARANTOOL_CNNSTR = string.Format("{0}:{1}@{2}:{3}", TARANTOOL_USER, TARANTOOL_PWD, TARANTOOL_HOST, TARANTOOL_PORT);
            Environment.SetEnvironmentVariable(Constants.TarantoolConnectionString_VariableName, TARANTOOL_CNNSTR);

            worker = new ComboWorker(
                new MongoDB.Driver.MongoClient(Environment.GetEnvironmentVariable(Constants.MongoConnectionString_VariableName)), 
                new ProGaudi.Tarantool.Client.Box(new ProGaudi.Tarantool.Client.Model.ClientOptions(Environment.GetEnvironmentVariable(Constants.TarantoolConnectionString_VariableName))));
        }


        [TestMethod]
        public void FuncTest1()
        {
            DateTime dt = DateTime.UtcNow;
            TestModel testModel = new TestModel()
            {
                MongoId= ObjectId.GenerateNewId(),
                Entity = 100500,
                Type=100500,
                Time= dt
            };
            Console.WriteLine("TestData: " + System.Text.Json.JsonSerializer.Serialize(testModel));
            worker.Write(testModel).Wait();
            Thread.Sleep(1000);
            var result1  = worker.ReadTarantool(new TestModel() {Entity= testModel.Entity }).Result;
            var result2  = worker.ReadMongo(new TestModel() {MongoId= testModel.MongoId }).Result;
            Console.WriteLine("Readed: "+System.Text.Json.JsonSerializer.Serialize(result1));
            Assert.IsTrue(result1.Entity==testModel.Entity);
            Assert.IsTrue(result1.MongoId==testModel.MongoId);
            Assert.IsTrue(result1.Type==testModel.Type);
            //Assert.IsTrue(result1.Time==testModel.Time);

            Assert.IsTrue(result2.Entity == testModel.Entity);
            Assert.IsTrue(result2.MongoId == testModel.MongoId);
            Assert.IsTrue(result2.Type == testModel.Type);
            //Assert.IsTrue(result2.Time == testModel.Time);
        }


        //[TestMethod]
        //public void FuncTest2()
        //{
        //    HttpClient httpClient = new HttpClient();
        //    DateTime dt = DateTime.UtcNow;
        //    TestModel testModel = new TestModel()
        //    {
        //        //MongoId = ObjectId.GenerateNewId(),
        //        Entity = 100501,
        //        Type = 100501,
        //        Time = dt
        //    };
        //    string cont = System.Text.Json.JsonSerializer.Serialize(testModel);

        //    string url1 = "http://" + Environment.GetEnvironmentVariable(Constants.MongoHost_VariableName) + ":5005/test/write";
        //    var res1 = httpClient.PostAsync(url1, new StringContent(cont, Encoding.UTF8, "application/json")).Result;
        //    Thread.Sleep(1000);

        //    var r = System.Text.Json.JsonSerializer.Deserialize<TestModel>(res1.Content.ReadAsStringAsync().Result);
        //    var rest = httpClient.PostAsync("http://" + Environment.GetEnvironmentVariable(Constants.MongoHost_VariableName) + ":5005/test/read_t", new StringContent(System.Text.Json.JsonSerializer.Serialize(testModel), Encoding.UTF8, "application/json")).Result;
        //    var resm = httpClient.PostAsync("http://" + Environment.GetEnvironmentVariable(Constants.MongoHost_VariableName) + ":5005/test/read_m", new StringContent(System.Text.Json.JsonSerializer.Serialize(testModel), Encoding.UTF8, "application/json")).Result;
        //    TestModel testModelFromT = System.Text.Json.JsonSerializer.Deserialize<TestModel>(rest.Content.ReadAsStringAsync().Result);
        //    TestModel testModelFromM = System.Text.Json.JsonSerializer.Deserialize<TestModel>(resm.Content.ReadAsStringAsync().Result);

        //    Assert.IsTrue(testModelFromT.Entity == testModel.Entity);
        //    Assert.IsTrue(testModelFromT.MongoId == r.MongoId);
        //    Assert.IsTrue(testModelFromT.Type == testModel.Type);
        //    //Assert.IsTrue(testModelFromT.Time == testModel.Time);

        //    Assert.IsTrue(testModelFromM.Entity == testModel.Entity);
        //    Assert.IsTrue(testModelFromM.MongoId == r.MongoId);
        //    Assert.IsTrue(testModelFromM.Type == testModel.Type);
        //    //Assert.IsTrue(testModelFromM.Time == testModel.Time);
        //}

    }
}
