using BallastLaneTestAssignment.Application.Common.Exceptions;
using BallastLaneTestAssignment.Application.Common.Security;
using BallastLaneTestAssignment.Application.PrescriptionLists.Commands.CreatePrescriptionList;
using BallastLaneTestAssignment.Application.PrescriptionLists.Commands.PurgePrescriptionLists;
using BallastLaneTestAssignment.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace BallastLaneTestAssignment.Application.IntegrationTests.PrescriptionLists.Commands;

using static Testing;

public class PurgePrescriptionListsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldDenyAnonymousUser()
    {
        var command = new PurgePrescriptionListsCommand();

        command.GetType().Should().BeDecoratedWith<AuthorizeAttribute>();

        var action = () => SendAsync(command);

        await action.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task ShouldDenyNonAdministrator()
    {
        await RunAsDefaultUserAsync();

        var command = new PurgePrescriptionListsCommand();

        var action = () => SendAsync(command);

        await action.Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task ShouldAllowAdministrator()
    {
        await RunAsAdministratorAsync();

        var command = new PurgePrescriptionListsCommand();

        var action = () => SendAsync(command);

        await action.Should().NotThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task ShouldDeleteAllLists()
    {
        var userId = await RunAsAdministratorAsync();

        await SendAsync(new CreatePrescriptionListCommand
        {
            Title = "New List #1",
            UserId = userId
        });

        await SendAsync(new CreatePrescriptionListCommand
        {
            Title = "New List #2",
            UserId = userId
        });

        await SendAsync(new CreatePrescriptionListCommand
        {
            Title = "New List #3",
            UserId = userId
        });

        await SendAsync(new PurgePrescriptionListsCommand());

        var count = await CountAsync<PrescriptionList>();

        count.Should().Be(0);
    }
}
