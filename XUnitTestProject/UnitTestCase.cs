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
            var promotions = new List<Promotion>() { new Promotion(1, new List<char> { 'A' }, 3, 130) };
            var cart = new Dictionary<char, int> { { 'A', 3 }, { 'B', 1 }, { 'C', 1 }, { 'D', 1 } };

            var actual = PromoEngine.Calculate(items, promotions, cart);
            
            int expected = 195;
            Assert.Equal(expected, actual);
        }
    }
}
