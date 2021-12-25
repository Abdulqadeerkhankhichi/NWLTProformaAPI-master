using System;
using System.Collections.Generic;
using System.Text;

namespace NWLTLambda.Models
{
    public class CalcParameterOutputModel
    {
        public string ParameterId { get; set; }
        public string ParameterName { get; set; }
        public string value { get; set; }
        public string Phase { get; set; }
        public string ParameterTypeId { get; set; }
        public string ParamOrder { get; set; }
    }
}
