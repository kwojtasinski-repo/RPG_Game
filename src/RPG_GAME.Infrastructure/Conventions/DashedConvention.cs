using Humanizer;
using Microsoft.AspNetCore.Routing;

namespace RPG_GAME.Infrastructure.Conventions
{
    internal class DashedConvention : IOutboundParameterTransformer
    {
        public string? TransformOutbound(object? value)
        {
            if (value == null) { return null; }

            var routeName = value.ToString().Kebaberize();

            return routeName;
        }
    }
}
