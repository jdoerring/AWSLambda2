using System;
using System.Text;

namespace AWSLambda2
{
    public class Submission
    {
        public int TaxYear { get; set; }
        public string ClientType { get; set; }
        public string ProductType { get; set; }
        public string FileName { get; set; }
        public string FileData { get; set; }
        public string Email { get; set; }
        public string TrackingId { get; set; }
        public string Environment { get; set; }
    }
}
