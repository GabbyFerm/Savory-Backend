using Application.Common.Models;
using Application.DTOs;
using MediatR;

namespace Application.Dashboard.Queries.GetDashboardStats;

// <summary>
/// Query to get dashboard statistics for current user
/// </summary>
public class GetDashboardStatsQuery : IRequest<OperationResult<DashboardStatsDto>>
{
}