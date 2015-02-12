using System;
using System.Collections.Generic;
using System.IO;
using Core;
using IntegrationServices.Extensions;

namespace IntegrationServices
{
    public class FileGenerationService : IFileGenerationService
    {
        public void CreateDelimitedFile<T>(List<T> items, string path)
        {
            var file = SetupFile(path);
            File.WriteAllText(file.FullName, items.ToDelimitedString());
        }

        private FileInfo SetupFile(string path)
        {
            var file = new FileInfo(path);
            if (file.Directory == null)
                throw new NullReferenceException(string.Format("File directory is null for path: {0}", path));

            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }
            return file;
        }

        public void CreateFileFromText(string data, string path)
        {
            var file = SetupFile(path);
            File.WriteAllText(file.FullName, data);
        }
    }

    public interface IFileGenerationService : IDependency
    {
        void CreateDelimitedFile<T>(List<T> items, string path);
        void CreateFileFromText(string data, string path);
    }
}
