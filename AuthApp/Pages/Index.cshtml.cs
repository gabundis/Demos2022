using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Web;

namespace AuthApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ITokenAcquisition _tokenAcquisition;
        public string accessToken;
        public string blobContent;

        public IndexModel(ILogger<IndexModel> logger, ITokenAcquisition tokenAcquisition)
        {
            _logger = logger;
            _tokenAcquisition = tokenAcquisition;
        }

        [Obsolete]
        public async Task OnGet()
        {
            TokenAcquisitionTokenCredential credential = new(_tokenAcquisition);

            Uri blobUri = new("https://gabundisaz204storage.blob.core.windows.net/data/script.sql");
            BlobClient blobClient = new(blobUri, credential);

            MemoryStream ms = new();
            blobClient.DownloadTo(ms);
            ms.Position = 0;

            StreamReader sr = new(ms);
            blobContent = sr.ReadToEnd();
        }
    }
}