using SqlDefinitionStorageExample.EFCore;
using System.Threading.Tasks;
using Telerik.WebReportDesigner.Services;
using Telerik.WebReportDesigner.Services.Models;

namespace SqlDefinitionStorageExample
{
    public class CustomTemplateDefinitionStorage(SqlDefinitionStorageContext dbContext, string rootFolder = "Templates") : CustomDefinitionStorage(dbContext, rootFolder), IDefinitionStorage
    {
        public new Task<byte[]> GetAsync(string resourceName)
        {
            return base.GetAsync<ReportNotFoundException>(resourceName);
        }
        public new Task<ResourceFileModel> RenameAsync(RenameResourceModel model)
        {
            return base.RenameAsync<InvalidReportNameException>(model);
        }

        public new Task<ResourceFileModel> SaveAsync(SaveResourceModel model, byte[] resource)
        {
            return base.SaveAsync<ReportNotFoundException>(model, resource);
        }
    }
}
