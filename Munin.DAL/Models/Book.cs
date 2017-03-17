using System.ComponentModel.DataAnnotations;

namespace Munin.DAL.Models
{
    public class Book
    {
        [Required]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Der skal angives billedindex")]
        [RegularExpression(@"^(?i)BB.\d{4}$", ErrorMessage = "Bogkode skal opfylde formatet B.1234")]
        public string BookCode { get; set; }

        [Required]
        public Journal Journal { get; set; }
        public string DK5 { get; set; }

        //Ordningsord
        public string Tag { get; set; }

        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Author { get; set; }

        //Redaktør
        public string Editor { get; set; }

        public int PublishYear { get; set; }
        public string Attache { get; set; }
        public string Comment { get; set; }

    }
}
