using DynamodbTraining.V1.Boundary.Request;
using DynamodbTraining.V1.Boundary.Response;
using DynamodbTraining.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DynamodbTraining.V1.Controllers
{
    [ApiController]
    [Route("api/v1/residents")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    public class DynamodbTrainingController : BaseController
    {
        private readonly IGetByIdUseCase _getByIdUseCase;
        private readonly IPostNewPersonUseCase _newPersonUseCase;
        private readonly IUpdatePersonUseCase _updatePersonUseCase;
        public DynamodbTrainingController(IGetByIdUseCase getByIdUseCase, IPostNewPersonUseCase newPersonUseCase, IUpdatePersonUseCase updatePersonUseCase)
        {
            _getByIdUseCase = getByIdUseCase;
            _newPersonUseCase = newPersonUseCase;
            _updatePersonUseCase = updatePersonUseCase;
        }

        /// <summary>
        /// ...
        /// </summary>
        /// <response code="200">...</response>
        /// <response code="404">No ? found for the specified ID</response>
        [ProducesResponseType(typeof(PersonResponseObject), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("{Id}")]
        public async Task<IActionResult> ViewRecord([FromQuery] PersonQueryObject personQueryObject )
        {
            var response = await _getByIdUseCase.Execute(personQueryObject).ConfigureAwait(false);
            if (response == null) return NotFound(personQueryObject.Id);
            return Ok(response);
        }

        [ProducesResponseType(typeof(PersonResponseObject), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> PostNewPerson([FromBody] PersonRequestObject personRequestObject)
        {

            var person = await _newPersonUseCase.ExecuteAsync(personRequestObject)
                .ConfigureAwait(false);

            return Created(new Uri($"api/v1/residents/{person.Id}", UriKind.Relative), person);
        }

        [ProducesResponseType(typeof(PersonResponseObject), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPatch]
        [Route("{id}")]

        public async Task<IActionResult> UpdatePersonByIdAsync([FromBody]PersonRequestObject personRequestObject, [FromRoute] PersonQueryObject query)
        {
            if (query.Id == null) return BadRequest(query.Id);
            query.Id = personRequestObject.Id;

            
            var person = await _updatePersonUseCase.ExecuteAsync(personRequestObject, query).ConfigureAwait(false);
            if (person == null) return NotFound(query.Id);

            return NoContent();
        }
    }
}
