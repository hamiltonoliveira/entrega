using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using Domain.Dto;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Domain.Entities.User;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace SisTarefa.Ui.Controller
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {

        private readonly IAutenticarService _autenticarService;
        private readonly IUserService _usersService;
        protected readonly DataContext _db;

        private readonly IMapper _mapper;

        public UsersController(DataContext db, IMapper mapper, IAutenticarService autenticarService, IUserService usersService)
        {
            _db = db;
            _mapper = mapper;
            _autenticarService = autenticarService;
            _usersService = usersService;
        }

        [AllowAnonymous]
        [HttpPost("Criar")]
        [ProducesResponseType(typeof(UserDTO), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> Criar([FromBody] UserDTO userDto)
        {
            if (userDto is null)
            {
                throw new ArgumentNullException(nameof(userDto));
            }

            UserValidation validator = new UserValidation();
            var validationResult = validator.Validate(userDto);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                return BadRequest(errors);
            }

           
            User user = _mapper.Map<User>(userDto);

            user.Pessoa = new Pessoa(userDto.Celular, userDto.Foto);

            user.Password = Criptograph.Encrypt(userDto.Password);
            user.SetUserName(user.Email);
            TokensDto? tokens = null;

            try
            {
                List<User> usuarios = await _usersService.WhereAsync(x => x.Email == userDto.Email);

                if (usuarios.Count == 0)
                {
                    var _idUsers = await _usersService.InsertAsync(user);
                    tokens = await _autenticarService.GerarToKen(userDto.Email);
                }

            }
            catch (DbUpdateException ex)
            {
                var errorResponse = new ErrorResponse
                {
                    Message = "Ocorreu um erro ao criar o usuário.",
                    ErrorCode = "CREATE_USER_ERROR"
                };
                return BadRequest(errorResponse);
            }
            return Ok(tokens);
        }

        [Authorize(Roles = "Desenvolvedor")]
        [HttpGet("DadosUsuario")]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), 200)]
        public IActionResult DadosUsuario()
        {
            var claims = User.Claims.GetEnumerator();
            var claims1 = User.Claims.Select(claim => new { claim.Type, claim.Value }).ToArray();
            string Guid = string.Empty;
            int exp = 0;

            foreach (var item in claims1)
            {
                if (item.Type == "Guid")
                {
                    Guid = item.Value;
                }
                if (item.Type == "exp")
                {
                    exp = int.Parse(item.Value);
                    break;
                }
            }


            DateTimeOffset expirationTime = DateTimeOffset.FromUnixTimeSeconds(exp);
            DateTime localDateTime = expirationTime.LocalDateTime;

            bool isExpired = DateTimeOffset.Now > localDateTime;

            if (isExpired)
            {
                return BadRequest("Token expirado.");
            }

            var query = from user in _db.User
                        where user.GuidI == Guid
                        select new
                        {
                            UserId = user.Id,
                            UserName = user.UserName,
                            Email = user.Email
                         };

            return Ok(query);
        }

        [AllowAnonymous]
        [HttpPost("RenovarToken")]
        [ProducesResponseType(typeof(IEnumerable<RenovarToken>), 200)]
        public async Task<IActionResult> RenovarToken([FromBody] RenovarToken renovarToken)
        {
            TokensDto? tokens = null;
            try
            {
                tokens = await _autenticarService.GerarToKen(renovarToken.Email);
            }
            catch (DbUpdateException)
            {
                var errorResponse = new ErrorResponse
                {
                    Message = "Ocorreu um erro ao criar o usuário.",
                    ErrorCode = "CREATE_USER_ERROR"
                };
                return BadRequest(errorResponse);
            }
            return Ok(tokens);
        }
    }
}
