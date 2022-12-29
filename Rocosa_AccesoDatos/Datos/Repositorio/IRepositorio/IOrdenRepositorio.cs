﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Rocosa_Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocosa_AccesoDatos.Datos.Repositorio.IRepositorio
{
    public interface IOrdenRepositorio : IRepositorio<Orden>
    {
        void Actualizar(Orden orden);

    }
}
