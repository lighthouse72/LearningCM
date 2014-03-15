namespace QuickRenameTool.ViewModels
{
    using System.ComponentModel.Composition;
    using Caliburn.Micro;

    [Export(typeof(IScreen))]
    public class MoveViewModel : Screen
    {
        public MoveViewModel()
        {
            this.DisplayName = "Move";
        }

        #region commands/events

        private string _defaultDirectory = string.Empty;
        /// <summary>
        /// Default working location.
        /// </summary>
        /// <remarks>
        /// We only rename one directory at the time.
        /// When renaming subdirectories we use this as base directory that we
        /// can easy go back to.
        /// </remarks>
        public string defaultDirectory
        {
            get { return _defaultDirectory; }
            set
            {
                if (_defaultDirectory == value) return;
                _defaultDirectory = value;
                NotifyOfPropertyChange(() => defaultDirectory);
            }
        }

        private string _extFrom = string.Empty;
        public string extFrom
        {
            get { return _extFrom; }
            set
            {
                if (_extFrom == value) return;
                _extFrom = Lib.String.Trim(value);

                NotifyOfPropertyChange(() => extFrom);
                NotifyOfPropertyChange(() => toMoveFiles);
            }
        }

        private string _extTo = string.Empty;
        public string extTo
        {
            get { return _extTo; }
            set
            {
                if (_extTo == value) return;
                _extTo = Lib.String.Trim(value);

                NotifyOfPropertyChange(() => extTo);
            }
        }

        private bool _move = false;
        /// <summary>
        /// Should we move the file Yes/No.
        /// </summary>
        public bool move
        {
            get { return _move; }
            set
            {
                if (_move == value) return;
                _move = value;

                NotifyOfPropertyChange(() => move);

                targetDirectory = _workingDirectory;
            }
        }

        private string _targetDirectory = string.Empty;
        /// <summary>
        /// Is the directory we us to move the files to.
        /// </summary>
        /// <remarks>
        /// If we do not want to rename the file use the same extensions in
        /// extFrom and extTo.
        /// </remarks>
        public string targetDirectory
        {
            get { return _targetDirectory; }
            set
            {
                if (_targetDirectory == value) return;
                _targetDirectory = value;

                NotifyOfPropertyChange(() => targetDirectory);
            }
        }

        /// <summary>
        /// Reset the working directory to the default directory.
        /// </summary>
        public void restoreDefaultDirectoy()
        {
            workingDirectory = _defaultDirectory;
            NotifyOfPropertyChange(() => Canrun);
            NotifyOfPropertyChange(() => toMoveFiles);
        }

        private bool _run = false;
        public bool Canrun
        {
            /// To be able to move files we need:
            /// - an status that is not cancelled
            /// - a working directory, "C:\".Length is three this is why i
            ///     have set 3 as min length.
            get { return _cancel & _workingDirectory.Length > 3; }
        }
        /// <summary>
        /// Start to make the changes, start executing. 
        /// </summary>
        public void run()
        {
            _run = true;
            _cancel = false;
            NotifyOfPropertyChange(() => Canrun);
            NotifyOfPropertyChange(() => Cancancel);

            renameFiles();

            _run = false;
            _cancel = true;
            NotifyOfPropertyChange(() => Canrun);
            NotifyOfPropertyChange(() => Cancancel);
            NotifyOfPropertyChange(() => toMoveFiles);
        }

        private string _workingDirectory = string.Empty;
        /// <summary>
        /// This is the directory that contains the file we want to rename.
        /// </summary>
        public string workingDirectory
        {
            get { return _workingDirectory; }
            set
            {
                if (_workingDirectory == value) return;
                _workingDirectory = value;

                NotifyOfPropertyChange(() => workingDirectory);
                NotifyOfPropertyChange(() => toMoveFiles);
            }
        }

        private bool _cancel = true;
        public bool Cancancel
        {
            get { return _run; }
        }
        public void cancel()
        {
            _cancel = true;
            _run = false;
            NotifyOfPropertyChange(() => Canrun);
            NotifyOfPropertyChange(() => Cancancel);
            NotifyOfPropertyChange(() => toMoveFiles);
        }

        #endregion commands/events

        /// <summary>
        /// Check and corrects the extension.
        /// </summary>
        /// <param name="ext">Extension to check.</param>
        /// <returns>Returns if extension is change, false otherwise.</returns>
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
            System.IO.FileInfo[] fis = _toMoveFiles;

            // If there are no files we exit here.
            if (fis.Length == 0) return;

            if (_move)
                // Move and rename the file
                Lib.IO.File.MoveRenFileExt(fis, _targetDirectory, ref _cancel, _extTo);
            else
                try
                {
                    // Only rename the files
                    Lib.IO.File.RenFileExt(fis, _extTo, ref _cancel);
                }
                catch (System.Exception){ }

        } // renameFiles

        /// <summary>
        /// Get an list of all the files that will be renamed/moved
        /// </summary>
        private System.IO.FileInfo[] _toMoveFiles;
        public System.IO.FileInfo[] toMoveFiles
        {
            get
            {
                // Check if we have something in the working directory.
                if (string.IsNullOrWhiteSpace(workingDirectory)) return null;

                // Get the Directory Information.
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(workingDirectory);

                // return the File Information list
                _toMoveFiles = di.GetFiles("*" + extFrom);
                return _toMoveFiles;
            }
        } // toMoveFiles

    }
}