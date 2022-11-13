using Contracts;
using Contracts.Contracts;
using Microsoft.AspNetCore.Mvc;
using SubClient1.Services;

namespace SubClient1.Controllers
{
    [ApiController]
    [Route("")]
    public class ReceiverController : Controller
    {
        private readonly DataWrapper _dataWrapper;

        public ReceiverController(DataWrapper dataWrapper)
        {
            _dataWrapper = dataWrapper;
        }

        [HttpPost]
        [Route("receiver1")]
        public IActionResult Receiver1(ContractReceived<Contract1>  message)
        {
            _dataWrapper.Topic1.Add(message.Data.Message);
            return Ok();
        }
        [HttpPost]
        [Route("receiver2")]
        public IActionResult Receiver2(ContractReceived<Contract2> message)
        {
            _dataWrapper.Topic2.Add(message.Data.Number.ToString());
            return Ok();
        }
        [HttpPost]
        [Route("receiver3")]
        public IActionResult Receiver3(ContractReceived<Contract3> message)
        {
            _dataWrapper.Topic3.Add(string.Join(", ",message.Data.ListOfMessages));
            return Ok();
        }
    }
}
