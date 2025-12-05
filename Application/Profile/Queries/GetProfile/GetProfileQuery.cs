using Application.Common.Models;
using Application.DTOs;
using MediatR;

namespace Application.Profile.Queries.GetProfile;

/// <summary>
/// Query to get current user's profile
/// </summary>
public class GetProfileQuery : IRequest<OperationResult<UserProfileDto>>
{

}