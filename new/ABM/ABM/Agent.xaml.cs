using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ABM
{
    /// <summary>
    /// Interaction logic for Agent.xaml
    /// </summary>
    public partial class Agent : UserControl
    {
        private int u;
        private bool p;
        public List<Product> Products;
        public int ID;

        public Agent()
        {
            InitializeComponent();

            //draw circle
            StackPanel myStackPanel = new StackPanel();

            // Create a red Ellipse.
            Ellipse myEllipse = new Ellipse();

            // Create a SolidColorBrush with a red color to fill the 
            // Ellipse with.
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();

            // Describes the brush's color using RGB values. 
            // Each value has a range of 0-255.
            mySolidColorBrush.Color = Color.FromArgb(255, 255, 255, 0);
            myEllipse.Fill = mySolidColorBrush;
            myEllipse.StrokeThickness = 1;
            myEllipse.Stroke = Brushes.Black;

            // Set the width and height of the Ellipse.
            myEllipse.Width = 25;
            myEllipse.Height = 25;

            // Add the Ellipse to the StackPanel.
            myStackPanel.Children.Add(myEllipse);

            this.Content = myStackPanel;
            
        }

        public Agent(int u, bool p)
        {
            InitializeComponent();

            //draw circle
            StackPanel myStackPanel = new StackPanel();

            // Create a red Ellipse.
            Ellipse myEllipse = new Ellipse();

            // Create a SolidColorBrush with a red color to fill the 
            // Ellipse with.
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();

            // Describes the brush's color using RGB values. 
            // Each value has a range of 0-255.
            mySolidColorBrush.Color = Color.FromArgb(255, 255, 255, 0);
            myEllipse.Fill = mySolidColorBrush;
            myEllipse.StrokeThickness = 1;
            myEllipse.Stroke = Brushes.Black;

            // Set the width and height of the Ellipse.
            myEllipse.Width = 35;
            myEllipse.Height = 35;

            // Add the Ellipse to the StackPanel.
            myStackPanel.Children.Add(myEllipse);

            this.Content = myStackPanel;

            // TODO: Complete member initialization
            this.u = u;
            this.p = p;
        }

        internal void UpdateValue()
        {
            throw new NotImplementedException();
        }
    }
}
