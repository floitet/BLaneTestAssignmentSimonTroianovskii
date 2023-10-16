using BallastLaneTestAssignment.Application.Common.Exceptions;
using BallastLaneTestAssignment.Application.PrescriptionLists.Commands.CreatePrescriptionList;
using BallastLaneTestAssignment.Application.PrescriptionLists.Commands.DeletePrescriptionList;
using BallastLaneTestAssignment.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace BallastLaneTestAssignment.Application.IntegrationTests.PrescriptionLists.Commands;

using static Testing;

public class DeletePrescriptionListTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidPrescriptionListId()
    {
        var command = new DeletePrescriptionListCommand(99);
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldDeletePrescriptionList()
    {
        var userId = await RunAsDefaultUserAsync();
        
        var listId = await SendAsync(new CreatePrescriptionListCommand
        {
            Title = "New List",
            UserId = userId
        });

        await SendAsync(new DeletePrescriptionListCommand(listId));

        var list = await FindAsync<PrescriptionList>(listId);

        list.Should().BeNull();
    }
}
