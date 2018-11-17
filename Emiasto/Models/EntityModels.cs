using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Emiasto.Models
{
    public class Facility
    {
        [Key]
        [Column("Id")]
        [Display(Name = "Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
            
        [Column("Category Id")]
        [Display(Name = "Category Id")]
        public int CategoryId { get; set; }

        [Required]
        [Column("Name")]
        [Display(Name = "Name")]
        [StringLength(100, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Name { get; set; }

        [Required]
        [Column("Address")]
        [Display(Name = "Address")]
        [StringLength(100, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Address { get; set; }
            
        [Column("Website")]
        [DataType(DataType.Url)]
        [Display(Name = "Website")]
        [StringLength(100, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Website { get; set; }
            
        [Column("Phone")]
        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(100, ErrorMessage = "{0} cannot be longer than {1} characters and must be at least {2} characters long.", MinimumLength = 9)]
        public string Phone { get; set; }
            
        [Column("Category")]
        [ForeignKey("CategoryId")]
        [Display(Name = "Category")]
        public virtual Category Category { get; set; }
            
        [Column("Reviews")]
        [Display(Name = "Reviews")]
        public virtual ICollection<Review> Reviews { get; set; }
            
        [Column("Images")]
        [Display(Name = "Images")]
        public virtual ICollection<FacilityImage> Images { get; set; }
    }

    public class FacilityImage
    {
        [Key]
        [Column("Id")]
        [Display(Name = "Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
            
        [Column("Facility Id")]
        [Display(Name = "Facility Id")]
        public int FacilityId { get; set; }

        [Required]
        [Column("Image")]
        [Display(Name = "Image")]
        [StringLength(100, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        [RegularExpression(@"^\S+\.(?i)jpg|png$", ErrorMessage = "{0} must be image file with .jpg or .png extension.")]
        public string Image { get; set; }
            
        [Column("Facility")]
        [ForeignKey("FacilityId")]
        [Display(Name = "Facility")]
        public virtual Facility Facility { get; set; }
    }

    public class Category
    {
        [Key]
        [Column("Id")]
        [Display(Name = "Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            
        public int Id { get; set; }
            
        [Column("Parent Id")]
        [Display(Name = "Parent Id")]
        public int? ParentId { get; set; }

        [Required]
        [Column("Name")]
        [Display(Name = "Name")]
        [StringLength(100, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Name { get; set; }
            
        [Column("Parent")]
        [ForeignKey("ParentId")]
        [Display(Name = "Parent")]
        public virtual Category Parent { get; set; }
            
        [Column("Facilities")]
        [Display(Name = "Facilities")]
        public virtual ICollection<Facility> Facilities { get; set; }
    }

    public class Review
    {
        [Key]
        [Column("Id")]
        [Display(Name = "Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
            
        [Column("Facility Id")]
        [Display(Name = "Facility Id")]
        public int FacilityId { get; set; }

        [Required]
        [Column("User Id")]
        [Display(Name = "User Id")]
        [StringLength(100, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string UserId { get; set; }

        [Column("Rating")]
        [Display(Name = "Rating")]
        [Range(typeof(double), "0", "5", ErrorMessage = "{0} must be between {1} and {2}")]
        public double Rating { get; set; }
            
        [Column("Text")]
        [Display(Name = "Text")]
        [DataType(DataType.MultilineText)]
        [StringLength(250, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Text { get; set; }

            
        [Column("Release")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Release")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Release { get; set; }
            
        [Column("Facility")]
        [ForeignKey("FacilityId")]
        [Display(Name = "Facility")]
        public virtual Facility Facility { get; set; }

        [Column("User")]
        [ForeignKey("UserId")]
        [Display(Name = "User")]
        public virtual ApplicationUser User { get; set; }

        [Column("Images")]
        [Display(Name = "Images")]
        public virtual ICollection<ReviewImage> Images { get; set; }
            
        [Column("Likes")]
        [Display(Name = "Likes")]
        public virtual ICollection<ReviewLike> Likes { get; set; }
    }

    public class ReviewImage
    {
        [Key]
        [Column("Id")]
        [Display(Name = "Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
            
        [Column("Review Id")]
        [Display(Name = "Review Id")]
        public int ReviewId { get; set; }

        [Required]
        [Column("Image")]
        [Display(Name = "Image")]
        [StringLength(100, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        [RegularExpression(@"^\S+\.(?i)jpg|png$", ErrorMessage = "{0} must be image file with .jpg or .png extension.")]
        public string Image { get; set; }
            
        [Column("Review")]
        [ForeignKey("ReviewId")]
        [Display(Name = "Review")]
        public virtual Review Review { get; set; }
    }

    public class ReviewLike
    {
        [Key]
        [Column("Id")]
        [Display(Name = "Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("Review Id")]
        [Display(Name = "Review Id")]
        public int ReviewId { get; set; }
        
        [Column("User Id")]
        [Display(Name = "User Id")]
        [StringLength(100, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string UserId { get; set; }

        [Column("Review")]
        [ForeignKey("ReviewId")]
        [Display(Name = "Review")]
        public virtual Review Review { get; set; }

        [Column("User")]
        [ForeignKey("UserId")]
        [Display(Name = "User")]
        public virtual ApplicationUser User { get; set; }
    }
}
