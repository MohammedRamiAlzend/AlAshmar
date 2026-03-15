namespace AlAshmar.Infrastructure;

/// <summary>
/// Handler registration is managed via MediatR in Program.cs using AddMediatR.
/// This class is kept for future extensibility if custom handler registration is needed.
/// </summary>
public static class HandlerRegistration
{
    // No custom handler registration needed at the moment.
    // MediatR handlers are automatically registered via:
    // builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(...));
}
