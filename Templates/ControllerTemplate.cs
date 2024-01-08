namespace MicroSwarm.Templates
{
    public static class ControllerTemplate
    {
        public static string Render(string solutionName, string serviceName, string className)
        {
            return
$$"""
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using {{solutionName}}Core.Actors;

namespace {{serviceName}}
{
    [ApiController]
    public class {{className}}(IActorBridge bridge) : ControllerBase
    {
        private readonly IActorBridge _bridge = bridge;
        /// <summary>
        /// Send cmds to the API.
        /// </summary>
        /// <response code="200">Successful.</response>
        /// <response code="400">If the command is invalid.</response>
        [Route(Program.ROOT + Program.CMD)]
        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> Cmd([FromBody][Required] JsonDocument jsonData)
        {
            var result = await _bridge.Ask(jsonData);
            return result.Ok switch
            {
                true => Ok(result.Value),
                false => BadRequest(result.Value),
            };
        }

        /// <summary>
        /// Performs a query based on the specified parameters.
        /// </summary>
        /// <returns>An IActionResult representing the result of the query.</returns>
        /// <response code="200">Successful.</response>
        /// <response code="400">If the command is invalid.</response>
        [Route(Program.ROOT + Program.QUERY)]
        [HttpGet]
        public async Task<IActionResult> Query([FromQuery] string filter)
        {
            var result = await _bridge.Ask(filter);
            return result.Ok switch
            {
                true => Ok(result.Value),
                false => BadRequest(result.Value),
            };
        }
    }
}
""";
        }
    }
}