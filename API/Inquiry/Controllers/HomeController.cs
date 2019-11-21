using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using InquiryService.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace InquiryService.Controllers
{
    [ApiController]
    [Route("/")]
    public class HomeController : ControllerBase
    {

        private readonly ILogger<HomeController> _logger;
        private readonly RabbitClient _listener;
        private readonly IHttpContextAccessor _accessor;

        [HttpPost]
        public IActionResult AddInquiry(Inquiry inquiry)
        {
            // ЛОГИРУЕМ ДАТУ, IP, ВХОДНЫЕ ДАННЫЕ
            _logger.LogInformation(JsonConvert.SerializeObject(
                    new
                    {
                        DateTime = DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss.fff"), // ЛОКАЛЬНОЕ ВРЕМЯ СИСТЕМЫ
                        IP = _accessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                        Inquiry = new { client_id = inquiry.client_id, amout = inquiry.amout, department_address = inquiry.department_address, UAN = inquiry.UAN }
                    }
                    ) + "\n\n");

            // ОТПРАВЛЯЕМ ЗАЯВКУ В ОЧЕРЕДЬ
            _listener.Register(JsonConvert.SerializeObject(inquiry));
            return Ok();
        }
        public HomeController(ILogger<HomeController> logger, RabbitClient listener, IHttpContextAccessor accessor)
        {
            _logger = logger;
            _listener = listener;
            _accessor = accessor;
        }
    }
}
