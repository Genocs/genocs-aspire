using GenocsAspire.Multitenancy.Application.Common.Interfaces;

namespace GenocsAspire.Multitenancy.Application.Common.Mailing;

public interface IMailService : ITransientService
{
    Task SendAsync(MailRequest request, CancellationToken ct);
}