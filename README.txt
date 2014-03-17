Learning Caliburn Micro
==========

The goal of this repository is to learn Caliburn Micro(CM).
Doing this in small steps can help others learning it.

These samples are not to be used as a guide for best practice, it are
Examples that should help with learning CM.

All of these WPF samples are using
NuGet:
   - Caliburn.Micro version: 1.5.2
   - PropertyTools.Wpf version: 2014.1.13.1

Sample 1 Is only using the Shell no extra views are used.

Sample 2 We have split the view in two views. The shell view has now an
         tabcontrol that will contain the other views.
         Some code have been moved to an Lib class. More can be done
         but not now.

Sample 3 We now have a TabControl that is filled from the click on an menu item.
         For now we use the same Views as in sample 2. Closing all the tabs
         will not close the program, you now have the option to open the tabs again.
