namespace AppsGenerator.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Theme
    {
        public Theme()
        {
            Applications = new HashSet<Application>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(70)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string FileName { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(200)]
        public string Url { get; set; }

        public virtual ICollection<Application> Applications { get; set; }
    }
}
