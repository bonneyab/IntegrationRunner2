using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Core;

namespace IntegrationDataServices
{
    public class CsvDataConnector : ICsvDataExtractor
    {
        //THis doesn't handle commas in the values correctly, consider using a library for this.
        public XDocument GetCsvFileAsXml(string filePath)
        {
            var source = File.ReadAllLines(filePath);
            return GetCsvFileAsXml(source.Select(s => s.Split(',')).ToList());
        }

        public XDocument GetCsvFileAsXml(List<string[]> source)
        {
            var header = source.First();
            var xml = new XElement("root",
                from str in source.Skip(1)
                let fields = str
                select new XElement("item",
                    header.Select((t, i) => new XElement(t, fields[i])).ToList()
                )
            );
            var xmlDocument = new XDocument { Declaration = new XDeclaration("1.0", "utf-8", "yes") };
            xmlDocument.Add(xml);
            return xmlDocument;
        }
    }

    public interface ICsvDataExtractor : IDependency
    {
        XDocument GetCsvFileAsXml(string filePath);
        XDocument GetCsvFileAsXml(List<string[]> source);
    }
}
