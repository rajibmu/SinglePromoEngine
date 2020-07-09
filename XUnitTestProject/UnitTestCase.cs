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

            var actual = PromoEngine.Calculate();
            
            int expected = 100;
            Assert.Equal(expected, actual);
        }
    }
}
