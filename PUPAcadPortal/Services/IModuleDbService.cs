using PUPAcadPortal.Models;
using System.Collections.Generic;

namespace PUPAcadPortal.Services
{
    public interface IModuleDbService
    {
        //  Read 
        List<Module> GetModulesForOffering(string subjectOfferingId);

        //── Create 
        Module CreateModule(string subjectOfferingId, string title, string description, string? fileUrl = null);

        //  Update 
        void UpdateModule(string moduleId, string title, string description, string? fileUrl);

        //  Delete 
        void DeleteModule(string moduleId);
    }
}