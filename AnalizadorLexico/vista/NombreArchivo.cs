using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnalizadorLexico.vista
{
    public partial class NombreArchivo : Form
    {
        public string nombreArchivo { get; set; }
        public NombreArchivo()
        {
            InitializeComponent();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNombreArchivo.Text))
            {
                nombreArchivo = txtNombreArchivo.Text;
                Close();
            }
            else
            {
                MessageBox.Show("Debe ingresar el nombre del archivo", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
