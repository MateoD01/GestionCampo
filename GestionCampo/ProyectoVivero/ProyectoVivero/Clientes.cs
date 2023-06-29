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
    public partial class Clientes : Form
    {
        SqlConnection conexion7;
        public Clientes(SqlConnection conexion)
        {
            InitializeComponent();
            conexion7 = conexion;
        }
        public void DesHabilitar()
        {
            txtDni.Enabled = false;
            txtNombre.Enabled = false;
            txtApellido.Enabled = false;
            txtTelefono.Enabled = false;
            txtDireccion.Enabled = false;
            btnModificar.Enabled = false;
            btnAgregar.Enabled = false;
            btnEliminar.Enabled = false;

        }
        public void Habilitar()
        {
            txtDni.Enabled = true;
            txtNombre.Enabled = true;
            txtApellido.Enabled = true;
            txtTelefono.Enabled = true;
            txtDireccion.Enabled = true;
            txtDni.Text = "";
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtTelefono.Text = "";
            txtDireccion.Text = "";
        }
        public void LimpiarDatos()
        {
            txtDni.Text = "";
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtTelefono.Text = "";
            txtDireccion.Text = "";
        }

        //método para cargar los CLIENTES
        private void CargarClientes()
        {
            try
            {
                string cadena = "SELECT * FROM Clientes";

                SqlDataAdapter Adaptador = new SqlDataAdapter(cadena, conexion7);

                DataSet Conjunto = new DataSet();

                Adaptador.Fill(Conjunto, "Clientes");

                dgvClientes.DataSource = Conjunto;
                dgvClientes.DataMember = "Clientes";

                // Ajustar el formato de visualización para la columna "Telefono"
                DataGridViewColumn telefonoColumna = dgvClientes.Columns["Telefono"];
                telefonoColumna.DefaultCellStyle.Format = "0";

                //para que se ajusten el tamaño de las columnas automáticamente
                dgvClientes.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                DesHabilitar();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error al cargar los clientes: " + ex.Message);
            }
            finally
            {
                conexion7.Close();
            }
        }

/* =============================================== OOOOOOOOO =========================================================== */
        //Para mostrar los datos, ya se cargan los clientes
        private void Clientes_Load(object sender, EventArgs e)
        {
            CargarClientes();
        }

/* =============================================== OOOOOOOOO =========================================================== */
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Habilitar();
            btnAgregar.Enabled = true;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            //validación de datos
            if (txtDni.Text == "" || txtNombre.Text == "" || txtApellido.Text == "" || txtTelefono.Text == "" || txtDireccion.Text == "")
            {
                MessageBox.Show("Inserción inválida, por favor completar los datos en los campos...", "Información");
                txtNombre.Focus();
                return;
            }
            else
            {
                try
                {
                    conexion7.Open();

                    int Dni = Convert.ToInt32(txtDni.Text);
                    string Nombre = txtNombre.Text;
                    string Apellido = txtApellido.Text;
                    string Telefono = txtTelefono.Text;
                    string Direccion = txtDireccion.Text;

                    string cadena = "INSERT INTO Clientes (Dni, Nombre, Apellido, Telefono, Direccion) VALUES (@Dni, @Nom, @Ape, @Tel, @Dir)";

                    SqlCommand comando = new SqlCommand(cadena, conexion7);
                    comando.Parameters.AddWithValue("@Dni", Dni);
                    comando.Parameters.AddWithValue("@Nom", Nombre);
                    comando.Parameters.AddWithValue("@Ape", Apellido);
                    comando.Parameters.AddWithValue("@Tel", Telefono);
                    comando.Parameters.AddWithValue("@Dir", Direccion);



                    comando.ExecuteNonQuery();

                    //actualizar los datos
                    CargarClientes();

                    MessageBox.Show("Los datos se guardaron correctamente");

                    conexion7.Close();

                }
                catch (SqlException)
                {
                    MessageBox.Show("No se pudo realizar la operación", "Alerta");
                }
                finally
                {
                    conexion7.Close();
                }
            }
        }

/* =============================================== OOOOOOOOO =========================================================== */
        //variable miembro de la clase Ventas
        private string clienteSeleccionado;

        //OBTENER EN DONDE HIZO CLICK EL USUARIO
        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvClientes.Rows.Count)
            {
                Habilitar();
                btnNuevo.Enabled = false;
                btnAgregar.Enabled = false;
                btnModificar.Enabled = true;
                btnEliminar.Enabled = true;

                // Obtener el identificador del registro seleccionado
                clienteSeleccionado = dgvClientes.Rows[e.RowIndex].Cells["Dni"].Value.ToString();
            }
        }

        //MODIFICAR DATOS
        private void btnModificar_Click(object sender, EventArgs e)
        {
            // Validación de datos
            if (txtDni.Text == "" || txtNombre.Text == "" || txtApellido.Text == "" || txtTelefono.Text == "" || txtDireccion.Text == "")
            {
                MessageBox.Show("Inserción inválida, por favor completar los datos en los campos...", "Información");
                txtNombre.Focus();
                return;
            }

            try
            {
                conexion7.Open();

                // Obtener los valores de los controles de edición
                int Dni = Convert.ToInt32(txtDni.Text);
                string Nombre = txtNombre.Text;
                string Apellido = txtApellido.Text;
                string Telefono = txtTelefono.Text;
                string Direccion = txtDireccion.Text;

                // Actualizar los datos en la base de datos
                string cadena = "UPDATE Clientes SET Dni = @Dni, Nombre = @Nom, Apellido = @Ape, Telefono = @Tel, Direccion = @Dir WHERE Dni = @dniSelec"; ;

                SqlCommand comando = new SqlCommand(cadena, conexion7);
                comando.Parameters.AddWithValue("@Dni", Dni);
                comando.Parameters.AddWithValue("@Nom", Nombre);
                comando.Parameters.AddWithValue("@Ape", Apellido);
                comando.Parameters.AddWithValue("@Tel", Telefono);
                comando.Parameters.AddWithValue("@Dir", Direccion);
                comando.Parameters.AddWithValue("@dniSelec", clienteSeleccionado);

                comando.ExecuteNonQuery();

                //actualizar los datos
                CargarClientes();

                MessageBox.Show("Los datos se actualizaron correctamente");

                conexion7.Close();
                btnNuevo.Enabled = true;
                LimpiarDatos();
            }
            catch (SqlException)
            {
                MessageBox.Show("No se pudo realizar la operación", "Alerta");
            }
            finally
            {
                conexion7.Close();
            }
        }

/* =============================================== OOOOOOOOO =========================================================== */
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            // Validar si se ha seleccionado una fila para eliminar
            if (string.IsNullOrEmpty(clienteSeleccionado))
            {
                MessageBox.Show("No se ha seleccionado ningún registro para eliminar.", "Información");
                return;
            }

            // Confirmar la eliminación con el usuario
            DialogResult confirmar = MessageBox.Show("¿Estás seguro de que deseas eliminar el registro seleccionado?", "Confirmar eliminación", MessageBoxButtons.YesNo);
            if (confirmar == DialogResult.Yes)
            {
                try
                {
                    conexion7.Open();

                    // Eliminar el registro de la base de datos
                    string cadena = "DELETE FROM Clientes WHERE Dni = @Dni";

                    SqlCommand comando = new SqlCommand(cadena, conexion7);
                    comando.Parameters.AddWithValue("@Dni", clienteSeleccionado);

                    comando.ExecuteNonQuery();

                    //actualizar los datos
                    CargarClientes();

                    MessageBox.Show("El registro se eliminó correctamente");

                    conexion7.Close();
                    btnNuevo.Enabled = true;
                }
                catch (SqlException)
                {
                    MessageBox.Show("No se pudo realizar la operación", "Alerta");
                }
                finally
                {
                    conexion7.Close();
                }
            }
        }
    }
}
