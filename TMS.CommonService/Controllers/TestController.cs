using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMS.Lib.Models;


namespace TMS.CommonService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly MongoClient mongoClient;
        public TestController(MongoClient mongoClient)
        {
            this.mongoClient = mongoClient;
        }

        [HttpPost()]
        public async Task<string> Test(TestModel testModel)
        {
            await mongoClient.GetDatabase("test").GetCollection<TestModel>("test").InsertOneAsync(testModel);
            return System.Text.Json.JsonSerializer.Serialize(testModel);
        }
    }
}
