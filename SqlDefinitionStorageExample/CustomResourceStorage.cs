using SqlDefinitionStorageExample.EFCore;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Telerik.WebReportDesigner.Services;
using Telerik.WebReportDesigner.Services.Models;

namespace SqlDefinitionStorageExample
{
    public class CustomResourceStorage(SqlDefinitionStorageContext context, string rootFolder = "Resources") : CustomDefinitionStorage(context, rootFolder), IResourceStorage
    {

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

        public IEnumerable<ResourceFileModel> GetAllByExtension(string[] extensions)
        {
            throw new System.NotImplementedException();
        }

        public byte[] GetByUri(string uri)
        {
            throw new System.NotImplementedException();
        }

        public ResourceFileDataModel GetFile(string resourceUri)
        {
            throw new System.NotImplementedException();
        }

        public ResourceFolderModel GetFolderByName(string folderName)
        {
            throw new System.NotImplementedException();
        }

        public ResourceFileModel GetModelByName(string resourceName)
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

        public bool ResourceExists(string uri)
        {
            throw new System.NotImplementedException();
        }

        public bool ResourceNameExists(string resourceName)
        {
            throw new System.NotImplementedException();
        }

        public string Save(string resourceName, byte[] resource)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ResourceModelBase> Search(SearchResourcesModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}
