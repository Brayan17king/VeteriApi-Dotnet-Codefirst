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

public class ServicioController : BaseControllerApi
{
    private IUnitOfWork _unitOfWork;
    private IMapper _mapper;

    public ServicioController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ServicioDto>>> Getm()
    {
        var servicio = await _unitOfWork.Servicios.GetAllAsync();

        return _mapper.Map<List<ServicioDto>>(servicio);
    }

    [HttpGet("{Id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ServicioDto>> Get(int Id)
    {
        var servicio = await _unitOfWork.Servicios.GetByIdAsync(Id);
        if (servicio == null)
        {
            return NotFound();
        }
        return _mapper.Map<ServicioDto>(servicio);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Servicio>> Post(ServicioDto servicioDto)
    {
        var servicio = _mapper.Map<Servicio>(servicioDto);
        _unitOfWork.Servicios.Add(servicio); //Agregar la informacioon que ya se encuentra en Pais
        await _unitOfWork.SaveAsync();
        if (servicio == null) //Error en la peticion
        {
            return BadRequest();
        }
        servicioDto.Id = servicio.Id; // Asignar el Id que se genero a nuestro atributo Id
        return CreatedAtAction(nameof(Post), new { id = servicioDto.Id }, servicioDto); 
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ServicioDto>> Put(int id, [FromBody] ServicioDto servicioDto)
    {
        var servicio = _mapper.Map<Servicio>(servicioDto);
        if (servicio.Id == 0)
        {
            servicio.Id = id;
        }
        if (servicio.Id != id)
        {
            return NotFound();
        }
        if(servicioDto == null)
        return NotFound();

        servicioDto.Id = servicio.Id;

        _unitOfWork.Servicios.Update(servicio);
        await _unitOfWork.SaveAsync();
        return servicioDto;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var servicio = await _unitOfWork.Servicios.GetByIdAsync(id);
        if (servicio == null)
        {
            return NotFound();
        }
        _unitOfWork.Servicios.Remove(servicio);
        await _unitOfWork.SaveAsync();
        return NoContent();
    }
}
