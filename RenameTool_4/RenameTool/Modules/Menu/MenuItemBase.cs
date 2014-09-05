namespace RenameTool.Modules.Menu
{
    using System;
    using System.Collections.Generic;
    using Caliburn.Micro;

    public class MenuItemBase : PropertyChangedBase, IEnumerable<MenuItemBase>, IMenuItem
    {
        #region fields
        private Func<bool> canExecute;
        private string displayText = string.Empty;
        private Func<IEnumerable<IResult>> execute;
        private string name;
        #endregion fields

        #region Constructors
        protected MenuItemBase()
        {
            this.Items = new BindableCollection<MenuItemBase>();
        }

        public MenuItemBase(string name, string displayText = "")
            : this()
        {
            this.name = name;
            if (string.IsNullOrWhiteSpace(displayText))
                this.displayText = name;
            else
                this.displayText = displayText;
        }

        public MenuItemBase(string name, Func<IEnumerable<IResult>> execute, string displayText = "")
            : this(name, displayText)
        {
            this.execute = execute;
        }

        public MenuItemBase(string name, Func<IEnumerable<IResult>> execute, Func<bool> canExecute, 
                string displayText = "")
            : this(name, execute, displayText)
        {
            this.canExecute = canExecute;
        }
        #endregion Constructors

        #region Interfaces
        #region IEnumerable<MenuItemBase> Members

        public IEnumerator<MenuItemBase> GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
        #endregion Interfaces

        #region Properties
        public string DisplayText
        {
            get { return this.displayText; }
            set
            {
                if (this.displayText == value)
                    return;

                if (string.IsNullOrWhiteSpace(value))
                    this.displayText = string.Empty;
                else
                    this.displayText = Lib.String.Trim(value);

                NotifyOfPropertyChange(() => DisplayText);
            }
        }
        public string Name
        {
            get { return name; }
            protected set { ; }
        }
        public IObservableCollection<MenuItemBase> Items { get; private set; }
        #endregion Properties

        public void Add(params MenuItemBase[] items)
        {
            items.Apply(Items.Add);
        }

        //public virtual IEnumerable<IResult> Execute()
        //{
        //    return this.execute != null ? this.execute() : new IResult[] { };
        //}

        #region IMenuItem Members

        string IMenuItem.Name
        {
            get { return this.name; }
        }

        #endregion

    }
}
