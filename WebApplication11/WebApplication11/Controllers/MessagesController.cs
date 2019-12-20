using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingApplication.API.Data.Interfaces;
using ShoppingApplication.API.DTO;
using ShoppingApplication.API.Helpers;
using ShoppingApplication.API.Model;

namespace ShoppingApplication.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))] // that modifies the way in which the action is executed.
    [Authorize]
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;

        public MessagesController(
            IDatingRepository repo,
            IMapper mapper
            )
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name ="GetMessage")]
        public async Task<IActionResult> GetMessage(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageFromRepo = await _repo.GetMessage(id);

            if (messageFromRepo == null)
            {
                return NotFound();
            }
            return Ok(messageFromRepo);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesForUser(int userId, [FromQuery]MessageParams messageParams)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            messageParams.UserId = userId;

            var messagesFromRepo = await _repo.GetMessagesForUser(messageParams);

            var messages = _mapper.Map<IEnumerable<MessageToReturn>>(messagesFromRepo);


            Response.AddPagination(messagesFromRepo.CurrentPage, 
                messagesFromRepo.PageSize, messagesFromRepo.TotalCount, messagesFromRepo.TotalPages);

            return Ok(messages);
        }

        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetMessageThread(int userId, int recipientId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageFromRepo = await _repo.GetMessageThread(userId, recipientId);

            var messageThread = _mapper.Map<IEnumerable<MessageToReturn>>(messageFromRepo);

            return Ok(messageThread);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, 
            MessageForCreation messageForCreation)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            messageForCreation.SenderId = userId;

            var recipient = await _repo.GetUser(messageForCreation.RecipientId);

            if (recipient == null)
                return BadRequest("Could not find user");

            var message = _mapper.Map<Message>(messageForCreation);

            _repo.Add(message);

            var messageToReturn = _mapper.Map<MessageToReturn>(message);


            if (await _repo.SaveAll())
                return CreatedAtRoute("GetMessage", new
                {
                    id = message.Id
                }, messageToReturn);
            throw new Exception("Creating the message failed on save");
        }
        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteMessage(int id, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var message = await _repo.GetMessage(id);

            if (message.SenderId == userId)
                message.SenderDeleted = true;
            if (message.RecipientId == userId)
                message.RecipientDeleted = true;

            if ( message.SenderDeleted && message.RecipientDeleted)
            {
                _repo.Delete(message);
            }

            if (await _repo.SaveAll())
            {
                return NoContent();
            }

            throw new Exception("Error Deleteing the message");

        }
        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var message = await _repo.GetMessage(id);

            if (message.RecipientId != userId)
                return Unauthorized();

            message.IsRead = true;
            message.DateRead = DateTime.Now;

            await _repo.SaveAll();

            return NoContent();

        }
    }
}