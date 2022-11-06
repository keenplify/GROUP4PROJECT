using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QRCoder;
using System.IO;
using System.Drawing.Imaging;

namespace GROUP4PROJECT.Services
{
    public class QRCodeService
    {
        public string GenerateBase64(string payload,QRCodeGenerator.ECCLevel eccLevel=QRCodeGenerator.ECCLevel.Q)
        {
            var generator = new QRCodeGenerator();

            var data = generator.CreateQrCode(payload, eccLevel);

            var qrCode = new QRCode(data);

            var bitmap = qrCode.GetGraphic(20);

            var ms = new MemoryStream();

            bitmap.Save(ms, ImageFormat.Jpeg);

            return Convert.ToBase64String(ms.ToArray());
        }
    }
}