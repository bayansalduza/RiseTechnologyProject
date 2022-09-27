using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RiseTechnologyProject.Data.Context;
using RiseTechnologyProject.Data.Dto;
using RiseTechnologyProject.Data.Models;
using RiseTechnologyProject.DataAccess.PostgreSqlUnitOfWork;

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
            using (PostgreSqlUnitOfWork unitOfWork = new PostgreSqlUnitOfWork(context))
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
                using (PostgreSqlUnitOfWork unitOfWork = new PostgreSqlUnitOfWork(context))
                {
                    var contact = unitOfWork.GetRepository<Contact>().Get(id);
                    if (contact != null)
                        unitOfWork.GetRepository<Contact>().Delete(contact);
                    else
                        return NoContent();
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
                using (PostgreSqlUnitOfWork unitOfWork = new PostgreSqlUnitOfWork(context))
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
                    if (contactDtos.Count > 0)
                        return Ok(new GetAllContactDto()
                        {
                            Contacts = contactDtos,
                            UUID = id
                        });
                    else
                        return NoContent();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
