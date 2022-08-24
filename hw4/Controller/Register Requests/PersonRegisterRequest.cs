using System;
namespace ContractControlCentre.Requests
{
    public class PersonRegisterRequest
    {
        public int Age { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
    }
}

