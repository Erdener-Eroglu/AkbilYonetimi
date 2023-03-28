using System.Data.SqlClient;

namespace AkbilYonetimiUI;

public partial class FrmGiris : Form
{
    public string Email { get; set; } //Kayıt ol formunda kayıt olan kullanıcının emaili buraya gelsin.
    public FrmGiris()
    {
        InitializeComponent();
    }
    private void FrmGiris_Load(object sender, EventArgs e)
    {
        if (Email != null)
        {
            txtEmail.Text = Email;
        }
        txtSifre.PasswordChar = '*';
        if (Properties.Settings1.Default.BeniHatirla == true)
        {
            txtEmail.Text = Properties.Settings1.Default.KullaniciEmail;
            txtSifre.Text = Properties.Settings1.Default.KullaniciSifre;
            checkBoxHatirla.Checked = true;
        }
    }

    private void btnKayitOl_Click(object sender, EventArgs e)
    {
        this.Hide();
        FrmKayitOl frm = new FrmKayitOl();
        frm.Show();
    }

    private void btnGiris_Click(object sender, EventArgs e)
    {
        GirisYap();
    }

    private void GirisYap()
    {
        try
        {
            //1) Email ve şifre text boxları dolu mu?
            if (string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtSifre.Text))
            {
                MessageBox.Show("Bilgileri eksiksiz giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }
            //2)Girdiği email ve  şifre veri tabanında mevcut mu?
            string baglantiCumlesi = @"Server=MONSTER; Database=AkbilDB; Trusted_Connection=True;";
            SqlConnection baglanti = new SqlConnection(baglantiCumlesi);
            string sorgu = $"select * from Kullanicilar (nolock) where Email='{txtEmail.Text.Trim()}' and Parola='{txtSifre.Text.Trim()}'";
            SqlCommand komut = new SqlCommand(sorgu, baglanti);
            baglanti.Open();
            SqlDataReader okuyucu = komut.ExecuteReader();
            if (!okuyucu.HasRows)
            {
                //değilse yanlış giriş mesajı verecek.
                MessageBox.Show("E-mail ya da şifre yanlış", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                baglanti.Close();
                return;
            }
            else
            {
                //hoşgeldiniz yazacak ve anasayfa formuna yönlendirilecek
                while (okuyucu.Read())
                {
                    MessageBox.Show($"HOŞGELDİNİZ {okuyucu["Ad"]} {okuyucu["Soyad"]}");
                    Properties.Settings1.Default.KullaniciId = (int)okuyucu["ID"];
                }
                baglanti.Close();
            }
            //Eğer email şifre doğru ise
            //Eğer beni hatırla tıklıysa Bilgileri hatırlanacak...
            if (checkBoxHatirla.Checked)
            {
                Properties.Settings1.Default.BeniHatirla = true;
                Properties.Settings1.Default.KullaniciEmail = txtEmail.Text.Trim();
                Properties.Settings1.Default.KullaniciSifre = txtSifre.Text.Trim();
                Properties.Settings1.Default.Save();
            }
            else
            {
                Properties.Settings1.Default.BeniHatirla = false;
                Properties.Settings1.Default.KullaniciEmail = "";
                Properties.Settings1.Default.KullaniciSifre = "";
                Properties.Settings1.Default.Save();
            }
            this.Hide();
            FrmAnaSayfa frmAnaSayfa = new FrmAnaSayfa();
            frmAnaSayfa.Show();

        }
        catch (Exception hata)
        {
            //DipNot exceptionlar asla kullanıcıya gösterilmez
            //Exceptionlar loglanır. Viz şu an çğrenme/geliştirme aşamasında oluğumuz için yazdık.
            MessageBox.Show("Beklenmedik bir sorun oluştu: " + hata.Message, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }
    }

    private void checkBoxHatirla_CheckedChanged(object sender, EventArgs e)
    {
        //Properties.Settings1.Default.BeniHatirla = checkBoxHatirla.Checked;
    }
    private void txtSifre_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar == Convert.ToChar(Keys.Enter))
            GirisYap();
    }
}
