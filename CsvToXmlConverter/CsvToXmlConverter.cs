using System;
using System.Configuration;
using System.IO;
using Core;
using IntegrationDataServices;
using IntegrationServices;
using IntegrationServices.Interfaces;

namespace CsvToXmlConverter
{
    public class CsvToXmlConverter : IIntegration
    {
        private readonly string _outgoingLocation = string.Format("{0}{1}{2}.xml",
            ConfigurationManager.AppSettings["OutgoingLocation"], ConfigurationManager.AppSettings["BaseFileName"],
            DateTime.Now.ToString("_yyyyMMdd_hhmmss"));

        public string IntegrationName { get { return "CsvToXmlConverter"; } }
        public string Directions
        {
            get
            {
                return
                    "Specify \"CsvToXmlConverter\" this will pull files from the incoming folder specified in the config, convert them to xml, and drop them in the outgoing folder";
            }
        }

        private readonly ICsvDataExtractor _csvDataExtractor;
        private readonly IFileGenerationService _fileGenerationService;
        private readonly ILoggingService _loggingService;

        public CsvToXmlConverter(ICsvDataExtractor csvDataExtractor, IFileGenerationService fileGenerationService,
            ILoggingService loggingService)
        {
            _csvDataExtractor = csvDataExtractor;
            _fileGenerationService = fileGenerationService;
            _loggingService = loggingService;
        }


        public void Execute(string[] args)
        {

            _loggingService.Info("Starting Csv to Xml execution");
            //Pull data from DHA
            foreach (var file in Directory.GetFiles(ConfigurationManager.AppSettings["IncomingLocation"]))
            {
                _loggingService.Info(string.Format("Retrieving data from file {0}", file));
                var xml = _csvDataExtractor.GetCsvFileAsXml(file);
                _loggingService.Info("Generating XML File");
                _fileGenerationService.CreateFileFromText(string.Format("{0}\r\n{1}", xml.Declaration, xml),
                    _outgoingLocation);
                _loggingService.Info("Archiving incoming file");
                var fileName = Path.GetFileName(file);
                File.Move(file, ConfigurationManager.AppSettings["ArchiveLocation"] +"\\" + fileName);
            }
            _loggingService.Info("Finished execution");
        }

    }
}
