using Microsoft.VisualStudio.TestTools.UnitTesting;
using CardService.Services.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardService.Domain;

namespace CardService.Services.Validators.Tests
{
    [TestClass()]
    public class CardParamsValidatorTests
    {
        [TestMethod()]
        public void IsCardExpireWithMonthLessZeroReturnsFalse()
        {
            bool IsDateValid =  CardParamsValidator.IsCardExpireDateValidateTrue(new Card { Expire = new Expire (-2, 2003) });
            Assert.IsFalse(IsDateValid);
        }
        [TestMethod()]
        public void IsCardExpireWithMonthEqualZeroReturnsFalse()
        {
            bool IsDateValid = CardParamsValidator.IsCardExpireDateValidateTrue(new Card { Expire = new Expire(0, 2003) });
            Assert.IsFalse(IsDateValid);
        }

        [TestMethod()]
        public void IsCardExpireWithMonthGreater_12_ReturnsFalse()
        {
            bool IsDateValid = CardParamsValidator.IsCardExpireDateValidateTrue(new Card { Expire = new Expire(-2, 2003) });
            Assert.IsFalse(IsDateValid);
        }

        [TestMethod()]
        public void IsCardExpireWithYearGreaterDateTimeNowReturnsFalse()
        {
            int oneYear = 1;
            bool IsDateValid = CardParamsValidator.IsCardExpireDateValidateTrue(new Card { Expire = new Expire(DateTime.Now.Month, DateTime.Now.Year - oneYear) });
            Assert.IsFalse(IsDateValid);
        }

        [TestMethod()]
        public void IsCardExpireFutureDateYearReturnsTrue()
        {
            int oneYear = 1;
            bool IsDateValid = CardParamsValidator.IsCardExpireDateValidateTrue(new Card { Expire = new Expire(11, DateTime.Now.Year + oneYear) });
            Assert.IsTrue(IsDateValid);
        }

        [TestMethod()]
        public void IsCardPanGreater_16_ReturnsFalse()
        {
            string incorrectPan = "4  5  6  1     2  6  1  2     1  2  3  4     5  4  6  4";
            bool IsPanValid = CardParamsValidator.IsCardPanValidateTrue(new Card { Pan = incorrectPan });
            Assert.IsFalse(IsPanValid);
        }
        
        [TestMethod()]

        public void IsCardPanLess_16_ReturnsFalse()
        {
            string incorrectPan = "4 5 6 1 2 6 1 2";
            bool IsPanValid = CardParamsValidator.IsCardPanValidateTrue(new Card { Pan = incorrectPan });
            Assert.IsFalse(IsPanValid);
        }

        [TestMethod()]
        public void IsCardWithNoDigitSymbolInPanReturnsFalse()
        {
            string correctPan = "4561261212345A67";
            bool IsPanValid = CardParamsValidator.IsCardPanValidateTrue(new Card { Pan = correctPan });
            Assert.IsFalse(IsPanValid);
        }

        [TestMethod()]
        public void IsCardWithIncorrectPanReturnsfalse()
        {
            string incorrectPan = "4561261212345464";
            bool IsPanValid = CardParamsValidator.IsCardPanValidateTrue(new Card { Pan = incorrectPan });
            Assert.IsFalse(IsPanValid);
        }

        [TestMethod()]
        public void IsCardWithCorrectPanReturnsTrue()
        {
            string correctPan = "4561261212345467";
            bool IsPanValid = CardParamsValidator.IsCardPanValidateTrue(new Card { Pan = correctPan });
            Assert.IsTrue(IsPanValid);
        }
    }
}