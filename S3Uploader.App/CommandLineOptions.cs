using CommandLine;

namespace S3Uploader.App
{
    internal class CommandLineOptions
    {
        [Option('p', "path", Required = true)]
        public string Path { get; set; }

        [Option('b', "bucket", Required = true)]
        public string Bucket { get; set; }
    }
}
