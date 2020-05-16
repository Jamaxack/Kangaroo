using MediatR;
using System;

namespace Delivery.API.Application.Commands
{
    public class IdentifiedCommand<TRequest, TResponse> : IRequest<TResponse>
        where TRequest : IRequest<TResponse>
    {
        public TRequest Command { get; }
        public Guid Id { get; }
        public IdentifiedCommand(TRequest command, Guid id)
        {
            Command = command;
            Id = id;
        }
    }
}
