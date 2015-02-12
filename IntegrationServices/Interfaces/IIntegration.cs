using Core;

namespace IntegrationServices.Interfaces
{
    public interface IIntegration : IDependency
    {
        void Execute(string[] args);
        string IntegrationName { get; }
        string Directions { get; }
    }
}
