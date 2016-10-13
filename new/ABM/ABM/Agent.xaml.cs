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
        public List<Product> Products { get; set; }

        public int ID;
        private int TotalValue;

        public string Name;
        private List<string> AgentNames;

        public Agent()
        {
            InitializeComponent();
        }

        public Agent(int u, bool p, Random ra)
        {
            ID = u; //??

            InitializeComponent();

            Products = new List<Product>();

            StackPanel myStackPanel = new StackPanel();
            Ellipse myEllipse = new Ellipse();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();

            mySolidColorBrush.Color = Color.FromArgb(255, 255, 255, 0);
            myEllipse.Fill = mySolidColorBrush;
            myEllipse.StrokeThickness = 1;
            myEllipse.Stroke = Brushes.Black;

            // Set the width and height of the Ellipse.
            myEllipse.Width = 15;
            myEllipse.Height = 15;

            // Add the Ellipse to the StackPanel.
            myStackPanel.Children.Add(myEllipse);
            PopulateNames();


            Label l = new Label();
            int pp = ra.Next(AgentNames.Count() - 1);            
            Name = AgentNames[pp];
            l.Content = Name;
         
            if (p)
            {
                Product product = new Product(MainWindow.SampleLicense, MainWindow.ProductIDer);

                product.OriginAgent = ID;
                product.NameOfOriginAgent = Name;
                product.OriginProductID = MainWindow.ProductIDer;

                product.Contributors.Add(ID);

                Products.Add(product);
                MainWindow.ProductIDer++;
            }


            myStackPanel.Children.Add(l);            

            Label lp = new Label();
            lp.Name = "ProductLabel";
            lp.Content = "P: " + Products.Count();
            
            myStackPanel.Children.Add(lp);

            this.Content = myStackPanel;

            // TODO: Complete member initialization
            this.u = u;
            this.p = p;
        }

        internal void UpdateValue()
        {
            TotalValue = 0;
            for (int u = 0; u < Products.Count(); u++)
            {
                TotalValue += Products[u].Value;
            }
        }

        private void PopulateNames() //slecht, elke agent heeft nu deze lijst
        {
            AgentNames = new List<String>();

            AgentNames.Add("Aaron");
            AgentNames.Add("Benny");
            AgentNames.Add("Claude");
            AgentNames.Add("Dwayne");
            AgentNames.Add("Ezekiel");
            AgentNames.Add("Fred");
            AgentNames.Add("Gunther");
            AgentNames.Add("Heather");
            AgentNames.Add("Ivan");
            AgentNames.Add("James");
            AgentNames.Add("Karl");
            AgentNames.Add("Leeroy");
            AgentNames.Add("Martin");
            AgentNames.Add("Nolan");
            AgentNames.Add("Olga");
            AgentNames.Add("Peter");
            AgentNames.Add("Quintus");
            AgentNames.Add("Reed");
            AgentNames.Add("Steve");
            AgentNames.Add("Tyrone");
            AgentNames.Add("Uma");
            AgentNames.Add("Victor");
            AgentNames.Add("Wesley");
            AgentNames.Add("Xavier");
            AgentNames.Add("Yvette");
            AgentNames.Add("Zeke");

            AgentNames.Add("Anton");
            AgentNames.Add("Bradley");
            AgentNames.Add("Christian");
            AgentNames.Add("Devon");
            AgentNames.Add("Eloy");
            AgentNames.Add("Frank");
            AgentNames.Add("George");
            AgentNames.Add("Hetfield");
            AgentNames.Add("Irene");
            AgentNames.Add("Johnny");
            AgentNames.Add("Keith");
            AgentNames.Add("LeBron");
            AgentNames.Add("Michael");
            AgentNames.Add("Ned");
            AgentNames.Add("Octavo");
            AgentNames.Add("Pavel");
            AgentNames.Add("Quinn");
            AgentNames.Add("Ravel");
            AgentNames.Add("Stephen");
            AgentNames.Add("Tola");
            AgentNames.Add("Ulf");
            AgentNames.Add("Val");
            AgentNames.Add("Walter");
            AgentNames.Add("Xander");
            AgentNames.Add("Yale");
            AgentNames.Add("Zack");

            AgentNames.Add("PANIC");
        }
    }
}
