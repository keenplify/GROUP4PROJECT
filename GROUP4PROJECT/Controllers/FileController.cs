﻿using System;
using System.Web;
using System.Web.Mvc;
using GROUP4PROJECT.Helpers;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GROUP4PROJECT.Controllers
{
    public class FileController : Controller

    {
        [HttpPost]
        public async Task<ActionResult> Upload(HttpPostedFileBase file)
        {
            Debug.WriteLine(file.ContentLength);
            Debug.WriteLine(file.FileName);
            Debug.WriteLine(file.ContentType);
            try
            {
                if (file.ContentLength > 0)
                {
                    string ext  = Path.GetExtension(file.FileName);
                    string _FileName = Guid.NewGuid().ToString() + ext;

                    var bucket = Configs.Storage.GetResourceBucket();

                    byte[] imgData;

                    using (var reader = new BinaryReader(file.InputStream))
                    {
                        imgData = reader.ReadBytes(file.ContentLength);
                    }

                    var uploaded = await bucket.Upload(imgData, _FileName, new Supabase.Storage.FileOptions {
                        Upsert = true,
                    });

                    var url = bucket.GetPublicUrl(_FileName);

                    return Json(new { 
                        url
                    });

                } else
                {
                    return Json(Http.JsonError(412, "File has no content!"));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return Json(Http.JsonError(412, "Unable to upload image"));
            }
        }
    }
}