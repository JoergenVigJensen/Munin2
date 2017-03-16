using System.Diagnostics;

namespace Munin.DAL.Models
{
    /// <summary>
    /// Arkivalie
    /// </summary>
    public class ArchiveDocument
    {
        public int ArchiveDocumentId { get; set; }
        public Journal Journal { get; set; }
        public Archive AriArchive { get; set; }

        public int BoxNumber { get; set; }

        public string Signature { get; set; }

        public bool Copyright { get; set; }

        public int YearStart { get; set; }

        public int YearEnd { get; set; }

        public Cover Cover { get; set; } 

        public string Comment { get; set; }
    }
}
