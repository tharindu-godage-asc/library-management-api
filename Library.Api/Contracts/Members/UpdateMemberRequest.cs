namespace Library.Api.Contracts.Members;

public record UpdateMemberRequest(
    string FullName,
    string Email,
    string PhoneNumber,
    bool IsActive
);