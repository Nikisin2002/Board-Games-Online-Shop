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
using System.IO;


namespace Приложение_для_курсовой
{
    public partial class ShopRegistration : Form
    {
        public ShopRegistration()
        {
            InitializeComponent();
        }

        private void RegShopComplete_Click(object sender, EventArgs e)
        {
            ShopReg();
            Hide();
           // ShopWindow sw = new ShopWindow(UserID);
            //sw.Show();

        }
        public string connection = @"Data Source=DESKTOP-NIKISIN\JET; Initial Catalog=Онлайн-магазин; User ID=sa; Password=Q1w2e3r4";

        void ShopReg()
        {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            SqlCommand sqlMaxUserID = new SqlCommand(@"Select MAX(ID_Пользователя) FROM [dbo].[Пользователи]", sqlConnect);
            object result1 = sqlMaxUserID.ExecuteScalar();
            int UserID = Convert.ToInt32(result1) + 1;

            SqlCommand sqlMaxClientID = new SqlCommand("Select MAX(Код_Магазина) FROM [dbo].[Магазин]", sqlConnect);
            object result2 = sqlMaxClientID.ExecuteScalar();
            int ShopID = Convert.ToInt32(result2) + 1;

            SqlCommand sqlInsertUser = new SqlCommand("INSERT INTO [dbo].[Пользователи] ( ID_Пользователя,Логин, Пароль, Роль) " +
                "VALUES ('" + UserID + "','" + ShopLogin.Text + "', '" + ShopPassword.Text + "', '" + 3 + "')", sqlConnect);
            SqlCommand sqlInsertClient = new SqlCommand("INSERT INTO [dbo].[Магазин] (Код_магазина, Сопоставленный_пользователь, Название_магазина, Дата_регистрации, ИНН, Сайт) " +
              "VALUES ('" + ShopID + "', '" + UserID + "', '" + ShopName.Text + "', '" + DateTime.Now + "', '" + ShopIdentity.Text + "', '" + ShopSite.Text + "')", sqlConnect);
            sqlInsertUser.ExecuteNonQuery();
            sqlInsertClient.ExecuteNonQuery();


            SqlCommand sqlView = new SqlCommand(@"Select Код_магазина, Сопоставленный_пользователь, Название_магазина, ИНН, Сайт, Дата_регистрации
from [dbo].[Магазин] INNER JOIN [dbo].[Пользователи] ON Магазин.Сопоставленный_пользователь=Пользователи.ID_Пользователя WHERE ID_Пользователя='" + UserID + "'", sqlConnect);
            SqlDataReader sqlReader = null;
            sqlReader = sqlView.ExecuteReader();

            sqlReader.Close(); // Закрываем sqlReader
            SqlCommand sqlGetUserID = new SqlCommand(@"Select Код_Пользователя FROM [dbo].[Покльзователи] Where Логин = '" + ShopLogin.Text + "' AND Пароль = '" + ShopPassword.Text + "' " +
                "AND Название_магазина = '" + ShopName.Text + "', AND ИНН='" + ShopIdentity.Text + "', AND Сайт='" + ShopSite.Text + "')", sqlConnect);
            CreateRegDoc();
            ShopWindow sw = new ShopWindow(UserID, ShopID);
            Hide();
            sw.Show();
            Hide();
        }

        public void CreateRegDoc()
        {
            try
            {
                SqlConnection sqlConnect = new SqlConnection(connection);

                sqlConnect.Open();

                // Создание команды с параметром @ShopCode
                // Задание значения параметра @ShopCode
                SqlCommand sqlMaxUserID = new SqlCommand(@"Select MAX(ID_Пользователя) FROM [dbo].[Пользователи]", sqlConnect);
                object result1 = sqlMaxUserID.ExecuteScalar();
                int UserID = Convert.ToInt32(result1);

                string outputFilePath = @"C:\Выходные_документы\Данные_регистрации_пользоветеля_N" + UserID + ".txt";




                SqlCommand sqlMaxClientID = new SqlCommand("Select MAX(Код_магазина) FROM [dbo].[Магазин]", sqlConnect);
                object result2 = sqlMaxClientID.ExecuteScalar();
                int ClientID = Convert.ToInt32(result2);
                SqlCommand sqlQuery = new SqlCommand("SELECT Пользователи.ID_Пользователя, Пользователи.Логин, Пользователи.Пароль, " +
                    "Магазин.Код_магазина, Магазин.Название_магазина, Магазин.ИНН, Магазин.Сайт, Магазин.Дата_регистрации FROM Пользователи" +
                    " INNER JOIN Магазин ON Магазин.Сопоставленный_пользователь=Пользователи.ID_пользователя" +
                    " WHERE Пользователи.ID_пользователя = '" + UserID + "' ", sqlConnect);

                // Создание объекта SqlDataReader для чтения данных из базы данных
                using (SqlDataReader reader = sqlQuery.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Получение значений полей из результата запроса
                        string login = reader["Логин"].ToString();
                        string password = reader["Пароль"].ToString();
                        string ShopID = reader["Код_магазина"].ToString();
                        string ShopName = reader["Название_магазина"].ToString();
                        string Identity = reader["ИНН"].ToString();
                        string Site = reader["Сайт"].ToString();

                        string Date = reader["Дата_регистрации"].ToString();


                        // Создание строки с информацией о магазине
                        string outputText =
                            "\n|-----------------------------------------------|" +
                            "\n|Справка о регистрации пользователя N" + UserID + "\t\t|" +
                            "\n|\t\tДанные регистрации" + "\t\t|" +
                            "\n|Логин:" + login + "\t\t\t\t|" +
                            "\n|Пароль:" + password + "\t\t\t\t|" +
                            "\n|Название магазина:" + ShopName + "\t\t\t\t|" +
                            "\n|ИНН:" + Identity + "\t\t\t\t|" +
                            "\n|Сайт:" + Site + "\t\t|" +
                            "\n|\n|\tДата регистрации:" + Date + "\t|" +
                            "\n|-----------------------------------------------|"

                            ;

                        // Запись строки в файл
                        File.WriteAllText(outputFilePath, outputText);

                        MessageBox.Show("Выходной документ успешно создан.");
                    }
                    else
                    {
                        MessageBox.Show("Магазин с указанным кодом не найден.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при создании выходного документа: " + ex.Message);
            }
        }

        private void ShopRegistration_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}

