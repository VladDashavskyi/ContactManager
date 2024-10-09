using AutoMapper;
using ContactManager.BLL.DTO;
using ContactManager.DAL.Context;
using ContactManager.DAL.Entity;
using ContactManager.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;

namespace ContactManager.Services
{
    public class MainService : IMainService

    {

        private readonly TenantContext _appContext;
        private readonly IMapper _mapper;
        public MainService(TenantContext appContext, IMapper mapper)
        {
            _appContext = appContext;
            _mapper = mapper;
        }


        public async Task<IEnumerable<Person>> GetPersonsAsync()
        {
            var user = await _appContext.Persons.ToListAsync();
            
           return user;
        }

        public async Task<IEnumerable<PersonDto>> UploadFileAsync(IFormFile file)
        {
            List<PersonDto> persons = new List<PersonDto>();
            try
            {
                
                if (file != null && file.Length > 0)
                {
                    string fileExtension = Path.GetExtension(file.FileName);
                    string[] allowedExtensions = { ".csv" };

                    if (allowedExtensions.Contains(fileExtension.ToLower()))
                    {
                        var maxFileSize = 5 * 1024 * 1024;

                        var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                        Directory.CreateDirectory(uploadsFolderPath);
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(uploadsFolderPath, fileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        using (TextFieldParser parser = new TextFieldParser(path))
                        {
                            parser.TextFieldType = FieldType.Delimited;
                            parser.SetDelimiters(";");

                            Dictionary<string, string[]> parsedData = new Dictionary<string, string[]>();

                            while (!parser.EndOfData)
                            {

                                string[] fields = parser.ReadFields();
                                int count = 0;

                                if (count++ == 0)
                                {
                                    persons.Add(new PersonDto
                                    {
                                        Name = fields[0],
                                        DateOfBirth = fields[1],
                                        Married = fields[2],
                                        Phone = fields[3],
                                        Salary = fields[4],
                                        ErrorMessage = ValidateInputData(fields),
                                    });


                                    continue;
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}");
            }

            return persons;
        }

        public async Task SaveFileToDbAsync(List<PersonDto> personDtos)
        {
            _appContext.Persons.RemoveRange(_appContext.Persons);
            _appContext.SaveChanges();

            List<Person> entity = _mapper.Map<List<Person>>(personDtos);
            await _appContext.Persons.AddRangeAsync(entity);
            await _appContext.SaveChangesAsync();

        }


        private string ValidateInputData(string[] fields)
        {
            string errorMessage = string.Empty;

            if (string.IsNullOrEmpty(fields[0]))
            {
                errorMessage += "Field 'Name' can't be null.\n";
            }
            else
            {
                const string regexName = @"^[a-zA-Z ,.'-]+$";
                if (!System.Text.RegularExpressions.Regex.IsMatch(fields[0], regexName))
                {
                    errorMessage += "Field 'Name' is invalid.\n";
                }
            }

            if (string.IsNullOrEmpty(fields[1]))
            {
                errorMessage += "Field 'Date of Birth' can't be null.\n";
            }
            else
            {
                const string regexDate = @"^\d{2}/\d{2}/\d{4}$"; 
                if (!System.Text.RegularExpressions.Regex.IsMatch(fields[1], regexDate))
                {
                    errorMessage += "Field 'Date of Birth' is invalid. Use DD/MM/YYYY format.\n";
                }
            }

            if (string.IsNullOrEmpty(fields[2]))
            {
                errorMessage += "Field 'Married' can't be null.\n";
            }
            else
            {
                const string regexMarried = @"^(true|false)$";
                if (!System.Text.RegularExpressions.Regex.IsMatch(fields[2], regexMarried, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    errorMessage += "Field 'Married' must be either true or false.\n";
                }
            }

            if (string.IsNullOrEmpty(fields[3]))
            {
                errorMessage += "Field 'Phone' can't be null.\n";
            }
            else
            {
                const string regexPhone = @"^\+?\d{10,15}$"; 
                if (!System.Text.RegularExpressions.Regex.IsMatch(fields[3], regexPhone))
                {
                    errorMessage += "Field 'Phone' is invalid. Use a valid phone number format.\n";
                }
            }

            if (string.IsNullOrEmpty(fields[4]))
            {
                errorMessage += "Field 'Salary' can't be null.\n";
            }
            else
            {
                const string regexSalary = @"^\d+(\.\d{1,2})?$"; 
                if (!System.Text.RegularExpressions.Regex.IsMatch(fields[4], regexSalary))
                {
                    errorMessage += "Field 'Salary' must be a valid decimal number.\n";
                }
            }

            return errorMessage;
        }



    }
}
