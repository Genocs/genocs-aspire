﻿using System.ComponentModel;

namespace Genocs.MultitenancyAspire.Application.Catalog.Brands;

public interface IBrandGeneratorJob : IScopedService
{
    [DisplayName("Generate Random Brand example job on Queue notDefault")]
    Task GenerateAsync(int nSeed, CancellationToken cancellationToken);

    [DisplayName("Removes all random brands created example job on Queue notDefault")]
    Task CleanAsync(CancellationToken cancellationToken);
}
