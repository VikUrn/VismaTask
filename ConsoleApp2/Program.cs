using MeetingsApp.Model;
using MeetingsApp.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MeetingsApp
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.ForegroundColor = ConsoleColor.Green;

            // MSTest Test Project

            //setup dependecy injection
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IFunctions, Functions>()
                .BuildServiceProvider();

            // service
            var functions = serviceProvider.GetService<IFunctions>();

            var application = new Application(functions);

            application.Run();
        }
    }
}
