namespace DraggableAvaloniaList.Behaviors;

public class ItemsListBoxDropHandler : DropHandlerBase
{
	public DragDropEffects DragDropEffects { get; private set; }
	
	private bool Validate<T>(ListBox listBox, DragEventArgs e, object? sourceContext, object? targetContext, bool bExecute) where T : PersonViewModel
	{
		if (sourceContext is not T sourceItem
			|| targetContext is not MainWindowViewModel vm
			|| listBox.GetVisualAt(e.GetPosition(listBox)) is not Control {DataContext: T targetItem})
			return false;

		var items = vm.People;
		var sourceIndex = items.IndexOf(sourceItem);
		var targetIndex = items.IndexOf(targetItem);

		if (sourceIndex < 0
		    || targetIndex < 0
		    || sourceIndex == targetIndex
		    || e.DragEffects != DragDropEffects.Move)
			return false;

		if (bExecute)
		{
			DragDropEffects = e.DragEffects;
			MoveItem(items, sourceIndex, targetIndex);
			DragDropEffects = DragDropEffects.None;
		}

		return true;
	}

	public override bool Validate(
		object? sender,
		DragEventArgs e,
		object? sourceContext,
		object? targetContext,
		object? state) =>
		e.Source is Control && sender is ListBox listBox && Validate<PersonViewModel>(listBox, e, sourceContext, targetContext, false);

	public override bool Execute(
		object? sender, 
		DragEventArgs e, 
		object? sourceContext, 
		object? targetContext, 
		object? state) =>
		e.Source is Control && sender is ListBox listBox && Validate<PersonViewModel>(listBox, e, sourceContext, targetContext, true);
}