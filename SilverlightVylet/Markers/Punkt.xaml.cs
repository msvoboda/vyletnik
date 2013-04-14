using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomMarkers
{
    /// <summary>
    /// Interaction logic for Punkt.xaml
    /// </summary>
    public partial class Punkt : UserControl
    {
        public Punkt()
        {
            InitializeComponent();            
        }

        public Brush BackColor
        {            
            set
            {
                Elipse.Fill = value;
            }
        }
    }
}
