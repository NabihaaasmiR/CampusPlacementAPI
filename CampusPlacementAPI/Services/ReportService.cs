using CampusPlacementAPI.Data;
using CampusPlacementAPI.DTOs.Reports;
using CampusPlacementAPI.Models;
using CampusPlacementAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementAPI.Services
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PlacementSummaryResponseDto> GetOverallSummaryAsync()
        {
            var students = await _context.Students.ToListAsync();
            var totalStudents = students.Count;

            // A student is considered placed if they have at least one application with Status == Offered (or similar placed status)
            var placedStudentIds = await _context.JobApplications
                .Where(ja => ja.Status == ApplicationStatus.Offered)
                .Select(ja => ja.StudentId)
                .Distinct()
                .CountAsync();

            var totalJobs = await _context.JobOpenings.CountAsync();
            var totalCompanies = await _context.Companies.CountAsync();

            var avgSalary = await _context.JobOpenings
                .Where(j => j.SalaryPackage.HasValue)
                .AverageAsync(j => (decimal?)j.SalaryPackage) ?? 0;

            return new PlacementSummaryResponseDto
            {
                TotalStudents = totalStudents,
                PlacedStudents = placedStudentIds,
                UnplacedStudents = totalStudents - placedStudentIds,
                OverallPlacementPercentage = totalStudents > 0 ? (decimal)placedStudentIds / totalStudents * 100 : 0,
                TotalJobOpenings = totalJobs,
                TotalCompaniesRecruiting = totalCompanies,
                AverageSalaryPackage = avgSalary
            };
        }

        public async Task<IEnumerable<DepartmentStatsDto>> GetDepartmentStatsAsync()
        {
            var students = await _context.Students.ToListAsync();
            var placedStudentIds = await _context.JobApplications
                .Where(ja => ja.Status == ApplicationStatus.Offered)
                .Select(ja => ja.StudentId)
                .Distinct()
                .ToListAsync();

            var deptGroups = students.GroupBy(s => s.Department);
            var result = new List<DepartmentStatsDto>();

            foreach (var group in deptGroups)
            {
                var total = group.Count();
                var placed = group.Count(s => placedStudentIds.Contains(s.StudentId));

                result.Add(new DepartmentStatsDto
                {
                    Department = group.Key,
                    TotalStudents = total,
                    PlacedStudents = placed,
                    PlacementPercentage = total > 0 ? (decimal)placed / total * 100 : 0
                });
            }

            return result;
        }

        public async Task<IEnumerable<CompanyRecruitmentStatsDto>> GetCompanyRecruitmentStatsAsync()
        {
            var companies = await _context.Companies.Include(c => c.CompanyId).ToListAsync();
            var jobs = await _context.JobOpenings.ToListAsync();
            var offers = await _context.JobApplications.Where(ja => ja.Status == ApplicationStatus.Offered).ToListAsync();

            var stats = new List<CompanyRecruitmentStatsDto>();

            foreach (var company in companies)
            {
                var companyJobs = jobs.Where(j => j.CompanyId == company.CompanyId).ToList();
                var jobIds = companyJobs.Select(j => j.JobId).ToList();
                var totalOffers = offers.Count(o => jobIds.Contains(o.JobId));

                stats.Add(new CompanyRecruitmentStatsDto
                {
                    CompanyId = company.CompanyId,
                    CompanyName = company.CompanyName,
                    TotalJobOpenings = companyJobs.Count,
                    TotalOffersMade = totalOffers
                });
            }

            return stats;
        }
    }
}