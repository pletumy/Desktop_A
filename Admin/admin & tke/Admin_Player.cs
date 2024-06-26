﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace admin___tke
{
    public partial class Admin_Player : Form
    {
        private TextBox txt_Search;
        public Admin_Player()
        {
            InitializeComponent();
        }

        private void btn_Edit_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Edit_info f2 = new Edit_info();
            f2.ShowDialog();
        }

        private void btn_GPage_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin_Game admin_Game = new Admin_Game();
            admin_Game.Show();
        }

        private void btn_Find_Click(object sender, EventArgs e)
        {
            string userName = txt_S.Text.Trim();

            if (string.IsNullOrEmpty(userName))
            {
                MessageBox.Show("Vui lòng nhập tên người dùng để tìm kiếm.", "Tìm kiếm người dùng", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataTable userTable = GetUser(userName);

            if (userTable.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy người dùng nào với tên người dùng được cung cấp.", "Người dùng tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            dgv_Player.DataSource = userTable;
        }

        private DataTable GetUser(string userName)
        {
            DataTable userTable = new DataTable();

            using (SqlConnection sqlConnection = admin___tke.Kết_nối.getConnection())
            {
                sqlConnection.Open();

                // Escape the search term to handle potential wildcard characters
                string escapedUserName = SqlHelper.EscapeLike(userName);

                // Using the LIKE operator with wildcard characters
                string query = $"SELECT UserID, UserName, Email, NGAYTHAMGIA, TEN, HO, SDT, QUOCGIA, THANHPHO, Address FROM Users WHERE UserName LIKE '%{escapedUserName}%'";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlDataAdapter.Fill(userTable);

                sqlConnection.Close();
            }

            return userTable;
        }

        // Extension method to escape the search term
        public static class SqlHelper
        {
            public static string EscapeLike(string input)
            {
                if (string.IsNullOrEmpty(input))
                {
                    return input;
                }

                return input.Replace("_", "[_]").Replace("%", "[%]");
            }
        }

        private void btn_R_Click(object sender, EventArgs e)
        {
            this.Hide();
            Reports Reports = new Reports();
            Reports.Show();
        }

        private void btn_S_Click(object sender, EventArgs e)
        {
            this.Hide();
            Search Search = new Search();
            Search.Show();
        }
    }
}
