# The project consists of 4 layers with its general design.

The first one is Data: It contains entity models, Dto(data transfer object) and migrations.

Second layer DataAccess: This layer contains Repository patterns and "RabbitMq Extensions". This layer is referenced to all APIs that require us to access the data.

Third layer Consumer: A console application written to listen to the RabbitMq queue structure is running here.

The fourth layer(s): It contains the presentation i.e. apis. There are 3 projects, 2 api and an apigateway, in this section and we can say that this is the part where the main work is carried out.

# Used libraries and technologies:
While I installed some technologies in my locale, I preferred to install some of them with docker.

-PostgreSQL

-RabbitMQ

-MongoDB

## Libraries installed in the project:

-Microsoft.EntityFrameworkCore (6.0.9)

-Microsoft.EntityFrameworkCore.Design (6.0.9)

-Microsoft.EntityFrameworkCore.Tools (6.0.9)

-Microsoft.VisualStudio.Azure.Containers.Tools.Targets (1.17.0)

-Npgsql.EntityFrameworkCore.PostgreSQL (6.0.7)

-MongoDB.Bson (2.17.1)

-Newtonsoft.Json (13.0.1)

-RabbitMQ.Client (6.4.0)

-Ocelot (18.0.0)

-Swashbuckle.AspNetCore (6.2.3)

# What we need to do in order to run and test the project:

-We map the connection string in the "Data" layer to the settings of the db we want to run(File path : *\RiseTechnologyProject.Data\Properties\Resources.resx).

-We map the config settings in the "DataAcces" layer according to our local(File path : *\RiseTechnologyProject.DataAccess\Properties\Resources.resx).

-We also map the config settings in the "Consumer" layer according to our local(File path : *\RiseTechnologyProject.Consumer\Properties\Resources.resx).

### If you want to uninstall the project on different ports after the connection settings, you must set the port settings from the "launchSettings.json" file of the api and the project is ready to start.

# Test Phase

-Set the project startup settings to "Multiple Select". Projects to launch: "RiseTechnologyProject.ApiGateway", "...Consumer", "...ReportApi", "...UserApi" and get the project up and running.

-Import the "RiseTechnologyPostmanApiRequestExample-postman_collection.json" file located in the project directory in postman.

-If you have changed the config settings, do not forget to change the routing of the requests.

-Then test the microservice architecture that sends requests to other APIs via "ApiGateway".


