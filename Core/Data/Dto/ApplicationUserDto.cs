﻿namespace Core.Data.Dto {
    public class ApplicationUserDto {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public long ProfileId { get; set; }

        //public string Name { get; set; }
        //public string SurName { get; set; }
        //public string MiddleName { get; set; }
        //public string[] Roles { get; set; }
    }
}
