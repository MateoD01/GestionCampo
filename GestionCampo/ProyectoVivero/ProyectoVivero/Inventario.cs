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
    public partial class Inventario : Form
    {
        SqlConnection conexion4;
        public Inventario(SqlConnection conexion)
        {
            InitializeComponent();
            conexion4 = conexion;
        }

        public void DesHabilitarTodo()
        {
            txtNomProd.Enabled = false;
            txtCantidadTon.Enabled = false;
            txtPrecio.Enabled = false;
            btnModificar.Enabled = false;
            btnAgregar.Enabled = false;
            btnEliminar.Enabled = false;

        }
        public void HabilitarStock()
        {
            txtNomProd.Enabled = true;
            txtCantidadTon.Enabled = true;
            txtPrecio.Enabled = true;
            txtNomProd.Text = "";
            txtCantidadTon.Text = "";
            txtPrecio.Text = "";
        }
        public void LimpiarDatos()
        {
            txtNomProd.Text = "";
            txtCantidadTon.Text = "";
            txtPrecio.Text = "";
        }

        //método para cargar el stock cosechado
        private void CargarDatosStock()
        {
            try
            {
                string cadena = "SELECT * FROM InventarioStock";

                SqlDataAdapter Adaptador = new SqlDataAdapter(cadena, conexion4);

                DataSet Conjunto = new DataSet();

                Adaptador.Fill(Conjunto, "InventarioStock");

                dgvStock.DataSource = Conjunto;
                dgvStock.DataMember = "InventarioStock";

                //para que se ajusten el tamaño de las columnas automáticamente
                dgvStock.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                DesHabilitarTodo();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error al cargar el stock: " + ex.Message);
            }
            finally
            {
                conexion4.Close();
            }
        }

        /* =============================================== OOOOOOOOO =========================================================== */

        //Para mostrar los datos, ya se cargan las siembras
        private void Inventario_Load(object sender, EventArgs e)
        {
            CargarDatosStock();
        }

        /* =============================================== OOOOOOOOO =========================================================== */
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            HabilitarStock();
            btnAgregar.Enabled = true;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            //validación de datos
            if (txtNomProd.Text == "" || txtCantidadTon.Text == "" || txtPrecio.Text == "")
            {
                MessageBox.Show("Inserción inválida, por favor completar los datos en los campos...", "Información");
                txtNomProd.Focus();
                return;
            }
            else
            {
                try
                {
                    conexion4.Open();

                    string NombreProd = txtNomProd.Text;
                    double CantidadProd = Convert.ToDouble(txtCantidadTon.Text);
                    double PrecioProd = Convert.ToDouble(txtPrecio.Text);

                    string cadena = "INSERT INTO InventarioStock (Nombre, Cantidad, Precio) VALUES (@Nom, @Cant, @Precio)";

                    SqlCommand comando = new SqlCommand(cadena, conexion4);
                    comando.Parameters.AddWithValue("@Nom", NombreProd);
                    comando.Parameters.AddWithValue("@Cant", CantidadProd);
                    comando.Parameters.AddWithValue("@Precio", PrecioProd);

                    comando.ExecuteNonQuery();

                    //actualizar los datos
                    CargarDatosStock();

                    MessageBox.Show("Los datos se guardaron correctamente");

                    conexion4.Close();

                }
                catch (SqlException)
                {
                    MessageBox.Show("No se pudo realizar la operación", "Alerta");
                }
                finally
                {
                    conexion4.Close();
                }
            }
        }

        /* =============================================== OOOOOOOOO =========================================================== */

        //variable miembro de la clase Stock
        private int idSeleccionado;

        //OBTENER EN DONDE HIZO CLICK EL USUARIO
        private void dgvStock_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvStock.Rows.Count)
            {
                HabilitarStock();
                btnNuevo.Enabled = false;
                btnAgregar.Enabled = false;
                btnModificar.Enabled = true;
                btnEliminar.Enabled = true;

                // Obtener el identificador del registro seleccionado
                idSeleccionado = Convert.ToInt32(dgvStock.Rows[e.RowIndex].Cells["Id"].Value);
            }
        }

        //MODIFICAR DATOS
        private void btnModificar_Click(object sender, EventArgs e)
        {
            // Validación de datos
            if (txtNomProd.Text == "" || txtCantidadTon.Text == "" || txtPrecio.Text == "")
            {
                MessageBox.Show("Inserción inválida, por favor completar los datos en los campos...", "Información");
                txtNomProd.Focus();
                return;
            }

            try
            {
                conexion4.Open();

                // Obtener los valores de los controles de edición
                string NombreProd = txtNomProd.Text;
                double CantidadProd = Convert.ToDouble(txtCantidadTon.Text);
                double PrecioProd = Convert.ToDouble(txtPrecio.Text);

                // Actualizar los datos en la base de datos
                string cadena = "UPDATE InventarioStock SET Nombre = @Nom, Cantidad = @Cant, Precio = @Pre WHERE Id = @IdSelec";

                SqlCommand comando = new SqlCommand(cadena, conexion4);
                comando.Parameters.AddWithValue("@Nom", NombreProd);
                comando.Parameters.AddWithValue("@Cant", CantidadProd);
                comando.Parameters.AddWithValue("@Pre", PrecioProd);
                comando.Parameters.AddWithValue("@IdSelec", idSeleccionado);

                comando.ExecuteNonQuery();

                //actualizar los datos
                CargarDatosStock();

                MessageBox.Show("Los datos se actualizaron correctamente");

                conexion4.Close();
                btnNuevo.Enabled = true;
                LimpiarDatos();

            }
            catch (SqlException)
            {
                MessageBox.Show("No se pudo realizar la operación", "Alerta");
            }
            finally
            {
                conexion4.Close();
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
                    conexion4.Open();

                    // Eliminar el registro de la base de datos
                    string cadena = "DELETE FROM InventarioStock WHERE Id = @Identificador";

                    SqlCommand comando = new SqlCommand(cadena, conexion4);
                    comando.Parameters.AddWithValue("@Identificador", idSeleccionado);

                    comando.ExecuteNonQuery();

                    //actualizar los datos
                    CargarDatosStock();

                    MessageBox.Show("El registro se eliminó correctamente");

                    conexion4.Close();
                    btnNuevo.Enabled = true;
                }
                catch (SqlException)
                {
                    MessageBox.Show("No se pudo realizar la operación", "Alerta");
                }
                finally
                {
                    conexion4.Close();
                }
            }
        }
    }
}
