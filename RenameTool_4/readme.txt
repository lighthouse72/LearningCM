Small sample.
The Menu from sample 3 has been updated, from one level to an nested menu.
The TabView has moved to Modules.Tabs.TabsView, the TabsViewModel is now the
tabs manager with help of the ITabsManager.
The menu is still loaded with help op the ViewModels.
The code style is now more base on StyleCop, and has been moved around to get
an better layout.

How does it work?
With the help of mef we load the ViewModels, this how we load the menu and the
TabView. First the menu and tab managers are created, then the managers is
passed around pass to the constructors that needs them. If we need the one of
the two we use the interface (Menu.IMenuManager menuManager, Tabs.ITabsManager
tabManager) so that mef knows that we need the created instance(s).

Shell:
In the we only use the menu/tab managers, all the rest is done in the managers.

Menu:
While loading an module, the menu item is created and add to the menuManager if
needed. If the menu item is clicked the Execute method runs the method that is
passed (if any)while creating the menu item.

Tab:
The tabManger is passed around and stored in the modules that are tab items, if
the menu item is clicked the method that is executed will pass the class that it
belongs to, to the tabManager. If the manager thus not yet contain the class it
will add it and then show it, otherwise it will just activated it.
Closing the tab causes the class to be removed from the manager

Copy/Move
As you can see in the constructor, we create the menu item and add it to the
menuManager. The tabManager is stored in an local variable, it is used to
show the tabview.
