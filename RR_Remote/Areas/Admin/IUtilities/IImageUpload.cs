namespace RR_Remote.Areas.Admin.IUtilities
{
    public interface IImageUpload
    {
        public string UploadImage(IFormFile File, string folderName);
    }
}
