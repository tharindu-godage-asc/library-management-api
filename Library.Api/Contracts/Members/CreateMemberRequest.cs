namespace Library.Api.Contracts.Members;

public record CreateMemberRequest(
    string FullName,
    string Email,
    string PhoneNumber
);