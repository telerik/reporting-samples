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
    public class CustomSharedDataSourceStorage(SqlDefinitionStorageContext context) : CustomDefinitionStorage(context, "Shared Data Sources"), ISharedDataSourceStorage
    {
        public bool FolderExists(string uri)
        {
            throw new System.NotImplementedException();
        }

        public bool FolderHasContents(string uri)
        {
            throw new System.NotImplementedException();
        }

        public bool FolderNameExists(string folderName)
        {
            return false;
        }

        public IEnumerable<ResourceFileModel> GetAllByExtension(string[] extensions)
        {
            throw new System.NotImplementedException();
        }

        public byte[] GetByUri(string uri)
        {
            throw new System.NotImplementedException();
        }

        public ResourceFileDataModel GetFile(string resourceUri)
        {
            throw new System.NotImplementedException();
        }

        public ResourceFolderModel GetFolderByName(string folderName)
        {
            throw new System.NotImplementedException();
        }

        public ResourceFileModel GetModelByName(string resourceName)
        {
            throw new System.NotImplementedException();
        }

        public ResourceFileModel Move(MoveResourceModel model)
        {
            throw new System.NotImplementedException();
        }

        public ResourceFolderModel MoveFolder(MoveFolderModel model)
        {
            throw new System.NotImplementedException();
        }

        public ResourceFileModel Overwrite(OverwriteResourceModel model, byte[] resource)
        {
            throw new System.NotImplementedException();
        }

        public bool ResourceExists(string uri)
        {
            return DbContext.SharedDataSources.Any(d => d.Uri == uri);
        }

        public bool ResourceNameExists(string resourceName)
        {
            throw new System.NotImplementedException();
        }

        public string Save(string resourceName, byte[] resource)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ResourceModelBase> Search(SearchResourcesModel model)
        {
            throw new System.NotImplementedException();
        }

        Task<SharedDataSourceModel> ISharedDataSourceStorage.GetModelAsync(string uri)
        {
            return Task.FromResult(
                    this.GetDbSharedDataSourceModel(uri)
                     .ToSharedDataSourceModel());
        }

        Task<SharedDataSourceModel> ISharedDataSourceStorage.RenameAsync(RenameResourceModel model)
        {
            throw new System.NotImplementedException();
        }

        Task<SharedDataSourceModel> ISharedDataSourceStorage.SaveAsync(SaveResourceModel model, byte[] resource)
        {
            var entity = DbContext.SharedDataSources.FirstOrDefault(r => r.Uri == model.ParentUri + model.Name);

            if (entity != null)
            {
                entity.Bytes = resource;
                entity.ModifiedOn = DateTime.Now;
                DbContext.SaveChanges();
                return Task.FromResult(entity.ToSharedDataSourceModel());
            }

            var entityEntry = DbContext.SharedDataSources.Add(model.ToDbSharedDataSource(resource));
            DbContext.SaveChanges();

            return Task.FromResult(entityEntry.Entity.ToSharedDataSourceModel());
        }

        EFCore.Models.SharedDataSource GetDbSharedDataSourceModel(string uri)
        {
            if (DbContext.SharedDataSources.Any())
            {
                return DbContext.SharedDataSources.FirstOrDefault(r => r.Uri == uri);
            }

            return null;
        }
    }
}
