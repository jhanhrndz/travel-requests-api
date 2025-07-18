using System.ComponentModel.DataAnnotations;

namespace TravelRequests.Api.DTOs
{
    public class CreateTravelRequestDto
    {
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
    }

    public class TravelRequestDto
    {
        public int Id { get; set; }
        public string OriginCity { get; set; } = string.Empty;
        public string DestinationCity { get; set; } = string.Empty;
        public DateTime DepartureDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Justification { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
    }

    public class UpdateTravelRequestStatusDto
    {
        [Required]
        public string Status { get; set; } = string.Empty;
    }
}