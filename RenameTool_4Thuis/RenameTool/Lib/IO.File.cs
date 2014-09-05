namespace RenameTool.Lib.IO
{
    using System;

    public static class File
    {
        //ToDo: add ALL exceptions to xml; add more checks?
        /// <summary>
        /// Move files in FileInfo list. If new extension is provided, this will also be updated.
        /// </summary>
        /// <param name="fis">List with FileInfo.</param>
        /// <param name="targerDir">The directory where the file should be moved to.</param>
        /// <param name="cancel">Not yet implemented. true move/rename, false will skip all.</param>
        /// <param name="newExt">The new file extension</param>
        /// <returns></returns>
        public static bool MoveRenFileExt(System.IO.FileInfo[] fis, string targerDir, ref bool cancel, string newExt = "")
        {
            if (fis == null)
                throw new ArgumentNullException("File information is null.");

            if (targerDir == null)
                throw new ArgumentNullException("Target directory can not be null.");
            if (targerDir == string.Empty || Char.IsWhiteSpace(targerDir, 0))
                throw new ArgumentException("Target directory is empty, contains only white spaces, or contains invalid characters.");

            int cnt = 0;
            System.IO.FileInfo fi;
            while (cnt < fis.Length && cancel == false)
            {
                // There are still files to rename, and we are not called to cancel(stop).

                // Get one File Info item.
                fi = (System.IO.FileInfo)fis.GetValue(cnt);

                // Check if the directory exists. If not create it.
                if (!System.IO.Directory.Exists(targerDir))
                    System.IO.Directory.CreateDirectory(targerDir);

                // Move files or move and rename files.
                if (string.IsNullOrWhiteSpace(newExt))
                {
                    // Only move the files
                    fi.MoveTo(System.IO.Path.Combine(targerDir,
                        System.IO.Path.GetFileName(fi.Name)));
                }
                else
                {
                    // Move the file to the new location (and name).
                    fi.MoveTo(System.IO.Path.Combine(targerDir,
                        System.IO.Path.GetFileNameWithoutExtension(fi.Name) + newExt)
                    );
                }
                cnt++;
            }
            return true;
        }

        //ToDo: add ALL exceptions to xml; add more checks?
        /// <summary>
        /// Rename files in FileInfo list.
        /// </summary>
        /// <param name="fis">List with FileInfo.</param>
        /// <param name="newExt">The new file extension</param>
        /// <param name="cancel">Not yet implemented. true rename, false will skip all.</param>
        /// <returns>Returns true if all file have been renamed, false otherwise.</returns>
        /// <remarks></remarks>
        public static bool RenFileExt(System.IO.FileInfo[] fis, string newExt, ref bool cancel)
        {
            if (fis == null)
                throw new ArgumentNullException("File information is null.");

            if (newExt == null)
                throw new ArgumentNullException("File extension can not be null.");
            if (newExt == string.Empty || Char.IsWhiteSpace(newExt, 0))
                throw new ArgumentException("File extension is empty, contains only white spaces, or contains invalid characters.");

            int i = 0;
            System.IO.FileInfo fi;

            while (i < fis.Length && cancel == false)
            {
                // There are still files to rename, and we are not called to cancel(stop).

                // Get one File Info item.
                fi = (System.IO.FileInfo)fis.GetValue(i);

                // Move the file to the same location with a divert name.
                fi.MoveTo(System.IO.Path.Combine(fi.DirectoryName,
                        System.IO.Path.GetFileNameWithoutExtension(fi.Name) + newExt)
                );
                i++;
            }
            return true;
        }

    } // File
} //QuickRenameTool.Lib.IO
