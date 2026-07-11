namespace Library.Api.Contracts.Members;

public record MemberResponse(
    int Id,
    string FullName,
    string Email,
    string PhoneNumber,
    DateTime RegisteredDate,
    bool IsActive
);