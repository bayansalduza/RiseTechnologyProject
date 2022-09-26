using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RiseTechnologyProject.Data.Context;
using RiseTechnologyProject.Data.Dto;
using RiseTechnologyProject.Data.Models;
using RiseTechnologyProject.DataAccess.MongoDbRepository;
using RiseTechnologyProject.DataAccess.PostreSqlUnitOfWork;
using RiseTechnologyProject.DataAccess.RabbitMQExtensions;
using System.Xml.Linq;

namespace RiseTechnologyProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        MasterContext context = new MasterContext();

        [HttpGet("TakeReports")]
        public async Task<ActionResult> TakeReports(int uUID)
        {
            try
            {
                using (PostreSqlUnitOfWork unitOfWork = new PostreSqlUnitOfWork(context))
                {
                    if (unitOfWork.GetRepository<Report>().Get(x=> x.UUID == uUID) == null)
                    {                                                    
                        unitOfWork.GetRepository<Report>().Add(new Report()
                        {
                            DateTime = DateTime.Now,
                            UUID = uUID,
                            IsOkey = false
                        });
                        unitOfWork.SaveChangesAsync();
                        new RabbitMQExtensions().AddToQueue(uUID);
                        return Ok("Report request created, in process");
                    }
                    else if (unitOfWork.GetRepository<Report>().Get(x=> x.UUID == uUID).IsOkey == false)
                    {
                        unitOfWork.GetRepository<Report>().Add(new Report()
                        {
                            DateTime = DateTime.Now,
                            UUID = uUID,
                            IsOkey = false
                        });
                        unitOfWork.SaveChangesAsync();
                        new RabbitMQExtensions().AddToQueue(uUID);
                        return Ok("Report request created, in process");
                    }
                    else
                        return Ok(unitOfWork.GetRepository<Report>().Get(x => x.UUID == uUID));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllReports")]
        public async Task<ActionResult> GetAllReports(int uUID)
        {
            try
            {
                using (MongoDbRepository<ReportsDto> mongoRepo = new MongoDbRepository<ReportsDto>())
                {
                    var a = mongoRepo.GetAll(x => x.UUID == uUID).ToList();
                    return Ok(a);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetReportStatus")]
        public async Task<ActionResult> GetReportStatus(int uUID)
        {
            try
            {
                using (PostreSqlUnitOfWork unitOfWork = new PostreSqlUnitOfWork(context))
                {
                    return Ok(unitOfWork.GetRepository<Report>().GetAll(x => x.UUID == uUID).OrderByDescending(x => x.DateTime).FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
