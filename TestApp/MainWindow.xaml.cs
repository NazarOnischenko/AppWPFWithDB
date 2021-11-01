using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //поле для работы с БД
        private Context context;
        //поле для работы з таблицей, чтобы можно было использовать таблицу и в событиях
        private DbSet<Shipment> shipmentsTable;
        //словарь колонок
        private Dictionary<string,DataGridTextColumn> dictOfColumns = new Dictionary<string, DataGridTextColumn>()
        {
            { "ID" , new DataGridTextColumn() { Header = "Id", Binding = new Binding("Id") }},
            { "DATE" , new DataGridTextColumn() { Header = "Date", Binding = new Binding("Date") } },
            { "TOWN", new DataGridTextColumn() { Header = "Town", Binding = new Binding("Town") } },
            {"COUNTRY", new DataGridTextColumn() { Header = "Country", Binding = new Binding("Country") } },
            { "MANAGER", new DataGridTextColumn() { Header = "Manager", Binding = new Binding("Manager") }},
            { "AMOUNT", new DataGridTextColumn() { Header = "Amount", Binding = new Binding("Amount") }},
            { "TOTAL", new DataGridTextColumn() { Header = "Total", Binding = new Binding("Total") } }

        };
        //текущие значения колонок
        private List<DataGridTextColumn> currentColumns = new List<DataGridTextColumn>();
        //значения колонок, что были использованы в прошлый раз
        private List<DataGridTextColumn> bufferColumns = new List<DataGridTextColumn>(); 
        //текущие значения строк
        private ObservableCollection<Shipment> shipments = new ObservableCollection<Shipment>();
        //значение строк, что были использованы в прошлый раз
        private ObservableCollection<Shipment> bufferShipments = new ObservableCollection<Shipment>();
        public MainWindow()
        {
            InitializeComponent();

            //поключение к БД
            context = new Context();
            //получаем таблицу
            shipmentsTable = context.Shipments;
            
            //заполняем grid
            dataGrid.Columns.Add(dictOfColumns["AMOUNT"]);
            currentColumns.Add(dictOfColumns["AMOUNT"]);
            dataGrid.Columns.Add(dictOfColumns["TOTAL"]);
            currentColumns.Add(dictOfColumns["TOTAL"]);
            foreach (var shipment in shipmentsTable)
            {
                var shipmen = new Shipment()
                {
                    Amount = shipment.Amount,
                    Total = shipment.Total
                };
                shipments.Add(shipmen);
            }
           
            dataGrid.ItemsSource = shipments;
            
        }

        
        //обработка события нажатия на кнопку apply, с проверками на чекбоксы
        private void button_Click(object sender, RoutedEventArgs e)
        {
            //очищаем буферы если они заполнены и заполняем их новыми значениями
            bufferShipments.Clear();
            bufferColumns.Clear();
            for (int i = 0; i < currentColumns.Count; i++)
            {
                bufferColumns.Add(new DataGridTextColumn());
                bufferColumns[i] = currentColumns[i];
            }
            for (int i = 0; i < shipments.Count; i++)
            {
                bufferShipments.Add(new Shipment());
                bufferShipments[i] = shipments[i];
            }
            //очищаем текущие значения и заполняем новыми
            shipments.Clear();
            currentColumns.Clear();
            dataGrid.Columns.Clear();//очищение grida
            var temp = shipmentsTable.ToList();
            foreach (var shipment in shipmentsTable)
            {
                var shipmen = new Shipment();
                shipments.Add(shipmen);
            }
            //проверки чекбоксов, если чекбокс отмечен, то берем данные с даной колонки
            if (checkBox.IsChecked == true)
            {
                dataGrid.Columns.Add(dictOfColumns["ID"]);
                currentColumns.Add(dictOfColumns["ID"]);
                for (int i=0;i<temp.Count;i++)
                {
                    shipments[i].Id = temp[i].Id;  
                }
            }
            if (checkBox1.IsChecked == true)
            {
                dataGrid.Columns.Add(dictOfColumns["DATE"]);
                currentColumns.Add(dictOfColumns["DATE"]);
                for (var i=0;i<temp.Count;i++)
                {
                    shipments[i].Date = temp[i].Date;
                }
            }
            if (checkBox2.IsChecked == true)
            {
                dataGrid.Columns.Add(dictOfColumns["TOWN"]);
                currentColumns.Add(dictOfColumns["TOWN"]);
                for (var i = 0; i < temp.Count; i++)
                {
                    shipments[i].Town = temp[i].Town;
                }
            }
            if (checkBox3.IsChecked == true)
            {
                dataGrid.Columns.Add(dictOfColumns["COUNTRY"]);
                currentColumns.Add(dictOfColumns["COUNTRY"]);
                for (var i = 0; i < temp.Count; i++)
                {
                    shipments[i].Country = temp[i].Country;
                }
            }
            if (checkBox4.IsChecked == true)
            {
                dataGrid.Columns.Add(dictOfColumns["MANAGER"]);
                currentColumns.Add(dictOfColumns["MANAGER"]);
                for (var i = 0; i < temp.Count; i++)
                {
                    shipments[i].Manager = temp[i].Manager;
                }
            }
            //добавление полей которые всегда должны находиться в таблице
            dataGrid.Columns.Add(dictOfColumns["AMOUNT"]);
            currentColumns.Add(dictOfColumns["AMOUNT"]);
            dataGrid.Columns.Add(dictOfColumns["TOTAL"]);
            currentColumns.Add(dictOfColumns["TOTAL"]);
            for (var i = 0; i < temp.Count; i++)
            {
                shipments[i].Amount = temp[i].Amount;
                shipments[i].Total = temp[i].Total;
            }
            //заполнение строк в таблице
            dataGrid.ItemsSource = shipments;
            
        }

        //обработка события кнопки, чтобы вернуть всю предыдущую выборку, которая была до этой
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //очищаем текущие значения и заполняем новыми
            shipments.Clear();
            currentColumns.Clear();
            for (int i = 0; i < bufferShipments.Count; i++)
            {
                shipments.Add(new Shipment());
                shipments[i] = bufferShipments[i];
            }
            for (int i = 0; i < bufferColumns.Count; i++)
            {
                currentColumns.Add(new DataGridTextColumn());
                currentColumns[i] = bufferColumns[i];
            }
            //очищаем grid и заполняем новыми данными
            dataGrid.Columns.Clear();
            foreach (var col in bufferColumns) { dataGrid.Columns.Add(col); }
            dataGrid.ItemsSource = bufferShipments;
            
        }
        //обработка события кнопки на выход
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            context.Dispose();
            Close();
        }
    }
}
