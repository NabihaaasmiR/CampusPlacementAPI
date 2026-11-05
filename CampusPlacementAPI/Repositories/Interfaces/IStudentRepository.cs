using CampusPlacementAPI.Models;

namespace CampusPlacementAPI.Repositories.Interfaces
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllAsync();
        Task<Student?> GetByIdAsync(int studentId);
        Task<Student?> GetByUserIdAsync(string userId);
        Task<Student?> GetByRegNumberAsync(string regNumber);
        Task<IEnumerable<Student>> GetByStatusAsync(AccountStatus status);
        Task<Student> AddAsync(Student student);
        Task UpdateAsync(Student student);
        Task<bool> ExistsAsync(int studentId);
    }
}