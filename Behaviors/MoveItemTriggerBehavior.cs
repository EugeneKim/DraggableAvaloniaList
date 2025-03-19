namespace DraggableAvaloniaList.Behaviors;

public sealed class MoveItemTriggerBehavior : StyledElementTrigger<ListBox>
{
	// ReSharper disable once MemberCanBePrivate.Global
	public static readonly StyledProperty<ICommand?> NotifyMoveItemCompleteCommandProperty =
		AvaloniaProperty.Register<MoveItemTriggerBehavior, ICommand?>(nameof(NotifyMoveItemCompleteCommandProperty));
	
	public static readonly StyledProperty<ObservableCollection<PersonViewModel>?> AllPeopleProperty =
		AvaloniaProperty.Register<MoveItemTriggerBehavior, ObservableCollection<PersonViewModel>?>(nameof(AllPeopleProperty));

	public ICommand? NotifyMoveItemCompleteCommand
	{
		get => GetValue(NotifyMoveItemCompleteCommandProperty);
		set => SetValue(NotifyMoveItemCompleteCommandProperty, value);
	}
	
	public ObservableCollection<PersonViewModel>? AllPeople
	{
		get => GetValue(AllPeopleProperty);
		set => SetValue(AllPeopleProperty, value);
	}

	protected override void OnAttached()
	{
		base.OnAttached();

		if (AssociatedObject?.Items is INotifyCollectionChanged collection)
			collection.CollectionChanged += OnCollectionChanged;
	}

	protected override void OnDetaching()
	{
		base.OnDetaching();

		if (AssociatedObject?.Items is INotifyCollectionChanged collection)
			collection.CollectionChanged -= OnCollectionChanged;
	}

	private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		if (e.Action != NotifyCollectionChangedAction.Add && e.Action != NotifyCollectionChangedAction.Remove)
			return;

		var b = Interaction.GetBehaviors(AssociatedObject!).OfType<ContextDropBehavior>().FirstOrDefault();

		if (b?.Handler is not ItemsListBoxDropHandler { DragDropEffects: DragDropEffects.Move })
			return;

		var person = e.Action switch
		{
			NotifyCollectionChangedAction.Remove => e.OldItems![0] as PersonViewModel,
			NotifyCollectionChangedAction.Add => e.NewItems?[0] as PersonViewModel,
			_ => null
		};

		if (person == null || AllPeople == null)
			return;

		switch (e.Action)
		{
			case NotifyCollectionChangedAction.Remove:
			{
				var f = AllPeople.SingleOrDefault(a => a.Name == person.Name);
				
				if (f != null)
					AllPeople.Remove(f);
				break;
			}
			case NotifyCollectionChangedAction.Add:
			{
				var people = AssociatedObject!.Items;
				var index = e.NewStartingIndex + 1;
				var pp = index == people.Count
					? null
					: (PersonViewModel)people.GetAt(index)!;

				if (pp == null)
				{
					AllPeople.Add(person);
				}
				else
				{
					var f = AllPeople.Single(a => a.Name == pp.Name);

					AllPeople.Insert(AllPeople.IndexOf(f), person);
				}

				break;
			}
		}

		if (e.Action == NotifyCollectionChangedAction.Add)
			NotifyMoveItemCompleteCommand?.Execute(null);
	}
}