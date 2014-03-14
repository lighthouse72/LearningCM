/// Note: NuGet:
///             - Caliburn.Micro version: 1.5.2
///             - PropertyTools.Wpf version: 2014.1.13.1

namespace QuickRenameTool.Shell
{
    using Caliburn.Micro;
    using System.ComponentModel.Composition;
    using System.Windows;

    [Export(typeof(IShell))]
    public class ShellViewModel : PropertyChangedBase, IShell
    {
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
                _extFrom = Trim(value);

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
                _extTo = Trim(value);

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
        /// Is is the directory we us to move the files to.
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
            /// - an status that is not canceled
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
            get{return _workingDirectory;}
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
            get { return _run;}
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

        public static void checkExtension(ref string ext)
        {
            if (string.IsNullOrEmpty(ext)) return;

            if (ext.Substring(0, 1) != ".")
                ext = "." + ext;

            if (ext.Substring(ext.Length - 1, 1) == ".")
                ext = ext.Substring(0, ext.Length - 1);
        }

        private void renameFiles()
        {
            // Check if the directory exists
            if (System.IO.Directory.Exists(workingDirectory))
            {
                // Get the Directory Information
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(workingDirectory);

                // Check/correct the extension
                checkExtension(ref _extTo);
                NotifyOfPropertyChange(() => extFrom);

                // Get the File Info list
                System.IO.FileInfo[] fis = di.GetFiles("*" + extFrom);

                // If there are no files we exit here.
                if (fis.Length == 0) return;

                // Check/correct the extension
                checkExtension(ref _extTo);
                NotifyOfPropertyChange(() => extTo);

                int i = 0;
                System.IO.FileInfo fi;
                while (i < fis.Length && _cancel == false)
                {
                    // There are still files to rename, and we are not called to cancel(stop).

                    // Get one File Info item.
                    fi = (System.IO.FileInfo)fis.GetValue(i);
                    if (_move)
                    {
                        // We need to move the file

                        // Check if the directory exists. If not create it.
                        if (!System.IO.Directory.Exists(_targetDirectory))
                            System.IO.Directory.CreateDirectory(_targetDirectory);

                        // Move the file to the new location (and name).
                        fi.MoveTo(System.IO.Path.Combine(_targetDirectory,
                                System.IO.Path.GetFileNameWithoutExtension(fi.Name) + _extTo)
                            );
                    }
                    else
                    {
                        // Only rename, do not move.

                        // Move the file to the same location with a divert name.
                        fi.MoveTo(System.IO.Path.Combine(_workingDirectory,
                                System.IO.Path.GetFileNameWithoutExtension(fi.Name) + _extTo)
                            );
                    }
                    i++;
                }
            }
        } // renameFiles

        /// <summary>
        /// Get an list of all the files that will be renamed/moved
        /// </summary>
        public System.IO.FileInfo[] toMoveFiles
        {
            get
            {
                // Check if we have something in the working directory.
                if (string.IsNullOrWhiteSpace(workingDirectory)) return null;

                // Get the Directory Information.
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(workingDirectory);
                // Check/correct the extension
                checkExtension(ref _extTo);
                NotifyOfPropertyChange(() => extFrom);
                // return the File Information list
                return di.GetFiles("*" + extFrom);
            }
        } // toMoveFiles

        public static string Trim(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return string.Empty;

            return str.Trim();
        }

    } // ShellViewModel
} // namespace QuickRenameTool.Shell