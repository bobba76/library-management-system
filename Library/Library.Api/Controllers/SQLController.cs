using Library.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers;

public class SQLController : ApiController
{
    [HttpPut]
    [Route("setup/connection-string/{connectionString}")]
    [Produces("application/json")]
    public string UpdateConnectionString(string connectionString)
    {
        return DB.UpdateConnectionString(connectionString);
    }

    [HttpGet]
    [Route("setup")]
    [Produces("application/json")]
    public string Setup()
    {
        return DB.Setup();
    }

    [HttpDelete]
    [Route("setup/table")]
    [Produces("application/json")]
    public string DropTable()
    {
        return DB.DropTable();
    }

    [HttpDelete]
    [Route("setup/data")]
    [Produces("application/json")]
    public string RemoveData()
    {
        return DB.RemoveData();
    }
}