using CampusPlacementAPI.DTOs.Students;

namespace CampusPlacementAPI.Services.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentResponseDto>> GetAllStudentsAsync();
        Task<StudentResponseDto?> GetStudentByIdAsync(int id);
        Task<StudentResponseDto?> GetStudentByUserIdAsync(string userId);
        Task<IEnumerable<StudentResponseDto>> GetPendingStudentsAsync();
        Task<StudentResponseDto> RegisterStudentAsync(StudentRegisterDto dto);
        Task<bool> ApproveStudentAsync(int id, string adminUserId);
        Task<bool> RejectStudentAsync(int id);
        Task<bool> UpdateStudentProfileAsync(string userId, StudentProfileUpdateDto dto);
    }
}