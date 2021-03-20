using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EcoSystemAPI.Context.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }
        public string AuthorName { get; set; }

        public string AuthorImg { get; set; }

        public string Name { get; set; }

        public string DateofPublish { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string Image { get; set; }

        public string AuthorId { get; set; }

        public string ContentIntro { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string Content { get; set; }
        




    }
}
