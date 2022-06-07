using System.Text;

namespace KConsoleDebugger
{
    public class Log
    {
        //public readonly string Name;
        public readonly string Path;
        public readonly DateTime CreationDate;
        public readonly string Extension;
        public List<string> Content;

        protected int _logsExistingWithSameName = 0;

        public Log(string path/*, DateTime creationDate*/)
        {
            Path = path;
            //CreationDate = creationDate;
            Content = new List<string>();

            Extension = path.Split('.').Last();
        }
        public Log(string path, bool createLog)
        {
            Path = path;
            //CreationDate = creationDate;
            Content = new List<string>();

            Extension = path.Split('.').Last();
        }

        public virtual Log CreateLog()
        {
            string? path = Path.Substring(0, Path.Length - Extension.Length - 1);

            if (File.Exists(path + $".{Extension}"))
            {
                _logsExistingWithSameName++;
                Console.WriteLine($"{_logsExistingWithSameName}");
                path += $"_({_logsExistingWithSameName})";
            }
            path += $".{Extension}";

            using (FileStream fs = File.Create(path))
            {
                string content = string.Empty;
                foreach (string line in Content) { content += $"{line}\n"; }
                //content += $"{Content.Count}";
                //content += Extension;
                byte[] encodedText = new UTF8Encoding().GetBytes(content);
                fs.Write(encodedText);
            }

            return this;
        }

        public virtual int DeleteOldLogs(string path = @"Logs")
        {
            int logsDeleted = 0;
            string[] files = Directory.GetFiles(path);
            List<string> logs = new();

            foreach (string file in files)
            {
                if (file.EndsWith($".{Extension}"))
                {
                    logs.Add(file);
                }
            }
            int count = logs.Count - 10;
            for (int i = 0; i < count; i++)
            {
                string log = logs[i];
                File.Delete(log);
                logsDeleted++;
            }

            return logsDeleted;
        }
    }
}