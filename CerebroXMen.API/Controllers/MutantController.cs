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
public class MutantController(IMutantDetectorService detector, IDnaRepository repository) : ControllerBase
{
    private readonly IMutantDetectorService _detector = detector;
    private readonly IDnaRepository _repository = repository;

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
            IsMutant = isMutant,
            FechaAlta = DateTime.Now
        };

        await _repository.AddAsync(dna);

        return isMutant ? Ok() : Forbid(); // 403 si no es mutante
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        int mutants = await _repository.CountMutantsAsync();
        int humans = await _repository.CountHumansAsync();

        double ratio = humans == 0 ? 0 : (double)mutants / humans;

        var stats = new StatsResponse
        {
            CountMutantDna = mutants,
            CountHumanDna = humans,
            Ratio = Math.Round(ratio, 2)
        };

        return Ok(stats);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var allDna = await _repository.GetAllAsync();
        return Ok(allDna);
    }

    [HttpGet("allstring")]
    public IActionResult GetAllstring()
    {
        var allDna = _repository.GetAllJustStringAsync();
        return Ok(allDna);
    }
}