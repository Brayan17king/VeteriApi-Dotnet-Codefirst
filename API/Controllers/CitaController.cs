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

public class CitaController : BaseControllerApi
{
    private IUnitOfWork _unitOfWork;
    private IMapper _mapper;

    public CitaController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<CitaDto>>> Getm()
    {
        var citas = await _unitOfWork.Citas.GetAllAsync();

        return _mapper.Map<List<CitaDto>>(citas);
    }

    [HttpGet("{Id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CitaDto>> Get(int Id)
    {
        var cita = await _unitOfWork.Citas.GetByIdAsync(Id);
        if (cita == null)
        {
            return NotFound();
        }
        return _mapper.Map<CitaDto>(cita);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Cita>> Post(CitaDto citaDto)
    {
        var cita = _mapper.Map<Cita>(citaDto);
        _unitOfWork.Citas.Add(cita); //Agregar la informacioon que ya se encuentra en Pais
        await _unitOfWork.SaveAsync();
        if (cita == null) //Error en la peticion
        {
            return BadRequest();
        }
        citaDto.Id = cita.Id; // Asignar el Id que se genero a nuestro atributo Id
        return CreatedAtAction(nameof(Post), new { id = citaDto.Id }, citaDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CitaDto>> Put(int id, [FromBody] CitaDto citaDto)
    {
        var citas = _mapper.Map<Cita>(citaDto);
        if (citas.Id == 0)
        {
            citas.Id = id;
        }
        if (citas.Id != id)
        {
            return NotFound();
        }
        if (citaDto == null)
            return NotFound();

        citaDto.Id = citas.Id;

        _unitOfWork.Citas.Update(citas);
        await _unitOfWork.SaveAsync();
        return citaDto;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var cita = await _unitOfWork.Citas.GetByIdAsync(id);
        if (cita == null)
        {
            return NotFound();
        }
        _unitOfWork.Citas.Remove(cita);
        await _unitOfWork.SaveAsync();
        return NoContent();
    }
}
