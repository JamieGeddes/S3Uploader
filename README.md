# S3Uploader
A simple AWS S3 upload utility. This will upload all files and directories from a specified directory to a named AWS S3 bucket (assuming correct IAM permissions).
Note the uploader will attempt to determine the MIME content type of the files - for S3 static hosting, a bulk upload of files causes files with a .css or .html exension to be treated as content-type binary/octet-stream (see [http://markgibson.info/2013/09/css-and-amazon-s3.html](http://markgibson.info/2013/09/css-and-amazon-s3.html)).

## Usage
The utility runs as a console application, to be run with command line arguments as follows:

S3Uploader.App.exe -b "mytestbucket" -p "C:\Uploads\MyFilesToUploadToS3"
