using MediatR;

namespace Library.SharedKernel.Interfaces;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
