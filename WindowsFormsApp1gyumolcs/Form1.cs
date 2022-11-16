using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace WindowsFormsApp1gyumolcs
{
    public partial class Form1 : Form
    {
        MySqlConnection conn = null;
        MySqlCommand cmd = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = "localhost";
            builder.UserID = "root";
            builder.Password = "";
            builder.Database = "gyumolcsok";
            conn = new MySqlConnection(builder.ConnectionString);
            try
            {
                conn.Open();
                cmd = conn.CreateCommand();
            }
            catch (MySqlException ex)
            {

                MessageBox.Show(ex.Message + Environment.NewLine + "A program leáll");

                Environment.Exit(0);
            }
            finally
            {
                conn.Close();
            }
            listBox_csumolcs_update();

        }

        private void listBox_csumolcs_update()
        {
            listBox_csumolcsok.Items.Clear();
            cmd.CommandText = "SELECT * FROM `gyumolcsok`;";
            conn.Open();
            using (MySqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    Gyumolcs uj = new Gyumolcs(dr.GetInt32("darab"), dr.GetString("fajta"), dr.GetInt32("meret"), dr.GetString("szin"));
                    listBox_csumolcsok.Items.Add(uj);
                }
            }
            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Adjon meg darabszamot");
                textBox1.Focus();
                return;
            }
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Adjon meg fajtát");
                textBox2.Focus();
                return;
            }
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Adjon meg meretett");
                textBox3.Focus();
                return;

            }
            if (string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("Adjon meg színt");
                textBox4.Focus();
                return;

            }


            cmd.CommandText = "INSERT INTO `gyumolcsok`(`darab`, `meret`, `fajta` ,`szin`) VALUES (@darab, @meret, @fajta ,@szin)";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@darab", textBox1.Text);
            cmd.Parameters.AddWithValue("@meret", textBox3.Text);
            cmd.Parameters.AddWithValue("@fajta", textBox2.Text);
            cmd.Parameters.AddWithValue("@szin", textBox4.Text);
            conn.Open();
            try
            {
                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Sikeresen rögzítve!");
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";

                }
                else
                {
                    MessageBox.Show("sikertelen rögzítés!");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox_csumolcsok.SelectedIndex < 0)
            {
                MessageBox.Show("Nincs kijelölve gyümölcs!");
                return;
            }
            cmd.Parameters.Clear();
            Gyumolcs gyumolcs = (Gyumolcs)listBox_csumolcsok.SelectedItem;
            cmd.CommandText = "UPDATE `gyumolcsok` SET `fajta`= @fajta,`meret`= @meret,`szin`= @szin WHERE `darab` = @darab";
            cmd.Parameters.AddWithValue("@darab", textBox1.Text);
            cmd.Parameters.AddWithValue("@fajta", textBox2.Text);
            cmd.Parameters.AddWithValue("@meret", textBox3.Text);
            cmd.Parameters.AddWithValue("@szin", textBox4.Text);
            conn.Open();
            if (cmd.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Módosítás sikeres votl!");
                conn.Close();
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";

                listBox_csumolcs_update();
            }
            else
            {
                MessageBox.Show("Az adatok módosítása sikertelen!");
            }
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }


        private void listBox_Autok_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox_csumolcsok.SelectedIndex <0)
            {
                return;
            }
            Gyumolcs gyumolcs = (Gyumolcs)listBox_csumolcsok.SelectedItem;
            textBox1.Text = gyumolcs.DB.ToString();
            textBox2.Text = gyumolcs.Fajta;
            textBox3.Text = gyumolcs.Meret.ToString();
            textBox4.Text = gyumolcs.Szin.ToString();



        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox_csumolcsok.SelectedIndex < 0)
            {
                MessageBox.Show("nincs gyumolcs");
                return;
            }
            cmd.CommandText = "DELETE FROM `gyumolcsok` WHERE darab = @darab";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@darab", textBox1.Text);
            cmd.Parameters.AddWithValue("@meret", textBox3.Text);
            cmd.Parameters.AddWithValue("@fajta", textBox2.Text);
            cmd.Parameters.AddWithValue("@szin", textBox4.Text);

            conn.Open();
            if (cmd.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Sikeresen törlés!");
                conn.Close();
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";

                listBox_csumolcs_update();
            }
            else
            {
                MessageBox.Show("sikertelen  törlés!");
            }

        }
    }
    
}
