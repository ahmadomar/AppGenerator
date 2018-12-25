namespace AppsGenerator.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Member
    {
        public Member()
        {
            Applications = new HashSet<Application>();
        }

        public int id { get; set; }

        public DateTime created_at { get; set; }

        [Required]
        [StringLength(25)]
        public string username { get; set; }

        [Required]
        [StringLength(100)]
        public string password { get; set; }

        [Required]
        [StringLength(50)]
        public string email { get; set; }

        public bool is_active { get; set; }

        [StringLength(200)]
        public string reset_token { get; set; }

        public string confirm_token { get; set; }

        [StringLength(100)]
        public string public_id { get; set; }

        public virtual ICollection<Application> Applications { get; set; }
    }
}
