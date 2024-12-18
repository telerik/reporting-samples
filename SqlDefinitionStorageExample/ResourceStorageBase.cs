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

        protected DbSet<ResourceFolder> Folders { get; set; } = dbContext.ResourceFolders;

        protected DbSet<Resource> Files { get; set; } = dbContext.Resources;


        // IF PARENT URI DOES NOT END WITH
        static string FixParentUri(string uri) => uri.EndsWith("\\") ? uri : uri + "\\";

        #region Base Implementations

        protected Task<ResourceFolderModel> CreateFolderAsync(CreateFolderModel model)
        {

            if (this.Folders.Any(f => f.Uri == FixParentUri(model.ParentUri) + model.Name))
            {
                throw new ResourceFolderAlreadyExistsException();
            }

            var entityEntry = this.Folders.Add(model.ToDbResourceFolderModel());

            if (!string.IsNullOrEmpty(model.ParentUri))
            {
                var parentFolder = this.Folders.FirstOrDefault(f => f.Uri == model.ParentUri);

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
            var resources = this.Files.Where(r => r.ParentUri == uri)
                .Select(r => r.ToResourceFileModel()).AsEnumerable<ResourceModelBase>();
            var folders = this.Folders.Where(f => f.ParentUri == uri)
                .Select(f => f.ToResourceFolderModel()).AsEnumerable<ResourceModelBase>();

            var result = folders.Union(resources);

            return Task.FromResult(result);
        }

        protected Task DeleteAsync(string uri)
        {
            var report = this.Files
                .FirstOrDefault(r => r.Uri == uri) ?? throw new ReportNotFoundException();
            this.Files.Remove(report);

            DbContext.SaveChanges();
            return Task.CompletedTask;
        }

        protected Task DeleteFolderAsync(string uri)
        {
            try
            {
                var folderForDeletion = this.Folders.FirstOrDefault(f => f.Uri == uri);
                var parentUri = folderForDeletion.ParentUri;
                if (folderForDeletion != null)
                {
                    DeleteResourceFolder(folderForDeletion);
                    DbContext.SaveChanges();
                    var subFoldersCount = this.Folders.Count(f => f.ParentUri == parentUri);
                    if (subFoldersCount > 0)
                    {
                        var parentFolderName = parentUri.Split("\\").Last();
                        var parentFolder = this.Folders.FirstOrDefault(f => f.Name == parentFolderName);
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
            var entity = this.Files.FirstOrDefault(r => r.Uri == FixParentUri(model.ParentUri) + model.Name);

            if (entity != null)
            {
                entity.Bytes = resource;
                entity.ModifiedOn = DateTime.Now;
                DbContext.SaveChanges();
                return Task.FromResult(entity.ToResourceFileModel());
            }

            var entityEntry = this.Files.Add(model.ToDbResourceModel(resource));
            DbContext.SaveChanges();

            return Task.FromResult(entityEntry.Entity.ToResourceFileModel());
        }

        protected Task<byte[]> GetAsync(string resourceName)
        {
            var resourceBytes = this.GetDbResourceModel(resourceName)?.Bytes;
            return resourceBytes == null ? throw new ResourceNotFoundException() : Task.FromResult(resourceBytes);
        }

        protected Task<ResourceFileModel> GetModelAsync(string uri)
        {
            return Task.FromResult(GetDbResourceModel(uri)
                         .ToResourceFileModel());
        }

        protected Task<ResourceFileModel> RenameAsync(RenameResourceModel model)
        {
            string oldName = model.OldUri.Split("\\").Last();
            var report = this.Files.FirstOrDefault(r => r.Uri == model.OldUri);
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
            var folder = this.Folders.FirstOrDefault(r => r.Uri == model.OldUri);
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

            var subfolders = this.Folders.Where(f => f.ParentUri == folder.Uri).ToList();
            this.Folders.Remove(folder);

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
            var reports = this.Files.Where(r => r.ParentUri == folder.Uri).ToList();
            if (reports.Count > 0)
            {
                reports.ForEach(r =>
                {
                    this.Files.Remove(r);
                    DbContext.SaveChanges();
                });
            }
        }

        protected async Task UpdateResourceParentUriAfterFolderRename(string oldName, RenameFolderModel model)
        {
            await this.Files.ForEachAsync(r =>
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

            await this.Folders
                .Where(f => f.ParentUri.Contains(model.OldUri))
                .ForEachAsync(f =>
                {
                    f.Uri = f.Uri.Replace(oldName, model.Name);
                    f.ParentUri = f.ParentUri.Replace(oldName, model.Name);
                });
        }

        protected EFCore.Models.Resource GetDbResourceModel(string uri)
        {
            if (this.Files.Any())
            {
                return this.Files.FirstOrDefault(r => r.Uri == uri);
            }

            return null;
        }

        protected Task<ResourceFolderModel> GetFolderAsync(string uri)
        {
            var folder = this.Folders.FirstOrDefault(f => f.Uri == uri);
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
