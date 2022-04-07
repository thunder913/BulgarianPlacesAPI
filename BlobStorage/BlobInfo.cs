using System.IO;

namespace BlobStorage
{
    public class BlobInfo
    {
        public BlobInfo(Stream content, string contentType)
        {
            this.Content = content;
            this.ContentType = contentType;
        }
        public Stream Content { get; set; }
        public string ContentType { get; set; }
    }
}
