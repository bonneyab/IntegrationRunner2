using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Core;

namespace IntegrationDataServices.Validators
{
    public class ListValidationService : IListVerificationService
    {
        private readonly ILoggingService _loggingService;

        public ListValidationService(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public void CheckForEmptyColumns<T>(List<T> items)
        {
            if (!Convert.ToBoolean(ConfigurationManager.AppSettings["ValidateColumns"])) return;
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                if (items.All(i => string.IsNullOrEmpty(Convert.ToString(property.GetValue(i)))))
                {
                    _loggingService.Error(string.Format("Empty column detected: {0}", property.Name));
                }
            }
        }
    }

    public interface IListVerificationService : IDependency
    {
        void CheckForEmptyColumns<T>(List<T> items);
    }
}
