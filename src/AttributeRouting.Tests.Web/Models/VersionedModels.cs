using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttributeRouting.Tests.Web.Models
{
    public class VersionedModel_old
    {
        public string Text { get; set; }
        public DateTime GeneratedTime { get; set; } 
    }

    public class VersionedModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime GeneratedTime { get; set; } 
    }
}