using CardService.Domain;
using CardService.Services.Validators;
using System;
using Xunit;

namespace CardServiceTests
{
    public class UnitTest1
    {
        [Fact]
        public void IsCardExpireWithMonthLessZeroReturnsFalse()
        {
            bool IsDateValid = CardParamsValidator.IsCardExpireDateValidateTrue(new Card { Expire = new Expire(-2, 2003) });
            Assert.False(IsDateValid);
        }

        [Fact]
        public void IsCardExpireWithMonthEqualZeroReturnsFalse()
        {
            bool IsDateValid = CardParamsValidator.IsCardExpireDateValidateTrue(new Card { Expire = new Expire(0, 2003) });
            Assert.False(IsDateValid);
        }

        [Fact]
        public void IsCardExpireWithMonthGreater_12_ReturnsFalse()
        {
            bool IsDateValid = CardParamsValidator.IsCardExpireDateValidateTrue(new Card { Expire = new Expire(-2, 2003) });
            Assert.False(IsDateValid);
        }

        [Fact]
        public void IsCardExpireWithYearGreaterDateTimeNowReturnsFalse()
        {
            int oneYear = 1;
            bool IsDateValid = CardParamsValidator.IsCardExpireDateValidateTrue(new Card { Expire = new Expire(DateTime.Now.Month, DateTime.Now.Year - oneYear) });
            Assert.False(IsDateValid);
        }

        [Fact]
        public void IsCardExpireFutureDateYearReturnsTrue()
        {
            int oneYear = 1;
            bool IsDateValid = CardParamsValidator.IsCardExpireDateValidateTrue(new Card { Expire = new Expire(11, DateTime.Now.Year + oneYear) });
            Assert.True(IsDateValid);
        }

        [Fact]
        public void IsCardPanGreater_16_ReturnsFalse()
        {
            string incorrectPan = "4  5  6  1     2  6  1  2     1  2  3  4     5  4  6  4";
            bool IsPanValid = CardParamsValidator.IsCardPanValidateTrue(new Card { Pan = incorrectPan });
            Assert.False(IsPanValid);
        }

        [Fact]
        public void IsCardPanLess_16_ReturnsFalse()
        {
            string incorrectPan = "4 5 6 1 2 6 1 2";
            bool IsPanValid = CardParamsValidator.IsCardPanValidateTrue(new Card { Pan = incorrectPan });
            Assert.False(IsPanValid);
        }

        [Fact]
        public void IsCardWithNoDigitSymbolInPanReturnsFalse()
        {
            string correctPan = "4561261212345A67";
            bool IsPanValid = CardParamsValidator.IsCardPanValidateTrue(new Card { Pan = correctPan });
            Assert.False(IsPanValid);
        }

        [Fact]
        public void IsCardWithIncorrectPanReturnsfalse()
        {
            string incorrectPan = "4561261212345464";
            bool IsPanValid = CardParamsValidator.IsCardPanValidateTrue(new Card { Pan = incorrectPan });
            Assert.False(IsPanValid);
        }

        [Fact]
        public void IsCardWithCorrectPanReturnsTrue()
        {
            string correctPan = "4561261212345467";
            bool IsPanValid = CardParamsValidator.IsCardPanValidateTrue(new Card { Pan = correctPan });
            Assert.True(IsPanValid);
        }
    }
   

}
