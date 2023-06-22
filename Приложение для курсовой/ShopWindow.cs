using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Приложение_для_курсовой
{
    public partial class ShopWindow : Form
    {
public int UserID;
        public int ShopID;
        
        public ShopWindow(int userID, int shopID)
        {

            InitializeComponent();
            UserID = userID;
            ShopID = shopID;
            ShowProfileData(UserID);
            OrdersData.CellContentClick += Button_CLick;
            OrdersData.CellClick += OrdersData_CellClick;
            OrderDeliveryList.CellClick += OrderDeliveryList_CellClick;
            OrderDeliveryList.CellContentClick += OrderDeliveryList_CellContentClick;
            DeliveryList.CellClick += DeliveryList_CellClick;
            ReportList.CellClick += ReportList_CellClick;

        }
        public string connection = @"Data Source=DESKTOP-NIKISIN\JET; Initial Catalog=Онлайн-магазин; User ID=sa; Password=Q1w2e3r4";

        private void GameName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedGame = GameName.SelectedValue.ToString();

            using (SqlConnection sqlConnect = new SqlConnection(connection))
            {
                sqlConnect.Open();

                // Получите номер ассортимента выбранной игры
                SqlCommand assortmentQuery = new SqlCommand("SELECT Настольная_игра.Артикул FROM [dbo].[Настольная_игра] WHERE Название = @GameName", sqlConnect);

                // Создайте команду SQL с параметрами

                assortmentQuery.Parameters.AddWithValue("@GameName", selectedGame);
                assortmentQuery.Parameters.AddWithValue("@ShopID", ShopID);

                // Выполните запрос и получите номер ассортимента
                int assortmentNumber = Convert.ToInt32(assortmentQuery.ExecuteScalar());

                // Измените картинку в PictureBox, связанную с выбранной игрой
                SqlCommand sqlView = new SqlCommand("SELECT Изображение_игры FROM [dbo].[Настольная_игра] WHERE Название = @GameName", sqlConnect);
                sqlView.Parameters.AddWithValue("@GameName", selectedGame);



                using (SqlDataReader sqlReader = sqlView.ExecuteReader())
                {
                    if (sqlReader.Read())
                    {
                        string picture = sqlReader["Изображение_игры"].ToString();
                        GamePicture.Image = new Bitmap(picture);
                    }
                }
                InsertCount.Text = "";
                InsertAvalaible.Text = "";
                InsertPrice.Text = "";


                sqlConnect.Close();

            }


           
        }
 public void FillNewAssortment()
            {

                SqlConnection sqlConnect = new SqlConnection(connection);
                sqlConnect.Open();
                SqlCommand sqlQuery = new SqlCommand("SELECT DISTINCT Артикул, Название FROM [dbo].[Настольная_игра] WHERE Настольная_игра.Артикул " +
                    "NOT IN (SELECT Настольная_игра FROM [dbo].[Ассортимент] WHERE Магазин = @ShopID)", sqlConnect);


                sqlQuery.Parameters.AddWithValue("@ShopID", ShopID);

                // Создайте адаптер данных для заполнения DataTable
                SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery);

                // Создайте DataTable для хранения данных
                DataTable dataTable = new DataTable();




                // Заполним DataTable данными из запроса
                adapter.Fill(dataTable);

            // Установите DataTable как источник данных для ComboBox
            GameName.DataSource = dataTable;
            GameName.DisplayMember = "Название";
            GameName.ValueMember = "Название";

                // Получите номер ассортимента выбранной игры
                string assortmentQuery = "SELECT Номер_ассортимента FROM [dbo].[Ассортимент] INNER JOIN [dbo].[Настольная_игра] ON " +
                    "Ассортимент.Настольная_игра = Настольная_игра.Артикул WHERE Название = @GameName AND Магазин = @ShopID";

                // Создайте команду SQL с параметрами
                SqlCommand assortmentSqlCommand = new SqlCommand(assortmentQuery, sqlConnect);
                string selectedGame = GameName.SelectedValue.ToString();
                assortmentSqlCommand.Parameters.AddWithValue("@GameName", selectedGame);
                assortmentSqlCommand.Parameters.AddWithValue("@ShopID", ShopID);



                SqlCommand sqlView = new SqlCommand("SELECT Изображение_игры FROM [dbo].[Настольная_игра] WHERE Название = @GameName", sqlConnect);
                sqlView.Parameters.AddWithValue("@GameName", selectedGame);



                using (SqlDataReader sqlReader = sqlView.ExecuteReader())
                {
                    if (sqlReader.Read())
                    {
                        string picture = sqlReader["Изображение_игры"].ToString();
                        GamePicture.Image = new Bitmap(picture);
                    }


                    sqlConnect.Close();
                }

                InsertCount.Enabled = true;
                InsertAvalaible.Enabled = true;
                InsertPrice.Enabled = true;

            }

        private void Game_SelectedIndexChanged(object sender, EventArgs e)
        {


            string selectedGame = GameName.SelectedValue.ToString();

            using (SqlConnection sqlConnect = new SqlConnection(connection))
            {
                sqlConnect.Open();

                // Получите номер ассортимента выбранной игры
                SqlCommand assortmentQuery = new SqlCommand("SELECT Настольная_игра.Артикул FROM [dbo].[Настольная_игра] WHERE Название = @GameName", sqlConnect);

                // Создайте команду SQL с параметрами

                assortmentQuery.Parameters.AddWithValue("@GameName", selectedGame);
                assortmentQuery.Parameters.AddWithValue("@ShopID", ShopID);

                // Выполните запрос и получите номер ассортимента
                int assortmentNumber = Convert.ToInt32(assortmentQuery.ExecuteScalar());

                // Измените картинку в PictureBox, связанную с выбранной игрой
                SqlCommand sqlView = new SqlCommand("SELECT Изображение_игры FROM [dbo].[Настольная_игра] WHERE Название = @GameName", sqlConnect);
                sqlView.Parameters.AddWithValue("@GameName", selectedGame);



                using (SqlDataReader sqlReader = sqlView.ExecuteReader())
                {
                    if (sqlReader.Read())
                    {
                        string picture = sqlReader["Изображение_игры"].ToString();
                        GamePicture.Image = new Bitmap(picture);
                    }
                }
                InsertCount.Text = "";
                InsertAvalaible.Text = "";
                InsertPrice.Text = "";


                sqlConnect.Close();

            }


        }

        public void FillDeleteAssortment()
        {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            SqlCommand sqlQuery = new SqlCommand("SELECT Настольная_игра.Артикул, Настольная_игра.Название FROM [dbo].[Настольная_игра] " +
                "INNER JOIN [dbo].[Ассортимент] ON Настольная_игра.Артикул = Ассортимент.Настольная_игра WHERE Ассортимент.Магазин = @ShopID", sqlConnect);
            sqlQuery.Parameters.AddWithValue("@ShopID", ShopID);


            SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery);

            // Создайте DataTable для хранения данных
            DataTable dataTable = new DataTable();




            // Заполним DataTable данными из запроса
            adapter.Fill(dataTable);


            DeleteGame.DataSource = dataTable;
            DeleteGame.ValueMember = "Название";
            DeleteGame.DisplayMember = "Название";



            //Получите номер ассортимента выбранной игры
            string assortmentQuery = "SELECT Номер_ассортимента FROM [dbo].[Ассортимент] INNER JOIN [dbo].[Настольная_игра] ON " +
                "Ассортимент.Настольная_игра = Настольная_игра.Артикул WHERE Название = @GameName AND Магазин = @ShopID";

            // Создайть команду SQL с параметрами
            SqlCommand assortmentSqlCommand = new SqlCommand(assortmentQuery, sqlConnect);
            string selectedGame = DeleteGame.SelectedValue.ToString();
            assortmentSqlCommand.Parameters.AddWithValue("@GameName", selectedGame);
            assortmentSqlCommand.Parameters.AddWithValue("@ShopID", ShopID);


        }


        public void FillUpdateAssortment()
        {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            SqlCommand sqlQuery = new SqlCommand("SELECT Настольная_игра.Артикул, Настольная_игра.Название FROM [dbo].[Настольная_игра] " +
                 "INNER JOIN [dbo].[Ассортимент] ON Настольная_игра.Артикул = Ассортимент.Настольная_игра WHERE Ассортимент.Магазин = @ShopID", sqlConnect);
            sqlQuery.Parameters.AddWithValue("@ShopID", ShopID);



            // Создайте адаптер данных для заполнения DataTable
            SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery);

            // Создайте DataTable для хранения данных
            DataTable dataTable = new DataTable();

            // Заполним DataTable данными из запроса
            adapter.Fill(dataTable);

            // Установите DataTable как источник данных для ComboBox
            UpdateGame.DataSource = dataTable;
            UpdateGame.DisplayMember = "Название";
            UpdateGame.ValueMember = "Название";

            // Получите номер ассортимента выбранной игры
            string assortmentQuery = "SELECT Номер_ассортимента FROM [dbo].[Ассортимент] INNER JOIN [dbo].[Настольная_игра] ON " +
                "Ассортимент.Настольная_игра = Настольная_игра.Артикул WHERE Название = @GameName AND Магазин = @ShopID";

            // Создайте команду SQL с параметрами
            SqlCommand assortmentSqlCommand = new SqlCommand(assortmentQuery, sqlConnect);
            string selectedGame = UpdateGame.SelectedValue.ToString();
            assortmentSqlCommand.Parameters.AddWithValue("@GameName", selectedGame);
            assortmentSqlCommand.Parameters.AddWithValue("@ShopID", ShopID);

            SqlCommand sqlView = new SqlCommand("SELECT Настольная_игра.Изображение_игры, Ассортимент.Количество, Ассортимент.Наличие, Ассортимент.Цена FROM [dbo].[Настольная_игра] " +
                   "INNER JOIN [dbo].[Ассортимент] ON Ассортимент.Настольная_игра=Настольная_игра.Артикул " +
                   "WHERE Настольная_игра.Название = @GameName AND Ассортимент.Магазин='" + ShopID + "' ", sqlConnect);
            sqlView.Parameters.AddWithValue("@GameName", selectedGame);



            using (SqlDataReader sqlReader = sqlView.ExecuteReader())
            {
                if (sqlReader.Read())
                {
                    string picture = sqlReader["Изображение_игры"].ToString();
                    UpdatePicture.Image = new Bitmap(picture);
                    UpdateCount.Text = sqlReader["Количество"].ToString();
                    UpdateAvailable.Text = sqlReader["Наличие"].ToString();
                    UpdatePrice.Text = sqlReader["Цена"].ToString();

                }


                sqlConnect.Close();
            }

            InsertCount.Enabled = true;
            InsertAvalaible.Enabled = true;
            InsertPrice.Enabled = true;

        }

        public void Delete()
        {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            string selectedGame = DeleteGame.Text;

            SqlCommand FindGameID = new SqlCommand("SELECT Настольная_игра.Артикул FROM [dbo].[Настольная_игра] WHERE Название ='" + selectedGame + "'", sqlConnect);

            // Создайте команду SQL с параметрами
            int GameID = Convert.ToInt32(FindGameID.ExecuteScalar());

            SqlCommand FindAssortmentID = new SqlCommand("SELECT Ассортимент.Номер_ассортимента FROM [dbo].[Ассортимент] " +
                "INNER JOIN [dbo].[Настольная_игра] ON Ассортимент.Настольная_игра=Настольная_игра.Артикул WHERE Ассортимент.Настольная_игра ='" + GameID + "' AND Ассортимент.Магазин='" + ShopID + "'", sqlConnect);

            // Создайте команду SQL с параметрами
            int AssormnentID = Convert.ToInt32(FindAssortmentID.ExecuteScalar());
            // Выполните запрос и получите номер ассортимента



            SqlCommand PurchasetDelete = new SqlCommand(@"DELETE FROM Покупка WHERE Покупка.Ассортимент =  (SELECT Ассортимент.Номер_ассортимента FROM Ассортимент WHERE Покупка.Ассортимент=Ассортимент.Номер_ассортимента AND Ассортимент.Номер_ассортимента ='" + AssormnentID + "')", sqlConnect);
            SqlCommand AssortmentDelete = new SqlCommand(@"DELETE FROM [dbo].[Ассортимент] WHERE Ассортимент.Настольная_игра=" + GameID + " AND Ассортимент.Магазин='" + ShopID + "'", sqlConnect);

            PurchasetDelete.ExecuteNonQuery();
            AssortmentDelete.ExecuteNonQuery();
            sqlConnect.Close();
            MessageBox.Show("Игра удалена из ассортимента", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);


        }

        public void ShowProfileData(int UserID)
        {

            ShopProfile.Location = new Point(20, 30);
            ShopProfile.Size = new Size(400, 400);
            ShopProfile.Visible = true;
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            
            SqlCommand sqlView = new SqlCommand(@"Select Код_магазина, Логин, Пароль, Название_магазина, Дата_регистрации, ИНН, Сайт, Дата_регистрации 
from [dbo].[Магазин] INNER JOIN [dbo].[Пользователи] ON Магазин.Сопоставленный_пользователь=Пользователи.ID_Пользователя WHERE ID_Пользователя='" + UserID + "'", sqlConnect);
            SqlDataReader sqlReader = null;
            sqlReader = sqlView.ExecuteReader();
            while (sqlReader.Read())
            {
                ShopID = (int)sqlReader["Код_магазина"];
                label7.Text = UserID.ToString();
                ShopLogin.Text = sqlReader["Логин"].ToString();
                ShopPassword.Text = sqlReader["Пароль"].ToString();
                ShopName.Text = sqlReader["Название_магазина"].ToString();
                ShopIdentity.Text = sqlReader["ИНН"].ToString();
                ShopSite.Text = sqlReader["Сайт"].ToString();
                ShopRegDate.Text = sqlReader["Дата_регистрации"].ToString();

            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void редактированиеМагазинаToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void профильToolStripMenuItem1_Click(object sender, EventArgs e)
        {
           

        }

        private void заказыОтПокупателейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reports.Visible = false;
            NewOrders.Visible = false;
            DeiiveryDetails.Visible = false;
            ReportDetails.Visible = false;
            OrderDetails.Visible = false;
            Deliveries.Visible = false;
            ProcessedOrders.Visible = false;
            AssortmentData.Visible = false;
            AssortmentInserting.Visible = false;
            AssortmentDeleting.Visible = false;
            AssortmentUpdating.Visible = false;
            ShopProfile.Visible = false;

            ShowOrders();
            NewOrders.Visible = true;
            NewOrders.Size=new Size(1250,600);
            NewOrders.Location = new Point(20, 30);
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ShopWindow_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "_Онлайн_магазинDataSet.Магазин". При необходимости она может быть перемещена или удалена.
            // TODO: данная строка кода позволяет загрузить данные в таблицу "_Онлайн_магазинDataSet.Ассортимент". При необходимости она может быть перемещена или удалена.
            // TODO: данная строка кода позволяет загрузить данные в таблицу "_Онлайн_магазинDataSet.Настольная_игра". При необходимости она может быть перемещена или удалена.

        }

        private void ShopWindow_ShopWindowClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void ShopSite_TextChanged(object sender, EventArgs e)
        {

        }

        private void UpdateProfile_Click(object sender, EventArgs e)
        {
            MessageBox.Show("ID пользователя: " + UserID + "ID магазина:" + ShopID);
            ShopLogin.Enabled = true;
            ShopPassword.Enabled = true;
            ShopName.Enabled = true;
            ShopIdentity.Enabled = true;
            ShopSite.Enabled = true;
            
        }

        public void SaveChanges()
        {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
  SqlCommand sqlFindUserID = new SqlCommand("Select ID_Пользователя FROM [dbo].[Пользователи] INNER JOIN [dbo].[Магазин] ON Магазин.Сопоставленный_пользователь=Пользователи.ID_пользователя WHERE Магазин.Код_магазина='" + ShopID + "'", sqlConnect);
            object result2 = sqlFindUserID.ExecuteScalar();
            int UserID = Convert.ToInt32(result2);
            SqlCommand sqlUpdateUser = new SqlCommand("Update [dbo].[Пользователи] Set Логин='" + ShopLogin.Text + "', Пароль='" + ShopPassword.Text + "'WHERE ID_Пользователя='" + UserID + "' ", sqlConnect);
          
            SqlCommand sqlUpdateClient = new SqlCommand("Update [dbo].[Магазин] Set Название_магазина='" + ShopName.Text + "', ИНН='" + ShopIdentity.Text + "', Сайт='" + ShopSite.Text + "' WHERE Код_магазина='" + ShopID + "' ", sqlConnect);
            sqlUpdateUser.ExecuteNonQuery();
            sqlUpdateClient.ExecuteNonQuery();

            sqlConnect.Close();
            MessageBox.Show("Данные магазина успешно изменены", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Save_Click(object sender, EventArgs e)
        {
            SaveChanges();
        }

        public void GameUpdate()
        {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            string selectedGame = UpdateGame.Text;

            SqlCommand FindGameID = new SqlCommand("SELECT Настольная_игра.Артикул FROM [dbo].[Настольная_игра] WHERE Название ='" + selectedGame + "'", sqlConnect);

            // Создайте команду SQL с параметрами
            int GameID = Convert.ToInt32(FindGameID.ExecuteScalar());


            // Выполните запрос и получите номер ассортимента

            SqlCommand FindAssortmentID = new SqlCommand("SELECT Ассортимент.Номер_ассортимента FROM [dbo].[Ассортимент] " +
                   "INNER JOIN [dbo].[Настольная_игра] ON Ассортимент.Настольная_игра=Настольная_игра.Артикул WHERE Ассортимент.Настольная_игра ='" + GameID + "' AND Ассортимент.Магазин='" + ShopID + "'", sqlConnect);

            int AssortmentID = Convert.ToInt32(FindAssortmentID.ExecuteScalar());
            SqlCommand AssortmentUpdate = new SqlCommand("Update [dbo].[Ассортимент] " +
                "SET Ассортимент.Цена='"+ UpdatePrice.Text+"', Ассортимент.Количество='" + UpdateCount.Text + "', Ассортимент.Наличие='"+ UpdateAvailable.Text+"' " +
                "WHERE Ассортимент.Номер_Ассортимента='"+AssortmentID+"' ", sqlConnect);

            // Измените картинку в PictureBox, связанную с выбранной игрой
            AssortmentUpdate.ExecuteNonQuery();
            sqlConnect.Close();
            MessageBox.Show("Данные ассортимента успешно изменены", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        public void GameDataSelect()
        {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();

            // Создаем команду SQL для выборки данных из таблицы
            SqlCommand sqlQuery = new SqlCommand("SELECT Ассортимент.Номер_ассортимента, Настольная_игра.Название, Настольная_игра.Изображение_игры, Ассортимент.Количество, Ассортимент.Наличие, Ассортимент.Цена From [dbo].[Ассортимент] " +
                "INNER JOIN [dbo].[Настольная_игра] ON Ассортимент.Настольная_игра=Настольная_игра.Артикул WHERE Ассортимент.Магазин='" + ShopID + "'", sqlConnect);


            // Создаем адаптер данных для заполнения DataSet
            SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery);
            // Создаем DataSet для хранения данных
            DataSet dataSet = new DataSet();

            adapter.Fill(dataSet);


            AssortmentTable.AutoGenerateColumns = false;


            // Заполняем DataSet данными из таблицы      
            DataGridViewColumn AssortmentIDColumn = new DataGridViewTextBoxColumn();
            AssortmentIDColumn.HeaderText = "Номер ассортимента";
            AssortmentIDColumn.DataPropertyName = "Номер_ассортимента";
            AssortmentTable.Columns.Add(AssortmentIDColumn);

            DataGridViewColumn nameColumn = new DataGridViewTextBoxColumn();
            nameColumn.HeaderText = "Название";
            nameColumn.DataPropertyName = "Название";
            AssortmentTable.Columns.Add(nameColumn);



            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
            imageColumn.HeaderText = "Изображение";
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            imageColumn.DataPropertyName = "Изображение_игры";
            AssortmentTable.Columns.Add(imageColumn);

            DataGridViewColumn CountColumn = new DataGridViewTextBoxColumn();
            CountColumn.HeaderText = "Количество";
            CountColumn.DataPropertyName = "Количество";
            AssortmentTable.Columns.Add(CountColumn);

            DataGridViewColumn AvalaibleColumn = new DataGridViewTextBoxColumn();
            AvalaibleColumn.HeaderText = "Наличие";
            AvalaibleColumn.DataPropertyName = "Наличие";
            AssortmentTable.Columns.Add(AvalaibleColumn);

            DataGridViewColumn PriceColumn = new DataGridViewTextBoxColumn();
            PriceColumn.HeaderText = "Цена";
            PriceColumn.DataPropertyName = "Цена";
            AssortmentTable.Columns.Add(PriceColumn);



            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                // Получаем данные из строки
                string Assortment = row["Номер_ассортимента"].ToString();
                string name = row["Название"].ToString();
                string imagepath = row["Изображение_игры"].ToString();
                Image image = Image.FromFile(imagepath);
                string Count = row["Количество"].ToString();
                string Avalaible = row["Наличие"].ToString();
                string Price = row["Цена"].ToString();

                // Остальные атрибуты...
                // Загружаем изображение из пути


                // Создаем строку и заполняем ее данными
                DataGridViewRow dataGridViewRow = new DataGridViewRow();
                DataGridViewColumn dataGridViewColumn = new DataGridViewColumn();

                dataGridViewRow.CreateCells(AssortmentTable);

                dataGridViewRow.Cells[0].Value = Assortment;
                dataGridViewRow.Cells[1].Value = name;
                dataGridViewRow.Cells[2].Value = image;
                dataGridViewRow.Cells[3].Value = Count;
                dataGridViewRow.Cells[4].Value = Avalaible;
                dataGridViewRow.Cells[5].Value = Price;

                dataGridViewRow.Height = 150;
                dataGridViewColumn.Width = 150;
                AssortmentTable.Rows.Add(dataGridViewRow);

            }




            sqlConnect.Close();// Закрываем соединение   

        }

       

        private void GoInsertAssortment_Click(object sender, EventArgs e)
        {
            InsertGameWindowShow();
        }

        private void GoDeleteAssortment_Click(object sender, EventArgs e)
        {
            DeleteWindowShop();
        }

        private void GoAssortmentUpdate_Click(object sender, EventArgs e)
        {
            UpdateWindowShop();
        }

        private void InsertAssortment_Click(object sender, EventArgs e)
        {
            GameInsert();
        }

        public void GameInsert()
        {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            string selectedGame = GameName.Text;

            SqlCommand FindGameID = new SqlCommand("SELECT Настольная_игра.Артикул FROM [dbo].[Настольная_игра] WHERE Название ='"+ selectedGame + "'", sqlConnect);

            // Создайте команду SQL с параметрами
int GameID = Convert.ToInt32(FindGameID.ExecuteScalar());
           

            // Выполните запрос и получите номер ассортимента
            
            SqlCommand assortmentQuery = new SqlCommand("SELECT MAX(Ассортимент.Номер_ассортимента) FROM [dbo].[Ассортимент]", sqlConnect);
            int AssortmentID = Convert.ToInt32(assortmentQuery.ExecuteScalar())+1;


            SqlCommand AssortmentInsert = new SqlCommand("INSERT INTO [dbo].[Ассортимент] (Номер_ассортимента, Магазин, Настольная_игра, Цена, Количество, наличие) " +
                "VALUES('"+AssortmentID +"', '"+ShopID+"','"+GameID+"','"+InsertPrice.Text+"','"+InsertCount.Text+"','"+InsertAvalaible.Text+"')", sqlConnect);

            // Измените картинку в PictureBox, связанную с выбранной игрой
            AssortmentInsert.ExecuteNonQuery();
            sqlConnect.Close();
            MessageBox.Show("Игра добавлена в ассортимент", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);


        }

 
        private void просмотрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reports.Visible = false;
            NewOrders.Visible = false;
            DeiiveryDetails.Visible = false;
            ReportDetails.Visible = false;
            OrderDetails.Visible = false;
            Deliveries.Visible = false;
            ProcessedOrders.Visible = false;
            AssortmentInserting.Visible = false;
            AssortmentDeleting.Visible = false;
            AssortmentUpdating.Visible = false;
            AssortmentTable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            AssortmentData.Size = new Size(1000, 600);
            AssortmentData.Location = new Point(20, 30);
            AssortmentData.Visible = true;
            GameDataSelect();
        }

        private void редактированиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateWindowShop();
        }

        private void доБToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InsertGameWindowShow();
        }

        private void удалениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteWindowShop();
        }

 public void InsertGameWindowShow()
        {
            Reports.Visible = false;
            NewOrders.Visible = false;
            DeiiveryDetails.Visible = false;
            ReportDetails.Visible = false;
            OrderDetails.Visible = false;
            Deliveries.Visible = false;
            ProcessedOrders.Visible = false;
            AssortmentData.Visible = false;
            AssortmentDeleting.Visible = false;
            ShopProfile.Visible = false;

            AssortmentUpdating.Visible = false;
            AssortmentInserting.Size = new Size(500, 490);
            AssortmentInserting.Location = new Point(20, 30);
            AssortmentInserting.Visible = true;
            FillNewAssortment();
        }

        public void DeleteWindowShop()
        {
            Reports.Visible = false;
            NewOrders.Visible = false;
            DeiiveryDetails.Visible = false;
            ReportDetails.Visible = false;
            OrderDetails.Visible = false;
            Deliveries.Visible = false;
            ProcessedOrders.Visible = false;
            AssortmentData.Visible = false;
            AssortmentInserting.Visible = false;
            AssortmentUpdating.Visible = false;
            ShopProfile.Visible = false;
            AssortmentDeleting.Visible = true;
            AssortmentDeleting.Location = new Point(20, 30);
            AssortmentDeleting.Size = new Size(500, 150);
            FillDeleteAssortment();
        }

        public void UpdateWindowShop()
        {
            Reports.Visible = false;
            NewOrders.Visible = false;
            DeiiveryDetails.Visible = false;
            ReportDetails.Visible = false;
            OrderDetails.Visible = false;
            Deliveries.Visible = false;
            ProcessedOrders.Visible = false;
            AssortmentData.Visible = false;
            AssortmentInserting.Visible = false;
            ShopProfile.Visible = false;

            AssortmentDeleting.Visible = false;
            AssortmentUpdating.Visible = true;
            AssortmentUpdating.Location = new Point(20, 30);
            AssortmentUpdating.Size = new Size(500, 500);
            FillUpdateAssortment();
        }

        private void профильToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {

            Reports.Visible = false;
            NewOrders.Visible = false;
            DeiiveryDetails.Visible = false;
            ReportDetails.Visible = false;
            OrderDetails.Visible = false;
            Deliveries.Visible = false;
            ProcessedOrders.Visible = false;
            AssortmentData.Visible = false;
            AssortmentInserting.Visible = false;
            AssortmentDeleting.Visible = false;
            AssortmentUpdating.Visible = false;
            ShopProfile.Location = new Point(20, 30);
            ShopProfile.Size = new Size(400, 400);
            ShopProfile.Visible = true;
        }

        private void DeleteGame_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedGame = DeleteGame.SelectedValue.ToString();

            using (SqlConnection sqlConnect = new SqlConnection(connection))
            {
                sqlConnect.Open();

                // Получите номер ассортимента выбранной игры
                SqlCommand assortmentQuery = new SqlCommand("SELECT Настольная_игра.Артикул FROM [dbo].[Настольная_игра] WHERE Название = @GameName", sqlConnect);

                // Создайте команду SQL с параметрами

                assortmentQuery.Parameters.AddWithValue("@GameName", selectedGame);
                assortmentQuery.Parameters.AddWithValue("@ShopID", ShopID);

                // Выполните запрос и получите номер ассортимента
                int assortmentNumber = Convert.ToInt32(assortmentQuery.ExecuteScalar());

                sqlConnect.Close();

            }

        }

        private void DeleteAssortment_Click(object sender, EventArgs e)
        {
            Delete();

        }

        private void Update_Click(object sender, EventArgs e)
        {
            GameUpdate();
        }

        private void UpdateGame_SelectedIndexChanged(object sender, EventArgs e)
        {

            string selectedGame = UpdateGame.SelectedValue.ToString();

            using (SqlConnection sqlConnect = new SqlConnection(connection))
            {
                sqlConnect.Open();

                // Получите номер ассортимента выбранной игры
                SqlCommand assortmentQuery = new SqlCommand("SELECT Настольная_игра.Артикул FROM [dbo].[Настольная_игра] WHERE Название = @GameName", sqlConnect);

                // Создайте команду SQL с параметрами

                assortmentQuery.Parameters.AddWithValue("@GameName", selectedGame);
                assortmentQuery.Parameters.AddWithValue("@ShopID", ShopID);

                
                int assortmentNumber = Convert.ToInt32(assortmentQuery.ExecuteScalar());// Выполните запрос и получите артикул

              
                SqlCommand sqlView = new SqlCommand("SELECT Настольная_игра.Изображение_игры, Ассортимент.Количество, Ассортимент.Наличие, Ассортимент.Цена FROM [dbo].[Настольная_игра] " +
                    "INNER JOIN [dbo].[Ассортимент] ON Ассортимент.Настольная_игра=Настольная_игра.Артикул " +
                    "WHERE Настольная_игра.Название = @GameName AND Ассортимент.Магазин='"+ShopID+"' ", sqlConnect);
                sqlView.Parameters.AddWithValue("@GameName", selectedGame);


                using (SqlDataReader sqlReader = sqlView.ExecuteReader())
                {
                    if (sqlReader.Read())
                    {
                        string picture = sqlReader["Изображение_игры"].ToString();
                        UpdatePicture.Image = new Bitmap(picture);
                        UpdateCount.Text = sqlReader["Количество"].ToString();
                        UpdateAvailable.Text = sqlReader["Наличие"].ToString();
                        UpdatePrice.Text = sqlReader["Цена"].ToString();
                    }
                }
              

                sqlConnect.Close();

            }
        }


        public void ShowOrders()
       {
            // Очистить контейнер для отображения данных корзины (например, DataGridView)
            OrdersData.Rows.Clear();

            // Создайть необходимые столбцы в контейнере
            OrdersData.Columns.Clear();
            // Создаем столбцы
            DataGridViewColumn IdColumn = new DataGridViewTextBoxColumn();
            IdColumn.HeaderText = "Номер заказа";
            IdColumn.DataPropertyName = "Номер_заказа";
            OrdersData.Columns.Add(IdColumn);


          
            // Создаем столбец для отображения картинок
            DataGridViewColumn NameColumn = new DataGridViewTextBoxColumn();
            NameColumn.HeaderText = "ФИО получателя";
            NameColumn.DataPropertyName = "ФИО";
            OrdersData.Columns.Add(NameColumn);

            DataGridViewColumn SumColumn = new DataGridViewTextBoxColumn();
            SumColumn.HeaderText = "Сумма оплаты";
            SumColumn.DataPropertyName = "Сумаа_оплаты";
            OrdersData.Columns.Add(SumColumn);

            DataGridViewColumn WayOfPaymentColumn = new DataGridViewTextBoxColumn();
            WayOfPaymentColumn.HeaderText = "Способ_оплаты";
            WayOfPaymentColumn.DataPropertyName = "Способ оплаты";
            OrdersData.Columns.Add(WayOfPaymentColumn);

            DataGridViewColumn WayOfGettingColumn = new DataGridViewTextBoxColumn();
            WayOfGettingColumn.HeaderText = "Способ_получения";
            WayOfGettingColumn.DataPropertyName = "Способ получения";
            OrdersData.Columns.Add(WayOfGettingColumn);

            DataGridViewColumn StreetColumn = new DataGridViewTextBoxColumn();
            StreetColumn.HeaderText = "Адрес";
            StreetColumn.DataPropertyName = "Адрес";
            OrdersData.Columns.Add(StreetColumn);

            DataGridViewColumn DateColumn = new DataGridViewTextBoxColumn();
            DateColumn.HeaderText = "Дата заказа";
            DateColumn.DataPropertyName = "Дата_заказа";
            OrdersData.Columns.Add(DateColumn);
            // Создаем столбец "Добавить
            DataGridViewColumn StatusColumn = new DataGridViewTextBoxColumn();
            StatusColumn.HeaderText = "Статус заказа";
            StatusColumn.DataPropertyName = "Статус_заказа";
            OrdersData.Columns.Add(StatusColumn);

            // Создаем столбец "Уменьшить"
            DataGridViewButtonColumn AcceptColumn = new DataGridViewButtonColumn();
            AcceptColumn.HeaderText = "Принять";
            AcceptColumn.Text = "Принять";
            AcceptColumn.UseColumnTextForButtonValue = true;
            OrdersData.Columns.Add(AcceptColumn);

            // Создаем столбец "Уменьшить"
            DataGridViewButtonColumn DeclineColumn = new DataGridViewButtonColumn();
            DeclineColumn.HeaderText = "Отклонить";
            DeclineColumn.Text = "Отклонить";
            DeclineColumn.UseColumnTextForButtonValue = true;
            OrdersData.Columns.Add(DeclineColumn);


            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            SqlCommand OrderList = new SqlCommand("SELECT Заказ.Номер_заказа AS [Номер заказа], CONCAT(Покупатель.Фамилия, ' ', Покупатель.Имя, ' ', Покупатель.Отчество) AS [ФИО], Заказ.Сумма_оплаты AS [Сумма оплаты], Способ_оплаты.Название_способа_оплаты AS [Способ оплаты]," +
                " Способ_получения.Название_способа_получения AS [Способ получения], CONCAT(Пункт_назначения.Улица, ', ', Пункт_назначения.Дом) AS [Адрес], Заказ.Дата_заказа AS [Дата заказа], Статус_заказа.Название_статуса AS [Статус заказа] FROM Заказ" +
                " INNER JOIN Покупатель ON Покупатель.ID_покупателя=Заказ.Заказчик " +
                " INNER JOIN Способ_оплаты ON Способ_оплаты.Код_способа_оплаты=Заказ.Способ_оплаты " +
                " INNER JOIN Способ_получения ON Способ_получения.Код_способа_получения=Заказ.Способ_получения" +
                " INNER JOIN Пункт_назначения ON Пункт_назначения.Код_пункта_назначения=Заказ.Адрес_получения" +
                " INNER JOIN Покупка ON Покупка.Заказ=Заказ.Номер_заказа " +
                " INNER JOIN Ассортимент ON Покупка.Ассортимент=Ассортимент.Номер_ассортимента" +
                " INNER JOIN Статус_заказа ON Статус_заказа.ID_Cтатуса=Заказ.Статус_заказа" +
                " WHERE Заказ.Статус_заказа = 1 AND Ассортимент.Магазин="+ShopID+" " +
                " GROUP BY  Заказ.Номер_заказа, Покупатель.Фамилия, Покупатель.Имя, Покупатель.Отчество, Заказ.Сумма_оплаты,  " +
                " Способ_оплаты.Название_способа_оплаты, Способ_получения.Название_способа_получения, " +
                " Пункт_назначения.Улица, Пункт_назначения.Дом, Заказ.Дата_заказа, Статус_заказа.Название_статуса", sqlConnect);
            //sqlCmd.Parameters.AddWithValue("@GameID", GameID);
            OrdersData.AllowUserToAddRows = false;

            // Создаем адаптер данных для заполнения DataSet
            SqlDataAdapter adapter = new SqlDataAdapter(OrderList);

            // Создаем DataSet для хранения данных
            DataSet dataSet = new DataSet();

            // Заполняем DataSet данными из таблицы
            adapter.Fill(dataSet);

            OrdersData.AutoGenerateColumns = false;

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                // Получаем данные из строки

                string ID = row["Номер заказа"].ToString();
                string FullName = row["ФИО"].ToString();
                string Sum = row["Сумма оплаты"].ToString();
                string WayOfPaymentName = row["Способ оплаты"].ToString();
                string WayOfGettingtName = row["Способ получения"].ToString();
                string Address = row["Адрес"].ToString();
                string Date = row["Дата заказа"].ToString();
                string Status = row["Статус заказа"].ToString();

            
                // Создаем строку и заполняем ее данными
                DataGridViewRow dataGridViewRow = new DataGridViewRow();
                DataGridViewColumn dataGridViewColumn = new DataGridViewColumn();

                dataGridViewRow.CreateCells(OrdersData);


                dataGridViewRow.Cells[0].Value = ID;
                dataGridViewRow.Cells[1].Value = FullName;
                dataGridViewRow.Cells[2].Value = Sum;
                dataGridViewRow.Cells[3].Value = WayOfPaymentName;
                dataGridViewRow.Cells[4].Value = WayOfGettingtName;
                dataGridViewRow.Cells[5].Value = Address;
                dataGridViewRow.Cells[6].Value = Date;
                dataGridViewRow.Cells[7].Value = Status;


                DataGridViewButtonCell AcceptButtonCell = new DataGridViewButtonCell();
                AcceptButtonCell.Value = "Принять";
                AcceptButtonCell.FlatStyle = FlatStyle.Flat;
                AcceptButtonCell.Style.BackColor = Color.Green;
                dataGridViewRow.Cells[8] = AcceptButtonCell;
                DataGridViewButtonCell DeclineButtonCell = new DataGridViewButtonCell();
                DeclineButtonCell.Value = "Отклонить";
                DeclineButtonCell.FlatStyle = FlatStyle.Flat;
                DeclineButtonCell.Style.BackColor = Color.Red;
                dataGridViewRow.Cells[9] = DeclineButtonCell;
              


                dataGridViewColumn.Width = 150;
                // Остальные значения...

                // Добавляем строку в таблицу
                OrdersData.Rows.Add(dataGridViewRow);

            }

        }


        private void Button_CLick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow selectedRow = OrdersData.Rows[e.RowIndex];
                int orderNumber = Convert.ToInt32(selectedRow.Cells[0].Value);
                SqlConnection sqlConnect = new SqlConnection(connection);
                sqlConnect.Open();


                if (e.ColumnIndex == 8) // Клик на кнопке "принять"
                {

                    // Проверяем, есть ли товары, связанные с заказом
                    SqlCommand checkProductCountCommand = new SqlCommand("SELECT COUNT(*) FROM Покупка WHERE Заказ = @OrderNumber", sqlConnect);
                    checkProductCountCommand.Parameters.AddWithValue("@OrderNumber", orderNumber);
                    int productCount = Convert.ToInt32(checkProductCountCommand.ExecuteScalar());

                    if (productCount > 0)
                    {
                        // Если есть товары, меняем статус заказа на 2
                        SqlCommand updateStatusCommand = new SqlCommand("UPDATE Заказ SET Статус_заказа = 2 WHERE Номер_заказа = @OrderNumber", sqlConnect);
                        updateStatusCommand.Parameters.AddWithValue("@OrderNumber", orderNumber);
                        updateStatusCommand.ExecuteNonQuery();
                        // Логика принятия заказа (изменение статуса на 2)

                        SqlCommand GetMaxDeliveryID = new SqlCommand("SELECT MAX(Номер_отслеживания) FROM Доставка ", sqlConnect);
                        object result5 = GetMaxDeliveryID.ExecuteScalar();
                        int DeliveryID = Convert.ToInt32(result5) + 1;// Получаем последний номер покупки из таблицы "Доставка"
                        SqlCommand InsertDelivery = new SqlCommand("INSERT INTO Доставка (Номер_отслеживания, Заказ, Дата_доставки, Отметка_о_выполнении, Претензии) " +
                                 "VALUES ('" + DeliveryID + "',  " + orderNumber + ", NULL, '" + 0 + "', NULL) ", sqlConnect);
                        InsertDelivery.ExecuteNonQuery();

                    }


                    OrdersData.Rows.Remove(selectedRow);
                }
                else if (e.ColumnIndex == 9) // Клик на кнопке "Отклонить"
                {
                    // Логика отклонения заказа (удаление ассортиментов из таблицы Покупка)
                    // Логика отклонения заказа (удаление ассортиментов из таблицы Покупка)
                    SqlCommand deleteAssortmentCommand = new SqlCommand("DELETE FROM Покупка WHERE Заказ = @OrderNumber AND Ассортимент IN (SELECT Номер_ассортимента FROM Ассортимент WHERE Магазин = @ShopID)", sqlConnect);
                    deleteAssortmentCommand.Parameters.AddWithValue("@OrderNumber", orderNumber);
                    deleteAssortmentCommand.Parameters.AddWithValue("@ShopID", ShopID);
                    deleteAssortmentCommand.ExecuteNonQuery();

                    // Получаем количество товаров после удаления
                    SqlCommand updatedQuantityCommand = new SqlCommand("SELECT COUNT(*) FROM Покупка WHERE Заказ = @OrderNumber", sqlConnect);
                    updatedQuantityCommand.Parameters.AddWithValue("@OrderNumber", orderNumber);
                    int updatedQuantity = Convert.ToInt32(updatedQuantityCommand.ExecuteScalar());

                    // Если количество товаров стало равным 0, изменяем статус заказа на 4
                    if (updatedQuantity == 0)
                    {
                        SqlCommand updateStatusCommand = new SqlCommand("UPDATE Заказ SET Статус_заказа = 4 WHERE Номер_заказа = @OrderNumber", sqlConnect);
                        updateStatusCommand.Parameters.AddWithValue("@OrderNumber", orderNumber);
                        updateStatusCommand.ExecuteNonQuery();
                    }
                    OrdersData.Rows.Remove(selectedRow);

                }


            }


        }

        public void ShowOrderDetails(int orderNumber)
        {
            NewOrders.Visible = false;
            ProcessedOrders.Visible = false;
            OrderDetails.Visible = true;
            OrderDetails.Size = new Size(1200,420);
            OrderDetails.Location=new Point(20,30);


            // Очистить контейнер для отображения данных корзины (например, DataGridView)
            PurchaseList.Rows.Clear();

            // Создайть необходимые столбцы в контейнере
            PurchaseList.Columns.Clear();
            // Создаем столбцы
            DataGridViewColumn IdColumn = new DataGridViewTextBoxColumn();
            IdColumn.HeaderText = "Товар";
            IdColumn.DataPropertyName = "Номер_ассортимента";
            PurchaseList.Columns.Add(IdColumn);

            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
            imageColumn.HeaderText = "Изображение";
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            imageColumn.DataPropertyName = "Изображение_игры";
            PurchaseList.Columns.Add(imageColumn);

            // Создаем столбец для отображения картинок
            DataGridViewColumn NameColumn = new DataGridViewTextBoxColumn();
            NameColumn.HeaderText = "Название";
            NameColumn.DataPropertyName = "Настольная игра";
            PurchaseList.Columns.Add(NameColumn);
            // Создаем столбец для отображения картинок
            DataGridViewColumn QuantityColumn = new DataGridViewTextBoxColumn();
            QuantityColumn.HeaderText = "Количество";
            QuantityColumn.DataPropertyName = "Количество";
            PurchaseList.Columns.Add(QuantityColumn);

            DataGridViewColumn WayOfPaymentColumn = new DataGridViewTextBoxColumn();
            WayOfPaymentColumn.HeaderText = "Цена";
            WayOfPaymentColumn.DataPropertyName = "Цена_покупки";
            PurchaseList.Columns.Add(WayOfPaymentColumn);

          


            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            SqlCommand getOrderDetails = new SqlCommand("SELECT Заказ.Номер_заказа AS [Номер заказа], CONCAT(Покупатель.Фамилия, ' ', Покупатель.Имя, ' ', Покупатель.Отчество) AS [ФИО], " +
                "Способ_оплаты.Название_способа_оплаты AS [Способ оплаты], Способ_получения.Название_способа_получения AS [Способ получения], " +
                "CONCAT(Пункт_назначения.Улица, ', ', Пункт_назначения.Дом) AS [Адрес], Заказ.Дата_заказа AS [Дата заказа], " +
                "Статус_заказа.Название_статуса AS [Статус заказа], SUM(Ассортимент.Цена) AS [Сумма покупки]  FROM Заказ" +
                " INNER JOIN Покупатель ON Покупатель.ID_покупателя=Заказ.Заказчик " +
                " INNER JOIN Способ_оплаты ON Способ_оплаты.Код_способа_оплаты=Заказ.Способ_оплаты " +
                " INNER JOIN Способ_получения ON Способ_получения.Код_способа_получения=Заказ.Способ_получения" +
                " INNER JOIN Пункт_назначения ON Пункт_назначения.Код_пункта_назначения=Заказ.Адрес_получения" +
                " INNER JOIN Покупка ON Покупка.Заказ=Заказ.Номер_заказа " +
                " INNER JOIN Ассортимент ON Покупка.Ассортимент=Ассортимент.Номер_ассортимента" +
                " INNER JOIN Статус_заказа ON Статус_заказа.ID_Cтатуса=Заказ.Статус_заказа" +
                " WHERE Ассортимент.Магазин=" + ShopID + " AND Заказ.Номер_заказа= "+orderNumber+" " +
                " GROUP BY  Заказ.Номер_заказа, Покупатель.Фамилия, Покупатель.Имя, Покупатель.Отчество, " +
                " Способ_оплаты.Название_способа_оплаты, Способ_получения.Название_способа_получения, " +
                " Пункт_назначения.Улица, Пункт_назначения.Дом, Заказ.Дата_заказа, Статус_заказа.Название_статуса", sqlConnect);
            //sqlCmd.Parameters.AddWithValue("@GameID", GameID);
           

            using (SqlDataReader sqlReader = getOrderDetails.ExecuteReader())
            {
                if (sqlReader.Read())
                {
                    OrderIDLabel.Text = orderNumber.ToString();
                    OrderStatusLabel.Text = sqlReader["Статус заказа"].ToString();
                    OrderDateLabel.Text = sqlReader["Дата заказа"].ToString();
                    PaymentLabel.Text = sqlReader["Сумма покупки"].ToString() + "₽";
                    FullNameBox.Text = sqlReader["ФИО"].ToString();
                    AddressBox.Text = sqlReader["Адрес"].ToString();
                    WayOfPaymentBox.Text = sqlReader["Способ оплаты"].ToString();
                    WayOfGettingBox.Text = sqlReader["Способ получения"].ToString();
                }
            }


            SqlCommand getPurchaseList = new SqlCommand("SELECT Покупка.Ассортимент, Настольная_игра.Изображение_игры, Настольная_игра.Название, Покупка.Цена_покупки, COUNT(*) AS [Количество] FROM Покупка " +
                " INNER JOIN Ассортимент ON Ассортимент.Номер_ассортимента=Покупка.Ассортимент " +
                " INNER JOIN Настольная_игра ON Ассортимент.Настольная_игра=Настольная_игра.Артикул WHERE Покупка.Заказ="+orderNumber+" AND Ассортимент.Магазин= "+ShopID+" " +
                "GROUP BY Покупка.Ассортимент, Настольная_игра.Изображение_игры, Настольная_игра.Название, Покупка.Цена_покупки", sqlConnect);

            PurchaseList.AllowUserToAddRows = false;
            // Создаем адаптер данных для заполнения DataSet
            SqlDataAdapter adapter = new SqlDataAdapter(getPurchaseList);

            // Создаем DataSet для хранения данных
            DataSet dataSet = new DataSet();

            // Заполняем DataSet данными из таблицы
            adapter.Fill(dataSet);

            PurchaseList.AutoGenerateColumns = false;

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                // Получаем данные из строки

                string ID = row["Ассортимент"].ToString();
                string imagePath = row["Изображение_игры"].ToString();
                Image image = Image.FromFile(imagePath);

                string Name = row["Название"].ToString();
                string quantity = row["Количество"].ToString();

                string Price = row["Цена_покупки"].ToString();
              



                // Создаем строку и заполняем ее данными
                DataGridViewRow dataGridViewRow = new DataGridViewRow();
                DataGridViewColumn dataGridViewColumn = new DataGridViewColumn();

                dataGridViewRow.CreateCells(PurchaseList);


                dataGridViewRow.Cells[0].Value = ID;
                dataGridViewRow.Cells[1].Value = image;
                dataGridViewRow.Cells[2].Value = Name;
                dataGridViewRow.Cells[3].Value = quantity;
                dataGridViewRow.Cells[4].Value = Price;
              




                dataGridViewColumn.Width = 150;
                // Остальные значения...
                dataGridViewRow.Height = 150;

                // Добавляем строку в таблицу
                PurchaseList.Rows.Add(dataGridViewRow);

            }

        }

        // Обработчик нажатия на ячейку
        private void OrdersData_CellClick(object sender, DataGridViewCellEventArgs e)
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && !(OrdersData.Columns[e.ColumnIndex] is DataGridViewButtonColumn)) // Проверяем, что нажатие не произошло на кнопке
                {
                    DataGridViewRow selectedRow = OrdersData.Rows[e.RowIndex];
                    int orderNumber = Convert.ToInt32(selectedRow.Cells[0].Value);
                // Вызов вашего метода для обработки нажатия на ячейку
                // Например:
                 ShowOrderDetails(orderNumber);
            }

            }

        private void BackToProcessingOrders_Click(object sender, EventArgs e)
        {
            OrderDetails.Visible = false;
            ShowOrders();
            NewOrders.Visible = true;
            NewOrders.Size = new Size(1250, 600);
            NewOrders.Location = new Point(20, 30);
        }

        public void ShowOrderDeliveries()
        {
            // Очистить контейнер для отображения данных корзины (например, DataGridView)
            OrderDeliveryList.Rows.Clear();

            // Создайть необходимые столбцы в контейнере
            OrderDeliveryList.Columns.Clear();
            // Создаем столбцы
            DataGridViewColumn IdColumn = new DataGridViewTextBoxColumn();
            IdColumn.HeaderText = "Номер заказа";
            IdColumn.DataPropertyName = "Номер_заказа";
            OrderDeliveryList.Columns.Add(IdColumn);



            // Создаем столбец для отображения картинок
            DataGridViewColumn NameColumn = new DataGridViewTextBoxColumn();
            NameColumn.HeaderText = "ФИО получателя";
            NameColumn.DataPropertyName = "ФИО";
            OrderDeliveryList.Columns.Add(NameColumn);

            DataGridViewColumn SumColumn = new DataGridViewTextBoxColumn();
            SumColumn.HeaderText = "Сумма оплаты";
            SumColumn.DataPropertyName = "Сумаа_оплаты";
            OrderDeliveryList.Columns.Add(SumColumn);

            DataGridViewColumn WayOfPaymentColumn = new DataGridViewTextBoxColumn();
            WayOfPaymentColumn.HeaderText = "Способ_оплаты";
            WayOfPaymentColumn.DataPropertyName = "Способ оплаты";
            OrderDeliveryList.Columns.Add(WayOfPaymentColumn);

            DataGridViewColumn WayOfGettingColumn = new DataGridViewTextBoxColumn();
            WayOfGettingColumn.HeaderText = "Способ_получения";
            WayOfGettingColumn.DataPropertyName = "Способ получения";
            OrderDeliveryList.Columns.Add(WayOfGettingColumn);

            DataGridViewColumn StreetColumn = new DataGridViewTextBoxColumn();
            StreetColumn.HeaderText = "Адрес";
            StreetColumn.DataPropertyName = "Адрес";
            OrderDeliveryList.Columns.Add(StreetColumn);

            DataGridViewColumn DateColumn = new DataGridViewTextBoxColumn();
            DateColumn.HeaderText = "Дата заказа";
            DateColumn.DataPropertyName = "Дата_заказа";
            OrderDeliveryList.Columns.Add(DateColumn);
            // Создаем столбец "Добавить
            DataGridViewColumn StatusColumn = new DataGridViewTextBoxColumn();
            StatusColumn.HeaderText = "Статус заказа";
            StatusColumn.DataPropertyName = "Статус_заказа";
            OrderDeliveryList.Columns.Add(StatusColumn);

            // Создаем столбец "Уменьшить"
            DataGridViewButtonColumn CompleteDeliveryColumn = new DataGridViewButtonColumn();
            CompleteDeliveryColumn.HeaderText = "Решение по заказу";
            CompleteDeliveryColumn.Text = "Доставлен";
            CompleteDeliveryColumn.UseColumnTextForButtonValue = true;
            OrderDeliveryList.Columns.Add(CompleteDeliveryColumn);



            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            SqlCommand OrderList = new SqlCommand("SELECT Заказ.Номер_заказа AS [Номер заказа], CONCAT(Покупатель.Фамилия, ' ', Покупатель.Имя, ' ', Покупатель.Отчество) AS [ФИО], Заказ.Сумма_оплаты AS [Сумма оплаты], Способ_оплаты.Название_способа_оплаты AS [Способ оплаты]," +
                " Способ_получения.Название_способа_получения AS [Способ получения], CONCAT(Пункт_назначения.Улица, ', ', Пункт_назначения.Дом) AS [Адрес], Заказ.Дата_заказа AS [Дата заказа], Статус_заказа.Название_статуса AS [Статус заказа] FROM Заказ" +
                " INNER JOIN Покупатель ON Покупатель.ID_покупателя=Заказ.Заказчик " +
                " INNER JOIN Способ_оплаты ON Способ_оплаты.Код_способа_оплаты=Заказ.Способ_оплаты " +
                " INNER JOIN Способ_получения ON Способ_получения.Код_способа_получения=Заказ.Способ_получения" +
                " INNER JOIN Пункт_назначения ON Пункт_назначения.Код_пункта_назначения=Заказ.Адрес_получения" +
                " INNER JOIN Покупка ON Покупка.Заказ=Заказ.Номер_заказа " +
                " INNER JOIN Ассортимент ON Покупка.Ассортимент=Ассортимент.Номер_ассортимента" +
                " INNER JOIN Статус_заказа ON Статус_заказа.ID_Cтатуса=Заказ.Статус_заказа" +
                " WHERE Заказ.Статус_заказа = 2 AND Ассортимент.Магазин=" + ShopID + " " +
                " GROUP BY  Заказ.Номер_заказа, Покупатель.Фамилия, Покупатель.Имя, Покупатель.Отчество, Заказ.Сумма_оплаты,  " +
                " Способ_оплаты.Название_способа_оплаты, Способ_получения.Название_способа_получения, " +
                " Пункт_назначения.Улица, Пункт_назначения.Дом, Заказ.Дата_заказа, Статус_заказа.Название_статуса", sqlConnect);
            //sqlCmd.Parameters.AddWithValue("@GameID", GameID);
            OrderDeliveryList.AllowUserToAddRows = false;

            // Создаем адаптер данных для заполнения DataSet
            SqlDataAdapter adapter = new SqlDataAdapter(OrderList);

            // Создаем DataSet для хранения данных
            DataSet dataSet = new DataSet();

            // Заполняем DataSet данными из таблицы
            adapter.Fill(dataSet);

            OrderDeliveryList.AutoGenerateColumns = false;

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                // Получаем данные из строки

                string ID = row["Номер заказа"].ToString();
                string FullName = row["ФИО"].ToString();
                string Sum = row["Сумма оплаты"].ToString();
                string WayOfPaymentName = row["Способ оплаты"].ToString();
                string WayOfGettingtName = row["Способ получения"].ToString();
                string Address = row["Адрес"].ToString();
                string Date = row["Дата заказа"].ToString();
                string Status = row["Статус заказа"].ToString();


                // Создаем строку и заполняем ее данными
                DataGridViewRow dataGridViewRow = new DataGridViewRow();
                DataGridViewColumn dataGridViewColumn = new DataGridViewColumn();

                dataGridViewRow.CreateCells(OrderDeliveryList);


                dataGridViewRow.Cells[0].Value = ID;
                dataGridViewRow.Cells[1].Value = FullName;
                dataGridViewRow.Cells[2].Value = Sum;
                dataGridViewRow.Cells[3].Value = WayOfPaymentName;
                dataGridViewRow.Cells[4].Value = WayOfGettingtName;
                dataGridViewRow.Cells[5].Value = Address;
                dataGridViewRow.Cells[6].Value = Date;
                dataGridViewRow.Cells[7].Value = Status;


                DataGridViewButtonCell CompleteButtonCell = new DataGridViewButtonCell();
                CompleteButtonCell.Value = "Отметить выполнение заказа";
                CompleteButtonCell.FlatStyle = FlatStyle.Flat;
                CompleteButtonCell.Style.BackColor = Color.Green;
                dataGridViewRow.Cells[8].Value = CompleteButtonCell;
            



                dataGridViewColumn.Width = 150;
                // Остальные значения...

                // Добавляем строку в таблицу
                OrderDeliveryList.Rows.Add(dataGridViewRow);

            }

        }

        // Обработчик нажатия на ячейку
        private void OrderDeliveryList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && !(OrderDeliveryList.Columns[e.ColumnIndex] is DataGridViewButtonColumn)) // Проверяем, что нажатие не произошло на кнопке
            {
                DataGridViewRow selectedRow = OrderDeliveryList.Rows[e.RowIndex];
                int orderNumber = Convert.ToInt32(selectedRow.Cells[0].Value);
                // Вызов вашего метода для обработки нажатия на ячейку
                // Например:
                ShowOrderDetails(orderNumber);
            }

        }

        private void OrderDeliveryList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 8)
            {
                DataGridViewRow selectedRow = OrderDeliveryList.Rows[e.RowIndex];
                int orderNumber = Convert.ToInt32(selectedRow.Cells[0].Value);

                // Обновление статуса заказа на 3 (доставлен) в базе данных
                SqlConnection sqlConnect = new SqlConnection(connection);
                sqlConnect.Open();
                SqlCommand updateStatusCommand = new SqlCommand("UPDATE Заказ SET Статус_заказа = 3, Дата_выдачи='"+DateTime.Now+"' WHERE Номер_заказа = @OrderNumber", sqlConnect);
                updateStatusCommand.Parameters.AddWithValue("@OrderNumber", orderNumber);
                updateStatusCommand.ExecuteNonQuery();

                SqlCommand updateDelivryCommand = new SqlCommand("UPDATE Доставка SET Доставка.Отметка_о_выполнении = "+1+", Доставка.Дата_доставки= '"+DateTime.Now+"' WHERE Заказ = @OrderNumber", sqlConnect);
                updateDelivryCommand.Parameters.AddWithValue("@OrderNumber", orderNumber);
                updateDelivryCommand.ExecuteNonQuery();
                sqlConnect.Close();


                OrderDeliveryList.Rows.Remove(selectedRow);

            }

        }


        private void обработанныеЗаказыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShopProfile.Visible = false;

            Reports.Visible = false;
            NewOrders.Visible = false;
            DeiiveryDetails.Visible = false;
            ReportDetails.Visible = false;
            OrderDetails.Visible = false;
            Deliveries.Visible = false;
            AssortmentData.Visible = false;
            AssortmentInserting.Visible = false;
            AssortmentDeleting.Visible = false;
            AssortmentUpdating.Visible = false;
            ProcessedOrders.Visible = true;
            ProcessedOrders.Size = new Size(1250, 300);
            ProcessedOrders.Location = new Point(20, 30);
            ShowOrderDeliveries();
        }

        private void жалобыПокупателеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShopProfile.Visible = false;

            Reports.Visible = false;
            NewOrders.Visible = false;
            DeiiveryDetails.Visible = false;
            ReportDetails.Visible = false;
            OrderDetails.Visible = false;
            ProcessedOrders.Visible = false;
            AssortmentData.Visible = false;
            AssortmentInserting.Visible = false;
            AssortmentDeleting.Visible = false;
            AssortmentUpdating.Visible = false;
            Deliveries.Visible = true;
            Deliveries.Size = new Size(1250, 300);
            Deliveries.Location = new Point(20, 30);
            DeliveryListShow();
        }

 

        private void заявленияНаВозвратToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShopProfile.Visible = false;

            NewOrders.Visible = false;
            DeiiveryDetails.Visible = false;
            ReportDetails.Visible = false;
            OrderDetails.Visible = false;
            Deliveries.Visible = false;
            ProcessedOrders.Visible = false;
            AssortmentData.Visible = false;
            AssortmentInserting.Visible = false;
            AssortmentDeleting.Visible = false;
            AssortmentUpdating.Visible = false;
            Reports.Visible = true;
            Reports.Size = new Size(1050, 300);
            Reports.Location = new Point(20, 30);
            ReportListShow();

        }

        public void ReportListShow()
        {
            // Очистить контейнер для отображения данных корзины (например, DataGridView)
            ReportList.Rows.Clear();

            // Создайть необходимые столбцы в контейнере
            ReportList.Columns.Clear();
            // Создаем столбцы
            DataGridViewColumn ReportIdColumn = new DataGridViewTextBoxColumn();
            ReportIdColumn.HeaderText = "Номер заявления";
            ReportIdColumn.DataPropertyName = "Номер_отслеживания";
            ReportList.Columns.Add(ReportIdColumn);

            DataGridViewColumn PurchaseIDColumn = new DataGridViewTextBoxColumn();
            PurchaseIDColumn.HeaderText = "Номер покупки";
            PurchaseIDColumn.DataPropertyName = "Номер_покупки";
            ReportList.Columns.Add(PurchaseIDColumn);



            // Создаем столбец для отображения картинок
            DataGridViewColumn NameColumn = new DataGridViewTextBoxColumn();
            NameColumn.HeaderText = "ФИО отправителя";
            NameColumn.DataPropertyName = "ФИО";
            ReportList.Columns.Add(NameColumn);


            DataGridViewColumn GameNameColumn = new DataGridViewTextBoxColumn();
            GameNameColumn.HeaderText = "Название";
            GameNameColumn.DataPropertyName = "Название";
            ReportList.Columns.Add(GameNameColumn);

            DataGridViewColumn DateColumn = new DataGridViewTextBoxColumn();
            DateColumn.HeaderText = "Дата отправки";
            DateColumn.DataPropertyName = "Дата_отправки_заявления";
            ReportList.Columns.Add(DateColumn);

            DataGridViewColumn SumColumn = new DataGridViewTextBoxColumn();
            SumColumn.HeaderText = "Сумма возврата";
            SumColumn.DataPropertyName = "Сумма_возврата";
            ReportList.Columns.Add(SumColumn);
           

            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            SqlCommand sqlCommand = new SqlCommand("SELECT Заявление_на_возврат.Номер_заявления AS [Номер заявления],  " +
                "Заявление_на_возврат.Покупка AS [Номер покупки], CONCAT(Покупатель.Фамилия, ' ', Покупатель.Имя, ' ', Покупатель.Отчество) AS [ФИО],  Настольная_игра.Название, " +
               " Заявление_на_возврат.Дата_отправки_заявления AS [Дата отправки]," +
                " Заявление_на_возврат.Сумма_возврата AS [Сумма возврата] FROM Заявление_на_возврат"+
                " INNER JOIN Покупка ON Покупка.Номер_покупки=Заявление_на_возврат.Покупка " +
                " INNER JOIN Ассортимент ON Покупка.Ассортимент=Ассортимент.Номер_ассортимента " +
                " INNER JOIN Настольная_игра ON Настольная_игра.Артикул=Ассортимент.Настольная_игра " +
                " INNER JOIN Заказ ON Покупка.Заказ=Заказ.Номер_заказа " +
                " INNER JOIN Покупатель ON Покупатель.ID_покупателя=Заказ.Заказчик" +
                " WHERE Заявление_на_возврат.Продавец="+ShopID+"", sqlConnect);
            //sqlCmd.Parameters.AddWithValue("@GameID", GameID);
            ReportList.AllowUserToAddRows = false;

            // Создаем адаптер данных для заполнения DataSet
            SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);

            // Создаем DataSet для хранения данных
            DataSet dataSet = new DataSet();

            // Заполняем DataSet данными из таблицы
            adapter.Fill(dataSet);

            ReportList.AutoGenerateColumns = false;

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                // Получаем данные из строки
                string RepoertID = row["Номер заявления"].ToString();
               
                string FullName = row["ФИО"].ToString();
 string PurchaseID = row["Номер покупки"].ToString();
                string GameName = row["Название"].ToString();
                string Date = row["Дата отправки"].ToString();
                string Sum = row["Сумма возврата"].ToString();


                // Создаем строку и заполняем ее данными
                DataGridViewRow dataGridViewRow = new DataGridViewRow();
                DataGridViewColumn dataGridViewColumn = new DataGridViewColumn();

                dataGridViewRow.CreateCells(ReportList);


                dataGridViewRow.Cells[0].Value = RepoertID;
                dataGridViewRow.Cells[1].Value = FullName;
                dataGridViewRow.Cells[2].Value = PurchaseID;
                dataGridViewRow.Cells[3].Value = GameName;
                dataGridViewRow.Cells[4].Value = Date;
                dataGridViewRow.Cells[5].Value = Sum;


                dataGridViewColumn.Width = 150;
                // Остальные значения...

                // Добавляем строку в таблицу
                ReportList.Rows.Add(dataGridViewRow);

            }
        }

        // Обработчик нажатия на ячейку
        private void ReportList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && !(ReportList.Columns[e.ColumnIndex] is DataGridViewButtonColumn)) // Проверяем, что нажатие не произошло на кнопке
            {
                DataGridViewRow selectedRow = ReportList.Rows[e.RowIndex];
                int ReportNumber = Convert.ToInt32(selectedRow.Cells[0].Value);
                // Вызов  метода для обработки нажатия на ячейку
                ShowReportDetails(ReportNumber);
            }

        }

        public void ShowReportDetails(int ReportNumber)
        {
            ShopProfile.Visible = false;
            AssortmentDeleting.Visible = false;
            AssortmentUpdating.Visible = false;
            AssortmentInserting.Visible = false;
            OrderDetails.Visible = false;
            NewOrders.Visible = false;
            ProcessedOrders.Visible = false;
            Deliveries.Visible = false;
            Reports.Visible = false;
            ReportDetails.Visible = true;
            ReportDetails.Size = new Size(960, 400);
            ReportDetails.Location = new Point(20, 30);


            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            SqlCommand getReportDetails = new SqlCommand("SELECT Заявление_на_возврат.Номер_заявления AS [Номер заявления],"+
                "Заявление_на_возврат.Покупка AS [Номер покупки], CONCAT(Покупатель.Фамилия, ' ', Покупатель.Имя, ' ', Покупатель.Отчество) AS [ФИО],  Настольная_игра.Название,"+
                " Заявление_на_возврат.Дата_отправки_заявления AS [Дата отправки],"+
                "Заявление_на_возврат.Сумма_возврата AS [Сумма возврата], Заявление_на_возврат.Причина_возврата FROM Заявление_на_возврат" +
                " INNER JOIN Покупка ON Покупка.Номер_покупки=Заявление_на_возврат.Покупка " +
                " INNER JOIN Ассортимент ON Покупка.Ассортимент=Ассортимент.Номер_ассортимента " +
                " INNER JOIN Настольная_игра ON Настольная_игра.Артикул=Ассортимент.Настольная_игра " +
                " INNER JOIN Заказ ON Покупка.Заказ=Заказ.Номер_заказа " +
                " INNER JOIN Покупатель ON Покупатель.ID_покупателя=Заказ.Заказчик " +
                " WHERE Заявление_на_возврат.Номер_заявления="+ReportNumber+" ", sqlConnect);
            //sqlCmd.Parameters.AddWithValue("@GameID", GameID);


            using (SqlDataReader sqlReader = getReportDetails.ExecuteReader())
            {
                if (sqlReader.Read())
                {
                    ReportIDLabel.Text = ReportNumber.ToString();
                    PurchaseIDLabel.Text= sqlReader["Номер покупки"].ToString();


                    ReportFullName.Text = sqlReader["ФИО"].ToString();
                    ReportGameName.Text = sqlReader["Название"].ToString();
                    ReportDate.Text = sqlReader["Дата отправки"].ToString();
                    ReportReason.Text= sqlReader["Причина_возврата"].ToString();
                  

                }
            }
        }

        public void DeliveryListShow()
        {
                // Очистить контейнер для отображения данных корзины (например, DataGridView)

                // Создайть необходимые столбцы в контейнере
                // Создаем столбцы
                DataGridViewColumn DeliveryIdColumn = new DataGridViewTextBoxColumn();
                DeliveryIdColumn.HeaderText = "Номер отслеживания";
                DeliveryIdColumn.DataPropertyName = "Номер_отслеживания";
                DeliveryList.Columns.Add(DeliveryIdColumn);

                DataGridViewColumn OrderIDColumn = new DataGridViewTextBoxColumn();
                OrderIDColumn.HeaderText = "Номер заказа";
                OrderIDColumn.DataPropertyName = "Номер_заказа";
                DeliveryList.Columns.Add(OrderIDColumn);

         

                // Создаем столбец для отображения картинок
                DataGridViewColumn NameColumn = new DataGridViewTextBoxColumn();
                NameColumn.HeaderText = "ФИО получателя";
                NameColumn.DataPropertyName = "ФИО";
            DeliveryList.Columns.Add(NameColumn);


                DataGridViewColumn WayOfGettingColumn = new DataGridViewTextBoxColumn();
                WayOfGettingColumn.HeaderText = "Способ_получения";
                WayOfGettingColumn.DataPropertyName = "Способ получения";
            DeliveryList.Columns.Add(WayOfGettingColumn);

                DataGridViewColumn StreetColumn = new DataGridViewTextBoxColumn();
                StreetColumn.HeaderText = "Адрес";
                StreetColumn.DataPropertyName = "Адрес";
            DeliveryList.Columns.Add(StreetColumn);

                DataGridViewColumn DateColumn = new DataGridViewTextBoxColumn();
                DateColumn.HeaderText = "Дата заказа";
                DateColumn.DataPropertyName = "Дата_заказа";
            DeliveryList.Columns.Add(DateColumn);
                // Создаем столбец "Добавить
                DataGridViewColumn StatusColumn = new DataGridViewTextBoxColumn();
                StatusColumn.HeaderText = "Статус заказа";
                StatusColumn.DataPropertyName = "Статус_заказа";
            DeliveryList.Columns.Add(StatusColumn);

       DataGridViewColumn CompleteMark = new DataGridViewTextBoxColumn();
                CompleteMark.HeaderText = "Отметка о выполнении";
                CompleteMark.DataPropertyName = "Отметка_о_выполнении";
                DeliveryList.Columns.Add(CompleteMark);

                SqlConnection sqlConnect = new SqlConnection(connection);
                sqlConnect.Open();
                SqlCommand OrderList = new SqlCommand("SELECT Доставка.Номер_отслеживания AS [Номер отслеживания], Заказ.Номер_заказа AS [Номер заказа], " +
                    "CONCAT(Покупатель.Фамилия, ' ', Покупатель.Имя, ' ', Покупатель.Отчество) AS [ФИО], Способ_получения.Название_способа_получения AS [Способ получения], " +
                    "CONCAT(Пункт_назначения.Улица, ', ', Пункт_назначения.Дом) AS [Адрес], Заказ.Дата_заказа AS [Дата заказа], Заказ.Дата_заказа AS [Дата заказа], " +
                    "Статус_заказа.Название_статуса AS [Статус заказа], Доставка.Отметка_о_выполнении AS [Отметка о выполнении] FROM Доставка" +
                    " INNER JOIN Заказ ON Заказ.Номер_заказа=Доставка.Заказ " +
                    " INNER JOIN Покупатель ON Покупатель.ID_покупателя=Заказ.Заказчик " +
                    " INNER JOIN Способ_оплаты ON Способ_оплаты.Код_способа_оплаты=Заказ.Способ_оплаты " +
                    " INNER JOIN Способ_получения ON Способ_получения.Код_способа_получения=Заказ.Способ_получения" +
                    " INNER JOIN Пункт_назначения ON Пункт_назначения.Код_пункта_назначения=Заказ.Адрес_получения" +
                    " INNER JOIN Покупка ON Покупка.Заказ=Заказ.Номер_заказа " +
                    " INNER JOIN Ассортимент ON Покупка.Ассортимент=Ассортимент.Номер_ассортимента" +
                    " INNER JOIN Статус_заказа ON Статус_заказа.ID_Cтатуса=Заказ.Статус_заказа" +
                    " WHERE (Заказ.Статус_заказа=3 OR Заказ.Статус_заказа= 2) AND Ассортимент.Магазин=" + ShopID + " " +
                    " GROUP BY Доставка.Номер_отслеживания, Заказ.Номер_заказа, Покупатель.Фамилия, Покупатель.Имя, Покупатель.Отчество, Заказ.Сумма_оплаты, " +
                    " Способ_получения.Название_способа_получения, " +
                    " Пункт_назначения.Улица, Пункт_назначения.Дом, Заказ.Дата_заказа, Заказ.Дата_заказа, Статус_заказа.Название_статуса, Доставка.Отметка_о_выполнении ", sqlConnect);
            //sqlCmd.Parameters.AddWithValue("@GameID", GameID);
            DeliveryList.AllowUserToAddRows = false;

                // Создаем адаптер данных для заполнения DataSet
                SqlDataAdapter adapter = new SqlDataAdapter(OrderList);

                // Создаем DataSet для хранения данных
                DataSet dataSet = new DataSet();

                // Заполняем DataSet данными из таблицы
                adapter.Fill(dataSet);

            DeliveryList.AutoGenerateColumns = false;

                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    // Получаем данные из строки
                    string DeliveyID = row["Номер отслеживания"].ToString();
                    string OrderID = row["Номер заказа"].ToString();
                    string FullName = row["ФИО"].ToString();
                    string WayOfGettingName = row["Способ получения"].ToString();
                    string Address = row["Адрес"].ToString();
                    string Date = row["Дата заказа"].ToString();
                    string Status = row["Статус заказа"].ToString();
                    string Mark = row["Отметка о выполнении"].ToString();


                    // Создаем строку и заполняем ее данными
                    DataGridViewRow dataGridViewRow = new DataGridViewRow();
                    DataGridViewColumn dataGridViewColumn = new DataGridViewColumn();

                    dataGridViewRow.CreateCells(DeliveryList);


                    dataGridViewRow.Cells[0].Value = DeliveyID;
                    dataGridViewRow.Cells[1].Value = OrderID;
                    dataGridViewRow.Cells[2].Value = FullName;
                    dataGridViewRow.Cells[3].Value = WayOfGettingName;
                    dataGridViewRow.Cells[4].Value = Address;
                    dataGridViewRow.Cells[5].Value = Date;
                    dataGridViewRow.Cells[6].Value = Status;
                    dataGridViewRow.Cells[7].Value = Mark;


                    dataGridViewColumn.Width = 150;
                // Остальные значения...

                // Добавляем строку в таблицу
                DeliveryList.Rows.Add(dataGridViewRow);

                }

            }


        // Обработчик нажатия на ячейку
        private void DeliveryList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && !(DeliveryList.Columns[e.ColumnIndex] is DataGridViewButtonColumn)) // Проверяем, что нажатие не произошло на кнопке
            {
                DataGridViewRow selectedRow = DeliveryList.Rows[e.RowIndex];
                int DeliveryNumber = Convert.ToInt32(selectedRow.Cells[0].Value);

                ShowDeliveryDetails(DeliveryNumber);


            }

        }

        public void ShowDeliveryDetails(int DeliveryNumber) 
        {
            ShopProfile.Visible = false;
            AssortmentDeleting.Visible = false;
            AssortmentUpdating.Visible = false;
            AssortmentInserting.Visible = false;
            OrderDetails.Visible = false;
            NewOrders.Visible = false;
            ProcessedOrders.Visible = false;
            Deliveries.Visible = false;
            DeiiveryDetails.Visible = true;
            DeiiveryDetails.Size = new Size(960, 360);
            DeiiveryDetails.Location = new Point(20, 30);


            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            SqlCommand getOrderDetails = new SqlCommand("SELECT Доставка.Номер_отслеживания AS [Номер отслеживания], Доставка.Претензии, Заказ.Номер_заказа AS [Номер заказа]," +
                " CONCAT(Покупатель.Фамилия, ' ', Покупатель.Имя, ' ', Покупатель.Отчество) AS [ФИО], Способ_получения.Название_способа_получения AS [Способ получения]," +
                " CONCAT(Пункт_назначения.Улица, ', ', Пункт_назначения.Дом) AS [Адрес], Заказ.Дата_заказа AS [Дата заказа], Заказ.Дата_выдачи AS [Дата доставки], Статус_заказа.Название_статуса AS [Статус заказа], " +
                " Доставка.Отметка_о_выполнении AS [Отметка о выполнении] FROM Доставка  " +
                " INNER JOIN Заказ ON Заказ.Номер_заказа=Доставка.Заказ " +
                " INNER JOIN Покупатель ON Покупатель.ID_покупателя=Заказ.Заказчик" +
                " INNER JOIN Способ_оплаты ON Способ_оплаты.Код_способа_оплаты=Заказ.Способ_оплаты " +
                " INNER JOIN Способ_получения ON Способ_получения.Код_способа_получения=Заказ.Способ_получения " +
                " INNER JOIN Пункт_назначения ON Пункт_назначения.Код_пункта_назначения=Заказ.Адрес_получения " +
                "INNER JOIN Покупка ON Покупка.Заказ=Заказ.Номер_заказа " +
                " INNER JOIN Ассортимент ON Покупка.Ассортимент=Ассортимент.Номер_ассортимента" +
                " INNER JOIN Статус_заказа ON Статус_заказа.ID_Cтатуса=Заказ.Статус_заказа" +
                " WHERE Доставка.Номер_отслеживания= "+DeliveryNumber+" AND Ассортимент.Магазин=" + ShopID+"" +
                " GROUP BY Доставка.Номер_отслеживания, Заказ.Номер_заказа, Покупатель.Фамилия, Покупатель.Имя, Покупатель.Отчество, Заказ.Сумма_оплаты, Способ_получения.Название_способа_получения," +
                " Пункт_назначения.Улица, Пункт_назначения.Дом, Заказ.Дата_заказа, Статус_заказа.Название_статуса, Доставка.Отметка_о_выполнении,  Заказ.Дата_выдачи, Доставка.Претензии", sqlConnect);
            //sqlCmd.Parameters.AddWithValue("@GameID", GameID);


            using (SqlDataReader sqlReader = getOrderDetails.ExecuteReader())
            {
                if (sqlReader.Read())
                {
                    DeliveryLabel.Text = DeliveryNumber.ToString();
                    
                  
                   
                    FullNameDelivery.Text = sqlReader["ФИО"].ToString();
                    AddressDelivery.Text = sqlReader["Адрес"].ToString();
                    WayOfGettingDelivery.Text = sqlReader["Способ получения"].ToString();

  DeliveryStatusLabel.Text = sqlReader["Статус заказа"].ToString();
OrderDeliveryLabel.Text = sqlReader["Номер заказа"].ToString();
 DeliveryDateLabel.Text = sqlReader["Дата заказа"].ToString();
                    IssueDateLabel.Text = sqlReader["Дата доставки"].ToString();
                    CompleteMarkLabel.Text = sqlReader["Отметка о выполнении"].ToString();
                    Claims.Text = sqlReader["Претензии"].ToString();

                }
            }
        }

        private void OrderOpen_Click(object sender, EventArgs e)
        {
            DeiiveryDetails.Visible = false;
            Deliveries.Visible = false;
            ShopProfile.Visible = false;
            AssortmentDeleting.Visible = false;
            AssortmentUpdating.Visible = false;
            AssortmentInserting.Visible = false;
            NewOrders.Visible = false;
            OrderDetails.Visible = true;
            OrderDetails.Size = new Size(1250, 300);
            OrderDetails.Location = new Point(20, 30);
            int orderNumber = Convert.ToInt32(OrderDeliveryLabel.Text);
            ShowOrderDetails(orderNumber);

        }

        private void ComeBackToReports_Click(object sender, EventArgs e)
        {
            DeiiveryDetails.Visible = false;
            Deliveries.Visible = false;
            ShopProfile.Visible = false;
            AssortmentDeleting.Visible = false;
            AssortmentUpdating.Visible = false;
            AssortmentInserting.Visible = false;
            NewOrders.Visible = false;
            Reports.Visible = true;
            Reports.Size = new Size(1050, 300);
            Reports.Location = new Point(20, 30);
            ReportListShow();


        }

     

        private void ComeBackToReports_Click_1(object sender, EventArgs e)
        {
            DeiiveryDetails.Visible = false;
            Deliveries.Visible = false;
            ShopProfile.Visible = false;
            AssortmentDeleting.Visible = false;
            AssortmentUpdating.Visible = false;
            AssortmentInserting.Visible = false;
            NewOrders.Visible = false;
            ReportDetails.Visible = false;
            Reports.Visible = true;
            Reports.Size = new Size(1050, 300);
            Reports.Location = new Point(20, 30);
            DeliveryListShow();
        }

        private void Game_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void AssortmentUpdating_Enter(object sender, EventArgs e)
        {

        }
    }


    }

