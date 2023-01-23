using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Hospitalform
{
    public partial class Form1 : Form
    {
        SQLiteConnection con = new SQLiteConnection(@"Data Source=C:\Users\shamr\OneDrive\Desktop\vspjf\hos.db");
        SQLiteCommand cmd = new SQLiteCommand();
        SQLiteDataAdapter da = new SQLiteDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt;
        SQLiteDataReader dr;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            con.Open();
            string strCmd = "SELECT num, type FROM bloddtype ORDER BY num";
            cmd = new SQLiteCommand(strCmd, con);

            da = new SQLiteDataAdapter(strCmd, con);

            dt = new DataTable();
            da.Fill(dt);
            string striCmd = "SELECT num, gend FROM genderss ORDER BY num";
            cmd = new SQLiteCommand(striCmd, con);

            da = new SQLiteDataAdapter(strCmd, con);

            dt = new DataTable();
            da.Fill(dt);


            comboBox1.DisplayMember = "type";
            comboBox1.ValueMember = "num";
            comboBox1.DataSource = dt;
            comboBox2.DisplayMember = "type";
            comboBox2.ValueMember = "num";
            comboBox2.DataSource = dt;
            comboBox3.DisplayMember = "gend";
            comboBox3.ValueMember = "num";
            comboBox3.DataSource = dt;
            comboBox4.DisplayMember = "gend";
            comboBox4.ValueMember = "num";
            comboBox4.DataSource = dt;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "Insert Into patients (PatientID, FirstName, LastName, Email, phone, BloodType, Gender)" +
               "values (@ptid, @ptFname, @ptSname,@ptemail, @ptphone, @ptBloodtype, @ptGender)";
            cmd.Parameters.AddWithValue("ptid", IDtxt.Text);
            cmd.Parameters.AddWithValue("ptFname", Fname.Text);
            cmd.Parameters.AddWithValue("ptSname", Sname.Text);
            cmd.Parameters.AddWithValue("ptemail", Email.Text);
            cmd.Parameters.AddWithValue("ptphone", int.Parse(phone.Text));
            cmd.Parameters.AddWithValue("ptBloodtype", comboBox1.SelectedItem);
            cmd.Parameters.AddWithValue("ptgender", comboBox3.SelectedItem);
            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected == 0)
                MessageBox.Show("Error .. Record Not Added");
            else
                MessageBox.Show("Record Inserted Successfully");

            con.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            con.Open();

            
            cmd.Connection = con; 
            cmd.CommandText = "SELECT * from patients WHERE PatientID=@ptid";

            cmd.Parameters.AddWithValue("ptid", sbox.Text);

            dr = cmd.ExecuteReader();

            if (!dr.HasRows)
                MessageBox.Show("ID entred wasn't available");
            else
            {
                
                groupBox3.Show();
                

                button3.Show();
                button4.Show();

                while (dr.Read())
                {
                    
                    fn.Text = dr["FirstName"].ToString();
                    sn.Text = dr["LastName"].ToString();
                    em.Text = dr["Email"].ToString();
                    ph.Text = dr["Phone"].ToString();
                    int Bloodt = Convert.ToInt32(dr["BloodType"].ToString());
                    comboBox2.SelectedValue = Bloodt;
                    int gend = Convert.ToInt32(dr["Gender"].ToString());
                    comboBox2.SelectedValue = gend;



                }
            }

       
            dr.Close();
            con.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            con.Open();
            try
            {
                
                cmd.Connection = con; 
                cmd.CommandText = "Update Students SET FirstName=@ptFname,LastName=@ptSname, Email=@ptemail,  phone=@ptphone,Gender=@Ptgender, BloodType=@ptBloodtype " +
                    " WHERE PatientID=@ptid";

                cmd.Parameters.AddWithValue("ptid", IDtxt.Text);
                cmd.Parameters.AddWithValue("ptFname", Fname.Text);
                cmd.Parameters.AddWithValue("ptSname", Sname.Text);
                cmd.Parameters.AddWithValue("ptemail", Email.Text);
                cmd.Parameters.AddWithValue("ptphone", int.Parse(phone.Text));
                cmd.Parameters.AddWithValue("ptBloodtype", comboBox1.SelectedItem);
                cmd.Parameters.AddWithValue("ptgender", comboBox3.SelectedItem);

              
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0)
                    MessageBox.Show("Record Not Updated due to an error");
                else
                    MessageBox.Show("Record Updated");
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }
            finally
            {   
                con.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "DELETE FROM patients " +
                " WHERE PatientID=@ptid";
            cmd.Parameters.AddWithValue("ptid", sbox.Text);
            DialogResult result = MessageBox.Show("Are you Sure you want to delete this Recor?",
                "Delete Record", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0)
                    MessageBox.Show("Record Not Deleted due to an error");
                else
                    MessageBox.Show("Record Deleted");
            }  
            con.Close();

        }
        private void Database_Enter(object sender, EventArgs e)
        {
            con.Open();
            string strCmd = "SELECT * FROM patients ORDER BY PatientID";
            cmd = new SQLiteCommand(strCmd, con);
            da = new SQLiteDataAdapter(strCmd, con);
            ds = new DataSet();
            da.Fill(ds, "patients");
            dataGridView1.DataSource = ds.Tables["patients"].DefaultView;
            // 4- close DB connection
            con.Close();
        }
    }   
    
}
