using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using IntegrationServices.Attributes;

namespace IntegrationServices.Extensions
{
    public static class ListToCsvExtension
    {
        public static string ToDelimitedString<T>(this List<T> list, string delimiter = "|", bool includeHeaders = false)
        {
            var propertyInfos = typeof(T).GetProperties()
                .Where(x => x.CustomAttributes.Any(c => c.AttributeType == typeof(CsvColumnAttribute))).ToList();
            var sb = new StringBuilder();
            if(includeHeaders)
                sb.AppendLine(GetCsvHeaderSorted(propertyInfos, delimiter));
            list.ForEach(d => sb.AppendLine(GetCsvDataRowSorted(d, propertyInfos, delimiter)));
            return sb.ToString();
        }

        private static string GetCsvDataRowSorted<T>(T csvDataObject, IEnumerable<PropertyInfo> propertyInfos, string delimiter)
        {
            IEnumerable<string> valuesSorted = propertyInfos
                .Select(x => new
                {
                    Value = x.GetValue(csvDataObject, null),
                    Attribute = (CsvColumnAttribute)Attribute.GetCustomAttribute(x, typeof(CsvColumnAttribute), false)
                })
                .OrderBy(x => x.Attribute.Order)
                .Select(x => GetPropertyValueAsString(x.Value));
            return String.Join(delimiter, valuesSorted);
        }

        private static string GetCsvHeaderSorted(IEnumerable<PropertyInfo> propertyInfos, string delimiter)
        {
            var headersSorted = propertyInfos
                .Select(x => (CsvColumnAttribute)Attribute.GetCustomAttribute(x, typeof(CsvColumnAttribute), false))
                .OrderBy(x => x.Order)
                .Select(x => x.Name);
            return String.Join(delimiter, headersSorted);
        }

        private static string GetPropertyValueAsString(object propertyValue)
        {
            string propertyValueString;

            if (propertyValue == null)
                propertyValueString = "";
            else if (propertyValue is DateTime)
                propertyValueString = ((DateTime)propertyValue).ToString("yyyy-MM-dd");
            else // treat as a string
                //stripping out pipes...who uses those anyways
                propertyValueString = propertyValue.ToString().Replace("|", ""); 

            return propertyValueString;
        }
    }
}
