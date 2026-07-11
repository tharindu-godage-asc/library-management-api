using Library.Api.Application.Interfaces;
using Library.Api.Domain.Entities;

namespace Library.Api.Applications.Services;

public class MemberService
{
    private readonly IMemberRepository _memberRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MemberService(
        IMemberRepository memberRepository,
        IUnitOfWork unitOfWork)
    {
        _memberRepository = memberRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Member>> GetAllAsync()
    {
        return await _memberRepository.GetAllAsync();
    }

    public async Task<Member?> GetByIdAsync(int id)
    {
        return await _memberRepository.GetByIdAsync(id);
    }

    public async Task<Member> CreateAsync(Member member)
    {
        var existingMember =
            await _memberRepository.GetByEmailAsync(member.Email);

        if (existingMember is not null)
        {
            throw new InvalidOperationException(
                "Email already exists.");
        }

        await _memberRepository.AddAsync(member);

        await _unitOfWork.SaveChangesAsync();

        return member;
    }

    public async Task DeleteAsync(int id)
    {
        var member = await _memberRepository.GetByIdAsync(id);

        if (member is null)
        {
            throw new KeyNotFoundException(
                "Member not found.");
        }

        _memberRepository.Delete(member);

        await _unitOfWork.SaveChangesAsync();
    }
}