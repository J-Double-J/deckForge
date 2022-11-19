namespace DeckForge.HelperObjects
{
    /// <summary>
    /// A class that handles verifying that costs are properly paid.
    /// </summary>
    public static class CostVerifier
    {
        /// <summary>
        /// Verifies if the passed in cost and paid resources dictionaries are the same.
        /// </summary>
        /// <param name="cost">Listed cost of some action or resource.</param>
        /// <param name="paidResources">Number of resources paid to cover the cost.</param>
        /// <returns><c>true</c> if dictionaries have the same <see cref="KeyValuePair"/>s, otherwise <c>false</c>.</returns>
        public static bool VerifyPaymentExactly(Dictionary<Type, int> cost, Dictionary<Type, int> paidResources)
        {
            bool equal = false;
            if (cost.Count == paidResources.Count)
            {
                equal = true;
                foreach (var costPair in cost)
                {
                    if (paidResources.TryGetValue(costPair.Key, out int value))
                    {
                        if (value != costPair.Value)
                        {
                            equal = false;
                            break;
                        }
                    }
                    else
                    {
                        equal = false;
                        break;
                    }
                }
            }

            return equal;
        }

        /// <summary>
        /// Verifies if the minimum number of resources were payed, and if they were what the remaining
        /// resources are.
        /// </summary>
        /// <param name="cost">Number of resources that are required to be payed.</param>
        /// <param name="paidResources">Number of resources that were paid to cover the cost.</param>
        /// <param name="overpayedResources">Difference in resources if payment was succesful. Otherwise output is <c>null</c>
        /// if payment failed.</param>
        /// <returns><c>true</c> if payment covered the cost, otherwise <c>false</c>. <paramref name="overpayedResources"/> will
        /// show the difference in paid resources if <c>true</c>, otherwise will be null.</returns>
        public static bool VerifyMinimumPayment(
            Dictionary<Type, int> cost,
            Dictionary<Type, int> paidResources,
            out Dictionary<Type, int>? overpayedResources)
        {
            overpayedResources = paidResources;

            bool payed = false;
            if (paidResources.Count >= cost.Count)
            {
                payed = true;
                foreach (var costPair in cost)
                {
                    if (paidResources.TryGetValue(costPair.Key, out int paidValue))
                    {
                        if (paidValue < costPair.Value)
                        {
                            payed = false;
                            break;
                        }
                        else
                        {
                            overpayedResources[costPair.Key] = paidValue - costPair.Value;
                        }
                    }
                    else
                    {
                        payed = false;
                        break;
                    }
                }
            }

            if (!payed)
            {
                overpayedResources = null;
            }

            return payed;
        }
    }
}
