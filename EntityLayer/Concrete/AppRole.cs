using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Signatures;
using Microsoft.AspNetCore.Identity;

namespace EntityLayer.Concrete
{
    public class AppRole : IdentityRole<int>
    {
    }
}
