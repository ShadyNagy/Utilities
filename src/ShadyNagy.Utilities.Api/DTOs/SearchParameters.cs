﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ShadyNagy.Utilities.Api.DTOs
{
    public class SearchParameters: ISearchParameters
    {
        public int Page { get; set; }
        public string FieldName { get; set; }
        public string Value { get; set; }
    }
}
