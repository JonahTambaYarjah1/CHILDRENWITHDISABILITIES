using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChildrenWithDisabilitiesAPI.DTOs;
using ChildrenWithDisabilitiesAPI.Models;
using ChildrenWithDisabilitiesAPI.Services;
using ChildrenWithDisabilitiesAPI.Data;
using System.Net.Mail;
using System.Net;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace ChildrenWithDisabilitiesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AuthService authService, AppDbContext context, IConfiguration config)
        {
            _authService = authService;
            _context = context;
            _config = config;
        }

        // ✅ REGISTER
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            var existing = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (existing != null)
                return BadRequest("Email already exists.");

            var verificationToken = new Random().Next(100000, 999999).ToString();

            var user = new User
            {
                Full_Name = dto.Full_Name,
                Email = dto.Email,
                Password = dto.Password,
                EmailVerificationToken = verificationToken
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            SendEmail(dto.Email, dto.Full_Name, "Verify Your Email",
                $"Hello {dto.Full_Name},\n\nYour verification code is: {verificationToken}\nUse this code to activate your account.");

            return Ok("User registered. Check your email for verification code.");
        }

        // ✅ LOGIN + TOKEN
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var user = await _authService.Login(dto.Email, dto.Password);
            if (user == null)
                return Unauthorized("Invalid credentials.");

            var token = GenerateJwtToken(user);

            return Ok(new
            {
                token,
                full_Name = user.Full_Name,
                email = user.Email
            });
        }

        // ✅ REQUEST PASSWORD RESET
        [HttpPost("request-reset")]
        public async Task<IActionResult> RequestReset([FromBody] ResetRequestDTO dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null) return BadRequest("User not found.");

            var resetToken = new Random().Next(100000, 999999).ToString();
            user.Reset_Token = int.Parse(resetToken);
            user.Reset_Token_Expiry = DateTime.UtcNow.AddMinutes(5);

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            SendEmail(dto.Email, user.Full_Name, "Password Reset Request",
                $"Your password reset token is: {resetToken}\nIt will expire in 5 minutes.");

            return Ok("Reset token sent to email.");
        }

        // ✅ CONFIRM PASSWORD RESET
        [HttpPost("confirm-reset")]
        public async Task<IActionResult> ConfirmReset([FromBody] ConfirmResetDTO dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null) return BadRequest("User not found.");
            if (user.Reset_Token == null || user.Reset_Token_Expiry == null)
                return BadRequest("No reset token set.");
            if (user.Reset_Token != int.Parse(dto.Token))
                return BadRequest("Invalid token.");
            if (user.Reset_Token_Expiry < DateTime.UtcNow)
                return BadRequest("Token expired.");

            user.Password = dto.NewPassword;
            user.Reset_Token = null;
            user.Reset_Token_Expiry = null;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok("Password reset successfully.");
        }

        // ✅ Helper: Generate JWT Token
        private string GenerateJwtToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("FullName", user.Full_Name)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // ✅ Helper: Send Email
        private void SendEmail(string toEmail, string toName, string subject, string body)
        {
            var fromEmail = _config["Email:From"];
            var fromPassword = _config["Email:AppPassword"];

            var fromAddress = new MailAddress(fromEmail, "Children With Disabilities");
            var toAddress = new MailAddress(toEmail, toName);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }
}
