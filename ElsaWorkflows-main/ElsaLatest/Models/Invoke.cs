using Elsa.Persistence;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elsa.Samples.UserRegistration.Web.Models
{
    public class Invoke
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public string PersonNumber { get; set; }
    }
}
