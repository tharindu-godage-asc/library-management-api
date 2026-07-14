using Library.Api.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Library.Api.Common.Filters;
using Library.Api.Contracts.Mappings;
using Library.Api.Contracts.Members;
using Library.Api.Domain.Entities;

namespace Library.Api.Endpoints;

public static class MemberEndpoints
{
    public static void MapMemberEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/members")
            .WithTags("Members");

        // Get All Members
        group.MapGet("/",
            async ([FromServices] IMemberService service) =>
            {
                var members =
                    await service.GetAllAsync();

                return Results.Ok(
                    members.Select(x => x.ToResponse()));
            });

        // Get Member By Id
        group.MapGet("/{id:int}",
            async (
                int id,
                [FromServices] IMemberService service) =>
            {
                var member =
                    await service.GetByIdAsync(id);

                return Results.Ok(
                    member?.ToResponse());
            });

        // Create Member
        group.MapPost("/",
            async (
                CreateMemberRequest request,
                [FromServices] IMemberService service) =>
            {
                var member = new Member(
                    request.FullName,
                    request.Email,
                    request.PhoneNumber);

                await service.CreateAsync(member);

                return Results.Created(
                    $"/api/members/{member.Id}",
                    member.ToResponse());
            })
            .AddEndpointFilter<ValidationFilter<CreateMemberRequest>>();

        // Update Member
        group.MapPut("/{id:int}",
            async (
                int id,
                UpdateMemberRequest request,
                [FromServices] IMemberService service) =>
            {
                await service.UpdateAsync(
                    id,
                    request);

                return Results.NoContent();
            })
            .AddEndpointFilter<ValidationFilter<UpdateMemberRequest>>();

        // Delete Member
        group.MapDelete("/{id:int}",
            async (
                int id,
                [FromServices] IMemberService service) =>
            {
                await service.DeleteAsync(id);

                return Results.NoContent();
            });
    }
}