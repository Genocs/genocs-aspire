using GenocsAspire.Multitenancy.Application.Common.Interfaces;

namespace GenocsAspire.Multitenancy.Application.Common.Mailing;

public interface IEmailTemplateService : ITransientService
{
    string GenerateEmailTemplate<T>(string templateName, T mailTemplateModel);
}