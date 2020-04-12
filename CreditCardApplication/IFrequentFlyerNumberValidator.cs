using System;

namespace CreditCardApplications
{
    public interface IFrequentFlyerNumberValidator
    {

        public interface ILicenseData
        {
            string LicenseKey { get; }        
        }

        public interface IServiceInformation
        {
            ILicenseData License{ get; set; }
        }
        bool IsValid(string frequentFlyerNumber);
        void IsValid(string frequentFlyerNumber, out bool isValid);

        IServiceInformation ServiceInformation { get; }
    }
}