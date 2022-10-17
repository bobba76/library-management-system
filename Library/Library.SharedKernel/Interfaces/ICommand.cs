using MediatR;

namespace Library.SharedKernel.Interfaces;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}

