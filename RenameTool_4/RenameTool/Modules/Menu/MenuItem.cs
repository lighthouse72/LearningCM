namespace RenameTool.Modules.Menu
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Caliburn.Micro;

    public class MenuItem : MenuItemBase
    {
        #region fields
        private string toolTip;
        #endregion fields

        #region Constructors
        public MenuItem() { }

        /// <summary>
        /// Create new menu item.
        /// </summary>
        /// <param name="name">Name of the menu item.</param>
        /// <param name="displayText">Text to display for this menu item.</param>
        public MenuItem(string name, string displayText = "")
            : base(name, displayText)
        {
        }

        ///// <summary>
        ///// Create new menu item.
        ///// </summary>
        ///// <param name="name">Name of the menu item.</param>
        ///// <param name="execute">Reference to the function that must be executed.</param>
        ///// <param name="displayText">Text to display for this menu item.</param>
        //public MenuItem(string name, Func<IEnumerable<IResult>> execute, string displayText = "")
        //    : base(name, execute, displayText)
        //{
        //}

        /// <summary>
        /// Create new menu item.
        /// </summary>
        /// <param name="name">Name of the menu item.</param>
        /// <param name="execute">Reference to the function that must be executed.</param>
        /// <param name="canExecute">Reference to the the function that tells us if we can use the execute function.</param>
        /// <param name="displayText">Text to display for this menu item.</param>
        public MenuItem(string name, Func<IEnumerable<IResult>> execute, Func<bool> canExecute,
                string displayText = "")
            : base(name, execute, canExecute, displayText)
        {
        }

        public MenuItem(string name, Func<IResult> execute, string displayText = "")
            : base(name, displayText)
        {
            //: base(name, execute, canExecute, displayText)
            this.execute = execute;
        }

        #endregion Constructors

        public bool HasChilderen
        {
            get { return this.Items.Count() > 0; }
            //set { hasCh = value; }
        }
        
        public string ToolTip
        {
            get { return toolTip; }
            set
            {
                if (this.toolTip == value)
                    return;

                this.toolTip = Lib.String.Trim(value);

                NotifyOfPropertyChange(() => ToolTip);
            }
        }

        public IScreen ShowItem { get; set; }

        private Func<IResult> execute;
        public void Execute()
        {
            //return this.execute != null ? this.execute() : new IResult[] { };
            //this.execute != null ? this.execute() : null;
            if (this.execute != null)
                this.execute();
            //else null
        }
    } // MenuItem
} // namespace