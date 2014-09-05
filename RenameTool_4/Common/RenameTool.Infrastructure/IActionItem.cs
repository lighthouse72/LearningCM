namespace RenameTool.Infrastructure
{
    using Caliburn.Micro;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;

    public interface IActionItem : IHaveDisplayName, IActivate, IDeactivate
    {
        /// <summary>
        /// Gets or sets the name used internally for grouping and finding the VM. If not set explicitly is the same as <see cref="DisplayName"/>
        /// </summary>
        string Name { get; set; }

        string DisplayNameShort { get; set; }
        string ToolTip { get; set; }

        /// <summary>
        /// Gets the nested items.
        /// </summary>
        ObservableCollection<IActionItem> Items { get; }

        /// <summary>
        /// Calls the underlying canExecute function.
        /// </summary>
        bool CanExecute { get; }

        /// <summary>
        /// The action associated to the ActionItem
        /// </summary>
        void Execute();
    }
}
