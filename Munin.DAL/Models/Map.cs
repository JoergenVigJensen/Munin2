namespace Munin.DAL.Models
{
    public class Map
    {
        public int MapId { get; set; }

        public string MapNb { get; set; }

        public Journal Journal { get; set; }

        public string Title { get; set; }
        public MapType MapType { get; set; }

        public string Proportion { get; set; }

        public string Publisher { get; set; }

        public int PublishYear { get; set; }

        public int RevisionYear { get; set; }

        public string Area { get; set; }

        public string Format { get; set; }

        public MapMaterial Material { get; set; }

        public string Depot { get; set; }

        public string Comment { get; set; }

    }
}


