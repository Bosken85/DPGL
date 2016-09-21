using System.IO;

namespace Digipolis.Web.Guidelines.Api.Data.Entiteiten
{
    public class File
    {
        public int Id { get; set; }

        public int ValueId { get; set; }

        public MemoryStream Stream { get; set; }

        public File()
        {
        }

        public File(int id, int valueId)
        {
            Id = id;
            ValueId = valueId;
            Stream = new MemoryStream();
            using (TextWriter writer = new StreamWriter(Stream))
            {
                writer.WriteLine(string.Format("This file is for id {0} under value {1}", Id, ValueId));
            }
        }
    }
}
