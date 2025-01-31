﻿using System.Threading;
using System.Threading.Tasks;
using Elsa.Samples.UserRegistration.Web.Models;
using Elsa.Scripting.Liquid.Messages;
using Fluid;
using MediatR;

namespace Elsa.Samples.UserRegistration.Web.Handlers
{
    /// <summary>
    /// Configure the Liquid template context to allow access to certain models. 
    /// </summary>
    public class LiquidConfigurationHandler : INotificationHandler<EvaluatingLiquidExpression>
    {
        public Task Handle(EvaluatingLiquidExpression notification, CancellationToken cancellationToken)
        {
            var context = notification.TemplateContext;
            context.Options.MemberAccessStrategy.Register<User>();
            context.Options.MemberAccessStrategy.Register<RegistrationModel>();
            
            return Task.CompletedTask;
        }
    }
}