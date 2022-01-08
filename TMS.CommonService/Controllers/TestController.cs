using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMS.Lib;
using TMS.Lib.Models;
using TMS.Lib.Services;

namespace TMS.CommonService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ComboWorker comboWorker;
        public TestController(ComboWorker comboWorker)
        {
            this.comboWorker = comboWorker;
        }

        [HttpPost("write")]
        public async Task<string> TestWrite(TestModel testModel)
        {
            testModel.MongoId = ObjectId.GenerateNewId();
            testModel.Time = DateTime.UtcNow;
            await comboWorker.Write(testModel);
            return System.Text.Json.JsonSerializer.Serialize(testModel);
        }

        [HttpPost("read_t")]
        public async Task<string> TestRead1(TestModel testModel)
        {
            var res = await comboWorker.ReadTarantool(testModel);
            return System.Text.Json.JsonSerializer.Serialize(res);
        }

        [HttpPost("read_m")]
        public async Task<string> TestRead2(TestModel testModel)
        {
            var res = await comboWorker.ReadMongo(testModel);
            return System.Text.Json.JsonSerializer.Serialize(res);
        }
    }
}
