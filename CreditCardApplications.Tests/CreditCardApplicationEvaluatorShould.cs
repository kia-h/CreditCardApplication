using System;
using Xunit;
using Moq;
using Range = Moq.Range;

namespace CreditCardApplications.Tests
{
    public class CreditCardApplicationEvaluatorShould
    {
        [Fact]
        public void AcceptHighIncomeApplications()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var application = new CreditCardApplication
            {
                GrossAnnualIncome = 100_000
            };
            CreditCardApplicationDecision decision = sut.Evaluate(application);
            Assert.Equal(CreditCardApplicationDecision.AutoAccepted,decision);

        }

        [Fact]
        public void ReferYoungApplications()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            mockValidator.Setup(m => m.IsValid(It.IsAny<string>())).Returns(true);
            //mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");
            mockValidator.DefaultValueProvider = DefaultValueProvider.Mock;
            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var application = new CreditCardApplication { Age = 19};
            CreditCardApplicationDecision decision = sut.Evaluate(application);
            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman,decision);
        }

        [Fact]
        public void DeclinedLowIncomeApplication()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            //mockValidator.Setup(m => m.IsValid(It.IsAny<string>())).Returns(true);
            //mockValidator.Setup(m => m.IsValid(It.Is<string>(number=>number.StartsWith('x')))).Returns(true);
            //mockValidator.Setup(m => m.IsValid(It.IsIn("x","y","z"))).Returns(true);
            mockValidator.Setup(m => m.IsValid(It.IsInRange("a","z",Range.Inclusive))).Returns(true);
            mockValidator.Setup((x => x.ServiceInformation.License.LicenseKey)).Returns(("OK"));
            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var application = new CreditCardApplication {GrossAnnualIncome = 19_999,Age =42,FrequentFlyerNumber ="x"};
            var decision = sut.Evaluate(application);
            Assert.Equal(CreditCardApplicationDecision.AutoDeclined,decision);
        }

        [Fact]
        public void ReferWhenLicenseKeyExpired()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();
           
            mockValidator.Setup(m => m.IsValid(It.IsAny<string>())).Returns(true);
            //var mockLicenseData = new Mock<IFrequentFlyerNumberValidator.ILicenseData>();
            //mockLicenseData.Setup(x => x.LicenseKey).Returns("Expired");
            //var mockServiceInfo = new Mock<IFrequentFlyerNumberValidator.IServiceInformation>();
            //mockServiceInfo.Setup(x => x.License).Returns(mockLicenseData.Object);

            //mockValidator.Setup(x => x.ServiceInformation).Returns(mockServiceInfo.Object);

            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("EXPIRED");

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var application = new CreditCardApplication { Age = 42 };
            var decision = sut.Evaluate(application);
            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }


        string GetLicenseKeyExpiryString()
        {
            return "EXPIRED";
        }
    }
}
