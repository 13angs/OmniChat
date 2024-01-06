using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OmniChat.Models;
using OmniChat.Services;

namespace OmniChat.Controllers
{
    [Route("api/v1")]
    public class MessageController : Controller
    {
        private readonly MessageService _messageService;

        public MessageController(MessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet]
        [Route("messages")]
        public ActionResult GetMessages([FromQuery] MessageRequest request)
        {
            return Ok(new OkResponse<MessageResponse>
            {
                Data = _messageService.GetMessages(request)
            });
        }

        [HttpPost]
        [Route("message/send")]
        public async Task<ActionResult> SendMessageAsync([FromBody] MessageRequest request)
        {
            return Ok(await _messageService.SendMessageAsync(request));
        }
    }
}