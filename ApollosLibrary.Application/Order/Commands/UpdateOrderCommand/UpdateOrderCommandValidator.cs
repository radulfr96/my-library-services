﻿using ApollosLibrary.Application.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Order.Commands.UpdateOrderCommand
{
    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator(IDateTimeService dateTimeService)
        {
            RuleFor(c => c.OrderId).GreaterThan(0);
            RuleFor(c => c.BusinessId).GreaterThan(0);
            RuleFor(c => c.OrderDate).LessThan(dateTimeService.Now);
            RuleFor(c => c.OrderItems).NotEmpty();

            RuleForEach(c => c.OrderItems).SetValidator(new OrderItemValidator());
        }

        public class OrderItemValidator : AbstractValidator<OrderItemDTO>
        {
            public OrderItemValidator()
            {
                RuleFor(i => i.BookId).GreaterThan(0);
                RuleFor(i => i.Price).GreaterThanOrEqualTo(0.00m);
                RuleFor(i => i.Quantity).GreaterThan(0);
            }
        }
    }
}
