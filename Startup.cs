using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionStartup(typeof(Startup))]
namespace Name
{
    public class Startup: FunctionStartup
    {}
}