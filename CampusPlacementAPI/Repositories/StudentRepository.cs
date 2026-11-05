using CampusPlacementAPI.Data;
using CampusPlacementAPI.Models;
using CampusPlacementAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementAPI.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<Student>> GetAllAsync() =>
            await _context.Students
                .Include(s => s.User)
                .Include(s => s.Department)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Student?> GetByIdAsync(int studentId) =>
            await _context.Students
                .Include(s => s.User)
                .Include(s => s.Department)
                .FirstOrDefaultAsync(s => s.StudentId == studentId);

        public async Task<Student?> GetByUserIdAsync(string userId) =>
            await _context.Students
                .Include(s => s.User)
                .Include(s => s.Department)
                .FirstOrDefaultAsync(s => s.UserId == userId);

        public async Task<Student?> GetByRegNumberAsync(string regNumber) =>
            await _context.Students
                .Include(s => s.User)
                .Include(s => s.Department)
                .FirstOrDefaultAsync(s => s.RegistrationNumber.ToLower() == regNumber.ToLower());

        public async Task<IEnumerable<Student>> GetByStatusAsync(AccountStatus status) =>
            await _context.Students
                .Include(s => s.User)
                .Include(s => s.Department)
                .Where(s => s.User.AccountStatus == status)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Student> AddAsync(Student student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task UpdateAsync(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int studentId) =>
            await _context.Students.AnyAsync(s => s.StudentId == studentId);
    }
}