using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Приложение_для_курсовой
{


    public partial class AdminWindow : Form
    {
        public string FilePath;
 public AdminWindow()
        {
            InitializeComponent();
        }

        public string connection = @"Data Source=DESKTOP-NIKISIN\JET; Initial Catalog=Онлайн-магазин; User ID=sa; Password=Q1w2e3r4";

        public void FillGameText()
        {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            int GameID = Convert.ToInt32(UpdateName.SelectedIndex)+1;
            SqlCommand sqlView = new SqlCommand("SELECT Настольная_игра.Название, Настольная_игра.Изображение_игры, Настольная_игра.Минимальное_количество_игроков, " +
                "Настольная_игра.Максимальное_количество_игроков, Настольная_игра.Возрастное_ограничение, Настольная_игра.Год_издания, Настольная_игра.Рейтинг, Жанр.Название_жанра, Изготовитель.Название_фирмы, Страна.Название_страны FROM Настольная_игра " +
                "INNER JOIN Жанр ON Настольная_игра.жанр = Жанр.Код_жанра " +
                "INNER JOIN Изготовитель ON Настольная_игра.Производитель=Изготовитель.Код_производителя " +
                "INNER JOIN Страна ON Изготовитель.Страна=Страна.Код_страны WHERE Настольная_игра.Название='" + UpdateName.Text + "'", sqlConnect);
            SqlDataReader sqlReader = null;
            sqlReader = sqlView.ExecuteReader();
           
            while (sqlReader.Read())
            {
                string Picture= sqlReader["Изображение_игры"].ToString();
                UpdatePicture.Image =Image.FromFile(Picture);
                UpdateMinPlayerCount.Text = sqlReader["Минимальное_количество_игроков"].ToString();
                UpdateMaxPlayerCount.Text = sqlReader["Максимальное_количество_игроков"].ToString();
                UpdateAge.Text = sqlReader["Возрастное_ограничение"].ToString();
                UpdateJenre.Text = sqlReader["Название_жанра"].ToString();
                UpdateManufacturer.Text = sqlReader["Название_фирмы"].ToString();
                UpdateYear.Text = sqlReader["Год_издания"].ToString();
                UpdateRating.Text = sqlReader["Рейтинг"].ToString();
            }
        }



        string imageUrl = null;

       

        public void FillClientText()
        {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();

           
            int selectedClientID = Convert.ToInt32(Client.SelectedIndex)+1;

            SqlCommand sqlView = new SqlCommand(@"SELECT Логин, Пароль, ID_покупателя, Фамилия, Имя, Отчество, Дата_регистрации, Дата_рождения 
FROM [dbo].[Покупатель] INNER JOIN [dbo].[Пользователи] ON Покупатель.Сопоставленный_пользователь = Пользователи.ID_Пользователя 
WHERE ID_Покупателя='" + selectedClientID + "'", sqlConnect);


            SqlDataReader sqlReader = sqlView.ExecuteReader();

            while (sqlReader.Read())
            {
                DeleteLogin.Text = sqlReader["Логин"].ToString();
                DeletePassword.Text = sqlReader["Пароль"].ToString();
                DeleteSurname.Text = sqlReader["Фамилия"].ToString();
                DeleteName.Text = sqlReader["Имя"].ToString();
                DeleteThirdName.Text = sqlReader["Отчество"].ToString();
                DeleteBirthDate.Text = sqlReader["Дата_рождения"].ToString();
                DeleteRegDate.Text = sqlReader["Дата_регистрации"].ToString();
            }



        }

        public void FillShopText()
        {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();

            int selectedShopID = Convert.ToInt32(Shop.SelectedIndex)+1;

            SqlCommand sqlView = new SqlCommand(@"SELECT Логин, Пароль, Название_магазина, ИНН, Сайт, Дата_регистрации 
FROM [dbo].[Магазин] INNER JOIN [dbo].[Пользователи] ON Магазин.Сопоставленный_пользователь = Пользователи.ID_Пользователя 
WHERE Название_магазина='" + Shop.Text + "'", sqlConnect);


            SqlDataReader sqlReader = sqlView.ExecuteReader();

            while (sqlReader.Read())
            {
                DeleteShopLogin.Text = sqlReader["Логин"].ToString();
                DeleteShopPassword.Text = sqlReader["Пароль"].ToString();
                DeleteShopName.Text = sqlReader["Название_магазина"].ToString();
                DeleteIdentity.Text = sqlReader["ИНН"].ToString();
                DeleteSite.Text = sqlReader["Сайт"].ToString();
                DeleteShopRegDate.Text = sqlReader["Дата_регистрации"].ToString();
            }



        }

        private void заказыToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void AdminWindow_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "_Онлайн_магазинDataSet9.Магазин". При необходимости она может быть перемещена или удалена.
            this.магазинTableAdapter1.Fill(this._Онлайн_магазинDataSet9.Магазин);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "_Онлайн_магазинDataSet9.Настольная_игра". При необходимости она может быть перемещена или удалена.
            this.настольная_играTableAdapter5.Fill(this._Онлайн_магазинDataSet9.Настольная_игра);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "_Онлайн_магазинDataSet9.Изготовитель". При необходимости она может быть перемещена или удалена.
            this.изготовительTableAdapter3.Fill(this._Онлайн_магазинDataSet9.Изготовитель);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "_Онлайн_магазинDataSet9.Жанр". При необходимости она может быть перемещена или удалена.
            this.жанрTableAdapter3.Fill(this._Онлайн_магазинDataSet9.Жанр);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "_Онлайн_магазинDataSet9.Настольная_игра". При необходимости она может быть перемещена или удалена.
            this.настольная_играTableAdapter5.Fill(this._Онлайн_магазинDataSet9.Настольная_игра);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "_Онлайн_магазинDataSet9.Покупатель". При необходимости она может быть перемещена или удалена.
            this.покупательTableAdapter4.Fill(this._Онлайн_магазинDataSet9.Покупатель);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "_Онлайн_магазинDataSet9.Покупатель". При необходимости она может быть перемещена или удалена.
            this.покупательTableAdapter4.Fill(this._Онлайн_магазинDataSet9.Покупатель);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "_Онлайн_магазинDataSet8.Покупатель". При необходимости она может быть перемещена или удалена.
            this.покупательTableAdapter4.Fill(this._Онлайн_магазинDataSet8.Покупатель);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "_Онлайн_магазинDataSet8.Настольная_игра". При необходимости она может быть перемещена или удалена.
            this.настольная_играTableAdapter5.Fill(this._Онлайн_магазинDataSet8.Настольная_игра);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "_Онлайн_магазинDataSet7.Настольная_игра". При необходимости она может быть перемещена или удалена.









        }
        public void InsertClientData()
        {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            SqlCommand sqlMaxUserID = new SqlCommand(@"Select MAX(ID_Пользователя) FROM [dbo].[Пользователи]", sqlConnect);
            object result1 = sqlMaxUserID.ExecuteScalar();
            int UserID = Convert.ToInt32(result1) + 1;

            SqlCommand sqlMaxClientID = new SqlCommand("Select MAX(ID_Покупателя) FROM [dbo].[Покупатель]", sqlConnect);
            object result2 = sqlMaxClientID.ExecuteScalar();
            int ClientID = Convert.ToInt32(result2) + 1;

            SqlCommand sqlInsertUser = new SqlCommand("INSERT INTO [dbo].[Пользователи] ( ID_Пользователя,Логин, Пароль, Роль) " +
                "VALUES ('" + UserID + "','" + RegLogin.Text + "', '" + RegPassword.Text + "', '" + 2 + "')", sqlConnect);
            SqlCommand sqlInsertClient = new SqlCommand("INSERT INTO [dbo].[Покупатель] (ID_Покупателя, Сопоставленный_пользователь, Фамилия, Имя, Отчество, Дата_регистрации, Дата_рождения) " +
              "VALUES ('" + ClientID + "', '" + UserID + "', '" + RegSurname.Text + "', '" + RegName.Text + "', '" + RegThirdName.Text + "', '" + DateTime.Now + "','" + RegBirthDate.Value.Date.ToString() + "')", sqlConnect);
            sqlInsertUser.ExecuteNonQuery();
            sqlInsertClient.ExecuteNonQuery();
            sqlConnect.Close();
            MessageBox.Show("Пользователь успешно добавлен в систему", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        public void DeleteClientData()
        {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            
            int selectedClientID = Convert.ToInt32(Client.SelectedIndex) + 1;
            SqlCommand sqlDeletePurchase = new SqlCommand(@"DELETE Покупка FROM Покупка JOIN Заказ ON Покупка.Заказ=Заказ.Номер_заказа  JOIN Покупатель ON Покупатель.ID_покупателя=Заказ.Заказчик
  WHERE Покупатель.Фамилия='" + Client.Text + "'", sqlConnect);
            SqlCommand sqlDeleteOrder= new SqlCommand(@"DELETE Заказ FROM Заказ JOIN Покупатель ON Покупатель.ID_покупателя=Заказ.Заказчик WHERE Покупатель.Фамилия = '" + Client.Text + "'", sqlConnect);
            SqlCommand sqlFindUserID = new SqlCommand(@"SELECT ID_Пользователя from [dbo].[Пользователи] 
INNER JOIN [dbo].[Покупатель] ON Покупатель.Сопоставленный_Пользователь=Пользователи.ID_Пользователя WHERE Фамилия='" + Client.Text + "' ", sqlConnect);
            object result1 = sqlFindUserID.ExecuteScalar();
            int UserID = Convert.ToInt32(result1);
SqlCommand sqlDeleteClient = new SqlCommand(@"DELETE FROM Покупатель WHERE Покупатель.Фамилия = '" + Client.Text + "'", sqlConnect);
            SqlCommand sqlDeleteUser = new SqlCommand("Delete From Пользователи Where ID_Пользователя='"+UserID+"' ", sqlConnect);
            
            sqlDeletePurchase.ExecuteNonQuery();
            sqlDeleteOrder.ExecuteNonQuery();
           
            sqlDeleteClient.ExecuteNonQuery();
 sqlDeleteUser.ExecuteNonQuery();
            sqlConnect.Close();
            MessageBox.Show("Пользователь успешно удален из системы", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Client.Items.Remove(Client.Text);
            Client.Text = "";
            DeleteLogin.Text = "";
            DeletePassword.Text = "";
            DeleteSurname.Text = "";
            DeleteName.Text = "";
            DeleteThirdName.Text = "";
            DeleteBirthDate.Text = "";
            DeleteRegDate.Text = "";
            sqlConnect.Close();

        }

        public void UpdateClientData()
        {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
           
            int ClientRow = Convert.ToInt32(Client.SelectedIndex)+1;
            SqlCommand sqlFindClientID = new SqlCommand(@"SELECT ID_покупателя from [dbo].[Покупатель] WHERE Фамилия= '"+ Client.Text + "' ", sqlConnect);
            object result1 = sqlFindClientID.ExecuteScalar();
            int ClientID = Convert.ToInt32(result1);

            SqlCommand sqlFindUserID = new SqlCommand(@"SELECT ID_Пользователя from [dbo].[Пользователи] 
INNER JOIN [dbo].[Покупатель] ON Покупатель.Сопоставленный_Пользователь=Пользователи.ID_Пользователя WHERE ID_покупателя='" + ClientID + "' ", sqlConnect);
            object result2 = sqlFindUserID.ExecuteScalar();
            int UserID = Convert.ToInt32(result2);


            SqlCommand sqlUpdateUser = new SqlCommand("Update [dbo].[Пользователи] Set Логин='" + DeleteLogin.Text + "', Пароль='" + DeletePassword.Text +  "'WHERE ID_Пользователя='" + UserID + "' ", sqlConnect);
           

            SqlCommand sqlUpdateClient= new SqlCommand("Update [dbo].[Покупатель] Set Фамилия='" + DeleteSurname.Text + "', Имя='" + DeleteName.Text +"', Отчество='"+DeleteThirdName.Text+"', " +
                "Дата_рождения='"+DeleteBirthDate.Text+"', Дата_регистрации='"+DeleteRegDate.Text+"' WHERE ID_Покупателя='" + ClientID + "' ", sqlConnect);
             sqlUpdateUser.ExecuteNonQuery();
            sqlUpdateClient.ExecuteNonQuery();

            sqlConnect.Close();
            MessageBox.Show("Данные клиента успешно изменены", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void InsertShopData()
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
              "VALUES ('" + ShopID + "', '" + UserID + "', '" + ShopName.Text + "', '" + DateTime.Now + "', '" + (long)Convert.ToInt32(ShopIdentity.Text)+ "', '" + ShopSite.Text + "')", sqlConnect);
            sqlInsertUser.ExecuteNonQuery();
            sqlInsertClient.ExecuteNonQuery();
            sqlConnect.Close();
            MessageBox.Show("Магазин успешно добавлен в систему", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void InsertGameData()
        {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            SqlCommand sqlMaxGameID = new SqlCommand(@"Select MAX(Артикул) FROM [dbo].[Настольная_игра]", sqlConnect);
            object result1 = sqlMaxGameID.ExecuteScalar();
            int GameID = Convert.ToInt32(result1) + 1;
            
            


            int JenreID = Convert.ToInt32(Jenre.SelectedIndex)+1;

            int ManufacturerID = Convert.ToInt32(Manufacturer.SelectedIndex)+1;

        

            SqlCommand sqlInsertGame = new SqlCommand("INSERT INTO [dbo].[Настольная_игра] (Артикул, Название, Изображение_игры, Минимальное_количество_игроков, Максимальное_количество_игроков, Возрастное_ограничение, Жанр, Производитель, Год_издания, Рейтинг) " +
              "VALUES ('" + GameID + "',  '" + GameName.Text + "', '" + FilePath + "', '" + MinPlayerCount.Text + "', '" + MaxPlayerCount.Text + "', '"+Age.Text+ "', '"+JenreID+ "', '"+ManufacturerID+ "', '"+Year.Text+ "', '"+Rating.Text+"')", sqlConnect);
            sqlInsertGame.ExecuteNonQuery();
            sqlConnect.Close();
            MessageBox.Show( "Игра добавлена в систему", "Сообщение",MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        public void DeleteShopData()
        {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();

            // int selectedGameID = Convert.ToInt32(Game.SelectedIndex) + 1;
            int ShopID = Convert.ToInt32(Shop.SelectedIndex)+1;
            SqlCommand sqlDeletePurchase = new SqlCommand(@"DELETE Покупка FROM Покупка  JOIN Ассортимент ON Покупка.Ассортимент=Ассортимент.Номер_ассортимента JOIN Магазин ON Ассортимент.Магазин=Магазин.Код_магазина 
WHERE Магазин.Название_магазина='"+Shop.Text+"' ", sqlConnect);
            SqlCommand sqlDeleteAssortment = new SqlCommand(@"DELETE Ассортимент FROM Ассортимент JOIN Магазин ON Ассортимент.Магазин =Магазин.Код_магазина WHERE Магазин.Название_магазина='" + Shop.Text + "'", sqlConnect);
            SqlCommand sqlFindUserID = new SqlCommand(@"SELECT ID_Пользователя from [dbo].[Пользователи] 
INNER JOIN [dbo].[Магазин] ON Магазин.Сопоставленный_Пользователь=Пользователи.ID_Пользователя WHERE Магазин.Название_магазина ='" + Shop.Text + "' ", sqlConnect);
            object result1 = sqlFindUserID.ExecuteScalar();
            int UserID = Convert.ToInt32(result1);
            SqlCommand sqlDeleteShop = new SqlCommand(@"DELETE FROM Магазин WHERE Магазин.Название_магазина = '" + Shop.Text + "' ", sqlConnect);
            SqlCommand sqlDeleteUser = new SqlCommand("Delete From Пользователи Where ID_Пользователя='" + UserID + "' ", sqlConnect);

            sqlDeletePurchase.ExecuteNonQuery();
            sqlDeleteAssortment.ExecuteNonQuery();

            sqlDeleteShop.ExecuteNonQuery();
            sqlDeleteUser.ExecuteNonQuery();
            sqlConnect.Close();
            MessageBox.Show("Магазин успешно удален из системы", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Shop.Items.Remove(Client.Text);
            Shop.Text = "";
            DeleteShopLogin.Text = "";
            DeleteShopPassword.Text = "";
            DeleteShopName.Text = "";
            DeleteIdentity.Text = "";
            DeleteSite.Text = "";
            DeleteShopRegDate.Text = "";
            sqlConnect.Close();
        }

        public void UpdateShopData()
        {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();

            int ShopRow = Convert.ToInt32(Shop.SelectedIndex)+1;

            SqlCommand sqlFindShopID = new SqlCommand(@"SELECT Код_магазина from [dbo].[Магазин]  WHERE Магазин.Название_магазина='" + Shop.Text + "' ", sqlConnect);
            object result1 = sqlFindShopID.ExecuteScalar();
            int ShopID = Convert.ToInt32(result1);

            SqlCommand sqlFindUserID = new SqlCommand(@"SELECT ID_Пользователя from [dbo].[Пользователи] 
INNER JOIN [dbo].[Магазин] ON Магазин.Сопоставленный_Пользователь=Пользователи.ID_Пользователя WHERE Магазин.Название_магазина='" + Shop.Text + "' ", sqlConnect);
            object result2 = sqlFindUserID.ExecuteScalar();
            int UserID = Convert.ToInt32(result2);
            SqlCommand sqlUpdateUser = new SqlCommand("Update [dbo].[Пользователи] Set Логин='" + DeleteShopLogin.Text + "', Пароль='" + DeleteShopPassword.Text + "'WHERE ID_Пользователя='" + UserID + "' ", sqlConnect);
            SqlCommand sqlUpdateClient = new SqlCommand("Update [dbo].[Магазин] Set Название_магазина='" + DeleteShopName.Text + "', ИНН='" + DeleteIdentity.Text + "', Сайт='" + DeleteSite.Text + "', Дата_регистрации='" + DeleteShopRegDate.Text + "' WHERE Код_магазина='" + ShopID + "' ", sqlConnect);
            sqlUpdateUser.ExecuteNonQuery();
            sqlUpdateClient.ExecuteNonQuery();

            sqlConnect.Close();
            MessageBox.Show("Данные магазина успешно изменены", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void InsertShopWindowShow()
        {
            ClientData.Visible = false;
            ClientInserting.Visible = false;
            ClientDeleting.Visible = false;
            ShopData.Visible = false;

            ShopDeleting.Visible = false;
            GameData.Visible = false;
            GameInserting.Visible = false;
            GameDeleting.Visible = false;
            GameUpdating.Visible = false;
            ClientInserting.Size = new Size(500, 400);
            ClientInserting.Location = new Point(20, 30);
            ClientInserting.Visible = false;
            ShopInserting.Size = new Size(600, 300);
            ShopInserting.Location = new Point(20, 30);
           
            ShopInserting.Visible = true;
        }

      


public void DeleteGameData()
        {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            // int selectedGameID = Convert.ToInt32(Game.SelectedIndex) + 1;
           int GameID = Game.SelectedIndex+1;
            SqlCommand sqlDeletePurchase = new SqlCommand(@"DELETE Покупка FROM Покупка 
JOIN Ассортимент ON Ассортимент.Номер_ассортимента = Покупка.Ассортимент 
WHERE Ассортимент.Настольная_игра = '" + GameID + "'", sqlConnect);
            SqlCommand sqlDeleteAssortment= new SqlCommand(@"DELETE Ассортимент FROM Ассортимент JOIN Настольная_игра ON Настольная_игра.Артикул= Ассортимент.Настольная_игра 
WHERE Настольная_игра.Название = '" + Game.Text + "'", sqlConnect);
            SqlCommand sqlDeleteGame = new SqlCommand(@"DELETE FROM Настольная_игра
 WHERE Настольная_игра.Название = '" + Game.Text + "'", sqlConnect);
           sqlDeletePurchase.ExecuteNonQuery();
            sqlDeleteGame.ExecuteNonQuery();
            sqlDeleteAssortment.ExecuteNonQuery();
            sqlConnect.Close();
            MessageBox.Show("Игра успешно удалена из системы", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Game.Items.Remove(Game.Text);
            Game.Text = "";

            // Очистка выбора в ComboBox
            Game.SelectedIndex = -1;
        }

        public void DeleteClientWindowShow()
        {
            FillClientText();
            ClientData.Visible = false;
            ShopData.Visible = false;
            ShopInserting.Visible = false;
            ShopDeleting.Visible = false;
            GameData.Visible = false;
            GameInserting.Visible = false;
            GameDeleting.Visible = false;
            GameUpdating.Visible = false;
            ClientInserting.Size = new Size(500, 400);
            ClientInserting.Location = new Point(20, 30);
            ClientInserting.Visible = false;
            ClientDeleting.Size = new Size(400, 500);
            ClientDeleting.Location = new Point(20, 35);
            ClientDeleting.Visible = true;
        }

        public void DeleteShopWindowShow()
        {
            ClientData.Visible = false;
            ClientInserting.Visible= false;
            ClientDeleting.Visible = false;
            ShopData.Visible = false;
            ShopInserting.Visible = false;
            GameData.Visible = false;
            GameInserting.Visible = false;
            GameDeleting.Visible = false;
            GameUpdating.Visible = false;
            ClientInserting.Size = new Size(500, 400);
            ClientInserting.Location = new Point(20, 30);
            ClientInserting.Visible = false;
            ShopDeleting.Size = new Size(500, 400);
           ShopDeleting.Location = new Point(20, 30);
            ShopDeleting.Visible = true;
UpdateShop.Visible=false;
            DeleteShop.Visible=true;
            
            FillShopText();
        }

        

        public void UpdateShopWindowShow()
        {
            ClientData.Visible = false;
            ClientInserting.Visible = false;
            ClientDeleting.Visible = false;
            ShopData.Visible = false;
            ShopInserting.Visible = false;
            GameData.Visible = false;
            GameInserting.Visible = false;
            GameDeleting.Visible = false;
            GameUpdating.Visible = false;
            ClientInserting.Size = new Size(500, 400);
            ClientInserting.Location = new Point(20, 30);
            ClientInserting.Visible = false;
ShopDeleting.Visible = true;
            ShopDeleting.Size = new Size(500, 400);
            ShopDeleting.Location = new Point(20, 30);

            DeleteShop.Visible = false;
            UpdateShop.Visible = true;
            UpdateShop.Location= new Point(150, 290);
            FillShopText();
        }

        
        public void UpdateGameData()
        {
           

            
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
          
            int selectedGameID = Convert.ToInt32(UpdateName.SelectedIndex)+1;
            
            SqlCommand FindGameID=new SqlCommand("SELECT Настольная_игра.Артикул FROM Настольная_игра WHERE Настольная_игра.Название='"+UpdateName.Text+"' ", sqlConnect);
            int GameID = Convert.ToInt32(FindGameID.ExecuteScalar());
            int genreID = Convert.ToInt32(UpdateJenre.SelectedIndex)+1;
           
            int manufacturerID = Convert.ToInt32(UpdateManufacturer.SelectedIndex)+1;
            SqlCommand sqlUpdate = new SqlCommand("Update [dbo].[Настольная_игра] Set Настольная_игра.Название='"+UpdateName.Text+"', Минимальное_количество_игроков='"+UpdateMinPlayerCount.Text+"',Максимальное_количество_игроков='"+UpdateMaxPlayerCount.Text+"', Возрастное_ограничение='"+UpdateAge.Text+"', " +
                "Жанр='"+genreID+"', Производитель='"+manufacturerID+"', " +
                "Год_издания='"+UpdateYear.Text+"', Настольная_игра.Рейтинг='"+UpdateRating.Text +"' WHERE Настольная_игра.Артикул='"+ GameID + "' ",sqlConnect);
            sqlUpdate.ExecuteNonQuery();
         
            sqlConnect.Close();
            MessageBox.Show("Игра успешно изменена", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                 

        }

        public void InsertGameWindowShow()
        {

            ClientData.Visible = false;
            ClientDeleting.Visible = false;
            ShopData.Visible = false;
            ShopInserting.Visible = false;
            ShopDeleting.Visible = false;
            GameData.Visible = false;
            GameInserting.Visible = false;
            GameDeleting.Visible = false;
            GameUpdating.Visible = false;
       
            ClientInserting.Visible = false;
            GameInserting.Visible= true;
            GameInserting.Size = new Size(400, 500);
            GameInserting.Location = new Point(20, 35);
            
        }

        public void DeleteGameWindowShow()
        {
            ClientData.Visible = false;
            ClientDeleting.Visible = false;
            ShopData.Visible = false;
            ShopInserting.Visible = false;
            ShopDeleting.Visible = false;
            GameData.Visible = false;
            GameInserting.Visible = false;
            GameDeleting.Visible = false;
            GameUpdating.Visible = false;
            ClientInserting.Visible = false;
            GameDeleting.Visible = true;
            GameDeleting.Size = new Size(300, 100);
            GameDeleting.Location = new Point(20, 35);
           
        }

        public void UpdateGameWindowShow()
        {
            ClientData.Visible = false;
            ClientDeleting.Visible = false;
            ShopData.Visible = false;
            ShopInserting.Visible = false;
            ShopDeleting.Visible = false;
            GameData.Visible = false;
            GameInserting.Visible = false;
            GameDeleting.Visible = false;
            GameUpdating.Visible = false;
            ClientInserting.Visible = false;
            GameUpdating.Visible = true;
            GameUpdating.Size = new Size(400, 500);
            GameUpdating.Location = new Point(20, 35);
            
            FillGameText();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void fillByToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {

            }
         
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }
private void UpdateClient_Click(object sender, EventArgs e)
        {
            UpdateClientData();
        }
        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void магазиныToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void настольныеИгрыToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void покупателиToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void покупателиToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

        }

        private void магазиныToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

        }

        private void настольныеИгрыToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

        }

        private void добавлениеToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void удалениеToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void редактированиеToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void добавлениеToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void удалениеToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void редактированиеToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void добавлениеToolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void удалениеToolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void редактированиеToolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void GameInsert_Click(object sender, EventArgs e)
        {
            InsertGameData();
        }

      
        private void GameUpdate_Click(object sender, EventArgs e)
        {
            UpdateGameData();
        }

        private void ShopInsert_Click(object sender, EventArgs e)
        {
            InsertShopData();
              
         
        }

        private void ShopDelete_Click(object sender, EventArgs e)
        {

        }

        private void ShopUpdate_Click(object sender, EventArgs e)
        {

        }

      

      

        private void настольныеИгрыToolStripMenuItem_Click_2(object sender, EventArgs e)
        {

        }

       public void InsertClientWindowShow()
        {
            ClientData.Visible = false;
            ClientDeleting.Visible = false;
            ShopData.Visible = false;
            ShopInserting.Visible = false;
            ShopDeleting.Visible = false;
            GameData.Visible = false;
            GameInserting.Visible = false;
            GameDeleting.Visible=false;
            GameUpdating.Visible=false;
            ClientInserting.Size = new Size(500, 400);
            ClientInserting.Location = new Point(20, 30);
            ClientInserting.Visible = true;
         }

       

      public void ClientDataSelect()

        {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();


            // Создаем команду SQL для выборки данных из таблицы
            SqlCommand sqlQuery = new SqlCommand("SELECT Пользователи.Логин, Пользователи.Пароль, Покупатель.Фамилия, Покупатель.Имя, Покупатель.Отчество, Покупатель.Дата_регистрации, Покупатель.Дата_рождения" +
                " FROM [dbo].[Пользователи] INNER JOIN Покупатель ON Покупатель.Сопоставленный_пользователь=Пользователи.ID_Пользователя ", sqlConnect);


            // Создаем адаптер данных для заполнения DataSet
            SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery);

            // Создаем DataSet для хранения данных
            DataSet dataSet = new DataSet();

            // Заполняем DataSet данными из таблицы
            adapter.Fill(dataSet);

            // Закрываем соединение
            sqlConnect.Close();

            // Устанавливаем DataSet как источник данных для DataGridView
            ClientsTable.DataSource = dataSet.Tables[0];
        }

        private void просморToolStripMenuItem_Click(object sender, EventArgs e)//Просмотр покупателей
        {

            ClientInserting.Visible = false;
            ClientDeleting.Visible = false;
            ShopData.Visible = false;
            ShopInserting.Visible = false;
            ShopDeleting.Visible = false;
            GameData.Visible = false;
            GameInserting.Visible = false;
            GameDeleting.Visible = false;
            GameUpdating.Visible = false;
            ClientInserting.Size = new Size(500, 400);
            ClientInserting.Location = new Point(20, 30);
            ClientData.Visible = true;
            ClientData.Size = new Size(750, 300);
            ClientData.Location = new Point(20, 30);
ClientDataSelect();
        }

        private void добавлениеToolStripMenuItem_Click_1(object sender, EventArgs e)//Открывается окно добавления покупателей
        {
            InsertClientWindowShow();
        }

        private void удалениеToolStripMenuItem_Click_1(object sender, EventArgs e)//Открывается окно удаления покупателей
        {
            DeleteClientWindowShow();
            UpdateClient.Visible = false;
            DeleteClient.Visible = true;
            FillClientData();

        }

        private void редактированиеToolStripMenuItem_Click_1(object sender, EventArgs e)//Открывается окно редактирования покупателей
        {
            DeleteClientWindowShow();
            DeleteClient.Visible = false;
            UpdateClient.Visible = true;
            FillClientData();
        }

      public void ShopDataSelect()
        {
  SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();


            // Создаем команду SQL для выборки данных из таблицы
            SqlCommand sqlQuery = new SqlCommand("SELECT Пользователи.Логин, Пользователи.Пароль, Магазин.Название_магазина, Магазин.Дата_регистрации, Магазин.ИНН, Магазин.Сайт" +
                " FROM [dbo].[Пользователи] INNER JOIN Магазин ON Магазин.Сопоставленный_пользователь=Пользователи.ID_Пользователя ", sqlConnect);


            // Создаем адаптер данных для заполнения DataSet
            SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery);

            // Создаем DataSet для хранения данных
            DataSet dataSet = new DataSet();

            // Заполняем DataSet данными из таблицы
            adapter.Fill(dataSet);

            // Закрываем соединение
            sqlConnect.Close();

            // Устанавливаем DataSet как источник данных для DataGridView
            ShopTable.DataSource = dataSet.Tables[0];
        }
        
        private void просмотрToolStripMenuItem_Click(object sender, EventArgs e)//Просмотр магазинов
        {
            ClientData.Visible = false;
            ClientInserting.Visible = false;
            ClientDeleting.Visible = false;
            ShopData.Visible = false;
            ShopInserting.Visible = false;
            ShopDeleting.Visible = false;
            GameData.Visible = false;
            GameInserting.Visible = false;
            GameDeleting.Visible = false;
            GameUpdating.Visible = false;           
            ShopData.Visible = true;
            ShopData.Size = new Size(1000, 250);
            ShopData.Location = new Point(20, 30);
            ShopDataSelect();
        }

        private void ПросмотрToolStripMenuItem1_Click_1(object sender, EventArgs e)//Открывается окно добавления магазинов
        {
            
        }

        private void добавлениеToolStripMenuItem1_Click_1(object sender, EventArgs e)//Открывается окно добавления магазинов
        {
            InsertShopWindowShow();
        }

        private void удалениеToolStripMenuItem1_Click_1(object sender, EventArgs e)//Открывается окно удаления магазинов
        {
          

            DeleteShopWindowShow();
  FillShopData();
        }

       

        private void редактированиеToolStripMenuItem1_Click_1(object sender, EventArgs e)//Открывается окно редактирования магазинов
        {
            UpdateShopWindowShow();
            FillShopData();

        }

        public void GameDataSelect()
        {
  SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            

            // Создаем команду SQL для выборки данных из таблицы
            SqlCommand sqlQuery = new SqlCommand("SELECT Настольная_игра.Артикул, Настольная_игра.Название, Настольная_игра.Изображение_игры, Настольная_игра.Минимальное_количество_игроков, " +
                "Настольная_игра.Максимальное_количество_игроков, Настольная_игра.Возрастное_ограничение, Настольная_игра.Год_издания, Настольная_игра.Рейтинг, Жанр.Название_жанра, Изготовитель.Название_фирмы, Страна.Название_страны FROM Настольная_игра " +
                "INNER JOIN Жанр ON Настольная_игра.жанр = Жанр.Код_жанра " +
                "INNER JOIN Изготовитель ON Настольная_игра.Производитель=Изготовитель.Код_производителя " +
                "INNER JOIN Страна ON Изготовитель.Страна=Страна.Код_страны", sqlConnect);
          

            // Создаем адаптер данных для заполнения DataSet
            SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery);

            // Создаем DataSet для хранения данных
            DataSet dataSet = new DataSet();

            // Заполняем DataSet данными из таблицы
            adapter.Fill(dataSet);

            
          

            GamesTable.AutoGenerateColumns = false;

            // Создаем столбец для отображения картинок


            DataGridViewColumn articleColumn = new DataGridViewTextBoxColumn();
            articleColumn.HeaderText = "Артикул";
            articleColumn.DataPropertyName = "Артикул";
            GamesTable.Columns.Add(articleColumn);

            DataGridViewColumn nameColumn = new DataGridViewTextBoxColumn();
            nameColumn.HeaderText = "Название";
            nameColumn.DataPropertyName = "Название";
            GamesTable.Columns.Add(nameColumn);

            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
            imageColumn.HeaderText = "Изображение";
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            imageColumn.DataPropertyName = "Изображение_игры";
            GamesTable.Columns.Add(imageColumn);

            // Добавьте остальные столбцы...

            DataGridViewColumn minPlayerCountColumn = new DataGridViewTextBoxColumn();
            minPlayerCountColumn.HeaderText = "Мин. кол-во игроков";
            minPlayerCountColumn.DataPropertyName = "Минимальное_количество_игроков";
            GamesTable.Columns.Add(minPlayerCountColumn);

            DataGridViewColumn maxPlayerCountColumn = new DataGridViewTextBoxColumn();
            maxPlayerCountColumn.HeaderText = "Макс. кол-во игроков";
            maxPlayerCountColumn.DataPropertyName = "Максимальное_количество_игроков";
            GamesTable.Columns.Add(maxPlayerCountColumn);

            DataGridViewColumn AgeColumn = new DataGridViewTextBoxColumn();
            AgeColumn.HeaderText = "Возрастное ограничение";
            AgeColumn.DataPropertyName = "Возрастное_ограничение";
            GamesTable.Columns.Add(AgeColumn);

            DataGridViewColumn YearColumn = new DataGridViewTextBoxColumn();
            YearColumn.HeaderText = "Год издания";
            YearColumn.DataPropertyName = "Год_издания";
            GamesTable.Columns.Add(YearColumn);

            DataGridViewColumn JenreColumn = new DataGridViewTextBoxColumn();
            JenreColumn.HeaderText = "Название жанра";
            JenreColumn.DataPropertyName = "Название_жанра";
            GamesTable.Columns.Add(JenreColumn);

            DataGridViewColumn ManufacturerColumn = new DataGridViewTextBoxColumn();
            ManufacturerColumn.HeaderText = "Производитель";
            ManufacturerColumn.DataPropertyName = "Название_фирмы";
            GamesTable.Columns.Add(ManufacturerColumn);

            DataGridViewColumn CountryColumn = new DataGridViewTextBoxColumn();
            CountryColumn.HeaderText = "Страна";
            CountryColumn.DataPropertyName = "Название_страны";
            GamesTable.Columns.Add(CountryColumn);

            DataGridViewColumn RatingColumn = new DataGridViewTextBoxColumn();
            RatingColumn.HeaderText = "Рейтинг";
            RatingColumn.DataPropertyName = "Рейтинг";
            GamesTable.Columns.Add(RatingColumn);


            foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                // Получаем данные из строки
           

                string article = row["Артикул"].ToString();
                string name = row["Название"].ToString();
                string imagePath = row["Изображение_игры"].ToString();
                string MinPlayerCount = row["Минимальное_количество_игроков"].ToString();
                string MaxPlayerCount = row["Максимальное_количество_игроков"].ToString();
                string Age = row["Возрастное_ограничение"].ToString();
                string Year = row["Год_издания"].ToString();
                
                string JenreName = row["Название_жанра"].ToString();
                string Manufacturer = row["Название_фирмы"].ToString();
                string Country = row["Название_страны"].ToString();
               string Rating = row["Рейтинг"].ToString();
                // Остальные атрибуты...

                // Загружаем изображение из пути
                Image image = Image.FromFile(imagePath);

                    // Создаем строку и заполняем ее данными
                    DataGridViewRow dataGridViewRow = new DataGridViewRow();
                DataGridViewColumn dataGridViewColumn = new DataGridViewColumn();
               
                    dataGridViewRow.CreateCells(GamesTable);
              
                    dataGridViewRow.Cells[0].Value = article;
                    dataGridViewRow.Cells[1].Value = name;
                dataGridViewRow.Cells[2].Value = image;
                dataGridViewRow.Cells[3].Value = MinPlayerCount;
                dataGridViewRow.Cells[4].Value = MaxPlayerCount;
                dataGridViewRow.Cells[5].Value = Age;
                dataGridViewRow.Cells[6].Value = Year;              
                dataGridViewRow.Cells[7].Value = JenreName;
                dataGridViewRow.Cells[8].Value = Manufacturer;
                dataGridViewRow.Cells[9].Value = Country;
                dataGridViewRow.Cells[10].Value = Rating;

                dataGridViewRow.Height = 200;
                dataGridViewColumn.Width = 150;
                // Остальные значения...

                // Добавляем строку в таблицу
                GamesTable.Rows.Add(dataGridViewRow);
             
                }


        
   
                 
  sqlConnect.Close();// Закрываем соединение   

        }
            


  private void просмотрToolStripMenuItem1_Click(object sender, EventArgs e)//Просмотр магазинов
        {
          
            ClientData.Visible = false;
            ShopData.Visible = false;
            GameData.Visible=true;
            GamesTable.VirtualMode = false;
            GamesTable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GameData.Size = new Size(1750, 400);
            GameData.Location = new Point(20, 30);
            GameDataSelect();
        } 
        
        private void добавлениеToolStripMenuItem2_Click_1(object sender, EventArgs e)//Открывается окно добавления магазинов
        {
            InsertGameWindowShow();
            FillJenreData();
            FillManufacturerData();
        }

        private void удалениеToolStripMenuItem2_Click_1(object sender, EventArgs e)//Открывается окно удаления настольных игр
        {
            DeleteGameWindowShow();
            DeleteFillGameData();
        }

        private void редактированиеToolStripMenuItem2_Click_1(object sender, EventArgs e)
        {
            FillUpdateJenreData();
            UpdateGameWindowShow();
            UpdateFillGameData();
            FillUpdateManufacturerData();


        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void GamesData_Enter(object sender, EventArgs e)
        {

        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            LoadImage();
        }

        public void LoadImage()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    imageUrl = ofd.FileName;
                    pictureBox1.Image = Image.FromFile(ofd.FileName);
                    FilePath = ofd.FileName.ToString();
                }
            }

        }

      
        

        private void fillByToolStripButton_Click_1(object sender, EventArgs e)
        {
            try
            {
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private void GameDelete_Click_1(object sender, EventArgs e)
        {

            DeleteGameData();
        }

     

        private void DeleteClient_Click(object sender, EventArgs e)
        {
            DeleteClientData();
        }

        

        private void CompleteClientInsert_Click(object sender, EventArgs e)
        {
            InsertClientData();
        }

        private void DeleteShop_Click(object sender, EventArgs e)
        {
            DeleteShopData();
        }

        

        private void UpdateShop_Click(object sender, EventArgs e)
        {
            UpdateShopData();
        }

        private void GoGameInserting_Click(object sender, EventArgs e)
        {
            InsertGameWindowShow();
        }

        private void GoGameDeleting_Click(object sender, EventArgs e)
        {
            DeleteGameWindowShow();
        }

        private void GoClientInsert_Click(object sender, EventArgs e)
        {
            InsertClientWindowShow();
        }

        private void GoClientDelete_Click(object sender, EventArgs e)
        {
            DeleteClientWindowShow();
            UpdateClient.Visible = false;
            DeleteClient.Visible = true;
        }

        private void GoClientUpdate_Click(object sender, EventArgs e)
        {
            DeleteClientWindowShow();
            DeleteClient.Visible = false;
            UpdateClient.Visible = true;
        }

        private void GoInsertShop_Click(object sender, EventArgs e)
        {
            InsertShopWindowShow();
        }

        private void GoShopUpdate_Click(object sender, EventArgs e)
        {
            UpdateShopWindowShow();
            DeleteShop.Visible = false;
            UpdateShop.Visible = true;
        }

        private void GoShopDelete_Click(object sender, EventArgs e)
        {
            DeleteShopWindowShow();
        }

        private void просмотрToolStripMenuItem1_Click_2(object sender, EventArgs e)
        {
            ClientData.Visible = false;
            ClientInserting.Visible = false;
            ClientDeleting.Visible = false;
            ShopData.Visible = false;
            ShopInserting.Visible = false;
            ShopDeleting.Visible = false;
            
            GameInserting.Visible = false;
            GameDeleting.Visible = false;
            GameUpdating.Visible = false;
            GameData.Visible = true;
            GameData.Size = new Size(1500, 500);
            GameData.Location = new Point(20, 30);
            GameDataSelect();
        }

        private void просморToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            ClientData.Visible = false;
            ClientInserting.Visible = false;
            ClientDeleting.Visible = false;
            ShopData.Visible = false;
            ShopInserting.Visible = false;
            ShopDeleting.Visible = false;

            GameInserting.Visible = false;
            GameDeleting.Visible = false;
            GameUpdating.Visible = false;
            ClientData.Visible = true;
            ClientData.Size = new Size(1500, 500);
            ClientData.Location = new Point(20, 30);
            ClientDataSelect();
        }

        private void покупателиToolStripMenuItem_Click_2(object sender, EventArgs e)
        {

        }

        private void магазиныToolStripMenuItem_Click_2(object sender, EventArgs e)
        {

        }

        private void пунктыВыдачиToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        public void FillJenreData()
        {

            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            SqlCommand sqlQuery = new SqlCommand("SELECT Жанр.Код_жанра, Жанр.Название_жанра FROM Жанр", sqlConnect);
            SqlDataReader sqlReader = sqlQuery.ExecuteReader();


            // Создайте адаптер данных для заполнения DataTable
            SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery);

            // Создайте DataTable для хранения данных
            DataTable dataTable = new DataTable();
            Jenre.Items.Clear();


            while (sqlReader.Read())
            {
             string JenreName = sqlReader["Название_жанра"].ToString();
                Jenre.Items.Add(JenreName);

               

            }




        }


  

        private void Jenre_SelectedIndexChanged(object sender, EventArgs e)
        {
      
                int SelectedJenreID = Convert.ToInt32(Jenre.SelectedIndex)+1;



                using (SqlConnection sqlConnect = new SqlConnection(connection))
                {
                    sqlConnect.Open();

                    // Получить номер пункта выдачи


                    SqlCommand sqlView = new SqlCommand("SELECT Жанр.Код_жанра, Жанр.Название_жанра" +
                        " FROM [dbo].[Жанр] WHERE Код_Жанра=" + SelectedJenreID + "", sqlConnect);




                    using (SqlDataReader sqlReader = sqlView.ExecuteReader())
                    {
                        if (sqlReader.Read())
                        {
                        string Name = sqlReader["Название_жанра"].ToString(); 
                        }
                    }


                    sqlConnect.Close();

                }
            }

        private void Manufacturer_SelectedIndexChanged(object sender, EventArgs e)
        {
            int SelectedManufacturerID = Convert.ToInt32(Manufacturer.SelectedIndex) + 1;



            using (SqlConnection sqlConnect = new SqlConnection(connection))
            {
                sqlConnect.Open();

                // Получить номер пункта выдачи


                SqlCommand sqlView = new SqlCommand("SELECT Изготовитель.Код_производителя, Изготовитель.Название_фирмы" +
                    " FROM [dbo].[Изготовитель] WHERE Изготовитель.Код_производителя=" + SelectedManufacturerID + "", sqlConnect);




                using (SqlDataReader sqlReader = sqlView.ExecuteReader())
                {
                    if (sqlReader.Read())
                    {
                        string Name = sqlReader["Название_фирмы"].ToString();
                    }
                }


                sqlConnect.Close();

            }
        }
    
      public void FillManufacturerData()
        {

            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            SqlCommand sqlQuery = new SqlCommand("SELECT Изготовитель.Код_производителя, Изготовитель.Название_фирмы FROM [dbo].[Изготовитель]", sqlConnect);
            SqlDataReader sqlReader = sqlQuery.ExecuteReader();


            // Создайте адаптер данных для заполнения DataTable
            SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery);

            // Создайте DataTable для хранения данных
            DataTable dataTable = new DataTable();
            Manufacturer.Items.Clear();


            while (sqlReader.Read())
            {
                string ManufacturerName = sqlReader["Название_фирмы"].ToString();
                Manufacturer.Items.Add(ManufacturerName);



            }




        }

        private void Game_SelectedIndexChanged(object sender, EventArgs e)
        {
            int SelectedGameID = Convert.ToInt32(Game.SelectedIndex) + 1;



            using (SqlConnection sqlConnect = new SqlConnection(connection))
            {
                sqlConnect.Open();

                // Получить номер пункта выдачи


                SqlCommand sqlView = new SqlCommand("SELECT Настольная_игра.Артикул, Настольная_игра.Название" +
                    " FROM [dbo].[Настольная_игра] WHERE Настольная_игра.Артикул=" + SelectedGameID + "", sqlConnect);




                using (SqlDataReader sqlReader = sqlView.ExecuteReader())
                {
                    if (sqlReader.Read())
                    {
                        string GameName = sqlReader["Название"].ToString();
                    }
                }


                sqlConnect.Close();

            }
        }

        public void DeleteFillGameData()
        {

            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            SqlCommand sqlQuery = new SqlCommand("SELECT Настольная_игра.Артикул, Настольная_игра.Название FROM [dbo].[Настольная_игра]", sqlConnect);
            SqlDataReader sqlReader = sqlQuery.ExecuteReader();


            // Создайте адаптер данных для заполнения DataTable
            SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery);

            // Создайте DataTable для хранения данных
            DataTable dataTable = new DataTable();
            Game.Items.Clear();


            while (sqlReader.Read())
            {
                string GameName = sqlReader["Название"].ToString();
                Game.Items.Add(GameName);



            }




        }

        public void UpdateFillGameData()
        {

            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            SqlCommand sqlQuery = new SqlCommand("SELECT Настольная_игра.Артикул, Настольная_игра.Название FROM [dbo].[Настольная_игра]", sqlConnect);
            SqlDataReader sqlReader = sqlQuery.ExecuteReader();


            // Создайте адаптер данных для заполнения DataTable
            SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery);

            // Создайте DataTable для хранения данных
            DataTable dataTable = new DataTable();
            UpdateName.Items.Clear();


            while (sqlReader.Read())
            {
                string GameName = sqlReader["Название"].ToString();
                UpdateName.Items.Add(GameName);
            }
        }

        private void UpdateName_SelectedIndexChanged_1(object sender, EventArgs e)
        {
         

            if (UpdateName.SelectedItem != null)
            {
   int SelectedGameID = Convert.ToInt32(UpdateName.SelectedIndex) + 1;
                SqlConnection sqlConnect = new SqlConnection(connection);
                sqlConnect.Open();


                SqlCommand sqlView = new SqlCommand("SELECT Настольная_игра.Название, Настольная_игра.Изображение_игры, Настольная_игра.Минимальное_количество_игроков, " +
                    "Настольная_игра.Максимальное_количество_игроков, Настольная_игра.Возрастное_ограничение, Настольная_игра.Год_издания, Настольная_игра.Рейтинг, Жанр.Название_жанра, Изготовитель.Название_фирмы, Страна.Название_страны FROM Настольная_игра " +
                    "INNER JOIN Жанр ON Настольная_игра.жанр = Жанр.Код_жанра " +
                    "INNER JOIN Изготовитель ON Настольная_игра.Производитель=Изготовитель.Код_производителя " +
                    "INNER JOIN Страна ON Изготовитель.Страна=Страна.Код_страны WHERE Настольная_игра.Название='" + UpdateName.Text + "'", sqlConnect);

                SqlDataReader sqlReader = sqlView.ExecuteReader();

                while (sqlReader.Read())
                {
                    string GameName = sqlReader["Название"].ToString();
                    string Picture = sqlReader["Изображение_игры"].ToString();
                    UpdatePicture.Image = new Bitmap(Picture);
                    UpdateMinPlayerCount.Text = sqlReader["Минимальное_количество_игроков"].ToString();
                    UpdateMaxPlayerCount.Text = sqlReader["Максимальное_количество_игроков"].ToString();
                    UpdateAge.Text = sqlReader["Возрастное_ограничение"].ToString();
                    UpdateJenre.Text = sqlReader["Название_жанра"].ToString();
                    UpdateManufacturer.Text = sqlReader["Название_фирмы"].ToString();
                    UpdateYear.Text = sqlReader["Год_издания"].ToString();
                    UpdateRating.Text = sqlReader["Рейтинг"].ToString();
                }

                sqlReader.Close();
                sqlConnect.Close();
            }
        }

      

        private void UpdateJenre_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            int SelectedJenreID = Convert.ToInt32(UpdateJenre.SelectedIndex) + 1;



            using (SqlConnection sqlConnect = new SqlConnection(connection))
            {
                sqlConnect.Open();

                // Получить номер пункта выдачи


                SqlCommand sqlView = new SqlCommand("SELECT Жанр.Код_жанра, Жанр.Название_жанра" +
                    " FROM [dbo].[Жанр] WHERE Код_Жанра=" + SelectedJenreID + "", sqlConnect);




                using (SqlDataReader sqlReader = sqlView.ExecuteReader())
                {
                    if (sqlReader.Read())
                    {
                        string Name = sqlReader["Название_жанра"].ToString();
                    }
                }


                sqlConnect.Close();

            }
        }

        public void FillUpdateJenreData()
        {

            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            SqlCommand sqlQuery = new SqlCommand("SELECT Жанр.Код_жанра, Жанр.Название_жанра FROM Жанр", sqlConnect);
            SqlDataReader sqlReader = sqlQuery.ExecuteReader();


            // Создайте адаптер данных для заполнения DataTable
            SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery);

            // Создайте DataTable для хранения данных
            DataTable dataTable = new DataTable();
            UpdateJenre.Items.Clear();

            while (sqlReader.Read())
            {
                string JenreName = sqlReader["Название_жанра"].ToString();
                UpdateJenre.Items.Add(JenreName);

            }

        }

        public void FillUpdateManufacturerData()
        {

            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            SqlCommand sqlQuery = new SqlCommand("SELECT Изготовитель.Код_производителя, Изготовитель.Название_фирмы FROM [dbo].[Изготовитель]", sqlConnect);
            SqlDataReader sqlReader = sqlQuery.ExecuteReader();


            // Создайте адаптер данных для заполнения DataTable
            SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery);

            // Создайте DataTable для хранения данных
            DataTable dataTable = new DataTable();
            UpdateManufacturer.Items.Clear();


            while (sqlReader.Read())
            {
                string ManufacturerName = sqlReader["Название_фирмы"].ToString();
                UpdateManufacturer.Items.Add(ManufacturerName);

            }

        }  
        
        private void UpdateManufacturer_SelectedIndexChanged(object sender, EventArgs e)
        {
            int SelectedManufacturerID = Convert.ToInt32(UpdateManufacturer.SelectedIndex) + 1;
            using (SqlConnection sqlConnect = new SqlConnection(connection))
            {
                sqlConnect.Open();

                // Получить номер пункта выдачи


                SqlCommand sqlView = new SqlCommand("SELECT Изготовитель.Код_производителя, Изготовитель.Название_фирмы" +
                    " FROM [dbo].[Изготовитель] WHERE Изготовитель.Код_производителя=" + SelectedManufacturerID + "", sqlConnect);

                using (SqlDataReader sqlReader = sqlView.ExecuteReader())
                {
                    if (sqlReader.Read())
                    {
                        string Name = sqlReader["Название_фирмы"].ToString();
                    }
                }


                sqlConnect.Close();

            }
        }

private void Shop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Shop.SelectedItem != null)
            {
                SqlConnection sqlConnect = new SqlConnection(connection);
                sqlConnect.Open();
                int SelectedShopID = Convert.ToInt32(Shop.SelectedIndex) + 1;

             

                SqlCommand sqlView = new SqlCommand(@"SELECT Логин, Пароль, Название_магазина, ИНН, Сайт, Дата_регистрации
FROM [dbo].[Магазин] INNER JOIN [dbo].[Пользователи] ON Магазин.Сопоставленный_пользователь = Пользователи.ID_Пользователя 
WHERE Название_магазина='" + Shop.Text + "'", sqlConnect);

                SqlDataReader sqlReader = sqlView.ExecuteReader();

                while (sqlReader.Read())
                {
                    string ShopName = sqlReader["Название_магазина"].ToString();
                    DeleteShopLogin.Text = sqlReader["Логин"].ToString();
                    DeleteShopPassword.Text = sqlReader["Пароль"].ToString();
                    DeleteShopName.Text = sqlReader["Название_магазина"].ToString();
                    DeleteIdentity.Text = sqlReader["ИНН"].ToString();
                    DeleteSite.Text = sqlReader["Сайт"].ToString();
                   
                    DeleteShopRegDate.Text = sqlReader["Дата_регистрации"].ToString();
                }
                sqlReader.Close();
                sqlConnect.Close();

            }
        }

        public void FillShopData()
        {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            SqlCommand sqlQuery = new SqlCommand("SELECT Магазин.Код_магазина, Магазин.Название_магазина FROM [dbo].[Магазин]", sqlConnect);
            SqlDataReader sqlReader = sqlQuery.ExecuteReader();


            // Создайте адаптер данных для заполнения DataTable
            SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery);

            // Создайте DataTable для хранения данных
            DataTable dataTable = new DataTable();
            Shop.Items.Clear();


            while (sqlReader.Read())
            {
                string ShopName = sqlReader["Название_магазина"].ToString();
                Shop.Items.Add(ShopName);

            }
        }

private void Client_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Client.SelectedItem != null)
            {
                SqlConnection sqlConnect = new SqlConnection(connection);
                sqlConnect.Open();
                int SelectedClientID = Convert.ToInt32(Client.SelectedIndex) + 1;
              

                

                SqlCommand sqlView = new SqlCommand(@"SELECT Логин, Пароль, ID_Покупателя, Фамилия, Имя, Отчество, Дата_регистрации, Дата_рождения 
FROM [dbo].[Покупатель] INNER JOIN [dbo].[Пользователи] ON Покупатель.Сопоставленный_пользователь = Пользователи.ID_Пользователя 
WHERE ID_Покупателя='" + SelectedClientID + "'", sqlConnect);

                SqlDataReader sqlReader = sqlView.ExecuteReader();

                while (sqlReader.Read())
                {
                    DeleteLogin.Text = sqlReader["Логин"].ToString();
                    DeletePassword.Text = sqlReader["Пароль"].ToString();
                    DeleteSurname.Text = sqlReader["Фамилия"].ToString();
                    DeleteName.Text = sqlReader["Имя"].ToString();
                    DeleteThirdName.Text = sqlReader["Отчество"].ToString();
                    DeleteBirthDate.Text = sqlReader["Дата_рождения"].ToString();
                    DeleteRegDate.Text = sqlReader["Дата_регистрации"].ToString();
                }
             sqlReader.Close();
                sqlConnect.Close();
            
            }

       
    
        }
 public void FillClientData()
            {
                SqlConnection sqlConnect = new SqlConnection(connection);
                sqlConnect.Open();
                SqlCommand sqlQuery = new SqlCommand("SELECT Покупатель.ID_покупателя, Покупатель.Фамилия, Имя, Отчество FROM [dbo].[Покупатель]", sqlConnect);
                SqlDataReader sqlReader = sqlQuery.ExecuteReader();


                // Создайте адаптер данных для заполнения DataTable
                SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery);

                // Создайте DataTable для хранения данных
                DataTable dataTable = new DataTable();
                Client.Items.Clear();


                while (sqlReader.Read())
                {
                    string Surname = sqlReader["Фамилия"].ToString();
                Client.Items.Add(Surname);

                }
            }

        private void GameName_TextChanged(object sender, EventArgs e)
        {

        }

        private void AdminWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }    
}
