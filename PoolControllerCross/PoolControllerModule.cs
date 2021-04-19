﻿using Autofac;
using eHub.Common.Api;
using eHub.Common.Models;
using eHub.Common.Services;

namespace PoolControllerCross
{
    public class PoolControllerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(ctx =>
            {
                Configuration config = ctx.Resolve<Configuration>();
                return new WebInterface(config);
            }).As<IWebInterface>();

            builder.Register(ctx =>
            {
                IWebInterface webApi = ctx.Resolve<IWebInterface>();
                return new PoolApi(webApi);
            }).As<IPoolApi>();

            builder.Register(ctx =>
            {
                IPoolApi poolApi = ctx.Resolve<IPoolApi>();
                return new PoolService(poolApi);
            }).As<IPoolService>();
        }
    }
}