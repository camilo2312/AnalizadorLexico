using AnalizadorLexico.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        private void txtCodigo_KeyUp(object sender, KeyEventArgs e)
        {
            if (!txtCodigo.Equals(""))
            {
                analizadorLexico.codigo = txtCodigo.Text.Replace("\n", "");
                analizadorLexico.lstTokens = new List<Token>();
                analizadorLexico.extraerTokens();

                var source = new BindingSource();
                source.DataSource = analizadorLexico.lstTokens;
                dataGridTokens.DataSource = source;
            }
            else 
            {
                dataGridTokens.DataSource = null;
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtCodigo.Clear();
            analizadorLexico = new model.AnalizadorLexico("");
            dataGridTokens.Rows.Clear();
            dataGridTokens.Refresh();
        }

        private void txtCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCodigo.Text)) 
            {
                int currentPosition = txtCodigo.SelectionStart;

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
}
