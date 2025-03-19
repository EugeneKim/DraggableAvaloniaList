namespace DraggableAvaloniaList.ViewModels;

public class PersonViewModel : ViewModelBase
{
	public string Name { get; set; }
	public int Age { get; set; }
	public string Gender { get; set; }

	public override string ToString() => $"Name: {Name}, Age: {Age}, Gender: {Gender}";
}