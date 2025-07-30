﻿using MediatR;

namespace Shared.Contracts.CQRS
{
    public interface IQueryHandler<in IQuery, TResponse> : IRequestHandler<IQuery, TResponse>
        where IQuery : IQuery<TResponse>
        where TResponse : notnull
    {
    }
}
