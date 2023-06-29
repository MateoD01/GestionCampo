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
    public partial class Cosechas : Form
    {
        SqlConnection conexion3;
        public Cosechas(SqlConnection conexion)
        {
            InitializeComponent();
            conexion3 = conexion;
        }
        public void DesHabilitar()
        {
            txtTipo.Enabled = false;
            txtDimension.Enabled = false;
            txtFecha.Enabled = false;
            txtCantidad.Enabled = false;
            txtCalidad.Enabled = false;
            btnModificar.Enabled = false;
            btnAgregar.Enabled = false;
            btnEliminar.Enabled = false;

        }
        public void Habilitar()
        {
            txtTipo.Enabled = true;
            txtDimension.Enabled = true;
            txtFecha.Enabled = true;
            txtCantidad.Enabled = true;
            txtCalidad.Enabled = true;
            txtTipo.Text = "";
            txtDimension.Text = "";
            txtCantidad.Text = "";
            txtCalidad.Text = "";
        }
        public void LimpiarDatos()
        {
            txtTipo.Text = "";
            txtDimension.Text = "";
            txtCantidad.Text = "";
            txtCalidad.Text = "";
        }

        //método para cargar las siembras
        private void CargarCosechas()
        {
            try
            {
                string cadena = "SELECT * FROM Cosechas";

                SqlDataAdapter Adaptador = new SqlDataAdapter(cadena, conexion3);

                DataSet Conjunto = new DataSet();

                Adaptador.Fill(Conjunto, "Cosechas");

                dgvCosechas.DataSource = Conjunto;
                dgvCosechas.DataMember = "Cosechas";

                //para que se ajusten el tamaño de las columnas automáticamente
                dgvCosechas.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                DesHabilitar();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error al cargar las cosechas: " + ex.Message);
            }
            finally
            {
                conexion3.Close();
            }
        }
   
/* =============================================== OOOOOOOOO =========================================================== */

        //Para mostrar los datos, ya se cargan las siembras
        private void Cosechas_Load(object sender, EventArgs e)
        {
            CargarCosechas();
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
            if (txtTipo.Text == "" || txtDimension.Text == "" || txtCantidad.Text == "" || txtCalidad.Text == "")
            {
                MessageBox.Show("Inserción inválida, por favor completar los datos en los campos...", "Información");
                txtTipo.Focus();
                return;
            }
            else
            {
                try
                {
                    conexion3.Open();

                    string Tipo = txtTipo.Text;
                    double Dimension = Convert.ToDouble(txtDimension.Text);
                    DateTime Fecha = txtFecha.Value;
                    double Cantidad = Convert.ToDouble(txtCantidad.Text);
                    string Calidad = txtCalidad.Text;

                    string cadena = "INSERT INTO Cosechas (TipoCultivo, Dimension, Fecha, Cantidad, Calidad) VALUES (@TipoCult, @Dimens, @Fech, @Cant, @Cali)";

                    SqlCommand comando = new SqlCommand(cadena, conexion3);
                    comando.Parameters.AddWithValue("@TipoCult", Tipo);
                    comando.Parameters.AddWithValue("@Dimens", Dimension);
                    comando.Parameters.AddWithValue("@Fech", Fecha);
                    comando.Parameters.AddWithValue("@Cant", Cantidad);
                    comando.Parameters.AddWithValue("@Cali", Calidad);

                    comando.ExecuteNonQuery();

                    //actualizar los datos
                    CargarCosechas();

                    MessageBox.Show("Los datos se guardaron correctamente");

                    conexion3.Close();

                }
                catch (SqlException)
                {
                    MessageBox.Show("No se pudo realizar la operación", "Alerta");
                }
                finally
                {
                    conexion3.Close();
                }
            }
        }

        /* =============================================== OOOOOOOOO =========================================================== */

        //variable miembro de la clase Cosechas
        private string cultivoSeleccionado;

        //OBTENER EN DONDE HIZO CLICK EL USUARIO
        private void dgvCosechas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvCosechas.Rows.Count)
            {
                Habilitar();
                btnNuevo.Enabled = false;
                btnAgregar.Enabled = false;
                btnModificar.Enabled = true;
                btnEliminar.Enabled = true;

                // Obtener el identificador del registro seleccionado
                cultivoSeleccionado = dgvCosechas.Rows[e.RowIndex].Cells["TipoCultivo"].Value.ToString();
            }
        }

        //MODIFICAR DATOS
        private void btnModificar_Click(object sender, EventArgs e)
        {
            // Validación de datos
            if (txtTipo.Text == "" || txtDimension.Text == "" || txtCantidad.Text == "" || txtCalidad.Text == "")
            {
                MessageBox.Show("Inserción inválida, por favor completar los datos en los campos...", "Información");
                txtTipo.Focus();
                return;
            }

            try
            {
                conexion3.Open();

                // Obtener los valores de los controles de edición
                string Tipo = txtTipo.Text;
                double Dimension = Convert.ToDouble(txtDimension.Text);
                DateTime Fecha = txtFecha.Value;
                double Cantidad = Convert.ToDouble(txtCantidad.Text);
                string Calidad = txtCalidad.Text;

                // Actualizar los datos en la base de datos
                string cadena = "UPDATE Cosechas SET TipoCultivo = @TipoCult, Dimension = @Dimens, Fecha = @Fech, Cantidad = @Cant, Calidad = @Cali WHERE TipoCultivo = @TipoCultivo";

                SqlCommand comando = new SqlCommand(cadena, conexion3);
                comando.Parameters.AddWithValue("@TipoCult", Tipo);
                comando.Parameters.AddWithValue("@Dimens", Dimension);
                comando.Parameters.AddWithValue("@Fech", Fecha);
                comando.Parameters.AddWithValue("@Cant", Cantidad);
                comando.Parameters.AddWithValue("@Cali", Calidad);
                comando.Parameters.AddWithValue("@TipoCultivo", cultivoSeleccionado);

                comando.ExecuteNonQuery();

                //actualizar los datos
                CargarCosechas();

                MessageBox.Show("Los datos se actualizaron correctamente");

                conexion3.Close();
                btnNuevo.Enabled = true;
                LimpiarDatos();
            }
            catch (SqlException)
            {
                MessageBox.Show("No se pudo realizar la operación", "Alerta");
            }
            finally
            {
                conexion3.Close();
            }
        }

        /* =============================================== OOOOOOOOO =========================================================== */
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            // Validar si se ha seleccionado una fila para eliminar
            if (string.IsNullOrEmpty(cultivoSeleccionado))
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
                    conexion3.Open();

                    // Eliminar el registro de la base de datos
                    string cadena = "DELETE FROM Cosechas WHERE TipoCultivo = @TipoCultivo";

                    SqlCommand comando = new SqlCommand(cadena, conexion3);
                    comando.Parameters.AddWithValue("@TipoCultivo", cultivoSeleccionado);

                    comando.ExecuteNonQuery();

                    //actualizar los datos
                    CargarCosechas();

                    MessageBox.Show("El registro se eliminó correctamente");

                    conexion3.Close();
                    btnNuevo.Enabled = true;
                }
                catch (SqlException)
                {
                    MessageBox.Show("No se pudo realizar la operación", "Alerta");
                }
                finally
                {
                    conexion3.Close();
                }
            }
        }
    }
}
