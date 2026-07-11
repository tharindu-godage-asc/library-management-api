using Library.Api.Domain.Entities;

namespace Library.Api.Application.Interfaces;

public interface IMemberRepository
{
    Task<Member?> GetByIdAsync(int id);

    Task<Member?> GetByEmailAsync(string email);

    Task<IEnumerable<Member>> GetAllAsync();

    Task AddAsync(Member member);

    void Update(Member member);

    void Delete(Member member);
}