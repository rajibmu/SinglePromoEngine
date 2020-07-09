using System;
using System.Collections.Generic;
using System.Linq;

namespace PromotionEngine
{
    public class PromoEngine
    {
        static void Main(string[] args)
        {
            var items = new List<Item>() { new Item('A', 50), new Item('B', 30), new Item('C', 20), new Item('D', 15) };
            var promotions = new List<Promotion>() { new Promotion(1, new List<char> { 'A' }, 3, 130)};
            var cart = new Dictionary<char, int> { { 'A', 1 }, { 'B', 1 }, { 'C', 1 }, { 'D', 1 } };
            PromoEngine.Calculate(items, promotions, cart);
        }

        public static double Calculate(List<Item> items, List<Promotion> promotions, Dictionary<char, int> cart)
        {
            double totalOrdersAmount = 0;

            //Create Order
            List<Order> OrderList = new List<Order>();
            foreach (var item in cart)
            {
                var order = new Order();
                order.SKU = item.Key;
                order.Quantity = item.Value;
                order.IsPromoApplied = false;
                order.Price = items.Where(w => w.SKU == item.Key).Select(s => s.Price).FirstOrDefault();
                order.TotalAmount = order.Quantity * order.Price;
                OrderList.Add(order);
            }
            totalOrdersAmount = OrderList.Select(s => s.TotalAmount).Sum();

            var promolist = promotions.Select(s => s.Type).Distinct().ToList();
            var NitemsList = promotions.Where(w => w.Type == PromotionType.Nitems).ToList();
            totalOrdersAmount -= ApplyPromoNitems(NitemsList, OrderList);

            return totalOrdersAmount;
        }

        private static double ApplyPromoNitems(List<Promotion> promotions, List<Order> OrderList)
        {
            double promoValue = 0;
            foreach (var order in OrderList)
            {
                var promo = promotions
                    .Where(w => w.Type == PromotionType.Nitems && w.SKUs[0] == order.SKU && w.Value <= order.Quantity)
                    .Select(s => new { TimeValue = (int)(order.Quantity / s.Value), s.Value, s.Price }).FirstOrDefault();

                if (promo != null && promo.TimeValue > 0)
                {
                    promoValue += ((promo.TimeValue * promo.Value) * order.Price) - (promo.TimeValue * promo.Price);
                    order.IsPromoApplied = true;
                }
            }

            return promoValue;

        }
    }

    public class Order : Item
    {
        public int Quantity { get; set; }
        public double TotalAmount { get; set; }
        public bool IsPromoApplied { get; set; }
    }

    public class Item
    {
        public Item()
        {
        }
        public Item(char sku, double price)
        {
            SKU = sku;
            Price = price;
        }
        public char SKU { get; set; }
        public double Price { get; set; }
    }

    public class Promotion
    {
        public Promotion(int type, List<char> skus, int value, double price)
        {
            Type = (PromotionType)type;
            SKUs = skus;
            Value = value;
            Price = price;
        }
        public PromotionType Type { get; set; }
        public List<char> SKUs { get; set; }
        public int Value { get; set; }
        public double Price { get; set; }
    }

    public enum PromotionType
    {
        Nitems = 1,
        Combo = 2,
        Percentage = 3
    }
}
