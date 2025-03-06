﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.BusinessLogic.BL.DTOs
{
    public class AuthDTOs
    {
        public class Request
        {
            public class Login
            {
                public string Username { get; set; } = null!;
                public string Password { get; set; } = null!;
            }
            
        }
    }
}
