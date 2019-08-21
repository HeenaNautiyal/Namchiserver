using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using MNServer.Models;

namespace MNServer.ImageConverter
{
    public class ImageConverter
    {
        public static string base64String = null;

        public  static string ImageToBase64(HttpPostedFileBase files)
        {
           
             string FileExt = Path.GetExtension(files.FileName).ToUpper();

            if (FileExt == ".PDF")
            {
                Stream str = files.InputStream;
                BinaryReader Br = new BinaryReader(str);
                Byte[] FileDet = Br.ReadBytes((Int32)str.Length);
                base64String = Convert.ToBase64String(FileDet);

                Tbl_BirthRegistration Fd = new Tbl_BirthRegistration();
                Fd.FileName = files.FileName;
                Fd.FileContent = FileDet.ToString();
                return base64String;
            }

            return base64String;
        }
    }
}