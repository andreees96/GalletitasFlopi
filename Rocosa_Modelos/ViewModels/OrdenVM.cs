﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocosa_Modelos.ViewModels
{
    public class OrdenVM
    {
        public Orden Orden { get; set; }
        public IEnumerable<OrdenDetalle> OrdenDetalles { get; set; }
    }
}
