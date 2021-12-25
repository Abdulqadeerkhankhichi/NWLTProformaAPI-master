using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace NWLTLambda.Models
{
    public class StructureImages
    {

        public int form_id { get; set; }
        //public int image_id { get; set; }
        public string structure_type_id { get; set; }
        public string image_type { get; set; }
        public string image_url { get; set; }
        public string image_name { get; set; }
        public IFormFile image_file { get; set; }
    }
}
