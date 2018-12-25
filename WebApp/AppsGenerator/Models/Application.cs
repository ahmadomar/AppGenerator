namespace AppsGenerator.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Application
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        public DateTime created_at { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name="Data Source")]
        public string db_datasource { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "Database Name")]
        public string db_name { get; set; }

        [StringLength(250)]
        [Display(Name = "User Id")]
        public string db_user_id { get; set; }

        [StringLength(250)]
        [Display(Name = "Password")]
        public string db_password { get; set; }

        public bool? generated { get; set; }

        public int member_id { get; set; }

        public int? theme_id { get; set; }

        public virtual Member Member { get; set; }

        public virtual Theme Theme { get; set; }

        [Required]
        [Display(Name = "Application Url")]
        public string url { get; set; }

        public bool? is_running { get; set; }

        public bool? published_azure { get; set; }
    }
}
