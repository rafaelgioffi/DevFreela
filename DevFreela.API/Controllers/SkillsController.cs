using DevFreela.Application.Models;
using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace DevFreela.API.Controllers
{
    [Route("api/skills")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        private readonly DevFreelaDbContext _dbContext;

        public SkillsController(DevFreelaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET api/skills
        [HttpGet]
        public IActionResult GetAll()
        {
            var skills = _dbContext.Skills.ToList();

            return Ok(skills);
        }

        // POST api/skills
        [HttpPost]
        public IActionResult Post(CreateSkillInputModel model)
        {
            var skill = new Skill(model.Description);

            _dbContext.Skills.Add(skill);
            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}
