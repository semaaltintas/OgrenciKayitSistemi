using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//System.Data.oledb Kütüphanesinin tanımlanması
using System.Data.OleDb;
//Giriş-Çıkış işlemlerine ilişkin kütüphanenin tanımlanması
using System.IO;
namespace Staj
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //Veri tabanı dosya yolu ve provider nesnesinin belirlenmesi
        OleDbConnection baglantim = new OleDbConnection("Provider=Microsoft.Ace.OleDb.12.0;Data Source=Ogrenci.accdb");
        public static string Ogr_Tc_No, Ogr_No, Ogr_Adi, Ogr_Soyadi, Ogr_Sinif;

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length < 6)
                errorProvider1.SetError(textBox2, "Öğrenci No 6 karakter olmalı!");
            else
                errorProvider1.Clear();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((int)e.KeyChar >= 48 && (int)e.KeyChar <= 57) || (int)e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((int)e.KeyChar >= 48 && (int)e.KeyChar <= 57) || (int)e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void maskedTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) == true || char.IsControl(e.KeyChar) == true || char.IsSeparator(e.KeyChar) == true)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void maskedTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) == true || char.IsControl(e.KeyChar) == true || char.IsSeparator(e.KeyChar) == true)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length < 11)
                errorProvider1.SetError(textBox1, "Tc Kimlik No 11 karakter olmalı!");
            else
                errorProvider1.Clear();
        }
        private void ogrecileri_goster()
        {
            try
            {
                baglantim.Open();
                OleDbDataAdapter ogrencileri_listele = new OleDbDataAdapter("select Ogr_Tc_No AS[Öğrenci Tc Kimlik No]," +
                    "Ogr_No AS[Öğrenci Numarası],Ogr_Adi AS[Öğrenci Adı],Ogr_Soyadi AS[Öğrenci Soyadı],Ogr_Sinifi AS[Öğrenci Sınıfı] " +
                    "from ogrenciler Order By Ogr_Adi ASC", baglantim);
                DataSet dshafiza = new DataSet();
                ogrencileri_listele.Fill(dshafiza);
                dataGridView1.DataSource = dshafiza.Tables[0];
                baglantim.Close();
            }
            catch (Exception hatamsj)
            {
                MessageBox.Show(hatamsj.Message, "Öğrenci Kayıt Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglantim.Close();
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            ogrecileri_goster();
            maskedTextBox1.Mask = "LL???????????????????????";
            maskedTextBox2.Mask = "LL???????????????????????";
            textBox2.MaxLength = 6;
            textBox1.MaxLength = 11;
            toolTip1.SetToolTip(this.textBox1, "Tc Kimlik No 11 Karaketer Olmalı!");
            toolTip2.SetToolTip(this.textBox2, "Öğrenci No 6 Karakter olmalı!");
            maskedTextBox1.Text.ToUpper();
            maskedTextBox2.Text.ToUpper();
            comboBox1.Items.Add("1"); comboBox1.Items.Add("2"); comboBox1.Items.Add("3"); comboBox1.Items.Add("4");
        }
        private void topPage1_temizle()
        {
            textBox1.Clear();textBox2.Clear();maskedTextBox1.Clear();maskedTextBox2.Clear();comboBox1.SelectedIndex = -1;
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            bool kayit_arama_durumu = false;
            if (textBox1.Text.Length == 11)
            {
                baglantim.Open();
                OleDbCommand selectsorgu = new OleDbCommand("select * from ogrenciler where Ogr_Tc_No = '" + textBox1.Text + "'", baglantim);
                OleDbDataReader kayitokuma = selectsorgu.ExecuteReader();
                while (kayitokuma.Read())
                {
                    kayit_arama_durumu = true;
                    textBox2.Text = kayitokuma.GetValue(1).ToString();
                    maskedTextBox1.Text = kayitokuma.GetValue(2).ToString();
                    maskedTextBox2.Text = kayitokuma.GetValue(3).ToString();
                    comboBox1.Text = kayitokuma.GetValue(4).ToString();
                    break;
                }
                if (kayit_arama_durumu == false)
                {
                    MessageBox.Show("Aranan Kayıt Bulunamadı!", "Öğrenci Kayıt Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                baglantim.Close();
            }
            else
            { MessageBox.Show("Lütfe 11 Haneli Tc Kimlik No Giriniz!","Öğrenci Kayıt Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
              topPage1_temizle(); 
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
                //Tc kimlik no kontrol
                if (textBox1.Text.Length < 11 || textBox1.Text == "")
                    label1.ForeColor = Color.Red;
                else
                    label1.ForeColor = Color.Black;
                //Öğrenci no kontrol
                if (textBox2.Text.Length < 6 || textBox2.Text == "")
                    label2.ForeColor = Color.Red;
                else
                    label2.ForeColor = Color.Black;
                //Öğrenci adı kontrol
                if (maskedTextBox1.MaskCompleted == false)
                    label3.ForeColor = Color.Red;
                else
                    label3.ForeColor = Color.Black;
                //Öğrenci soyadı kontrol
                if (maskedTextBox2.MaskCompleted == false)
                    label4.ForeColor = Color.Red;
                else
                    label4.ForeColor = Color.Black;
                //Öğrenci sınıf kontrolu
                if (comboBox1.Text == "")
                    label5.ForeColor = Color.Red;
                else
                    label5.ForeColor = Color.Black;

                if (textBox1.Text.Length == 11 && textBox1.Text != "" && textBox2.Text != "" && textBox2.Text.Length == 6 && maskedTextBox1.MaskCompleted != false && maskedTextBox2.MaskCompleted != false && comboBox1.Text != "")
                {
                    try
                    {
                        baglantim.Open();
                        OleDbCommand guncellekomutu = new OleDbCommand("update ogrenciler set Ogr_No = '" + textBox2.Text + "', Ogr_Adi = '" + maskedTextBox1.Text + "' , Ogr_Soyadi = '" + maskedTextBox2.Text + "' , Ogr_Sinifi = '" + comboBox1.Text + "' where Ogr_Tc_No = '"+textBox1.Text+"'" ,baglantim);
                        guncellekomutu.ExecuteNonQuery();
                        baglantim.Close();
                        MessageBox.Show("Kayıt Güncellendi!", "Öğrenci Kayıt Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        ogrecileri_goster();
                    }
                    catch (Exception hatamsj)
                    {
                        MessageBox.Show(hatamsj.Message,"Öğrenci Kayıt Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        baglantim.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Yazı rengi kırmızı olan alanları gözden geçiriniz!", "Öğrenci Kayıt Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            bool kayitkontrol = false;
            baglantim.Open();
            OleDbCommand selectsorgu = new OleDbCommand("select * from ogrenciler where Ogr_Tc_No='" + textBox1.Text + "'", baglantim);
            OleDbDataReader kayitokuma = selectsorgu.ExecuteReader();
            while (kayitokuma.Read())
            {
                kayitkontrol = true;
                break;
            }
            baglantim.Close();
            if (kayitkontrol == false)
            {
                //Tc kimlik no kontrol
                if (textBox1.Text.Length < 11 || textBox1.Text == "")
                    label1.ForeColor = Color.Red;
                else
                    label1.ForeColor = Color.Black;
                //Öğrenci no kontrol
                if (textBox2.Text.Length < 6 || textBox2.Text == "")
                    label2.ForeColor = Color.Red;
                else
                    label2.ForeColor = Color.Black;
                //Öğrenci adı kontrol
                if (maskedTextBox1.MaskCompleted == false)
                    label3.ForeColor = Color.Red;
                else
                    label3.ForeColor = Color.Black;
                //Öğrenci soyadı kontrol
                if (maskedTextBox2.MaskCompleted == false)
                    label4.ForeColor = Color.Red;
                else
                    label4.ForeColor = Color.Black;
                //Öğrenci sınıf kontrolu
                if (comboBox1.Text == "")
                    label5.ForeColor = Color.Red;
                else
                    label5.ForeColor = Color.Black;

                if(textBox1.Text.Length == 11 && textBox1.Text != "" && textBox2.Text != "" && textBox2.Text.Length == 6 && maskedTextBox1.MaskCompleted != false && maskedTextBox2.MaskCompleted != false && comboBox1.Text != "")
                {
                    try
                    {
                        baglantim.Open();
                        OleDbCommand eklekomutu = new OleDbCommand("insert into ogrenciler values ('" + textBox1.Text + "','" + textBox2.Text + "','" + maskedTextBox1.Text + "','" + maskedTextBox2.Text + "','" + comboBox1.Text + "')", baglantim);
                        eklekomutu.ExecuteNonQuery();
                        baglantim.Close();
                        MessageBox.Show("Yeni Kayıt Oluşturuldu!","Öğrenci Kayıt Sistemi",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                        ogrecileri_goster();
                        topPage1_temizle();
                    }
                    catch(Exception hatamsj)
                    {
                        MessageBox.Show(hatamsj.Message);
                        baglantim.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Yazı rengi kırmızı olan alanları gözden geçiriniz!", "Öğrenci Kayıt Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("Girilen Tc Kimlik Numarası önceden kayıtlıdır!", "Öğrenci Kayıt Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 11)
            {
                bool kayit_arama_durumu = false;
                baglantim.Open();
                OleDbCommand selectsorgu = new OleDbCommand("select * from ogrenciler where Ogr_Tc_No= '" + textBox1.Text + "'", baglantim);
                OleDbDataReader kayitokuma = selectsorgu.ExecuteReader();
                while (kayitokuma.Read())
                {
                    kayit_arama_durumu = true;
                    OleDbCommand deletesorgu = new OleDbCommand("delete from ogrenciler where Ogr_Tc_No='" + textBox1.Text + "'", baglantim);
                    deletesorgu.ExecuteNonQuery();
                    MessageBox.Show("Öğrenci Kaydı Silindi", "Öğrenci Kayıt Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    baglantim.Close();
                    ogrecileri_goster();
                    topPage1_temizle();
                    break;
                }
                if (kayit_arama_durumu == false)
                    MessageBox.Show("Silinecek Kayıt Bulunamadı!", "Öğrenci Kayıt Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglantim.Close();
                topPage1_temizle();
            }
            else
                MessageBox.Show("Lütfen 11 Haneli Tc Kimlik No Giriniz!", "Öğrenci Kayıt Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            topPage1_temizle();
        }
    }
}
