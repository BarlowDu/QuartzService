using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace QuartzService.Web.Utils
{
    public class MultipartFormDataConverter
    {
        public NameValueCollection FormData { get; private set; }
        
        public Dictionary<string, FileContent> FileContents { get; private set; }
        public MultipartFormDataConverter(HttpContent content)
        {
            FormData = new NameValueCollection();
            FileContents = new Dictionary<string, FileContent>();

            Task.Factory
                    .StartNew(() =>
                    {
                        var provider = content.ReadAsMultipartAsync().Result;
                        foreach (var _content in provider.Contents)
                        {
                            if (_content.Headers.ContentDisposition.FileName == null)
                            {
                                FormData.Add(_content.Headers.ContentDisposition.Name.Trim('\"'), _content.ReadAsStringAsync().Result);

                            }
                            else
                            {
                                FileContents.Add(_content.Headers.ContentDisposition.Name.Trim('\"'), new FileContent(_content));
                            }
                        }

                    },
                    CancellationToken.None,
                    TaskCreationOptions.LongRunning, // guarantees separate thread
                    TaskScheduler.Default)
                    .Wait();

        }

    }

    public class FileContent
    {


        public FileContent(HttpContent content)
        {
            Content = content;
            Name = content.Headers.ContentDisposition.Name.Trim('\"');
            FileName = content.Headers.ContentDisposition.FileName.Trim('\"');
        }


        public HttpContent Content { get; private set; }

        public string Name { get; private set; }

        public string FileName { get; private set; }
        public void Save(string fileFullName)
        {
            if (Content.Headers.ContentDisposition.FileName != null)
            {
                using (FileStream stream = new FileStream(fileFullName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    Content.CopyToAsync(stream).Wait();
                    stream.Close();
                }
            }
        }
    }


}