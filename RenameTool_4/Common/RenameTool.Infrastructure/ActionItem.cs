namespace RenameTool.Infrastructure
{
    using Caliburn.Micro;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using Action = System.Action;

    /// <summary>
    /// View model for an action item.
    /// </summary>
    public class ActionItem : PropertyChangedBase, IActionItem
    {
        protected ActionItem(string displayName)
        {
            this.DisplayName = displayName;
            this.DisplayNameShort = displayName;
            this.Name = displayName;
            this.execute = (() => { });
            this.canExecute = (() => IsActive);
        }

        /// <summary>
        /// Initializes a new instance of ActionItem class.
        /// </summary>
        /// <param name="displayName">The display name.</param>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        public ActionItem(string displayName, Action execute, Func<bool> canExecute = null)
            : this(displayName)
        {
            this.execute = execute ?? (() => { });
            this.canExecute = canExecute ?? (() => IsActive);
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; NotifyOfPropertyChange(() => Name); }
        }

        private string displayName;
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; NotifyOfPropertyChange(() => DisplayName); }
        }

        private string displayNameShort;
        public string DisplayNameShort
        {
            get { return displayNameShort; }
            set { displayNameShort = value; NotifyOfPropertyChange(() => DisplayNameShort); }
        }

        private string toolTip;
        public string ToolTip
        {
            get { return toolTip; }
            set { toolTip = value; NotifyOfPropertyChange(() => ToolTip); }
        }

        private ObservableCollection<IActionItem> items = new ObservableCollection<IActionItem>();
        public ObservableCollection<IActionItem> Items
        {
            get { return items; }
        }

        #region Execution
        private readonly Action execute;
        /// <summary>
        /// The action associated to the ActionItem
        /// </summary>
        public virtual void Execute()
        {
            this.execute();
        }

        private readonly Func<bool> canExecute;
        /// <summary>
        /// Calls the underlying canExecute function.
        /// </summary>
        public virtual bool CanExecute
        {
            get { return canExecute(); }
        }
        
        #endregion

        #region Activation & Deactivation
        public event EventHandler<ActivationEventArgs> Activated;
        public event EventHandler<DeactivationEventArgs> AttemptingDeactivation;
        public event EventHandler<DeactivationEventArgs> Deactivated;

        private bool isActive = true;
        public bool IsActive
        {
            get { return isActive; }
        }

        public void Activate()
        {
            if (IsActive)
                return;

            isActive = true;
            OnActivate();
            if (Activated != null)
                Activated(this, new ActivationEventArgs { WasInitialized = false });
            NotifyOfPropertyChange(() => CanExecute);
        }
        protected virtual void OnActivate() { }

        public virtual void Deactivate(bool close)
        {
            if (!IsActive)
                return;

            if (AttemptingDeactivation != null)
                AttemptingDeactivation(this, new DeactivationEventArgs { WasClosed = close });

            isActive = false;
            OnDeactivate(close);
            NotifyOfPropertyChange(() => CanExecute);
            if (Deactivated != null)
                Deactivated(this, new DeactivationEventArgs { WasClosed = close });
        }
        protected virtual void OnDeactivate(bool close) { }
        #endregion
    }

}
