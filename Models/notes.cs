using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXAMEN_TERCER_PARCIAL.Models
{
    public class notes
    {
        [JsonProperty("id_nota")]
        public int NoteID { get; set; }

        [JsonProperty("descripcion")]
        public string Desc { get; set; }

        [JsonProperty("fecha")]
        public double Date { get; set; }

    }
}

