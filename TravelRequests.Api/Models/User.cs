using System.ComponentModel.DataAnnotations;

namespace TravelRequests.Api.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = "Solicitante";
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Relación con solicitudes
        public ICollection<TravelRequest> TravelRequests { get; set; } = new List<TravelRequest>();
        
        // Para recuperación de contraseña
        public string? PasswordResetCode { get; set; }
        public DateTime? PasswordResetExpires { get; set; }
    }
}