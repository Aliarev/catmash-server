using Catmash.Dto;
using Catmash.Model;
using Catmash.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catmash.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class CatController : Controller
    {
        private CatService _catService { get; set; }


        public CatController(CatmashContext context)
        {
            _catService = new CatService(context);
        }


        /// <summary>Retourne tous les chats.</summary>
        [AllowAnonymous]
        [HttpGet("All")]
        public async Task<List<CatDto>> GetAll()
        {
            return await _catService.GetAllAsync();
        }

        /// <summary>Retourne deux chats ayant le moins de votes.</summary>
        [AllowAnonymous]
        [HttpGet("Next")]
        public async Task<List<CatDto>> GetNext()
        {
            return await _catService.GetNextAsync();
        }

        /// <summary>Vote +1 pour un chat, 61 pour un autre.</summary>
        /// <param name="vote">Le vote</param>
        [AllowAnonymous]
        [HttpPost("Vote")]
        public async Task<bool> PostVote([FromBody] VoteDto vote)
        {
            return await _catService.Vote(vote);
        }
    }
}
