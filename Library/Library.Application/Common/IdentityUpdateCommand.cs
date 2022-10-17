namespace Library.Application.Common;

public abstract class IdentityUpdateCommand
{
    public Guid Id { get; set; } = Guid.Empty;
}