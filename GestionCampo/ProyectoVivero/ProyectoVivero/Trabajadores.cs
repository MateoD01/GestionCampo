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
    public partial class Trabajadores : Form
    {
        SqlConnection conexion8;
        public Trabajadores(SqlConnection conexion)
        {
            InitializeComponent();
            conexion8 = conexion;
        }
        public void DesHabilitar()
        {
            txtDni.Enabled = false;
            txtNombre.Enabled = false;
            txtApellido.Enabled = false;
            txtTelefono.Enabled = false;
            txtTrabajo.Enabled = false;
            txtSalario.Enabled = false;
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
            txtTrabajo.Enabled = true;
            txtSalario.Enabled = true;
            txtDni.Text = "";
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtTelefono.Text = "";
            txtTrabajo.Text = "";
            txtSalario.Text = "";
        }
        public void LimpiarDatos()
        {
            txtDni.Text = "";
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtTelefono.Text = "";
            txtTrabajo.Text = "";
            txtSalario.Text = "";
        }

        //método para cargar los EMPLEADOS
        private void CargarEmpleados()
        {
            try
            {
                string cadena = "SELECT * FROM Empleados";

                SqlDataAdapter Adaptador = new SqlDataAdapter(cadena, conexion8);

                DataSet Conjunto = new DataSet();

                Adaptador.Fill(Conjunto, "Empleados");

                dgvTrabajadores.DataSource = Conjunto;
                dgvTrabajadores.DataMember = "Empleados";

                // Ajustar el formato de visualización para la columna "Telefono"
                DataGridViewColumn telefonoColumna = dgvTrabajadores.Columns["Telefono"];
                telefonoColumna.DefaultCellStyle.Format = "0";

                //para que se ajusten el tamaño de las columnas automáticamente
                dgvTrabajadores.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                DesHabilitar();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error al cargar los trabajadores: " + ex.Message);
            }
            finally
            {
                conexion8.Close();
            }
        }

        /* =============================================== OOOOOOOOO =========================================================== */
        //Para mostrar los datos, ya se cargan los clientes
        private void Trabajadores_Load(object sender, EventArgs e)
        {
            CargarEmpleados();
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
            if (txtDni.Text == "" || txtNombre.Text == "" || txtApellido.Text == "" || txtTelefono.Text == "" || txtTrabajo.Text == "" || txtSalario.Text == "")
            {
                MessageBox.Show("Inserción inválida, por favor completar los datos en los campos...", "Información");
                txtNombre.Focus();
                return;
            }
            else
            {
                try
                {
                    conexion8.Open();

                    int Dni = Convert.ToInt32(txtDni.Text);
                    string Nombre = txtNombre.Text;
                    string Apellido = txtApellido.Text;
                    string Telefono = txtTelefono.Text;
                    string Trabajo = txtTrabajo.Text;
                    double Salario = Convert.ToDouble(txtSalario.Text);

                    string cadena = "INSERT INTO Empleados (Dni, Nombre, Apellido, Telefono, Trabajo, Salario) VALUES (@Dni, @Nom, @Ape, @Tel, @Tra, @Sal)";

                    SqlCommand comando = new SqlCommand(cadena, conexion8);
                    comando.Parameters.AddWithValue("@Dni", Dni);
                    comando.Parameters.AddWithValue("@Nom", Nombre);
                    comando.Parameters.AddWithValue("@Ape", Apellido);
                    comando.Parameters.AddWithValue("@Tel", Telefono);
                    comando.Parameters.AddWithValue("@Tra", Trabajo);
                    comando.Parameters.AddWithValue("@Sal", Salario);



                    comando.ExecuteNonQuery();

                    //actualizar los datos
                    CargarEmpleados();

                    MessageBox.Show("Los datos se guardaron correctamente");

                    conexion8.Close();

                }
                catch (SqlException)
                {
                    MessageBox.Show("No se pudo realizar la operación", "Alerta");
                }
                finally
                {
                    conexion8.Close();
                }
            }
        }

        /* =============================================== OOOOOOOOO =========================================================== */
        //variable miembro de la clase Empleados
        private string empleadoSeleccionado;

        //OBTENER EN DONDE HIZO CLICK EL USUARIO
        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvTrabajadores.Rows.Count)
            {
                Habilitar();
                btnNuevo.Enabled = false;
                btnAgregar.Enabled = false;
                btnModificar.Enabled = true;
                btnEliminar.Enabled = true;

                // Obtener el identificador del registro seleccionado
                empleadoSeleccionado = dgvTrabajadores.Rows[e.RowIndex].Cells["Dni"].Value.ToString();
            }
        }

        //MODIFICAR DATOS
        private void btnModificar_Click(object sender, EventArgs e)
        {
            // Validación de datos
            if (txtDni.Text == "" || txtNombre.Text == "" || txtApellido.Text == "" || txtTelefono.Text == "" || txtTrabajo.Text == "" || txtSalario.Text == "")
            {
                MessageBox.Show("Inserción inválida, por favor completar los datos en los campos...", "Información");
                txtNombre.Focus();
                return;
            }

            try
            {
                conexion8.Open();

                // Obtener los valores de los controles de edición
                int Dni = Convert.ToInt32(txtDni.Text);
                string Nombre = txtNombre.Text;
                string Apellido = txtApellido.Text;
                string Telefono = txtTelefono.Text;
                string Trabajo = txtTrabajo.Text;
                double Salario = Convert.ToDouble(txtSalario.Text);

                // Actualizar los datos en la base de datos
                string cadena = "UPDATE Empleados SET Dni = @Dni, Nombre = @Nom, Apellido = @Ape, Telefono = @Tel, Trabajo = @Tra, Salario = @Sal WHERE Dni = @dniSelec"; ;

                SqlCommand comando = new SqlCommand(cadena, conexion8);
                comando.Parameters.AddWithValue("@Dni", Dni);
                comando.Parameters.AddWithValue("@Nom", Nombre);
                comando.Parameters.AddWithValue("@Ape", Apellido);
                comando.Parameters.AddWithValue("@Tel", Telefono);
                comando.Parameters.AddWithValue("@Tra", Trabajo);
                comando.Parameters.AddWithValue("@Sal", Salario);
                comando.Parameters.AddWithValue("@dniSelec", empleadoSeleccionado);

                comando.ExecuteNonQuery();

                //actualizar los datos
                CargarEmpleados();

                MessageBox.Show("Los datos se actualizaron correctamente");

                conexion8.Close();
                btnNuevo.Enabled = true;
                LimpiarDatos();
            }
            catch (SqlException)
            {
                MessageBox.Show("No se pudo realizar la operación", "Alerta");
            }
            finally
            {
                conexion8.Close();
            }
        }

        /* =============================================== OOOOOOOOO =========================================================== */
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            // Validar si se ha seleccionado una fila para eliminar
            if (string.IsNullOrEmpty(empleadoSeleccionado))
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
                    conexion8.Open();

                    // Eliminar el registro de la base de datos
                    string cadena = "DELETE FROM Empleados WHERE Dni = @Dni";

                    SqlCommand comando = new SqlCommand(cadena, conexion8);
                    comando.Parameters.AddWithValue("@Dni", empleadoSeleccionado);

                    comando.ExecuteNonQuery();

                    //actualizar los datos
                    CargarEmpleados();

                    MessageBox.Show("El registro se eliminó correctamente");

                    btnNuevo.Enabled = true;
                    conexion8.Close();
                }
                catch (SqlException)
                {
                    MessageBox.Show("No se pudo realizar la operación", "Alerta");
                }
                finally
                {
                    conexion8.Close();
                }
            }
        }
    }
}
