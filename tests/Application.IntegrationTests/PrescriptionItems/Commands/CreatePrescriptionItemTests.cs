using BallastLaneTestAssignment.Application.Common.Exceptions;
using BallastLaneTestAssignment.Application.PrescriptionItems.Commands.CreatePrescriptionItem;
using BallastLaneTestAssignment.Application.PrescriptionLists.Commands.CreatePrescriptionList;
using BallastLaneTestAssignment.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace BallastLaneTestAssignment.Application.IntegrationTests.PrescriptionItems.Commands;

using static Testing;

public class CreatePrescriptionItemTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreatePrescriptionItemCommand();

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldCreatePrescriptionItem()
    {
        var userId = await RunAsDefaultUserAsync();

        var listId = await SendAsync(new CreatePrescriptionListCommand
        {
            Title = "New List",
            UserId = userId
        });

        var command = new CreatePrescriptionItemCommand
        {
            ListId = listId,
            Title = "Tasks",
            CreatedBy = userId
        };

        var itemId = await SendAsync(command);

        var item = await FindAsync<PrescriptionItem>(itemId);

        item.Should().NotBeNull();
        item!.ListId.Should().Be(command.ListId);
        item.Title.Should().Be(command.Title);
        item.CreatedBy.Should().Be(userId);
        item.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(10000));
        item.LastModifiedBy.Should().Be(userId);
        item.LastModifiedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(10000));
    }
}
