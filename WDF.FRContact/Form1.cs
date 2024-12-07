using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Text.RegularExpressions;

namespace WDF.FRContact
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnSuccess_Click(object sender, EventArgs e)
        {


            if (isValidateDataCompleted() == true)
            {

                if (isolddata() == false)
                {
                    insert();
                    clear();
                }
                else
                {
                    Updateform();
                    //clear();
                }




            }

        }

        private void insert()
        {
            string server = "DESKTOP-JG4CG59\\SQLEXPRESS";
            string database = "46MKT";
            string username = "sa";
            string password = "passw0rd";

            string connectionString = $"Data Source={server};Initial Catalog={database};User ID={username};Password={password};";

            string insertQuery = @"
INSERT INTO [dbo].[FRContact]
           ([StaffCode]
           ,[Project]
           ,[Name]
           ,[LastName]
           ,[NickName]
           ,[PhoneNumber]
           ,[BirthDay]
           ,[IDNumber]
           ,[Bank]
           ,[BankAccount]
           ,[Email]
           ,[Active])
     VALUES
           (@StaffCode
           ,@Project
           ,@Name
           ,@LastName
           ,@NickName
           ,@PhoneNumber
           ,@BirthDay
           ,@IDNumber
           ,@Bank
           ,@BankAccount
           ,@Email
           ,@Active)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(insertQuery, connection);
                command.Parameters.AddWithValue("@StaffCode", txtCode.Text);
                command.Parameters.AddWithValue("@Project", CbbProject.Text); // ใส่ค่าจาก text box สำหรับชื่อ
                command.Parameters.AddWithValue("@Name", txtName.Text); // ใส่ค่าจาก text box สำหรับนามสกุล
                command.Parameters.AddWithValue("@LastName", txtLName.Text);
                command.Parameters.AddWithValue("@NickName", TxtNickname.Text);
                command.Parameters.AddWithValue("@PhoneNumber", TxtTel.Text);
                command.Parameters.AddWithValue("@Active", "Y");
                command.Parameters.AddWithValue("@IDNumber", TxtIdNumber.Text);
                command.Parameters.AddWithValue("@BankAccount", TxtBAccount.Text);
                command.Parameters.AddWithValue("@Email", TxtEmail.Text);
                command.Parameters.AddWithValue("@BirthDay", DtBd.Value.ToString("yyyy/MM/dd"));
                command.Parameters.AddWithValue("@Bank", CbbBank.Text);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("เพิ่มข้อมูลเรียบร้อยแล้ว!");
                    }
                    else
                    {
                        MessageBox.Show("เพิ่มข้อมูลล้มเหลว");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
                }


            }


        }
        private Boolean isValidateDataCompleted()


        {
            string selectedValue = CbbProject.SelectedItem?.ToString() ; 

            if (string.IsNullOrEmpty(selectedValue) || selectedValue == "โปรดเลือก")
            {
                MessageBox.Show("กรุณาเลือกข้อมูลใน ComboBox");
                return false; // หยุดการดำเนินการต่อ
            }
            if (txtCode.Text.Trim().Length == 0 || txtName.Text.Trim().Length == 0 || txtLName.Text.Trim().Length == 0 || TxtNickname.Text.Trim().Length == 0 || TxtTel.Text.Trim().Length == 0 || TxtIdNumber.Text.Trim().Length == 0 || TxtBAccount.Text.Trim().Length == 0 || TxtEmail.Text.Trim().Length == 0)
            {
                MessageBox.Show("กรุณากรอกข้อมูล");
                return false;

            }


            return true;

        }

        private void clear()
        {
            CbbBank.Items.Clear();
            CbbProject.Items.Clear();
            txtCode.Text = "";
            txtName.Text = "";
            txtLName.Text = "";
            TxtNickname.Text = "";
            TxtTel.Text = "";
            TxtIdNumber.Text = "";
            DtBd.Text = "";
            TxtBAccount.Text = "";
            TxtEmail.Text = "";


        }
       

        private void TxtIdNumber_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // ยกเลิกการป้อนตัวอักษร
            }



        }

        private void txtCode_TextChanged(object sender, EventArgs e)
        {
            string input = txtCode.Text;
            string pattern = @"^[a-zA-Z0-9]*$"; // Regular Expression สำหรับตรวจสอบภาษาอังกฤษและตัวเลขเท่านั้น

            if (!Regex.IsMatch(input, pattern))
            {
                MessageBox.Show("โปรดป้อนเฉพาะภาษาอังกฤษและตัวเลขเท่านั้น", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // ลบอักขระที่ไม่ถูกต้องออกจาก TextBox
                txtCode.Text = Regex.Replace(input, "[^a-zA-Z0-9]", "");
                txtCode.Select(txtCode.Text.Length, 0); // นำเคอร์เซอร์ไปที่จุดสุดท้ายของข้อความ
            }
        }

        //public void SetFormbyID(string staffCode)
        //{
        //    string server = "DESKTOP-JG4CG59\\SQLEXPRESS";
        //    string database = "46MKT";
        //    string username = "sa";
        //    string password = "passw0rd";

        //    string connectionString = $"Data Source={server};Initial Catalog={database};User ID={username};Password={password};";

        //    string query = "select * from FRContact";
        //    query += " where StaffCode = " + staffCode;

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {

        //        using (SqlCommand command = new SqlCommand(query, connection))
        //        {
        //            //// เพิ่มพารามิเตอร์และกำหนดค่า
        //            //command.Parameters.AddWithValue("@SearchValue", searchValue);
        //            connection.Open();
        //            // สร้าง DataReader เพื่อดึงข้อมูล
        //            using (SqlDataReader reader = command.ExecuteReader())
        //            {
        //                // ตรวจสอบว่ามีข้อมูลหรือไม่
        //                if (reader.HasRows)
        //                {
        //                    while (reader.Read())
        //                    {
        //                        // ดึงข้อมูลจากแต่ละคอลัมน์
        //                        //int id = reader.GetInt32(reader.GetOrdinal("Id"));
        //                        //string name = reader.GetString(reader.GetOrdinal("Name"));
        //                        LBCheck.Text = staffCode;
        //                        CbbProject.Text = reader.GetString(reader.GetOrdinal("Project"));
        //                        txtName.Text = reader.GetString(reader.GetOrdinal("Name"));
        //                        txtLName.Text = reader.GetString(reader.GetOrdinal("LastName"));
        //                        TxtNickname.Text = reader.GetString(reader.GetOrdinal("NickName"));
        //                       TxtTel.Text = reader.GetString(reader.GetOrdinal("PhoneNumber"));
        //                        TxtIdNumber.Text = reader.GetString(reader.GetOrdinal("IDNumber"));
        //                        CbbBank.Text = reader.GetString(reader.GetOrdinal("Bank"));
        //                        TxtBAccount.Text = reader.GetString(reader.GetOrdinal("BankAccount"));
        //                        TxtEmail.Text = reader.GetString(reader.GetOrdinal("Email"));


        //                        string Bdstring = reader.GetString(reader.GetOrdinal("Birthday"));

        //                        DtBd.Value = DateTime.Parse(Bdstring);


        //                        //// ทำสิ่งที่ต้องการกับข้อมูลที่ได้ เช่น แสดงผลหรือเก็บไว้ในรายการ
        //                        //Console.WriteLine($"ID: {id}, Name: {name}");
        //                    }
        //                }
        //                else
        //                {
        //                    Console.WriteLine("No data found.");
        //                }
        //            }
        //        }

        //        connection.Close();
        //    }
        //}


        private void Updateform()
        {
            string server = "DESKTOP-JG4CG59\\SQLEXPRESS";
            string database = "46MKT";
            string username = "sa";
            string password = "passw0rd";

            string connectionString = $"Data Source={server};Initial Catalog={database};User ID={username};Password={password};";

            string sql = "update FRContact";
            sql += " set StaffCode = '" + txtCode.Text + "' , Project = '" + CbbProject.Text + "' , Name = '" + txtName.Text + "', LastName = '" + txtLName.Text + "' , NickName = '" + TxtNickname.Text + "' , PhoneNumber = '" + TxtTel.Text + "' , IDNumber = '" + TxtIdNumber.Text + "' , Bank = '" + CbbBank.Text + "' , BankAccount = '" + TxtBAccount.Text + "' , Email = '" + TxtEmail.Text + "'";
            sql += " where Staffcode = '" + txtCode.Text + "'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("แก้ไขข้อมูลเรียบร้อยแล้ว!");
                    }
                    else
                    {
                        MessageBox.Show("แก้ไขข้อมูลล้มเหลว");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
                }
            }

        }

        private void TxtTel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // ยกเลิกการป้อนตัวอักษร
            }
        }

        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                getdatabystaffcode();
            }
        }

        private Boolean isolddata()
        {
            string server = "DESKTOP-JG4CG59\\SQLEXPRESS";
            string database = "46MKT";
            string username = "sa";
            string password = "passw0rd";

            string connectionString = $"Data Source={server};Initial Catalog={database};User ID={username};Password={password};";
            string sql = "select * from FRContact";
            sql += " where staffcode ='" + txtCode.Text +"'";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    //// เพิ่มพารามิเตอร์และกำหนดค่า
                    //command.Parameters.AddWithValue("@SearchValue", searchValue);
                    connection.Open();
                    // สร้าง DataReader เพื่อดึงข้อมูล
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // ตรวจสอบว่ามีข้อมูลหรือไม่
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                // ดึงข้อมูลจากแต่ละคอลัมน์
                                //int id = reader.GetInt32(reader.GetOrdinal("Id"));
                                //string name = reader.GetString(reader.GetOrdinal("Name"));
                                LBCheck.Text = reader.GetString(reader.GetOrdinal("StaffCode"));
                                txtCode.Text = reader.GetString(reader.GetOrdinal("StaffCode"));


                                //CbbProject.Text = reader.GetString(reader.GetOrdinal("Project"));
                                //txtName.Text = reader.GetString(reader.GetOrdinal("Name"));
                                //txtLName.Text = reader.GetString(reader.GetOrdinal("LastName"));
                                //TxtNickname.Text = reader.GetString(reader.GetOrdinal("NickName"));
                                //TxtTel.Text = reader.GetString(reader.GetOrdinal("PhoneNumber"));
                                //TxtIdNumber.Text = reader.GetString(reader.GetOrdinal("IDNumber"));
                                //CbbBank.Text = reader.GetString(reader.GetOrdinal("Bank"));
                                //TxtBAccount.Text = reader.GetString(reader.GetOrdinal("BankAccount"));
                                //TxtEmail.Text = reader.GetString(reader.GetOrdinal("Email"));


                                ////string Bdstring = reader.GetString(reader.GetOrdinal("Birthday"));

                                //DtBd.Value = reader.GetDateTime(reader.GetOrdinal("BirthDay"));


                                //// ทำสิ่งที่ต้องการกับข้อมูลที่ได้ เช่น แสดงผลหรือเก็บไว้ในรายการ
                                //Console.WriteLine($"ID: {id}, Name: {name}");
                                return true;
                            }
                        }
                        else
                        {
                            
                            Console.WriteLine("No data found.");
                        }
                        return false;
                    }
                }

                connection.Close();
            }
        }

        private void getdatabystaffcode()
        {
            string server = "DESKTOP-JG4CG59\\SQLEXPRESS";
            string database = "46MKT";
            string username = "sa";
            string password = "passw0rd";

            string connectionString = $"Data Source={server};Initial Catalog={database};User ID={username};Password={password};";
            string sql = "select * from FRContact";
            sql += " where staffcode ='" + txtCode.Text + "'";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    //// เพิ่มพารามิเตอร์และกำหนดค่า
                    //command.Parameters.AddWithValue("@SearchValue", searchValue);
                    connection.Open();
                    // สร้าง DataReader เพื่อดึงข้อมูล
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // ตรวจสอบว่ามีข้อมูลหรือไม่
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                // ดึงข้อมูลจากแต่ละคอลัมน์
                                //int id = reader.GetInt32(reader.GetOrdinal("Id"));
                                //string name = reader.GetString(reader.GetOrdinal("Name"));
                                LBCheck.Text = reader.GetString(reader.GetOrdinal("StaffCode"));
                                txtCode.Text = reader.GetString(reader.GetOrdinal("StaffCode"));


                                CbbProject.Text = reader.GetString(reader.GetOrdinal("Project"));
                                txtName.Text = reader.GetString(reader.GetOrdinal("Name"));
                                txtLName.Text = reader.GetString(reader.GetOrdinal("LastName"));
                                TxtNickname.Text = reader.GetString(reader.GetOrdinal("NickName"));
                                TxtTel.Text = reader.GetString(reader.GetOrdinal("PhoneNumber"));
                                TxtIdNumber.Text = reader.GetString(reader.GetOrdinal("IDNumber"));
                                CbbBank.Text = reader.GetString(reader.GetOrdinal("Bank"));
                                TxtBAccount.Text = reader.GetString(reader.GetOrdinal("BankAccount"));
                                TxtEmail.Text = reader.GetString(reader.GetOrdinal("Email"));


                                //string Bdstring = reader.GetString(reader.GetOrdinal("Birthday"));

                                DateTime bdtime = reader.GetDateTime(reader.GetOrdinal("BirthDay"));
                                DtBd.Value = bdtime.AddYears(-543);


                            }
                        }
                        else
                        {

                            Console.WriteLine("No data found.");
                        }
                        
                    }
                }

                connection.Close();
            }
        }

    }
}




