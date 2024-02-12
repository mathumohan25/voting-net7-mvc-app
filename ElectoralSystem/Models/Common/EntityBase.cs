using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectoralSystem.Models.Common
{
    public abstract class EntityBase 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
        [BindNever]
        public string CreatedBy { get; set; }
        [BindNever]
        public DateTime CreatedDate { get; set; }
        [BindNever]
        public string? LastModifiedBy { get; set; }
        [BindNever]
        public DateTime? LastModifiedDate { get; set; }
    }
}
