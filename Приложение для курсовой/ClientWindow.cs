using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Приложение_для_курсовой
{
    public partial class ClientWindow : Form
    {
        public int UserID;
        public int ClientID;
        public ClientWindow(int userID, int clientID)
        {
            InitializeComponent();
            UserID = userID;
            ClientID = clientID;

            ShowProfileData(UserID, ClientID);
            label10.Text = count.ToString();
            CartPurchase.CellContentClick += CartPurchase_CellContentClick;


            OrdersData.CellClick += OrdersData_CellClick;
            ReportList.CellClick += ReportList_CellClick;
            DeliveryList.CellClick += DeliveryList_CellClick;
            CompletedOrderList.CellClick += CompletedOrderList_CellClick;
            CompletePurchaseList.CellClick += CompletePurchaseList_CellClick;
            CompletedOrderList.CellContentClick += CompletedOrderList_CellContetClick;

        }
        public string connection = @"Data Source=DESKTOP-NIKISIN\JET;Initial Catalog=Онлайн-магазин; User ID=sa;Password=Q1w2e3r4";


        public event System.Data.SqlClient.SqlInfoMessageEventHandler InfoMessage;
        public float sum;
        public int count;

        // Обработчик события InfoMessage



        public void ShowProfileData(int UserID, int ClientID)
        {

            Profile.Visible = true;
            Profile.Size = new Size(500, 500);
            Profile.Location = new Point(25, 30);
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();

            SqlCommand sqlView = new SqlCommand(@"Select ID_Покупателя, Логин, Пароль, Фамилия, Имя, Отчество, Дата_регистрации, Дата_рождения 
from [dbo].[Покупатель] INNER JOIN [dbo].[Пользователи] ON Покупатель.Сопоставленный_пользователь=Пользователи.ID_Пользователя WHERE ID_Пользователя='" + UserID + "'", sqlConnect);
            SqlDataReader sqlReader = null;
            sqlReader = sqlView.ExecuteReader();
            while (sqlReader.Read())
            {
                label9.Text = UserID.ToString();
                UserLogin.Text = sqlReader["Логин"].ToString();
                UserPassword.Text = sqlReader["Пароль"].ToString();
                UserSurname.Text = sqlReader["Фамилия"].ToString();
                UserName.Text = sqlReader["Имя"].ToString();
                UserThirdName.Text = sqlReader["Отчество"].ToString();
                UserBirthDate.Text = sqlReader["Дата_рождения"].ToString();
                UserRegDate.Text = sqlReader["Дата_Регистрации"].ToString();


            }


        }



        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void ClientWindow_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "_Онлайн_магазинDataSet.Способ_получения". При необходимости она может быть перемещена или удалена.
            this.способ_полученияTableAdapter.Fill(this._Онлайн_магазинDataSet.Способ_получения);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "_Онлайн_магазинDataSet.Способ_оплаты". При необходимости она может быть перемещена или удалена.
            this.способ_оплатыTableAdapter.Fill(this._Онлайн_магазинDataSet.Способ_оплаты);

        }

        private void UserDateBirth_TextChanged(object sender, EventArgs e)
        {

        }

        private void профильToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Очистить контейнер для отображения данных корзины (например, DataGridView)
            CartPurchase.Rows.Clear();

            // Создайть необходимые столбцы в контейнере
            DeiiveryDetails.Visible = false;
            OrderDetails.Visible = false;
            CompletedOrderDetails.Visible = false;
            OrderCreating.Visible = false;
            Orders.Visible = false;
            CartBox.Visible = false;
            ReportInserting.Visible = false;
            ReportDetails.Visible = false;
            ShopListBox.Visible = false;
            Deliveries.Visible  = false;
            Assortment.Visible = false;
            Reports.Visible = false;
            CompletedOrders.Visible = false;
            ShopAssortment.Visible = false;
            ProductPage.Visible = false;
            OrderCreating.Visible = false;
            CartPurchase.Columns.Clear();
            Profile.Visible = true;
            Profile.Size = new Size(500, 500);
            Profile.Location = new Point(25, 30);
        }

        private void UpdateProfile_Click(object sender, EventArgs e)
        {

            UserLogin.Enabled = true;
            UserPassword.Enabled = true;
            UserSurname.Enabled = true;
            UserName.Enabled = true;
            UserThirdName.Enabled = true;
            UserBirthDate.Enabled = true;

        }

        public void SaveData()
        {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();

            SqlCommand sqlUpdateUser = new SqlCommand("Update [dbo].[Пользователи] Set Логин='" + UserLogin.Text + "', Пароль='" + UserPassword.Text + "'WHERE ID_пользователя='" + UserID + "' ", sqlConnect);
            SqlCommand sqlUpdateClient = new SqlCommand("Update [dbo].[Покупатель] Set Фамилия='" + UserSurname.Text + "', Имя='" + UserName.Text + "', Отчество='" + UserThirdName.Text + "', Дата_рождения='" + UserBirthDate.Text + "' WHERE ID_покупателя='" + ClientID + "'", sqlConnect);
            sqlUpdateUser.ExecuteNonQuery();
            sqlUpdateClient.ExecuteNonQuery();

            sqlConnect.Close();
            MessageBox.Show("Данные покупател успешно изменены", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Save_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        public void ShowShops()
        {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            SqlCommand ShopList = new SqlCommand("SELECT Магазин.Код_магазина, Магазин.Название_магазина AS Магазин, COUNT (*) AS Количество_игр FROM Ассортимент" +
                " INNER JOIN Магазин ON Ассортимент.Магазин=Магазин.Код_магазина WHERE Ассортимент.Наличие = 1 GROUP BY Магазин.Код_магазина, Магазин.Название_магазина", sqlConnect);
            //sqlCmd.Parameters.AddWithValue("@GameID", GameID);
            ShopTable.AllowUserToAddRows = false;

            SqlDataReader ShopReader = ShopList.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(ShopReader);
            ShopTable.DataSource = dataTable;
            if (!ShopTable.Columns.Contains("GoShopButton"))
            {
                DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
                buttonColumn.Name = "GoShopButton";
                buttonColumn.HeaderText = "Перейти в магазин";
                buttonColumn.Text = "Перейти в магазин";
                buttonColumn.UseColumnTextForButtonValue = true;
                ShopTable.Columns.Add(buttonColumn);
            }
            ShopTable.Columns["Код_магазина"].Visible = false;
            ShopTable.CellContentClick += ShopTable_CellContentClick;
        }

        private void ShopTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == ShopTable.Columns["GoShopButton"].Index)
            {


                int ShopID = Convert.ToInt32(ShopTable.Rows[e.RowIndex].Cells["Код_магазина"].Value);



                GoShopAssortment(ShopID);
                ShopListBox.Visible = false;
                Profile.Visible = false;
                ShopAssortment.Visible = true;
                ShopAssortment.Size = new Size(690, 550);
                ShopAssortment.Location = new Point(20, 30);
            }
        }

        public void GoShopAssortment(int ShopID)
        {
            flowLayoutPanel2.Controls.Clear();
            label17.Text = ShopID.ToString();
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();


            // Выполнение SQL-запроса для получения данных
            SqlCommand sqlCmd = new SqlCommand("SELECT Ассортимент.Номер_ассортимента, Настольная_игра.Название, Настольная_игра.Изображение_игры, Ассортимент.Цена, Жанр.Название_жанра FROM Настольная_игра" +
                " INNER JOIN Жанр ON Настольная_игра.жанр=Жанр.Код_жанра" +
                " INNER JOIN Ассортимент ON Настольная_игра.Артикул = Ассортимент.Настольная_игра " +
                " INNER JOIN Магазин ON Магазин.Код_магазина=Ассортимент.Магазин" +
                " WHERE Ассортимент.Наличие=" + 1 + " AND Магазин.Код_магазина=" + ShopID + " ", sqlConnect);
            SqlDataReader sqlReader = sqlCmd.ExecuteReader();

            // Создание FlowLayoutPanel для размещения рамок с информацией
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
            flowLayoutPanel.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel.WrapContents = false;

            // Цикл для создания рамок с информацией о настольных играх
            while (sqlReader.Read())
            {

                string ImagePath = sqlReader["Изображение_игры"].ToString();


                PictureBox pic = new PictureBox();
                Image image = Image.FromFile(ImagePath);
                pic.Image = image;

                pic.SizeMode = PictureBoxSizeMode.Zoom;
                pic.Width = 190;
                pic.Height = 190;
                pic.BorderStyle = BorderStyle.FixedSingle;

                int AssortmentID = Convert.ToInt32(sqlReader["Номер_ассортимента"]);
                pic.Tag = AssortmentID;


                // Add the click event handler to the picture box
                pic.Click += PictureBox_Click;


                Label Name = new Label();
                Name.Text = sqlReader["Название"].ToString();
                Name.BackColor = Color.FromArgb(0, 150, 255);
                Name.TextAlign = ContentAlignment.MiddleCenter;
                Name.Dock = DockStyle.Bottom;


                Label Genre = new Label();

                Genre.Text = sqlReader["Название_жанра"].ToString();
                Genre.AutoSize = true;
                //Name.BackColor = Color.FromArgb(46, 134, 222);
                Genre.TextAlign = ContentAlignment.MiddleLeft;


                Label Price = new Label();
                Price.Text = sqlReader["Цена"].ToString() + "₽";

                Price.TextAlign = ContentAlignment.MiddleCenter;
                Price.Dock = DockStyle.Bottom;


                pic.Controls.Add(Genre);
                pic.Controls.Add(Name);
                pic.Controls.Add(Price);
                flowLayoutPanel2.Controls.Add(pic);

            }

            // Dispose the SqlDataReader and SqlCommand
            sqlReader.Close();
            sqlCmd.Dispose();

            // Close the SqlConnection
            sqlConnect.Close();
        }

        private void магазиныToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Очистить контейнер для отображения данных корзины (например, DataGridView)
            CartPurchase.Rows.Clear();

            // Создайть необходимые столбцы в контейнере
            CartPurchase.Columns.Clear();

            // Создайть необходимые столбцы в контейнере
            DeiiveryDetails.Visible = false;
            OrderDetails.Visible = false;
            CompletedOrderDetails.Visible = false;
            OrderCreating.Visible = false;
            Orders.Visible = false;
            CartBox.Visible = false;
            ReportInserting.Visible = false;
            ReportDetails.Visible = false;
            Deliveries.Visible = false;
            Assortment.Visible = false;
            Reports.Visible = false;
            CompletedOrders.Visible = false;
            ShopAssortment.Visible = false;
            ProductPage.Visible = false;
            OrderCreating.Visible = false;
            Profile.Visible = false;
            ShopListBox.Size = new Size(600, 200);
            ShopListBox.Visible = true;
            ShopListBox.Location = new Point(20, 30);
            ShowShops();
        }

        private void магазиныToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Очистить контейнер для отображения данных корзины (например, DataGridView)
            CartPurchase.Rows.Clear();
            OrderCreating.Visible = false;

            // Создайть необходимые столбцы в контейнере
            CartPurchase.Columns.Clear();
            CartPurchase.Rows.Clear();

            // Создайть необходимые столбцы в контейнере
            DeiiveryDetails.Visible = false;
            OrderDetails.Visible = false;
            CompletedOrderDetails.Visible = false;
            OrderCreating.Visible = false;
            Orders.Visible = false;
            CartBox.Visible = false;
            ReportInserting.Visible = false;
            ReportDetails.Visible = false;
            ShopListBox.Visible = false;
            Deliveries.Visible = false;
            Reports.Visible = false;
            CompletedOrders.Visible = false;
            ShopAssortment.Visible = false;
            ProductPage.Visible = false;
            OrderCreating.Visible = false;
            Profile.Visible = false;
            Assortment.Visible = true;

            Assortment.Size = new Size(650, 550);
            Assortment.Location = new Point(20, 30);
            ShowAssortment();

        }

        public void ShowProduct(int AssortmentID, int ShopID)
        {
            CartBox.Visible = false;

            label18.Text = AssortmentID.ToString();
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();

            // Выполнение SQL-запроса для получения данных
            SqlCommand sqlCmd = new SqlCommand("SELECT Ассортимент.Номер_ассортимента, Настольная_игра.Название, Настольная_игра.Изображение_игры, " +
    "Настольная_игра.Минимальное_количество_игроков, Настольная_игра.Максимальное_количество_игроков, Настольная_игра.Возрастное_ограничение, " +
    "Настольная_игра.Год_издания, Ассортимент.Цена, Жанр.Название_жанра, Изготовитель.Название_фирмы, Страна.Название_страны " +
    "FROM Настольная_игра " +
    "INNER JOIN Жанр ON Настольная_игра.Жанр=Жанр.Код_жанра " +
    "INNER JOIN Ассортимент ON Настольная_игра.Артикул = Ассортимент.Настольная_игра " +
    "INNER JOIN Изготовитель ON Настольная_игра.Производитель=Изготовитель.Код_производителя " +
    "INNER JOIN Страна ON Изготовитель.Страна=Страна.Код_страны " +
    "WHERE Ассортимент.Номер_ассортимента=" + AssortmentID, sqlConnect);
            SqlDataReader sqlReader = sqlCmd.ExecuteReader();
            while (sqlReader.Read())
            {
                label11.Text = sqlReader["Название"].ToString();
                label12.Text = sqlReader["Цена"].ToString() + "₽";
                string ImagePath = sqlReader["Изображение_игры"].ToString();
                pictureBox2.Image = new Bitmap(ImagePath);
                String MinPlayerCount = sqlReader["Минимальное_количество_игроков"].ToString();
                String MaxPlayerCount = sqlReader["Максимальное_количество_игроков"].ToString();
                String Age = sqlReader["Возрастное_ограничение"].ToString();
                String Jenre = sqlReader["Название_жанра"].ToString();
                String Manufacturer = sqlReader["Название_фирмы"].ToString();
                String Country = sqlReader["Название_страны"].ToString();
                String Year = sqlReader["Год_издания"].ToString();
                Description.Text = "Жанр: " + Jenre + "\nПроизводитель: " + Manufacturer + "\nСтрана: " + Country + "\nМинимальное количество игроков: " + MinPlayerCount
                    + "\nМаксимальное количество игроков: " + MaxPlayerCount + "\nВозрастное органичение: " + Age + "\nГод издания: " + Year;
            }

            sqlReader.Close();

            // Set the AllowUserToAddRows property to false
            dataGridView1.AllowUserToAddRows = false;

            SqlCommand FindGameID = new SqlCommand("Select Настольная_игра.Артикул FROM Настольная_игра" +
                " INNER JOIN Ассортимент ON Ассортимент.Настольная_игра=Настольная_игра.Артикул WHERE Ассортимент.Номер_ассортимента=" + AssortmentID + " ", sqlConnect);
            int GameID = Convert.ToInt32(FindGameID.ExecuteScalar());
            // Retrieve data for other stores selling the board game
            SqlCommand ShopList = new SqlCommand("SELECT Магазин.Название_магазина AS Магазин, Ассортимент.Цена, Ассортимент.Номер_Ассортимента, Магазин.Код_магазина" +
                " FROM Ассортимент INNER JOIN Магазин ON Ассортимент.Магазин = Магазин.Код_магазина " +
                " INNER JOIN Настольная_игра ON Настольная_игра.Артикул=Ассортимент.Настольная_игра" +
                " WHERE Ассортимент.Настольная_игра =" + GameID + " AND Ассортимент.Наличие = " + 1 + " ORDER BY Ассортимент.Цена ASC ", sqlConnect);
            //sqlCmd.Parameters.AddWithValue("@GameID", GameID);
            SqlDataReader ShopReader = ShopList.ExecuteReader();

            // Create a DataTable to store the retrieved data
            DataTable dataTable = new DataTable();
            dataTable.Load(ShopReader);

            // Set the DataSource property of the DataGridView to the DataTable
            dataGridView1.DataSource = dataTable;

            // Add a button column to the DataGridView
            /* DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
             buttonColumn.Name = "BuyButton";
             buttonColumn.HeaderText = "Купить";
             buttonColumn.Text = "Купить";

             buttonColumn.UseColumnTextForButtonValue = true;
             dataGridView1.Columns.Add(buttonColumn);
 */

            if (!dataGridView1.Columns.Contains("BuyButton"))
            {
                DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
                buttonColumn.Name = "BuyButton";
                buttonColumn.HeaderText = "Купить";
                buttonColumn.Text = "Купить";
                buttonColumn.UseColumnTextForButtonValue = true;
                dataGridView1.Columns.Add(buttonColumn);
            }
            dataGridView1.Columns["Номер_ассортимента"].Visible = false;
            dataGridView1.Columns["Код_магазина"].Visible = false;


            // Handle the button click event
            dataGridView1.CellContentClick -= DataGridView1_CellContentClick; // Remove the event handler to prevent multiple subscriptions
            dataGridView1.CellContentClick += DataGridView1_CellContentClick;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow row = dataGridView1.Rows[i];
                row.Tag = new int[] { AssortmentID, ShopID };
            }
        }

        private void Buy_Click(object sender, EventArgs e)
        {
            int AssortmentID = Convert.ToInt32(label18.Text);
            count = count + 1;
            label10.Text = count.ToString();
            using (SqlConnection sqlConnect = new SqlConnection(connection))
            {
                sqlConnect.Open();
                SqlCommand getPriceCmd = new SqlCommand("SELECT Ассортимент.Цена FROM Ассортимент WHERE Номер_ассортимента=@AssortmentID", sqlConnect);
                getPriceCmd.Parameters.AddWithValue("@AssortmentID", AssortmentID);
                float price = Convert.ToSingle(getPriceCmd.ExecuteScalar());
                sum += price;
            }

            Assorments.Add(AssortmentID);
            purchasedGames.Add(AssortmentID);
            if (gameCopies.ContainsKey(AssortmentID))
            {
                // Увеличиваем количество копий на 1
                gameCopies[AssortmentID]++;
            }
            else
            {
                // Если нет, добавляем номер ассортимента в словарь со значением 1
                gameCopies.Add(AssortmentID, 1);
            }
            MessageBox.Show("Товар добавлен в корзину", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);

            CartPurchase.Controls.Clear();

        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["BuyButton"].Index)
            {

                int AssortmentID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["Номер_ассортимента"].Value);
                int ShopID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["Код_магазина"].Value);


                count = count + 1;
                label10.Text = count.ToString();
                using (SqlConnection sqlConnect = new SqlConnection(connection))
                {
                    sqlConnect.Open();
                    SqlCommand getPriceCmd = new SqlCommand("SELECT Цена FROM Ассортимент WHERE Номер_ассортимента=@AssortmentID", sqlConnect);
                    getPriceCmd.Parameters.AddWithValue("@AssortmentID", AssortmentID);
                    float price = Convert.ToSingle(getPriceCmd.ExecuteScalar());
                    sum += price;
                }
                purchasedGames.Add(AssortmentID);
                Assorments.Add(AssortmentID);
                if (gameCopies.ContainsKey(AssortmentID))
                {
                    // Увеличиваем количество копий на 1
                    gameCopies[AssortmentID]++;
                }
                else
                {
                    // Если нет, добавляем номер ассортимента в словарь со значением 1
                    gameCopies.Add(AssortmentID, 1);
                }
                CartPurchase.Controls.Clear();
                MessageBox.Show("Товар добавлен в корзину", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }


        public void ShowAssortment()
        {
            flowLayoutPanel1.Controls.Clear();
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();

            // Выполнение SQL-запроса для получения данных
            SqlCommand sqlCmd = new SqlCommand("SELECT Ассортимент.Номер_ассортимента, Настольная_игра.Название, Настольная_игра.Изображение_игры, Ассортимент.Цена, Жанр.Название_жанра FROM Настольная_игра INNER JOIN Жанр ON Настольная_игра.жанр=Жанр.Код_жанра INNER JOIN Ассортимент ON Настольная_игра.Артикул = Ассортимент.Настольная_игра WHERE Ассортимент.Наличие=" + 1 + "", sqlConnect);
            SqlDataReader sqlReader = sqlCmd.ExecuteReader();

            // Создание FlowLayoutPanel для размещения рамок с информацией
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
            flowLayoutPanel.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel.WrapContents = false;

            // Цикл для создания рамок с информацией о настольных играх
            while (sqlReader.Read())
            {

                string ImagePath = sqlReader["Изображение_игры"].ToString();


                PictureBox pic = new PictureBox();
                Image image = Image.FromFile(ImagePath);
                pic.Image = image;

                pic.SizeMode = PictureBoxSizeMode.Zoom;
                pic.Width = 190;
                pic.Height = 190;
                pic.BorderStyle = BorderStyle.FixedSingle;

                int AssortmentID = Convert.ToInt32(sqlReader["Номер_ассортимента"]);
                pic.Tag = AssortmentID;


                // Add the click event handler to the picture box
                pic.Click += PictureBox_Click;


                Label Name = new Label();
                Name.Text = sqlReader["Название"].ToString();
                Name.BackColor = Color.FromArgb(0, 150, 255);
                Name.TextAlign = ContentAlignment.MiddleCenter;
                Name.Dock = DockStyle.Bottom;


                Label Genre = new Label();

                Genre.Text = sqlReader["Название_жанра"].ToString();
                Genre.AutoSize = true;
                //Name.BackColor = Color.FromArgb(46, 134, 222);
                Genre.TextAlign = ContentAlignment.MiddleLeft;


                Label Price = new Label();
                Price.Text = sqlReader["Цена"].ToString() + "₽";

                Price.TextAlign = ContentAlignment.MiddleCenter;
                Price.Dock = DockStyle.Bottom;


                pic.Controls.Add(Genre);
                pic.Controls.Add(Name);
                pic.Controls.Add(Price);
                flowLayoutPanel1.Controls.Add(pic);

            }

            // Dispose the SqlDataReader and SqlCommand
            sqlReader.Close();
            sqlCmd.Dispose();

            // Close the SqlConnection
            sqlConnect.Close();

        }




        private void PictureBox_Click(object sender, EventArgs e)
        {

            Profile.Visible = false;
            Assortment.Visible = false;
            Assortment.Visible = false;
            ShopAssortment.Visible = false;
            ProductPage.Size = new Size(600, 600);
            ProductPage.Location = new Point(20, 30);
            ProductPage.Visible = true;
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            PictureBox pictureBox = (PictureBox)sender;
            dataGridView1.Columns.Clear();
            // Retrieve the assortment number and shop code from the Tag property


            int AssortmentID = (int)pictureBox.Tag;
            SqlCommand FindShopID = new SqlCommand("Select Магазин.Код_магазина FROM Магазин INNER JOIN Ассортимент ON Магазин.Код_магазина=Ассортимент.Магазин " +
                   "WHERE Ассортимент.Номер_ассортимента=" + AssortmentID + "", sqlConnect);
            FindShopID.Dispose();
            int ShopID = Convert.ToInt32(FindShopID.ExecuteScalar());

            // MessageBox.Show("Номер_ассортимента" + AssortmentID + "Код магазина:" + ShopID);

            // Call your method with the assortment number and shop code
            ShowProduct(AssortmentID, ShopID);


        }


        private void заказыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CartPurchase.Rows.Clear();
            CartPurchase.Columns.Clear();

            // Создайть необходимые столбцы в контейнере
            DeiiveryDetails.Visible = false;
            OrderDetails.Visible = false;
            CompletedOrderDetails.Visible = false;
            OrderCreating.Visible = false;
            CartBox.Visible = false;
            ReportInserting.Visible = false;
            ReportDetails.Visible = false;
            ShopListBox.Visible = false;
            Deliveries.Visible = false;
            Assortment.Visible = false;
            Reports.Visible = false;
            CompletedOrders.Visible = false;
            ShopAssortment.Visible = false;
            ProductPage.Visible = false;
            OrderCreating.Visible = false;
            Profile.Visible = false;
            Orders.Size = new Size(1300, 270);
            Orders.Location = new Point(20, 30);
            Orders.Visible = true;

            ShowOrdersList();

        }

        public void ShowOrdersList()
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




            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            SqlCommand OrderList = new SqlCommand("SELECT Заказ.Номер_заказа AS [Номер заказа], Заказ.Сумма_оплаты AS [Сумма оплаты], Способ_оплаты.Название_способа_оплаты AS [Способ оплаты]," +
    " Способ_получения.Название_способа_получения AS [Способ получения], CONCAT(Пункт_назначения.Улица, ', ', Пункт_назначения.Дом) AS [Адрес], Заказ.Дата_заказа AS [Дата заказа], Статус_заказа.Название_статуса AS [Статус заказа] FROM Заказ" +
    " INNER JOIN Покупатель ON Покупатель.ID_покупателя=Заказ.Заказчик " +
    " INNER JOIN Способ_оплаты ON Способ_оплаты.Код_способа_оплаты=Заказ.Способ_оплаты " +
    " INNER JOIN Способ_получения ON Способ_получения.Код_способа_получения=Заказ.Способ_получения" +
    " INNER JOIN Пункт_назначения ON Пункт_назначения.Код_пункта_назначения=Заказ.Адрес_получения" +
    " INNER JOIN Статус_заказа ON Статус_заказа.ID_Cтатуса=Заказ.Статус_заказа " +
    " WHERE Заказ.Заказчик=" + ClientID +
    " GROUP BY  Заказ.Номер_заказа,  Заказ.Сумма_оплаты,  " +
    " Способ_оплаты.Название_способа_оплаты, Способ_получения.Название_способа_получения, " +
    " Пункт_назначения.Улица, Пункт_назначения.Дом, Заказ.Дата_заказа, Статус_заказа.Название_статуса ", sqlConnect);
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
                dataGridViewRow.Cells[1].Value = Sum;
                dataGridViewRow.Cells[2].Value = WayOfPaymentName;
                dataGridViewRow.Cells[3].Value = WayOfGettingtName;
                dataGridViewRow.Cells[4].Value = Address;
                dataGridViewRow.Cells[5].Value = Date;
                dataGridViewRow.Cells[6].Value = Status;

                switch (Status)
                {
                    case "Отменен":
                        dataGridViewRow.DefaultCellStyle.ForeColor = Color.Red;
                        break;
                    case "Доставлен":
                        dataGridViewRow.DefaultCellStyle.ForeColor = Color.Green;
                        break;
                    case "В обработке":
                        dataGridViewRow.DefaultCellStyle.ForeColor = Color.Orange;
                        break;
                }
                dataGridViewColumn.Width = 150;
                // Остальные значения...

                // Добавляем строку в таблицу
                OrdersData.Rows.Add(dataGridViewRow);

            }

        }

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

        public void ShowOrderDetails(int orderNumber)
        {
            Orders.Visible = false;
            OrderDetails.Visible = true;
            OrderDetails.Size = new Size(1200, 420);
            OrderDetails.Location = new Point(20, 30);


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

            DataGridViewColumn NameColumn = new DataGridViewTextBoxColumn();
            NameColumn.HeaderText = "Название";
            NameColumn.DataPropertyName = "Настольная игра";
            PurchaseList.Columns.Add(NameColumn);

            // Создаем столбец для отображения картинок
            DataGridViewColumn ShopNameColumn = new DataGridViewTextBoxColumn();
            ShopNameColumn.HeaderText = "Магазин";
            ShopNameColumn.DataPropertyName = "Название_магазина";
            PurchaseList.Columns.Add(ShopNameColumn);

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
                " WHERE Заказ.Номер_заказа= " + orderNumber + " " +
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


            SqlCommand getPurchaseList = new SqlCommand("SELECT Покупка.Ассортимент, Настольная_игра.Изображение_игры, Настольная_игра.Название, Магазин.Название_магазина,  COUNT(*) AS [Количество], Покупка.Цена_покупки FROM Покупка " +
                " INNER JOIN Ассортимент ON Ассортимент.Номер_ассортимента=Покупка.Ассортимент" +
                " INNER JOIN Настольная_игра ON Ассортимент.Настольная_игра=Настольная_игра.Артикул" +
                " INNER JOIN Магазин ON Магазин.Код_магазина=Ассортимент.Магазин " +
                " INNER JOIN Заказ ON Заказ.Номер_заказа=Покупка.Заказ WHERE Покупка.Заказ=" + orderNumber +
                " GROUP BY Покупка.Ассортимент, Настольная_игра.Изображение_игры, Настольная_игра.Название, Магазин.Название_магазина, Покупка.Цена_покупки, [Количество]", sqlConnect);

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
                string ShopName = row["Название_магазина"].ToString();
                string quantity = row["Количество"].ToString();
                string Price = row["Цена_покупки"].ToString();

                // Создаем строку и заполняем ее данными
                DataGridViewRow dataGridViewRow = new DataGridViewRow();
                DataGridViewColumn dataGridViewColumn = new DataGridViewColumn();

                dataGridViewRow.CreateCells(PurchaseList);

                dataGridViewRow.Cells[0].Value = ID;
                dataGridViewRow.Cells[1].Value = image;
                dataGridViewRow.Cells[2].Value = Name;
                dataGridViewRow.Cells[3].Value = ShopName;
                dataGridViewRow.Cells[4].Value = quantity;
                dataGridViewRow.Cells[5].Value = Price;

                dataGridViewColumn.Width = 150;
                // Остальные значения...
                dataGridViewRow.Height = 150;

                // Добавляем строку в таблицу
                PurchaseList.Rows.Add(dataGridViewRow);

            }
        }



        private void заявленияНаВозвратToolStripMenuItem_Click(object sender, EventArgs e)
        {

            CartPurchase.Rows.Clear();
            ReportList.Controls.Clear();
            CartPurchase.Columns.Clear();

            // Создайть необходимые столбцы в контейнере
            DeiiveryDetails.Visible = false;
            OrderDetails.Visible = false;
            CompletedOrderDetails.Visible = false;
            OrderCreating.Visible = false;
            Orders.Visible = false;
            CartBox.Visible = false;
            ReportInserting.Visible = false;
            ReportDetails.Visible = false;
            ShopListBox.Visible = false;
            Deliveries.Visible = false;
            Assortment.Visible = false;
            CompletedOrders.Visible = false;
            ShopAssortment.Visible = false;
            ProductPage.Visible = false;
            OrderCreating.Visible = false;
            Profile.Visible = false;
            Reports.Location = new Point(20, 30);
            Reports.Size = new Size(1000, 300);
            Reports.Visible = true;
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
            NameColumn.HeaderText = "Магазин";
            NameColumn.DataPropertyName = "Магазин";
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
                "Заявление_на_возврат.Покупка AS [Номер покупки], Магазин.Название_магазина AS Магазин,  Настольная_игра.Название, " +
               " Заявление_на_возврат.Дата_отправки_заявления AS [Дата отправки]," +
                " Заявление_на_возврат.Сумма_возврата AS [Сумма возврата] FROM Заявление_на_возврат" +
                " INNER JOIN Магазин ON Магазин.Код_магазина=Заявление_на_возврат.Продавец " +
                " INNER JOIN Покупка ON Покупка.Номер_покупки=Заявление_на_возврат.Покупка " +
                " INNER JOIN Ассортимент ON Покупка.Ассортимент=Ассортимент.Номер_ассортимента " +
                " INNER JOIN Настольная_игра ON Настольная_игра.Артикул=Ассортимент.Настольная_игра " +
                " INNER JOIN Заказ ON Покупка.Заказ=Заказ.Номер_заказа " +
                " INNER JOIN Покупатель ON Покупатель.ID_покупателя=Заказ.Заказчик" +
                " WHERE Заказ.Заказчик=" + ClientID + " ", sqlConnect);
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


                string PurchaseID = row["Номер покупки"].ToString();
                string ShopName = row["Магазин"].ToString();
                string GameName = row["Название"].ToString();
                string Date = row["Дата отправки"].ToString();
                string Sum = row["Сумма возврата"].ToString();


                // Создаем строку и заполняем ее данными
                DataGridViewRow dataGridViewRow = new DataGridViewRow();
                DataGridViewColumn dataGridViewColumn = new DataGridViewColumn();

                dataGridViewRow.CreateCells(ReportList);


                dataGridViewRow.Cells[0].Value = RepoertID;
                dataGridViewRow.Cells[1].Value = PurchaseID;
                dataGridViewRow.Cells[2].Value = ShopName;
                dataGridViewRow.Cells[3].Value = GameName;
                dataGridViewRow.Cells[4].Value = Date;
                dataGridViewRow.Cells[5].Value = Sum;


                dataGridViewColumn.Width = 150;
                // Остальные значения...

                // Добавляем строку в таблицу
                ReportList.Rows.Add(dataGridViewRow);

            }
        }

        private void ReportList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && !(ReportList.Columns[e.ColumnIndex] is DataGridViewButtonColumn)) // Проверяем, что нажатие не произошло на кнопке
            {
                DataGridViewRow selectedRow = ReportList.Rows[e.RowIndex];
                int ReportNumber = Convert.ToInt32(selectedRow.Cells[0].Value);
                // Вызов вашего метода для обработки нажатия на ячейку
                // Например:
                ShowReportDetails(ReportNumber);
            }

        }

        public void ShowReportDetails(int ReportNumber)
        {
            Deliveries.Visible = false;
            Reports.Visible = false;
            ReportDetails.Visible = true;
            ReportDetails.Size = new Size(960, 400);
            ReportDetails.Location = new Point(20, 30);


            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            SqlCommand getReportDetails = new SqlCommand("SELECT Заявление_на_возврат.Номер_заявления AS [Номер заявления]," +
                "Заявление_на_возврат.Покупка AS [Номер покупки], Магазин.Название_магазина,  Настольная_игра.Название," +
                " Заявление_на_возврат.Дата_отправки_заявления AS [Дата отправки]," +
                "Заявление_на_возврат.Сумма_возврата AS [Сумма возврата], Заявление_на_возврат.Причина_возврата FROM Заявление_на_возврат" +
                " INNER JOIN Покупка ON Покупка.Номер_покупки=Заявление_на_возврат.Покупка " +
                " INNER JOIN Ассортимент ON Покупка.Ассортимент=Ассортимент.Номер_ассортимента" +
                " INNER JOIN Магазин ON Магазин.Код_магазина=Ассортимент.Магазин " +
                " INNER JOIN Настольная_игра ON Настольная_игра.Артикул=Ассортимент.Настольная_игра " +
                " INNER JOIN Заказ ON Покупка.Заказ=Заказ.Номер_заказа " +
                " INNER JOIN Покупатель ON Покупатель.ID_покупателя=Заказ.Заказчик " +
                " WHERE Заявление_на_возврат.Номер_заявления=" + ReportNumber + " ", sqlConnect);
            //sqlCmd.Parameters.AddWithValue("@GameID", GameID);


            using (SqlDataReader sqlReader = getReportDetails.ExecuteReader())
            {
                if (sqlReader.Read())
                {
                    ReportIDLabel.Text = ReportNumber.ToString();
                    PurchaseIDLabel.Text = sqlReader["Номер покупки"].ToString();


                    ReportShopName.Text = sqlReader["Название_магазина"].ToString();
                    ReportGameName.Text = sqlReader["Название"].ToString();
                    ReportDateLabel.Text = sqlReader["Дата отправки"].ToString();
                    ReportReason.Text = sqlReader["Причина_возврата"].ToString();
                    ReportSum.Text = sqlReader["Сумма возврата"].ToString();

                }
            }

        }


        private void ChooseSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            switch (ChooseSort.SelectedIndex)
            {
                case 0:
                    SortPriceUP();

                    break;


                case 1:
                    SortPriceDown();
                    break;

                case 2:
                    SortRatingUP();
                    break;

                case 3:
                    SortRatingDown();
                    break;

            }
        }

        public void SortPriceUP()
        {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();

            // Выполнение SQL-запроса для получения данных
            SqlCommand sqlCmd = new SqlCommand("SELECT Ассортимент.Номер_ассортимента, Настольная_игра.Название, Настольная_игра.Изображение_игры, Ассортимент.Цена, Жанр.Название_жанра FROM Настольная_игра INNER JOIN Жанр ON Настольная_игра.жанр=Жанр.Код_жанра INNER JOIN Ассортимент ON Настольная_игра.Артикул = Ассортимент.Настольная_игра WHERE Ассортимент.Наличие=" + 1 + " ORDER BY Ассортимент.Цена ASC", sqlConnect);
            SqlDataReader sqlReader = sqlCmd.ExecuteReader();

            // Создание FlowLayoutPanel для размещения рамок с информацией
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
            flowLayoutPanel.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel.WrapContents = false;

            // Цикл для создания рамок с информацией о настольных играх
            while (sqlReader.Read())
            {

                string ImagePath = sqlReader["Изображение_игры"].ToString();


                PictureBox pic = new PictureBox();
                Image image = Image.FromFile(ImagePath);
                pic.Image = image;

                pic.SizeMode = PictureBoxSizeMode.Zoom;
                pic.Width = 190;
                pic.Height = 190;
                pic.BorderStyle = BorderStyle.FixedSingle;

                int AssortmentID = Convert.ToInt32(sqlReader["Номер_ассортимента"]);
                pic.Tag = AssortmentID;


                // Add the click event handler to the picture box
                pic.Click += PictureBox_Click;


                Label Name = new Label();
                Name.Text = sqlReader["Название"].ToString();
                Name.BackColor = Color.FromArgb(0, 150, 255);
                Name.TextAlign = ContentAlignment.MiddleCenter;
                Name.Dock = DockStyle.Bottom;


                Label Genre = new Label();

                Genre.Text = sqlReader["Название_жанра"].ToString();
                Genre.AutoSize = true;
                //Name.BackColor = Color.FromArgb(46, 134, 222);
                Genre.TextAlign = ContentAlignment.MiddleLeft;


                Label Price = new Label();
                Price.Text = sqlReader["Цена"].ToString() + "₽";

                Price.TextAlign = ContentAlignment.MiddleCenter;
                Price.Dock = DockStyle.Bottom;


                pic.Controls.Add(Genre);
                pic.Controls.Add(Name);
                pic.Controls.Add(Price);
                flowLayoutPanel1.Controls.Add(pic);

            }

            // Dispose the SqlDataReader and SqlCommand
            sqlReader.Close();
            sqlCmd.Dispose();

            // Close the SqlConnection
            sqlConnect.Close();

        }

        public void SortPriceDown()
        {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();

            // Выполнение SQL-запроса для получения данных
            SqlCommand sqlCmd = new SqlCommand("SELECT Ассортимент.Номер_ассортимента, Настольная_игра.Название, Настольная_игра.Изображение_игры, Ассортимент.Цена, Жанр.Название_жанра FROM Настольная_игра INNER JOIN Жанр ON Настольная_игра.жанр=Жанр.Код_жанра INNER JOIN Ассортимент ON Настольная_игра.Артикул = Ассортимент.Настольная_игра WHERE Ассортимент.Наличие=" + 1 + " ORDER BY Ассортимент.Цена DESC", sqlConnect);
            SqlDataReader sqlReader = sqlCmd.ExecuteReader();

            // Создание FlowLayoutPanel для размещения рамок с информацией
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
            flowLayoutPanel.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel.WrapContents = false;

            // Цикл для создания рамок с информацией о настольных играх
            while (sqlReader.Read())
            {

                string ImagePath = sqlReader["Изображение_игры"].ToString();


                PictureBox pic = new PictureBox();
                Image image = Image.FromFile(ImagePath);
                pic.Image = image;

                pic.SizeMode = PictureBoxSizeMode.Zoom;
                pic.Width = 190;
                pic.Height = 190;
                pic.BorderStyle = BorderStyle.FixedSingle;

                int AssortmentID = Convert.ToInt32(sqlReader["Номер_ассортимента"]);
                pic.Tag = AssortmentID;


                // Add the click event handler to the picture box
                pic.Click += PictureBox_Click;


                Label Name = new Label();
                Name.Text = sqlReader["Название"].ToString();
                Name.BackColor = Color.FromArgb(0, 150, 255);
                Name.TextAlign = ContentAlignment.MiddleCenter;
                Name.Dock = DockStyle.Bottom;


                Label Genre = new Label();

                Genre.Text = sqlReader["Название_жанра"].ToString();
                Genre.AutoSize = true;
                //Name.BackColor = Color.FromArgb(46, 134, 222);
                Genre.TextAlign = ContentAlignment.MiddleLeft;


                Label Price = new Label();
                Price.Text = sqlReader["Цена"].ToString() + "₽";

                Price.TextAlign = ContentAlignment.MiddleCenter;
                Price.Dock = DockStyle.Bottom;


                pic.Controls.Add(Genre);
                pic.Controls.Add(Name);
                pic.Controls.Add(Price);
                flowLayoutPanel1.Controls.Add(pic);

            }

            // Dispose the SqlDataReader and SqlCommand
            sqlReader.Close();
            sqlCmd.Dispose();

            // Close the SqlConnection
            sqlConnect.Close();

        }

        public void SortRatingUP()
        {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();

            // Выполнение SQL-запроса для получения данных
            SqlCommand sqlCmd = new SqlCommand("SELECT Ассортимент.Номер_ассортимента, Настольная_игра.Название, Настольная_игра.Изображение_игры, Ассортимент.Цена, Жанр.Название_жанра FROM Настольная_игра INNER JOIN Жанр ON Настольная_игра.жанр=Жанр.Код_жанра INNER JOIN Ассортимент ON Настольная_игра.Артикул = Ассортимент.Настольная_игра WHERE Ассортимент.Наличие=" + 1 + " ORDER BY Настольная_игра.Рейтинг ASC", sqlConnect);
            SqlDataReader sqlReader = sqlCmd.ExecuteReader();

            // Создание FlowLayoutPanel для размещения рамок с информацией
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
            flowLayoutPanel.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel.WrapContents = false;

            // Цикл для создания рамок с информацией о настольных играх
            while (sqlReader.Read())
            {

                string ImagePath = sqlReader["Изображение_игры"].ToString();


                PictureBox pic = new PictureBox();
                Image image = Image.FromFile(ImagePath);
                pic.Image = image;

                pic.SizeMode = PictureBoxSizeMode.Zoom;
                pic.Width = 190;
                pic.Height = 190;
                pic.BorderStyle = BorderStyle.FixedSingle;

                int AssortmentID = Convert.ToInt32(sqlReader["Номер_ассортимента"]);
                pic.Tag = AssortmentID;


                // Add the click event handler to the picture box
                pic.Click += PictureBox_Click;


                Label Name = new Label();
                Name.Text = sqlReader["Название"].ToString();
                Name.BackColor = Color.FromArgb(0, 150, 255);
                Name.TextAlign = ContentAlignment.MiddleCenter;
                Name.Dock = DockStyle.Bottom;


                Label Genre = new Label();

                Genre.Text = sqlReader["Название_жанра"].ToString();
                Genre.AutoSize = true;
                //Name.BackColor = Color.FromArgb(46, 134, 222);
                Genre.TextAlign = ContentAlignment.MiddleLeft;


                Label Price = new Label();
                Price.Text = sqlReader["Цена"].ToString() + "₽";

                Price.TextAlign = ContentAlignment.MiddleCenter;
                Price.Dock = DockStyle.Bottom;


                pic.Controls.Add(Genre);
                pic.Controls.Add(Name);
                pic.Controls.Add(Price);
                flowLayoutPanel1.Controls.Add(pic);

            }

            // Dispose the SqlDataReader and SqlCommand
            sqlReader.Close();
            sqlCmd.Dispose();

            // Close the SqlConnection
            sqlConnect.Close();
        }

        public void SortRatingDown()
        {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();

            // Выполнение SQL-запроса для получения данных
            SqlCommand sqlCmd = new SqlCommand("SELECT Ассортимент.Номер_ассортимента, Настольная_игра.Название, Настольная_игра.Изображение_игры, Ассортимент.Цена, Жанр.Название_жанра FROM Настольная_игра INNER JOIN Жанр ON Настольная_игра.жанр=Жанр.Код_жанра INNER JOIN Ассортимент ON Настольная_игра.Артикул = Ассортимент.Настольная_игра WHERE Ассортимент.Наличие=" + 1 + " ORDER BY Настольная_игра.Рейтинг DESC", sqlConnect);
            SqlDataReader sqlReader = sqlCmd.ExecuteReader();

            // Создание FlowLayoutPanel для размещения рамок с информацией
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
            flowLayoutPanel.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel.WrapContents = false;

            // Цикл для создания рамок с информацией о настольных играх
            while (sqlReader.Read())
            {

                string ImagePath = sqlReader["Изображение_игры"].ToString();


                PictureBox pic = new PictureBox();
                Image image = Image.FromFile(ImagePath);
                pic.Image = image;

                pic.SizeMode = PictureBoxSizeMode.Zoom;
                pic.Width = 190;
                pic.Height = 190;
                pic.BorderStyle = BorderStyle.FixedSingle;

                int AssortmentID = Convert.ToInt32(sqlReader["Номер_ассортимента"]);
                pic.Tag = AssortmentID;


                // Add the click event handler to the picture box
                pic.Click += PictureBox_Click;


                Label Name = new Label();
                Name.Text = sqlReader["Название"].ToString();
                Name.BackColor = Color.FromArgb(0, 150, 255);
                Name.TextAlign = ContentAlignment.MiddleCenter;
                Name.Dock = DockStyle.Bottom;


                Label Genre = new Label();

                Genre.Text = sqlReader["Название_жанра"].ToString();
                Genre.AutoSize = true;
                //Name.BackColor = Color.FromArgb(46, 134, 222);
                Genre.TextAlign = ContentAlignment.MiddleLeft;


                Label Price = new Label();
                Price.Text = sqlReader["Цена"].ToString() + "₽";

                Price.TextAlign = ContentAlignment.MiddleCenter;
                Price.Dock = DockStyle.Bottom;


                pic.Controls.Add(Genre);
                pic.Controls.Add(Name);
                pic.Controls.Add(Price);
                flowLayoutPanel1.Controls.Add(pic);

            }

            // Dispose the SqlDataReader and SqlCommand
            sqlReader.Close();
            sqlCmd.Dispose();

            // Close the SqlConnection
            sqlConnect.Close();
        }

        public void ShopSortPriceUP(int ShopID)
        {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();


            // Выполнение SQL-запроса для получения данных
            SqlCommand sqlCmd = new SqlCommand("SELECT Ассортимент.Номер_ассортимента, Настольная_игра.Название, Настольная_игра.Изображение_игры, Ассортимент.Цена, Жанр.Название_жанра FROM Настольная_игра" +
                " INNER JOIN Жанр ON Настольная_игра.жанр=Жанр.Код_жанра" +
                " INNER JOIN Ассортимент ON Настольная_игра.Артикул = Ассортимент.Настольная_игра " +
                " INNER JOIN Магазин ON Магазин.Код_магазина=Ассортимент.Магазин" +
                " WHERE Ассортимент.Наличие=" + 1 + " AND Магазин.Код_магазина=" + ShopID + " ORDER BY Ассортимент.Цена ASC ", sqlConnect);
            SqlDataReader sqlReader = sqlCmd.ExecuteReader();

            // Создание FlowLayoutPanel для размещения рамок с информацией
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
            flowLayoutPanel.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel.WrapContents = false;

            // Цикл для создания рамок с информацией о настольных играх
            while (sqlReader.Read())
            {

                string ImagePath = sqlReader["Изображение_игры"].ToString();


                PictureBox pic = new PictureBox();
                Image image = Image.FromFile(ImagePath);
                pic.Image = image;

                pic.SizeMode = PictureBoxSizeMode.Zoom;
                pic.Width = 190;
                pic.Height = 190;
                pic.BorderStyle = BorderStyle.FixedSingle;

                int AssortmentID = Convert.ToInt32(sqlReader["Номер_ассортимента"]);
                pic.Tag = AssortmentID;


                // Add the click event handler to the picture box
                pic.Click += PictureBox_Click;


                Label Name = new Label();
                Name.Text = sqlReader["Название"].ToString();
                Name.BackColor = Color.FromArgb(0, 150, 255);
                Name.TextAlign = ContentAlignment.MiddleCenter;
                Name.Dock = DockStyle.Bottom;


                Label Genre = new Label();

                Genre.Text = sqlReader["Название_жанра"].ToString();
                Genre.AutoSize = true;
                //Name.BackColor = Color.FromArgb(46, 134, 222);
                Genre.TextAlign = ContentAlignment.MiddleLeft;


                Label Price = new Label();
                Price.Text = sqlReader["Цена"].ToString() + "₽";

                Price.TextAlign = ContentAlignment.MiddleCenter;
                Price.Dock = DockStyle.Bottom;


                pic.Controls.Add(Genre);
                pic.Controls.Add(Name);
                pic.Controls.Add(Price);
                flowLayoutPanel2.Controls.Add(pic);

            }

            // Dispose the SqlDataReader and SqlCommand
            sqlReader.Close();
            sqlCmd.Dispose();

            // Close the SqlConnection
            sqlConnect.Close();
        }

        public void ShopSortPriceDown(int ShopID)
        {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();


            // Выполнение SQL-запроса для получения данных
            SqlCommand sqlCmd = new SqlCommand("SELECT Ассортимент.Номер_ассортимента, Настольная_игра.Название, Настольная_игра.Изображение_игры, Ассортимент.Цена, Жанр.Название_жанра FROM Настольная_игра" +
                " INNER JOIN Жанр ON Настольная_игра.жанр=Жанр.Код_жанра" +
                " INNER JOIN Ассортимент ON Настольная_игра.Артикул = Ассортимент.Настольная_игра " +
                " INNER JOIN Магазин ON Магазин.Код_магазина=Ассортимент.Магазин" +
                " WHERE Ассортимент.Наличие=" + 1 + " AND Магазин.Код_магазина=" + ShopID + " ORDER BY Ассортимент.Цена DESC ", sqlConnect);
            SqlDataReader sqlReader = sqlCmd.ExecuteReader();

            // Создание FlowLayoutPanel для размещения рамок с информацией
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
            flowLayoutPanel.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel.WrapContents = false;

            // Цикл для создания рамок с информацией о настольных играх
            while (sqlReader.Read())
            {

                string ImagePath = sqlReader["Изображение_игры"].ToString();


                PictureBox pic = new PictureBox();
                Image image = Image.FromFile(ImagePath);
                pic.Image = image;

                pic.SizeMode = PictureBoxSizeMode.Zoom;
                pic.Width = 190;
                pic.Height = 190;
                pic.BorderStyle = BorderStyle.FixedSingle;

                int AssortmentID = Convert.ToInt32(sqlReader["Номер_ассортимента"]);
                pic.Tag = AssortmentID;


                // Add the click event handler to the picture box
                pic.Click += PictureBox_Click;


                Label Name = new Label();
                Name.Text = sqlReader["Название"].ToString();
                Name.BackColor = Color.FromArgb(0, 150, 255);
                Name.TextAlign = ContentAlignment.MiddleCenter;
                Name.Dock = DockStyle.Bottom;


                Label Genre = new Label();

                Genre.Text = sqlReader["Название_жанра"].ToString();
                Genre.AutoSize = true;
                //Name.BackColor = Color.FromArgb(46, 134, 222);
                Genre.TextAlign = ContentAlignment.MiddleLeft;


                Label Price = new Label();
                Price.Text = sqlReader["Цена"].ToString() + "₽";

                Price.TextAlign = ContentAlignment.MiddleCenter;
                Price.Dock = DockStyle.Bottom;


                pic.Controls.Add(Genre);
                pic.Controls.Add(Name);
                pic.Controls.Add(Price);
                flowLayoutPanel2.Controls.Add(pic);

            }

            // Dispose the SqlDataReader and SqlCommand
            sqlReader.Close();
            sqlCmd.Dispose();

            // Close the SqlConnection
            sqlConnect.Close();
        }

        public void ShopSortRatingUp(int ShopID)
        {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();


            // Выполнение SQL-запроса для получения данных
            SqlCommand sqlCmd = new SqlCommand("SELECT Ассортимент.Номер_ассортимента, Настольная_игра.Название, Настольная_игра.Изображение_игры, Ассортимент.Цена, Жанр.Название_жанра FROM Настольная_игра" +
                " INNER JOIN Жанр ON Настольная_игра.жанр=Жанр.Код_жанра" +
                " INNER JOIN Ассортимент ON Настольная_игра.Артикул = Ассортимент.Настольная_игра " +
                " INNER JOIN Магазин ON Магазин.Код_магазина=Ассортимент.Магазин" +
                " WHERE Ассортимент.Наличие=" + 1 + " AND Магазин.Код_магазина=" + ShopID + " ORDER BY Настольная_игра.Рейтинг ASC ", sqlConnect);
            SqlDataReader sqlReader = sqlCmd.ExecuteReader();

            // Создание FlowLayoutPanel для размещения рамок с информацией
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
            flowLayoutPanel.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel.WrapContents = false;

            // Цикл для создания рамок с информацией о настольных играх
            while (sqlReader.Read())
            {

                string ImagePath = sqlReader["Изображение_игры"].ToString();


                PictureBox pic = new PictureBox();
                Image image = Image.FromFile(ImagePath);
                pic.Image = image;

                pic.SizeMode = PictureBoxSizeMode.Zoom;
                pic.Width = 190;
                pic.Height = 190;
                pic.BorderStyle = BorderStyle.FixedSingle;

                int AssortmentID = Convert.ToInt32(sqlReader["Номер_ассортимента"]);
                pic.Tag = AssortmentID;


                // Add the click event handler to the picture box
                pic.Click += PictureBox_Click;


                Label Name = new Label();
                Name.Text = sqlReader["Название"].ToString();
                Name.BackColor = Color.FromArgb(0, 150, 255);
                Name.TextAlign = ContentAlignment.MiddleCenter;
                Name.Dock = DockStyle.Bottom;


                Label Genre = new Label();

                Genre.Text = sqlReader["Название_жанра"].ToString();
                Genre.AutoSize = true;
                //Name.BackColor = Color.FromArgb(46, 134, 222);
                Genre.TextAlign = ContentAlignment.MiddleLeft;


                Label Price = new Label();
                Price.Text = sqlReader["Цена"].ToString() + "₽";

                Price.TextAlign = ContentAlignment.MiddleCenter;
                Price.Dock = DockStyle.Bottom;


                pic.Controls.Add(Genre);
                pic.Controls.Add(Name);
                pic.Controls.Add(Price);
                flowLayoutPanel2.Controls.Add(pic);

            }

            // Dispose the SqlDataReader and SqlCommand
            sqlReader.Close();
            sqlCmd.Dispose();

            // Close the SqlConnection
            sqlConnect.Close();
        }

        public void ShopSortRatingDown(int ShopID)
        {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();


            // Выполнение SQL-запроса для получения данных
            SqlCommand sqlCmd = new SqlCommand("SELECT Ассортимент.Номер_ассортимента, Настольная_игра.Название, Настольная_игра.Изображение_игры, Ассортимент.Цена, Жанр.Название_жанра FROM Настольная_игра" +
                " INNER JOIN Жанр ON Настольная_игра.жанр=Жанр.Код_жанра" +
                " INNER JOIN Ассортимент ON Настольная_игра.Артикул = Ассортимент.Настольная_игра " +
                " INNER JOIN Магазин ON Магазин.Код_магазина=Ассортимент.Магазин" +
                " WHERE Ассортимент.Наличие=" + 1 + " AND Магазин.Код_магазина=" + ShopID + " ORDER BY Настольная_игра.Рейтинг DESC ", sqlConnect);
            SqlDataReader sqlReader = sqlCmd.ExecuteReader();

            // Создание FlowLayoutPanel для размещения рамок с информацией
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
            flowLayoutPanel.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel.WrapContents = false;

            // Цикл для создания рамок с информацией о настольных играх
            while (sqlReader.Read())
            {

                string ImagePath = sqlReader["Изображение_игры"].ToString();


                PictureBox pic = new PictureBox();
                Image image = Image.FromFile(ImagePath);
                pic.Image = image;

                pic.SizeMode = PictureBoxSizeMode.Zoom;
                pic.Width = 190;
                pic.Height = 190;
                pic.BorderStyle = BorderStyle.FixedSingle;

                int AssortmentID = Convert.ToInt32(sqlReader["Номер_ассортимента"]);
                pic.Tag = AssortmentID;


                // Add the click event handler to the picture box
                pic.Click += PictureBox_Click;


                Label Name = new Label();
                Name.Text = sqlReader["Название"].ToString();
                Name.BackColor = Color.FromArgb(0, 150, 255);
                Name.TextAlign = ContentAlignment.MiddleCenter;
                Name.Dock = DockStyle.Bottom;


                Label Genre = new Label();

                Genre.Text = sqlReader["Название_жанра"].ToString();
                Genre.AutoSize = true;
                //Name.BackColor = Color.FromArgb(46, 134, 222);
                Genre.TextAlign = ContentAlignment.MiddleLeft;


                Label Price = new Label();
                Price.Text = sqlReader["Цена"].ToString() + "₽";

                Price.TextAlign = ContentAlignment.MiddleCenter;
                Price.Dock = DockStyle.Bottom;


                pic.Controls.Add(Genre);
                pic.Controls.Add(Name);
                pic.Controls.Add(Price);
                flowLayoutPanel2.Controls.Add(pic);

            }

            // Dispose the SqlDataReader and SqlCommand
            sqlReader.Close();
            sqlCmd.Dispose();

            // Close the SqlConnection
            sqlConnect.Close();
        }

        private void ShopSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ShopID = Convert.ToInt32(label17.Text);
            flowLayoutPanel2.Controls.Clear();
            switch (ShopSort.SelectedIndex)
            {
                case 0:
                    ShopSortPriceUP(ShopID);

                    break;


                case 1:
                    ShopSortPriceDown(ShopID);
                    break;

                case 2:
                    ShopSortRatingUp(ShopID);
                    break;

                case 3:
                    ShopSortRatingDown(ShopID);
                    break;

            }
        }




        private void CarButton_Click(object sender, EventArgs e)
        {

            CartPurchase.Rows.Clear();

            // Создайть необходимые столбцы в контейнере
            DeiiveryDetails.Visible = false;
            OrderDetails.Visible = false;
            CompletedOrderDetails.Visible = false;
            OrderCreating.Visible = false;
            Orders.Visible = false;
            ReportInserting.Visible = false;
            ReportDetails.Visible = false;
            ShopListBox.Visible = false;
            Deliveries.Visible = false;
            Assortment.Visible = false;
            Reports.Visible = false;
            CompletedOrders.Visible = false;
            ShopAssortment.Visible = false;
            ProductPage.Visible = false;
            OrderCreating.Visible = false;
            CartPurchase.Columns.Clear();
            Profile.Visible = false;
            CarShow();
            CartBox.Visible = true;
            CartBox.Location = new Point(20, 30);
            CartBox.Size = new Size(1200, 400);
            Payment.Text = sum.ToString() + "₽";
        }

        private List<int> purchasedGames = new List<int>();
        private HashSet<int> Assorments = new HashSet<int>();
        Dictionary<int, int> gameCopies = new Dictionary<int, int>();

        public void CarShow()
        {
            // Очистить контейнер для отображения данных корзины (например, DataGridView)
            CartPurchase.Rows.Clear();

            // Создайть необходимые столбцы в контейнере
            CartPurchase.Columns.Clear();
            // Создаем столбец для отображения картинок
            DataGridViewColumn IdColumn = new DataGridViewTextBoxColumn();
            IdColumn.HeaderText = "Ассортимент";
            IdColumn.DataPropertyName = "Ассортимент";
            CartPurchase.Columns.Add(IdColumn);


            // Создаем столбец для отображения картинок
            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
            imageColumn.HeaderText = "Изображение";
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            imageColumn.DataPropertyName = "Изображение_игры";
            CartPurchase.Columns.Add(imageColumn);

            DataGridViewColumn nameColumn = new DataGridViewTextBoxColumn();
            nameColumn.HeaderText = "Название";
            nameColumn.DataPropertyName = "Название";
            CartPurchase.Columns.Add(nameColumn);




            DataGridViewColumn ShopColumn = new DataGridViewTextBoxColumn();
            ShopColumn.HeaderText = "Магазин";
            ShopColumn.DataPropertyName = "Название_магазина";
            CartPurchase.Columns.Add(ShopColumn);

            DataGridViewColumn PriceColumn = new DataGridViewTextBoxColumn();
            PriceColumn.HeaderText = "Цена";
            PriceColumn.DataPropertyName = "Цена";
            CartPurchase.Columns.Add(PriceColumn);


            // Создаем столбец "Количество"
            DataGridViewTextBoxColumn quantityColumn = new DataGridViewTextBoxColumn();
            quantityColumn.HeaderText = "Количество";
            quantityColumn.DataPropertyName = "Количество";
            CartPurchase.Columns.Add(quantityColumn);

            // Создаем столбец "Добавить
            DataGridViewButtonColumn AddColumn = new DataGridViewButtonColumn();
            AddColumn.HeaderText = "Добавить";
            AddColumn.Text = "Действие добавления";
            AddColumn.UseColumnTextForButtonValue = true;
            CartPurchase.Columns.Add(AddColumn);


            // Создаем столбец "Уменьшить"
            DataGridViewButtonColumn ReduceColumn = new DataGridViewButtonColumn();
            ReduceColumn.HeaderText = "Уменьшить";
            ReduceColumn.Text = "Действие уменьшения";
            ReduceColumn.UseColumnTextForButtonValue = true;
            CartPurchase.Columns.Add(ReduceColumn);

            // Создаем столбец "Удалить"
            DataGridViewButtonColumn RemoveColumn = new DataGridViewButtonColumn();
            RemoveColumn.HeaderText = "Удалить";
            RemoveColumn.Text = "Действие удаления";
            RemoveColumn.UseColumnTextForButtonValue = true;
            CartPurchase.Columns.Add(RemoveColumn);




            foreach (int AssortmentID in Assorments)
            {

                // Выполните запрос к базе данных, чтобы получить информацию о выбранной игре
                using (SqlConnection sqlConnect = new SqlConnection(connection))
                {
                    sqlConnect.Open();

                    SqlCommand gameInfo = new SqlCommand("SELECT TOP 1 Ассортимент.Номер_ассортимента, Настольная_игра.Изображение_игры, Настольная_игра.Название, Магазин.Название_магазина, Ассортимент.Цена " +
                                                "FROM Ассортимент " +
                                                "INNER JOIN Настольная_игра ON Ассортимент.Настольная_игра = Настольная_игра.Артикул " +
                                                "INNER JOIN Магазин ON Ассортимент.Магазин = Магазин.Код_магазина " +
                                                "WHERE Ассортимент.Номер_ассортимента = @AssortmentID", sqlConnect);
                    gameInfo.Parameters.AddWithValue("@AssortmentID", AssortmentID);


                    // Создаем адаптер данных для заполнения DataSet
                    SqlDataAdapter adapter = new SqlDataAdapter(gameInfo);

                    // Создаем DataSet для хранения данных
                    DataSet dataSet = new DataSet();

                    // Заполняем DataSet данными из таблицы
                    adapter.Fill(dataSet);

                    CartPurchase.AutoGenerateColumns = false;
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        // Получаем данные из строки

                        string ID = row["Номер_ассортимента"].ToString();
                        string imagePath = row["Изображение_игры"].ToString();
                        string name = row["Название"].ToString();

                        string Shop = row["Название_магазина"].ToString();
                        string Price = row["Цена"].ToString();

                        int quantity = gameCopies[AssortmentID]; // Получаем количество копий из словаря
                                                                 // Загружаем изображение из пути
                        Image image = Image.FromFile(imagePath);

                        // Создаем строку и заполняем ее данными
                        DataGridViewRow dataGridViewRow = new DataGridViewRow();
                        DataGridViewColumn dataGridViewColumn = new DataGridViewColumn();

                        dataGridViewRow.CreateCells(CartPurchase);


                        dataGridViewRow.Cells[0].Value = ID;
                        dataGridViewRow.Cells[1].Value = image;
                        dataGridViewRow.Cells[2].Value = name;
                        dataGridViewRow.Cells[3].Value = Shop;
                        dataGridViewRow.Cells[4].Value = Price;
                        dataGridViewRow.Cells[5].Value = quantity;
                        DataGridViewButtonCell plusButtonCell = new DataGridViewButtonCell();
                        plusButtonCell.Value = "+";
                        plusButtonCell.FlatStyle = FlatStyle.Flat;
                        plusButtonCell.Style.BackColor = Color.White;
                        dataGridViewRow.Cells[6] = plusButtonCell;
                        DataGridViewButtonCell minusButtonCell = new DataGridViewButtonCell();
                        minusButtonCell.Value = "-";
                        minusButtonCell.FlatStyle = FlatStyle.Flat;
                        minusButtonCell.Style.BackColor = Color.White;
                        dataGridViewRow.Cells[7] = minusButtonCell;
                        DataGridViewButtonCell RemoveButtonCell = new DataGridViewButtonCell();
                        RemoveButtonCell.Value = "Убрать";
                        RemoveButtonCell.FlatStyle = FlatStyle.Flat;
                        RemoveButtonCell.Style.BackColor = Color.Red;
                        dataGridViewRow.Cells[8] = RemoveButtonCell;





                        dataGridViewRow.Height = 200;
                        dataGridViewColumn.Width = 150;
                        // Остальные значения...

                        // Добавляем строку в таблицу
                        CartPurchase.Rows.Add(dataGridViewRow);

                    }
                    // Закрыть соединение с базой данных
                    sqlConnect.Close();
                }

            }



        }
        private void CartPurchase_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = CartPurchase.Rows[e.RowIndex];
                int assortmentID = Convert.ToInt32(selectedRow.Cells[0].Value);

                if (e.ColumnIndex == 6) // Клик на кнопке "+"
                {

                    // Увеличиваем количество копий игры
                    gameCopies[assortmentID]++;
                    int quantity = gameCopies[assortmentID];
                    purchasedGames.Add(assortmentID);
                    // Assorments.Add(assortmentID);
                    count = count + 1;
                    label10.Text = count.ToString();
                    using (SqlConnection sqlConnect = new SqlConnection(connection))
                    {
                        sqlConnect.Open();
                        SqlCommand getPriceCmd = new SqlCommand("SELECT Цена FROM Ассортимент WHERE Номер_ассортимента=@AssortmentID", sqlConnect);
                        getPriceCmd.Parameters.AddWithValue("@AssortmentID", assortmentID);
                        float price = Convert.ToSingle(getPriceCmd.ExecuteScalar());
                        sum += price;
                        Payment.Text = sum.ToString() + "₽";
                    }
                    // Обновляем значение в соответствующей ячейке
                    selectedRow.Cells[5].Value = quantity;
                    selectedRow.Cells[5].Value = gameCopies[assortmentID];

                }
                else if (e.ColumnIndex == 7) // Клик на кнопке "-"
                {

                    // Создаем столбец для отображения картинок

                    gameCopies[assortmentID]--;
                    int quantity = gameCopies[assortmentID];

                    int index = purchasedGames.IndexOf(assortmentID); // Находим индекс первого элемента с указанным номером ассортимента

                    if (index != -1)
                    {
                        purchasedGames.RemoveAt(index); // Удаляем элемент по индексу
                    }
                    if (quantity < 0)
                    {

                        gameCopies[assortmentID] = 0;
                        quantity = 0;
                    }

                    using (SqlConnection sqlConnect = new SqlConnection(connection))
                    {
                        sqlConnect.Open();
                        SqlCommand getPriceCmd = new SqlCommand("SELECT Цена FROM Ассортимент WHERE Номер_ассортимента=@AssortmentID", sqlConnect);
                        getPriceCmd.Parameters.AddWithValue("@AssortmentID", assortmentID);
                        float price = Convert.ToSingle(getPriceCmd.ExecuteScalar());
                        sum -= price;
                        Payment.Text = sum.ToString() + "₽";

                    }
                    count = count - 1;
                    label10.Text = count.ToString();
                    // Обновляем значение в соответствующей ячейке
                    selectedRow.Cells[5].Value = quantity;
                    selectedRow.Cells[5].Value = gameCopies[assortmentID];

                }

                else if (e.ColumnIndex == 8) // Клик на кнопке "Убрать"
                {

                    // Создаем столбец для отображения картинок


                    int quantity = gameCopies[assortmentID];
                    count = count - quantity;
                    label10.Text = count.ToString();
                    using (SqlConnection sqlConnect = new SqlConnection(connection))
                    {
                        sqlConnect.Open();
                        SqlCommand getPriceCmd = new SqlCommand("SELECT Цена FROM Ассортимент WHERE Номер_ассортимента=@AssortmentID", sqlConnect);
                        getPriceCmd.Parameters.AddWithValue("@AssortmentID", assortmentID);
                        float price = Convert.ToSingle(getPriceCmd.ExecuteScalar());
                        sum = sum - price * quantity;
                        Payment.Text = sum.ToString() + "₽";

                    }


                    // Удаляем запись из таблицы
                    CartPurchase.Rows.Remove(selectedRow);

                    // Удаляем ID ассортимента из HashSet и списков
                    Assorments.Remove(assortmentID);
                    purchasedGames.Remove(assortmentID);
                    gameCopies.Remove(assortmentID);
                }


            }
        }

        private void KeepShopping_Click(object sender, EventArgs e)
        {
            // Очистить контейнер для отображения данных корзины (например, DataGridView)
            CartPurchase.Rows.Clear();

            // Создайть необходимые столбцы в контейнере
            CartPurchase.Columns.Clear();
            ProductPage.Visible = false;
            Profile.Visible = false;
            ShopListBox.Visible = false;
            ShopAssortment.Visible = false;
            CartBox.Visible = false;
            Assortment.Visible = true;
            Assortment.Size = new Size(650, 550);
            Assortment.Location = new Point(20, 30);
            ShowAssortment();
        }

        private void CleanCart_Click(object sender, EventArgs e)
        {
            CartPurchase.Rows.Clear(); // Очищаем все строки таблицы
            Assorments.Clear(); // Очищаем HashSet с ID ассортиментов
            purchasedGames.Clear(); // Очищаем список с играми в корзине
            gameCopies.Clear(); // Очищаем словарь с количеством копий игр
            count = 0;
            label10.Text = count.ToString();
            sum = 0;
            Payment.Text = sum.ToString();
        }




        public void CreateOrderShow()
        {

            PointDataLabel.Visible = false;
            label22.Visible = false;
            SumPayment.Text = sum.ToString() + "₽";
            CartBox.Visible = false;
            OrderCreating.Visible = true;
            OrderCreating.Location = new Point(20, 30);
            OrderCreating.Size = new Size(830, 500);
        }

        private void GoCreateOrder_Click(object sender, EventArgs e)
        {
            if ((purchasedGames.Count == 0) && (Assorments.Count == 0))
            {
                MessageBox.Show("Ошибка: Вы не выбрали ниодного товара!", "Ошибка создания заказа", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                CreateOrderShow();
                FillPointData();
            }

        }

        private void CreateOrder_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection sqlConnect = new SqlConnection(connection);
                sqlConnect.Open();
                if ((Street.Text == "") && (House.Text == ""))
                {
                    MessageBox.Show("Ошибка: Вы не заполнили адрес тоставки!", "Ошибка создания заказа", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    DataRowView selectedPaymentRow = (DataRowView)PaymentWay.SelectedItem;
                    int WayOfPaymentID = Convert.ToInt32(selectedPaymentRow["Код_способа_оплаты"]);

                    DataRowView selectedGettingrRow = (DataRowView)WayOfGetting.SelectedItem;
                    int WayOfGettingID = Convert.ToInt32(selectedGettingrRow["Код_способа_получения"]);

                    int PointID = Convert.ToInt32(Point.SelectedIndex) + 1;

                    SqlCommand PointType = new SqlCommand(@"Select Тип_пункта_назначения FROM Пункт_назначения WHERE Код_пункта_назначения=" + PointID + " ", sqlConnect);
                    object result1 = PointType.ExecuteScalar();
                    int PointTypeID = Convert.ToInt32(result1);
                    MessageBox.Show("Тип адреса" + PointTypeID);
                    SqlCommand sqlMaxOrderID = new SqlCommand(@"Select MAX(Номер_заказа) FROM [dbo].[Заказ]", sqlConnect);
                    object result2 = sqlMaxOrderID.ExecuteScalar();
                    int OrderID = Convert.ToInt32(result2) + 1;
                    if (PointTypeID == 1)
                    {
                        SqlCommand InsertOrder = new SqlCommand("INSERT INTO Заказ (Номер_заказа, Заказчик, Сумма_оплаты, Способ_оплаты, Способ_получения, Адрес_получения, Дата_заказа, Дата_выдачи, Статус_заказа) " +
                            "VALUES ('" + OrderID + "', '" + ClientID + "', '" + sum + "', '" + WayOfPaymentID + "', '" + WayOfGettingID + "', '" + PointID + "', '" + DateTime.Now + "', NULL, '" + 1 + "'  ) ", sqlConnect);
                        InsertOrder.ExecuteNonQuery();

                        SqlCommand GetMaxPurchaseID = new SqlCommand("SELECT MAX(Номер_покупки) FROM Покупка ", sqlConnect);
                        object result3 = GetMaxPurchaseID.ExecuteScalar();
                        int PurchaseID = Convert.ToInt32(result3) + 1;// Получаем последний номер покупки из таблицы "Покупка"



                        // Увеличиваем последний номер покупки на 1 для следующей покупки

                        // Перебираем список purchasedGames
                        foreach (int assortmentID in purchasedGames)
                        {
                            // Вставляем каждый номер ассортимента в таблицу "Покупка"
                            string insertQuery = "INSERT INTO Покупка (Номер_покупки, Заказ, Ассортимент, Цена_покупки) " +
                                                 "VALUES (" + PurchaseID + ", " + OrderID + ", " + assortmentID + ", NULL)";

                            SqlCommand insertCommand = new SqlCommand(insertQuery, sqlConnect);

                            insertCommand.ExecuteNonQuery();

                            // Увеличиваем номер покупки для следующей итерации цикла
                            PurchaseID++;
                        }
                        MessageBox.Show("Заказ успешно оформлен", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        purchasedGames.Clear();
                        Assorments.Clear();
                        gameCopies.Clear();
                        sum = 0;
                        count = 0;
                        label10.Text = "0";
                        OrderCreating.Visible = false;
                        ShowProfileData(UserID, ClientID);

                    }
                    else if (WayOfGettingID == 2)
                    {
                        SqlCommand MaxAddressID = new SqlCommand(@"Select MAX(Код_пункта_назначения) FROM Пункт_назначения", sqlConnect);
                        object result4 = MaxAddressID.ExecuteScalar();
                        int AddressID = Convert.ToInt32(result4) + 1;
                        SqlCommand InsertDestination = new SqlCommand("INSERT INTO Пункт_назначения (Код_пункта_назначения, Улица, Дом, Срок_хранения, Начало_работы, Конец_работы, Тип_пункта_назначения) " +
                            "VALUES('" + AddressID + "', '" + Street.Text + "', '" + House.Text + "', NULL, NULL, NULL, '" + 2 + "')", sqlConnect);
                        InsertDestination.ExecuteNonQuery();
                        SqlCommand InsertOrder = new SqlCommand("INSERT INTO Заказ (Номер_заказа, Заказчик, Сумма_оплаты, Способ_оплаты, Способ_получения, Адрес_получения, Дата_заказа, Дата_выдачи, Статус_заказа) " +
                           "VALUES ('" + OrderID + "', '" + ClientID + "', '" + sum + "', '" + WayOfPaymentID + "', '" + WayOfGettingID + "', '" + AddressID + "', '" + DateTime.Now + "', NULL, '" + 1 + "'  ) ", sqlConnect);
                        InsertOrder.ExecuteNonQuery();

                        SqlCommand GetMaxPurchaseID = new SqlCommand("SELECT MAX(Номер_покупки) FROM Покупка", sqlConnect);
                        object result3 = GetMaxPurchaseID.ExecuteScalar();
                        int PurchaseID = Convert.ToInt32(result3) + 1;// Получаем последний номер покупки из таблицы "Покупка"

                        // Перебираем список purchasedGames
                        foreach (int assortmentID in purchasedGames)
                        {
                            // Вставляем каждый номер ассортимента в таблицу "Покупка"
                            string insertQuery = "INSERT INTO Покупка (Номер_покупки, Заказ, Ассортимент, Цена_покупки) " +
                                                 "VALUES (" + PurchaseID + ", " + OrderID + ", " + assortmentID + ", NULL)";

                            SqlCommand insertCommand = new SqlCommand(insertQuery, sqlConnect);

                            insertCommand.ExecuteNonQuery();

                            // Увеличиваем номер покупки для следующей итерации цикла
                            PurchaseID++;
                        }
                        MessageBox.Show("Заказ успешно оформлен", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        purchasedGames.Clear();
                        Assorments.Clear();
                        gameCopies.Clear();
                        sum = 0;
                        count = 0;
                        label10.Text = "0";
                        OrderCreating.Visible = false;
                        ShowProfileData(UserID, ClientID);

                    }
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }





        }

        private void CancelOrder_Click(object sender, EventArgs e)
        {
            ReportDetails.Visible = false;

            CartPurchase.Rows.Clear();

            // Создайть необходимые столбцы в контейнере
            CartPurchase.Columns.Clear();
            ShopAssortment.Visible = false;
            Assortment.Visible = false;
            ProductPage.Visible = false;
            CarShow();
            CartBox.Visible = true;
            CartBox.Location = new Point(20, 30);
            CartBox.Size = new Size(1200, 400);
            Payment.Text = sum.ToString() + "₽";
        }

        private void WayOfGetting_SelectedIndexChanged(object sender, EventArgs e)
        {

            switch (WayOfGetting.SelectedIndex)
            {
                case 0:
                    label23.Visible = true;
                    Point.Visible = true;
                    label22.Visible = false;
                    PointDataLabel.Visible = true;
                    PointDataLabel.Location = new Point(10, 100);
                    Street.Visible = true;
                    Street.Enabled = false;
                    StreetLabel.Visible = true;
                    House.Visible = true;
                    House.Enabled = false;
                    HouseLabel.Visible = true;
                    StoragePeriod.Visible = true;
                    PeriodLabel.Visible = true;
                    WorkBeginTime.Visible = true;
                    WorkBeginLabel.Visible = true;
                    WorkEndTime.Visible = true;
                    EndWorkLabel.Visible = true;
                    break;


                case 1:
                    label23.Visible = false;
                    Point.Visible = false;
                    label22.Visible = true;
                    label22.Location = new Point(10, 100);
                    PointDataLabel.Visible = false;
                    Street.Visible = true;
                    Street.Enabled = true;
                    StreetLabel.Visible = true;
                    House.Visible = true;
                    House.Enabled = true;
                    HouseLabel.Visible = true;
                    StoragePeriod.Visible = false;
                    PeriodLabel.Visible = false;
                    WorkBeginTime.Visible = false;
                    WorkBeginLabel.Visible = false;
                    WorkEndTime.Visible = false;
                    WorkBeginLabel.Visible = false;
                    EndWorkLabel.Visible = false;
                    break;
            }
        }


        private void Point_SelectedIndexChanged(object sender, EventArgs e)
        {

            int SelectedPointNumber = Convert.ToInt32(Point.SelectedIndex) + 1;



            using (SqlConnection sqlConnect = new SqlConnection(connection))
            {
                sqlConnect.Open();

                // Получить номер пункта выдачи


                SqlCommand sqlView = new SqlCommand("SELECT Пункт_назначения.Код_пункта_назначения, Пункт_назначения.Улица, Пункт_назначения.Дом, Пункт_назначения.Срок_хранения, Пункт_назначения.Начало_работы, Пункт_назначения.Конец_работы" +
                    " FROM [dbo].[Пункт_назначения] WHERE Тип_пункта_назначения=" + 1 + " AND Код_пункта_назначения=" + SelectedPointNumber + "", sqlConnect);




                using (SqlDataReader sqlReader = sqlView.ExecuteReader())
                {
                    if (sqlReader.Read())
                    {
                        Street.Text = sqlReader["Улица"].ToString();
                        House.Text = sqlReader["Дом"].ToString();
                        StoragePeriod.Text = sqlReader["Срок_хранения"].ToString();
                        WorkBeginTime.Text = sqlReader["Начало_работы"].ToString();
                        WorkEndTime.Text = sqlReader["Конец_работы"].ToString();
                    }
                }


                sqlConnect.Close();

            }
        }


        public void FillPointData()
        {

            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            SqlCommand sqlQuery = new SqlCommand("SELECT Пункт_назначения.Код_пункта_назначения, Пункт_назначения.Улица, Пункт_назначения.Дом, Пункт_назначения.Срок_хранения, Пункт_назначения.Начало_работы, Пункт_назначения.Конец_работы FROM [dbo].[Пункт_назначения] WHERE Тип_пункта_назначения=" + 1 + " ", sqlConnect);
            SqlDataReader sqlReader = sqlQuery.ExecuteReader();


            // Создайте адаптер данных для заполнения DataTable
            SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery);

            // Создайте DataTable для хранения данных
            DataTable dataTable = new DataTable();
            Point.Items.Clear();


            while (sqlReader.Read())
            {
                string street = sqlReader["Улица"].ToString();
                string house = sqlReader["Дом"].ToString();
                string address = street + ", " + house;
                Point.Items.Add(address);

                Street.Text = sqlReader["Улица"].ToString();
                House.Text = sqlReader["Дом"].ToString();
                StoragePeriod.Text = sqlReader["Срок_хранения"].ToString();
                WorkBeginTime.Text = sqlReader["Начало_работы"].ToString();
                WorkEndTime.Text = sqlReader["Конец_работы"].ToString();

            }



        }

        private void доставкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CartPurchase.Rows.Clear();
            CartPurchase.Columns.Clear();

            DeiiveryDetails.Visible = false;
            OrderDetails.Visible = false;
            CompletedOrderDetails.Visible = false;
            OrderCreating.Visible = false;
            Orders.Visible = false;
            CartBox.Visible = false;
            ReportInserting.Visible = false;
            ReportDetails.Visible = false;
            ShopListBox.Visible = false;
            Deliveries.Visible = false;
            Assortment.Visible = false;
            Reports.Visible = false;
            CompletedOrders.Visible = false;
            ShopAssortment.Visible = false;
            ProductPage.Visible = false;
            OrderCreating.Visible = false;
            Profile.Visible = false;
            Deliveries.Visible = true;
            Deliveries.Size = new Size(1220, 270);
            Deliveries.Location = new Point(20, 30);
            ShowDeliveryList();
        }

        public void ShowDeliveryList()
        {
            // Очистить контейнер для отображения данных корзины (например, DataGridView)
            DeliveryList.Rows.Clear();

            // Создайть необходимые столбцы в контейнере
            DeliveryList.Columns.Clear();
            // Создаем столбцы
            DataGridViewColumn DeliveryIdColumn = new DataGridViewTextBoxColumn();
            DeliveryIdColumn.HeaderText = "Номер отслеживания";
            DeliveryIdColumn.DataPropertyName = "Номер_отслеживания";
            DeliveryList.Columns.Add(DeliveryIdColumn);

            DataGridViewColumn OrderIDColumn = new DataGridViewTextBoxColumn();
            OrderIDColumn.HeaderText = "Номер заказа";
            OrderIDColumn.DataPropertyName = "Номер_заказа";
            DeliveryList.Columns.Add(OrderIDColumn);




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
    " Способ_получения.Название_способа_получения AS [Способ получения], " +
    "CONCAT(Пункт_назначения.Улица, ', ', Пункт_назначения.Дом) AS [Адрес], Заказ.Дата_заказа AS [Дата заказа], " +
    "Статус_заказа.Название_статуса AS [Статус заказа], Доставка.Отметка_о_выполнении AS [Отметка о выполнении] FROM Доставка" +
    " INNER JOIN Заказ ON Заказ.Номер_заказа=Доставка.Заказ " +
    " INNER JOIN Покупатель ON Покупатель.ID_покупателя=Заказ.Заказчик " +
    " INNER JOIN Способ_оплаты ON Способ_оплаты.Код_способа_оплаты=Заказ.Способ_оплаты " +
    " INNER JOIN Способ_получения ON Способ_получения.Код_способа_получения=Заказ.Способ_получения" +
    " INNER JOIN Пункт_назначения ON Пункт_назначения.Код_пункта_назначения=Заказ.Адрес_получения" +
    " INNER JOIN Статус_заказа ON Статус_заказа.ID_Cтатуса=Заказ.Статус_заказа" +
    " WHERE Заказ.Заказчик= '" + ClientID + "'" +
    " GROUP BY Доставка.Номер_отслеживания, Заказ.Номер_заказа, Способ_получения.Название_способа_получения, " +
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
                dataGridViewRow.Cells[2].Value = WayOfGettingName;
                dataGridViewRow.Cells[3].Value = Address;
                dataGridViewRow.Cells[4].Value = Date;
                dataGridViewRow.Cells[5].Value = Status;
                dataGridViewRow.Cells[6].Value = Mark;

                switch (Status)
                {

                    case "True":
                        dataGridViewRow.DefaultCellStyle.ForeColor = Color.Green;
                        break;
                }
                dataGridViewColumn.Width = 150;
                // Остальные значения...

                // Добавляем строку в таблицу
                DeliveryList.Rows.Add(dataGridViewRow);

            }
        }

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
            ReportDetails.Visible = false;

            Deliveries.Visible = false;
            OrderCreating.Visible = false;
            Profile.Visible = false;
            Assortment.Visible = false;
            Assortment.Visible = false;
            OrderDetails.Visible = false;
            Deliveries.Visible = false;
            DeiiveryDetails.Visible = true;
            DeiiveryDetails.Size = new Size(960, 360);
            DeiiveryDetails.Location = new Point(20, 30);


            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            SqlCommand getOrderDetails = new SqlCommand("SELECT Доставка.Номер_отслеживания AS [Номер отслеживания], Доставка.Претензии, Заказ.Номер_заказа AS [Номер заказа]," +
                "  Способ_получения.Название_способа_получения AS [Способ получения]," +
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
                " WHERE Доставка.Номер_отслеживания= " + DeliveryNumber + " ", sqlConnect);
            //sqlCmd.Parameters.AddWithValue("@GameID", GameID);


            using (SqlDataReader sqlReader = getOrderDetails.ExecuteReader())
            {
                if (sqlReader.Read())
                {
                    DeliveryLabel.Text = DeliveryNumber.ToString();


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

        private void BackToProcessingOrders_Click(object sender, EventArgs e)
        {
            ReportDetails.Visible = false;

            Deliveries.Visible = false;
            OrderCreating.Visible = false;
            Profile.Visible = false;
            Assortment.Visible = false;
            Assortment.Visible = false;
            OrderDetails.Visible = false;
            ShopAssortment.Visible = false;
            Orders.Size = new Size(1300, 270);
            Orders.Location = new Point(20, 30);

            Orders.Visible = true;

            ShowOrdersList();

        }

        private void ComeBackToReports_Click(object sender, EventArgs e)
        {
            Deliveries.Visible = false;
            OrderCreating.Visible = false;
            Profile.Visible = false;
            Assortment.Visible = false;
            Assortment.Visible = false;
            OrderDetails.Visible = false;
            ShopAssortment.Visible = false;
            ReportDetails.Visible = false;
            Reports.Size = new Size(1300, 270);
            Reports.Location = new Point(20, 30);

            Reports.Visible = true;
            ShowOrdersList();
        }

        private void ComeBackToDeliveries_Click(object sender, EventArgs e)
        {
            DeiiveryDetails.Visible = false;
            Deliveries.Visible = false;
            Profile.Visible = false;
            ReportDetails.Visible = false;
            Deliveries.Visible = true;
            Deliveries.Size = new Size(1100, 300);
            Deliveries.Location = new Point(20, 30);
            ShowDeliveryList();
        }


        private void OrderOpen_Click(object sender, EventArgs e)
        {
            DeiiveryDetails.Visible = false;
            Deliveries.Visible = false;
            Profile.Visible = false;
            Orders.Visible = false;
            OrderDetails.Visible = true;
            OrderDetails.Size = new Size(1250, 300);
            OrderDetails.Location = new Point(20, 30);
            int orderNumber = Convert.ToInt32(OrderDeliveryLabel.Text);
            ShowOrderDetails(orderNumber);

        }

        private void ComeBackToCompletedOrders_Click(object sender, EventArgs e)
        {
            DeiiveryDetails.Visible = false;
            Deliveries.Visible = false;
            Profile.Visible = false;
            Orders.Visible = false;
            OrderDetails.Visible = false;
            CompletedOrderDetails.Visible = false;
            CompletedOrders.Visible = true;
            CompletedOrders.Size = new Size(1250, 300);
            CompletedOrders.Location = new Point(20, 30);
            ShowCompletedOrderListShow();
        }

        private void выполненныеЗаказыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CartPurchase.Rows.Clear();
            CartPurchase.Columns.Clear();

            // Создайть необходимые столбцы в контейнере
            DeiiveryDetails.Visible = false;
            OrderDetails.Visible = false;
            CompletedOrderDetails.Visible = false;
            OrderCreating.Visible = false;
            Orders.Visible = false;
            CartBox.Visible = false;
            ReportInserting.Visible = false;
            ReportDetails.Visible = false;
            ShopListBox.Visible = false;
            Deliveries.Visible = false;
            Assortment.Visible = false;
            Reports.Visible = false;
            ShopAssortment.Visible = false;
            ProductPage.Visible = false;
            OrderCreating.Visible = false;
            Profile.Visible = false;
            CompletedOrders.Visible = true;
            CompletedOrders.Size = new Size(1250, 300);
            CompletedOrders.Location = new Point(20, 30);
            ShowCompletedOrderListShow();
        }

        public void ShowCompletedOrderListShow()
        {
            // Очистить контейнер для отображения данных корзины (например, DataGridView)
            CompletedOrderList.Rows.Clear();

            // Создайть необходимые столбцы в контейнере
            CompletedOrderList.Columns.Clear();
            // Создаем столбцы
            DataGridViewColumn IdColumn = new DataGridViewTextBoxColumn();
            IdColumn.HeaderText = "Номер заказа";
            IdColumn.DataPropertyName = "Номер_заказа";
            CompletedOrderList.Columns.Add(IdColumn);


            DataGridViewColumn SumColumn = new DataGridViewTextBoxColumn();
            SumColumn.HeaderText = "Сумма оплаты";
            SumColumn.DataPropertyName = "Сумаа_оплаты";
            CompletedOrderList.Columns.Add(SumColumn);

            DataGridViewColumn WayOfPaymentColumn = new DataGridViewTextBoxColumn();
            WayOfPaymentColumn.HeaderText = "Способ_оплаты";
            WayOfPaymentColumn.DataPropertyName = "Способ оплаты";
            CompletedOrderList.Columns.Add(WayOfPaymentColumn);

            DataGridViewColumn WayOfGettingColumn = new DataGridViewTextBoxColumn();
            WayOfGettingColumn.HeaderText = "Способ_получения";
            WayOfGettingColumn.DataPropertyName = "Способ получения";
            CompletedOrderList.Columns.Add(WayOfGettingColumn);

            DataGridViewColumn StreetColumn = new DataGridViewTextBoxColumn();
            StreetColumn.HeaderText = "Адрес";
            StreetColumn.DataPropertyName = "Адрес";
            CompletedOrderList.Columns.Add(StreetColumn);

            DataGridViewColumn DateColumn = new DataGridViewTextBoxColumn();
            DateColumn.HeaderText = "Дата заказа";
            DateColumn.DataPropertyName = "Дата_заказа";
            CompletedOrderList.Columns.Add(DateColumn);

            DataGridViewColumn IssueDateColumn = new DataGridViewTextBoxColumn();
            IssueDateColumn.HeaderText = "Дата выдачи";
            IssueDateColumn.DataPropertyName = "Дата_выдачи";
            CompletedOrderList.Columns.Add(IssueDateColumn);
            // Создаем столбец "Добавить
            DataGridViewColumn StatusColumn = new DataGridViewTextBoxColumn();
            StatusColumn.HeaderText = "Статус заказа";
            StatusColumn.DataPropertyName = "Статус_заказа";
            CompletedOrderList.Columns.Add(StatusColumn);

            DataGridViewColumn PrintButtonCollumn = new DataGridViewTextBoxColumn();
            PrintButtonCollumn.HeaderText = "Печать";
            StatusColumn.DataPropertyName = "Печать";
            CompletedOrderList.Columns.Add(PrintButtonCollumn);



            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            SqlCommand OrderList = new SqlCommand("SELECT Заказ.Номер_заказа AS [Номер заказа], Заказ.Сумма_оплаты AS [Сумма оплаты], Способ_оплаты.Название_способа_оплаты AS [Способ оплаты]," +
    " Способ_получения.Название_способа_получения AS [Способ получения], CONCAT(Пункт_назначения.Улица, ', ', Пункт_назначения.Дом) AS [Адрес], Заказ.Дата_заказа AS [Дата заказа],  Заказ.Дата_выдачи AS [Дата выдачи], Статус_заказа.Название_статуса AS [Статус заказа] FROM Заказ" +
    " INNER JOIN Покупатель ON Покупатель.ID_покупателя=Заказ.Заказчик " +
    " INNER JOIN Способ_оплаты ON Способ_оплаты.Код_способа_оплаты=Заказ.Способ_оплаты " +
    " INNER JOIN Способ_получения ON Способ_получения.Код_способа_получения=Заказ.Способ_получения" +
    " INNER JOIN Пункт_назначения ON Пункт_назначения.Код_пункта_назначения=Заказ.Адрес_получения" +
    " INNER JOIN Статус_заказа ON Статус_заказа.ID_Cтатуса=Заказ.Статус_заказа " +
    " WHERE Заказ.Заказчик=" + ClientID + " AND Заказ.Статус_заказа=3", sqlConnect);
            //sqlCmd.Parameters.AddWithValue("@GameID", GameID);
            CompletedOrderList.AllowUserToAddRows = false;

            // Создаем адаптер данных для заполнения DataSet
            SqlDataAdapter adapter = new SqlDataAdapter(OrderList);

            // Создаем DataSet для хранения данных
            DataSet dataSet = new DataSet();

            // Заполняем DataSet данными из таблицы
            adapter.Fill(dataSet);

            CompletedOrderList.AutoGenerateColumns = false;

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                // Получаем данные из строки

                string ID = row["Номер заказа"].ToString();
                string Sum = row["Сумма оплаты"].ToString();
                string WayOfPaymentName = row["Способ оплаты"].ToString();
                string WayOfGettingtName = row["Способ получения"].ToString();
                string Address = row["Адрес"].ToString();
                string Date = row["Дата заказа"].ToString();
                string IssueDate = row["Дата выдачи"].ToString();

                string Status = row["Статус заказа"].ToString();


                // Создаем строку и заполняем ее данными
                DataGridViewRow dataGridViewRow = new DataGridViewRow();
                DataGridViewColumn dataGridViewColumn = new DataGridViewColumn();

                dataGridViewRow.CreateCells(CompletedOrderList);


                dataGridViewRow.Cells[0].Value = ID;
                dataGridViewRow.Cells[1].Value = Sum;
                dataGridViewRow.Cells[2].Value = WayOfPaymentName;
                dataGridViewRow.Cells[3].Value = WayOfGettingtName;
                dataGridViewRow.Cells[4].Value = Address;
                dataGridViewRow.Cells[5].Value = Date;
                dataGridViewRow.Cells[6].Value = IssueDate;
                dataGridViewRow.Cells[7].Value = Status;

                DataGridViewButtonCell ReportButtonCell = new DataGridViewButtonCell();
                ReportButtonCell.Value = "Распечать";
                ReportButtonCell.FlatStyle = FlatStyle.Flat;
                ReportButtonCell.Style.BackColor = Color.White;
                dataGridViewRow.Cells[8] = ReportButtonCell;

                dataGridViewColumn.Width = 150;

                // Добавляем строку в таблицу
                CompletedOrderList.Rows.Add(dataGridViewRow);

            }
        }

        private void CompletePurchaseList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.ColumnIndex == 5)
            {

                DataGridViewRow selectedRow = CompletePurchaseList.Rows[e.RowIndex];
                int PurchaseID = Convert.ToInt32(selectedRow.Cells[0].Value);
                ReportInsert(PurchaseID);
            }
        }

        public void ReportInsert(int PurchaseID)
        {
            CompletedOrderDetails.Visible = false;
            CompletedOrders.Visible = false;
            ReportInserting.Visible = true;
            ReportInserting.Size = new Size(1200, 420);
            ReportInserting.Location = new Point(20, 30);

            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            SqlCommand getReportDetails = new SqlCommand("SELECT Покупка.Номер_покупки, Магазин.Название_магазина, Настольная_игра.Название " +
                " FROM Покупка" +
                " INNER JOIN Ассортимент ON Покупка.Ассортимент=Ассортимент.Номер_ассортимента" +
                " INNER JOIN Магазин ON Магазин.Код_магазина=Ассортимент.Магазин " +
                " INNER JOIN Настольная_игра ON Настольная_игра.Артикул=Ассортимент.Настольная_игра " +
                " WHERE Покупка.Номер_покупки=" + PurchaseID + " ", sqlConnect);

            using (SqlDataReader sqlReader = getReportDetails.ExecuteReader())
            {
                if (sqlReader.Read())
                {
                    PurchaseIDInsert.Text = PurchaseID.ToString();
                    ShopNameInsert.Text = sqlReader["Название_магазина"].ToString();
                    GameNameInsert.Text = sqlReader["Название"].ToString();
                }
            }

        }

        private void CompletedOrderList_CellClick(object sender, DataGridViewCellEventArgs e)
        {



          
            if (e.RowIndex >= 0 && e.ColumnIndex == 8)
            {

                SqlConnection sqlConnect = new SqlConnection(connection);
                sqlConnect.Open();


                DataGridViewRow selectedRow = CompletedOrderList.Rows[e.RowIndex];
                int orderNumber = Convert.ToInt32(selectedRow.Cells[0].Value);
                string outputFilePath = @"C:\Выходные_документы\Чек_заказа_N" + orderNumber + ".txt";
                SqlCommand getOrderDetails = new SqlCommand("SELECT Заказ.Номер_заказа AS [Номер заказа], CONCAT(Покупатель.Фамилия, ' ', Покупатель.Имя, ' ', Покупатель.Отчество) AS [ФИО], " +
                "Заказ.Сумма_оплаты AS [Сумма оплаты], Способ_оплаты.Название_способа_оплаты AS [Способ оплаты], Способ_получения.Название_способа_получения AS [Способ получения], " +
                "CONCAT(Пункт_назначения.Улица, ', ', Пункт_назначения.Дом) AS [Адрес], Заказ.Дата_заказа AS [Дата заказа], Заказ.Дата_выдачи AS [Дата выдачи]," +
                " Статус_заказа.Название_статуса AS [Статус заказа]  FROM Заказ" +
                " INNER JOIN Покупатель ON Покупатель.ID_покупателя=Заказ.Заказчик " +
                " INNER JOIN Способ_оплаты ON Способ_оплаты.Код_способа_оплаты=Заказ.Способ_оплаты " +
                " INNER JOIN Способ_получения ON Способ_получения.Код_способа_получения=Заказ.Способ_получения" +
                " INNER JOIN Пункт_назначения ON Пункт_назначения.Код_пункта_назначения=Заказ.Адрес_получения" +
                " INNER JOIN Покупка ON Покупка.Заказ=Заказ.Номер_заказа " +
                " INNER JOIN Ассортимент ON Покупка.Ассортимент=Ассортимент.Номер_ассортимента" +
                " INNER JOIN Статус_заказа ON Статус_заказа.ID_Cтатуса=Заказ.Статус_заказа" +
                " WHERE Заказ.Номер_заказа= " + orderNumber + " AND Заказ.Статус_заказа =3 ", sqlConnect);
                string OrderReceipt = "";
                using (SqlDataReader sqlReader = getOrderDetails.ExecuteReader())
                {
                    if (sqlReader.Read())
                    {
                        string OrderID = orderNumber.ToString();
                        string OrderStatus = sqlReader["Статус заказа"].ToString();
                        string OrderDate = sqlReader["Дата заказа"].ToString();
                        string OrderIssueDate = sqlReader["Дата выдачи"].ToString();
                        string Sum = sqlReader["Сумма оплаты"].ToString() + "₽";
                        string FullName = sqlReader["ФИО"].ToString();
                        string OrderAddress = sqlReader["Адрес"].ToString();
                        string WayOfPayment = sqlReader["Способ оплаты"].ToString();
                        string WayOfGetting = sqlReader["Способ получения"].ToString();

                        string MainText =
                            "\n     Чек заказа N " + OrderID + " от " + OrderDate +
                            "\n     ФИО Заказчика: " + FullName +
                            "\n     Адрес доставки: " + OrderAddress +
                            "\n     Способ получения: " + WayOfGetting +
                            "\n     Способ оплаты: " + WayOfPayment +
                            "\n     Статус заказа: " + OrderStatus +
                        "\n--------------------------------------\n";

                        OrderReceipt = OrderReceipt + MainText;
                    }

                }
                SqlCommand getPurchaseList = new SqlCommand("SELECT  Покупка.Ассортимент, Настольная_игра.Изображение_игры, Настольная_игра.Название, Магазин.Название_магазина, COUNT(*) AS Количество, Покупка.Цена_покупки FROM Покупка " +
  " INNER JOIN Ассортимент ON Ассортимент.Номер_ассортимента=Покупка.Ассортимент" +
  " INNER JOIN Настольная_игра ON Ассортимент.Настольная_игра=Настольная_игра.Артикул" +
  " INNER JOIN Магазин ON Магазин.Код_магазина=Ассортимент.Магазин " +
  " INNER JOIN Заказ ON Заказ.Номер_заказа=Покупка.Заказ WHERE Покупка.Заказ=" + orderNumber +
  " GROUP BY  Покупка.Ассортимент, Настольная_игра.Изображение_игры, Настольная_игра.Название, Магазин.Название_магазина, Покупка.Цена_покупки", sqlConnect);
                using (SqlDataReader reader = getPurchaseList.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string productID = reader["Ассортимент"].ToString();
                        string GameName = reader["Название"].ToString();
                        string ShopName = reader["Название_магазина"].ToString();
                        string Quantity = reader["Количество"].ToString();
                        string productPrice = reader["Цена_покупки"].ToString();
                        string ProductData = "\n\t" + productID + "" +
                            "\n\t" + GameName + "" +
                            "\n\t" + Quantity + "x" + productPrice + "" +
                            "\n\tНазвание магазина: " + ShopName +
                            "\n--------------------------------------\n";
                        OrderReceipt = OrderReceipt + ProductData;

                    }
                }


                SqlCommand getOtherDetails = new SqlCommand("SELECT Заказ.Номер_заказа AS [Номер заказа], CONCAT(Покупатель.Фамилия, ' ', Покупатель.Имя, ' ', Покупатель.Отчество) AS [ФИО], " +
                "Заказ.Сумма_оплаты AS [Сумма оплаты], Способ_оплаты.Название_способа_оплаты AS [Способ оплаты], Способ_получения.Название_способа_получения AS [Способ получения], " +
                "CONCAT(Пункт_назначения.Улица, ', ', Пункт_назначения.Дом) AS [Адрес], Заказ.Дата_заказа AS [Дата заказа], Заказ.Дата_выдачи AS [Дата выдачи]," +
                " Статус_заказа.Название_статуса AS [Статус заказа]  FROM Заказ" +
                " INNER JOIN Покупатель ON Покупатель.ID_покупателя=Заказ.Заказчик " +
                " INNER JOIN Способ_оплаты ON Способ_оплаты.Код_способа_оплаты=Заказ.Способ_оплаты " +
                " INNER JOIN Способ_получения ON Способ_получения.Код_способа_получения=Заказ.Способ_получения" +
                " INNER JOIN Пункт_назначения ON Пункт_назначения.Код_пункта_назначения=Заказ.Адрес_получения" +
                " INNER JOIN Покупка ON Покупка.Заказ=Заказ.Номер_заказа " +
                " INNER JOIN Ассортимент ON Покупка.Ассортимент=Ассортимент.Номер_ассортимента" +
                " INNER JOIN Статус_заказа ON Статус_заказа.ID_Cтатуса=Заказ.Статус_заказа" +
                " WHERE Заказ.Номер_заказа= " + orderNumber + " AND Заказ.Статус_заказа =3 ", sqlConnect);

                using (SqlDataReader lastReader = getOtherDetails.ExecuteReader())
                {
                    if (lastReader.Read())
                    {
                        string OrderDate = lastReader["Дата заказа"].ToString();
                        string OrderIssueDate = lastReader["Дата выдачи"].ToString();
                        string Sum = lastReader["Сумма оплаты"].ToString() + "₽";
                        string LastText =
                            "Итого: " + Sum +
                            "\nДата заказа: " + OrderDate +
                            "\nДата доставки: " + OrderIssueDate;
                        ;

                        OrderReceipt = OrderReceipt + LastText;
                    }

                }
                File.WriteAllText(outputFilePath, OrderReceipt);
                MessageBox.Show("Чек на заказ успешно сформирован", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
        }

        private void CompletedOrderList_CellContetClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && !(e.ColumnIndex==8)) // Проверяем, что нажатие не произошло на кнопке
            {
                   DataGridViewRow selectedRow = CompletedOrderList.Rows[e.RowIndex];
        int orderNumber = Convert.ToInt32(selectedRow.Cells[0].Value); 
        ShowCompletedOrderDetails(orderNumber);
            }
         
    }

       


        public void ShowCompletedOrderDetails(int orderNumber)
        {
            CompletedOrders.Visible = false;
            CompletedOrderDetails.Visible = true;
            CompletedOrderDetails.Size = new Size(1200, 420);
            CompletedOrderDetails.Location = new Point(20, 30);


            // Очистить контейнер для отображения данных корзины (например, DataGridView)
            CompletePurchaseList.Rows.Clear();

            // Создайть необходимые столбцы в контейнере
            CompletePurchaseList.Columns.Clear();
            // Создаем столбцы
          

            DataGridViewColumn IdColumn = new DataGridViewTextBoxColumn();
            IdColumn.HeaderText = "Покупка";
            IdColumn.DataPropertyName = "Номер_покупки";
            CompletePurchaseList.Columns.Add(IdColumn);

            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
            imageColumn.HeaderText = "Изображение";
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            imageColumn.DataPropertyName = "Изображение_игры";
            CompletePurchaseList.Columns.Add(imageColumn);

            DataGridViewColumn NameColumn = new DataGridViewTextBoxColumn();
            NameColumn.HeaderText = "Название";
            NameColumn.DataPropertyName = "Настольная игра";
            CompletePurchaseList.Columns.Add(NameColumn);

            // Создаем столбец для отображения картинок
            DataGridViewColumn ShopNameColumn = new DataGridViewTextBoxColumn();
            ShopNameColumn.HeaderText = "Магазин";
            ShopNameColumn.DataPropertyName = "Название_магазина";
            CompletePurchaseList.Columns.Add(ShopNameColumn);


            DataGridViewColumn WayOfPaymentColumn = new DataGridViewTextBoxColumn();
            WayOfPaymentColumn.HeaderText = "Цена";
            WayOfPaymentColumn.DataPropertyName = "Цена_покупки";
            CompletePurchaseList.Columns.Add(WayOfPaymentColumn);


            // Создаем столбец "Уменьшить"
            DataGridViewButtonColumn ReportButtonColumn = new DataGridViewButtonColumn();
            ReportButtonColumn.HeaderText = "Пожаловаться";
            ReportButtonColumn.Text = "Пожаловаться";
            ReportButtonColumn.UseColumnTextForButtonValue = true;
            CompletePurchaseList.Columns.Add(ReportButtonColumn);


            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            SqlCommand getOrderDetails = new SqlCommand("SELECT Заказ.Номер_заказа AS [Номер заказа], CONCAT(Покупатель.Фамилия, ' ', Покупатель.Имя, ' ', Покупатель.Отчество) AS [ФИО], " +
                "Заказ.Сумма_оплаты AS [Сумма оплаты], Способ_оплаты.Название_способа_оплаты AS [Способ оплаты], Способ_получения.Название_способа_получения AS [Способ получения], " +
                "CONCAT(Пункт_назначения.Улица, ', ', Пункт_назначения.Дом) AS [Адрес], Заказ.Дата_заказа AS [Дата заказа], Заказ.Дата_выдачи AS [Дата выдачи]," +
                " Статус_заказа.Название_статуса AS [Статус заказа]  FROM Заказ" +
                " INNER JOIN Покупатель ON Покупатель.ID_покупателя=Заказ.Заказчик " +
                " INNER JOIN Способ_оплаты ON Способ_оплаты.Код_способа_оплаты=Заказ.Способ_оплаты " +
                " INNER JOIN Способ_получения ON Способ_получения.Код_способа_получения=Заказ.Способ_получения" +
                " INNER JOIN Пункт_назначения ON Пункт_назначения.Код_пункта_назначения=Заказ.Адрес_получения" +
                " INNER JOIN Покупка ON Покупка.Заказ=Заказ.Номер_заказа " +
                " INNER JOIN Ассортимент ON Покупка.Ассортимент=Ассортимент.Номер_ассортимента" +
                " INNER JOIN Статус_заказа ON Статус_заказа.ID_Cтатуса=Заказ.Статус_заказа" +
                " WHERE Заказ.Номер_заказа= " + orderNumber + " AND Заказ.Статус_заказа =3 ", sqlConnect);
            //sqlCmd.Parameters.AddWithValue("@GameID", GameID);


            using (SqlDataReader sqlReader = getOrderDetails.ExecuteReader())
            {
                if (sqlReader.Read())
                {
                    CompleteOrderID.Text = orderNumber.ToString();
                    CompleteOrderStatus.Text = sqlReader["Статус заказа"].ToString();
                    CompleteOrderDateLabel.Text = sqlReader["Дата заказа"].ToString();
                    CompleteIssueDate.Text= sqlReader["Дата выдачи"].ToString();
                    CompleteSum.Text = sqlReader["Сумма оплаты"].ToString() + "₽";
                    CompleteFullName.Text = sqlReader["ФИО"].ToString();
                    CompleteAddress.Text = sqlReader["Адрес"].ToString();
                    CompleteWayOfPayment.Text = sqlReader["Способ оплаты"].ToString();
                    CompleteWayOfGetting.Text = sqlReader["Способ получения"].ToString();
                }
            }


            SqlCommand getPurchaseList = new SqlCommand("SELECT  Покупка.Номер_покупки, Настольная_игра.Изображение_игры, Настольная_игра.Название, Магазин.Название_магазина, Покупка.Цена_покупки FROM Покупка " +
    " INNER JOIN Ассортимент ON Ассортимент.Номер_ассортимента=Покупка.Ассортимент" +
    " INNER JOIN Настольная_игра ON Ассортимент.Настольная_игра=Настольная_игра.Артикул" +
    " INNER JOIN Магазин ON Магазин.Код_магазина=Ассортимент.Магазин " +
    " INNER JOIN Заказ ON Заказ.Номер_заказа=Покупка.Заказ WHERE Покупка.Заказ=" + orderNumber +
    " GROUP BY  Покупка.Номер_покупки, Настольная_игра.Изображение_игры, Настольная_игра.Название, Магазин.Название_магазина, Покупка.Цена_покупки", sqlConnect); 

            CompletePurchaseList.AllowUserToAddRows = false;
            // Создаем адаптер данных для заполнения DataSet
            SqlDataAdapter adapter = new SqlDataAdapter(getPurchaseList);

            // Создаем DataSet для хранения данных
            DataSet dataSet = new DataSet();

            // Заполняем DataSet данными из таблицы
            adapter.Fill(dataSet);

            CompletePurchaseList.AutoGenerateColumns = false;

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                // Получаем данные из строки

                string ID = row["Номер_покупки"].ToString();
                string imagePath = row["Изображение_игры"].ToString();
                Image image = Image.FromFile(imagePath);

                string Name = row["Название"].ToString();
                string ShopName = row["Название_магазина"].ToString();


                string Price = row["Цена_покупки"].ToString();




                // Создаем строку и заполняем ее данными
                DataGridViewRow dataGridViewRow = new DataGridViewRow();
                DataGridViewColumn dataGridViewColumn = new DataGridViewColumn();

                dataGridViewRow.CreateCells(CompletePurchaseList);


                dataGridViewRow.Cells[0].Value = ID;
                dataGridViewRow.Cells[1].Value = image;
                dataGridViewRow.Cells[2].Value = Name;
                dataGridViewRow.Cells[3].Value = ShopName;
                dataGridViewRow.Cells[4].Value = Price;

                DataGridViewButtonCell ReportButtonCell = new DataGridViewButtonCell();
                ReportButtonCell.Value = "Пожаловаться";
                ReportButtonCell.FlatStyle = FlatStyle.Flat;
                ReportButtonCell.Style.BackColor = Color.OrangeRed;
                dataGridViewRow.Cells[5] = ReportButtonCell;



                dataGridViewColumn.Width = 150;
                // Остальные значения...
                dataGridViewRow.Height = 150;

                // Добавляем строку в таблицу
                CompletePurchaseList.Rows.Add(dataGridViewRow);

            }
        }

        private void ButtonReportInsert_Click(object sender, EventArgs e)
        {
            Deliveries.Visible = false;
            Reports.Visible = false;
            ReportDetails.Visible = true;
            ReportDetails.Size = new Size(960, 400);
            ReportDetails.Location = new Point(20, 30);
            try
            {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            SqlCommand getMaxReportID = new SqlCommand("SELECT Max(Заявление_на_возврат.Номер_заявления) FROM Заявление_на_возврат", sqlConnect);
            int ReportID = Convert.ToInt32(getMaxReportID. ExecuteScalar())+1;
            SqlCommand getShopID = new SqlCommand("SELECT Магазин.Код_магазина FROM Магазин" +
                " INNER JOIN Ассортимент ON Ассортимент.Магазин=Магазин.Код_магазина " +
                " INNER JOIN Покупка ON Покупка.Ассортимент=Ассортимент.Номер_ассортимента" +
                " WHERE Покупка.Номер_покупки="+ Convert.ToInt32(PurchaseIDInsert.Text)+" " , sqlConnect);
            int ShopID = Convert.ToInt32(getShopID.ExecuteScalar());

            //sqlCmd.Parameters.AddWithValue("@GameID", GameID);
            SqlCommand ReportInsert = new SqlCommand("INSERT INTO Заявление_на_возврат (Номер_заявления, Покупка, Дата_отправки_заявления, Сумма_возврата, Причина_возврата, Продавец) " +
                " VALUES('"+ReportID+ "', '"+Convert.ToInt32(PurchaseIDInsert.Text)+"', '"+DateTime.Now+ "', '"+InsertSumReturn.Text+"', '"+ ReportTextInsert.Text+"', '"+ ShopID +"' )", sqlConnect);
            ReportInsert.ExecuteNonQuery();
            MessageBox.Show("Заявление на возврат успешно отправлено", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);

            OrderCreating.Visible = false;
            ReportDetails.Visible = false;
            ReportInserting.Visible = false;
            Reports.Location = new Point(20, 30);
            Reports.Size = new Size(1000, 300);
            Reports.Visible = true;
           CreateReportDoc(ReportID);


            ReportListShow();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public void CreateReportDoc(int ReportID)
        {
            try
            {
                SqlConnection sqlConnect = new SqlConnection(connection);

                sqlConnect.Open();

                // Создание команды с параметром @ShopCode
                // Задание значения параметра @ShopCode
             

                string outputFilePath = @"C:\Выходные_документы\Данные_заявления_на_возврат_N" + ReportID + ".txt";




                SqlCommand sqlQuery = new SqlCommand("Select Покупка.Номер_покупки, CONCAT(Покупатель.Фамилия, ' ', Покупатель.Имя, ' ', Покупатель.Отчество) AS [ФИО] FROM Покупатель " +
                    " INNER JOIN Заказ ON Заказ.Заказчик=Покупатель.ID_покупателя "+
                    " INNER JOIN Покупка ON Покупка.Заказ=Заказ.Номер_заказа " +
                    " INNER JOIN Заявление_на_возврат ON Заявление_на_возврат.Покупка=Покупка.Номер_покупки " +
                    " WHERE Заявление_на_возврат.Номер_заявления='" + ReportID + "' ", sqlConnect);

                // Создание объекта SqlDataReader для чтения данных из базы данных
                using (SqlDataReader reader = sqlQuery.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Получение значений полей из результата запроса
                        string FullName = reader["ФИО"].ToString();
                        string Purchase = reader["Номер_покупки"].ToString();



                        // Создание строки с информацией о магазине
                        string outputText =
                            "\n|-----------------------------------------------|" +
                            "\n| Заявление на возврат N" + ReportID + " по покупке N "+ Purchase +  "\t\t|" +
                            "\n| От пользователя " + FullName+ "\t\t|" +
                            "\n| К магазину:" + ShopNameInsert.Text + "\t\t\t\t|" +
                            "\n| Предмет спора:" + GameNameInsert.Text + "\t\t\t\t|" +
                            "\n| Запрашиваемая сумма возврата:" + InsertSumReturn.Text + "\t\t\t\t|" +
                            "\n| Причина возврата:" + ReportTextInsert.Text + "\t\t\t\t|" +
                            "\n|\n|\tДата Отправки заявления:" + DateTime.Now + "\t|" +
                            "\n|-----------------------------------------------|"

                            ;

                        // Запись строки в файл
                        File.WriteAllText(outputFilePath, outputText);

                        MessageBox.Show("Выходной документ успешно создан.");
                    }
                    else
                    {
                        MessageBox.Show("Заявление на возврат не найдено.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при создании выходного документа: " + ex.Message);
            }
        }

        private void ComeBackToOrder_Click(object sender, EventArgs e)
        {
            DeiiveryDetails.Visible = false;
            Deliveries.Visible = false;
            Profile.Visible = false;
            Orders.Visible = false;
            OrderDetails.Visible = false;

            CompletedOrders.Visible = true;
            CompletedOrders.Size = new Size(1250, 300);
            CompletedOrders.Location = new Point(20, 30);
            ShowCompletedOrderListShow();
        }

        private void UpdateDelivery_Click(object sender, EventArgs e)
        {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();
            SqlCommand UpdateDelivery = new SqlCommand("UPDATE Доставка SET Доставка.Претензии='"+Claims.Text+"' WHERE Доставка.Номер_отслеживания= '"+DeliveryLabel.Text+"' ", sqlConnect);
            UpdateDelivery.ExecuteNonQuery();
            MessageBox.Show("Претензия о доставке успешно сформирована", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ReportDetails.Visible = false;

            ProductPage.Visible = false;
            Profile.Visible = false;
            ShopListBox.Visible = false;
            ShopAssortment.Visible = false;
            CartBox.Visible = false;
            Assortment.Visible = false;
            OrderCreating.Visible = false;
            Deliveries.Visible = true;
            Deliveries.Size = new Size(1220, 270);
            Deliveries.Location = new Point(20, 30);
            ShowDeliveryList();
        }

        private void PrintReportDock_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection sqlConnect = new SqlConnection(connection);

                sqlConnect.Open();

                // Создание команды с параметром @ShopCode
                // Задание значения параметра @ShopCode

                int Report=Convert.ToInt32(ReportIDLabel.Text);
                string outputFilePath = @"C:\Выходные_документы\Данные_заявления_на_возврат_N" + Report + ".txt";




                SqlCommand sqlQuery = new SqlCommand("Select Покупка.Номер_покупки, CONCAT(Покупатель.Фамилия, ' ', Покупатель.Имя, ' ', Покупатель.Отчество) AS [ФИО] FROM Покупатель " +
                    " INNER JOIN Заказ ON Заказ.Заказчик=Покупатель.ID_покупателя " +
                    " INNER JOIN Покупка ON Покупка.Заказ=Заказ.Номер_заказа " +
                    " INNER JOIN Заявление_на_возврат ON Заявление_на_возврат.Покупка=Покупка.Номер_покупки " +
                    " WHERE Заявление_на_возврат.Номер_заявления='" + Report + "' ", sqlConnect);

                // Создание объекта SqlDataReader для чтения данных из базы данных
                using (SqlDataReader reader = sqlQuery.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Получение значений полей из результата запроса
                        string FullName = reader["ФИО"].ToString();
                        string Purchase = reader["Номер_покупки"].ToString();



                        // Создание строки с информацией о магазине
                        string outputText =
                            "\n|-----------------------------------------------|" +
                            "\n| Заявление на возврат N" + Report + " по покупке N " + PurchaseIDLabel.Text + "\t\t|" +
                            "\n| От пользователя " + FullName + "\t\t|" +
                            "\n| К магазину:" + ReportShopName.Text + "\t\t\t\t|" +
                            "\n| Предмет спора:" + ReportGameName.Text + "\t\t\t\t|" +
                            "\n| Запрашиваемая сумма возврата:" + InsertSumReturn.Text + "\t\t\t\t|" +
                            "\n| Причина возврата:" + ReportReason.Text + "|" +
                            "\n|\n|\n|\tДата Отправки заявления:" + ReportDateLabel + "\t|" +
                            "\n|-----------------------------------------------|"

                            ;

                        // Запись строки в файл
                        File.WriteAllText(outputFilePath, outputText);

                        MessageBox.Show("Выходной документ успешно создан.");
                    }
                    else
                    {
                        MessageBox.Show("Заявление на возврат не найдено.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при создании выходного документа: " + ex.Message);
            }
        }

        private void PrintOrderReceipt_Click(object sender, EventArgs e)
        {
            SqlConnection sqlConnect = new SqlConnection(connection);
            sqlConnect.Open();


            int orderNumber = Convert.ToInt32(CompleteOrderID.Text);
            string outputFilePath = @"C:\Выходные_документы\Чек_заказа_N" + orderNumber + ".txt";
            SqlCommand getOrderDetails = new SqlCommand("SELECT Заказ.Номер_заказа AS [Номер заказа], CONCAT(Покупатель.Фамилия, ' ', Покупатель.Имя, ' ', Покупатель.Отчество) AS [ФИО], " +
            "Заказ.Сумма_оплаты AS [Сумма оплаты], Способ_оплаты.Название_способа_оплаты AS [Способ оплаты], Способ_получения.Название_способа_получения AS [Способ получения], " +
            "CONCAT(Пункт_назначения.Улица, ', ', Пункт_назначения.Дом) AS [Адрес], Заказ.Дата_заказа AS [Дата заказа], Заказ.Дата_выдачи AS [Дата выдачи]," +
            " Статус_заказа.Название_статуса AS [Статус заказа]  FROM Заказ" +
            " INNER JOIN Покупатель ON Покупатель.ID_покупателя=Заказ.Заказчик " +
            " INNER JOIN Способ_оплаты ON Способ_оплаты.Код_способа_оплаты=Заказ.Способ_оплаты " +
            " INNER JOIN Способ_получения ON Способ_получения.Код_способа_получения=Заказ.Способ_получения" +
            " INNER JOIN Пункт_назначения ON Пункт_назначения.Код_пункта_назначения=Заказ.Адрес_получения" +
            " INNER JOIN Покупка ON Покупка.Заказ=Заказ.Номер_заказа " +
            " INNER JOIN Ассортимент ON Покупка.Ассортимент=Ассортимент.Номер_ассортимента" +
            " INNER JOIN Статус_заказа ON Статус_заказа.ID_Cтатуса=Заказ.Статус_заказа" +
            " WHERE Заказ.Номер_заказа= " + orderNumber + " AND Заказ.Статус_заказа =3 ", sqlConnect);
            string OrderReceipt = "";
            using (SqlDataReader sqlReader = getOrderDetails.ExecuteReader())
            {
                if (sqlReader.Read())
                {
                    string OrderID = orderNumber.ToString();
                    string OrderStatus = sqlReader["Статус заказа"].ToString();
                    string OrderDate = sqlReader["Дата заказа"].ToString();
                    string OrderIssueDate = sqlReader["Дата выдачи"].ToString();
                    string Sum = sqlReader["Сумма оплаты"].ToString() + "₽";
                    string FullName = sqlReader["ФИО"].ToString();
                    string OrderAddress = sqlReader["Адрес"].ToString();
                    string WayOfPayment = sqlReader["Способ оплаты"].ToString();
                    string WayOfGetting = sqlReader["Способ получения"].ToString();

                    string MainText =
                        "\n     Чек заказа N " + OrderID + " от " + OrderDate +
                        "\n     ФИО Заказчика: " + FullName +
                        "\n     Адрес доставки: " + OrderAddress +
                        "\n     Способ получения: " + WayOfGetting +
                        "\n     Способ оплаты: " + WayOfPayment +
                        "\n     Статус заказа: " + OrderStatus +
                    "\n--------------------------------------\n";

                    OrderReceipt = OrderReceipt + MainText;
                }

            }
            SqlCommand getPurchaseList = new SqlCommand("SELECT  Покупка.Ассортимент, Настольная_игра.Изображение_игры, Настольная_игра.Название, Магазин.Название_магазина, COUNT(*) AS Количество, Покупка.Цена_покупки FROM Покупка " +
" INNER JOIN Ассортимент ON Ассортимент.Номер_ассортимента=Покупка.Ассортимент" +
" INNER JOIN Настольная_игра ON Ассортимент.Настольная_игра=Настольная_игра.Артикул" +
" INNER JOIN Магазин ON Магазин.Код_магазина=Ассортимент.Магазин " +
" INNER JOIN Заказ ON Заказ.Номер_заказа=Покупка.Заказ WHERE Покупка.Заказ=" + orderNumber +
" GROUP BY  Покупка.Ассортимент, Настольная_игра.Изображение_игры, Настольная_игра.Название, Магазин.Название_магазина, Покупка.Цена_покупки", sqlConnect);
            using (SqlDataReader reader = getPurchaseList.ExecuteReader())
            {
                while (reader.Read())
                {
                    string productID = reader["Ассортимент"].ToString();
                    string GameName = reader["Название"].ToString();
                    string ShopName = reader["Название_магазина"].ToString();
                    string Quantity = reader["Количество"].ToString();
                    string productPrice = reader["Цена_покупки"].ToString();
                    string ProductData = "\n\t" + productID + "" +
                        "\n\t" + GameName + "" +
                        "\n\t" + Quantity + "x" + productPrice + "" +
                        "\n\tНазвание магазина: " + ShopName +
                        "\n--------------------------------------\n";
                    OrderReceipt = OrderReceipt + ProductData;

                }
            }


            SqlCommand getOtherDetails = new SqlCommand("SELECT Заказ.Номер_заказа AS [Номер заказа], CONCAT(Покупатель.Фамилия, ' ', Покупатель.Имя, ' ', Покупатель.Отчество) AS [ФИО], " +
            "Заказ.Сумма_оплаты AS [Сумма оплаты], Способ_оплаты.Название_способа_оплаты AS [Способ оплаты], Способ_получения.Название_способа_получения AS [Способ получения], " +
            "CONCAT(Пункт_назначения.Улица, ', ', Пункт_назначения.Дом) AS [Адрес], Заказ.Дата_заказа AS [Дата заказа], Заказ.Дата_выдачи AS [Дата выдачи]," +
            " Статус_заказа.Название_статуса AS [Статус заказа]  FROM Заказ" +
            " INNER JOIN Покупатель ON Покупатель.ID_покупателя=Заказ.Заказчик " +
            " INNER JOIN Способ_оплаты ON Способ_оплаты.Код_способа_оплаты=Заказ.Способ_оплаты " +
            " INNER JOIN Способ_получения ON Способ_получения.Код_способа_получения=Заказ.Способ_получения" +
            " INNER JOIN Пункт_назначения ON Пункт_назначения.Код_пункта_назначения=Заказ.Адрес_получения" +
            " INNER JOIN Покупка ON Покупка.Заказ=Заказ.Номер_заказа " +
            " INNER JOIN Ассортимент ON Покупка.Ассортимент=Ассортимент.Номер_ассортимента" +
            " INNER JOIN Статус_заказа ON Статус_заказа.ID_Cтатуса=Заказ.Статус_заказа" +
            " WHERE Заказ.Номер_заказа= " + orderNumber + " AND Заказ.Статус_заказа =3 ", sqlConnect);

            using (SqlDataReader lastReader = getOtherDetails.ExecuteReader())
            {
                if (lastReader.Read())
                {
                    string OrderDate = lastReader["Дата заказа"].ToString();
                    string OrderIssueDate = lastReader["Дата выдачи"].ToString();
                    string Sum = lastReader["Сумма оплаты"].ToString() + "₽";
                    string LastText =
                        "Итого: " + Sum +
                        "\nДата заказа: " + OrderDate +
                        "\nДата доставки: " + OrderIssueDate;
                    ;

                    OrderReceipt = OrderReceipt + LastText;
                }

            }
            File.WriteAllText(outputFilePath, OrderReceipt);
            MessageBox.Show("Чек на заказ успешно сформирован", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);


        }

        private void ClientWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
    }
    


 
    

    






