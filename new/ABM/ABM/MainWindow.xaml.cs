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
using OxyPlot.Wpf;
using OxyPlot;
using OxyPlot.Series;

namespace ABM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Agent> Agents;

        int Iterations = 50; //well, iterations. 
        int CurrentIteration = 0;
        int IterationTime = 5; //frames
        int AgentCount = 30; //laten we simpel beginnen (ofzo)
        int AgentProductInit = 4; //number of agents that start with a product.
        int PublicSpaceComplexity; //variety in available data
        int GroupStrength; //strength of relations
        int CurrentAgent = 0;
        public static int ProductIDer = 0;
        public static int SampleLicense = 0;

        bool PrintInfo = true;
        bool ShowIDs = true;

        public string Title { get; private set; }
        public IList<DataPoint> Points { get; private set; }
        List<Product> FinishedProducts = new List<Product>();

        List<List<ScatterPoint>> DivergenceConvergencePoints = new List<List<ScatterPoint>>();

        OxyPlot.Wpf.ScatterSeries ActiveConvergenceSeries = new OxyPlot.Wpf.ScatterSeries();
        List<Color> GeneratedColors = new List<Color>();

        Color ActiveColor;

        public MainWindow()
        {
            InitializeComponent();

            Licenses = new List<string>();

            Licenses.Add("Attribution");
            Licenses.Add("Attribution-ShareAlike");
            Licenses.Add("Attribution-NoDerivs");
            Licenses.Add("Attribution-NonCommercial");
            Licenses.Add("Attribution-NonCommercial-ShareAlike");
            Licenses.Add("Attribution-NonCommercial-NoDerivs");
            
            DivergenceConvergencePlot.Title = "Convergence / divergence"; //veel producten / low value vs little products / high value                       
       
            num_colors = 350;
            GenerateColors();
            RepopAgents();

            CompileConvergentSeries();

            DivergenceConvergencePlot.ActualModel.MouseDown += new EventHandler<OxyPlot.OxyMouseDownEventArgs>(plotModel_MouseDown);
            DivergenceConvergencePlot.ActualModel.MouseUp += new EventHandler<OxyMouseEventArgs>(plotModel_MouseUp);
        }

        // event handler
        void plotModel_MouseDown(object sender, OxyPlot.OxyMouseDownEventArgs e)       
        {            
            if (DivergenceConvergencePlot.Series.Any(p => p.GetType() == typeof(OxyPlot.Wpf.LineSeries)))
            {
                //  int l = DivergenceConvergencePlot.Series.Count;
                var list = DivergenceConvergencePlot.Series.Where(p => p.GetType() != typeof(OxyPlot.Wpf.LineSeries)).ToList();

                DivergenceConvergencePlot.Series.Clear();
                list.ForEach(p => DivergenceConvergencePlot.Series.Add(p));

                DivergenceConvergencePlot.InvalidatePlot();
            }

            try
            {
                ScatterPoint res = (ScatterPoint)e.HitTestResult.Item;

                if (res != null)
                {
                    string ID = res.Tag.ToString();
                    
                    OxyPlot.Wpf.LineSeries tempseries = new OxyPlot.Wpf.LineSeries();
                    List<DataPoint> points = new List<DataPoint>();

                    //find all series with the ID
                    foreach (OxyPlot.Wpf.ScatterSeries series in DivergenceConvergencePlot.Series)
                    {
                        if (series.Tag.ToString() == ID)
                        {
                            ScatterPoint point = (ScatterPoint)series.Items[0];

                            points.Add(new DataPoint(point.X, point.Y));
                            //contains single point
                        }
                    }

                    tempseries.ItemsSource = points;                    
                    DivergenceConvergencePlot.Series.Add(tempseries);

                    DivergenceConvergencePlot.InvalidatePlot();
                    
                }
            }
            catch (Exception ex)
            { }
        }

        void plotModel_MouseUp(object sender, OxyPlot.OxyMouseEventArgs e)
        {
            if (DivergenceConvergencePlot.Series.Any(p => p.GetType() == typeof(OxyPlot.Wpf.LineSeries)))
            {
                //  int l = DivergenceConvergencePlot.Series.Count;
                var list = DivergenceConvergencePlot.Series.Where(p => p.GetType() != typeof(OxyPlot.Wpf.LineSeries)).ToList();

                DivergenceConvergencePlot.Series.Clear();
                list.ForEach(p => DivergenceConvergencePlot.Series.Add(p));

                DivergenceConvergencePlot.InvalidatePlot();
            }
        }

        public void RepopAgents()
        {
            //reset id counter
            int ProductIDer = 0;

            //grap some numbers for product assignment
            int[] ProductAgents = new int[AgentProductInit];

            for (int j = 0; j < AgentProductInit; j++)
            {
                ProductAgents[j] = -1;
            }

            MainCanvas.Children.Clear();
            Agents = new List<Agent>();

            for (int y = 0; y < ProductAgents.Count(); y++)
            {
                bool unique = false;
                int r = 0;

                while (!unique)
                {
                    Random rand = new Random();
                    r = rand.Next(AgentCount);
                    bool present = false;

                    for (int x = 0; x < ProductAgents.Count(); x++)
                    {
                        if (ProductAgents[x] == r)
                        {
                            //zit al in de lijst, breek.
                            present = true;
                            break;
                        }
                    }

                    if (!present)
                        unique = true;
                }

                ProductAgents[y] = r;
            }

            float AngleFrag = (float)Math.PI * 2 / AgentCount;
            Random ra = new Random();
            for (int i = 0; i < AgentCount; i++)
            {
                float a = AngleFrag * i;

                double x = (550 / 2) + (250 * Math.Cos(a));
                double y = (550 / 2) + (250 * Math.Sin(a));

                //fill(255);

                //AGENT HIER TEKENENENENENENENE
                //ellipse(x, y, 50 - (AgentCount / 2), 50 - (AgentCount / 2));

                Agent age = new Agent(i, false, ra);

                //assign products if number equals a different number
                for (int n = 0; n < ProductAgents.Count(); n++)
                {
                    if (i == ProductAgents[n])
                    {
                        age = new Agent(i, true, ra);
                        break;
                    }
                }

                MainCanvas.Children.Add(age);

                Canvas.SetLeft(age, x);
                Canvas.SetTop(age, y);

                Agents.Add(age);

                if (ShowIDs)
                {
                    //fill(0);
                    //if (i == CurrentAgent)
                    //    textSize(20);
                    //else
                    //    textSize(12);

                    //text(i, x - 10, y + 5);
                    //text("P: " + Agents.get(i).Products.Count(), x - 10, y - 30);
                }
            }
        }

        List<DataPoint> SolvedSeriesItems = new List<DataPoint>();
        List<DataPoint> ContribItems = new List<DataPoint>();
        public void CompileAverageContributorsSeries()
        { 
            int contribucount = 0;
            Agents.ForEach(p => p.Products.ForEach(a => contribucount += a.Contributors.Count));

            int pcount = 0;
            Agents.ForEach(p => pcount += p.Products.Count);

            contribucount /= pcount;

            ContribItems.Add(new DataPoint(CurrentIteration, contribucount));

            var t = new OxyPlot.Wpf.LineSeries();
            t.ItemsSource = ContribItems;

            AverageContrib.Series.Clear();
            AverageContrib.Series.Add(t);

            AverageContrib.InvalidatePlot();
        }

        public void CompileSolvedProductSeries()
        { 
            SolvedSeriesItems.Add(new DataPoint(CurrentIteration, FinishedProducts.Count));
            var t = new OxyPlot.Wpf.LineSeries();            
            t.ItemsSource = SolvedSeriesItems;

            SolvedProductsPlot.Series.Clear();
            SolvedProductsPlot.Series.Add(t);

            SolvedProductsPlot.InvalidatePlot();
        }

        public void CompileValueVsCountSeries()
        {
            int count = 0;
            int value = 0;
            foreach (Agent a in Agents)
            {
                foreach (Product p in a.Products)
                {
                    value += p.Value;
                    count++;
                }
            }
           
            ValueSeries.ItemsSource = new List<BarItem>(new[]
            {           
                new BarItem{ Value = value }
            });

            CountSeries.ItemsSource = new List<BarItem>(new[]
            {           
                new BarItem{ Value = count }
            });       
        }

        public void CompileConvergentSeries()
        { 
            //alle producten
            /*
             * y is waarde product
             * x = aantal iteraties
             */

            foreach (Agent a in Agents)
	        {
                foreach (Product p in a.Products)
                {
                    ScatterPoint sca = new ScatterPoint(CurrentIteration, p.Value, 3.0, 10.0, p.ID);
            
                    var aa = new List<ScatterPoint>();                    
                    aa.Add(sca);

                    var series = new OxyPlot.Wpf.ScatterSeries { 
                        TrackerFormatString = "X: " + CurrentIteration + Environment.NewLine + 
                        "Y: " + p.Value + Environment.NewLine + 
                        "PID: " + p.ID + Environment.NewLine + 
                        "Agent: " + a.Name + Environment.NewLine +
                        "Origin: " + p.NameOfOriginAgent + Environment.NewLine + 
                        "Color: " + GeneratedColors[p.ID].ToString(),
                        Color = GeneratedColors[p.ID], //de kleuren zijn steeds anders...
                        MarkerFill = GeneratedColors[p.ID],
                    };  

                    DivergenceConvergencePlot.Series.Add(series);
                    series.ItemsSource = aa;
                    series.Tag = p.ID;
                }
	        }       
     

        }

        public void GenerateColors()
        {
            Random r = new Random();
            for (int i = 1; i < 360; i += 360 / num_colors)
            {                
                int hue = i;
                double saturation = 90 + r.NextDouble() * 10;
                double lightness = 50 + r.NextDouble() * 10;

                //addColor(c);
                GeneratedColors.Add(ColorFromHSL((double)hue, saturation, lightness));
            }
            
            for (int i = 1; i < 360; i += 360 / num_colors)
            {
                int hue = i;
                double saturation = 90 + r.NextDouble() * 10;
                double lightness = 50 + r.NextDouble() * 10;

                //addColor(c);
                GeneratedColors.Add(ColorFromHSL((double)hue, saturation, lightness));
            }

            for (int i = 1; i < 360; i += 360 / num_colors)
            {
                int hue = i;
                double saturation = 90 + r.NextDouble() * 10;
                double lightness = 50 + r.NextDouble() * 10;

                //addColor(c);
                GeneratedColors.Add(ColorFromHSL((double)hue, saturation, lightness));
            }

            for (int i = 1; i < 360; i += 360 / num_colors)
            {
                int hue = i;
                double saturation = 90 + r.NextDouble() * 10;
                double lightness = 50 + r.NextDouble() * 10;

                //addColor(c);
                GeneratedColors.Add(ColorFromHSL((double)hue, saturation, lightness));
            }        
        }

        public Color ColorFromHSL(double Hue, double Saturation, double Luminosity)
        {
            byte r, g, b;
            if (Saturation == 0)
            {
                r = (byte)Math.Round(Luminosity * 255d);
                g = (byte)Math.Round(Luminosity * 255d);
                b = (byte)Math.Round(Luminosity * 255d);
            }
            else
            {
                double t1, t2;
                double th = Hue / 6.0d;

                if (Luminosity < 0.5d)
                {
                    t2 = Luminosity * (1d + Saturation);
                }
                else
                {
                    t2 = (Luminosity + Saturation) - (Luminosity * Saturation);
                }
                t1 = 2d * Luminosity - t2;

                double tr, tg, tb;
                tr = th + (1.0d / 3.0d);
                tg = th;
                tb = th - (1.0d / 3.0d);

                tr = ColorCalc(tr, t1, t2);
                tg = ColorCalc(tg, t1, t2);
                tb = ColorCalc(tb, t1, t2);
                r = (byte)Math.Round(tr * 255d);
                g = (byte)Math.Round(tg * 255d);
                b = (byte)Math.Round(tb * 255d);
            }

            return Color.FromRgb(r, g, b);
        }

        private static double ColorCalc(double c, double t1, double t2)
        {
            if (c < 0) c += 1d;
            if (c > 1) c -= 1d;
            if (6.0d * c < 1.0d) return t1 + (t2 - t1) * 6.0d * c;
            if (2.0d * c < 1.0d) return t2;
            if (3.0d * c < 2.0d) return t1 + (t2 - t1) * (2.0d / 3.0d - c) * 6.0d;
            return t1;
        }

        private double GetColorComponent(double temp1, double temp2, double temp3)
        {
            if (temp3 < 0.0)
                temp3 += 1.0;
            else if (temp3 > 1.0)
                temp3 -= 1.0;

            if (temp3 < 1.0 / 6.0)
                return temp1 + (temp2 - temp1) * 6.0 * temp3;
            else if (temp3 < 0.5)
                return temp2;
            else if (temp3 < 2.0 / 3.0)
                return temp1 + ((temp2 - temp1) * ((2.0 / 3.0) - temp3) * 6.0);
            else
                return temp1;
        }

        Product evaluateSourceLicense(Agent B, Agent A)
        {
            Random rand = new Random();

            List<Product> Prods = B.Products;

            //grab a rand.Next for now
            Product source = Prods[rand.Next(0, Prods.Count())];

            int License = source.License;
            bool present = false;
            Product np = null;
            
            switch (License)
            {
                case 0: //Attribution
                    int ne = rand.Next(0, 5);
                    np = new Product(ne, ProductIDer); //use whatever license
                    np.Value = source.Value + rand.Next(50, 150);

                    np.OriginAgent = source.OriginAgent;
                    np.NameOfOriginAgent = source.NameOfOriginAgent;

                    ProductIDer++;
                    break;
                case 1: //Attribution-ShareAlike
                    np = new Product(source.License, ProductIDer);
                    np.Value = source.Value + rand.Next(50, 150);

                    np.OriginAgent = source.OriginAgent;
                    np.NameOfOriginAgent = source.NameOfOriginAgent;

                    ProductIDer++;
                    break;
                case 2: //Attribution-NoDerivs
                    //check if this product is already in list of new agent
                    present = false;
                    for (int i = 0; i < A.Products.Count(); i++)
                    {
                        if (A.Products[i].ID == source.ID)
                            present = true;
                    }

                    if (!present)
                    {
                        np = new Product(source.License, source.ID);
                        np.Value = source.Value;
                        ProductIDer++;
                    }
                    else
                    {
                        for (int p = 0; p < B.Products.Count(); p++)
                        {
                            int Bid = B.Products[p].ID;
                            //bestaat deze al in de target?
                            bool intarget = false;
                            for (int o = 0; o < A.Products.Count(); o++)
                            {
                                if (A.Products[o].ID == Bid)
                                {
                                    intarget = true;
                                    break;
                                }
                            }

                            if (!intarget)
                            {
                                np = new Product(B.Products[p].License, B.Products[p].ID);
                                np.Value = B.Products[p].Value;
                                ProductIDer++;

                                break;
                            }
                        }
                    }

                    if (np != null)
                    {
                        np.OriginAgent = source.OriginAgent;
                        np.NameOfOriginAgent = source.NameOfOriginAgent;
                    }
                    break;
                case 3: //Attribution-NonCommercial   
                    np = new Product(rand.Next(0, 5), ProductIDer); //use whatever license
                    np.Value = source.Value + rand.Next(50, 100); //iets minder value, 
                    ProductIDer++;

                    np.OriginAgent = source.OriginAgent;
                    np.NameOfOriginAgent = source.NameOfOriginAgent;
                    break;
                case 4: //Attribution-NonCommercial-ShareAlike
                    np = new Product(source.License, ProductIDer);
                    np.Value = source.Value + rand.Next(50, 100); //iets minder value,
                    ProductIDer++;

                    np.OriginAgent = source.OriginAgent;
                    np.NameOfOriginAgent = source.NameOfOriginAgent;
                    break;
                case 5: //Attribution-NonCommercial-NoDerivs
                    //check if this product is already in list of new agent
                    present = false;
                    for (int i = 0; i < A.Products.Count(); i++)
                    {
                        if (A.Products[i].ID == source.ID)
                            present = true;
                    }

                    if (!present)
                    {
                        np = new Product(source.License, source.ID);
                        np.Value = source.Value;
                        ProductIDer++;
                    }
                    else
                    {
                        for (int p = 0; p < B.Products.Count(); p++)
                        {
                            int Bid = B.Products[p].ID;
                            //bestaat deze al in de target?
                            bool intarget = false;
                            for (int o = 0; o < A.Products.Count(); o++)
                            {
                                if (A.Products[o].ID == Bid)
                                {
                                    intarget = true;
                                    break;
                                }
                            }

                            if (!intarget)
                            {
                                np = new Product(B.Products[p].License, B.Products[p].ID);
                                np.Value = B.Products[p].Value;
                                ProductIDer++;

                                break;
                            }
                        }
                    }
                    break;
            }

            if (np != null)
            {
                np.OriginProductID = source.ID;

                np.Contributors = new List<int>();
                np.Contributors = source.Contributors;
            }

            return np;
        }

        Agent returnRandomAgentWithProduct(int self) //prevent returning self
        {
            List<int> indices = new List<int>();
            for (int u = 0; u < Agents.Count(); u++)
            {
                if (Agents[u].Products.Count() != 0 && Agents[u].ID != self) 
                {

                    indices.Add(u);
                }
            }

            indices = Shuffle(indices);


            return Agents[indices[0]];
        }

        private static Random rng = new Random();
        private bool SimulateNow;
        private List<string> Licenses;
        private int num_colors;

        public List<int> Shuffle(List<int> list)
        {
            int n = list.Count();
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                int value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }

        void CheckForFinished()
        {
            for (int y = 0; y < Agents.Count(); y++)
            {
                for (int x = 0; x < Agents[y].Products.Count(); x++)
                {
                    if (Agents[y].Products[x].Value > 850)
                    {
                        //throw in finished works
                        FinishedProducts.Add(Agents[y].Products[x]);
                        lblFinishedProducts.Content = "Solved: " + FinishedProducts.Count;
                        Agents[y].Products.RemoveAt(x);
                        
                        var label = from r in ((StackPanel)Agents[y].Content).Children.OfType<Label>() where r.Name == "ProductLabel" select r; //lol lelijke code ahoy
                        label.Take(1).ToList()[0].Content = "P: " + Agents[y].Products.Count;
                    }
                }
            }
        }

        void Simulate()
        {
            CheckForFinished();
            
            bool NoProductsDispensed = true;

            for (int i = 0; i < AgentCount; i++)
            {
                //make every agent take stuff from other agents
                Agent A = Agents[i];

                //paar mogelijkheden. 
                /*
                1. agent pakt een nieuw product
                 2. agent werkt verder aan eigen product.
                 */

                //30-70 in het voordeel van doorwerken
                //is het sharealike? score hoger dan iets, kans van 25 dat de noderivs eraf gaat    
                Random rand = new Random();

                bool WorkOnOwnProduct = rand.Next(0, 100) > 70 ? true : false;
                bool Default = !WorkOnOwnProduct;

                if (WorkOnOwnProduct)
                {
                    //2. divergence (ratio van authors en het aantal producten)
                    //2.1 op basis daarvan kijken of bepaalde agents het beter doen dan anderen (puur op basis van licentie) 
                    //3. kans op ontwikkeling eigen product

                    //
                    Product P = GetAgentOwnProduct(A); //instantie maar moet link zijn
                    if (P != null)
                    {
                        P.Value += rand.Next(75, 150); //random score...
                        System.Console.WriteLine("Agent " + A.Name + " worked on own product (ID: " + P.ID + " OID: " + P.OriginProductID + ") , new value: " + P.Value);

                        if (P.Value > 750)
                        {
                            //waarom gebruiken mensen noderivs? Aan de ene kant om de ontwikkeling te beschermen, aan de andere kant om het resultaat te beschermen?
                            if (P.License == 2 || P.License == 5)
                            {
                                //dit is dus in het geval dat de voortgang word beschermd, nu is die voortgang bij een punt dat het 'vrijgegeven' kan worden              
                                bool ChangeLicense = rand.Next(0, 100) < 15 ? false : true;
                                if (ChangeLicense)
                                {
                                    //nu is het noderivs. pak een andere 
                                    List<int> li = new List<int>();

                                    li.Add(0);
                                    li.Add(1);
                                    li.Add(3);
                                    li.Add(4);

                                    li = Shuffle(li);

                                    P.License = li[0];

                                    System.Console.WriteLine("... and changed license to: " + Licenses[P.License]);

                                    //nu alle andere producten vinden die afkomstig van deze agent en ook de license veranderen
                                    foreach (Agent B in Agents)
                                    {
                                        foreach (Product O in B.Products)
                                        {
                                            if (O.OriginProductID == P.OriginProductID)
                                            {
                                                O.License = P.License;
                                                System.Console.WriteLine("... and " + B.Name + " thus changed product license of " + O.OriginProductID + " to " + Licenses[O.License]);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                        Default = true;
                }

                if (Default) //default action is new product
                {
                    //find product
                    Agent B = returnRandomAgentWithProduct(A.ID); //param is prevent self.

                    //evaluate license
                    Product P = evaluateSourceLicense(B, A);
                    if (P != null)
                    {
                        if (!ContainsProduct(P, A))
                        {
                            P.Contributors.Add(A.ID);//add agent to contributors..

                            A.Products.Add(P);
                            var label = from r in ((StackPanel)A.Content).Children.OfType<Label>() where r.Name == "ProductLabel" select r; //lol
                            label.Take(1).ToList()[0].Content = "P: " + A.Products.Count;

                            NoProductsDispensed = false;
                        }
                    }

                    A.UpdateValue();
                }
            }

            CurrentIteration++;

            if (NoProductsDispensed)
                System.Console.WriteLine("No products were dispensed, end of test run / 100% convergence?");

            //if (CurrentIteration == Iterations)
            //{
             //   SimulateNow = false;
              //  CurrentIteration = 0;

                // set complexity and also thing values
            //}

            CompileConvergentSeries();
            CompileValueVsCountSeries();
            CompileSolvedProductSeries();
            CompileAverageContributorsSeries();

        }

        Product GetAgentOwnProduct(Agent A) //dit returned altijd de eerste
        {
            foreach (Product P in A.Products)
            {
                if (P.OriginAgent == A.ID)
                    return P;
            }

            return null;
        }


        private bool ContainsProduct(Product A, Agent B)
        {
              foreach (Product P in B.Products)
              {
                if (P.OriginProductID == A.OriginProductID)
                  return true;
              }

              return false;
        }

        void draw()
        {
            //background(255);

            //fill(0);

            //textSize(12);

            //text("Agents: " + AgentCount, 10, 20);
            //text("Iterations: " + Iterations, 100, 20);
            //text("Iteration time interval: " + IterationTime, 200, 20);
            //text("Starting license: " + Licenses.get(SampleLicense), 375, 20);

            //in labels dit en hierboven
            //if (SimulateNow)
            //  text("Current iteration: " + CurrentIteration, 10, 40);

            //text("Public space variety: " + PublicSpaceComplexity, 155, 40);
            //text("Finished products: " + FinishedProducts.Count(), 345, 40);

            //ellipseMode(CENTER);

            //strokeWeight(1);
            //stroke(0);

            //fill(255);


            //public space complexity, strokewidth
            // strokeWeight(PublicSpaceComplexity);
            //   fill(0, 0, 0, 0);
            // ellipse(width / 2, height / 2, 900, 900);

            //overal group strength
            //wat hier?

            //if (Simulate && frameCount % IterationTime == 0)
            //  Simulate();
        }

        private void More(object sender, RoutedEventArgs e)
        {
            AgentCount++;
            RepopAgents();
        }

        private void Less(object sender, RoutedEventArgs e)
        {
            AgentCount--;
            RepopAgents();
        }

        private void Iterate(object sender, RoutedEventArgs e)
        {
            Simulate();
            lblIteration.Content = "Iteration: " + CurrentIteration;
        }

        private void Reset(object sender, RoutedEventArgs e)
        {
            DivergenceConvergencePlot.Series.Clear();
            DivergenceConvergencePlot.InvalidatePlot(); //force rerender

            ValueSeries.ItemsSource = null;
            ValueSeries.InvalidateVisual();

            CountSeries.ItemsSource = null;
            CountSeries.InvalidateVisual();

            SolvedSeriesItems = new List<DataPoint>();
            SolvedProductsPlot.Series.Clear();
            SolvedProductsPlot.InvalidatePlot();

            
            AverageContrib.Series.Clear();
            AverageContrib.InvalidatePlot();

            CurrentIteration = 0;
            RepopAgents();
        }

        private void Previous(object sender, RoutedEventArgs e)
        {
            if(SampleLicense != 0)
                SampleLicense--;

            lblLicense.Text = Licenses[SampleLicense];

            RepopAgents();
        }

        private void Next(object sender, RoutedEventArgs e)
        {
            if (SampleLicense != Licenses.Count - 1)
                SampleLicense++;

            lblLicense.Text = Licenses[SampleLicense];

            RepopAgents();
        }

        private void LessIterations(object sender, RoutedEventArgs e)
        {
            if (Iterations > 1)
            {
                Iterations--;
                txtIterations.Text = Iterations + " iterations";
                RepopAgents();
            }
        }

        private void MoreIterations(object sender, RoutedEventArgs e)
        {
            Iterations++;
            txtIterations.Text = Iterations + " iterations";
            RepopAgents();
        }

    }
}
