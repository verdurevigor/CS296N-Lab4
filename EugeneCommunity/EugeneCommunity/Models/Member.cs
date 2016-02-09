using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EugeneCommunity.Models
{
    public class Member
    {
        [Key]
        public virtual int MemberId { get; set; }
        [Display(Name = "User Name")]
        public virtual string UserName { get; set; }
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual bool IsAdmin { get; set; }
    }
}