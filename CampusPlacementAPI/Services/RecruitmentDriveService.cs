using CampusPlacementAPI.DTOs.RecruitmentDrives;
using CampusPlacementAPI.Models;
using CampusPlacementAPI.Repositories.Interfaces;
using CampusPlacementAPI.Services.Interfaces;

namespace CampusPlacementAPI.Services
{
    public class RecruitmentDriveService : IRecruitmentDriveService
    {
        private readonly IRecruitmentDriveRepository _driveRepo;
        private readonly IJobOpeningRepository _jobRepo;

        public RecruitmentDriveService(IRecruitmentDriveRepository driveRepo, IJobOpeningRepository jobRepo)
        {
            _driveRepo = driveRepo;
            _jobRepo = jobRepo;
        }

        public async Task<RecruitmentDriveResponseDto> CreateDriveAsync(RecruitmentDriveCreateDto dto)
        {
            var job = await _jobRepo.GetByIdAsync(dto.JobId);
            if (job == null)
                throw new KeyNotFoundException("Job opening not found.");

            var drive = new RecruitmentDrive
            {
                JobId = dto.JobId,
                CollegeId = dto.CollegeId,
                DriveDate = dto.DriveDate,
                Venue = dto.Venue,
                Description = dto.Description,
                Status = DriveStatus.Scheduled
            };

            await _driveRepo.AddAsync(drive);

            var created = await _driveRepo.GetByIdAsync(drive.DriveId);
            return MapToDto(created!);
        }

        public async Task<RecruitmentDriveResponseDto> UpdateDriveAsync(int driveId, RecruitmentDriveUpdateDto dto)
        {
            var drive = await _driveRepo.GetByIdAsync(driveId);
            if (drive == null)
                throw new KeyNotFoundException("Recruitment drive not found.");

            drive.DriveDate = dto.DriveDate;
            drive.Venue = dto.Venue;
            drive.Description = dto.Description;
            drive.Status = dto.Status;

            await _driveRepo.UpdateAsync(drive);
            return MapToDto(drive);
        }

        public async Task<RecruitmentDriveResponseDto?> GetDriveByIdAsync(int driveId)
        {
            var drive = await _driveRepo.GetByIdAsync(driveId);
            return drive == null ? null : MapToDto(drive);
        }

        public async Task<IEnumerable<RecruitmentDriveResponseDto>> GetDrivesForJobAsync(int jobId)
        {
            var drives = await _driveRepo.GetByJobIdAsync(jobId);
            return drives.Select(MapToDto);
        }

        public async Task<IEnumerable<RecruitmentDriveResponseDto>> GetDrivesForCollegeAsync(int collegeId)
        {
            var drives = await _driveRepo.GetByCollegeIdAsync(collegeId);
            return drives.Select(MapToDto);
        }

        public async Task<IEnumerable<RecruitmentDriveResponseDto>> GetAllDrivesAsync()
        {
            var drives = await _driveRepo.GetAllAsync();
            return drives.Select(MapToDto);
        }

        public async Task DeleteDriveAsync(int driveId)
        {
            var drive = await _driveRepo.GetByIdAsync(driveId);
            if (drive == null)
                throw new KeyNotFoundException("Recruitment drive not found.");

            await _driveRepo.DeleteAsync(drive);
        }

        private static RecruitmentDriveResponseDto MapToDto(RecruitmentDrive rd) => new()
        {
            DriveId = rd.DriveId,
            JobId = rd.JobId,
            JobTitle = rd.JobOpening?.JobTitle ?? string.Empty,
            CompanyName = rd.JobOpening?.Company?.CompanyName ?? string.Empty,
            CollegeId = rd.CollegeId,
            CollegeName = rd.College?.CollegeName ?? string.Empty,
            DriveDate = rd.DriveDate,
            Venue = rd.Venue,
            Description = rd.Description,
            Status = rd.Status
        };
    }
}