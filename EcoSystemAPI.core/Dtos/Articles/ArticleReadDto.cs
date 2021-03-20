using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EcoSystemAPI.Core.Dtos
{
    public class ArticleReadDto
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public string AuthorImg { get; set; }
        public string Name { get; set; }
        public string DateofPublish { get; set; }
        public string Image { get; set; }
        public string AuthorId { get; set; }
        public string Content { get; set; }
        public string ContentIntro { get; set; }
    }
}
