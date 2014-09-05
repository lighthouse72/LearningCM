namespace RenameTool.ViewModels
{
    using System.ComponentModel.Composition;
    using Caliburn.Micro;
    using RenameTool.Infrastructure;
    using System.Collections.Generic;

    [Export(typeof(IScreen))]
    [Export(typeof(MoveViewModel))]
    public class MoveViewModel : Screen
    {
        private Modules.Tabs.ITabsManager tabManager;

        public MoveViewModel()
        {
            this.DisplayName = "Move";
        }

        [ImportingConstructor()]
        public MoveViewModel(Modules.Menu.IMenuManager menuManager, Modules.Tabs.ITabsManager tabManager)
            : this()
        {
            this.tabManager = tabManager;
            Modules.Menu.MenuItem mi = new Modules.Menu.MenuItem(this.DisplayName, ShowItem);
            mi.ShowItem = this;
            menuManager.AddWithParent("File", mi);
        }

        /// <summary>
        /// Check and corrects the extension.
        /// </summary>
        /// <param name="ext">Extension to check.</param>
        /// <returns>Returns true if extension is change, false otherwise.</returns>
        public static bool checkExtension(ref string ext)
        {
            if (string.IsNullOrEmpty(ext)) return false;
            bool hasChanged = false;

            if (ext.Substring(0, 1) != ".")
            {
                ext = "." + ext;
                hasChanged = true;
            }

            if (ext.Substring(ext.Length - 1, 1) == ".")
            {
                ext = ext.Substring(0, ext.Length - 1);
                hasChanged = true;
            }
            return hasChanged;
        }

        private void renameFiles()
        {
            System.IO.FileInfo[] fis = this.toMoveFiles;

            // If there are no files we exit here.
            if (fis.Length == 0) return;

            if (checkExtension(ref extTo))
                NotifyOfPropertyChange(() => this.ExtTo);

            if (this.move)
                // Move and rename the file
                Lib.IO.File.MoveRenFileExt(fis, this.targetDirectory, ref this.cancel, this.extTo);
            else
                try
                {
                    // Only rename the files
                    Lib.IO.File.RenFileExt(fis, this.extTo, ref this.cancel);
                }
                catch (System.Exception){ }

        }

        public IResult ShowItem()
        {
            this.tabManager.ActivateItem(this);
            return null;
        }

        /// <summary>
        /// Get an list of all the files that will be renamed/moved
        /// </summary>
        private System.IO.FileInfo[] toMoveFiles;
        public System.IO.FileInfo[] ToMoveFiles
        {
            get
            {
                // Check if we have something in the working directory.
                if (string.IsNullOrWhiteSpace(this.WorkingDirectory)) return null;

                // Get the Directory Information.
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(this.WorkingDirectory);

                // Check and/or update the extension.
                if (checkExtension(ref this.extFrom))
                    NotifyOfPropertyChange(() => this.ExtFrom);

                // return the File Information list
                this.toMoveFiles = di.GetFiles("*" + this.ExtFrom);
                return this.toMoveFiles;
            }
        }

        #region commands/events

        private string defaultDirectory = string.Empty;
        /// <summary>
        /// Default working location.
        /// </summary>
        /// <remarks>
        /// We only rename one directory at the time.
        /// When renaming subdirectories we use this as base directory that we
        /// can easy go back to.
        /// </remarks>
        public string DefaultDirectory
        {
            get { return this.defaultDirectory; }
            set
            {
                if (this.defaultDirectory == value) return;
                this.defaultDirectory = value;
                NotifyOfPropertyChange(() => this.DefaultDirectory);
            }
        }

        private string extFrom = string.Empty;
        public string ExtFrom
        {
            get { return this.extFrom; }
            set
            {
                if (this.extFrom == value) return;
                this.extFrom = Lib.String.Trim(value);

                NotifyOfPropertyChange(() => this.ExtFrom);
                NotifyOfPropertyChange(() => this.ToMoveFiles);
            }
        }

        private string extTo = string.Empty;
        public string ExtTo
        {
            get { return this.extTo; }
            set
            {
                if (this.extTo == value) return;
                this.extTo = Lib.String.Trim(value);

                NotifyOfPropertyChange(() => this.ExtTo);
            }
        }

        private bool move = false;
        /// <summary>
        /// Should we move the file Yes/No.
        /// </summary>
        public bool Move
        {
            get { return this.move; }
            set
            {
                if (this.move == value) return;
                this.move = value;

                NotifyOfPropertyChange(() => this.Move);

                this.TargetDirectory = this.workingDirectory;
            }
        }

        private string targetDirectory = string.Empty;
        /// <summary>
        /// Is the directory we us to move the files to.
        /// </summary>
        /// <remarks>
        /// If we do not want to rename the file use the same extensions in
        /// extFrom and extTo.
        /// </remarks>
        public string TargetDirectory
        {
            get { return this.targetDirectory; }
            set
            {
                if (this.targetDirectory == value) return;
                this.targetDirectory = value;

                NotifyOfPropertyChange(() => this.TargetDirectory);
            }
        }

        /// <summary>
        /// Reset the working directory to the default directory.
        /// </summary>
        public void RestoreDefaultDirectoy()
        {
            this.WorkingDirectory = this.defaultDirectory;
            NotifyOfPropertyChange(() => this.CanRun);
            NotifyOfPropertyChange(() => this.ToMoveFiles);
        }

        private bool run = false;
        public bool CanRun
        {
            /// To be able to move files we need:
            /// - an status that is not cancelled
            /// - a working directory, "C:\".Length is three this is why i
            ///     have set 3 as min length.
            get { return this.cancel & this.workingDirectory.Length > 3; }
        }
        /// <summary>
        /// Start to make the changes, start executing. 
        /// </summary>
        public void Run()
        {
            this.run = true;
            this.cancel = false;
            NotifyOfPropertyChange(() => this.CanRun);
            NotifyOfPropertyChange(() => this.CanCancel);

            this.renameFiles();

            this.run = false;
            this.cancel = true;
            NotifyOfPropertyChange(() => this.CanRun);
            NotifyOfPropertyChange(() => this.CanCancel);
            NotifyOfPropertyChange(() => this.ToMoveFiles);
        }

        private string workingDirectory = string.Empty;
        /// <summary>
        /// This is the directory that contains the file we want to rename.
        /// </summary>
        public string WorkingDirectory
        {
            get { return this.workingDirectory; }
            set
            {
                if (this.workingDirectory == value) return;
                this.workingDirectory = value;

                NotifyOfPropertyChange(() => this.WorkingDirectory);
                NotifyOfPropertyChange(() => this.ToMoveFiles);
            }
        }

        private bool cancel = true;
        public bool CanCancel
        {
            get { return this.run; }
        }
        public void Cancel()
        {
            this.cancel = true;
            this.run = false;
            NotifyOfPropertyChange(() => this.CanRun);
            NotifyOfPropertyChange(() => this.CanCancel);
            NotifyOfPropertyChange(() => this.ToMoveFiles);
        }

        #endregion commands/events
    }
}