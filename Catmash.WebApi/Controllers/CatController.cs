using Catmash.Dto;
using Catmash.Model;
using Catmash.Service;
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


        [HttpGet("All")]
        public async Task<List<CatDto>> GetAll()
        {
            return await _catService.GetAllAsync();
        }

        [HttpGet("Next")]
        public async Task<List<CatDto>> GetNext()
        {
            return await _catService.GetNextAsync();
        }

        [HttpPost("Vote")]
        public async Task<bool> PostVote([FromBody] VoteDto vote)
        {
            return await _catService.Vote(vote);
        }
    }
}
