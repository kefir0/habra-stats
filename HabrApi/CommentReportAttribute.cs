using System;

namespace HabrApi
{
    public class CommentReportAttribute : Attribute
    {
        public string Category { get; set; }
        public string Name { get; set; }
        public int CategoryOrder { get; set; }

        public string Value
        {
            get { return Name.ToWebPageName(); }
            set { } // For serialization
        }
    }
}