using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiseTechnologyProject.Data.Context;
using RiseTechnologyProject.DataAccess.PostreSqlUnitOfWork;

namespace RiseTechnologyProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        MasterContext context = new MasterContext();
        UserController(MasterContext context)
        {
            this.context = context;
        }
        [HttpPost("AddUser")]
        public async Task<ActionResult> AddUser()
        {
            using (PostreSqlUnitOfWork unitOfWork = new PostreSqlUnitOfWork(context))
            {
                return Ok();
            }
        }
    }
}
