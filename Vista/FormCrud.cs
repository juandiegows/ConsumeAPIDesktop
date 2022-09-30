using API_Sencilla.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vista
{
    public partial class FormCrud : Form
    {
        CRUD CRUD = new CRUD();
        public FormCrud()
        {
            InitializeComponent();
        }
        private async void llenarTabla()
        {
            try
            {
                Respuesta Orespuesta = await CRUD.GetAll<Evento>(CRUD.EVENTO);
                List<Evento> eventos = Orespuesta.Data as List<Evento>;
                var even = from e in eventos.AsEnumerable()
                           select new {
                               ID = e.ID,
                               TipoEvento = e.TipoEvento.Nombre,
                               Estado = e.Estado.Nombre,
                               Nombre = e.Nombre,
                               Fecha = e.FechaProgramada,
                               Inicio = e.HoraInicio,
                               Fin = e.HoraFin,
                               Boleteria =  e.Boleteria==1?true:false
                              
                           };
                TablaEvento.DataSource = even.ToList();
            }
            catch (Exception)
            {

                MessageBox.Show("Problemas al conectar con el API");
            }
        }
        private async void llenarTipoEvento()
        {
            try
            {
                Respuesta Orespuesta = await CRUD.GetAll<TipoEvento>(CRUD.TIPO_EVENTO);
                List<TipoEvento> eventos = Orespuesta.Data as List<TipoEvento>;
                cmbTipoEvento.DataSource = eventos;
                cmbTipoEvento.DisplayMember = "Nombre";
                cmbTipoEvento.ValueMember = "ID";
            }
            catch (Exception)
            {

                MessageBox.Show("Problemas al conectar con el API");
            }
        }

        private async void llenarEstadoEvento()
        {
            try
            {
                Respuesta Orespuesta = await CRUD.GetAll<Estado>(CRUD.ESTADO);
                List<Estado> eventos = Orespuesta.Data as List<Estado>;
                cmbEstado.DataSource = eventos;
                cmbEstado.DisplayMember = "Nombre";
                cmbEstado.ValueMember = "ID";
            }
            catch (Exception)
            {

                MessageBox.Show("Problemas al conectar con el API");
            }
        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            await CRUD.Post<Estado>(CRUD.ESTADO, new Estado { ID = 3, Nombre = "Nuevo" });
            llenarTipoEvento();
            llenarEstadoEvento();
            llenarTabla();
        }

        private void Limpiar()
        {
            txtNombre.Text = String.Empty;
            ckbBoleteria.Checked = false;
            
            FechaEvento.Value = HoraInicio.Value = HoraFin.Value = DateTime.Now;
        }
        public void Refrescar()
        {
            btnActualizar.Enabled = false;
            btnGuardar.Enabled = true;
            btnEliminar.Enabled = true;
            Limpiar();
            llenarTipoEvento();
            llenarEstadoEvento();
            llenarTabla();
        }
        private async void btnGuardar_Click(object sender, EventArgs e)
        {
           

            Respuesta r =await CRUD.Post<Evento>(CRUD.EVENTO,new Evento {
                Boleteria = (byte?) (ckbBoleteria.Checked?1:0),
                EstadoID = (int) cmbEstado.SelectedValue,
                TipoEventoID = (int) cmbTipoEvento.SelectedValue,
                FechaProgramada = FechaEvento.Value,
                Nombre = txtNombre.Text,
                HoraFin = new TimeSpan(HoraFin.Value.Hour, HoraFin.Value.Minute, HoraFin.Value.Second),
                HoraInicio = new TimeSpan(HoraInicio.Value.Hour, HoraInicio.Value.Minute, HoraInicio.Value.Second),
                Estado = null,
                Historial = null,
                TipoEvento= null,
                ID =0


            });

            if(r.Estado == Respuesta.EXITO)
            {
                if (r.Data.ToString()== HttpStatusCode.Created.ToString())
                {
                    MessageBox.Show("Se guardo correctamente");
                    Refrescar();
                }
                else
                {
                    MessageBox.Show(r.Data.ToString());
                    Refrescar();
                }
            }
            else
            {
                MessageBox.Show("No se guardo correctamente");
            }
        }

        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("¿seguro que desear eliminar el evento","Eliminar",MessageBoxButtons.YesNo)== DialogResult.No)
            {
                return;
            }
            int ID =(int) TablaEvento.CurrentRow.Cells[0].Value;
            Respuesta r = await CRUD.Delete(CRUD.EVENTO, ID);
            if (r.Estado == Respuesta.EXITO)
            {
                if (r.Data.ToString() == HttpStatusCode.OK.ToString())
                {
                    MessageBox.Show("Se Eliminado correctamente");
                    Refrescar();
                }
                else
                {
                    MessageBox.Show(r.Data.ToString());
                    Refrescar();
                }
            }
            else
            {
                MessageBox.Show("No se Eliminado correctamente");
            }
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            Refrescar();
        }

        int IDEdit = 0;
        private async void btnEditar_Click(object sender, EventArgs e)
        {
            btnActualizar.Enabled = true;
            btnGuardar.Enabled = false;
            btnEliminar.Enabled = false;

            int ID = (int)TablaEvento.CurrentRow.Cells[0].Value;
            IDEdit = ID;
            Respuesta r = await CRUD.Get<Evento>(CRUD.EVENTO, ID);
            Evento ev = r.Data as Evento;
            txtNombre.Text = ev.Nombre;
            FechaEvento.Value = (DateTime) ev.FechaProgramada;
            cmbTipoEvento.SelectedValue = ev.TipoEventoID;
            cmbEstado.SelectedValue = ev.EstadoID;
            HoraInicio.Value = new DateTime(2001,10,21, ev.HoraInicio.Value.Hours, ev.HoraInicio.Value.Minutes, ev.HoraInicio.Value.Seconds);
            HoraFin.Value = new DateTime(2001, 10, 21, ev.HoraFin.Value.Hours, ev.HoraFin.Value.Minutes, ev.HoraFin.Value.Seconds);
            ckbBoleteria.Checked = ev.Boleteria == 1 ? true : false;

        }

        private async void btnActualizar_Click(object sender, EventArgs e)
        {

            Respuesta r = await CRUD.Get<Evento>(CRUD.EVENTO, IDEdit);
            Evento ev = r.Data as Evento;
            ev.Boleteria = (byte?)(ckbBoleteria.Checked ? 1 : 0);
            ev.EstadoID = (int)cmbEstado.SelectedValue;
            ev.TipoEventoID = (int)cmbTipoEvento.SelectedValue;
            ev.FechaProgramada = FechaEvento.Value;
            ev.Nombre = txtNombre.Text;
            ev.HoraFin = new TimeSpan(HoraFin.Value.Hour, HoraFin.Value.Minute, HoraFin.Value.Second);
            ev.HoraInicio = new TimeSpan(HoraInicio.Value.Hour, HoraInicio.Value.Minute, HoraInicio.Value.Second);
            ev.Estado = null;
            ev.Historial = null;
            ev.TipoEvento = null;
            ev.ID = IDEdit;
            r = await CRUD.Put<Evento>(CRUD.EVENTO, ev, IDEdit);

            if (r.Estado == Respuesta.EXITO)
            {
                if (r.Data.ToString() == HttpStatusCode.NoContent.ToString())
                {
                    MessageBox.Show("Se Actualizado correctamente");
                    Refrescar();
                }
                else
                {
                    MessageBox.Show(r.Data.ToString());
                    Refrescar();
                }
            }
            else
            {
                MessageBox.Show("No se Actualizado correctamente");
            }
        }
    }
}
