using System;
using System.Collections.Generic;
using System.Text;

namespace ShadyNagy.Utilities.Api.DTOs
{
    public class CheckUnique: ICheckUnique
    {
        public string FieldName { get; set; }
        public string Value { get; set; }
        public string Id { get; set; }
    }
}
