using BLL.Dto;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;

namespace BLL.Helper
{
    public static class UploadExcelSheets
    {
        public static async Task<List<T>> UploadSheet<T>(this IFormFile file) where T :new()
        {
            if (file is null || file.Length == 0)

                throw new Exception("File Not Valid It is Empty Or Damaged");

            var filePath = Path.Combine("Uploads", file.FileName);
            List<T> customers = new();

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelPackage.License.SetNonCommercialPersonal("Ali");
                var worksheet = package.Workbook.Worksheets[0];
                int columnCount = worksheet.Dimension.Columns;
                int rowCount = worksheet.Dimension.Rows;

                Dictionary<string, int> columnMapping = new();
                //Get The Properties Names 
                var classProperties = typeof(T).GetProperties().Select(p => p.Name).ToList();

                for (int col = 1; col <= columnCount; col++)
                {
                    string? header = worksheet.Cells[1, col].Value.ToString()?.Trim();
                    if (!string.IsNullOrEmpty(header))
                        columnMapping[header] = col; 
                }

                // Validate Headers
                var missingHeaders = classProperties.Except(columnMapping.Keys).ToList();
                if (missingHeaders.Any())
                    throw new Exception($"Missing Headers: {string.Join(", ", missingHeaders)}");

                for (int row = 2; row <= rowCount; row++)
                {
                    T Values = new();

                    foreach (var prop in typeof(T).GetProperties())
                    {
                        if (columnMapping.TryGetValue(prop.Name, out int colIndex))
                        {
                            var cellValue = worksheet.Cells[row, colIndex].Value;

                            if (cellValue != null)
                            {
                                var convertedValue = Convert.ChangeType(cellValue, prop.PropertyType);
                                prop.SetValue(Values, convertedValue);
                            }
                        }
                    }
                    customers.Add(Values);
                }  
                
            }
            return customers;
        }
    }
}
