using System;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Api.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Data;
using Api.Helper;

namespace Api.Controllers
{
    [Route("graphql")]
    public class GraphQLController : Controller
    {
        private IDocumentExecuter _documentExecuter { get; set; }
        private ISchema _schema { get; set; }
        private readonly ILogger _logger;

        private readonly SignInManager<ApplicationUser> _manager;

        public GraphQLController(IDocumentExecuter documentExecuter, ISchema schema, ILogger<GraphQLController> logger, SignInManager<ApplicationUser> manager)
        {
            _documentExecuter = documentExecuter;
            _schema = schema;
            _logger = logger;
            _manager = manager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogInformation("Got request for GraphiQL. Sending GUI back");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
        {
            if (query == null) { throw new ArgumentNullException(nameof(query)); }

            var executionOptions = new ExecutionOptions { Schema = _schema, Query = query.Query };

            try
            {
                var result = await _documentExecuter.ExecuteAsync(executionOptions).ConfigureAwait(false);

                if (result.Errors?.Count > 0)
                {
                    _logger.LogError("GraphQL errors: {0}", result.Errors);
                    return BadRequest(result);
                }

                _logger.LogDebug("GraphQL execution result: {result}", JsonConvert.SerializeObject(result.Data));
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Document exexuter exception", ex);
                return BadRequest(ex);
            }
        }
    }
}
