using Microsoft.EntityFrameworkCore;
using SqlDefinitionStorageExample.EFCore.Models;
using SqlDefinitionStorageExample.EFCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telerik.WebReportDesigner.Services.Models;
using Telerik.WebReportDesigner.Services;
using System.Linq;
using System;
using Microsoft.IdentityModel.Tokens;

namespace SqlDefinitionStorageExample
{
    public class ResourceStorageBase(SqlDefinitionStorageContext dbContext, string rootFolder)
    {
        protected SqlDefinitionStorageContext DbContext { get; } = dbContext;

        protected string Root { get; set; } = rootFolder;

        // IF PARENT URI DOES NOT END WITH
        public static string FixParentUri(string uri) => uri.EndsWith('\\') ? uri : uri + "\\";

        #region Base Implementations

        protected Task<ResourceFolderModel> CreateFolderAsync(CreateFolderModel model)
        {

            if (DbContext.ResourceFolders.Any(f => f.Uri == FixParentUri(model.ParentUri) + model.Name))
            {
                throw new ResourceFolderAlreadyExistsException();
            }

            var entityEntry = DbContext.ResourceFolders.Add(model.ToDbResourceFolderModel());

            if (!string.IsNullOrEmpty(model.ParentUri))
            {
                var parentFolder = DbContext.ResourceFolders.FirstOrDefault(f => f.Uri == model.ParentUri);

                if (parentFolder != null)
                {
                    parentFolder.HasSubFolders = true;
                }
            }

            DbContext.SaveChanges();

            return Task.FromResult(entityEntry.Entity.ToResourceFolderModel());
        }

        protected Task<IEnumerable<ResourceModelBase>> GetFolderContentsAsync(string uri)
        {
            var resources = DbContext.Resources.Where(r => r.ParentUri == uri)
                .Select(r => r.ToResourceFileModel()).AsEnumerable<ResourceModelBase>();
            var folders = DbContext.ResourceFolders.Where(f => f.ParentUri == uri)
                .Select(f => f.ToResourceFolderModel()).AsEnumerable<ResourceModelBase>();

            var result = folders.Union(resources);

            return Task.FromResult(result);
        }

        protected Task DeleteAsync(string uri)
        {
            var report = DbContext.Resources
                .FirstOrDefault(r => r.Uri == uri) ?? throw new ReportNotFoundException();
            DbContext.Resources.Remove(report);

            DbContext.SaveChanges();
            return Task.CompletedTask;
        }

        protected Task DeleteFolderAsync(string uri)
        {
            try
            {
                var folderForDeletion = DbContext.ResourceFolders.FirstOrDefault(f => f.Uri == uri);
                var parentUri = folderForDeletion.ParentUri;
                if (folderForDeletion != null)
                {
                    DeleteResourceFolder(folderForDeletion);
                    DbContext.SaveChanges();
                    var subFoldersCount = DbContext.ResourceFolders.Count(f => f.ParentUri == parentUri);
                    if (subFoldersCount > 0)
                    {
                        var parentFolderName = parentUri.Split("\\").Last();
                        var parentFolder = DbContext.ResourceFolders.FirstOrDefault(f => f.Name == parentFolderName);
                        if (parentFolder != null) parentFolder.HasSubFolders = false;
                        DbContext.SaveChanges();
                    }
                    return Task.CompletedTask;
                }
                return Task.FromException(new ResourceFolderNotFoundException());
            }
            catch (Exception)
            {
                throw;
            }

        }

        protected Task<ResourceFileModel> SaveAsync(SaveResourceModel model, byte[] resource)
        {
            var entity = DbContext.Resources.FirstOrDefault(r => r.Uri == FixParentUri(model.ParentUri) + model.Name);

            if (entity != null)
            {
                entity.Bytes = resource;
                entity.ModifiedOn = DateTime.Now;
                DbContext.SaveChanges();
                return Task.FromResult(entity.ToResourceFileModel());
            }

            var entityEntry = DbContext.Resources.Add(model.ToDbResourceModel(resource));
            DbContext.SaveChanges();

            return Task.FromResult(entityEntry.Entity.ToResourceFileModel());
        }

        protected Task<byte[]> GetAsync(string resourceName)
        {
            var resourceBytes = GetDbResourceModel(resourceName)?.Bytes;
            return resourceBytes == null ? throw new ResourceNotFoundException() : Task.FromResult(resourceBytes);
        }

        protected Task<ResourceFileModel> GetModelAsync(string uri)
        {
            return Task.FromResult(GetDbResourceModel(uri)
                         .ToResourceFileModel());
        }

        protected Task<ResourceFileModel> RenameAsync(RenameResourceModel model)
        {
            if (!model.OldUri.Contains($"{Root}\\"))
            {
                model.OldUri = $"{Root}\\{model.OldUri}";
            }

            string oldName = model.OldUri.Split("\\").Last();
            var report = DbContext.Resources.FirstOrDefault(r => r.Uri == model.OldUri);
            if (report != null)
            {
                report.Name = model.Name;
                report.Uri = report.Uri.Replace(oldName, model.Name);
                report.ModifiedOn = DateTime.Now;

                DbContext.SaveChanges();

                return Task.FromResult(report.ToResourceFileModel());
            }
            throw new ResourceNotFoundException();
        }

        protected async Task<ResourceFolderModel> RenameFolderAsync(RenameFolderModel model)
        {
            var folder = DbContext.ResourceFolders.FirstOrDefault(r => r.Uri == model.OldUri);
            if (folder != null)
            {
                await RenameFolderAndSubFolders(folder, model);
                folder.ModifiedOn = DateTime.Now;
                DbContext.SaveChanges();

                return folder.ToResourceFolderModel();
            }
            throw new ResourceFolderNotFoundException();
        }

        #endregion

        #region Helper Methods
        protected void DeleteResourceFolder(ResourceFolder folder)
        {
            DeleteResourcesInFolder(folder);

            var subfolders = DbContext.ResourceFolders.Where(f => f.ParentUri == folder.Uri).ToList();
            DbContext.ResourceFolders.Remove(folder);

            if (subfolders.Count == 0)
            {
                return;
            }

            foreach (var subfolder in subfolders)
            {
                DeleteResourceFolder(subfolder);
            }

        }

        protected void DeleteResourcesInFolder(ResourceFolder folder)
        {
            var reports = DbContext.Resources.Where(r => r.ParentUri == folder.Uri).ToList();
            if (reports.Count > 0)
            {
                reports.ForEach(r =>
                {
                    DbContext.Resources.Remove(r);
                    DbContext.SaveChanges();
                });
            }
        }

        protected async Task UpdateResourceParentUriAfterFolderRename(string oldName, RenameFolderModel model)
        {
            await DbContext.Resources.ForEachAsync(r =>
            {
                if (r.ParentUri.Contains(model.OldUri))
                {
                    r.Uri = r.Uri.Replace(oldName, model.Name);
                    r.ParentUri = r.ParentUri.Replace(oldName, model.Name);
                }
            });
        }

        protected async Task RenameFolderAndSubFolders(ResourceFolder folder, RenameFolderModel model)
        {
            string oldName = model.OldUri.Split("\\").Last();

            await UpdateResourceParentUriAfterFolderRename(oldName, model);

            folder.Name = model.Name;
            folder.Uri = folder.Uri.Replace(oldName, model.Name);

            if (!folder.HasSubFolders)
            {
                return;
            }

            await DbContext.ResourceFolders
                .Where(f => f.ParentUri.Contains(model.OldUri))
                .ForEachAsync(f =>
                {
                    f.Uri = f.Uri.Replace(oldName, model.Name);
                    f.ParentUri = f.ParentUri.Replace(oldName, model.Name);
                });
        }

        protected EFCore.Models.Resource GetDbResourceModel(string uri)
        {
            if (DbContext.IsDisposed) {
                var optionsBuilder = new DbContextOptionsBuilder<SqlDefinitionStorageContext>();
                using SqlDefinitionStorageContext dbContext = new(optionsBuilder.Options);
                return dbContext.Resources.FirstOrDefault(r => r.Uri == uri) ?? null;
            }

            return DbContext.Resources.FirstOrDefault(r => r.Uri == uri) ?? null;
        }

        protected Task<ResourceFolderModel> GetFolderAsync(string uri)
        {
            var folder = DbContext.ResourceFolders.FirstOrDefault(f => f.Uri == uri);
            if (folder == null)
            {
                if (uri.IsNullOrEmpty())
                {
                    CreateFolderModel model = new() { ParentUri = uri, Name = Root };
                    return this.CreateFolderAsync(model);
                }
            }
            return Task.FromResult(folder.ToResourceFolderModel());
        }

        #endregion

    }
}
