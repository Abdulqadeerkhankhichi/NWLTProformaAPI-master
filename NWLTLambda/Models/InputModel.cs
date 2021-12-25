﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NWLTLambda.Models
{
    public class InputModel
    {
        public string formId { get; set; }
        public string address { get; set; }
        public string username { get; set; }
        public string CityId { get; set; }
        public List<ParameterInputModel> parameters { get; set; }
        public List<StructureTypesVM> StructureTypes { get; set; }
        public List<StructureImages> StructureImages { get; set; }
    }
}
