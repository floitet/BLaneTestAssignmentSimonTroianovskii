using BallastLaneTestAssignment.Application.Common.Interfaces;

namespace BallastLaneTestAssignment.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
