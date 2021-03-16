using System;
using System.Collections.Generic;
using System.Text;

namespace EcoSystemAPI.Core.Dtos
{
    public class TeamsReadDto
    {
        public int TeamsId { get; set; }
        public string TeamsImage { get; set; }

        public string TeamsName { get; set; }

        public string TeamsRole { get; set; }

        public string TeamsDetails { get; set; }
        public string TeamsFacebook { get; set; }
        public string TeamsInstagram { get; set; }
        public string TeamsTwitter { get; set; }
    }
}
