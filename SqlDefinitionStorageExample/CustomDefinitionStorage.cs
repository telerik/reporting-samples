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
        
        public new Task<ResourceFileModel> SaveAsync(SaveResourceModel model, byte[] resource)
        {
            model.ParentUri = PrepareResourceUri(model.ParentUri);

            return base.SaveAsync(model, resource);
        }

        public new Task<ResourceFolderModel> GetFolderAsync(string uri) => base.GetFolderAsync(PrepareResourceUri(uri));
        public new Task DeleteAsync(string uri) => base.DeleteAsync(PrepareResourceUri(uri));

        public new Task<IEnumerable<ResourceModelBase>> GetFolderContentsAsync(string uri) => base.GetFolderContentsAsync(PrepareResourceUri(uri));

        Task IAssetsStorage.DeleteFolderAsync(string uri) => DeleteFolderAsync(PrepareResourceUri(uri));

        Task<byte[]> IAssetsStorage.GetAsync(string resourceName) => GetAsync(PrepareResourceUri(resourceName));

        Task<ResourceFileModel> IAssetsStorage.GetModelAsync(string uri) => GetModelAsync(PrepareResourceUri(uri));

        Task<ResourceFileModel> IAssetsStorage.RenameAsync(RenameResourceModel model) => RenameAsync(model);

        Task<ResourceFolderModel> IAssetsStorage.RenameFolderAsync(RenameFolderModel model) => RenameFolderAsync(model);
        
        protected string PrepareResourceUri(string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                return Root;
            }

            uri = uri.Replace("/", "\\");
            uri = uri.Contains($"{Root}\\") ? uri : $"{Root}\\{uri}";

            return uri;
        }
    }
}
