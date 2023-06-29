using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ProyectoVivero
{
    public partial class PanelPrincipal : Form
    {
        SqlConnection conexion = new SqlConnection("Data Source = NOTEBOOKMATEO; database = GestionCampo; integrated Security = true");
        public PanelPrincipal(string nombre)
        {
            InitializeComponent();
            lblMensaje.Text = nombre;
        }

        private void AbrirFormHija(object formHija)
        {
            if(this.panelContenedor.Controls.Count > 0)
                this.panelContenedor.Controls.RemoveAt(0);
            Form fh = formHija as Form;
            fh.TopLevel = false;
            fh.Dock = DockStyle.Fill;
            this.panelContenedor.Controls.Add(fh);
            this.panelContenedor.Tag = fh;
            fh.Show();
        }

        private void btnSiembras_Click(object sender, EventArgs e)
        {
            AbrirFormHija(new Siembras(conexion));
        }

        private void btnCosechas_Click(object sender, EventArgs e)
        {
            AbrirFormHija(new Cosechas(conexion));
        }

        private void btnInventario_Click(object sender, EventArgs e)
        {
            AbrirFormHija(new Inventario(conexion));
        }

        private void btnCompras_Click(object sender, EventArgs e)
        {
            AbrirFormHija(new Compras(conexion));
        }

        private void btnVentas_Click(object sender, EventArgs e)
        {
            AbrirFormHija(new Ventas(conexion));
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            AbrirFormHija(new Clientes(conexion));
        }

        private void btnEmpleados_Click(object sender, EventArgs e)
        {
            AbrirFormHija(new Trabajadores(conexion));
        }

        private void btnInformes_Click(object sender, EventArgs e)
        {
            AbrirFormHija(new Informes());
        }

        private void btnInicio_Click(object sender, EventArgs e)
        {
            AbrirFormHija(new Inicio());
        }

        private void PanelPrincipal_Load(object sender, EventArgs e)
        {
            btnInicio_Click(null, e);
        }
    }
}
