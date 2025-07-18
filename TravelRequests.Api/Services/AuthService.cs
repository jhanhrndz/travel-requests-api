using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TravelRequests.Api.Data;
using TravelRequests.Api.DTOs;
using TravelRequests.Api.Models;

namespace TravelRequests.Api.Services
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterUserDto dto);
        Task<LoginResponseDto> LoginAsync(LoginDto dto);
        Task<string> ForgotPasswordAsync(ForgotPasswordDto dto);
        Task<string> ResetPasswordAsync(ResetPasswordDto dto);
        Task<List<UserDto>> GetAllUsersAsync();
    }
    
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;
        
        public AuthService(ApplicationDbContext context, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }
        
        public async Task<string> RegisterAsync(RegisterUserDto dto)
        {
            try
            {
                // Verificar si el usuario ya existe
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
                if (existingUser != null)
                {
                    throw new Exception("El usuario ya existe");
                }
                
                // Crear nuevo usuario
                var user = new User
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    Role = dto.Role
                };
                
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                
                return "Usuario registrado exitosamente";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar usuario");
                throw;
            }
        }
        
        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            try
            {
                // Buscar usuario por email
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
                if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                {
                    throw new Exception("Credenciales inválidas");
                }
                
                // Generar token JWT
                var token = GenerateJwtToken(user);
                
                return new LoginResponseDto
                {
                    Token = token,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al iniciar sesión");
                throw;
            }
        }
        
        public async Task<string> ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
                if (user == null)
                {
                    throw new Exception("Usuario no encontrado");
                }
                
                // Generar código de 6 dígitos
                var code = new Random().Next(100000, 999999).ToString();
                
                // Guardar código con expiración de 5 minutos
                user.PasswordResetCode = code;
                user.PasswordResetExpires = DateTime.UtcNow.AddMinutes(5);
                
                await _context.SaveChangesAsync();
                
                return $"Código de recuperación: {code} (válido por 5 minutos)";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar código de recuperación");
                throw;
            }
        }
        
        public async Task<string> ResetPasswordAsync(ResetPasswordDto dto)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
                if (user == null)
                {
                    throw new Exception("Usuario no encontrado");
                }
                
                // Verificar código
                if (user.PasswordResetCode != dto.Code || 
                    user.PasswordResetExpires == null || 
                    user.PasswordResetExpires < DateTime.UtcNow)
                {
                    throw new Exception("Código inválido o expirado");
                }
                
                // Actualizar contraseña
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
                user.PasswordResetCode = null;
                user.PasswordResetExpires = null;
                
                await _context.SaveChangesAsync();
                
                return "Contraseña actualizada exitosamente";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al restablecer contraseña");
                throw;
            }
        }
        
        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            try
            {
                var users = await _context.Users
                    .Select(u => new UserDto
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Email = u.Email,
                        Role = u.Role,
                        CreatedAt = u.CreatedAt
                    })
                    .ToListAsync();
                
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuarios");
                throw;
            }
        }
        
        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            };
            
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: credentials
            );
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}