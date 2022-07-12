using Evolution.Common.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Evolution.Common.Extensions
{
    public static class HttpRequestExtension
    {
        private static readonly FormOptions _defaultFormOptions = new FormOptions();

        public static async Task<bool> StreamFile(this HttpRequest request, Stream targetStream)
        {
            if (!request.IsMultipartContentType())
                throw new Exception($"Expected a multipart request, but got {request.ContentType}");

            var formAccumulator = new KeyValueAccumulator();
            var boundary = request.GetBoundary();
            var reader = new MultipartReader(boundary, request.Body);
            var section = await reader.ReadNextSectionAsync();
            while (section != null)
            {
                ContentDispositionHeaderValue contentDisposition;
                var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out contentDisposition);

                if (hasContentDispositionHeader)
                {
                    if (HasFileContentDisposition(contentDisposition))
                    {
                        await section.Body.CopyToAsync(targetStream);
                    }
                    else if (HasFormDataContentDisposition(contentDisposition))
                    {
                        var key = HeaderUtilities.RemoveQuotes(contentDisposition.Name);
                        var encoding = Utility.GetEncoding(section);
                        using (var streamReader = new StreamReader(
                            section.Body,
                            encoding,
                            detectEncodingFromByteOrderMarks: true,
                            bufferSize: 1024,
                            leaveOpen: true))
                        {
                            var value = await streamReader.ReadToEndAsync();
                            if (String.Equals(value, "undefined", StringComparison.OrdinalIgnoreCase))
                            {
                                value = String.Empty;
                            }

                            formAccumulator.Append(key.Value, value);

                            if (formAccumulator.ValueCount > _defaultFormOptions.ValueCountLimit)
                            {
                                throw new InvalidDataException($"Form key count limit {_defaultFormOptions.ValueCountLimit} exceeded.");
                            }
                        }
                    }
                }

                // Drains any remaining section body that has not been consumed and reads the headers for the next section.
                section = await reader.ReadNextSectionAsync();
            }
            return true;
        }
        
        public static bool IsMultipartContentType(this HttpRequest request)
        {
            return !string.IsNullOrEmpty(request?.ContentType)
                   && request.ContentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static string GetBoundary(this HttpRequest request)
        {
            var contentType = MediaTypeHeaderValue.Parse(request.ContentType);
            var boundryLength = _defaultFormOptions.MultipartBoundaryLengthLimit;

            var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary);
            if (!boundary.HasValue)
                throw new InvalidDataException("Missing content-type boundary.");

            if (boundary.Length > boundryLength)
                throw new InvalidDataException($"Multipart boundary length limit {boundryLength} exceeded.");

            return boundary.Value;
        }

        private  static bool HasFormDataContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            // Content-Disposition: form-data; name="key";
            return contentDisposition != null
                   && contentDisposition.DispositionType.Equals("form-data")
                   && string.IsNullOrEmpty(contentDisposition.FileName.Value)
                   && string.IsNullOrEmpty(contentDisposition.FileNameStar.Value);
        }

        private static bool HasFileContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            // Content-Disposition: form-data; name="myfile1"; filename="Misc 002.jpg"
            return contentDisposition != null
                   && contentDisposition.DispositionType.Equals("form-data")
                   && (!string.IsNullOrEmpty(contentDisposition.FileName.Value)
                       || !string.IsNullOrEmpty(contentDisposition.FileNameStar.Value));
        }
    }
}
