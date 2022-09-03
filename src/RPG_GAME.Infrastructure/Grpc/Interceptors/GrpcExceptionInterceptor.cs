﻿using Grpc.Core;
using Grpc.Core.Interceptors;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RPG_GAME.Application.Exceptions;
using RPG_GAME.Core.Exceptions;
using System.Net;

namespace RPG_GAME.Infrastructure.Grpc.Interceptors
{
    internal sealed class GrpcExceptionInterceptor : Interceptor
    {
        private readonly ILogger<GrpcExceptionInterceptor> _logger;

        public GrpcExceptionInterceptor(ILogger<GrpcExceptionInterceptor> logger)
        {
            _logger = logger;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await continuation(request, context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                var httpContext = context.GetHttpContext();
                var (error, code, grpCode) = Map(exception);
                httpContext.Response.StatusCode = (int)code;
                var metadata = new Metadata() { new Metadata.Entry(error.Code, error.Message) };
                throw new RpcException(new Status(grpCode, error.Message, exception), metadata);
            }
        }

        private static (Error error, HttpStatusCode code, StatusCode grpcStatus) Map(Exception exception)
           => exception switch
           {
               DomainException ex => (new Error(GetErrorCode(ex), ex.Message), HttpStatusCode.BadRequest, StatusCode.FailedPrecondition),
               BusinessException ex => (new Error(GetErrorCode(ex), ex.Message), HttpStatusCode.BadRequest, StatusCode.FailedPrecondition),
               _ => (new Error("error", "There was an error."), HttpStatusCode.InternalServerError, StatusCode.Internal)
           };

        private record Error(string Code, string Message);

        private static string GetErrorCode(Exception exception)
            => exception.GetType().Name.Replace("Exception", string.Empty).Underscore();
    }
}
