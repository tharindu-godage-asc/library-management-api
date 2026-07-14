using Library.Api.Contracts.Members;
using Library.Api.Domain.Entities;

namespace Library.Api.Application.Interfaces;

public interface IMemberService
{
    Task<IEnumerable<Member>> GetAllAsync();

    Task<Member> GetByIdAsync(int id);

    Task<Member> CreateAsync(Member member);

    Task UpdateAsync(
        int id,
        UpdateMemberRequest request);

    Task DeleteAsync(int id);
}