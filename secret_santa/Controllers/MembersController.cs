using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using secret_santa.core.Contracts.Gateways;
using secret_santa.core.Entities;

namespace secret_santa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly ILogger<MembersController> _logger;
        private readonly IExchangeMemberRepository _exchangeMemberRepository;

        public MembersController(ILogger<MembersController> logger, IExchangeMemberRepository exchangeMemberRepository)
        {
            _logger = logger;
            _exchangeMemberRepository = exchangeMemberRepository;
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [ProducesResponseType(typeof(Member), StatusCodes.Status200OK)]
        public ActionResult<Member> Get(Guid id)
        {
            try
            {

                 
                if (id == null || id == Guid.Empty)
                {
                    return BadRequest();
                }

                var member = _exchangeMemberRepository.FindMember(id.ToString());

                return Ok(member);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Member>), StatusCodes.Status200OK)]
        public ActionResult<List<Member>> Get()
        {
            try
            {
                List<Member> members = _exchangeMemberRepository.GetAllMembers();

                return Ok(members);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

    
        [HttpPost]
        [ProducesResponseType(typeof(Member), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PostMember([FromBody]Member member)
        {

            if (string.IsNullOrEmpty(member?.Name))
            {
                return BadRequest();
            }

            try
            {
                member = _exchangeMemberRepository.AddMember(member);

                return CreatedAtAction("Post", new { id = member.Id }, member);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }


        [HttpPut]
        [ProducesResponseType(typeof(Member), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PutMember([FromBody]Member member)
        {

            if (string.IsNullOrEmpty(member?.Name))
            {
                return BadRequest();
            }

            try
            {
                member = _exchangeMemberRepository.UpdateMember(member);
                return Ok(member);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        [ProducesResponseType(typeof(Member), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteMember(string id)
        {

            if (id == null)
            {
                return BadRequest();
            }

            try
            {
                var member = _exchangeMemberRepository.FindMember(id);
                _exchangeMemberRepository.RemoveMember(member);
                return Ok(member);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }


        [HttpGet]
        [Route("results")]
        [ProducesResponseType(typeof(List<Match<Member>>), StatusCodes.Status200OK)]
        public IActionResult Results()
        {

            try
            {

                List<Match<Member>> matches = _exchangeMemberRepository.GenerateMatchResults();
                return Ok(matches);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}