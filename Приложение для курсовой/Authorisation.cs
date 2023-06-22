using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Приложение_для_курсовой
{
    public partial class Auto : Form
    {

        public Auto()
        {
            InitializeComponent();
        }
        public string connection = @"Data Source=DESKTOP-NIKISIN\JET; Initial Catalog=Онлайн-магазин; User ID=sa; Password=Q1w2e3r4";
        private void button1_Click(object sender, EventArgs e)
        {
           
            Try();

        }
        void Try()
        {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            SqlCommand sqlLogin = new SqlCommand(@"select *, Название_роли from [dbo].[Пользователи], dbo.[Роль] Where Логин = '" + Login.Text + "' AND Пароль = '" + Password.Text + "'", sqlConnect);
            SqlDataReader sqlReader = null;
            sqlReader = sqlLogin.ExecuteReader();
            
            if (sqlReader.Read())
            {
                
                if (sqlReader.GetValue(3) != null)
                {
                    if (sqlReader.GetValue(3).ToString() == "1")
                    {
                        Hide();
                        AdminWindow aw = new AdminWindow();
                        aw.Show();
                      
                    }
                    else if (sqlReader.GetValue(3).ToString() == "2")
                    {
                        sqlReader.Close(); // Закрываем sqlReader
                        SqlCommand sqlGetUserID = new SqlCommand(@"Select ID_Пользователя FROM [dbo].[Пользователи] Where Логин = '" + Login.Text + "' AND Пароль = '" + Password.Text + "'", sqlConnect);
                        object result1 = sqlGetUserID.ExecuteScalar();
                        int userID = Convert.ToInt32(result1);

                        SqlCommand sqlFindClientID = new SqlCommand("Select ID_покупателя FROM [dbo].[Покупатель] INNER JOIN [dbo].[Пользователи] ON Покупатель.Сопоставленный_пользователь=Пользователи.ID_пользователя WHERE Пользователи.ID_пользователя='" + userID + "'", sqlConnect);
                        object result2 = sqlFindClientID.ExecuteScalar();
                        int clientID = Convert.ToInt32(result2);
                       
                        ClientWindow cw = new ClientWindow(userID, clientID);
                        Hide();
                        cw.Show();
                     
                    }

                    else if (sqlReader.GetValue(3).ToString() == "3")
                    {
                        sqlReader.Close(); // Закрываем sqlReader
                        SqlCommand sqlGetUserID = new SqlCommand(@"Select ID_Пользователя FROM [dbo].[Пользователи] Where Логин = '" + Login.Text + "' AND Пароль = '" + Password.Text + "'", sqlConnect);
                        object result1 = sqlGetUserID.ExecuteScalar();
                        int userID = Convert.ToInt32(result1);
                        SqlCommand sqlFindShopID = new SqlCommand("Select Код_магазина FROM [dbo].[Магазин] INNER JOIN [dbo].[Пользователи] ON Магазин.Сопоставленный_пользователь=Пользователи.ID_пользователя WHERE Пользователи.ID_пользователя='" + userID + "'", sqlConnect);
                        object result2 = sqlFindShopID.ExecuteScalar();
                        int shopID = Convert.ToInt32(result2);
                      
                        ShopWindow sw = new ShopWindow(userID, shopID);
                        sw.Show();
                    }
                }
                // Получаем данные пользователя из SqlDataReader и сохраняем их в переменных

               
              
            }
            else
            {
                MessageBox.Show("Ошибка: такого пользователя в системе нет!", "Ошибка работы авторизации", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Auto aw= new Auto();
            }

        }

        private void RegUser_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
             Registration reg =new Registration();
            Hide();
            reg.Show();
        }

        private void RegShop_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShopRegistration sreg=new ShopRegistration();
            sreg.Show();
            Hide();

        }

        private void Auto_Load(object sender, EventArgs e)
        {

        }

        private void Auto_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
