using BallastLaneTestAssignment.Application.Common.Interfaces.Database.Repositories;
using BallastLaneTestAssignment.Domain.Entities;
using BallastLaneTestAssignment.Domain.Enums;
using BallastLaneTestAssignment.Domain.ValueObjects;

namespace BallastLaneTestAssignment.Infrastructure.Persistence;

public class DapperDbInitializer
{
    private readonly IUnitOfWork _context;

    public DapperDbInitializer(IUnitOfWork unitOfWork)
    {
        _context = unitOfWork;
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        int lstId = 0;
        var guid = Guid.NewGuid().ToString();
        var prescriptionLists = await _context.PrescriptionLists.GetAllAsync();
        if (prescriptionLists.Count == 0)
        {
            lstId = await _context.PrescriptionLists.AddAsync(new PrescriptionList
            {
                Title = "Prescription List", CreatedBy = guid, Colour = Colour.White
            });
        }
        else
        {
            lstId = prescriptionLists.First().ListId;
        }

        if ((await _context.PrescriptionItems.GetAllAsync()).Count == 0)
        {
            var items =
                new List<PrescriptionItem>()
                {
                    new PrescriptionItem
                    {
                        Title = "Make a prescription list 📃",
                        Note = "This is a note",
                        ListId = lstId,
                        CreatedBy = guid,
                        Priority = PriorityLevel.Medium
                    },
                    new PrescriptionItem
                    {
                        Title = "Check off the first item ✅",
                        CreatedBy = guid,
                        ListId = lstId,
                        Priority = PriorityLevel.Medium
                    },
                    new PrescriptionItem
                    {
                        Title = "Realise you've already done two things on the list! 🤯",
                        CreatedBy = guid,
                        ListId = lstId,
                    },
                    new PrescriptionItem
                    {
                        Title = "Reward yourself with a nice, long nap 🏆", CreatedBy = guid, ListId = lstId,
                    },
                };

            foreach (var p in items)
            {
                await _context.PrescriptionItems.AddAsync(p);
            }
        }

        // Default data
        // Seed, if necessary
    }
}