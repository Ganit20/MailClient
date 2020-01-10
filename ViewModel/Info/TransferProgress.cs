using MailClient.Model;
using MailClient.View;
using MailKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace MailClient.ViewModel
{
    class TransferProgress : ITransferProgress
    {
        public void Report(long bytesTransferred, long totalSize )
        {
            int progress = (int)(100 * bytesTransferred / totalSize) ;
            new DownloadData().percent = progress;
            DownloadData.bytes = bytesTransferred.ToString() + " / " + totalSize.ToString();
        }
            
        public void Report(long bytesTransferred)
        {
            throw new NotImplementedException();
        }
    }
}
