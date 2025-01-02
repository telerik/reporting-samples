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
    public class CustomResourceStorage(SqlDefinitionStorageContext context, string rootFolder = "Resources") : CustomDefinitionStorage(context, rootFolder), IResourceStorage
    {

        #region Not Used
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

        public string Save(string resourceName, byte[] resource)
        {
            throw new NotImplementedException();
        }

        #endregion

        public new Task<IEnumerable<ResourceModelBase>> GetFolderContentsAsync(string uri)
        {
            return base.GetFolderContentsAsync(uri);
        }

        public ResourceFileDataModel GetFile(string resourceUri)
        {
            var file = DbContext.Resources.FirstOrDefault(r => r.Uri == resourceUri);

            return new ResourceFileDataModel()
            {
                Data = file.Bytes,
                Name = file.Name,
                MimeType = null // Currently MIME-type is not supported. To be implemented based on file extension and/or containing parent folder.
            };
        }

        public ResourceFolderModel GetFolderByName(string folderName)
        {
            return DbContext.ResourceFolders.FirstOrDefault(x => x.Name == folderName).ToResourceFolderModel();
        }

        public bool ResourceExists(string uri)
        {
            return DbContext.Resources.Any(r => r.Name == uri);
        }

        public bool ResourceNameExists(string resourceName)
        {
            return DbContext.Resources.Any(r => r.Name == resourceName);
        }

        public IEnumerable<ResourceModelBase> Search(SearchResourcesModel model)
        {
            return DbContext.Resources.Where(r => r.ParentUri == model.ResourceFolderUri && r.Name.Contains(model.SearchPattern)).Select(r => r.ToResourceFileModel()); 
        }

        public IEnumerable<ResourceFileModel> GetAllByExtension(string[] extensions)
        { 
            return DbContext.Resources.Where(r => extensions.Any(ex => r.Name.Contains(ex.Substring(1)))).Select(r => r.ToResourceFileModel()).AsEnumerable<ResourceFileModel>();  
        }

        public byte[] GetByUri(string uri)
        {
            return DbContext.Resources.FirstOrDefault(r => r.Uri == uri).Bytes;
        }

        public ResourceFileModel GetModelByName(string resourceName)
        {
            return DbContext.Resources.FirstOrDefault(r => r.Uri == resourceName).ToResourceFileModel();
        }
    }
}
