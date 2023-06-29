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
    public partial class Compras : Form
    {
        SqlConnection conexion5;
        public Compras(SqlConnection conexion)
        {
            InitializeComponent();
            conexion5 = conexion;
        }
        public void DesHabilitarTodo()
        {
            txtNombre.Enabled = false;
            txtProveedor.Enabled = false;
            txtPrecio.Enabled = false;
            txtCantidad.Enabled = false;
            btnModificar.Enabled = false;
            btnAgregar.Enabled = false;
            btnEliminar.Enabled = false;

        }
        public void Habilitar()
        {
            txtNombre.Enabled = true;
            txtProveedor.Enabled = true;
            txtPrecio.Enabled = true;
            txtCantidad.Enabled = true;
            txtNombre.Text = "";
            txtProveedor.Text = "";
            txtPrecio.Text = "";
            txtCantidad.Text = "";
        }
        public void LimpiarDatos()
        {
            txtNombre.Text = "";
            txtProveedor.Text = "";
            txtPrecio.Text = "";
            txtCantidad.Text = "";
        }
        private void CargarDatos()
        {
            try
            {
                string cadena = "SELECT * FROM Compras";

                SqlDataAdapter Adaptador = new SqlDataAdapter(cadena, conexion5);

                DataSet Conjunto = new DataSet();

                Adaptador.Fill(Conjunto, "Compras");

                dgvCompras.DataSource = Conjunto;
                dgvCompras.DataMember = "Compras";

                //para que se ajusten el tamaño de las columnas automáticamente
                dgvCompras.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                DesHabilitarTodo();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error al cargar el stock: " + ex.Message);
            }
            finally
            {
                conexion5.Close();
            }
        }
/* =============================================== OOOOOOOOO =========================================================== */

        //Para mostrar los datos, ya se cargan las siembras
        private void Compras_Load(object sender, EventArgs e)
        {
            CargarDatos();
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
            if (txtNombre.Text == "" || txtProveedor.Text == "" || txtPrecio.Text == "" || txtCantidad.Text == "")
            {
                MessageBox.Show("Inserción inválida, por favor completar los datos en los campos...", "Información");
                txtNombre.Focus();
                return;
            }
            else
            {
                try
                {
                    conexion5.Open();

                    string NombreProd = txtNombre.Text;
                    string Proveedor = txtProveedor.Text;
                    double PrecioProd = Convert.ToDouble(txtPrecio.Text);
                    double CantidadProd = Convert.ToDouble(txtCantidad.Text);

                    string cadena = "INSERT INTO Compras (Nombre, Proveedor, Precio, Cantidad) VALUES (@Nom, @Prov, @Precio, @Cant)";

                    SqlCommand comando = new SqlCommand(cadena, conexion5);
                    comando.Parameters.AddWithValue("@Nom", NombreProd);
                    comando.Parameters.AddWithValue("@Prov", Proveedor);
                    comando.Parameters.AddWithValue("@Precio", PrecioProd);
                    comando.Parameters.AddWithValue("@Cant", CantidadProd);
                    

                    comando.ExecuteNonQuery();

                    //actualizar los datos
                    CargarDatos();

                    MessageBox.Show("Los datos se guardaron correctamente");

                    conexion5.Close();

                }
                catch (SqlException)
                {
                    MessageBox.Show("No se pudo realizar la operación", "Alerta");
                }
                finally
                {
                    conexion5.Close();
                }
            }
        }
/* =============================================== OOOOOOOOO =========================================================== */

        //variable miembro de la clase Compras
        private int idSeleccionado;

        //OBTENER EN DONDE HIZO CLICK EL USUARIO
        private void dgvStock_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvCompras.Rows.Count)
            {
                Habilitar();
                btnNuevo.Enabled = false;
                btnAgregar.Enabled = false;
                btnModificar.Enabled = true;
                btnEliminar.Enabled = true;

                // Obtener el identificador del registro seleccionado
                idSeleccionado = Convert.ToInt32(dgvCompras.Rows[e.RowIndex].Cells["Id"].Value);
            }
        }
        //MODIFICAR DATOS
        private void btnModificar_Click(object sender, EventArgs e)
        {
            // Validación de datos
            if (txtNombre.Text == "" || txtProveedor.Text == "" || txtPrecio.Text == "" || txtCantidad.Text == "")
            {
                MessageBox.Show("Inserción inválida, por favor completar los datos en los campos...", "Información");
                txtNombre.Focus();
                return;
            }

            try
            {
                conexion5.Open();

                // Obtener los valores de los controles de edición
                string NombreProd = txtNombre.Text;
                string Proveedor = txtProveedor.Text;
                double PrecioProd = Convert.ToDouble(txtPrecio.Text);
                double CantidadProd = Convert.ToDouble(txtCantidad.Text);

                // Actualizar los datos en la base de datos
                string cadena = "UPDATE Compras SET Nombre = @Nom, Proveedor = @Prov, Precio = @Pre, Cantidad = @Cant WHERE Id = @IdSelec";

                SqlCommand comando = new SqlCommand(cadena, conexion5);
                comando.Parameters.AddWithValue("@Nom", NombreProd);
                comando.Parameters.AddWithValue("@Prov", Proveedor);
                comando.Parameters.AddWithValue("@Pre", PrecioProd);
                comando.Parameters.AddWithValue("@Cant", CantidadProd);
                comando.Parameters.AddWithValue("@IdSelec", idSeleccionado);

                comando.ExecuteNonQuery();

                //actualizar los datos
                CargarDatos();

                MessageBox.Show("Los datos se actualizaron correctamente");

                conexion5.Close();
                btnNuevo.Enabled = true;
                LimpiarDatos();

            }
            catch (SqlException)
            {
                MessageBox.Show("No se pudo realizar la operación", "Alerta");
            }
            finally
            {
                conexion5.Close();
            }
        }

/* =============================================== OOOOOOOOO =========================================================== */
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            // Validar si se ha seleccionado una fila para eliminar
            if (idSeleccionado == -1)
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
                    conexion5.Open();

                    // Eliminar el registro de la base de datos
                    string cadena = "DELETE FROM Compras WHERE Id = @Identificador";

                    SqlCommand comando = new SqlCommand(cadena, conexion5);
                    comando.Parameters.AddWithValue("@Identificador", idSeleccionado);

                    comando.ExecuteNonQuery();

                    //actualizar los datos
                    CargarDatos();

                    MessageBox.Show("El registro se eliminó correctamente");

                    conexion5.Close();
                    btnNuevo.Enabled = true;
                }
                catch (SqlException)
                {
                    MessageBox.Show("No se pudo realizar la operación", "Alerta");
                }
                finally
                {
                    conexion5.Close();
                }
            }
        }
    }
}
