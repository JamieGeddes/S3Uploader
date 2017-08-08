using CommandLine;

namespace S3Uploader.App
{
    internal class CommandLineOptions
    {
        //[Option('p', "path", Required = true)]
        [Option('p', "path")]
        public string Path { get; set; }

        //[Option('b', "bucket", Required = true)]
        [Option('b', "bucket")]
        public string Bucket { get; set; }
    }
}
