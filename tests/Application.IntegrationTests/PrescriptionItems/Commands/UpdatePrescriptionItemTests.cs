using BallastLaneTestAssignment.Application.Common.Exceptions;
using BallastLaneTestAssignment.Application.PrescriptionItems.Commands.CreatePrescriptionItem;
using BallastLaneTestAssignment.Application.PrescriptionItems.Commands.UpdatePrescriptionItem;
using BallastLaneTestAssignment.Application.PrescriptionLists.Commands.CreatePrescriptionList;
using BallastLaneTestAssignment.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace BallastLaneTestAssignment.Application.IntegrationTests.PrescriptionItems.Commands;

using static Testing;

public class UpdatePrescriptionItemTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidPrescriptionItemId()
    {
        var command = new UpdatePrescriptionItemCommand { Id = 99, Title = "New Title" };
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldUpdatePrescriptionItem()
    {
        var userId = await RunAsDefaultUserAsync();

        var listId = await SendAsync(new CreatePrescriptionListCommand
        {
            Title = "New List",
            UserId = userId
        });

        var itemId = await SendAsync(new CreatePrescriptionItemCommand
        {
            ListId = listId,
            Title = "New Item",
            CreatedBy = userId
        });

        var command = new UpdatePrescriptionItemCommand
        {
            Id = itemId,
            Title = "Updated Item Title",
            UserId = userId
        };

        await SendAsync(command);

        var item = await FindAsync<PrescriptionItem>(itemId);

        item.Should().NotBeNull();
        item!.Title.Should().Be(command.Title);
        item.LastModifiedBy.Should().NotBeNull();
        item.LastModifiedBy.Should().Be(userId);
        item.LastModifiedAt.Should().NotBeNull();
        item.LastModifiedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(10000));
    }
}
