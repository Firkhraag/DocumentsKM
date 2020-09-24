using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Model
{
    public class User
    {
        public string Login { get; set; }

        public string Password { get; set; }
    }
}
