using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IngematicaAppTest.Business;

namespace IngematicaAppTest.Service
{
    public partial class CalculoService
    {
        public double calculoDiasSegunTipoDeRuta(bool esAutopista, double diasTotales)
        {
            if (esAutopista)
            {
                CalculoBusiness cb = new CalculoBusiness();
                diasTotales = cb.CantidadDiasTotal(diasTotales);
            }

           
            return diasTotales;
        }
    }
}
