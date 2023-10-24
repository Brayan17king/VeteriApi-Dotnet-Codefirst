# Creation of EF Code First SQL

- [Project Creation](#Project-creation)
  
  1. [Creacion de Solución](#creaci%C3%B3n-de-soluci%C3%B3n)
  
  2. [Creacion proyecto ClassLib](#Creacion-proyecto-ClassLib)
  
  3. [Creacion proyecto WebApi](#Creacion-Proyecto-WebApi)
  
  4. [Agregar proyectos a la solucion](#Agregar-proyectos-a-la-solucion)
  
  5. [Agregar referencia entre Proyectos](#Agregar-referencia-entre-Proyectos)
  
  6. [Realizar la Migracion](#Migración-del-Proyecto)

- [Instalacion de Paquetes](#Instalacion-de-paquetes)
  
  - [WebApi](#Proyecto-WebApi)
  
  - [Infrastructure](#Proyecto-Infrastructure)

- [API](#API)
  
  - [Controllers](#Controller-Layout)
    
    - [ExampleController.cs](#Controller-Layout)
  
  - [Dtos](#Dtos)
    
    - [ExampleDto.cs](#Dtos)
  
  - [Extensions](#Extensions)
    
    - [ApplicationServicesExtension.cs](#ApplicationServicesExtension)
  
  - [Helper](#Helper)
    
    - [Pager.cs](#Pager)
    
    - [Params.cs](#Params)
  
  - [Program.cs](#Program)

- [Core](#Core)
  
  - [Interfaces](#Interfaces)
    
    - [IGenericRepository.cs](#IGenericRepository)
    
    - [IUnitOfWork.cs](#IUnitOfWork)

- [Infrastructure](#Infrastructure)
  
  - [Data](#Data)
    
    - [Configuration](#Configuration)
      
      - [ExampleConfiguration.cs](#Configuration)
    
    - [DbContext.cs](#DbContext)
  
  - [Repositories](#Repositories)
    
    - [GenericRepository.cs](#GenericRepository)
    
    - [SomeRepository.cs](#GenericRepository)
  
  - [UnitOfWork](#UnitOfWork)
    
    - [UnitOfWork.cs](#UnitOfWork)

- [Migracion del Proyecto](#Migración-del-Proyecto)
  
  - [Migracion](#Migración)
  
  - [Actualización](#Actualizacion)

## Project creation

# Creación de Solución

```bash
dotnet new sln
```

# Creacion proyecto ClassLib

```bash
dotnet new classlib -o Core
dotnet new classlib -o Infrastructure
```

# Creacion Proyecto WebApi

```bash
dotnet new webapi -o FolderDestino
```

El folder destino corresponde a la carpeta donde se creara el proyecto. Se recomienda que el nombre tenga la palabra Api Por ej. ApiAnimals.

# Agregar proyectos a la solucion

```dotnet
dotnet sln add API
dotnet sln add Core
dotnet sln add Infrastructure
```

# Agregar referencia entre Proyectos

Nota. Recuerde que las referencias se establecen desde el proyecto que contiene la referencia

```bash
dotnet add reference ..\Infrastructure
dotnet add reference ..\Core
```

# Instalacion de paquetes

## Proyecto WebApi

```bash
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 7.0.11
dotnet add package Microsoft.EntityFrameworkCore --version 7.0.11
dotnet add package Microsoft.EntityFrameworkCore.Design --version 7.0.11
dotnet add package Microsoft.Extensions.DependencyInjection --version 7.0.0
dotnet add package System.IdentityModel.Tokens.Jwt --version 6.32.3
dotnet add package Serilog.AspNetCore --version 7.0.0
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 12.0.1
```

## Proyecto Infrastructure

```bash
dotnet add package Pomelo.EntityFrameworkCore.MySql --version 7.0.0
dotnet add package Microsoft.EntityFrameworkCore --version 7.0.11
dotnet add package CsvHelper --version 30.0.1
```

#### Directories

https://i.imgur.com/ISz0gY6.png

## API

### Controller Layout

```dotnet
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
```

### Dtos

```dotnet
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class CitaDto
    {
        public int Id { get; set; }
        public DateOnly FechaCita { get; set; }
        public TimeOnly HoraCita { get; set; }
        public int IdClienteFk { get; set; }
        public int IdMascotaFk { get; set; }
        public int IdServicioFk { get; set; }
    }
}
```

## Extensions

### ApplicationServicesExtension

```dotnet
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreRateLimit;
using Core.Interfaces;
using Infrastructure.UnitOfWork;

namespace API.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static void ConfigureCors(this IServiceCollection services) => services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin() // WithOrigins("https://domain.com")
                .AllowAnyMethod() // WithMethods("GET", "POST")
                .AllowAnyHeader(); // WithHeaders("accept", "content-type")
            });
        }); // Remember to put 'static' on the class and to add builder.Services.ConfigureCors(); and app.UseCors("CorsPolicy"); to Program.cs

        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        } // Remember to add builder.Services.AddApplicationServices(); to Program.cs

        public static void ConfigureRateLimiting(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddInMemoryRateLimiting();
            services.Configure<IpRateLimitOptions>(options =>
            {
                options.EnableEndpointRateLimiting = true;
                options.StackBlockedRequests = false;
                options.HttpStatusCode = 429;
                options.RealIpHeader = "X-Real-IP";
                options.GeneralRules = new List<RateLimitRule>
                {
                    new RateLimitRule
                    {
                        Endpoint = "*",  // Si quiere usar todos ponga *
                        Period = "10s", // Periodo de tiempo para hacer peticiones
                        Limit = 2       // Numero de peticiones durante el periodo de tiempo
                    }
                };
            });
        } // Remember adding builder.Services.ConfigureRateLimiting(); and builder.Services.AddAutoMapper(Assembly.GetEntryAssembly()); and app.UseRateLimiting(); to Program.cs
    }
}
```

### Profiles

#### MappingProfiles

```dotnet
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities;

namespace API.Profiles
{
    public class MappingProfiles : Profile //hereda de profile
    {
        public MappingProfiles()
        {
            CreateMap<Pais, PaisDto>().ReverseMap();
            CreateMap<Ciudad, CiudadDto>().ReverseMap();
            CreateMap<ClienteDireccion, ClienteDireccionDto>().ReverseMap();
            CreateMap<Cita, CitaDto>().ReverseMap();
            CreateMap<Cliente, ClienteDto>().ReverseMap();
            CreateMap<ClienteTelefono, ClienteTelefonoDto>().ReverseMap();
            CreateMap<Departamento, DepartamentoDto>().ReverseMap();
            CreateMap<Mascota, MascotaDto>().ReverseMap();
            CreateMap<Raza, RazaDto>().ReverseMap();
            CreateMap<Servicio, ServicioDto>().ReverseMap();

        }
    }
}
```

### Helper

#### Pager

```dotnet
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers;

public class Pager<T> where T : class
{
    public string Search { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int Total { get; set; }
    public List<T> Registers { get; private set; }

    public Pager()
    {
    }

    public Pager(List<T> registers, int total, int pageIndex, int pageSize, string search)
    {
        Registers = registers;
        Total = total;
        PageIndex = pageIndex;
        PageSize = pageSize;
        Search = search;
    }

    public int TotalPages
    {
        get { return (int)Math.Ceiling(Total / (double)PageSize); }
        set { this.TotalPages = value; }
    }

    public bool HasPreviousPage
    {
        get { return (PageIndex > 1); }
        set { this.HasPreviousPage = value; }
    }

    public bool HasNextPage
    {
        get { return (PageIndex < TotalPages); }
        set { this.HasNextPage = value; }
    }
}
```

### Params

```dotnet
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers;

public class Params
{
    private int _pageSize = 5;
    private const int MaxPageSize = 50;
    private int _pageIndex = 1;
    private string _search;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value <= MaxPageSize) ? MaxPageSize : value;
    }

    public int PageIndex
    {
        get => _pageIndex;
        set => _pageIndex = (value <= 0) ? 1 : value;
    }


    public string Search
    {
        get => _search;
        set => _search = (!String.IsNullOrEmpty(value)) ? value.ToLower() : "";
    }



}

```

### Program

```dotnet
using System.Reflection;
using API.Extensions;
using AspNetCoreRateLimit;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.ConfigureRatelimiting();
builder.Services.AddAutoMapper(Assembly.GetEntryAssembly());
builder.Services.ConfigureCors();
builder.Services.AddApplicationServices();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AnimalsContext>(optionsBuilder =>
{
    string connectionString = builder.Configuration.GetConnectionString("MySqlConex");
    optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");
app.UseIpRateLimiting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

```

## Core

### Entities

#### BaseEntity

```dotnet
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities;

public class BaseEntity
{
    public int Id { get; set; }
}

```

#### CitaEntity

```dotnet
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities;
public class Cita : BaseEntity
{
[Required]
        public DateOnly FechaCita { get; set; }

        [Required]
        public TimeOnly HoraCita { get; set; }

        [Required]
        public int IdClienteFk { get; set; }
        public Cliente Clientes { get; set; }
        [Required]
        public int IdMascotaFk { get; set; }
        public Mascota Mascotas { get; set; }
        [Required]
        public int IdServicioFk { get; set; }
        public Servicio Servicios { get; set; } 
    
}
```

### Interfaces

#### IGenericRepository

```dotnet
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T> GetByIdAsync(int Id);
    Task<IEnumerable<T>> GetAllAsync();
    IEnumerable<T> Find(Expression<Func<T, bool>> expression);
    Task<(int totalRegistros, IEnumerable<T> registros)> GetAllAsync(int pageIndex, int pageSize, string search);
    void Add(T entity);
    void AddRange(IEnumerable<T> entities);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    void Update(T entity);
} 
```

#### IUnitOfWork

```dotnet
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUnitOfWork
    {
        public IPaisRepository Paises { get; }
        public ICitaRepository Citas { get; }
        public ICiudadRepository Ciudades { get; }
        public IClienteRepository Clientes { get; }
        public IClienteDireccionRepository ClienteDirecciones { get; }
        public IClienteTelefonoRepository ClienteTelefonos { get; }
        public IDepartamentoRepository Departamentos { get; }
        public IMascotaRepository Mascotas { get; }
        public IRazaRepository Razas { get; }
        public IServicioRepository Servicios { get; }
        Task<int> SaveAsync();
    }
}
```

## Infrastructure

### Data

#### AnimalsContext

```dotnet
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data; // facilita el proceso de llamado de la clase

public class AnimalsContext : DbContext
{
    public AnimalsContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Cita> Citas { get; set; }
    public DbSet<Ciudad> Ciudades { get; set; }
    public DbSet<ClienteDireccion> ClienteDirecciones { get; set; }
    public DbSet<ClienteTelefono> ClienteTelefonos { get; set; }
    public DbSet<Departamento> Departamentos { get; set; }
    public DbSet<Mascota> Mascotas { get; set; }
    public DbSet<Pais> Paises { get; set; }
    public DbSet<Raza> Razas { get; set; }
    public DbSet<Servicio> Servicios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

```

### Configuration

#### CitaConfiguration

```dotnet
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration;

public class CitaConfiguration : IEntityTypeConfiguration<Cita>
{
    public void Configure(EntityTypeBuilder<Cita> builder)
    {
        //Here you can configure the properties using the object 'Builder'.
        builder.ToTable("cita");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id);

        builder.Property(c => c.FechaCita).HasColumnType("date");

        builder.Property(c => c.HoraCita).HasColumnType("time");

        builder.HasOne(c => c.Clientes).WithMany(c => c.Citas).HasForeignKey(c => c.IdClienteFk);
        builder.HasOne(c => c.Mascotas).WithMany(c => c.Citas).HasForeignKey(c => c.IdMascotaFk);
        builder.HasOne(c => c.Servicios).WithMany(c => c.Citas).HasForeignKey(c => c.IdServicioFk);
    }
}
```

### Repositories

#### GenericRepository

```dotnet
using System.Linq.Expressions;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly AnimalsContext _context;

    public GenericRepository(AnimalsContext context)
    {
        _context = context;
    }

    public virtual void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public virtual void AddRange(IEnumerable<T> entities)
    {
        _context.Set<T>().AddRange(entities);
    }

    public virtual IEnumerable<T> Find(Expression<Func<T, bool>> expression)
    {
        return _context.Set<T>().Where(expression);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
        // return (IEnumerable<T>) await _context.Entities.FromSqlRaw("SELECT * FROM entity").ToListAsync();
    }

    public virtual async Task<T> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public virtual Task<T> GetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public virtual void Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public virtual void RemoveRange(IEnumerable<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
    }

    public virtual void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }
    public virtual async Task<(int totalRegistros, IEnumerable<T> registros)> GetAllAsync(
        int pageIndex,
        int pageSize,
        string _search
    )
    {
        var totalRegistros = await _context.Set<T>().CountAsync();
        var registros = await _context
            .Set<T>()
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (totalRegistros, registros);
    }
}
```

#### CitaRepository

```dotnet
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class CitaRepository : GenericRepository<Cita> ,ICitaRepository
    {
        private readonly AnimalsContext _context;

        public CitaRepository(AnimalsContext context) : base(context)
        {
           _context = context;
        }
    }
}
```

### UnitOfWork

```dotnet
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly NotiAppContext _context;
        public AuditoriaRepository _Auditorias { get; set; }
        public BlockchainRepository _Blockchains { get; set; }
        public EstadoNotificacionRepository _EstadoNotificaciones { get; set; }
        public FormatoRepository _Formatos { get; set; }
        public GenericovsSubmoduloRepository _GenericovsSubmodulos { get; set; }
        public HiloRespuestaNotificacionRepository _HiloRespuestaNotificaciones { get; set; }
        public MaestrovsSubmoduloRepository _MaestrovsSubmodulos { get; set; }
        public ModuloMaestroRepository _ModuloMaestros { get; set; }
        public ModuloNotificacionRepository _ModuloNotificaciones { get; set; }
        public PermisosGenericoRepository _PermisosGenericos { get; set; }
        public RadicadoRepository _Radicados { get; set; }
        public RolRepository _Roles { get; set; }
        public RolvsMaestroRepository _RolvsMaestros { get; set; }
        public SubmoduloRepository _Submodulos { get; set; }
        public TipoNotificacionRepository _TipoNotificaciones { get; set; }
        public TipoRequerimientoRepository _TipoRequerimientos { get; set; }

        public IAuditoria Auditorias
        {
            get
            {
                if (_Auditorias == null)
                {
                    _Auditorias = new AuditoriaRepository(_context);
                }
                return _Auditorias;
            }
        }

        public IBlockchain Blockchains
        {
            get
            {
                if (_Blockchains == null)
                {
                    _Blockchains = new BlockchainRepository(_context);
                }
                return _Blockchains;
            }
        }

        public IEstadoNotificacion EstadoNotificaciones
        {
            get
            {
                if (_EstadoNotificaciones == null)
                {
                    _EstadoNotificaciones = new EstadoNotificacionRepository(_context);
                }
                return _EstadoNotificaciones;
            }
        }

        public IFormato Formatos
        {
            get
            {
                if (_Formatos == null)
                {
                    _Formatos = new FormatoRepository(_context);
                }
                return _Formatos;
            }
        }

        public IGenericovsSubmodulo GenericovsSubmodulos
        {
            get
            {
                if (_GenericovsSubmodulos == null)
                {
                    _GenericovsSubmodulos = new GenericovsSubmoduloRepository(_context);
                }
                return _GenericovsSubmodulos;
            }
        }

        public IHiloRespuestaNotificacion HiloRespuestaNotificaciones
        {
            get
            {
                if (_HiloRespuestaNotificaciones == null)
                {
                    _HiloRespuestaNotificaciones = new HiloRespuestaNotificacionRepository(_context);
                }
                return _HiloRespuestaNotificaciones;
            }
        }

        public IMaestrovsSubmodulo MaestrovsSubmodulos
        {
            get
            {
                if (_MaestrovsSubmodulos == null)
                {
                    _MaestrovsSubmodulos = new MaestrovsSubmoduloRepository(_context);
                }
                return _MaestrovsSubmodulos;
            }
        }

        public IModuloMaestro ModuloMaestros
        {
            get
            {
                if (_ModuloMaestros == null)
                {
                    _ModuloMaestros = new ModuloMaestroRepository(_context);
                }
                return _ModuloMaestros;
            }
        }

        public IModuloNotificacion ModuloNotificaciones
        {
            get
            {
                if (_ModuloNotificaciones == null)
                {
                    _ModuloNotificaciones = new ModuloNotificacionRepository(_context);
                }
                return _ModuloNotificaciones;
            }
        }

        public IPermisosGenerico PermisosGenericos
        {
            get
            {
                if (_PermisosGenericos == null)
                {
                    _PermisosGenericos = new PermisosGenericoRepository(_context);
                }
                return _PermisosGenericos;
            }
        }

        public IRadicado Radicados
        {
            get
            {
                if (_Radicados == null)
                {
                    _Radicados = new RadicadoRepository(_context);
                }
                return _Radicados;
            }
        }

        public IRol Roles
        {
            get
            {
                if (_Roles == null)
                {
                    _Roles = new RolRepository(_context);
                }
                return _Roles;
            }
        }

        public IRolvsMaestro RolvsMaestros
        {
            get
            {
                if (_RolvsMaestros == null)
                {
                    _RolvsMaestros = new RolvsMaestroRepository(_context);
                }
                return _RolvsMaestros;
            }
        }

        public ISubmodulo Submodulos
        {
            get
            {
                if (_Submodulos == null)
                {
                    _Submodulos = new SubmoduloRepository(_context);
                }
                return _Submodulos;
            }
        }

        public ITipoNotificacion TipoNotificaciones
        {
            get
            {
                if (_TipoNotificaciones == null)
                {
                    _TipoNotificaciones = new TipoNotificacionRepository(_context);
                }
                return _TipoNotificaciones;
            }
        }

        public ITipoRequerimiento TipoRequerimientos
        {
            get
            {
                if (_TipoRequerimientos == null)
                {
                    _TipoRequerimientos = new TipoRequerimientoRepository(_context);
                }
                return _TipoRequerimientos;
            }
        }

        public UnitOfWork(NotiAppContext context)
        {
            _context = context;
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
```

# Migración del Proyecto

## Migración

```bash
dotnet ef migrations add InitialCreate --project ./Infraestructure/ --startup-project ./API/ --output-dir ./Data/Migrations
```

## Actualización

```bash
dotnet ef database update --project ./Infrastructure/ --startup-project ./API/
```