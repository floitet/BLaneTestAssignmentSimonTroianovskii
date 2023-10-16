using BallastLaneTestAssignment.Application.PrescriptionLists.Queries.GetPrescriptions;
using BallastLaneTestAssignment.Domain.Entities;
using BallastLaneTestAssignment.Domain.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace BallastLaneTestAssignment.Application.IntegrationTests.PrescriptionLists.Queries;

using static Testing;

public class GetPrescriptionsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldReturnPriorityLevels()
    {
        await RunAsDefaultUserAsync();

        var query = new GetPrescriptionsQuery();

        var result = await SendAsync(query);

        result.PriorityLevels.Should().NotBeEmpty();
    }

    [Test]
    public async Task ShouldReturnAllListsAndItems()
    {
        var user = await RunAsDefaultUserAsync();

        var insertedId = await AddAsync(
            new PrescriptionList
            {
                Title = "Shopping",
                Colour = Colour.Blue, 
                CreatedBy = user
            });

        var items = new List<PrescriptionItem>()
        {
            new PrescriptionItem { Title = "Apples", Done = true, CreatedBy = user, ListId = insertedId},
            new PrescriptionItem { Title = "Milk", Done = true, CreatedBy = user, ListId = insertedId  },
            new PrescriptionItem { Title = "Bread", Done = true, CreatedBy = user , ListId = insertedId },
            new PrescriptionItem { Title = "Toilet paper", CreatedBy = user , ListId = insertedId },
            new PrescriptionItem { Title = "Pasta" , CreatedBy = user, ListId = insertedId },
            new PrescriptionItem { Title = "Tissues" , CreatedBy = user, ListId = insertedId },
            new PrescriptionItem { Title = "Tuna", CreatedBy = user, ListId = insertedId }
        };

        foreach (var pi in items)
        {
            await AddAsync(pi);
        }


        var query = new GetPrescriptionsQuery();

        var result = await SendAsync(query);

        result.Lists.Should().HaveCount(1);
        result.Lists.First().Items.Should().HaveCount(7);
    }

    [Test]
    public async Task ShouldDenyAnonymousUser()
    {
        var query = new GetPrescriptionsQuery();

        var action = () => SendAsync(query);

        await action.Should().ThrowAsync<UnauthorizedAccessException>();
    }
}