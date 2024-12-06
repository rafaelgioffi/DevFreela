using DevFreela.Application.Commands.CompleteProject;
using DevFreela.Application.Commands.DeleteProject;
using DevFreela.Application.Commands.InsertComment;
using DevFreela.Application.Commands.InsertProject;
using DevFreela.Application.Commands.StartProject;
using DevFreela.Application.Commands.UpdateProject;
using DevFreela.Application.Queries.GetAllProjects;
using DevFreela.Application.Queries.GetProjectById;
using DevFreela.Application.Services;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevFreela.API.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectsController : ControllerBase
    {
        private readonly DevFreelaDbContext _dbContext;
        private readonly IProjectService _service;
        private readonly IMediator _mediator;

        public ProjectsController(IProjectService service, IMediator mediator)
        {
            _service = service;
            _mediator = mediator;
        }

        // GET api/projects?search=crm
        [HttpGet]
        public async Task<IActionResult> Get(string search = "", int page = 0, int size = 3)
        {
            // USANDO SERVICES
            /*
            //var result = _service.GetAll();
            */

            // USANDO MEDIATOR (Forma 1)
            var query = new GetAllProjectsQuery();

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        // GET api/projects/1234
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            //throw new Exception();

            // USANDO SERVICES
            /*
            var result = _service.GetById(id);
            */

            // USANDO MEDIATOR (Forma 2)            
            var result = await _mediator.Send(new GetProjectByIdQuery(id));

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        // POST api/projects
        [HttpPost]
        // USANDO SERVICES
        /*
        public async Task<IActionResult> Post(CreateProjectInputModel model)
        */
        // USANDO MEDIATOR
        public async Task<IActionResult> Post(InsertProjectCommand command)
        {
            #region Usando Services
            /*
            var result = _service.Insert(model);
            */
            #endregion

            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            #region Usando Services
            /*
            return CreatedAtAction(nameof(GetById), new { id = result.Data }, model);
            */
            #endregion
            return CreatedAtAction(nameof(GetById), new { id = result.Data }, command);
        }

        // PUT api/projects/1234
        [HttpPut("{id}")]
        // USANDO SERVICES
        /*
        public IActionResult Put(int id, UpdateProjectInputModel model)
        */
        public async Task<IActionResult> Put(int id, UpdateProjectCommand command)
        {
            // USANDO SERVICES
            /*
            var result = _service.Update(model);
            */
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return NoContent();
        }

        // DELETE api/projects/1234
        [HttpDelete("{id}")]
        // USANDO SERVICES
        /*
                public IActionResult Delete(int id)
        */
        public async Task<IActionResult> Delete(int id)
        {
            // USANDO SERVICES
            /*
            var result = _service.Delete(id);
            */
            var result = await _mediator.Send(new DeleteProjectCommand(id));

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return NoContent();
        }

        // PUT api/projects/1234/start
        [HttpPut("{id}/start")]
        // USANDO SERVICES
        /*
        public IActionResult Start(int id)
        */
        public async Task<IActionResult> Start(int id)
        {
            // USANDO SERVICES
            /*
            var result = _service.Start(id);
            */

            var result = await _mediator.Send(new StartProjectCommand(id));

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return NoContent();
        }

        // PUT api/projects/1234/complete
        [HttpPut("{id}/complete")]
        // USANDO SERVICES
        /*
        public IActionResult Complete(int id)
        */
        public async Task<IActionResult> Complete(int id)
        {
            // USANDO SERVICES
            /*
            var result = _service.Complete(id);
            */
            var result = await _mediator.Send(new CompleteProjectCommand(id));

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return NoContent();
        }

        // POST api/projects/1234/comments
        [HttpPost("{id}/comments")]
        // USANDO SERVICES
        /*
        public IActionResult PostComment(int id, CreateProjectCommentInputModel model)
        */
        public async Task<IActionResult> PostComment(int id, InsertCommentCommand command)
        {
            // USANDO SERVICES
            /*
            var result = _service.InsertComment(id, model);
            */
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return NoContent();
        }

    }
}
