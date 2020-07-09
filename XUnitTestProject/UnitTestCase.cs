using System;
using System.Collections.Generic;
using Xunit;
using PromotionEngine;

namespace XUnitTestProject
{
    public class UnitTestCase
    {

        [Fact]
        public void Test_PromoEngine_Calculate()
        {
            var items = new List<Item>() { new Item('A', 50), new Item('B', 30), new Item('C', 20), new Item('D', 15) };
            var promotions = new List<Promotion>() { new Promotion(1, new List<char> { 'A' }, 3, 130)
                , new Promotion(1, new List<char> { 'B' }, 2, 45)
                , new Promotion(2, new List<char> { 'C', 'D' }, 0, 30) };
            //var promotions = new List<Promotion>() { new Promotion(1, new List<char> { 'A' }, 3, 130), new Promotion(3, new List<char> { 'A' }, 10, 0)};
            var cart = new Dictionary<char, int> { { 'A', 3 }, { 'B', 3 }, { 'C', 3 }, { 'D', 3 } };

            var actual = PromoEngine.Calculate(items, promotions, cart);
            
            double expected = 130;
            Assert.Equal(expected, actual);
        }
    }
}
