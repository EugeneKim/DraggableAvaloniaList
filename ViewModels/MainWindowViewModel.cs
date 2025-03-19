// ReSharper disable InconsistentNaming
namespace DraggableAvaloniaList.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
	private static readonly Random RandomGenerator = new();

	[ObservableProperty]
	private int selectedPeopleIndex;

	[ObservableProperty]
	private ObservableCollection<PersonViewModel> people;
	
	[ObservableProperty]
	private ObservableCollection<PersonViewModel> allPeople;

	[RelayCommand]
	private void AddItem() => People.Add(SummonRandomPerson());

	[RelayCommand]
	private void DeleteItem()
	{
		if (SelectedPeopleIndex > 0)
			People.RemoveAt(SelectedPeopleIndex);
	}

	[RelayCommand]
	private void DeleteAllItems() => People.Clear();

	[RelayCommand]
	private void NotifyMoveItemComplete(object _)
	{
		Debug.WriteLine("NotifyMoveItemCompleteCommand event triggered.");

		/*
		Debug.WriteLine("AllPeople");
		foreach (var p in AllPeople)
			Debug.WriteLine($"- {p.Name}");
		*/
		
		Debug.WriteLine("People");
		foreach (var p in People)
			Debug.WriteLine($"- {p.Name}");
	}

	public MainWindowViewModel()
	{
		AllPeople =
		[
			new PersonViewModel { Name = "Alice Johnson", Age = 28, Gender = "Female" },
			new PersonViewModel { Name = "Bob Smith", Age = 35, Gender = "Male" },
			new PersonViewModel { Name = "Charlie Brown", Age = 22, Gender = "Male" },
			new PersonViewModel { Name = "Diana White", Age = 30, Gender = "Female" },
			new PersonViewModel { Name = "Ethan Harris", Age = 40, Gender = "Male" },
			new PersonViewModel { Name = "Fiona Clark", Age = 26, Gender = "Female" },
			new PersonViewModel { Name = "George Miller", Age = 50, Gender = "Male" },
			new PersonViewModel { Name = "Hannah Wilson", Age = 33, Gender = "Female" },
			new PersonViewModel { Name = "Ian Martinez", Age = 29, Gender = "Male" },
			new PersonViewModel { Name = "Jessica Davis", Age = 27, Gender = "Female" }
		];
		
		People = [AllPeople[0], AllPeople[1], AllPeople[2]];
	}

	private PersonViewModel SummonRandomPerson()
	{
		string[] firstNames =
		[
			"Alice", "Bob", "Charlie", "Diana", "Ethan", "Fiona", "George", "Hannah", 
			"Ian", "Jessica", "Kevin", "Laura", "Michael", "Natalie", "Oliver", "Paula",
			"Quentin", "Rachel", "Samuel", "Tina", "Victor", "Wendy", "Xavier", "Yvonne",
			"Zachary", "Olivia", "Benjamin", "Sophia", "Daniel", "Emily", "Henry", "Grace",
			"Lucas", "Emma", "Jack", "Lily", "Owen", "Mia", "Noah", "Ava", "Liam", "Chloe"
		];
		
		string[] lastNames =
		[
			"Johnson", "Smith", "Brown", "White", "Harris", "Clark", "Miller", "Wilson",
			"Martinez", "Davis", "Anderson", "Thompson", "Scott", "Parker", "Reed", "Lewis",
			"Adams", "Green", "Turner", "Evans", "Foster", "Collins", "Stewart", "Mitchell",
			"Morgan", "Bennett", "Gray", "Bailey", "Sanders", "Carter", "Price", "Cooper",
			"Bell", "Ward", "Fisher", "Wells", "Richardson", "Gibson", "Simpson", "Hawkins"
		];

		var maxTry = 1000;

		while (maxTry-- > 0)
		{
			var firstName = firstNames[RandomGenerator.Next(firstNames.Length)];
			var lastName = lastNames[RandomGenerator.Next(lastNames.Length)];

			var fullName = $"{firstName} {lastName}";

			if (People.All(person => person.Name != firstName))
			{
				var age = RandomGenerator.Next(20, 45);
				var gender = RandomGenerator.Next(2) == 0 ? "Female" : "Male";

				return new PersonViewModel
				{
					Name = fullName,
					Age = age,
					Gender = gender
				};
			}
		}

		throw new InvalidOperationException("Unable to summon a random person");
	}
}