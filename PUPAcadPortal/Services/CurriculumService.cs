using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Data;
using PUPAcadPortal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PUPAcadPortal.Services
{
    public class CurriculumService
    {
        public async Task<List<CurriculumData>> GetCurriculumAsync(int revisionYear)
        {
            using (var db = new AppDbContext())
            {
                var curriculum = await db.Curricula
                    .Where(c => c.RevisionYear == revisionYear)
                    .Include(c => c.Subject)
                    .ToListAsync();

                List<CurriculumData> curriculumList = [];

                foreach (var c in curriculum)
                {
                    curriculumList.Add(new CurriculumData
                    {
                        SubjectId = c.SubjectId,
                        SubjectCode = c.Subject.SubjectCode,
                        SubjectName = c.Subject.SubjectName,
                        LabUnits = c.Subject.LabUnits,
                        LecUnits = c.Subject.LecUnits,
                        Units = c.Subject.Units,
                        Program = c.Program,
                        YearLevel = c.YearLevel,
                        Semester = c.SemesterIndex,
                        RevisionYear = c.RevisionYear,
                    });
                }

                return curriculumList;
            }
        }

        public async Task<List<Subject>> GetSubjects()
        {
            using (var context = new AppDbContext())
            {
                return await context.Subjects.ToListAsync();
            }
        }


        public async Task UpdateCurriculumAsync(List<CurriculumData> rawGridData, int revisionYear)
        {
            using (var db = new AppDbContext())
            {
                var cleanGridData = rawGridData
                    .Where(d => !string.IsNullOrWhiteSpace(d.SubjectCode) && !string.IsNullOrWhiteSpace(d.Program))
                    .GroupBy(d => new { d.Program, d.SubjectCode })
                    .Select(group => group.First())
                    .ToList();

                var existingCurricula = await db.Curricula
                    .Where(c => c.RevisionYear == revisionYear)
                    .ToListAsync();

                var allSubjects = await db.Subjects.ToListAsync();

                foreach (var data in cleanGridData)
                {
                    var subject = allSubjects.FirstOrDefault(s => s.SubjectCode == data.SubjectCode);
                    if (subject == null) continue;

                    var existingRecord = existingCurricula.FirstOrDefault(c =>
                        c.SubjectId == subject.SubjectId &&
                        c.Program == data.Program);

                    if (existingRecord != null)
                    {
                        existingRecord.YearLevel = data.YearLevel;
                        existingRecord.SemesterIndex = data.Semester;

                        existingCurricula.Remove(existingRecord);
                    }
                    else
                    {
                        db.Curricula.Add(new Curriculum
                        {
                            SubjectId = subject.SubjectId,
                            Program = data.Program,
                            YearLevel = data.YearLevel,
                            SemesterIndex = data.Semester,
                            RevisionYear = revisionYear
                        });
                    }
                }

                if (existingCurricula.Any())
                {
                    db.Curricula.RemoveRange(existingCurricula);
                }

                await db.SaveChangesAsync();
            }
        }

        public async Task<List<CurriculumData>> GetPreviousCurriculumAsync(int targetRevisionYear)
        {
            using (var db = new AppDbContext())
            {
                var previousYear = await db.Curricula
                    .Where(c => c.RevisionYear < targetRevisionYear)
                    .OrderByDescending(c => c.RevisionYear)
                    .Select(c => c.RevisionYear)
                    .FirstOrDefaultAsync();

                if (previousYear == 0)
                {
                    return new List<CurriculumData>();
                }

                return await GetCurriculumAsync(previousYear);
            }
        }
    }
}
