using System;
using System.Collections.Generic;
using System.Linq;

namespace PromotionEngine
{
    public class PromoEngine
    {
        static void Main(string[] args)
        {
            PromoEngine.Calculate();
        }

        public static double Calculate()
        {
            double totalOrdersAmount = 0;

            return totalOrdersAmount;
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
