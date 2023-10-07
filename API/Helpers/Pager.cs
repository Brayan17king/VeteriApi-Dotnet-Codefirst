using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace API.Helpers;

//Me recibe cualquier entidad o clase para paginar...
public class Pager<T> where T : class
{
    //Definimos unas variables que me van a permitir poder especificar:
    public string Search { get; set; } // Una palabra de busqueda
    public int PageIndex { get; set; } // Pagina actual
    public int PageSize { get; set; } // Me permite definir la cantidad de registros 
    public int Total { get; set; } // Total de registros
    public List<T> Registers { get; private set; } // Lista generica de registros

    public Pager(){}

    public Pager(List<T> registers, int total, int pageIndex, int pageSize, string search)
    {
        Registers = registers;
        Total = total;
        PageIndex = pageIndex;
        PageSize = pageSize;
        Search = search;
    }

    //Operaciones matematicas que me permiten calcular la cantidad de registros.
    public int TotalPages
    {
        get {return(int)Math.Ceiling(Total / (double)PageSize);}
        set {this.TotalPages = value;}
    }

    public bool HasPreviousPage
    {
        get{return(PageIndex > 1);}
        set {this.HasPreviousPage = value;}
    }

    public bool HasNextPage
    {
        get{return(PageIndex < PageSize);}
        set{this.HasNextPage = value;}
    }
}
