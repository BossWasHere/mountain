using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Mountain.Config
{
    public class IniFileSettings<T> : BaseFileSettings<T>
    {
        public virtual string Comment { get; set; }

        protected bool Init(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Init();
                return false;
            }
            var data = new Dictionary<string, string>();
            foreach (string row in File.ReadAllLines(filePath))
            {
                string[] parts = row.Split('=');
                if (parts.Length > 1) data.Add(parts[0], string.Join("=", parts.Skip(1).ToArray()));
            }

            Init(data);
            return true;
        }

        protected override void Write(Dictionary<string, string> data, string filePath)
        {
            StringBuilder builder = new StringBuilder();
            if (Comment != null) builder.Append(Comment).AppendLine();

            foreach (KeyValuePair<string, string> line in data)
            {
                builder.Append(line.Key).Append('=').Append(line.Value).AppendLine();
            }

            File.WriteAllText(filePath, builder.ToString(), Encoding.UTF8);
        }
    }
}
