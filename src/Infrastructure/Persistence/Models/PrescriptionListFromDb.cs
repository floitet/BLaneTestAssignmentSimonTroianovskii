using AutoMapper;
using BallastLaneTestAssignment.Application.Common.Mappings;
using BallastLaneTestAssignment.Domain.Entities;
using BallastLaneTestAssignment.Domain.ValueObjects;
using BallastLaneTestAssignment.Domain.ValueObjects;

using Colour = BallastLaneTestAssignment.Domain.ValueObjects.Colour;


namespace BallastLaneTestAssignment.Infrastructure.Persistence.Models;

public class PrescriptionListFromDb : PrescriptionListBase
{

    public int ListId { get; set; }
    public string? ColourCode { get; set; }
    
}