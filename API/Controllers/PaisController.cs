using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class PaisController : BaseControllerApi
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PaisController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<PaisDto>>> Getm()
    {
        var paises = await _unitOfWork.Paises.GetAllAsync();

        return _mapper.Map<List<PaisDto>>(paises);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Pais>> Post(PaisDto paisDto)
    {
        var pais = _mapper.Map<Pais>(paisDto);
        _unitOfWork.Paises.Add(pais);
        await _unitOfWork.SaveAsync();
        if (pais == null)
        {
            return BadRequest(
            );
        }
        paisDto.Id = pais.Id;
        return CreatedAtAction(nameof(Post), new { id = paisDto.Id }, paisDto);
    }
}
