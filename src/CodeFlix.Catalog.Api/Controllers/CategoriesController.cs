using System.Net;
using CodeFlix.Catalog.Api.ApiModels.Category;
using CodeFlix.Catalog.Api.ApiModels.Response;
using CodeFlix.Catalog.Application.UseCases.Category.Common;
using CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;
using CodeFlix.Catalog.Application.UseCases.Category.DeleteCategory;
using CodeFlix.Catalog.Application.UseCases.Category.GetCategory;
using CodeFlix.Catalog.Application.UseCases.Category.ListCategories;
using CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory;
using CodeFlix.Catalog.Domain.Domain.SeedWork.SearchableRepository;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CodeFlix.Catalog.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CategoryModelOutput>), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.UnprocessableEntity)]

    public async Task<IActionResult> Create(
        [FromBody] CreateCategoryInput input,
        CancellationToken cancellationToken)
    {
        var output = await _mediator.Send(input, cancellationToken);
        var response = new ApiResponse<CategoryModelOutput>(output);
        return CreatedAtAction(
            nameof(Create),
            new { output.Id },
            response);
    }

    //»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<CategoryModelOutput>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Get(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {

        var output = await _mediator.Send(new GetCategoryInput(id), cancellationToken);
        var response = new ApiResponse<CategoryModelOutput>(output);
        return Ok(response);
    }
    //»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(IEnumerable<CategoryModelOutput>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCategoryInput(id), cancellationToken);
        return NoContent();
    }
    //»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<CategoryModelOutput>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.UnprocessableEntity)]
    public async Task<IActionResult> Update(
        [FromBody] UpdateCategoryApiInput inputApi,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var input = new UpdateCategoryInput(
            id,
            inputApi.Name,
            inputApi.Description,
            inputApi.IsActive);

        var output = await _mediator.Send(input, cancellationToken);
        var response = new ApiResponse<CategoryModelOutput>(output);
        return Ok(response);
    }
    //»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»
    [HttpGet]
    [ProducesResponseType(typeof(CategoryModelOutput), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> List(
        CancellationToken cancellationToken,
        [FromQuery] int? page = null,
        [FromQuery] int? perPage = null,
        [FromQuery] string? search = "",
        [FromQuery] string? sort = "",
        [FromQuery] SearchOrder? dir = null)
    {
        var input = new ListCategoriesInput();
        if (page is not null) input.Page = page.Value;
        if (perPage is not null) input.PerPage = perPage.Value;
        if (!string.IsNullOrWhiteSpace(search)) input.Search = search;
        if (!string.IsNullOrWhiteSpace(sort)) input.Sort = sort;
        if (dir is not null) input.Dir = dir.Value;

        var output = await _mediator.Send(input, cancellationToken);
        var response = new ApiResponseList<CategoryModelOutput>(output);

        return Ok(response);
    }
}
