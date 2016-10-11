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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Agent> Agents;

        int Iterations = 1; //well, iterations. 
        int CurrentIteration = 0;
        int IterationTime = 5; //frames
        int AgentCount = 8; //laten we simpel beginnen (ofzo)
        int AgentProductInit = 4; //number of agents that start with a product.
        int PublicSpaceComplexity; //variety in available data
        int GroupStrength; //strength of relations
        int CurrentAgent = 0;
        public static int ProductIDer = 0;
        public static int SampleLicense = 0;

        bool PrintInfo = true;
        bool ShowIDs = true;

        List<Product> FinishedProducts = new List<Product>();

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

            RepopAgents();
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

                double x = (450 / 2) + (190 * Math.Cos(a));
                double y = (450 / 2) + (190 * Math.Sin(a));

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
                np.OriginProductID = source.ID;

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
                        lblFinishedProducts.Content = "Finished: " + FinishedProducts.Count;
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

            if (CurrentIteration == Iterations)
            {
                SimulateNow = false;
                CurrentIteration = 0;

                // set complexity and also thing values
            }
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
        }

        private void Reset(object sender, RoutedEventArgs e)
        {
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

    }
}
