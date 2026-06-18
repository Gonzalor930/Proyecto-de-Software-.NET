using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;

namespace SGE.WebApi.Endpoints;

public static class TramitesEndpoints
{
    public static void MapTramitesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/tramites");

        //Aca van a ir los endpoints
        
    }
}