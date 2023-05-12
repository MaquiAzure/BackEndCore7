namespace Application.Behaviors
{
    using MediatR;
    public class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
       where TRequest : IRequest<TResponse>
    {

        public UnhandledExceptionBehavior()
        {
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (System.Exception ex)
            {

                var requestName = typeof(TRequest).Name;
                throw;

            }
        }
    }

}
