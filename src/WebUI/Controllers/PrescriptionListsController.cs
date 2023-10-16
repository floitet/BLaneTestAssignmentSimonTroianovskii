using BallastLaneTestAssignment.Application.Common.Interfaces;
using BallastLaneTestAssignment.Application.PrescriptionLists.Commands.CreatePrescriptionList;
using BallastLaneTestAssignment.Application.PrescriptionLists.Commands.DeletePrescriptionList;
using BallastLaneTestAssignment.Application.PrescriptionLists.Commands.UpdatePrescriptionList;
using BallastLaneTestAssignment.Application.PrescriptionLists.Queries.ExportPrescriptions;
using BallastLaneTestAssignment.Application.PrescriptionLists.Queries.GetPrescriptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[Authorize]
public class PrescriptionListsController : ApiControllerBase
{
    private readonly ICurrentUserService _currentUserService;

    public PrescriptionListsController(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    [HttpGet]
    public async Task<ActionResult<PrescriptionsVm>> Get()
    {
        return await Mediator.Send(new GetPrescriptionsQuery());
    }

    [HttpGet("{id}")]
    public async Task<FileResult> Get(int id)
    {
        var vm = await Mediator.Send(new ExportPrescriptionsQuery { ListId = id });

        return File(vm.Content, vm.ContentType, vm.FileName);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreatePrescriptionListCommand command)
    {
        var userId = _currentUserService.UserId;
        command.UserId = userId;
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Update(int id, UpdatePrescriptionListCommand command)
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

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeletePrescriptionListCommand(id));

        return NoContent();
    }
}
