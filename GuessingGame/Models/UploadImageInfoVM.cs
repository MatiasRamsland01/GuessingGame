using System.ComponentModel.DataAnnotations;

namespace GuessingGame.Models;

public class UploadImageInfoVM
{
    [Required]
    public IFormFile? Image { get; set; }
    [Required]
    public string? Category { get; set; }
    [Required]
    public string? ImageName { get; set; }
    [Required]
    public bool ManualSlice { get; set; }

    public string SlicedImageBase64 { get; set; } = null!;

    public string Colors { get; set; } = null!;

    //lerret

}