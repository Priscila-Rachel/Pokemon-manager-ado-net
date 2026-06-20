using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;

namespace Winform_app
{
    public partial class frmPokemon : Form
    {
        private List<Pokemon> listaPokemon;
        private List<Elemento> listaElemento;
        public frmPokemon()
        {
            InitializeComponent();

        }

        private void frmPokemon_Load(object sender, EventArgs e)
        {
            cargar();
            cboCampo.Items.Add("Número");
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Descripción");

        }

        //El PictureBox muestra la imagen del seleccionado
        private void dgvPokemon_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPokemon.CurrentRow != null)
            {
                Pokemon seleccionado = (Pokemon)dgvPokemon.CurrentRow.DataBoundItem;
                CargarImagen(seleccionado.UrlImagen);
            }
        }

        private void cargar ()
        {
            try
            {
                //El DataGridView muestra los Pokémon de la tabla POKEMONS. 
                PokemonNegocio negocio = new PokemonNegocio();
                listaPokemon = negocio.listar();
                dgvPokemon.DataSource = listaPokemon;
                ocultarColumnas();
                CargarImagen(listaPokemon[0].UrlImagen);


                //El ComboBox muestra los tipos (Planta, Fuego, Agua) desde la tabla ELEMENTOS.
                ElementoNegocio elemento = new ElementoNegocio();
                listaElemento = elemento.listar();

                Elemento opcionTodos = new Elemento();
                opcionTodos.id = 0;
                opcionTodos.Descripcion = "Todos";
                listaElemento.Insert(0, opcionTodos);

                cboElemento.DataSource = listaElemento; //el ComboBox toma el contenido actual de la lista en ese momento y lo usa para mostrarse, por eso se pone al final para mostrar la lista
                cboElemento.DisplayMember = "Descripcion"; // Lo que se ve en pantalla
                cboElemento.ValueMember = "Id";            // Valor interno (clave primaria) esto si no reescribo el tostring
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ocultarColumnas()
        {
            dgvPokemon.Columns["urlimagen"].Visible = false;
            dgvPokemon.Columns["Id"].Visible = false;
        }

        private void CargarImagen (string imagen)
        {
            try
            {
                pbxPokemon.Load(imagen);
            }
            catch (Exception)
            {
                pbxPokemon.Load("https://w7.pngwing.com/pngs/819/548/png-transparent-photo-image-landscape-icon-images-thumbnail.png");
           
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaPokemon alta = new frmAltaPokemon();
            alta.ShowDialog(); //con esto permito que al tocar el boton aceptar se abra la ventana del formulario para agregar pokempn dentro del formulario
            cargar();
        }

       
        //private void cboElemento_SelectedIndexChanged(object sender, EventArgs e)
        //  {
        // int idSeleccionado = (int)cboElemento.SelectedValue;
        //   if (idSeleccionado == 0)
        //     dgvPokemon.DataSource = listaPokemon;
        //    else

        //      dgvPokemon.DataSource = listaPokemon.FindAll(x => x.Tipo.id == idSeleccionado);
        // }  //luego consultar esto es de chat gtp

        //modificar un pokemon, 
        private void btnModificar_Click(object sender, EventArgs e)
        {
            Pokemon seleccionado;
            seleccionado = (Pokemon)dgvPokemon.CurrentRow.DataBoundItem;//me devuelve el objeto seleccionado

            frmAltaPokemon modificar = new frmAltaPokemon(seleccionado);//llamo al constructor que tiene un obj
            modificar.ShowDialog();
            cargar();

        }

        private void btnEliminacionFisica_Click(object sender, EventArgs e)
        {
            eliminar();
        }

        private void btnEliminarLogico_Click(object sender, EventArgs e)
        {
            eliminar(true);
        }

        private void eliminar (bool logico = false) //no es logico
        {
            PokemonNegocio negocio = new PokemonNegocio();
            Pokemon seleccionado;
            try
            {
                DialogResult respuesta = MessageBox.Show("De verdad queres eliminarlo?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Pokemon)dgvPokemon.CurrentRow.DataBoundItem;

                    if (logico)//es logico ?
                        negocio.eliminarLogico(seleccionado.Id);//si
                    else 
                        negocio.eliminar(seleccionado.Id);//no es logico va aca


                    cargar();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnFiltro_Click(object sender, EventArgs e)
        {
            PokemonNegocio negocio = new PokemonNegocio();
            try
            {
                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;
                dgvPokemon.DataSource = negocio.filtrar(campo, criterio, filtro);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
           
        }

        private void textBoxFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Pokemon> ListaFiltrada;
            string filtro = textBoxFiltro.Text;

            if (filtro.Length >= 3)
            {
                ListaFiltrada = listaPokemon.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Tipo.Descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                ListaFiltrada = listaPokemon;
            }

            dgvPokemon.DataSource = null;
            dgvPokemon.DataSource = ListaFiltrada;
            ocultarColumnas();

        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();
            if (opcion == "Número")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("Igual a");
            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
        }
    }
}
