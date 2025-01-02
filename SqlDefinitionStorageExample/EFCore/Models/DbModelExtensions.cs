using System;
using Telerik.WebReportDesigner.Services.Models;

namespace SqlDefinitionStorageExample.EFCore.Models
{
    public static class DbModelExtensions
    {
        public static ResourceFileModel ToResourceFileModel(this EFCore.Models.Resource dbReportModel)
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
                ModifiedOn = dbReportModel.ModifiedOn
            };
        }

        public static SharedDataSourceModel ToSharedDataSourceModel(this EFCore.Models.Resource dbReportModel)
        {
            if (dbReportModel == null)
            {
                return null;
            }

            return new SharedDataSourceModel()
            {
                FileName = dbReportModel.Name,
                ParentPath = dbReportModel.ParentUri,
                Uri = dbReportModel.Uri,
                Size = dbReportModel.Bytes.Length / 1024f,
                CreatedOn = dbReportModel.CreatedOn,
                ModifiedOn = dbReportModel.ModifiedOn,
                Description = dbReportModel.Description,
                FullPath = dbReportModel.Uri,
            };
        }


        public static ResourceFolderModel ToResourceFolderModel(this EFCore.Models.ResourceFolder dbReportFolderModel)
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

        public static EFCore.Models.ResourceFolder ToDbResourceFolderModel(this CreateFolderModel createFolderModel)
        {
            return new EFCore.Models.ResourceFolder() {
                Name = createFolderModel.Name,
                ParentUri = createFolderModel.ParentUri,
                Uri = (string.IsNullOrEmpty(createFolderModel.ParentUri) 
                    ? createFolderModel.ParentUri 
                    : createFolderModel.ParentUri + "\\") + createFolderModel.Name,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };
        }

        public static EFCore.Models.Resource ToDbResourceModel(this SaveResourceModel saveResourceModel, byte[] data)
        {
            return new Models.Resource()
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

        public static EFCore.Models.Resource ToDbResourceModel(this SaveResourceModel saveResourceModel, byte[] data, string description)
        {
            return new Models.Resource()
            {
                Name = saveResourceModel.Name,
                Bytes = data,
                ParentUri = saveResourceModel.ParentUri,
                Uri = (string.IsNullOrEmpty(saveResourceModel.ParentUri)
                    ? saveResourceModel.ParentUri
                    : saveResourceModel.ParentUri + "\\") + saveResourceModel.Name,
                Size = data.Length,
                Description = description,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };
        }

        public static EFCore.Models.Resource ToDbResourceModel(this SharedDataSourceModel model, byte[] data)
        {
            return new Models.Resource()
            {
                Name = model.FileName,
                Bytes = data,
                ParentUri = model.ParentPath,
                Uri = (string.IsNullOrEmpty(model.ParentPath)
                    ? model.ParentPath
                    : model.ParentPath + "\\") + model.FileName,
                Size = data.Length,
                Description = model.Description,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };
        }
    }
}
