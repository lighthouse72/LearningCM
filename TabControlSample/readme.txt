The project 1 the tabcontrol is in the ShellView and the logic in the ShellViewModel.
The ShelViewModel inherits from the Conductor.

But we do not want the TabControl to be in the ShelView. In project 2 we moved the
TabControl to TabsView, the TabControl makes use of the Conductor thats why we move this
also to TabsView.
To be able to use and see the TabControl we create an property that holds the TabsView
and we add an ContentControl to the ShellView so we can display the view.

HTH