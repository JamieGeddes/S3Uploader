using S3Uploader.Core;
using System;
using System.Threading.Tasks;

namespace S3Uploader.App
{
    public static class Program
    {
        public static void Main(string[] args) => MainAsync(args).GetAwaiter().GetResult();

        private static async Task MainAsync(string[] args)
        {
            var options = new CommandLineOptions();

            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                var uploader = new AwsS3Uploader();

                var result = await uploader.AttemptUpload(options.Path, options.Bucket);

                if (result.IsFailure) Console.WriteLine(result.Error);
            }
            else
            {
                Console.WriteLine("Unable to parse command line arguments");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
