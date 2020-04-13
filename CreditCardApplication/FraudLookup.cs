using System;
using System.Collections.Generic;
using System.Text;

namespace CreditCardApplication
{
    public class FraudLookup
    {
        virtual public bool IsFraudRisk(CreditCardApplications.CreditCardApplication application)
        {
            if (application.LastName == "Smith")
            {
                return true;
            }

            return false;
        }
    }
}
