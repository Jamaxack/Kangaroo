using System.Collections.Generic;

namespace Pricing.API.DataTransferableObjects
{
    public class DistanceMetrixDto
    {
        public List<Row> Rows { get; set; }
        public string Status { get; set; }
        public string Error_Message { get; set; }
    }
}