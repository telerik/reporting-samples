using SqlDefinitionStorageExample.EFCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telerik.WebReportDesigner.Services;
using Telerik.WebReportDesigner.Services.Models;
using SqlDefinitionStorageExample.EFCore.Models;
using System;

namespace SqlDefinitionStorageExample
{
    public class CustomDefinitionStorage : IDefinitionStorage
    {
        private SqlDefinitionStorageContext _dbContext { get; }

        public CustomDefinitionStorage(SqlDefinitionStorageContext context)
        {
            this._dbContext = context;
        }

        public Task<ResourceFolderModel> CreateFolderAsync(CreateFolderModel model)
        {

            if(this._dbContext.ReportFolders.Any(f => f.Uri == model.ParentUri + "\\" + model.Name)) {
                throw new ResourceFolderAlreadyExistsException();
            }

            var entityEntry = this._dbContext.ReportFolders.Add(model.ToDbReportFolderModel());

            if (!string.IsNullOrEmpty(model.ParentUri))
            {
                var parentFolder = this._dbContext.ReportFolders.FirstOrDefault(f => f.Uri == model.ParentUri);

                if (parentFolder != null)
                {
                    parentFolder.HasSubFolders = true;
                }
            }

            this._dbContext.SaveChanges();

            return Task.FromResult(entityEntry.Entity.ToResourceFolderModel());
        }

        public Task DeleteAsync(string uri)
        {
            var report = this._dbContext.Reports
                .FirstOrDefault(r => r.Uri == this.PrepareResourceUri(uri)) ?? throw new ReportNotFoundException();
            this._dbContext.Reports.Remove(report);
            this._dbContext.SaveChanges();
            return Task.CompletedTask;
        }

        public Task DeleteFolderAsync(string uri)
        {
            try
            {
                var folderForDeletion = this._dbContext.ReportFolders.FirstOrDefault(f => f.Uri == uri);
                var parentUri = folderForDeletion.ParentUri;
                if (folderForDeletion != null)
                {
                    DeleteFolder(folderForDeletion);
                    this._dbContext.SaveChanges();
                    var subFoldersCount = this._dbContext.ReportFolders.Count(f => f.ParentUri == parentUri);
                    if (subFoldersCount > 0)
                    {
                        var parentFolderName = parentUri.Split("\\").Last();
                        var parentFolder = this._dbContext.ReportFolders.FirstOrDefault(f => f.Name == parentFolderName);
                            if(parentFolder != null) parentFolder.HasSubFolders = false;
                        this._dbContext.SaveChanges();
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
            if (!_dbContext.Reports.Any()) AddSampleReportToDatabase();

            var reportBytes = this.GetDbReportModel(this.PrepareResourceUri(resourceName))?.Bytes;
            return reportBytes == null ? throw new ReportNotFoundException() : Task.FromResult(reportBytes);
        }

        public Task<ResourceFolderModel> GetFolderAsync(string uri)
        {
            // it is not necessary to implement this one
            var folder = this._dbContext.ReportFolders.FirstOrDefault(f => f.Uri == uri);
            return folder == null 
                ? throw new ResourceFolderNotFoundException() 
                : Task.FromResult(folder.ToResourceFolderModel());
        }

        public Task<IEnumerable<ResourceModelBase>> GetFolderContentsAsync(string uri)
        {
            uri = (uri ?? string.Empty);

            var reps = this._dbContext.Reports.Where(r => r.ParentUri == uri)
                .Select(r => r.ToResourceFileModel()).AsEnumerable<ResourceModelBase>();
            var folders = this._dbContext.ReportFolders.Where(f => f.ParentUri == uri)
                .Select(f => f.ToResourceFolderModel()).AsEnumerable<ResourceModelBase>();

            var result = folders.Union(reps);

            return Task.FromResult<IEnumerable<ResourceModelBase>>(result);
        }

        public Task<ResourceFileModel> GetModelAsync(string uri)
        {
            return Task.FromResult(
                  this.GetDbReportModel(uri)
                         .ToResourceFileModel());
        }

        public Task<ResourceFileModel> RenameAsync(RenameResourceModel model)
        {
            string oldName = model.OldUri.Split("\\").Last();
            var report = this._dbContext.Reports.FirstOrDefault(r => r.Uri == this.PrepareResourceUri(model.OldUri));
            if (report != null)
            {
                report.Name = model.Name;
                report.Uri = report.Uri.Replace(oldName, model.Name);
                report.ModifiedOn = DateTime.Now;

                this._dbContext.SaveChanges();

                return Task.FromResult(report.ToResourceFileModel());
            }
            throw new ResourceNotFoundException();
        }

        public async Task<ResourceFolderModel> RenameFolderAsync(RenameFolderModel model)
        {
            var folder = this._dbContext.ReportFolders.FirstOrDefault(r => r.Uri == model.OldUri);
            if (folder != null)
            {
                await RenameFolderAndSubFolders(folder, model);
                folder.ModifiedOn = DateTime.Now;
                this._dbContext.SaveChanges();

                return folder.ToResourceFolderModel();
            }
            throw new ResourceFolderNotFoundException();
        }

        public Task<ResourceFileModel> SaveAsync(SaveResourceModel model, byte[] resource)
        {
            var entity = this._dbContext.Reports.FirstOrDefault(r => r.Uri == model.ParentUri + model.Name);

            if (entity != null)
            {
                entity.Bytes = resource;
                entity.ModifiedOn = DateTime.Now;
                this._dbContext.SaveChanges();
                return Task.FromResult(entity.ToResourceFileModel());
            }

            var entityEntry = this._dbContext.Reports.Add(model.ToDbReportModel(resource));
            this._dbContext.SaveChanges();

            return Task.FromResult(entityEntry.Entity.ToResourceFileModel());
        }

        string PrepareResourceUri(string resourceName)
        {
            resourceName = (resourceName ?? string.Empty);
            resourceName = resourceName.Replace("/", "\\");

            return resourceName;
        }

        EFCore.Models.Report GetDbReportModel(string uri)
        {
            if (_dbContext.Reports.Any())
            {
                return this._dbContext.Reports.FirstOrDefault(r => r.Uri == uri);
            }

            return null;
        }

        void DeleteReportsInFolder(ReportFolder folder)
        {
            var reports = this._dbContext.Reports.Where(r => r.ParentUri == folder.Uri).ToList();
            if (reports.Count > 0)
            {
                reports.ForEach(r =>
                {
                    this._dbContext.Reports.Remove(r);
                    this._dbContext.SaveChanges();
                });
            }
        }

        void DeleteFolder(ReportFolder folder)
        {
            DeleteReportsInFolder(folder);

            var subfolders = this._dbContext.ReportFolders.Where(f => f.ParentUri == folder.Uri).ToList();
            this._dbContext.ReportFolders.Remove(folder);

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
            await this._dbContext.Reports.ForEachAsync(r =>
            {
                if (r.ParentUri.Contains(model.OldUri))
                {
                    r.Uri = r.Uri.Replace(oldName, model.Name);
                    r.ParentUri = r.ParentUri.Replace(oldName, model.Name);
                }
            });
        }

        async Task RenameFolderAndSubFolders(ReportFolder folder, RenameFolderModel model)
        {
            string oldName = model.OldUri.Split("\\").Last();

            await UpdateReportParentUriAfterFolderRename(oldName, model);

            folder.Name = model.Name;
            folder.Uri = folder.Uri.Replace(oldName, model.Name);

            if (!folder.HasSubFolders)
            {
                return;
            }

            await this._dbContext.ReportFolders
                .Where(f => f.ParentUri.Contains(model.OldUri))
                .ForEachAsync(f =>
                {
                    f.Uri = f.Uri.Replace(oldName, model.Name);
                    f.ParentUri = f.ParentUri.Replace(oldName, model.Name);
                });
        }

        void AddSampleReportToDatabase()
        {
            var saveResourceModel = new SaveResourceModel() 
            { 
                Name = "SampleReport.trdp", 
                ParentUri = string.Empty 
            };

            var reportBytes = System.IO.File.ReadAllBytes("SampleReport.trdp");
            var entity = saveResourceModel.ToDbReportModel(reportBytes);

            this._dbContext.Reports.Add(entity);
            this._dbContext.SaveChanges();
        }

    }
}
