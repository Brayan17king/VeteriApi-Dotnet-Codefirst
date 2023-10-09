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

public class DepartamentoController : BaseControllerApi
{
    private IUnitOfWork _unitOfWork;
    private IMapper _mapper;

    public DepartamentoController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<DepartamentoDto>>> Getm()
    {
        var departamentos = await _unitOfWork.Departamentos.GetAllAsync();

        return _mapper.Map<List<DepartamentoDto>>(departamentos);
    }

    [HttpGet("{Id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DepartamentoDto>> Get(int Id)
    {
        var departamento = await _unitOfWork.Departamentos.GetByIdAsync(Id);
        if (departamento == null)
        {
            return NotFound();
        }
        return _mapper.Map<DepartamentoDto>(departamento);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Departamento>> Post(DepartamentoDto departamentoDto)
    {
        var departamento = _mapper.Map<Departamento>(departamentoDto);
        _unitOfWork.Departamentos.Add(departamento); //Agregar la informacioon que ya se encuentra en Pais
        await _unitOfWork.SaveAsync();
        if (departamento == null) //Error en la peticion
        {
            return BadRequest();
        }
        departamentoDto.Id = departamento.Id; // Asignar el Id que se genero a nuestro atributo Id
        return CreatedAtAction(nameof(Post), new { id = departamentoDto.Id }, departamentoDto); 
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DepartamentoDto>> Put(int id, [FromBody] DepartamentoDto departamentoDto)
    {
        var departamentos = _mapper.Map<Departamento>(departamentoDto);
        if (departamentos.Id == 0)
        {
            departamentos.Id = id;
        }
        if (departamentos.Id != id)
        {
            return NotFound();
        }
        if(departamentoDto == null)
        return NotFound();

        departamentoDto.Id = departamentos.Id;

        _unitOfWork.Departamentos.Update(departamentos);
        await _unitOfWork.SaveAsync();
        return departamentoDto;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var departamento = await _unitOfWork.Departamentos.GetByIdAsync(id);
        if (departamento == null)
        {
            return NotFound();
        }
        _unitOfWork.Departamentos.Remove(departamento);
        await _unitOfWork.SaveAsync();
        return NoContent();
    }
}
