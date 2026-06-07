using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Data;
using PUPAcadPortal.Models;

namespace PUPAcadPortal.Services
{
    public class SubjectService
    {
        public async Task<List<object>> GetSubjectsForGridAsync()
        {
            using (var context = new AppDbContext())
            {
                return await context.Subjects
                    .Include(s => s.Department)
                    .OrderBy(s => s.SubjectCode)
                    .Select(s => new {
                        s.SubjectId,
                        s.SubjectCode,
                        s.SubjectName,
                        Department = s.Department != null ? s.Department.DepartmentName : "Unassigned",
                        s.LecUnits,
                        s.LabUnits,
                        s.Units
                    })
                    .Cast<object>()
                    .ToListAsync();
            }
        }

        public async Task DeleteSubjectAsync(string subjectId)
        {
            using (var context = new AppDbContext())
            {
                var subject = await context.Subjects.FindAsync(subjectId);
                if (subject == null) throw new Exception("Subject not found.");

                context.Subjects.Remove(subject);
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsSemesterActiveAsync()
        {
            using (var context = new AppDbContext())
            {
                return await context.AcademicPeriods.AnyAsync(ap => ap.Status == "Current");
            }
        }

        public async Task<List<Department>> GetActiveDepartmentsAsync()
        {
            using (var context = new AppDbContext())
            {
                return await context.Departments
                                    .Where(d => d.IsActive == true)
                                    .ToListAsync();
            }
        }

        public async Task<Subject> GetSubjectByIdAsync(string subjectId)
        {
            using (var context = new AppDbContext())
            {
                return await context.Subjects.FindAsync(subjectId);
            }
        }

        public async Task<List<string>> GetPrerequisiteIdsAsync(string subjectId)
        {
            using (var context = new AppDbContext())
            {
                return await context.SubjectPrerequisites
                                    .Where(sp => sp.SubjectId == subjectId)
                                    .Select(sp => sp.RequiredSubjectId)
                                    .ToListAsync();
            }
        }
        public async Task<List<DepartmentPrefix>> GetDepartmentPrefixesAsync()
        {
            using (var context = new AppDbContext())
            {
                return await context.DepartmentPrefixes.ToListAsync();
            }
        }

        public async Task<List<Subject>> GetAllSubjectsAsync()
        {
            using (var context = new AppDbContext())
            {
                return await context.Subjects.OrderBy(s => s.SubjectCode).ToListAsync();
            }
        }

        public async Task CreateSubjectAsync(string code, string name, int deptId, string desc, int lec, int lab, int totalUnits, List<string> prerequisiteIds)
        {
            using (var context = new AppDbContext())
            {
                bool exists = await context.Subjects.AnyAsync(s => s.SubjectCode == code);
                if (exists) throw new Exception($"The subject code '{code}' already exists.");

                int subjectCount = await context.Subjects.CountAsync();
                string newId = $"SUB-{(subjectCount + 1):D4}";

                var newSubject = new Subject
                {
                    SubjectId = newId,
                    SubjectCode = code,
                    SubjectName = name,
                    DepartmentId = deptId,
                    Description = desc,
                    LecUnits = lec,
                    LabUnits = lab,
                    Units = totalUnits
                };

                context.Subjects.Add(newSubject);

                if (prerequisiteIds != null && prerequisiteIds.Count > 0)
                {
                    foreach (var reqId in prerequisiteIds)
                    {
                        context.SubjectPrerequisites.Add(new SubjectPrerequisite
                        {
                            SubjectId = newId,
                            RequiredSubjectId = reqId
                        });
                    }
                }

                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateSubjectAsync(string subjectId, string code, string name, int deptId, string desc, int lec, int lab, int totalUnits, List<string> prerequisiteIds)
        {
            using (var context = new AppDbContext())
            {
                var subject = await context.Subjects.FindAsync(subjectId);
                if (subject == null) throw new Exception("Subject not found.");

                subject.SubjectCode = code;
                subject.SubjectName = name;
                subject.DepartmentId = deptId;
                subject.Description = desc;
                subject.LecUnits = lec;
                subject.LabUnits = lab;
                subject.Units = totalUnits;

                context.Subjects.Update(subject);

                var oldPrereqs = context.SubjectPrerequisites.Where(sp => sp.SubjectId == subjectId);
                context.SubjectPrerequisites.RemoveRange(oldPrereqs);

                if (prerequisiteIds != null && prerequisiteIds.Count > 0)
                {
                    foreach (var reqId in prerequisiteIds)
                    {
                        context.SubjectPrerequisites.Add(new SubjectPrerequisite
                        {
                            SubjectId = subjectId,
                            RequiredSubjectId = reqId
                        });
                    }
                }

                await context.SaveChangesAsync();
            }
        }
    }
}