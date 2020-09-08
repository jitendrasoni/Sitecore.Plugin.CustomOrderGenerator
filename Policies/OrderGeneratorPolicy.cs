// © 2019 Sitecore Corporation A/S. All rights reserved. Sitecore® is a registered trademark of Sitecore Corporation A/S.

using Sitecore.Commerce.Core;
using System.Threading.Tasks;

namespace Sitecore.Commerce.Plugin.Sample
{
    /// <inheritdoc />
    /// <summary>
    /// Defines a policy
    /// </summary>
    /// <seealso cref="T:Sitecore.Commerce.Core.Policy" />
    public class OrderGeneratorPolicy : Policy
    {
        /// <summary>
        /// Gets or sets the sample entity display.
        /// </summary>
        /// <value>
        /// The sample entity display.
        /// </value>
        public string Prefix { get; set; }
        public bool IsRandomNumber { get; set; }
        public int TotalDigitMin { get; set; }
        public int TotalDigitMax { get; set; }

        public string Environment { get; set; }

        public OrderGeneratorPolicy()
        {
            Environment = "local";
            Prefix = "DN";
            IsRandomNumber = true;
            TotalDigitMin = 1000000;
            TotalDigitMax = 9999999;
        }

        public async Task<bool> IsValid(CommerceContext commerceContext)
        {
            if (!string.IsNullOrEmpty(Environment)
                && !string.IsNullOrEmpty(Prefix))
            {
                return true;
            }

            await commerceContext.AddMessage(
                    commerceContext.GetPolicy<KnownResultCodes>().Error,
                    "InvalidClientPolicy",
                    null,
                    "Invalid Customer Order Client Policy")
                .ConfigureAwait(false);
            return false;
        }
    }
}

