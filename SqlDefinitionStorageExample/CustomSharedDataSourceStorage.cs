using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.EntityFrameworkCore;
using SqlDefinitionStorageExample.EFCore;
using SqlDefinitionStorageExample.EFCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.WebReportDesigner.Services;
using Telerik.WebReportDesigner.Services.Models;

namespace SqlDefinitionStorageExample
{
    public class CustomSharedDataSourceStorage(SqlDefinitionStorageContext context) : CustomResourceStorage(context, "Shared Data Sources"), ISharedDataSourceStorage
    {
        Task<SharedDataSourceModel> ISharedDataSourceStorage.GetModelAsync(string uri)
        {
            return Task.FromResult(GetDbResourceModel(uri).ToResourceFileModel() as SharedDataSourceModel);
        }

        Task<SharedDataSourceModel> ISharedDataSourceStorage.SaveAsync(SaveResourceModel model, byte[] resource)
        {
            return Task.FromResult(SaveCore(model, resource) as SharedDataSourceModel);      
        }

        Task<SharedDataSourceModel> ISharedDataSourceStorage.RenameAsync(RenameResourceModel model)
        {
            throw new NotImplementedException();
        }

        public new Task<ResourceFileModel> SaveAsync(SaveResourceModel model, byte[] resource)
        {
            return Task.FromResult(SaveCore(model, resource));
        }

        public new ResourceFileModel Overwrite(OverwriteResourceModel model, byte[] resource)
        {
            throw new System.NotImplementedException();
        }


        ResourceFileModel SaveCore(SaveResourceModel model, byte[] resource)
        {
            var entity = DbContext.Resources.FirstOrDefault(r => r.Uri == model.ParentUri + "\\" + model.Name);

            if (entity != null)
            {
                entity.Bytes = resource;
                entity.ModifiedOn = DateTime.Now;
                entity.Description = SharedDataSourceDescriptionHelper.Read(resource);
                return entity.ToSharedDataSourceModel();
            }

            if (string.IsNullOrEmpty(model.ParentUri))
            {
                model.ParentUri = Root;
            }
            var entityEntry = DbContext.Resources.Add(model.ToDbResourceModel(resource));
            DbContext.SaveChanges();

            return entityEntry.Entity.ToSharedDataSourceModel();
        }
    }
}
