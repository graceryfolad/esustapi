

using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using Microsoft.AspNetCore.Http;
using System.IO;
using esust.Models;

namespace DataAccess.Helpers
{
    

    public static class ConfigHelper
    {
        //public static List<string> getErrors(ModelStateDictionary dictionary)
        //{
        //    var query = from state in dictionary.Values
        //                from error in state.Errors
        //                select error.ErrorMessage;

        //    var errorList = query.ToList();
        //    return errorList;
        //}
        public static string getConnectionString()
        {
            var buider = new ConfigurationBuilder();
          
            buider.AddJsonFile("appsettings.json", optional: false);
            var config = buider.Build();
            return config.GetConnectionString("DefaultConnections").ToString();
            //  return string.Empty;
        }

        public static string getAuditString()
        {
            var buider = new ConfigurationBuilder();
            buider.AddJsonFile("appsettings.json", optional: false);
            var config = buider.Build();
            return config.GetConnectionString("AuditConnections").ToString();
            //  return string.Empty;
        }

        public static string getAppSetting()
        {
            var buider = new ConfigurationBuilder();
            buider.AddJsonFile("appsettings.json", optional: false);
            var config = buider.Build();

            var appsett = config.GetSection("JWT");
            return appsett.GetSection("Key").Value;
            //  return string.Empty;
        }


        public static string getDataTyoes()
        {
           
              return string.Empty;
        }
        public static List<Providers> GetProviders()
        {
            List<Providers> providers = new List<Providers>();

            providers.Add(new Providers() {ProviderID=1,ProviderName="MS SQL" });
            providers.Add(new Providers() { ProviderID = 2, ProviderName = "ORACLE" });
            providers.Add(new Providers() { ProviderID = 3, ProviderName = "MYSQL" });
            providers.Add(new Providers() { ProviderID = 4, ProviderName = "POSTGREL" });

            return providers;
        }

        public static inProviders aProvider(int providerid, string host, string dbname, string userid, string password)
        {

            switch (providerid)
            {
                case 1:
                        
                    return new inProviders(){
                       //ProviderName = "System.Data.SqlClient" ,
                       ProviderName= "System.Data.SqlClient",

                       ConnectionString = string.Format("Data Source={0};Initial Catalog={1}; User ID={2}; password={3};Encrypt=True;TrustServerCertificate=True", host,dbname,userid,password)
                    
                    }
                    ;
                case 2:

                    

                    return new inProviders()
                    {
                        ProviderName = "Oracle.DataAccess.Client",
                        ConnectionString = string.Format("")

                    };

                case 3:
                    //server=localhost;database=aml;user=root;password=


                    return new inProviders()
                    {
                        ProviderName = "MySql.Data.MySqlClient",
                        ConnectionString = string.Format("server={0};database={1};user={2};password={3}", host, dbname, userid, password)

                    };

                case 4:
                    //Data Source=myServerAddress;location=myDataBase;User ID=myUsername;password=myPassword;timeout=1000
                    return new inProviders()
                    {
                        ProviderName = "Npgsql",
                        ConnectionString = string.Format("Data Source={0};location={1};User ID={2};password={0};timeout=1000", host, dbname, userid, password)

                    };


                      //  "Npgsql";


            }


            return null;
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

    }

    public class Providers
    {
        public int ProviderID { get; set; }
        public string ProviderName { get; set; }
    }

    public class inProviders
    {
        public int ProviderID { get; set; }
        public string ProviderName { get; set; }
        public string ConnectionString { get; set; }
    }

   

public static class FileValidator
    {
        private static readonly string[] ValidImageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };

        public static (bool IsValid, string ErrorMessage) ValidateImageFile(
            IFormFile file,
            long maxSizeBytes = 10 * 1024 * 1024, // Default 10MB
            int maxWidth = 4096,
            int maxHeight = 4096)
        {
            // 1. Check file extension
            var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            if (string.IsNullOrEmpty(extension) || !ValidImageExtensions.Contains(extension))
            {
                return (false, $"Invalid file type. Allowed: {string.Join(", ", ValidImageExtensions)}");
            }

            // 2. Check file size
            if (file.Length > maxSizeBytes)
            {
                return (false, $"File size exceeds {maxSizeBytes / 1024 / 1024}MB limit");
            }

            // 3. Check image dimensions
            try
            {
                using var image = System.Drawing.Image.FromStream(file.OpenReadStream());
                if (image.Width > maxWidth || image.Height > maxHeight)
                {
                    return (false, $"Image dimensions exceed {maxWidth}x{maxHeight}px limit");
                }
            }
            catch
            {
                return (false, "Invalid or corrupted image file");
            }
            finally
            {
                // Reset stream position for subsequent reads
                file.OpenReadStream().Position = 0;
            }

            return (true, "Validation passed");
        }

        public static string GenerateUniqueFileName(IFormFile file)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            var originalName = Path.GetFileNameWithoutExtension(file.FileName);
            var extension = Path.GetExtension(file.FileName);

            // Remove invalid characters and spaces
            var cleanName = new string(originalName
                .Where(c => !invalidChars.Contains(c))
                .Select(c => c == ' ' ? '_' : c)
                .ToArray());

            // Add timestamp for uniqueness
            return $"{DateTime.Now:yyyyMMddHHmmssfff}_{cleanName}{extension}";
        }

        public static async Task<(bool IsValid, string ErrorMessage)> UploadDocumentAsync(IFormFile file, string location)
        {

            try
            {
                var (isValid, errorMessage) = FileValidator.ValidateImageFile(
                    file,
                    maxSizeBytes: 2 * 1024 * 1024, // 2MB
                    maxWidth: 1920,
                    maxHeight: 1080);

                if (!isValid)
                    return (false, errorMessage);


                var uploadsFolder = $"{AppDomain.CurrentDomain.BaseDirectory}Upload\\{location}";
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = FileValidator.GenerateUniqueFileName(file);
                var filePath = Path.Combine(uploadsFolder, fileName);



                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return (true, filePath);
            }
            catch (Exception ex)
            {

                return (false, ex.Message);
            }

           // return (false, "Invalid or corrupted image file");
        }
    }
}
