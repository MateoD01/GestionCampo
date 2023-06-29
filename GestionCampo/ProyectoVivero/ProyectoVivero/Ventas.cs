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
    public partial class Ventas : Form
    {
        SqlConnection conexion6;
        public Ventas(SqlConnection conexion)
        {
            InitializeComponent();
            conexion6 = conexion;
        }
        public void DesHabilitar()
        {
            txtNombre.Enabled = false;
            txtCantidad.Enabled = false;
            txtPrecio.Enabled = false;
            txtCliente.Enabled = false;
            txtFecha.Enabled = false;
            btnModificar.Enabled = false;
            btnAgregar.Enabled = false;
            btnEliminar.Enabled = false;

        }
        public void Habilitar()
        {
            txtNombre.Enabled = true;
            txtCantidad.Enabled = true;
            txtPrecio.Enabled = true;
            txtCliente.Enabled = true;
            txtFecha.Enabled = true;
            txtNombre.Text = "";
            txtCantidad.Text = "";
            txtPrecio.Text = "";
            txtCliente.Text = "";
        }
        public void LimpiarDatos()
        {
            txtNombre.Text = "";
            txtCantidad.Text = "";
            txtPrecio.Text = "";
            txtCliente.Text = "";
        }

        //método para cargar las VENTAS
        private void CargarVentas()
        {
            try
            {
                string cadena = "SELECT * FROM Ventas";

                SqlDataAdapter Adaptador = new SqlDataAdapter(cadena, conexion6);

                DataSet Conjunto = new DataSet();

                Adaptador.Fill(Conjunto, "Ventas");

                dgvVentas.DataSource = Conjunto;
                dgvVentas.DataMember = "Ventas";

                //para que se ajusten el tamaño de las columnas automáticamente
                dgvVentas.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                DesHabilitar();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error al cargar las ventas: " + ex.Message);
            }
            finally
            {
                conexion6.Close();
            }
        }

/* =============================================== OOOOOOOOO =========================================================== */
        //Para mostrar los datos, ya se cargan las ventas
        private void Ventas_Load(object sender, EventArgs e)
        {
            CargarVentas();
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
            if (txtNombre.Text == "" || txtCantidad.Text == "" || txtPrecio.Text == "" || txtCliente.Text == "")
            {
                MessageBox.Show("Inserción inválida, por favor completar los datos en los campos...", "Información");
                txtNombre.Focus();
                return;
            }
            else
            {
                try
                {
                    conexion6.Open();

                    string Nombre = txtNombre.Text;
                    double Cantidad= Convert.ToDouble(txtCantidad.Text);
                    double Precio = Convert.ToDouble(txtPrecio.Text);
                    string Cliente = txtCliente.Text;
                    DateTime Fecha = txtFecha.Value;

                    string cadena = "INSERT INTO Ventas (Nombre, Cantidad, Precio, Cliente, Fecha) VALUES (@Nom, @Cant, @Pre, @Clie, @Fech)";

                    SqlCommand comando = new SqlCommand(cadena, conexion6);
                    comando.Parameters.AddWithValue("@Nom", Nombre);
                    comando.Parameters.AddWithValue("@Cant", Cantidad);
                    comando.Parameters.AddWithValue("@Pre", Precio);
                    comando.Parameters.AddWithValue("@Clie", Cliente);
                    comando.Parameters.AddWithValue("@Fech", Fecha);
                    
                    

                    comando.ExecuteNonQuery();

                    //actualizar los datos
                    CargarVentas();

                    MessageBox.Show("Los datos se guardaron correctamente");

                    conexion6.Close();

                }
                catch (SqlException)
                {
                    MessageBox.Show("No se pudo realizar la operación", "Alerta");
                }
                finally
                {
                    conexion6.Close();
                }
            }
        }

        /* =============================================== OOOOOOOOO =========================================================== */
        //variable miembro de la clase Ventas
        private string ventaSeleccionada;

        //OBTENER EN DONDE HIZO CLICK EL USUARIO
        private void dgvVentas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvVentas.Rows.Count)
            {
                Habilitar();
                btnNuevo.Enabled = false;
                btnAgregar.Enabled = false;
                btnModificar.Enabled = true;
                btnEliminar.Enabled = true;

                // Obtener el identificador del registro seleccionado
                ventaSeleccionada = dgvVentas.Rows[e.RowIndex].Cells["Nombre"].Value.ToString();
            }
        }

        //MODIFICAR DATOS
        private void btnModificar_Click(object sender, EventArgs e)
        {
            // Validación de datos
            if (txtNombre.Text == "" || txtCantidad.Text == "" || txtPrecio.Text == "" || txtCliente.Text == "")
            {
                MessageBox.Show("Inserción inválida, por favor completar los datos en los campos...", "Información");
                txtNombre.Focus();
                return;
            }

            try
            {
                conexion6.Open();

                // Obtener los valores de los controles de edición
                string Nombre = txtNombre.Text;
                double Cantidad = Convert.ToDouble(txtCantidad.Text);
                double Precio = Convert.ToDouble(txtPrecio.Text);
                string Cliente = txtCliente.Text;
                DateTime Fecha = txtFecha.Value;

                // Actualizar los datos en la base de datos
                string cadena = "UPDATE Ventas SET Nombre = @Nom, Cantidad = @Cant, Precio = @Pre, Cliente = @Clie, Fecha = @Fech WHERE Nombre = @Nomb";

                SqlCommand comando = new SqlCommand(cadena, conexion6);
                comando.Parameters.AddWithValue("@Nom", Nombre);
                comando.Parameters.AddWithValue("@Cant", Cantidad);
                comando.Parameters.AddWithValue("@Pre", Precio);
                comando.Parameters.AddWithValue("@Clie", Cliente);
                comando.Parameters.AddWithValue("@Fech", Fecha);
                comando.Parameters.AddWithValue("@Nomb", ventaSeleccionada);

                comando.ExecuteNonQuery();

                //actualizar los datos
                CargarVentas();

                MessageBox.Show("Los datos se actualizaron correctamente");

                conexion6.Close();
                btnNuevo.Enabled = true;
                LimpiarDatos();
            }
            catch (SqlException)
            {
                MessageBox.Show("No se pudo realizar la operación", "Alerta");
            }
            finally
            {
                conexion6.Close();
            }
        }

        /* =============================================== OOOOOOOOO =========================================================== */
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            // Validar si se ha seleccionado una fila para eliminar
            if (string.IsNullOrEmpty(ventaSeleccionada))
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
                    conexion6.Open();

                    // Eliminar el registro de la base de datos
                    string cadena = "DELETE FROM Ventas WHERE Nombre = @Nom";

                    SqlCommand comando = new SqlCommand(cadena, conexion6);
                    comando.Parameters.AddWithValue("@Nom", ventaSeleccionada);

                    comando.ExecuteNonQuery();

                    //actualizar los datos
                    CargarVentas();

                    MessageBox.Show("El registro se eliminó correctamente");

                    conexion6.Close();
                    btnNuevo.Enabled = true;
                }
                catch (SqlException)
                {
                    MessageBox.Show("No se pudo realizar la operación", "Alerta");
                }
                finally
                {
                    conexion6.Close();
                }
            }
        }
    }
}
