using System.Collections.Generic;

namespace Pricing.API.DataTransferableObjects
{
    public class DistanceMetrixDTO
    {
        public List<Row> Rows { get; set; }
        public string Status { get; set; }
        public string Error_Message { get; set; }
    }

    public class Row
    {
        public List<Element> Elements { get; set; }
    }

    public class Element
    {
        public Metrix Distance { get; set; }
        public Metrix Duration { get; set; }
    }

    public class Metrix
    {
        public long Value { get; set; }
    }
}