﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AkbilYonetimiUI
{
    public partial class FrmAnaSayfa : Form
    {
        public FrmAnaSayfa()
        {
            InitializeComponent();
        }

        private void btnAyarlar_Click(object sender, EventArgs e)
        {
            this.Hide();
            FrmAyarlar frmAyar = new FrmAyarlar();
            frmAyar.Show();
        }

        private void btnAkbil_Click(object sender, EventArgs e)
        {
            this.Hide();
            FrmAkbiller frmAkbiller = new FrmAkbiller();
            frmAkbiller.Show();
        }
    }
}
