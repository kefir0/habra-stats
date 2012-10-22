using HabrApi.EntityModel;

namespace HabrApi
{
    /// <summary>
    /// Report to be serialized and transformed by XSLT.
    /// </summary>
    public class Report
    {
        public string Title { get; set; }
        public Comment[] Comments { get; set; }
        public CommentReportAttribute[][] ReportGroups { get; set; }
    }
}