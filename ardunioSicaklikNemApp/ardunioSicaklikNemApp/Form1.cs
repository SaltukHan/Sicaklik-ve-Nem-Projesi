using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Data.SqlClient;

namespace ardunioSicaklikNemApp
{
    public partial class Form1 : Form
    {
        string veri;
        DateTime zaman = DateTime.Now;
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection("Data Source=SALTUKHAN;Initial Catalog=ardunioData;Integrated Security=True");

        private void Form1_Load(object sender, EventArgs e)
        {

            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
                comboBox1.Items.Add(port);
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(SerialPort1_DataReceived);

            listeleme();
            dataGridView1.Columns[0].Visible = false;
        }

        private void SerialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            veri = serialPort1.ReadLine();
            this.Invoke(new EventHandler(displayData_event));
        }

        private void displayData_event(object sender, EventArgs e)
        {
            listBox1.Items.Add(DateTime.Now.ToString() + "  -  " + veri);

            char[] ayrac = { ',',':' };
            String okunanDeger = veri;
            String[] geciciDegisken = okunanDeger.Split(ayrac);

            String Sicaklik = (geciciDegisken[1].ToString());
            String Nem = (geciciDegisken[3].ToString());

            textBox1.Text = Nem;
            textBox2.Text = Sicaklik + "°C";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.Text == "")
                {
                    MessageBox.Show("Lütfen port seçiniz", "Uyarı");
                }
                else
                {
                    serialPort1.PortName = comboBox1.Text;
                    serialPort1.BaudRate = 57600;
                    serialPort1.Open();

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata");
            }

            textBox3.Text = zaman.ToShortTimeString();
            textBox4.Text = zaman.ToShortDateString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
        }

        void listeleme()
        {
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = baglanti;
                cmd.CommandText = "select * from dhtDatas";
                SqlDataAdapter adpr = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adpr.Fill(ds, "dhtDatas");
                dataGridView1.DataSource = ds.Tables["dhtDatas"];
                dataGridView1.Columns[0].Visible = false;
                baglanti.Close();

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = baglanti;
                cmd.CommandText = "INSERT INTO dhtDatas(sicaklik,nem,tarih,saat) VALUES('" + textBox2.Text + "','" + textBox1.Text + "', '" + textBox4.Text + "','"+ textBox3.Text + "') ";
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                baglanti.Close();
                listeleme();
                MessageBox.Show("KAYIT BAŞARIYLA TAMAMLANMIŞTIR!");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("SİLMEK İSTEDİĞİNİZE EMİN MİSİNİZ?", "DİKKAT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (baglanti.State == ConnectionState.Closed)
                {
                    baglanti.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = baglanti;
                    cmd.CommandText = "delete from dhtDatas where id=@numara";
                    cmd.Parameters.AddWithValue("@numara", dataGridView1.CurrentRow.Cells[0].Value.ToString());
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    baglanti.Close();
                    MessageBox.Show("KAYIT SİLİNMİŞTİR");
                    listeleme();
                }
            }
        }
    }
}
