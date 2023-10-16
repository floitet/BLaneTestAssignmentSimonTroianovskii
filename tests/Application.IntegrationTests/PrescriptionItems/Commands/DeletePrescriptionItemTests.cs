using BallastLaneTestAssignment.Application.Common.Exceptions;
using BallastLaneTestAssignment.Application.PrescriptionItems.Commands.CreatePrescriptionItem;
using BallastLaneTestAssignment.Application.PrescriptionItems.Commands.DeletePrescriptionItem;
using BallastLaneTestAssignment.Application.PrescriptionLists.Commands.CreatePrescriptionList;
using BallastLaneTestAssignment.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace BallastLaneTestAssignment.Application.IntegrationTests.PrescriptionItems.Commands;

using static Testing;

public class DeletePrescriptionItemTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidPrescriptionItemId()
    {
        var command = new DeletePrescriptionItemCommand(99);

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldDeletePrescriptionItem()
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

        await SendAsync(new DeletePrescriptionItemCommand(itemId));

        var item = await FindAsync<PrescriptionItem>(itemId);

        item.Should().BeNull();
    }
}
