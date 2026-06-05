using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PUPAcadPortal.Services
{
    public class ModuleDbService : IModuleDbService
    {
        private readonly Func<AppDbContext> _ctxFactory;

        public ModuleDbService(Func<AppDbContext> ctxFactory)
        {
            _ctxFactory = ctxFactory
                ?? throw new ArgumentNullException(nameof(ctxFactory));
        }

        //  READ

        public List<Module> GetModulesForOffering(string subjectOfferingId)
        {
            using var ctx = _ctxFactory();

            return ctx.Modules
                .Where(m => m.SubjectOfferingId == subjectOfferingId)
                .AsNoTracking()
                .OrderBy(m => m.UploadDate)
                .ToList();
        }

        //  CREATE

        public Module CreateModule(
            string subjectOfferingId,
            string title,
            string description,
            string? fileUrl = null)
        {
            if (string.IsNullOrWhiteSpace(subjectOfferingId))
                throw new ArgumentException("SubjectOfferingId is required.", nameof(subjectOfferingId));
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required.", nameof(title));

            using var ctx = _ctxFactory();

            var entity = new Module
            {
                ModuleId = GenerateModuleId(),
                SubjectOfferingId = subjectOfferingId,
                Title = title.Trim(),
                ModuleDescription = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
                FileUrl = fileUrl,
                UploadDate = DateTime.Now,
            };

            ctx.Modules.Add(entity);
            ctx.SaveChanges();

            return entity;
        }

        //  UPDATE

        public void UpdateModule(
            string moduleId,
            string title,
            string description,
            string? fileUrl)
        {
            if (string.IsNullOrWhiteSpace(moduleId))
                throw new ArgumentException("ModuleId is required.", nameof(moduleId));

            using var ctx = _ctxFactory();

            var entity = ctx.Modules.Find(moduleId)
                ?? throw new InvalidOperationException($"Module '{moduleId}' not found.");

            entity.Title = title?.Trim() ?? entity.Title;
            entity.ModuleDescription = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
            entity.FileUrl = fileUrl;

            ctx.SaveChanges();
        }

        //  DELETE

        public void DeleteModule(string moduleId)
        {
            if (string.IsNullOrWhiteSpace(moduleId)) return;

            using var ctx = _ctxFactory();

            var entity = ctx.Modules.Find(moduleId);
            if (entity == null) return;   // already gone – idempotent

            ctx.Modules.Remove(entity);
            ctx.SaveChanges();
        }


        private static string GenerateModuleId()
            => $"MOD-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}";
    }
}