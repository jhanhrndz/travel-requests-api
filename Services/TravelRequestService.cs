using Microsoft.EntityFrameworkCore;
using TravelRequests.Api.Data;
using TravelRequests.Api.DTOs;
using TravelRequests.Api.Models;

namespace TravelRequests.Api.Services
{
    public interface ITravelRequestService
    {
        Task<string> CreateTravelRequestAsync(CreateTravelRequestDto dto, int userId);
        Task<List<TravelRequestDto>> GetMyTravelRequestsAsync(int userId);
        Task<List<TravelRequestDto>> GetAllTravelRequestsAsync();
        Task<string> UpdateTravelRequestStatusAsync(int requestId, UpdateTravelRequestStatusDto dto);
    }
    
    public class TravelRequestService : ITravelRequestService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TravelRequestService> _logger;
        
        public TravelRequestService(ApplicationDbContext context, ILogger<TravelRequestService> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public async Task<string> CreateTravelRequestAsync(CreateTravelRequestDto dto, int userId)
        {
            try
            {
                if (dto.OriginCity.Equals(dto.DestinationCity, StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception("La ciudad de origen no puede ser igual a la de destino");
                }
                
                if (dto.ReturnDate <= dto.DepartureDate)
                {
                    throw new Exception("La fecha de regreso debe ser posterior a la fecha de ida");
                }
                
                if (dto.DepartureDate <= DateTime.UtcNow)
                {
                    throw new Exception("La fecha de ida debe ser futura");
                }
                
                var travelRequest = new TravelRequest
                {
                    OriginCity = dto.OriginCity,
                    DestinationCity = dto.DestinationCity,
                    DepartureDate = dto.DepartureDate,
                    ReturnDate = dto.ReturnDate,
                    Justification = dto.Justification,
                    UserId = userId
                };
                
                _context.TravelRequests.Add(travelRequest);
                await _context.SaveChangesAsync();
                
                return "Solicitud de viaje creada exitosamente";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear solicitud de viaje");
                throw;
            }
        }
        
        public async Task<List<TravelRequestDto>> GetMyTravelRequestsAsync(int userId)
        {
            try
            {
                var requests = await _context.TravelRequests
                    .Where(tr => tr.UserId == userId)
                    .Include(tr => tr.User)
                    .Select(tr => new TravelRequestDto
                    {
                        Id = tr.Id,
                        OriginCity = tr.OriginCity,
                        DestinationCity = tr.DestinationCity,
                        DepartureDate = tr.DepartureDate,
                        ReturnDate = tr.ReturnDate,
                        Justification = tr.Justification,
                        Status = tr.Status,
                        CreatedAt = tr.CreatedAt,
                        UserName = tr.User.Name,
                        UserEmail = tr.User.Email
                    })
                    .OrderByDescending(tr => tr.CreatedAt)
                    .ToListAsync();
                
                return requests;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener solicitudes de viaje del usuario");
                throw;
            }
        }
        
        public async Task<List<TravelRequestDto>> GetAllTravelRequestsAsync()
        {
            try
            {
                var requests = await _context.TravelRequests
                    .Include(tr => tr.User)
                    .Select(tr => new TravelRequestDto
                    {
                        Id = tr.Id,
                        OriginCity = tr.OriginCity,
                        DestinationCity = tr.DestinationCity,
                        DepartureDate = tr.DepartureDate,
                        ReturnDate = tr.ReturnDate,
                        Justification = tr.Justification,
                        Status = tr.Status,
                        CreatedAt = tr.CreatedAt,
                        UserName = tr.User.Name,
                        UserEmail = tr.User.Email
                    })
                    .OrderByDescending(tr => tr.CreatedAt)
                    .ToListAsync();
                
                return requests;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las solicitudes de viaje");
                throw;
            }
        }
        
        public async Task<string> UpdateTravelRequestStatusAsync(int requestId, UpdateTravelRequestStatusDto dto)
        {
            try
            {
                var request = await _context.TravelRequests.FindAsync(requestId);
                if (request == null)
                {
                    throw new Exception("Solicitud no encontrada");
                }
                
                if (dto.Status != "Aprobada" && dto.Status != "Rechazada")
                {
                    throw new Exception("Estado inválido. Solo se permite 'Aprobada' o 'Rechazada'");
                }
                
                request.Status = dto.Status;
                await _context.SaveChangesAsync();
                
                return $"Solicitud {dto.Status.ToLower()} exitosamente";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar estado de solicitud");
                throw;
            }
        }
    }
}