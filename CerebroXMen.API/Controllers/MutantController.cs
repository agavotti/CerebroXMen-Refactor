using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;
using System.Net;
using CerebroXMen.API.Models;
using CerebroXMen.Application.Services;
using CerebroXMen.Domain.Entities;
using CerebroXMen.Domain.Repositories;

namespace CerebroXMen.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MutantController : ControllerBase
{
    private readonly IMutantDetectorService _detector;
    private readonly IDnaRepository _repository;

    public MutantController(IMutantDetectorService detector, IDnaRepository repository)
    {
        _detector = detector;
        _repository = repository;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] DnaRequest request)
    {
        if (request.Dna == null || request.Dna.Length == 0)
            return BadRequest("Invalid DNA");

        var existing = await _repository.GetBySequenceAsync(request.Dna);
        if (existing != null)
        {
            return existing.IsMutant ? Ok() : Forbid();
        }

        bool isMutant = _detector.IsMutant(request.Dna);

        var dna = new DnaSequence
        {
            Sequence = request.Dna,
            IsMutant = isMutant
        };

        await _repository.AddAsync(dna);

        return isMutant ? Ok() : Forbid(); // 403 si no es mutante
    }
}