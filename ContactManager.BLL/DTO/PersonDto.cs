using System;
using System.Collections.Generic;

namespace ContactManager.BLL.DTO
{
    public class PersonDto
    {
        public string Name { get; set; }
        public string DateOfBirth { get; set; }
        public string Married { get; set; }
        public string Phone{ get; set; }
        public string Salary { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class PersonsDto
    {
        public List<PersonDto> personDtos { get; set; }
    }
}
