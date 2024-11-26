using System.ComponentModel.DataAnnotations;

namespace THAMCOMVC.ViewModels;

public class ProductViewModel{

    public int Id { get; set; }

    public required string ProductName { get; set; }

    public  required string  ImagePath { get; set; }

    public  required string ProductDescription { get; set; }

}