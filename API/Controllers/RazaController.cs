using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class RazaController : BaseControllerApi
{
    private IUnitOfWork _unitOfWork;
    private IMapper _mapper;

    public RazaController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<RazaDto>>> Getm()
    {
        var razas = await _unitOfWork.Razas.GetAllAsync();

        return _mapper.Map<List<RazaDto>>(razas);
    }

    [HttpGet("{Id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RazaDto>> Get(int Id)
    {
        var raza = await _unitOfWork.Razas.GetByIdAsync(Id);
        if (raza == null)
        {
            return NotFound();
        }
        return _mapper.Map<RazaDto>(raza);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Raza>> Post(RazaDto razaDto)
    {
        var raza = _mapper.Map<Raza>(razaDto);
        _unitOfWork.Razas.Add(raza); //Agregar la informacioon que ya se encuentra en Pais
        await _unitOfWork.SaveAsync();
        if (raza == null) //Error en la peticion
        {
            return BadRequest();
        }
        razaDto.Id = raza.Id; // Asignar el Id que se genero a nuestro atributo Id
        return CreatedAtAction(nameof(Post), new { id = razaDto.Id }, razaDto); 
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RazaDto>> Put(int id, [FromBody] RazaDto razaDto)
    {
        var razas = _mapper.Map<Raza>(razaDto);
        if (razas.Id == 0)
        {
            razas.Id = id;
        }
        if (razas.Id != id)
        {
            return NotFound();
        }
        if(razaDto == null)
        return NotFound();

        razaDto.Id = razas.Id;

        _unitOfWork.Razas.Update(razas);
        await _unitOfWork.SaveAsync();
        return razaDto;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var raza = await _unitOfWork.Razas.GetByIdAsync(id);
        if (raza == null)
        {
            return NotFound();
        }
        _unitOfWork.Razas.Remove(raza);
        await _unitOfWork.SaveAsync();
        return NoContent();
    }
}
