using System;
using API.RequestHelpers;
using core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    protected async Task<ActionResult> CreatePageResult<T>(IGenericRepository<T> repo,
     ISpecification<T> spec, int PageIndex, int PageSize) where T : BaseEntity
    {
        var items = await repo.ListAsync(spec);
        var Count = await repo.CountAsync(spec);

        var Pagination = new Pagination<T>(PageIndex, PageSize, Count, items);

        return Ok(Pagination);
    }
}
