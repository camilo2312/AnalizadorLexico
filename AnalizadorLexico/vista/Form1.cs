using AnalizadorLexico.model;
using AnalizadorLexico.vista;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnalizadorLexico
{
    public partial class Form1 : Form
    {
        model.AnalizadorLexico analizadorLexico;
        PalabrasReservadas palabrasReservadas;
        public Form1()
        {
            InitializeComponent();
            analizadorLexico = new model.AnalizadorLexico(txtCodigo.Text);
            palabrasReservadas = new PalabrasReservadas();
        }

        /// <summary>
        /// Método que se ejecuta cuando el usuario escribe y de inmediato se realiza el análisis
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCodigo_KeyUp(object sender, KeyEventArgs e)
        {
            if (!txtCodigo.Equals(""))
            {
                analizar(false);
                resaltarPalabrasReservadas(txtCodigo.SelectionStart);
            }
            else 
            {
                dataGridTokens.DataSource = null;
            }
        }

        /// <summary>
        /// Método que se ejecuta cuando el usuario da click en el botón de salir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Método que se ejecuta cuando el usuario da click en el botón de limpiar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtCodigo.Clear();
            analizadorLexico = new model.AnalizadorLexico("");
            dataGridTokens.Rows.Clear();
            dataGridTokens.Refresh();
        }

        /// <summary>
        /// Método que se ejecuta cuando el usuario da click en el botón de guardar el archivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardarArchivo_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCodigo.Text))
            {
                NombreArchivo nombreArchivo = new NombreArchivo();
                nombreArchivo.ShowDialog(this);
                using (StreamWriter sw = new StreamWriter($"D:\\CodigoLenguaje\\{nombreArchivo.nombreArchivo}.txt"))
                {
                    sw.Write(txtCodigo.Text);
                    sw.Flush();
                    sw.Close();
                    MessageBox.Show("Archivo guardado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Debe ingresar un valor para poder realizar el guardado del archivo", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Método que se ejecuta cuando el usuario da click en el botón de importar archivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportarArchivo_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.DefaultExt = "txt";
            openFile.Filter = "Text|*.txt|All|*.*";

            if (openFile.ShowDialog(this) == DialogResult.OK) 
            {
                txtCodigo.Text = File.ReadAllText(openFile.FileName);
                analizar(true);
            }
        }

        /// <summary>
        /// Método que realiza el analisis de los tokens
        /// </summary>
        /// <param name="desdeArchivo">Parametros que permite saber si el usuario cargo un archivo para su lectura</param>
        private void analizar(bool desdeArchivo)
        {
            analizadorLexico.codigo = txtCodigo.Text;
            analizadorLexico.lstTokens = new List<Token>();
            analizadorLexico.extraerTokens();

            var source = new BindingSource();
            source.DataSource = analizadorLexico.lstTokens.Where(x => x.categoria != Categoria.ESPACIO).ToList();
            dataGridTokens.DataSource = source;


            if (desdeArchivo)
            {
                for (int i = 1; i < txtCodigo.Text.Length; i++)
                {
                    resaltarPalabrasReservadas(i);
                }
            }
        }

        /// <summary>
        /// Método que permite resaltar las palabras reservadas del lenguajr
        /// </summary>
        /// <param name="posicion"></param>
        private void resaltarPalabrasReservadas(int posicion) 
        {
            int currentPosition = posicion;
            int lastSpacePos = txtCodigo.Text.LastIndexOf((char)Keys.Space, currentPosition - 1);
            lastSpacePos = lastSpacePos > -1 ? lastSpacePos + 1 : 0;

            string lastWord = txtCodigo.Text.Substring(lastSpacePos, currentPosition - (lastSpacePos));
            string result = palabrasReservadas.lstPalabrasReservadas.FirstOrDefault(s => s == lastWord.Replace("\n", "").ToUpper());
            txtCodigo.Select(lastSpacePos, currentPosition - lastSpacePos);
            if (result != null)
            {
                txtCodigo.SelectionColor = Color.Blue;
            }
            else
            {
                txtCodigo.SelectionColor = Color.Black;
            }
            txtCodigo.SelectionStart = currentPosition;
            txtCodigo.SelectionLength = 0;
            txtCodigo.SelectionColor = txtCodigo.ForeColor;
        }
    }
}
