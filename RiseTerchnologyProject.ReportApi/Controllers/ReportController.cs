using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RiseTechnologyProject.Data.Context;
using RiseTechnologyProject.Data.Dto;
using RiseTechnologyProject.Data.Models;
using RiseTechnologyProject.DataAccess.MongoDbRepository;
using RiseTechnologyProject.DataAccess.PostreSqlUnitOfWork;
using RiseTechnologyProject.DataAccess.RabbitMQExtensions;
using System.Xml.Linq;

namespace RiseTechnologyProject.ReportApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        MasterContext context = new MasterContext();

        [HttpGet("TakeReports")]
        public async Task<ActionResult> TakeReports(int id,bool newReport = false)
        {
            try
            {
                using (PostreSqlUnitOfWork unitOfWork = new PostreSqlUnitOfWork(context))
                {
                    if (unitOfWork.GetRepository<Report>().Get(x=> x.UUID == id) == null && newReport == true)
                    {                                                    
                        unitOfWork.GetRepository<Report>().Add(new Report()
                        {
                            DateTime = DateTime.Now,
                            UUID = id,
                            IsOkey = false
                        });
                        unitOfWork.SaveChangesAsync();
                        new RabbitMQExtensions().AddToQueue(id);
                        return Ok("Report request created, in process");
                    }
                    else if (unitOfWork.GetRepository<Report>().Get(x=> x.UUID == id).IsOkey == false)
                    {
                        unitOfWork.GetRepository<Report>().Add(new Report()
                        {
                            DateTime = DateTime.Now,
                            UUID = id,
                            IsOkey = false
                        });
                        unitOfWork.SaveChangesAsync();
                        new RabbitMQExtensions().AddToQueue(id);
                        return Ok("Report request created, in process");
                    }
                    else
                        return Ok(unitOfWork.GetRepository<Report>().Get(x => x.UUID == id));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllReports")]
        public async Task<ActionResult> GetAllReports(int id)
        {
            try
            {
                using (MongoDbRepository<ReportsDto> mongoRepo = new MongoDbRepository<ReportsDto>())
                {
                    var a = mongoRepo.GetAll(x => x.UUID == id).ToList();
                    return Ok(a);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetReportStatus")]
        public async Task<ActionResult> GetReportStatus(int id)
        {
            try
            {
                using (PostreSqlUnitOfWork unitOfWork = new PostreSqlUnitOfWork(context))
                {
                    return Ok(unitOfWork.GetRepository<Report>().GetAll(x => x.UUID == id).OrderByDescending(x => x.DateTime).FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
