using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RiseTechnologyProject.Consumer.Properties;
using RiseTechnologyProject.Data.Context;
using RiseTechnologyProject.Data.Dto;
using RiseTechnologyProject.Data.Models;
using RiseTechnologyProject.DataAccess.MongoDbRepository;
using RiseTechnologyProject.DataAccess.PostreSqlUnitOfWork;
using System.Text;


AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

try
{
    var factory = new ConnectionFactory() { HostName = Resources.RabbitMQConnectionString };
    using (var connection = factory.CreateConnection())
    using (var channel = connection.CreateModel())
    {
        channel.QueueDeclare(queue: Resources.RabbitMQQueueName,
            durable: false,
            exclusive: false);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, eventArgs) =>
        {
            var uUID = Convert.ToInt32(Encoding.UTF8.GetString(eventArgs.Body.Span));
            using (PostreSqlUnitOfWork unitOfWork = new PostreSqlUnitOfWork(new MasterContext()))
            {
                var contactsLocation = unitOfWork.GetRepository<Contact>().GetAll(x => x.User.UUID == uUID).ToList();
                if (contactsLocation.Count > 0)
                {
                    var contactsLocationGB = contactsLocation.GroupBy(x => x.Location).ToList();
                    using (MongoDbRepository<ReportsDto> mongoRepo = new MongoDbRepository<ReportsDto>())
                    {
                        mongoRepo.Delete(x => x.UUID == uUID);
                        foreach (var item in contactsLocationGB)
                        {
                            var location = item.Where(x => x.Location == item.Key).ToList();
                            var phoneNumber = item.Where(x => x.PhoneNumber != null).ToList();
                            mongoRepo.Add(new ReportsDto()
                            {
                                Location = item.Key,
                                RegisteredLocationCount = location.Count(),
                                RegisteredPhoneNumberCount = phoneNumber.Count(),
                                UUID = uUID
                            });
                        }
                    }
                    var report = unitOfWork.GetRepository<Report>().Get(x => x.UUID == uUID);
                    report.IsOkey = true;
                    report.DateTime = DateTime.Now;
                    unitOfWork.GetRepository<Report>().Update(report);
                    unitOfWork.SaveChangesAsync();
                }
                else
                {
                    var report = unitOfWork.GetRepository<Report>().Get(x => x.UUID == uUID);
                    report.IsOkey = false;
                    report.DateTime = DateTime.Now;
                    report.Explanation = "Not found any contact";
                    unitOfWork.GetRepository<Report>().Update(report);
                    unitOfWork.SaveChangesAsync();
                }
            }
        };
        channel.BasicConsume(queue: Resources.RabbitMQQueueName, autoAck: true, consumer: consumer);
        Console.ReadKey();
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}



