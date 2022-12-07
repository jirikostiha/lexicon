using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;

public static class LexiconApi
{
    public static RouteGroupBuilder MapWords(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/words");

        group.WithTags("Words");

        //group.WithParameterValidation(typeof(TodoItem));

        group.MapGet("/", async (WordRecordContext db) =>
        {
        });


        return group;
    }
}