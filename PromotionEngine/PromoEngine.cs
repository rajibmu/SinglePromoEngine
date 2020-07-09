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
            foreach (var tempPromo in promolist)
            {
                switch (tempPromo)
                {
                    case PromotionType.Nitems:
                        var NitemsList = promotions.Where(w => w.Type == PromotionType.Nitems).ToList();
                        totalOrdersAmount -= ApplyPromoNitems(NitemsList, OrderList.Where(o=>o.IsPromoApplied==false).ToList());
                        break;

                    case PromotionType.Combo:
                        var ComboList = promotions.Where(w => w.Type == PromotionType.Combo).ToList();
                        totalOrdersAmount -= ApplyPromoCombo(ComboList, OrderList.Where(o => o.IsPromoApplied == false).ToList());
                        break;

                    case PromotionType.Percentage:
                        var PerList = promotions.Where(w => w.Type == PromotionType.Percentage).ToList();
                        totalOrdersAmount -= ApplyPromoDiscountPercentage(PerList, OrderList.Where(o => o.IsPromoApplied == false).ToList());
                        break;
                }
            }

            return totalOrdersAmount;
        }

        private static double ApplyPromoNitems(List<Promotion> promotions, List<Order> OrderList)
        {
            double promoValue = 0;
            foreach (var order in OrderList)
            {
                var promo = promotions
                    .Where(w => w.SKUs[0] == order.SKU && w.Value <= order.Quantity)
                    .Select(s => new { TimeValue = (int)(order.Quantity / s.Value), s.Value, s.Price }).FirstOrDefault();

                if (promo != null && promo.TimeValue > 0)
                {
                    promoValue += ((promo.TimeValue * promo.Value) * order.Price) - (promo.TimeValue * promo.Price);
                    order.IsPromoApplied = true;
                }
            }

            return promoValue;

        }

        private static double ApplyPromoCombo(List<Promotion> promotions, List<Order> OrderList)
        {
            double promoValue = 0;

            var promos = promotions
                .Select(s => new { s.SKUs, s.Price });

            foreach (var promo in promos)
            {
                if (promo != null && promo.SKUs.Count > 1)
                {
                    var tempOrder = OrderList.Where(o => promo.SKUs.Contains(o.SKU) && o.IsPromoApplied==false).ToList();
                    if (promo.SKUs.Count == tempOrder.Count)
                    {
                        var maxCount = tempOrder.Select(s => s.Quantity).Min();
                        double actualValue = 0;
                        foreach (var order in tempOrder)
                        {
                            order.IsPromoApplied = true;
                            actualValue += order.Price * maxCount;
                        }
                        promoValue = actualValue - promo.Price * maxCount;
                    }
                }
            }

            return promoValue;
        }

        private static double ApplyPromoDiscountPercentage(List<Promotion> promotions, List<Order> OrderList)
        {
            double promoValue = 0;

            foreach (var order in OrderList)
            {
                var promo = promotions
                    .Where(w => w.SKUs[0] == order.SKU)
                    .Select(s => new { Percentage = s.Value}).FirstOrDefault();

                if (promo != null && promo.Percentage > 0)
                {
                    promoValue += order.Quantity * order.Price * (double)(promo.Percentage/100);
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
        public double Value { get; set; }
        public double Price { get; set; }
    }

    public enum PromotionType
    {
        Nitems = 1,
        Combo = 2,
        Percentage = 3
    }
}
