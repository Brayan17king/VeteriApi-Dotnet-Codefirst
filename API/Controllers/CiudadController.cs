using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class CiudadController : BaseControllerApi
{
    private IUnitOfWork _unitOfWork;
    private IMapper _mapper;

    public CiudadController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<CiudadDto>>> Getm()
    {
        var ciudades = await _unitOfWork.Ciudades.GetAllAsync();

        return _mapper.Map<List<CiudadDto>>(ciudades);
    }

    [HttpGet("{Id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CiudadDto>> Get(int Id)
    {
        var ciudad = await _unitOfWork.Ciudades.GetByIdAsync(Id);
        if (ciudad == null)
        {
            return NotFound();
        }
        return _mapper.Map<CiudadDto>(ciudad);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Ciudad>> Post(CiudadDto ciudadDto)
    {
        var ciudad = _mapper.Map<Ciudad>(ciudadDto);
        _unitOfWork.Ciudades.Add(ciudad); //Agregar la informacioon que ya se encuentra en Pais
        await _unitOfWork.SaveAsync();
        if (ciudad == null) //Error en la peticion
        {
            return BadRequest();
        }
        ciudadDto.Id = ciudad.Id; // Asignar el Id que se genero a nuestro atributo Id
        return CreatedAtAction(nameof(Post), new { id = ciudadDto.Id }, ciudadDto); 
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CiudadDto>> Put(int id, [FromBody] CiudadDto ciudadDto)
    {
        var ciudades = _mapper.Map<Ciudad>(ciudadDto);
        if (ciudades.Id == 0)
        {
            ciudades.Id = id;
        }
        if (ciudades.Id != id)
        {
            return NotFound();
        }
        if(ciudadDto == null)
        return NotFound();

        ciudadDto.Id = ciudades.Id;

        _unitOfWork.Ciudades.Update(ciudades);
        await _unitOfWork.SaveAsync();
        return ciudadDto;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var ciudad = await _unitOfWork.Ciudades.GetByIdAsync(id);
        if (ciudad == null)
        {
            return NotFound();
        }
        _unitOfWork.Ciudades.Remove(ciudad);
        await _unitOfWork.SaveAsync();
        return NoContent();
    }
}
