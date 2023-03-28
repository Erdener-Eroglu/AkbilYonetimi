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

namespace AkbilYonetimiUI
{
    public partial class FrmAkbiller : Form
    {
        string baglantiCumlesi = @"Server=MONSTER; Database=AkbilDB; Trusted_Connection=True;";
        public FrmAkbiller()
        {
            InitializeComponent();
        }

        private void anaMenüToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                //kontroller
                if (cmbAkbilTipler.SelectedIndex < 0)
                {
                    MessageBox.Show("Lütfen ekleyeceğiniz akbilin türünü seçiniz.");
                    return;
                }
                //komut.CommandType = CommandType.Text;
                SqlConnection baglanti = new SqlConnection(baglantiCumlesi);
                string sorgu = "insert into Akbiller (AkbilNo,EklenmeTarihi,AkbilTipi,Bakiye,AkbilSahibiId,VizelendigiTarihi) values (@akblNo,@ektrh, @tip, @bakiye, @sahibi,null)";
                SqlCommand komut = new SqlCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@akblNo", maskedTextBoxAkbilNo.Text);
                komut.Parameters.AddWithValue("@ektrh", DateTime.Now);
                komut.Parameters.AddWithValue("@tip", cmbAkbilTipler.SelectedItem);
                komut.Parameters.AddWithValue("@bakiye", 0);
                komut.Parameters.AddWithValue("@sahibi", Properties.Settings1.Default.KullaniciId);
                baglanti.Open();
                if (komut.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Akbil eklenmiştir.");
                    maskedTextBoxAkbilNo.Clear();
                    cmbAkbilTipler.SelectedIndex = -1;
                    cmbAkbilTipler.Text = "Akbil türünü seçiniz...";
                    DataGridViewiDoldur();

                }
                else
                {
                    MessageBox.Show("Akbiliniz eklenememiştir.");
                }
                baglanti.Close();
            }
            catch (Exception hata)
            {
                MessageBox.Show("Bir hata oluştu" + hata.Message);
            }
        }

        private void FrmAkbiller_Load(object sender, EventArgs e)
        {
            cmbAkbilTipler.Text = "Akbil tipi seçiniz....";
            cmbAkbilTipler.SelectedIndex = -1;
            dataGridViewAkbiller.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DataGridViewiDoldur();
        }

        private void DataGridViewiDoldur()
        {
            try
            {
                SqlConnection connection = new SqlConnection(baglantiCumlesi);
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "select * from Akbiller where AkbilSahibiId = @sahibi";
                command.Parameters.AddWithValue("@sahibi", Properties.Settings1.Default.KullaniciId);
                //Data table
                //Data set --> içinde birden çok datatable barındırır.
                //SQLDataAdapter --> adaptör sorgu sonucundaki verileri Datatable/Datasete doldurur. (Fill)
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataTable dt = new DataTable();
                connection.Open();
                adp.Fill(dt);
                connection.Close();
                dataGridViewAkbiller.DataSource = dt;
                //Bazı kolonlar Gizlensin
                dataGridViewAkbiller.Columns["AkbilSahibiId"].Visible = false;
                dataGridViewAkbiller.Columns["VizelendigiTarihi"].HeaderText = "Vizelendiği Tarih";
                dataGridViewAkbiller.Columns["VizelendigiTarihi"].Width = 200;
            }
            catch (Exception hata)
            {
                MessageBox.Show("Akbilleri listeleyemedim" + hata.Message);
            }
        }
    }
}
