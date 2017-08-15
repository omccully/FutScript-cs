using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FutScriptFunctions.Mouse.Recorded
{
    public class RecordedMousePathsFile
    {
        string FilePath;
        public readonly RecordedMousePaths Paths;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FilePath">File path to read and write to</param>
        public RecordedMousePathsFile(string FilePath)
        {
            this.FilePath = FilePath;
            if (File.Exists(FilePath))
            {
                Paths = RecordedMousePaths.FromJson(File.ReadAllText(FilePath));
            }
            else
            {
                Paths = new RecordedMousePaths();
            }
        }

        public void Save()
        {
            File.WriteAllText(FilePath, Paths.ToJson());
        }
    }
}
