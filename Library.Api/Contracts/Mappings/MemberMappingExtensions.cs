using Library.Api.Contracts.Members;
using Library.Api.Domain.Entities;

namespace Library.Api.Contracts.Mappings;

public static class MemberMappingExtensions
{
    public static MemberResponse ToResponse(
        this Member member)
    {
        return new MemberResponse(
            member.Id,
            member.FullName,
            member.Email,
            member.PhoneNumber,
            member.RegisteredDate,
            member.IsActive
        );
    }
}