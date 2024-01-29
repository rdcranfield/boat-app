using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace boat_app_v2.Entities.Models;
[Table("boat")] 
[PrimaryKey(nameof(Code))]
public class Boat
{
    [Key] 
    [Required(ErrorMessage = "Code is required")] 
    [RegularExpression(@"^[A-Za-z]{4}-[0-9]{4}-[A-Za-z]{1}[0-9]{1}$")]
    public string Code { get; set; }  
    
    [Required(ErrorMessage = "Name is required")] 
    public string Name { get; set; }  

   // public Length Length { get; set; }
   
    public double Length { get; set; }  
    public double Width { get; set; }  //beam
}