using System;
using Telerik.WebReportDesigner.Services.Models;

namespace SqlDefinitionStorageExample.EFCore.Models
{
    public static class DbModelExtensions
    {
        public static ResourceFileModel ToResourceFileModel(this EFCore.Models.Report dbReportModel)
        {
            if (dbReportModel == null)
            {
                return null;
            }

            return new ResourceFileModel()
            {
                FileName = dbReportModel.Name,
                ParentPath = dbReportModel.ParentUri,
                Uri = dbReportModel.Uri,
                Size = dbReportModel.Bytes.Length / 1024f,
                CreatedOn = dbReportModel.CreatedOn,
                ModifiedOn = dbReportModel.ModifiedOn,
            };
        }

        public static ResourceFolderModel ToResourceFolderModel(this EFCore.Models.ReportFolder dbReportFolderModel)
        {
            if (dbReportFolderModel == null)
            {
                return null;
            }

            return new ResourceFolderModel()
            {
                Name = dbReportFolderModel.Name,
                ParentUri = dbReportFolderModel.ParentUri,
                HasSubFolders = dbReportFolderModel.HasSubFolders,
                CreatedOn = dbReportFolderModel.CreatedOn,
                ModifiedOn = dbReportFolderModel.ModifiedOn,
                Uri = dbReportFolderModel.Uri
            };
        }

        public static EFCore.Models.ReportFolder ToDbReportFolderModel(this CreateFolderModel createFolderModel)
        {
            return new EFCore.Models.ReportFolder() {
                Name = createFolderModel.Name,
                ParentUri = createFolderModel.ParentUri,
                Uri = (string.IsNullOrEmpty(createFolderModel.ParentUri) 
                    ? createFolderModel.ParentUri 
                    : createFolderModel.ParentUri + "\\") + createFolderModel.Name,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };
        }

        public static EFCore.Models.Report ToDbReportModel(this SaveResourceModel saveResourceModel, byte[] data)
        {
            return new Models.Report()
            {
                Name = saveResourceModel.Name,
                Bytes = data,
                ParentUri = saveResourceModel.ParentUri,
                Uri = (string.IsNullOrEmpty(saveResourceModel.ParentUri) 
                    ? saveResourceModel.ParentUri 
                    : saveResourceModel.ParentUri + "\\") + saveResourceModel.Name,
                Size = data.Length,
                CreatedOn = DateTime.Now, 
                ModifiedOn = DateTime.Now
            };
        }
    }
}
