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

public class MascotaController : BaseControllerApi
{
    private IUnitOfWork _unitOfWork;
    private IMapper _mapper;

    public MascotaController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<MascotaDto>>> Getm()
    {
        var mascota = await _unitOfWork.Mascotas.GetAllAsync();

        return _mapper.Map<List<MascotaDto>>(mascota);
    }

    [HttpGet("{Id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MascotaDto>> Get(int Id)
    {
        var mascota = await _unitOfWork.Mascotas.GetByIdAsync(Id);
        if (mascota == null)
        {
            return NotFound();
        }
        return _mapper.Map<MascotaDto>(mascota);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Mascota>> Post(MascotaDto mascotaDto)
    {
        var mascota = _mapper.Map<Mascota>(mascotaDto);
        if (mascotaDto.FechaNacimientoMascota == DateTime.MinValue)
        {
            mascotaDto.FechaNacimientoMascota = DateTime.Now;
        }
        _unitOfWork.Mascotas.Add(mascota); //Agregar la informacioon que ya se encuentra en Pais
        await _unitOfWork.SaveAsync();
        if (mascota == null) //Error en la peticion
        {
            return BadRequest();
        }
        mascotaDto.Id = mascota.Id; // Asignar el Id que se genero a nuestro atributo Id
        return CreatedAtAction(nameof(Post), new { id = mascotaDto.Id }, mascotaDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MascotaDto>> Put(int id, [FromBody] MascotaDto mascotaDto)
    {
        var mascotas = _mapper.Map<Mascota>(mascotaDto);
        if (mascotas.Id == 0)
        {
            mascotas.Id = id;
        }
        if (mascotas.Id != id)
        {
            return NotFound();
        }
        if (mascotaDto == null)
            return NotFound();

        mascotaDto.Id = mascotas.Id;

        _unitOfWork.Mascotas.Update(mascotas);
        await _unitOfWork.SaveAsync();
        return mascotaDto;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var mascota = await _unitOfWork.Mascotas.GetByIdAsync(id);
        if (mascota == null)
        {
            return NotFound();
        }
        _unitOfWork.Mascotas.Remove(mascota);
        await _unitOfWork.SaveAsync();
        return NoContent();
    }
}
