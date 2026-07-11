using Library.Api.Applications.Services;
using Library.Api.Common.Filters;
using Library.Api.Contracts.Borrowings;
using Library.Api.Contracts.Mappings;

namespace Library.Api.Endpoints;

public static class BorrowingEndpoints
{
    public static void MapBorrowingEndpoints(
        this WebApplication app)
    {
        var group = app.MapGroup("/api/borrowings")
            .WithTags("Borrowings");

        // Borrow Book
        group.MapPost("/",
            async (
                CreateBorrowingRequest request,
                BorrowingService service) =>
            {
                var borrowing =
                    await service.BorrowBookAsync(
                        request.MemberId,
                        request.BookId);

                return Results.Created(
                    $"/api/borrowings/{borrowing.Id}",
                    borrowing.ToResponse());
            })
            .AddEndpointFilter<ValidationFilter<CreateBorrowingRequest>>();

        // Get All Borrowings
        group.MapGet("/",
            async (BorrowingService service) =>
            {
                var borrowings =
                    await service.GetHistoryAsync();

                return Results.Ok(
                    borrowings.Select(
                        x => x.ToResponse()));
            });

        // Return Book
        group.MapPost("/{id:int}/return",
            async (
                int id,
                BorrowingService service) =>
            {
                await service.ReturnBookAsync(id);

                return Results.NoContent();
            });

        // Get Borrowings By Member
        app.MapGet(
            "/api/members/{memberId:int}/borrowings",
            async (
                int memberId,
                BorrowingService service) =>
            {
                var borrowings =
                    await service
                        .GetMemberHistoryAsync(memberId);

                return Results.Ok(
                    borrowings.Select(
                        x => x.ToResponse()));
            })
            .WithTags("Borrowings");
    }
}