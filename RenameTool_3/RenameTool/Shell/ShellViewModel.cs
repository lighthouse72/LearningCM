namespace RenameTool.Shell
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using Caliburn.Micro;
    using System;
    using System.Collections.ObjectModel;
    [Export(typeof(IShell))]
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive, IShell
    {
        #region Constructor
        [ImportingConstructor]
        public ShellViewModel([ImportMany]IEnumerable<IScreen> s)
        {
            // Save the screens in the Conductor base.
            Items.AddRange(s);
            activeTab = ActiveItem;
        }
        #endregion Constructor

        private IScreen _activeTab;
        /// <summary>
        /// This is the item that should be active in the tabcontrol.
        /// </summary>
        public IScreen activeTab
        {
            get { return _activeTab; }
            set
            {
                if (_activeTab == value) return;
                _activeTab = value;
                NotifyOfPropertyChange(() => activeTab);
            }
        }

        ObservableCollection<IScreen> _tabItems = new ObservableCollection<IScreen>();
        /// <summary>
        /// Here we store the items that are in tabcontrol.
        /// </summary>
        public ObservableCollection<IScreen> tabItems
        {
            get { return _tabItems; }
        }

        /// <summary>
        /// Deactives the specified item
        /// </summary>
        /// <param name="item">The item to close.</param>
        /// <param name="close">Indicates whether or not to close the item after deactivating it.</param>
        /// <remarks>If the last tab is closed, the app will close.</remarks>
        public override void DeactivateItem(IScreen item, bool close)
        {
            // If we use this we will be changing the menu
            //base.DeactivateItem(item, close);

            int index = _tabItems.IndexOf(item);
            _tabItems.RemoveAt(index);
        }

        public override void ActivateItem(IScreen item)
        {
            base.ActivateItem(item);

            if (!_tabItems.Contains(ActiveItem))
            {
                _tabItems.Add(ActiveItem);
                NotifyOfPropertyChange(() => tabItems);
            }

            // We want to select this item in the tabcontrol.
            activeTab = item;
        }

    } // ShellViewModel
} // namespace QuickRenameTool.Shell