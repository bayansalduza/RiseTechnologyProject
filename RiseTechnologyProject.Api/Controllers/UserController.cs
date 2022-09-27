using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using RiseTechnologyProject.Data.Context;
using RiseTechnologyProject.Data.Dto;
using RiseTechnologyProject.Data.Models;
using RiseTechnologyProject.DataAccess.MongoDbRepository;
using RiseTechnologyProject.DataAccess.PostreSqlUnitOfWork;
using RiseTechnologyProject.DataAccess.RabbitMQExtensions;
using System.Diagnostics;

namespace RiseTechnologyProject.UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        MasterContext context = new MasterContext();

        [HttpPost("AddUser")]
        public async Task<ActionResult> AddUser(AddUserDto userDto)
        {
            using (PostreSqlUnitOfWork unitOfWork = new PostreSqlUnitOfWork(context))
            {
                try
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
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

            }
        }

        [HttpDelete("DeleteUser")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            using (PostreSqlUnitOfWork unitOfWork = new PostreSqlUnitOfWork(context))
            {
                try
                {
                    unitOfWork.GetRepository<Contact>().Delete(x => x.User.UUID == id);
                    unitOfWork.GetRepository<User>().Delete(unitOfWork.GetRepository<User>().Get(id));
                    unitOfWork.SaveChanges();
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

            }
        }

        [HttpGet("GetAllInformation")]
        public async Task<ActionResult> GetAllInformation(int id)
        {
            try
            {
                using (PostreSqlUnitOfWork unitOfWork = new PostreSqlUnitOfWork(context))
                {
                    var user = unitOfWork.GetRepository<User>().Get(id);
                    if (user == null)
                        return NoContent();
                    GetAllInformationDto getAll = new GetAllInformationDto()
                    {
                        Company = user.Company,
                        LastName = user.LastName,
                        Name = user.Name,
                        ContactDtos = new List<ContactDto>()
                    };
                    var contacts = await unitOfWork.GetRepository<Contact>().GetAll(x => x.User.UUID == id).ToListAsync();
                    foreach (var item in contacts)
                    {
                        getAll.ContactDtos.Add(new ContactDto()
                        {
                            Email = item.Email,
                            Location = item.Location,
                            PhoneNumber = item.PhoneNumber
                        });
                    }
                    return Ok(getAll);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
