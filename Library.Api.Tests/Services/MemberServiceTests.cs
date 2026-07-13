using Library.Api.Application.Interfaces;
using Library.Api.Applications.Services;
using Library.Api.Common.Exceptions;
using Library.Api.Domain.Entities;
using Moq;
using Xunit;

namespace Library.Api.Tests.Services;

public class MemberServiceTests
{
    private readonly Mock<IMemberRepository> _memberRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;

    private readonly MemberService _service;

    public MemberServiceTests()
    {
        _memberRepository = new Mock<IMemberRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();

        _service = new MemberService(
            _memberRepository.Object,
            _unitOfWork.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenEmailAlreadyExists()
    {
        var member = new Member(
            "John",
            "john@test.com",
            "123");

        _memberRepository
            .Setup(x => x.GetByEmailAsync("john@test.com"))
            .ReturnsAsync(member);

        await Assert.ThrowsAsync<ConflictException>(
            () => _service.CreateAsync(member));
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrow_WhenMemberNotFound()
    {
        _memberRepository
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((Member?)null);

        await Assert.ThrowsAsync<NotFoundException>(
            () => _service.GetByIdAsync(1));
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrow_WhenMemberNotFound()
    {
        _memberRepository
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((Member?)null);

        await Assert.ThrowsAsync<NotFoundException>(
            () => _service.DeleteAsync(1));
    }
}