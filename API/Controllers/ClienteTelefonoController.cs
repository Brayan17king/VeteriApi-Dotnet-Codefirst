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

public class ClienteTelefonoController : BaseControllerApi
{
    private IUnitOfWork _unitOfWork;
    private IMapper _mapper;

    public ClienteTelefonoController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ClienteTelefonoDto>>> Getm()
    {
        var clienteTel = await _unitOfWork.ClienteTelefonos.GetAllAsync();

        return _mapper.Map<List<ClienteTelefonoDto>>(clienteTel);
    }

    [HttpGet("{Id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClienteTelefonoDto>> Get(int Id)
    {
        var clienteTel = await _unitOfWork.ClienteTelefonos.GetByIdAsync(Id);
        if (clienteTel == null)
        {
            return NotFound();
        }
        return _mapper.Map<ClienteTelefonoDto>(clienteTel);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ClienteTelefono>> Post(ClienteTelefonoDto clienteTelefonoDto)
    {
        var clienteTel = _mapper.Map<ClienteTelefono>(clienteTelefonoDto);
        _unitOfWork.ClienteTelefonos.Add(clienteTel); //Agregar la informacioon que ya se encuentra en Pais
        await _unitOfWork.SaveAsync();
        if (clienteTel == null) //Error en la peticion
        {
            return BadRequest();
        }
        clienteTelefonoDto.Id = clienteTel.Id; // Asignar el Id que se genero a nuestro atributo Id
        return CreatedAtAction(nameof(Post), new { id = clienteTelefonoDto.Id }, clienteTelefonoDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClienteTelefonoDto>> Put(int id, [FromBody] ClienteTelefonoDto clienteTelefonoDto)
    {
        var clienteTel = _mapper.Map<ClienteTelefono>(clienteTelefonoDto);
        if (clienteTel.Id == 0)
        {
            clienteTel.Id = id;
        }
        if (clienteTel.Id != id)
        {
            return NotFound();
        }
        if (clienteTelefonoDto == null)
            return NotFound();

        clienteTelefonoDto.Id = clienteTel.Id;

        _unitOfWork.ClienteTelefonos.Update(clienteTel);
        await _unitOfWork.SaveAsync();
        return clienteTelefonoDto;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var clienteTel = await _unitOfWork.ClienteTelefonos.GetByIdAsync(id);
        if (clienteTel == null)
        {
            return NotFound();
        }
        _unitOfWork.ClienteTelefonos.Remove(clienteTel);
        await _unitOfWork.SaveAsync();
        return NoContent();
    }
}
