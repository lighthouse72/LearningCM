using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickRenameTool.Lib
{
    namespace Windows.Controls
    {
        public class Panel
        {
            /// <summary>
            /// Get the client area size of the passed windows content.
            /// 
            /// </summary>
            /// <param name="element">This can be a Panel or an derive element like Grid</param>
            /// <remarks>
            /// This is just to remember/show how to get size.
            /// You should not use it as is in real-life apps
            /// <example>Lib.Windows.Controls.Panel.GetClientAreaSize((System.Windows.Controls.Grid)this.Content);</example>
            /// </remarks>
            public static void GetClientAreaSize(System.Windows.Controls.Panel element)
            {
                ///// Get windows size (in code behind)
                //System.Windows.MessageBox.Show("Width: " + this.Width.ToString() + System.Environment.NewLine +
                //                "Height: " + this.Height.ToString() + System.Environment.NewLine
                //);

                /// Get client area size
                System.Windows.MessageBox.Show("Client Width: " + element.ActualWidth + System.Environment.NewLine +
                                                "Client Height: " + element.ActualHeight
                );
            } //GetClientAreaSize
        } //Panel

    } //Windows.Controls
} //QuickRenameTool.Lib