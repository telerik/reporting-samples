using SqlDefinitionStorageExample.EFCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telerik.WebReportDesigner.Services;
using Telerik.WebReportDesigner.Services.Models;
using SqlDefinitionStorageExample.EFCore.Models;
using System;
using Microsoft.IdentityModel.Tokens;

namespace SqlDefinitionStorageExample
{
    public class CustomDefinitionStorage : ResourceStorageBase, IDefinitionStorage, IAssetsStorage
    {
        public CustomDefinitionStorage(SqlDefinitionStorageContext dbContext, string rootFolder = "Reports" ) : base(dbContext, rootFolder)
        {
        }

        public Task<ResourceFolderModel> CreateFolderAsync(CreateFolderModel model)
        {
            if (string.IsNullOrEmpty(model.ParentUri))
            {
                model.ParentUri = Root;
            }

            return base.CreateFolderAsyncBase(model);
        }

        public Task DeleteAsync(string uri)
        {
            var report = this.Files
                .FirstOrDefault(r => r.Uri == this.PrepareResourceUri(uri)) ?? throw new ReportNotFoundException();
            this.Files.Remove(report);
            DbContext.SaveChanges();
            return Task.CompletedTask;
        }

        public Task DeleteFolderAsync(string uri)
        {
            try
            {
                var folderForDeletion = this.Folders.FirstOrDefault(f => f.Uri == uri);
                var parentUri = folderForDeletion.ParentUri;
                if (folderForDeletion != null)
                {
                    DeleteFolder(folderForDeletion);
                    DbContext.SaveChanges();
                    var subFoldersCount = this.Folders.Count(f => f.ParentUri == parentUri);
                    if (subFoldersCount > 0)
                    {
                        var parentFolderName = parentUri.Split("\\").Last();
                        var parentFolder = this.Folders.FirstOrDefault(f => f.Name == parentFolderName);
                            if(parentFolder != null) parentFolder.HasSubFolders = false;
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

        public Task<byte[]> GetAsync(string resourceName)
        {
            var reportBytes = this.GetDbResourceModel(this.PrepareResourceUri(resourceName))?.Bytes;
            return reportBytes == null ? throw new ReportNotFoundException() : Task.FromResult(reportBytes);
        }

        public Task<ResourceFolderModel> GetFolderAsync(string uri)
        {
            var folder = this.Folders.FirstOrDefault(f => f.Uri == Root + "\\" + (uri ?? ""));
            if(folder == null)
            {
                if (uri.IsNullOrEmpty())
                {
                    CreateFolderModel model = new() { ParentUri = uri, Name = Root };
                    return this.CreateFolderAsync(model);
                }
            }
            return Task.FromResult(folder.ToResourceFolderModel());
        }

        public Task<IEnumerable<ResourceModelBase>> GetFolderContentsAsync(string uri)
        {
            uri ??= string.Empty;

            var reps = this.Files.Where(r => r.ParentUri == uri)
                .Select(r => r.ToResourceFileModel()).AsEnumerable<ResourceModelBase>();
            var folders = this.Folders.Where(f => f.ParentUri == uri)
                .Select(f => f.ToResourceFolderModel()).AsEnumerable<ResourceModelBase>();

            var result = folders.Union(reps);

            return Task.FromResult<IEnumerable<ResourceModelBase>>(result);
        }

        public Task<ResourceFileModel> GetModelAsync(string uri)
        {
            return Task.FromResult(
                  this.GetDbResourceModel(uri)
                         .ToResourceFileModel());
        }

        public Task<ResourceFileModel> RenameAsync(RenameResourceModel model)
        {
            string oldName = model.OldUri.Split("\\").Last();
            var report = this.Files.FirstOrDefault(r => r.Uri == this.PrepareResourceUri(model.OldUri));
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

        public async Task<ResourceFolderModel> RenameFolderAsync(RenameFolderModel model)
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

        public Task<ResourceFileModel> SaveAsync(SaveResourceModel model, byte[] resource)
        {
            var entity = this.Files.FirstOrDefault(r => r.Uri == model.ParentUri + model.Name);

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

        string PrepareResourceUri(string resourceName)
        {
            resourceName = (resourceName ?? string.Empty);
            resourceName = resourceName.Replace("/", "\\");

            return resourceName;
        }

        EFCore.Models.Resource GetDbResourceModel(string uri)
        {
            if (this.Files.Any())
            {
                return this.Files.FirstOrDefault(r => r.Uri == uri);
            }

            return null;
        }

        void DeleteReportsInFolder(ResourceFolder folder)
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

        void DeleteFolder(ResourceFolder folder)
        {
            DeleteReportsInFolder(folder);

            var subfolders = this.Folders.Where(f => f.ParentUri == folder.Uri).ToList();
            this.Folders.Remove(folder);

            if (subfolders.Count == 0)
            {
                return;
            }

            foreach (var subfolder in subfolders)
            {
                DeleteFolder(subfolder);
            }

        }

        async Task UpdateReportParentUriAfterFolderRename(string oldName, RenameFolderModel model)
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

        async Task RenameFolderAndSubFolders(ResourceFolder folder, RenameFolderModel model)
        {
            string oldName = model.OldUri.Split("\\").Last();

            await UpdateReportParentUriAfterFolderRename(oldName, model);

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
    }

    public class ResourceStorageBase(SqlDefinitionStorageContext dbContext, string rootFolder)
    {
        protected SqlDefinitionStorageContext DbContext { get; } = dbContext;

        protected string Root { get; set; } = rootFolder;

        protected DbSet<ResourceFolder> Folders { get; set; } = dbContext.ResourceFolders;

        protected DbSet<Resource> Files { get; set; } = dbContext.Resources;

        public Task<ResourceFolderModel> CreateFolderAsyncBase(CreateFolderModel model)
        {

            if (this.Folders.Any(f => f.Uri == model.ParentUri + "\\" + model.Name))
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
    }
}
