using SqlDefinitionStorageExample.EFCore;
using SqlDefinitionStorageExample.EFCore.Models;
using System;
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
            return Task.FromResult(SaveCore(model, resource));      
        }

        Task<SharedDataSourceModel> ISharedDataSourceStorage.RenameAsync(RenameResourceModel model)
        {
            return Task.FromResult(RenameCore(model));
        }

        Task<ResourceFileModel> IAssetsStorage.RenameAsync(RenameResourceModel model)
        {
            return Task.FromResult(RenameCore(model) as ResourceFileModel);
        }
        public new Task<ResourceFileModel> SaveAsync(SaveResourceModel model, byte[] resource)
        {
            return Task.FromResult(SaveCore(model, resource) as ResourceFileModel);
        }

        public new ResourceFileModel Overwrite(OverwriteResourceModel model, byte[] resource)
        {
            var entity = DbContext.Resources.FirstOrDefault(r => r.Uri == $"{Root}\\{model.Uri}") ?? throw new ResourceNotFoundException();
            entity.Bytes = resource;
            entity.ModifiedOn = DateTime.Now;
            entity.Description = SharedDataSourceDescriptionHelper.Read(resource);

            DbContext.SaveChanges();

            return entity.ToSharedDataSourceModel();
        }

        public new byte[] GetByUri(string uri)
        {
            if (!uri.Contains($"{Root}\\"))
            {
                uri = $"{Root}\\{uri}";
            }

            return base.GetByUri(uri);
        }


        SharedDataSourceModel SaveCore(SaveResourceModel model, byte[] resource)
        {
            model.ParentUri = string.IsNullOrEmpty(model.ParentUri) ? Root : $"{Root}\\{model.ParentUri}";
            model.ParentUri = model.ParentUri.Replace("/", "\\");

            if (model.ParentUri.EndsWith('\\'))
            {
                model.ParentUri = model.ParentUri.Remove(model.ParentUri.Length - 1);
            }

            var entity = DbContext.Resources.FirstOrDefault(r => r.Uri == FixParentUri(model.ParentUri) + model.Name);

            if (entity != null)
            {
                entity.Bytes = resource;
                entity.ModifiedOn = DateTime.Now;
                entity.Description = SharedDataSourceDescriptionHelper.Read(resource);
                DbContext.SaveChanges();

                return entity.ToSharedDataSourceModel();
            }

            var entityEntry = DbContext.Resources.Add(model.ToDbResourceModel(resource));
            DbContext.SaveChanges();

            return entityEntry.Entity.ToSharedDataSourceModel();
        }

        SharedDataSourceModel RenameCore(RenameResourceModel model)
        {
            string oldName = model.OldUri.Split("\\").Last();
            var resource = DbContext.Resources.FirstOrDefault(r => r.Uri == model.OldUri);
            if (resource != null)
            {
                resource.Name = model.Name;
                resource.Uri = resource.Uri.Replace(oldName, model.Name);
                resource.ModifiedOn = DateTime.Now;

                DbContext.SaveChanges();

                return resource.ToSharedDataSourceModel();
            }
            throw new ResourceNotFoundException();
        }
    }
}
