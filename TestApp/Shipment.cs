using System;

namespace TestApp
{
    //класс для таблицы отгрузки
    public class Shipment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Organization { get; set; }
        public string Town { get; set; }
        public string Country { get; set; }
        public string Manager { get; set; }
        public int Amount { get; set; }
        public decimal Total { get; set; }
    }
}
