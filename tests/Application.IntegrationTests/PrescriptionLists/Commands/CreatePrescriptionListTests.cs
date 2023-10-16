using BallastLaneTestAssignment.Application.Common.Exceptions;
using BallastLaneTestAssignment.Application.PrescriptionLists.Commands.CreatePrescriptionList;
using BallastLaneTestAssignment.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace BallastLaneTestAssignment.Application.IntegrationTests.PrescriptionLists.Commands;

using static Testing;

public class CreatePrescriptionListTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreatePrescriptionListCommand();
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireUniqueTitle()
    {
        
        var userId = await RunAsDefaultUserAsync();
        
        await SendAsync(new CreatePrescriptionListCommand
        {
            Title = "Shopping",
            UserId = userId
        });

        var command = new CreatePrescriptionListCommand
        {
            Title = "Shopping",
            UserId = userId
        };

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldCreatePrescriptionList()
    {
        var userId = await RunAsDefaultUserAsync();

        var command = new CreatePrescriptionListCommand
        {
            Title = "Tasks",
            UserId = userId
        };

        var id = await SendAsync(command);

        var list = await FindAsync<PrescriptionList>(id);

        list.Should().NotBeNull();
        list!.Title.Should().Be(command.Title);
        list.CreatedBy.Should().Be(userId);
        list.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(10000));
    }
}
