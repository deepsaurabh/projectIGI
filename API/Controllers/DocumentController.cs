using Entity.POCO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace API.Controllers
{
    public class DocumentController : BaseController
    {
        public HttpResponseMessage PostUploadAttachment(DocumentScope scope)
        {
            HttpContext context = HttpContext.Current;
            List<AttachedDocument> attachedFiles = new List<AttachedDocument>();
            try
            {
                string erroredAtchmnts = String.Empty;
                string imageURL = String.Empty;
                //Loop through the multiple files.
                for (int i = 0; i < context.Request.Files.Count; i++)
                {
                    var file = context.Request.Files.Get(i);

                    try
                    {
                        AttachedDocument fileAttachment = new AttachedDocument()
                        {
                            //Path = savePath,
                            FileSize = file.ContentLength,
                            FileType = file.ContentType,
                            DocumentName = file.FileName,
                            Scope = scope
                        };

                        using (var binaryReader = new BinaryReader(file.InputStream))
                        {
                            fileAttachment.FileData = binaryReader.ReadBytes(file.ContentLength);
                        }

                        UnitOfWork.AttachedDocumentRepository.Insert(fileAttachment);
                        UnitOfWork.SaveChange();
                        attachedFiles.Add(fileAttachment);
                        var base64String = Convert.ToBase64String(fileAttachment.FileData, 0, fileAttachment.FileData.Length);
                        imageURL = String.Format("data:{0};base64,{1}", fileAttachment.FileType, base64String);
                    }
                    catch (Exception)
                    {
                        if (String.IsNullOrEmpty(erroredAtchmnts)) { erroredAtchmnts = "Attachments that could not be saved due to some error: " + file.FileName; }
                        else { erroredAtchmnts += ", " + file.FileName; }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { erroredAttachments = erroredAtchmnts, attachedFiles = attachedFiles, imageURL = imageURL });
            }
            catch (UnauthorizedAccessException uex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, uex);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
