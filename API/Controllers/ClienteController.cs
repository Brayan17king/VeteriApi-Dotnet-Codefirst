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

public class ClienteController : BaseControllerApi
{
    private IUnitOfWork _unitOfWork;
    private IMapper _mapper;

    public ClienteController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ClienteDto>>> Getm()
    {
        var clientes = await _unitOfWork.Clientes.GetAllAsync();

        return _mapper.Map<List<ClienteDto>>(clientes);
    }

    [HttpGet("{Id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClienteDto>> Get(int Id)
    {
        var cliente = await _unitOfWork.Clientes.GetByIdAsync(Id);
        if (cliente == null)
        {
            return NotFound();
        }
        return _mapper.Map<ClienteDto>(cliente);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Cliente>> Post(ClienteDto clienteDto)
    {
        var cliente = _mapper.Map<Cliente>(clienteDto);
        _unitOfWork.Clientes.Add(cliente); //Agregar la informacioon que ya se encuentra en Pais
        await _unitOfWork.SaveAsync();
        if (cliente == null) //Error en la peticion
        {
            return BadRequest();
        }
        clienteDto.Id = cliente.Id; // Asignar el Id que se genero a nuestro atributo Id
        return CreatedAtAction(nameof(Post), new { id = clienteDto.Id }, clienteDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClienteDto>> Put(int id, [FromBody] ClienteDto clienteDto)
    {
        var cliente = _mapper.Map<Cliente>(clienteDto);
        if (cliente.Id == 0)
        {
            cliente.Id = id;
        }
        if (cliente.Id != id)
        {
            return NotFound();
        }
        if (clienteDto == null)
            return NotFound();

        clienteDto.Id = cliente.Id;

        _unitOfWork.Clientes.Update(cliente);
        await _unitOfWork.SaveAsync();
        return clienteDto;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var cliente = await _unitOfWork.Clientes.GetByIdAsync(id);
        if (cliente == null)
        {
            return NotFound();
        }
        _unitOfWork.Clientes.Remove(cliente);
        await _unitOfWork.SaveAsync();
        return NoContent();
    }
}
