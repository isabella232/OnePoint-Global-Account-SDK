﻿
using System.Collections.Generic;

namespace OnePoint.AccountSdk.Media
{
    public class Media
    {
        public int MediaId { get; set; }
        public string MediaName { get; set; }
        public string MediaDescription { get; set; }
        public int MediaTypeId { get; set; }
        public string MediaType { get; set; }
        public string CreatedDate { get; set; }
        public string LastUpdatedDate { get; set; }
        public bool IsVideo { get; set; }
        public bool IsSound { get; set; }
        public bool IsImage { get; set; }
        public string MediaUrl { get; set; }
    }

    public class MediaRoot
    {
        public List<Media> Media { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsSuccess { get; set; }
    }
}
