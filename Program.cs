using MetadataExtractor;
using MetadataExtractor.Formats.FileSystem;
using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata.Ecma335;


IConfiguration Configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();

var imagesPath = Configuration.GetValue<string>("myImagesPath");
//var imagesPath = Configuration.GetValue("string", "imagesPath");



DirectoryInfo di = new DirectoryInfo(imagesPath);
string[] cameraList = new string[]
{
"Blackmagic Design",
"VisionTek",
"Advert Tech",
"Aigo",
"Akaso",
"DJI",
"Foscam",
"Insta360",
"Seagull Camera",
"Phase One",
"Thomson",
"AgfaPhoto",
"Leica",
"Medion",
"Minox",
"Praktica",
"Rollei",
"Tevion",
"Traveler",
"Vageeswari",
"Canon",
"Casio",
"Epson",
"Fujifilm",
"Nikon",
"Olympus",
"Ricoh",
"Panasonic",
"Pentax",
"Sigma",
"Sony",
"Samsung",
"Hasselblad",
"Memoto",
"BenQ",
"Genius",
"Bell & Howell",
"GE",
"GoPro",
"HP",
"Kodak",
"Lytro",
"Polaroid",
"Vivitar",

};

foreach (FileInfo file in di.GetFiles())
{
    IEnumerable<MetadataExtractor.Directory> imageInfo = ImageMetadataReader.ReadMetadata(file.FullName);
    MetadataExtractor.Directory? exifInfo = imageInfo.FirstOrDefault(d => d.Name.ToUpper().Contains("EXIF IFD0"));

    if (exifInfo != null)
    {
        Tag? makeTag = exifInfo.Tags.Where(d => d.Name.ToUpper().Contains("MAKE")).FirstOrDefault();
        if (makeTag != null)
        {
            foreach (string camera in cameraList)
            {
                if (makeTag.Description.ToUpper().Contains(camera.ToUpper()))
                {
                    fileHandler.createFoldersAndMoveFile(imagesPath, camera.ToUpper(), file);
                }
            }
        }
    }
}


static public class fileHandler
{
    public static void createFoldersAndMoveFile(string origPath, string  camera, FileInfo file)
    {
        System.IO.Directory.CreateDirectory(origPath);
        if (!System.IO.Directory.Exists(origPath)) { throw new Exception("Sorry, but we couldn't create the root path");  }
        DirectoryInfo di = new DirectoryInfo(origPath);
        string newLocation = Path.Combine(di.FullName, camera, file.Name);
        System.IO.Directory.CreateDirectory(Path.GetDirectoryName(newLocation));
        file.MoveTo(newLocation);
    } 
}




