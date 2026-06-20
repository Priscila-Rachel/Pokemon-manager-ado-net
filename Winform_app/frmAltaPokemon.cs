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
using System.Configuration;
using System.IO;

namespace Winform_app
{
    public partial class frmAltaPokemon : Form
    {
        private Pokemon pokemon = null;
        private OpenFileDialog archivo = null;
        public frmAltaPokemon()
        {
            InitializeComponent();
        }

        public frmAltaPokemon(Pokemon pokemon )
        {
            InitializeComponent();

            this.pokemon = pokemon;
            Text = "Modificar Pokemon";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            //Pokemon poke = new Pokemon();//cuando creo un pokemon viene con un nombre,numero y descripcion porque asi lo defini en dominio esto lo elimino para poder usar un mismo boton para modificar o agregar
            PokemonNegocio negocio = new PokemonNegocio(); // creo un nuevo negocio q me permite el acceso a los metodos
            try
            {
                if (pokemon == null)//si el pokemon esta en nulo significa que toque aceptar en agregar 
                    pokemon = new Pokemon();//entonces creo nuevo pokemon
                pokemon.Numero =int.Parse(textBoxNumero.Text);// guardo en la variable numero lo que escribo en la caja de texto de numero.
                pokemon.Nombre = textBoxNombre.Text;
                pokemon.Descripcion = textBoxDescripcion.Text;
                pokemon.UrlImagen = textBoxUrlImagen.Text; //lo agrego aca 
                pokemon.Tipo = (Elemento)cboTipo.SelectedItem;//guardo en una variable tipo lo q selecciono en cbo tipo
                pokemon.Debilidad = (Elemento)cboDebilidad.SelectedItem;

                if (pokemon.Id != 0)
                {
                    negocio.modificar(pokemon);
                    MessageBox.Show("Modificado exitosamente");
                }
                else
                {
                    negocio.agregar(pokemon);//llamo al metodo agrgar para q agrege un pokemon
                    MessageBox.Show("agrgado exitosamente");// muestro mensaje si todo salio bien
                }

                if (archivo != null && !(textBoxUrlImagen.Text.ToUpper().Contains("HTTP")))
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + archivo.SafeFileName);

                
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void frmAltaPokemon_Load(object sender, EventArgs e)
        {
            ElementoNegocio elementoNegocio = new ElementoNegocio();//crreo un nuevo elementonegocio q tiene metodos listar 
            try
            {
                cboTipo.DataSource = elementoNegocio.listar();//recibe el datasourse  los datos de la lista y los modela  en este caso en el cbotipo
                cboTipo.ValueMember = "id";// le designo el valor que uso de contrasena
                cboTipo.DisplayMember = "Descripcion";// les designo en este caso el valor que quiero mostrar

                cboDebilidad.DataSource = elementoNegocio.listar();
                cboDebilidad.ValueMember = "id";
                cboDebilidad.DisplayMember = "Descripcion";

                if(pokemon != null)
                {
                    textBoxNumero.Text = pokemon.Numero.ToString();
                    textBoxNombre.Text = pokemon.Nombre;
                    textBoxDescripcion.Text = pokemon.Descripcion;
                    textBoxUrlImagen.Text = pokemon.UrlImagen;
                    CargarImagen(pokemon.UrlImagen);
                    cboTipo.SelectedValue = pokemon.Tipo.id;// muestro el valor seleccionado en el cbo
                    cboDebilidad.SelectedValue = pokemon.Debilidad.id;

                }// si es distinto de null lo cargo con los datos del pokemon seleccionado

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void textBoxUrlImagen_Leave(object sender, EventArgs e)
        {
            CargarImagen(textBoxUrlImagen.Text);//llamo ese metodo 
        }

        private void CargarImagen(string imagen)
        {
            try
            {
                pbxPokemon.Load(imagen);
            }
            catch (Exception)
            {
                pbxPokemon.Load("https://w7.pngwing.com/pngs/819/548/png-transparent-photo-image-landscape-icon-images-thumbnail.png");

            }
        }//copie el metodo de el form principal para usarlo aca 

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "Archivos de imagen|*.jpg;*.png";
            if (archivo.ShowDialog() == DialogResult.OK)
            {
                textBoxUrlImagen.Text = archivo.FileName;
                CargarImagen(archivo.FileName);
            }

        }
    }
}
