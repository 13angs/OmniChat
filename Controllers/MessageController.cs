using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        [HttpPost]
        [Route("message/send")]
        public async Task<ActionResult> SendMessageAsync([FromBody] MessageRequest request)
        {
            await _messageService.SendMessageAsync(request);
            return Ok();
        }
    }
}