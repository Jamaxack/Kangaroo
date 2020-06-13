using System;
using MediatR;

namespace Delivery.API.Application.Commands
{
    public class IdentifiedCommand<TRequest, TResponse> : IRequest<TResponse>
        where TRequest : IRequest<TResponse>
    {
        public IdentifiedCommand(TRequest command, Guid id)
        {
            Command = command;
            Id = id;
        }

        public TRequest Command { get; }
        public Guid Id { get; }
    }
}