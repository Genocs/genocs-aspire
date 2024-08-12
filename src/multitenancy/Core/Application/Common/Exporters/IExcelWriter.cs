using GenocsAspire.Multitenancy.Application.Common.Interfaces;

namespace GenocsAspire.Multitenancy.Application.Common.Exporters;

public interface IExcelWriter : ITransientService
{
    Stream WriteToStream<T>(IList<T> data);
}