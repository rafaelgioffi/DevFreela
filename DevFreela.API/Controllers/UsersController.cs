using DevFreela.Application.Models;
using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Auth;
using DevFreela.Infrastructure.Notifications;
using DevFreela.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace DevFreela.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly DevFreelaDbContext _dbContext;
        private readonly IAuthService _authService;
        private readonly IMemoryCache _cache;
        private readonly IEmailService _emailService;

        public UsersController(
            DevFreelaDbContext dbContext, 
            IAuthService authservice,
            IMemoryCache cache,
            IEmailService emailService)
        {
            _dbContext = dbContext;
            _authService = authservice;
            _cache = cache;
            _emailService = emailService;
        }

        // GET api/users
        [HttpGet]
        public IActionResult Get()
        {
            var users = _dbContext.Users.ToList();

            if (users.Any())
                return Ok(users);
            else
                return NotFound("Nenhum usuário.. :(");
        }

        // GET api/users/1234
        [HttpGet("{id}")]
        public IActionResult GetById(int id) 
        {
            var user = _dbContext.Users
                .Include(u => u.Skills)
                .ThenInclude(u => u.Skill)
                .SingleOrDefault(u => u.Id == id);

            if (user is null)
                return NotFound();

            var model = UserViewModel.FromEntity(user);

            return Ok(model);
        }
                
        // POST api/users
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Post(CreateUserInputModel model)
        {
            var hash = _authService.ComputeHash(model.Password);

            var user = new User(model.FullName, model.Email, model.BirthDate, hash, model.Role);

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return NoContent();
        }

        [HttpPost("{id}/skills")]
        public IActionResult PostSkills(int id, UserSkillsInputModel model)
        {
            var userSkills = model.SkillsIds.Select(s => new UserSkill(id, s)).ToList();

            _dbContext.UserSkills.AddRange(userSkills);
            _dbContext.SaveChanges();

            return NoContent();
        }

        // PUT api/users/1234/profile-picture
        [HttpPut("{id}/profile-picture")]
        public IActionResult PostProfilePicture(int id, IFormFile file)
        {
            var description = $"File: {file.FileName}, Size: {file.Length}";
            // Processar a imagem...
            return Ok(description);
        }

        [HttpPut("login")]
        [AllowAnonymous]
        public  IActionResult Login(LoginInputModel model)
        {
            var hash = _authService.ComputeHash(model.Password);

            var user = _dbContext.Users
                .SingleOrDefault(u => u.Email == model.Email && u.Password == hash);

            if (user is null)
            {
                var error = ResultViewModel<LoginViewModel?>.Error("Erro de login.");

                return BadRequest(error);
            }

            var token = _authService.GenerateToken(user.Email, user.Role);

            var viewModel = new LoginViewModel(token);

            var result = ResultViewModel<LoginViewModel>.Success(viewModel);

            return Ok(result);
        }

        [HttpPost("password-recovery/request")]
        public async Task<IActionResult> RequestPasswordRecovery(PasswordRecoveryRequestInputModel model)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == model.Email);
            if (user is null)
            {
                return BadRequest();
            }

            var code = new Random().Next(100000, 999999).ToString();

            var cacheKey = $"RecoveryCode:{model.Email}";
            _cache.Set(cacheKey, code, TimeSpan.FromMinutes(10));

            await _emailService.SendAsync(
                user.Email,
                "Código de Recuperação",
                $"Seu código de recuperação é: {code}");

            return NoContent();
        }

        [HttpPost("password-recovery/validate")]
        public IActionResult ValidateRecoveryCode(ValidadateRecoveryCodeInputModel model)
        {
            var cacheKey = $"RecoveryCode:{model.Email}";

            if (!_cache.TryGetValue(cacheKey, out string? code) || code != model.Code)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPost("password-recovery/change")]
        public async Task<IActionResult> ChangePassword(ChangePasswordInputModel model)
        {
            var cacheKey = $"RecoveryCode:{model.Email}";

            if (!_cache.TryGetValue(cacheKey, out string? code) || code != model.Code)
            {
                return BadRequest();
            }

            _cache.Remove(cacheKey);

            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == model.Email);
            
            if (user is null)
            {
                return BadRequest();
            }

            var hash = _authService.ComputeHash(model.NewPassword);

            user.UpdatePassword(hash);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

    }
}
