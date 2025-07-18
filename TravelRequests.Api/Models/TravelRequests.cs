using System.ComponentModel.DataAnnotations;

namespace TravelRequests.Api.Models
{
    public class TravelRequest
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string OriginCity { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string DestinationCity { get; set; } = string.Empty;
        
        [Required]
        public DateTime DepartureDate { get; set; }
        
        [Required]
        public DateTime ReturnDate { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string Justification { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Pendiente"; 
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Relación con usuario
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}