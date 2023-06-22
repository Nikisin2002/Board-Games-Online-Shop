using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Data.Entity.Infrastructure;
using System.Reflection.Emit;

namespace Приложение_для_курсовой
{
    public partial class Registration : Form
    {
        public Registration()
        {
            InitializeComponent();
        }
        public string connection = @"Data Source=DESKTOP-NIKISIN\JET; Initial Catalog=Онлайн-магазин; User ID=sa; Password=Q1w2e3r4";

        private void RegClientComplete_Click(object sender, EventArgs e)
        {
            UserReg();
           

        }
    
        void UserReg()
        {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            SqlCommand sqlMaxUserID = new SqlCommand(@"Select MAX(ID_Пользователя) FROM [dbo].[Пользователи]", sqlConnect);
            object result1 = sqlMaxUserID.ExecuteScalar();
            int UserID=Convert.ToInt32(result1)+1;

            SqlCommand sqlMaxClientID = new SqlCommand("Select MAX(ID_покупателя) FROM [dbo].[Покупатель]", sqlConnect);
            object result2 =sqlMaxClientID.ExecuteScalar();
            int ClientID=Convert.ToInt32(result2)+1;

            SqlCommand sqlInsertUser = new SqlCommand("INSERT INTO [dbo].[Пользователи] ( ID_Пользователя,Логин, Пароль, Роль) " +
                "VALUES ('" +UserID+"','" + RegLogin.Text + "', '" + RegPassword.Text + "', '"+2+"')", sqlConnect);
           SqlCommand sqlInsertClient =new SqlCommand("INSERT INTO [dbo].[Покупатель] (ID_покупателя, Сопоставленный_пользователь, Фамилия, Имя, Отчество, Дата_регистрации, Дата_рождения) " +
             "VALUES ('" + ClientID + "', '" + UserID + "', '"+ RegSurname.Text + "', '" + RegName.Text + "', '" + RegThirdName.Text + "', '" + DateTime.Now + "','" + RegBirthDate.Value.Date.ToString() + "')", sqlConnect);
            sqlInsertUser.ExecuteNonQuery();
            sqlInsertClient.ExecuteNonQuery();

            SqlCommand sqlView = new SqlCommand(@"Select ID_Покупателя, Сопоставленный_пользователь, Фамилия, Имя, Отчество, Дата_регистрации, Дата_рождения 
from [dbo].[Покупатель] INNER JOIN [dbo].[Пользователи] ON Покупатель.Сопоставленный_Пользователь=Пользователи.ID_Пользователя WHERE ID_Пользователя='" + UserID + "'", sqlConnect);
            SqlDataReader sqlReader = null;
            sqlReader = sqlView.ExecuteReader();

            sqlReader.Close(); // Закрываем sqlReader
            SqlCommand sqlGetUserID = new SqlCommand(@"Select ID_Пользователя FROM [dbo].[Пользователи] Where Логин = '" + RegLogin.Text + "' AND Пароль = '" + RegPassword.Text + "' " +
                "AND Фамилия = '"+ RegSurname.Text + "', AND Имя='" + RegName.Text + "', AND Отчество='"+ RegThirdName.Text +"' AND Дата_рождения ='"+ RegBirthDate.Value.Date.ToString() + "')", sqlConnect);
           CreateRegDoc();
            ClientWindow cw = new ClientWindow(UserID, ClientID);
            Hide();
            cw.Show();
            Hide();
            
            
        }

        private void RegLogin_TextChanged(object sender, EventArgs e)
        {

        }

        private void Registration_Load(object sender, EventArgs e)
        {

        }

        private void BirthDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void RegThirdName_TextChanged(object sender, EventArgs e)
        {

        }

        private void RegName_TextChanged(object sender, EventArgs e)
        {

        }

        private void RegSurname_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void RegPassword_TextChanged(object sender, EventArgs e)
        {

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
                int UserID = Convert.ToInt32(result1) ;

                string outputFilePath = @"C:\Выходные_документы\Данные_регистрации_пользоветеля_N"+UserID+".txt"; 

           
               

                SqlCommand sqlMaxClientID = new SqlCommand("Select MAX(ID_покупателя) FROM [dbo].[Покупатель]", sqlConnect);
                object result2 = sqlMaxClientID.ExecuteScalar();
                int ClientID = Convert.ToInt32(result2) ;
                SqlCommand sqlQuery = new SqlCommand("SELECT Пользователи.ID_Пользователя, Пользователи.Логин, Пользователи.Пароль, " +
                    "Покупатель.ID_покупателя, Покупатель.Фамилия, Покупатель.Имя, Покупатель.Отчество, Покупатель.Дата_регистрации, Покупатель.Дата_рождения FROM Пользователи" +
                    " INNER JOIN Покупатель ON Покупатель.Сопоставленный_пользователь=Пользователи.ID_пользователя" +
                    " WHERE Пользователи.ID_пользователя = '"+ UserID + "' ", sqlConnect);
                
                // Создание объекта SqlDataReader для чтения данных из базы данных
                using (SqlDataReader reader = sqlQuery.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Получение значений полей из результата запроса
                            string login = reader["Логин"].ToString();
                            string password = reader["Пароль"].ToString();
                        string Client= reader["ID_покупателя"].ToString();
                        string Surname = reader["Фамилия"].ToString();
                        string Name = reader["Имя"].ToString();
                        string ThirdName = reader["Отчество"].ToString();
                        string RegDate = reader["Дата_регистрации"].ToString();
                        string BirthDate = reader["Дата_рождения"].ToString();


                        // Создание строки с информацией о магазине
                        string outputText = 
                            "\n|-----------------------------------------------|" +
                            "\n|Справка о регистрации пользователя N" + UserID + "\t\t|" +
                            "\n|\t\tДанные регистрации" +"\t\t|"+
                            "\n|Логин:"+ login+ "\t\t\t\t|" +
                            "\n|Пароль:" + password+ "\t\t\t\t|" +
                            "\n|Фамилия:" + Surname+ "\t\t\t\t|" +
                            "\n|Имя:" + Name+ "\t\t\t\t|" +
                            "\n|Отчество:" + ThirdName+ "\t\t\t\t|" +
                            "\n|Дата рождения:" + BirthDate+ "\t\t|" +
                            "\n|\n|\tДата регистрации:" + RegDate+ "\t|" +
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

        private void Registration_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
        }
