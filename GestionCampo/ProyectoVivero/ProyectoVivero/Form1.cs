using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoVivero
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection conexion = new SqlConnection("Data Source = NOTEBOOKMATEO; database = GestionCampo; integrated Security = true");

        private void txtUsuario_Enter(object sender, EventArgs e)
        {
            if (txtUsuario.Text == "USUARIO")
            {
                txtUsuario.Text = "";
                txtUsuario.ForeColor = Color.Black;
            }
        }

        private void txtUsuario_Leave(object sender, EventArgs e)
        {
            if (txtUsuario.Text == "")
            {
                txtUsuario.Text = "USUARIO";
                txtUsuario.ForeColor = Color.DimGray;
            }
        }

        private void txtContraseña_Enter(object sender, EventArgs e)
        {
            if (txtContraseña.Text == "CONTRASEÑA")
            {
                txtContraseña.Text = "";
                txtContraseña.ForeColor = Color.Black;
                txtContraseña.UseSystemPasswordChar = true;
            }
        }

        private void txtContraseña_Leave(object sender, EventArgs e)
        {
            if (txtContraseña.Text == "")
            {
                txtContraseña.Text = "CONTRASEÑA";
                txtContraseña.ForeColor = Color.DimGray;
                txtContraseña.UseSystemPasswordChar = false;
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        public void Loguear(string usuario, string contrasena)
        {
            try
            {
                conexion.Open();
                SqlCommand comando = new SqlCommand("SELECT Nombre, TipoUsuario FROM usuarios WHERE Usuario = @usuario AND Password = @pas", conexion);
                comando.Parameters.AddWithValue("@usuario", usuario);
                comando.Parameters.AddWithValue("@pas", contrasena);

                SqlDataAdapter sda = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                //se llenan los datos de la tabla, le paso como argumento la data table
                sda.Fill(dt);

                if (dt.Rows.Count == 1)     //si encontró filas
                {
                    this.Hide();    //para ocultar el form de logueo
                    
                    if (dt.Rows[0][1].ToString() == "Administrador")
                    {
                        new PanelPrincipal(dt.Rows[0][0].ToString()).Show();
                    }
                    
                    else if((dt.Rows[0][1].ToString() == "Usuario"))
                    {
                        new PanelPrincipal(dt.Rows[0][0].ToString()).Show();
                    }
                
                }
                else
                {
                    MessageBox.Show("Usuario y/o Contraseña incorrecta");
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                conexion.Close();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Loguear(this.txtUsuario.Text, this.txtContraseña.Text);
        }
    }
}
