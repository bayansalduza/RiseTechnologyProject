using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RiseTechnologyProject.Data.Context;
using RiseTechnologyProject.Data.Dto;
using RiseTechnologyProject.Data.Models;
using RiseTechnologyProject.DataAccess.PostreSqlUnitOfWork;

namespace RiseTechnologyProject.UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        MasterContext context = new MasterContext();

        [HttpPost("AddContact")]
        public async Task<ActionResult> AddContact(AddContactDto addContactDto)
        {
            using (PostreSqlUnitOfWork unitOfWork = new PostreSqlUnitOfWork(context))
            {
                try
                {
                    if (unitOfWork.GetRepository<User>().Get(addContactDto.UUID) != null)
                        unitOfWork.GetRepository<Contact>().Add(new Contact()
                        {
                            Email = addContactDto.Email,
                            Location = addContactDto.Location,
                            PhoneNumber = addContactDto.PhoneNumber,
                            User = unitOfWork.GetRepository<User>().Get(addContactDto.UUID)
                        });
                    else
                        return BadRequest("Not found user");
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

        [HttpDelete("DeleteContact")]
        public async Task<ActionResult> DeleteContact(int id)
        {
            try
            {
                using (PostreSqlUnitOfWork unitOfWork = new PostreSqlUnitOfWork(context))
                {
                    unitOfWork.GetRepository<Contact>().Delete(unitOfWork.GetRepository<Contact>().Get(id));
                    if (unitOfWork.SaveChanges() == 1)
                        return Ok();
                    else
                        return BadRequest("An error occurred while deleting data.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllContact")]
        public async Task<ActionResult> GetAllAccount(int id)
        {
            try
            {
                using (PostreSqlUnitOfWork unitOfWork = new PostreSqlUnitOfWork(context))
                {
                    List<ContactDto> contactDtos = new List<ContactDto>();
                    var contacts = unitOfWork.GetRepository<Contact>().GetAll(x => x.User.UUID == id);
                    Parallel.ForEach(contacts, x =>
                    {
                        contactDtos.Add(new ContactDto()
                        {
                            Email = x.Email,
                            Location = x.Location,
                            PhoneNumber = x.PhoneNumber
                        });
                    });
                    return Ok(new GetAllContactDto()
                    {
                        Contacts = contactDtos,
                        UUID = id
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
