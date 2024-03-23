﻿using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Gaming_Dashboard
{
    public partial class DangNhap : UserControl
    {
        public Main main;
        private UserControl previousUserControl;
        public static string loggedInUsername;
        public DangNhap()
        {
            InitializeComponent();
        }
        private static DangNhap _instance;
        public static DangNhap Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DangNhap();
                return _instance;
            }
        }
        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void DangNhap_Load(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
            if (!pn_DangNhap2.Controls.Contains(QuenMatKhau.Instance))
            {
                pn_DangNhap2.Controls.Add(QuenMatKhau.Instance);
                QuenMatKhau.Instance.Dock = DockStyle.Fill;
                pn_DangNhap2.BringToFront();
                AutoScroll = false;
                QuenMatKhau.Instance.BringToFront();
            }
            else
                pn_DangNhap.BringToFront();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }

        public void guna2Button2_Click(object sender, EventArgs e)
        {
            if (!pn_DangNhap2.Controls.Contains(DangKy1.Instance))
            {
                pn_DangNhap2.Controls.Add(DangKy1.Instance);
                DangKy1.Instance.Dock = DockStyle.Fill;
                pn_DangNhap2.BringToFront();
                AutoScroll = false;
                DangKy1.Instance.BringToFront();
            }
            else
                pn_DangNhap.BringToFront();
        }

        private void guna2ToggleSwitch1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void lbl_DangNhap_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txt_MatKhau_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_Email_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_DangNhap_Click(object sender, EventArgs e)
        {
            string email = txt_DNEmail.Text;
            string password = txt_DNMatKhau.Text;
            loggedInUsername = "";

            // Kiểm tra trường hợp cụ thể email = "DoannhomA" và mật khẩu = "999999"
            if (email == "DoannhomA" && password == "999999")
            {
                // Open the Admin form
                this.Hide();
                var admin = new admin___tke.Admin();
                this.main.Hide();
                admin.ShowDialog();
                return;
            }

            // Xác thực email và mật khẩu
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập email và mật khẩu.");
                return;
            }

            // Hash the entered password before comparing it to the stored hash
            string hashedPassword = HashPassword(password);

            // Thực hiện logic đăng nhập
            bool isLoggedIn = PerformLogin(email, hashedPassword);

            if (isLoggedIn)
            {
                // Get username from email
                loggedInUsername = GetUsernameFromEmail(email);

                // Đóng biểu mẫu đăng nhập và mở biểu mẫu chính
                this.Hide();
                UserMain userMain = new UserMain(loggedInUsername); // pass the usernameto the UserMain constructor
                this.main.Hide();
                userMain.ShowDialog();
            }
            else
            {
                MessageBox.Show("Email hoặc mật khẩu không đúng.");
            }
        }

        private string GetUsernameFromEmail(string email)
        {
            // Using the connectionstring provided in the given function
            using (SqlConnection sqlConnection = admin___tke.Kết_nối.getConnection())
            {
                sqlConnection.Open();

                string sql = "SELECT * FROM Users WHERE Email = @Email";
                SqlCommand command = new SqlCommand(sql, sqlConnection);
                command.Parameters.AddWithValue("@Email", email);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // Get the username from the database
                    string username = reader["Username"].ToString();

                    // Close the connection and return the username
                    sqlConnection.Close();
                    return username;
                }

                // Close the connection and return an empty string
                sqlConnection.Close();
                return "";
            }
        }

        private bool PerformLogin(string email, string hashedPassword)
        {
            // Using the connection string provided in the given function
            using (SqlConnection sqlConnection = admin___tke.Kết_nối.getConnection())
            {
                sqlConnection.Open();

                string sql = "SELECT * FROM Users WHERE Email = @Email";
                SqlCommand command = new SqlCommand(sql, sqlConnection);
                command.Parameters.AddWithValue("@Email", email);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // Compare the hashed password from the database to the entered password's hash
                    string storedHash = reader["Password"].ToString();
                    if (storedHash == hashedPassword)
                    {
                        // Close the connection and return true
                        sqlConnection.Close();
                        return true;
                    }
                }

                // Close the connection and return false
                sqlConnection.Close();
                return false;
            }
        }

        private string HashPassword(string password)
        {
            // Hash the entered password using the provided HashPassword function
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashedPassword = sha256.ComputeHash(passwordBytes); return Convert.ToBase64String(hashedPassword);
            }
        }
    }
}