using SqlDefinitionStorageExample.EFCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telerik.WebReportDesigner.Services;
using Telerik.WebReportDesigner.Services.Models;
using System;

namespace SqlDefinitionStorageExample
{
    public class CustomDefinitionStorage(SqlDefinitionStorageContext dbContext, string rootFolder = "Reports") : ResourceStorageBase(dbContext, rootFolder), IDefinitionStorage, IAssetsStorage
    {
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

        protected virtual Task<ResourceFileModel> SaveAsync<TDefinitionNotFoundException>(SaveResourceModel model, byte[] resource) where TDefinitionNotFoundException : Exception
        {
            return Task.FromResult(WrapException<ResourceFileModel, TDefinitionNotFoundException, ResourceNotFoundException>(() => SaveAsync(model, resource).ToResult()));
        }
        protected virtual Task<ResourceFileModel> RenameAsync<TInvalidDefinitionNameException>(RenameResourceModel model) where TInvalidDefinitionNameException : Exception
        {
            return Task.FromResult(WrapException<ResourceFileModel, TInvalidDefinitionNameException, InvalidResourceNameException>(() => RenameAsync(model).ToResult()));
        }

        protected virtual Task<byte[]> GetAsync<TDefinitionNotFoundException>(string resourceName) where TDefinitionNotFoundException : Exception
        {
            return Task.FromResult(WrapException<byte[], TDefinitionNotFoundException, ResourceNotFoundException>(() => GetAsync(PrepareResourceUri(resourceName)).ToResult()));
        }


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

        protected TResult WrapException<TResult, TDefinitionException, TResourceException>(Func<TResult> callback) where TDefinitionException : Exception where TResourceException : Exception
        {
            try
            {
                return callback();
            }
            catch (TResourceException ex)
            {
                throw (TDefinitionException)Activator.CreateInstance(typeof(TDefinitionException), ex.Message, ex);
            }
        }
    }
}
