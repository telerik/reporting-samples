using SqlDefinitionStorageExample.EFCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telerik.WebReportDesigner.Services;
using Telerik.WebReportDesigner.Services.Models;
using System;

namespace SqlDefinitionStorageExample
{
    public class CustomDefinitionStorage : ResourceStorageBase, IDefinitionStorage, IAssetsStorage
    {
        public CustomDefinitionStorage(SqlDefinitionStorageContext dbContext, string rootFolder = "Reports" ) : base(dbContext, rootFolder)
        {
        }

        public new Task<ResourceFolderModel> CreateFolderAsync(CreateFolderModel model)
        {
            if(model.Name == Root)
            {
                throw new Exception($"Cannot name folder as base folder - '{Root}'");
            }

            if (string.IsNullOrEmpty(model.ParentUri))
            {
                model.ParentUri = Root;
            }

            return base.CreateFolderAsync(model);
        }

        public new Task DeleteAsync(string uri)
        {
            return base.DeleteAsync(this.PrepareResourceUri(uri));
        }

        public new Task<ResourceFolderModel> GetFolderAsync(string uri)
        {
            var newUri = string.IsNullOrEmpty(uri) 
                ? Root 
                : Root + "\\" + (uri ?? "");

            return base.GetFolderAsync(newUri);
        }

        public new Task<IEnumerable<ResourceModelBase>> GetFolderContentsAsync(string uri)
        {
            return base.GetFolderContentsAsync(PrepareResourceUri(uri));
        }

        public new Task<ResourceFileModel> SaveAsync(SaveResourceModel model, byte[] resource)
        {
            if (string.IsNullOrEmpty(model.ParentUri))
            {
                model.ParentUri = Root;
            }

            return base.SaveAsync(model, resource);
        }

        Task IAssetsStorage.DeleteFolderAsync(string uri)
        {
            return base.DeleteFolderAsync(PrepareResourceUri(uri));
        }

        Task<byte[]> IAssetsStorage.GetAsync(string resourceName)
        {
            if (!resourceName.Contains($"{Root}\\"))
            {
                resourceName = $"{Root}\\{resourceName}";
            }

            return base.GetAsync(PrepareResourceUri(resourceName));
        }

        Task<ResourceFileModel> IAssetsStorage.GetModelAsync(string uri)
        {
            return base.GetModelAsync(PrepareResourceUri(uri));
        }

        Task<ResourceFileModel> IAssetsStorage.RenameAsync(RenameResourceModel model)
        {
            return base.RenameAsync(model);
        }

        Task<ResourceFolderModel> IAssetsStorage.RenameFolderAsync(RenameFolderModel model)
        {
            return base.RenameFolderAsync(model);
        }

        string PrepareResourceUri(string resourceName)
        {
            if (string.IsNullOrEmpty(resourceName))
            {
                resourceName = Root;
            }

            resourceName = resourceName.Replace("/", "\\");
            return resourceName;
        }
    }
}
