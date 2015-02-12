using System;

namespace IntegrationServices.Attributes
{
    public class CsvColumnAttribute : Attribute
    {
        public int Order { get; set; }
        public string Name { get; set; }

        public CsvColumnAttribute()
        {
            Order = int.MaxValue; // so unordered columns are at the end
        }

    }
}
