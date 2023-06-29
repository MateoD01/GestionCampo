using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoVivero
{
    public partial class Siembras : Form
    {
        SqlConnection conexion2;
        public Siembras(SqlConnection conexion)
        {
            InitializeComponent();
            conexion2 = conexion;
        }

        public void DesHabilitar()
        {
            txtTipo.Enabled = false;
            txtDimension.Enabled = false;
            txtFecha.Enabled = false;
            txtFertilizante.Enabled = false;
            txtCantidad.Enabled = false;
            txtRiegos.Enabled = false;
            btnModificar.Enabled = false;
            btnAgregar.Enabled = false;
            btnEliminar.Enabled = false;

        }
        public void Habilitar()
        {
            txtTipo.Enabled = true;
            txtDimension.Enabled = true;
            txtFecha.Enabled = true;
            txtFertilizante.Enabled = true;
            txtCantidad.Enabled = true;
            txtRiegos.Enabled = true;
            txtTipo.Text = "";
            txtDimension.Text = "";
            txtFertilizante.Text = "";
            txtCantidad.Text = "";
        }
        public void LimpiarDatos()
        {
            txtTipo.Text = "";
            txtDimension.Text = "";
            txtFertilizante.Text = "";
            txtCantidad.Text = "";
        }

        //método para cargar las siembras
        private void CargarSiembras()
        {
            try
            {
                string cadena = "SELECT * FROM Siembra";

                SqlDataAdapter Adaptador = new SqlDataAdapter(cadena, conexion2);

                DataSet Conjunto = new DataSet();

                Adaptador.Fill(Conjunto, "Siembra");

                dgvSiembras.DataSource = Conjunto;
                dgvSiembras.DataMember = "Siembra";

                //para que se ajusten el tamaño de las columnas automáticamente
                dgvSiembras.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                DesHabilitar();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error al cargar las siembras: " + ex.Message);
            }
            finally
            {
                conexion2.Close();
            }
        }

        /* =============================================== OOOOOOOOO =========================================================== */

        //Para mostrar los datos, ya se cargan las siembras
        private void Siembras_Load(object sender, EventArgs e)
        {
            CargarSiembras(); 
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
            if (txtTipo.Text == "" || txtDimension.Text == "" || txtFertilizante.Text == "")
            {
                MessageBox.Show("Inserción inválida, por favor completar los datos en los campos...", "Información");
                txtTipo.Focus();
                return;
            }
            else 
            {
                try
                {
                    conexion2.Open();

                    string Tipo = txtTipo.Text;
                    double Dimension = Convert.ToDouble(txtDimension.Text);
                    DateTime Fecha = txtFecha.Value;
                    string Fertilizante = txtFertilizante.Text;
                    double Cantidad = Convert.ToDouble(txtCantidad.Text);
                    DateTime Riegos = txtRiegos.Value;

                    string cadena = "INSERT INTO Siembra (TipoCultivo, Dimension, Fecha, Fertilizante, Cantidad, RiegoProgramado) VALUES (@TipoCult, @Dimens, @Fech, @Fert, @Cant, @Rieg)";

                    SqlCommand comando = new SqlCommand(cadena, conexion2);
                    comando.Parameters.AddWithValue("@TipoCult", Tipo);
                    comando.Parameters.AddWithValue("@Dimens", Dimension);
                    comando.Parameters.AddWithValue("@Fech", Fecha);
                    comando.Parameters.AddWithValue("@Fert", Fertilizante);
                    comando.Parameters.AddWithValue("@Cant", Cantidad);
                    comando.Parameters.AddWithValue("@Rieg", Riegos);

                    comando.ExecuteNonQuery();

                    //actualizar los datos
                    CargarSiembras();
                    
                    MessageBox.Show("Los datos se guardaron correctamente");
                    
                    conexion2.Close();

                }
                catch (SqlException)
                {
                    MessageBox.Show("No se pudo realizar la operación", "Alerta");
                }
                finally
                {
                    conexion2.Close();
                }
            }
        }

/* =============================================== OOOOOOOOO =========================================================== */

        //variable miembro de la clase Siembra
        private string cultivoSeleccionado;

        //OBTENER EN DONDE HIZO CLICK EL USUARIO
        private void dgvSiembras_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvSiembras.Rows.Count)
            {
                Habilitar();
                btnNuevo.Enabled = false;
                btnAgregar.Enabled = false;
                btnModificar.Enabled = true;
                btnEliminar.Enabled = true;
                
                // Obtener el identificador del registro seleccionado
                cultivoSeleccionado = dgvSiembras.Rows[e.RowIndex].Cells["TipoCultivo"].Value.ToString();
            }
        }

        //MODIFICAR DATOS
        private void btnModificar_Click(object sender, EventArgs e)
        {
            // Validación de datos
            if (txtTipo.Text == "" || txtDimension.Text == "" || txtFertilizante.Text == "")
            {
                MessageBox.Show("Inserción inválida, por favor completar los datos en los campos...", "Información");
                txtTipo.Focus();
                return;
            }

            try
            {
                conexion2.Open();

                // Obtener los valores de los controles de edición
                string Tipo = txtTipo.Text;
                double Dimension = Convert.ToDouble(txtDimension.Text);
                DateTime Fecha = txtFecha.Value;
                string Fertilizante = txtFertilizante.Text;
                double Cantidad = Convert.ToDouble(txtCantidad.Text);
                DateTime Riegos = txtRiegos.Value;

                // Actualizar los datos en la base de datos
                string cadena = "UPDATE Siembra SET TipoCultivo = @TipoCult, Dimension = @Dimens, Fecha = @Fech, Fertilizante = @Fert, Cantidad = @Cant, RiegoProgramado = @Rieg WHERE TipoCultivo = @TipoCultivo";

                SqlCommand comando = new SqlCommand(cadena, conexion2);
                comando.Parameters.AddWithValue("@TipoCult", Tipo);
                comando.Parameters.AddWithValue("@Dimens", Dimension);
                comando.Parameters.AddWithValue("@Fech", Fecha);
                comando.Parameters.AddWithValue("@Fert", Fertilizante);
                comando.Parameters.AddWithValue("@Cant", Cantidad);
                comando.Parameters.AddWithValue("@Rieg", Riegos);
                comando.Parameters.AddWithValue("@TipoCultivo", cultivoSeleccionado);

                comando.ExecuteNonQuery();
                
                //actualizar los datos
                CargarSiembras();
                
                MessageBox.Show("Los datos se actualizaron correctamente");

                conexion2.Close();
                btnNuevo.Enabled = true;
                LimpiarDatos();
            }
            catch (SqlException)
            {
                MessageBox.Show("No se pudo realizar la operación", "Alerta");
            }
            finally
            {
                conexion2.Close();
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
                    conexion2.Open();

                    // Eliminar el registro de la base de datos
                    string cadena = "DELETE FROM Siembra WHERE TipoCultivo = @TipoCultivo";

                    SqlCommand comando = new SqlCommand(cadena, conexion2);
                    comando.Parameters.AddWithValue("@TipoCultivo", cultivoSeleccionado);

                    comando.ExecuteNonQuery();

                    //actualizar los datos
                    CargarSiembras();

                    MessageBox.Show("El registro se eliminó correctamente");

                    conexion2.Close();
                    btnNuevo.Enabled = true;
                }
                catch (SqlException)
                {
                    MessageBox.Show("No se pudo realizar la operación", "Alerta");
                }
                finally
                {
                    conexion2.Close();
                }
            }
        }
    }
}
