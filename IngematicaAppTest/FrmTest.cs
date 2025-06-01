using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IngematicaAppTest.Model;
using IngematicaAppTest.Service;

namespace IngematicaAppTest
{
    public partial class FrmTest : Form
    {
        public FrmTest()
        {
            InitializeComponent();

            InitializeCombos();
        }


        private void InitializeCombos()
        {
            InitializeComboTipoTransporte();

            InitializeCoboLocalidad();

        }

        private void InitializeComboTipoTransporte()
        {
            List<TipoTransporteModel> tipoTransporteList = new List<TipoTransporteModel>();
            TipoTransporteService tipoTransporteService = new TipoTransporteService();
            tipoTransporteList = tipoTransporteService.GetList();

            var bindingSourceTipoTransporte = new BindingSource();
            bindingSourceTipoTransporte.DataSource = tipoTransporteList;

            cbTipoTransporte.DataSource = bindingSourceTipoTransporte;
            cbTipoTransporte.DisplayMember = "Nombre";
            cbTipoTransporte.ValueMember = "Id";
        }

        private void InitializeCoboLocalidad()
        {
            List<LocalidadModel> localidadList = new List<LocalidadModel>();
            LocalidadService localidadService = new LocalidadService();
            localidadList = localidadService.GetList();

            var bindingSourceLocalidad = new BindingSource();
            bindingSourceLocalidad.DataSource = localidadList;

            cbLocalidadDestino.DataSource = bindingSourceLocalidad;
            cbLocalidadDestino.DisplayMember = "Nombre";
            cbLocalidadDestino.ValueMember = "Id";
        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            int idTipoTransporte = 0;
            int idLocalidad = 0;
            var localidadSeleccionada = cbLocalidadDestino.SelectedValue as IngematicaAppTest.Model.LocalidadModel;
            if (localidadSeleccionada != null)
            {
                idLocalidad = localidadSeleccionada.IdLocalidad;

            }

            var tipoTransporteSeleccionado = cbTipoTransporte.SelectedValue as IngematicaAppTest.Model.TipoTransporteModel;
            if (tipoTransporteSeleccionado != null)
            {
                idTipoTransporte = tipoTransporteSeleccionado.IdTipoTransporte;

            }
            TipoTransporteService tt = new TipoTransporteService();
            TipoTransporteModel objetoTipoTransporte = tt.GetById(idTipoTransporte);

            LocalidadService ll = new LocalidadService();
            LocalidadModel objetoLocalidad = ll.GetById(idLocalidad);



            DateTime fechaDestino = dtpFechaInicial.Value;

            double diasAAgregar = Convert.ToDouble(objetoTipoTransporte.CoeficineteDemora) * Convert.ToDouble(objetoLocalidad.DiasDemora);

            
            CalculoService calculoService = new CalculoService();
            bool esAutopista = chkAutopista.Checked;

            diasAAgregar = calculoService.calculoDiasSegunTipoDeRuta(esAutopista, diasAAgregar);
            bool porRuta = chkRuta.Checked;
            

           

            diasAAgregar = Math.Ceiling(diasAAgregar);
            double diasDemora = diasAAgregar;
           
            while (diasAAgregar > 0)
            {
                fechaDestino = fechaDestino.AddDays(1);
                if (fechaDestino.DayOfWeek != DayOfWeek.Saturday && fechaDestino.DayOfWeek != DayOfWeek.Sunday)
                {
                    diasAAgregar--;
                }
            }

            if (porRuta)
            {
                DateTime fechaOriginal = dtpFechaInicial.Value;
                double porcentaje = 0.1;
                TimeSpan incremento = TimeSpan.FromDays(fechaOriginal.Day * porcentaje);
                DateTime nuevaFecha = fechaOriginal.Add(incremento);
                fechaDestino = nuevaFecha;
            }

            txtDiasDemora.Text = diasDemora.ToString();
            txtFechaLlegada.Text = fechaDestino.ToString();
          
           
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            dtpFechaInicial.Value = DateTime.Now; 
            txtDiasDemora.Text = "";
            txtFechaLlegada.Text = "";
            chkAutopista.Checked = false; 
            InitializeCombos(); 
                }

        private void chkAutopista_CheckedChanged(object sender, EventArgs e)
        {
            chkRuta.Checked = false;
        }

        private void chkRuta_CheckedChanged(object sender, EventArgs e)
        {
            chkAutopista.Checked = false;
        }
    }
}
