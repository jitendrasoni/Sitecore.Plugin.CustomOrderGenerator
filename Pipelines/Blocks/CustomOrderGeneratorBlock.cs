// © 2019 Sitecore Corporation A/S. All rights reserved. Sitecore® is a registered trademark of Sitecore Corporation A/S.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.Plugin.Orders;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Sitecore.Commerce.Plugin.Sample
{

    [PipelineDisplayName("Sitecore.Commerce.Plugin.Sample.CustomOrderGeneratorBlock")]
    public class CustomOrderGeneratorBlock : PipelineBlock<Order, Order, CommercePipelineExecutionContext>
    {

        private readonly FindEntitiesInListCommand _getEntitiesInListCommand;

        public CustomOrderGeneratorBlock(FindEntitiesInListCommand findEntitiesInListCommand) : base((string)null)
        {
            _getEntitiesInListCommand = findEntitiesInListCommand;
        }

        public override Task<Order> Run(Order order, CommercePipelineExecutionContext context)
        {

            order.OrderConfirmationId = GenerateCustomOrderNumber(order, context, _getEntitiesInListCommand);

            return Task.FromResult<Order>(order);
        }

        private string GenerateCustomOrderNumber(Order order, CommercePipelineExecutionContext context, FindEntitiesInListCommand findEntitiesInListCommand)
        {
            var contactComponent = order.GetComponent<ContactComponent>();
            var orders = (IEnumerable<Order>)findEntitiesInListCommand.Process<Order>(context.CommerceContext, CommerceEntity.ListName<Order>(), 0, int.MaxValue).Result.Items;
            var orderCount = orders.Count();

            if (orders.Any())
            {
                OrderGeneratorPolicy policy = context.GetPolicy<OrderGeneratorPolicy>();
                string nextOrderNumber;
                if (policy.IsRandomNumber)
                {
                    nextOrderNumber = GenerateNumber(policy.TotalDigitMin, policy.TotalDigitMax);
                }
                else
                {
                    nextOrderNumber = Convert.ToString(orderCount + 1);
                }

                return policy.Prefix + nextOrderNumber.ToString();
            }
            return Guid.NewGuid().ToString("B");
        }

        private string GenerateNumber(int min, int max)
        {
            Random r = new Random();
            int randNum = r.Next(min, max);
            return randNum.ToString();

        }

    }
}
