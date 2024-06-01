using System.ComponentModel.DataAnnotations;

namespace OneToMany.ViewModels.Sliders
{
    public class SliderCreateVM
	{
		[Required]
		public List<IFormFile> Image { get; set; }
	}
}

