using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using RiseTechnologyProject.Data.Context;
using RiseTechnologyProject.Data.Dto;
using RiseTechnologyProject.Data.Models;
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
        public async Task<ActionResult> AddUser(AddUserDto userDto)
        {
            using (PostreSqlUnitOfWork unitOfWork = new PostreSqlUnitOfWork(context))
            {
                unitOfWork.GetRepository<User>().Add(new User()
                {
                    Company = userDto.Company,
                    LastName = userDto.LastName,
                    Name = userDto.Name
                });
                if (unitOfWork.SaveChanges() == 1)
                    return Ok();
                else
                    return BadRequest("An error occurred while creating the data.");
            }
        }

        [HttpDelete("DeleteUser")]
        public async Task<ActionResult> DeleteUser(int uUID)
        {
            using (PostreSqlUnitOfWork unitOfWork = new PostreSqlUnitOfWork(context))
            {
                unitOfWork.GetRepository<User>().Delete(unitOfWork.GetRepository<User>().Get(uUID));
                if (unitOfWork.SaveChanges() == 1)
                    return Ok();
                else
                    return BadRequest("An error occurred while deleting data.");
            }
        }
    }
}
