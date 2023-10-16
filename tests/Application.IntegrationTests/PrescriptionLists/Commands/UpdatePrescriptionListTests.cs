using BallastLaneTestAssignment.Application.Common.Exceptions;
using BallastLaneTestAssignment.Application.PrescriptionLists.Commands.CreatePrescriptionList;
using BallastLaneTestAssignment.Application.PrescriptionLists.Commands.UpdatePrescriptionList;
using BallastLaneTestAssignment.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace BallastLaneTestAssignment.Application.IntegrationTests.PrescriptionLists.Commands;

using static Testing;

public class UpdatePrescriptionListTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidPrescriptionListId()
    {
        var command = new UpdatePrescriptionListCommand { Id = 99, Title = "New Title" };
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldRequireUniqueTitle()
    {
        
        var userId = await RunAsDefaultUserAsync();
        
        var listId = await SendAsync(new CreatePrescriptionListCommand
        {
            Title = "New List",
            UserId = userId
        });

        await SendAsync(new CreatePrescriptionListCommand
        {
            Title = "Other List",
            UserId = userId
        });

        var command = new UpdatePrescriptionListCommand
        {
            Id = listId,
            Title = "Other List",
            UserId = userId
        };

        (await FluentActions.Invoking(() =>
            SendAsync(command))
                .Should().ThrowAsync<ValidationException>().Where(ex => ex.Errors.ContainsKey("Title")))
                .And.Errors["Title"].Should().Contain("The specified title already exists.");
    }

    [Test]
    public async Task ShouldUpdatePrescriptionList()
    {
        var userId = await RunAsDefaultUserAsync();

        var listId = await SendAsync(new CreatePrescriptionListCommand
        {
            Title = "New List",
            UserId = userId
        });

        var command = new UpdatePrescriptionListCommand
        {
            Id = listId,
            Title = "Updated List Title",
            UserId = userId
        };

        await SendAsync(command);

        var list = await FindAsync<PrescriptionList>(listId);

        list.Should().NotBeNull();
        list!.Title.Should().Be(command.Title);
        list.LastModifiedBy.Should().NotBeNull();
        list.LastModifiedBy.Should().Be(userId);
        list.LastModifiedAt.Should().NotBeNull();
        list.LastModifiedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(10000));
    }
}
