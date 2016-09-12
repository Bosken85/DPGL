//using System;
//using System.IO;
//using Digipolis.Web.Versioning;
//using Microsoft.Extensions.Configuration;

//namespace StarterKit.SwashBuckle
//{
//    public class Program
//    {
//        private readonly IApplicationEnvironment _appEnv;

//        public Program(IApplicationEnvironment appEnv)
//        {
//            _appEnv = appEnv;
//        }

//        public IConfiguration Configuration { get; set; }

//        public void Main(string[] args)
//        {
//            BuildConfiguration(args);

//            Console.WriteLine("Test succeeded");
//            Console.ReadLine();
//        }

//        private void BuildConfiguration(string[] args)
//        {
//            var builder = new ConfigurationBuilder()
//                .SetBasePath(_appEnv.ApplicationBasePath)
//                .AddJsonFile("config.json")
//                .AddCommandLine(args);

//            Configuration = builder.Build();
//        }
//    }
//}
