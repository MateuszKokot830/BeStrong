using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    [Table("Roles")]
    public class Role : IdentityRole<int>
    {
        
    }
}