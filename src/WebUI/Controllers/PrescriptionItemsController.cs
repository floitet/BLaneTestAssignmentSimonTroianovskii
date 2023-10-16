using BallastLaneTestAssignment.Application.Common.Interfaces;
using BallastLaneTestAssignment.Application.Common.Models;
using BallastLaneTestAssignment.Application.PrescriptionItems.Commands.CreatePrescriptionItem;
using BallastLaneTestAssignment.Application.PrescriptionItems.Commands.DeletePrescriptionItem;
using BallastLaneTestAssignment.Application.PrescriptionItems.Commands.UpdatePrescriptionItem;
using BallastLaneTestAssignment.Application.PrescriptionItems.Commands.UpdatePrescriptionItemDetail;
using BallastLaneTestAssignment.Application.PrescriptionItems.Queries.GetPrescriptionItemsWithPagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[Authorize]
public class PrescriptionItemsController : ApiControllerBase
{
    private readonly ICurrentUserService _currentUserService;

    public PrescriptionItemsController(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<PrescriptionItemBriefDto>>> GetPrescriptionItemsWithPagination([FromQuery] GetPrescriptionItemsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreatePrescriptionItemCommand command)
    {
        var userId = _currentUserService.UserId;
        command.CreatedBy = userId;
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Update(int id, UpdatePrescriptionItemCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }
        
        var userId = _currentUserService.UserId;
        command.UserId = userId;

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("[action]")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> UpdateItemDetails(int id, UpdatePrescriptionItemDetailCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeletePrescriptionItemCommand(id));

        return NoContent();
    }
}
