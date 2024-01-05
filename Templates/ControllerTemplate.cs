namespace MicroSwarm.Templates
{
    public static class ControllerTemplate
    {
        public static string Render(string serviceName, string className)
        {
            return
$$"""
using Microsoft.AspNetCore.Mvc;

namespace {{serviceName}}
{
    [ApiController]
    public class {{className}} : ControllerBase
    {
        /// <summary>
        /// Send cmds to the API.
        /// </summary>
        /// <response code="200">Successful.</response>
        /// <response code="400">If the command is invalid.</response>
        [Route(Program.ROOT + Program.CMD)]
        [HttpPost]
        // [Consumes("application/json")]
        public async Task<IActionResult> Cmd(/*[FromBody][Required] JsonDocument jsonData */)
        {
            return await Task.Run(() => BadRequest("{{serviceName}} does not have cmd functionality yet!"));
        }

        /// <summary>
        /// Performs a query based on the specified parameters.
        /// </summary>
        /// <returns>An IActionResult representing the result of the query.</returns>
        /// <response code="200">Successful.</response>
        /// <response code="400">If the command is invalid.</response>
        [Route(Program.ROOT + Program.QUERY)]
        [HttpGet]
        public async Task<IActionResult> Query(/*[FromQuery] string query, [FromQuery] string filter */)
        {
            return await Task.Run(() => BadRequest("{{serviceName}} does not have query functionality yet!"));
        }
    }
}
""";
        }
    }
}