using BallastLaneTestAssignment.Application.Common.Exceptions;
using BallastLaneTestAssignment.Application.PrescriptionItems.Commands.CreatePrescriptionItem;
using BallastLaneTestAssignment.Application.PrescriptionItems.Commands.UpdatePrescriptionItem;
using BallastLaneTestAssignment.Application.PrescriptionItems.Commands.UpdatePrescriptionItemDetail;
using BallastLaneTestAssignment.Application.PrescriptionLists.Commands.CreatePrescriptionList;
using BallastLaneTestAssignment.Domain.Entities;
using BallastLaneTestAssignment.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;

namespace BallastLaneTestAssignment.Application.IntegrationTests.PrescriptionItems.Commands;

using static Testing;

public class UpdatePrescriptionItemDetailTests : BaseTestFixture
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

        var command = new UpdatePrescriptionItemDetailCommand
        {
            Id = itemId,
            ListId = listId,
            Note = "This is the note.",
            Priority = PriorityLevel.High
        };

        await SendAsync(command);

        var item = await FindAsync<PrescriptionItem>(itemId);

        item.Should().NotBeNull();
        item!.ListId.Should().Be(command.ListId);
        item.Note.Should().Be(command.Note);
        item.Priority.Should().Be(command.Priority);
        item.LastModifiedBy.Should().NotBeNull();
        item.LastModifiedBy.Should().Be(userId);
        item.LastModifiedAt.Should().NotBeNull();
        item.LastModifiedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(10000));
    }
}
