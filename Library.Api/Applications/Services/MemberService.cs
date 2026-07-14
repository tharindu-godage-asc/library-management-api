using Library.Api.Application.Interfaces;
using Library.Api.Common.Exceptions;
using Library.Api.Contracts.Members;
using Library.Api.Domain.Entities;

namespace Library.Api.Applications.Services;

public class MemberService : IMemberService
{
    /*
 * Dependencies
 * ------------
 * These are objects that MemberService needs to perform its work.
 *
 * IMemberRepository:
 * - Handles database operations related to Members.
 * - Example: Get member, Add member, Delete member.
 *
 * IUnitOfWork:
 * - Controls saving changes to the database.
 * - Ensures multiple database operations can be committed together.
 */
    private readonly IMemberRepository _memberRepository;
    private readonly IUnitOfWork _unitOfWork;


        /*
         * Constructor Dependency Injection
         * --------------------------------
         * ASP.NET Core automatically creates these dependencies and passes
         * them into the constructor.
         *
         * This avoids creating repositories manually inside this class.
         */
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

    public async Task<Member> GetByIdAsync(int id)
    {
        var member =
            await _memberRepository.GetByIdAsync(id);

        if (member is null)
        {
            throw new NotFoundException(
                "Member not found");
        }

        return member;
    }

    public async Task<Member> CreateAsync(Member member)
    {
        var existingMember =
            await _memberRepository.GetByEmailAsync(member.Email);

        if (existingMember is not null)
        {
            throw new ConflictException(
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
            throw new NotFoundException(
                "Member not found.");
        }

        _memberRepository.Delete(member);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id,
    UpdateMemberRequest request)
    {
        var member =
            await _memberRepository.GetByIdAsync(id);

        if (member is null)
        {
            throw new NotFoundException(
                "Member not found.");
        }

        var existingMember =
            await _memberRepository.GetByEmailAsync(
                request.Email);

        if (existingMember is not null &&
            existingMember.Id != id)
        {
            throw new ConflictException(
                "Email already exists.");
        }

        member.Update(
            request.FullName,
            request.Email,
            request.PhoneNumber,
            request.IsActive);

        _memberRepository.Update(member);

        await _unitOfWork.SaveChangesAsync();
    }
}