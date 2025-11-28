using Application.Common.Models;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Profile.Queries.GetProfile;

public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, OperationResult<UserProfileDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetProfileQueryHandler(UserManager<ApplicationUser> userManager, ICurrentUserService currentUserService, IMapper mapper)
    {
        _userManager = userManager;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<OperationResult<UserProfileDto>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        // Get current user ID
        var userId = _currentUserService.GetUserId();
        if (userId == null)
        {
            return OperationResult<UserProfileDto>.Failure("User not authenticated");
        }

        // Get user from database
        var user = await _userManager.FindByIdAsync(userId.Value.ToString());
        if (user == null)
        {
            return OperationResult<UserProfileDto>.Failure("User not found");
        }

        // Map to DTO
        var userDto = _mapper.Map<UserProfileDto>(user);

        return OperationResult<UserProfileDto>.Success(userDto);
    }
}