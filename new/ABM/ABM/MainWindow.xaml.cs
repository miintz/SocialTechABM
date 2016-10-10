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
        int ProductIDer = 0;
        int SampleLicense = 0;

        bool PrintInfo = true;
        bool ShowIDs = true;
        
        List<Product> FinishedProducts = new List<Product>();

        public MainWindow()
        {
            InitializeComponent();

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
            for (int i = 0; i < AgentCount; i++)
            {
                float a = AngleFrag * i;

                double x = (Width / 2) + (300 * Math.Cos(a));
                double y = (Width / 2) + (300 * Math.Sin(a));

                //fill(255);

                //AGENT HIER TEKENENENENENENENE
                //ellipse(x, y, 50 - (AgentCount / 2), 50 - (AgentCount / 2));

                Agent age = new Agent(i, false);
                
                //assign products if number equals a different number
                for (int n = 0; n < ProductAgents.Count(); n++)
                {
                    if (i == ProductAgents[n])
                    {
                        age = new Agent(i, true);
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
                    //text("P: " + Agents.get(i).Products.size(), x - 10, y - 30);
                }
            }
        }

        Product evaluateSourceLicense(Agent B, Agent A)
        {
            List<Product> Prods = B.Products;

            Random rand = new Random();
            //grab a random for now
            Product source = Prods[(int)rand.Next(0, Prods.Count())];

            int License = source.License;
            bool present = false;
            Product np = null;

            switch (License)
            {
                case 0: //Attribution
                    int ne = (int)rand.Next(0, 5);
                    np = new Product(ne, ProductIDer); //use whatever license
                    np.Value = source.Value + (int)rand.Next(50, 150);
                    ProductIDer++;
                    break;
                case 1: //Attribution-ShareAlike
                    np = new Product(source.License, ProductIDer);
                    np.Value = source.Value + (int)rand.Next(50, 150);
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
                                break;
                            }
                        }
                    }
                    break;
                case 3: //Attribution-NonCommercial   
                    np = new Product(rand.Next(0, 5), ProductIDer); //use whatever license
                    np.Value = source.Value + rand.Next(50, 100); //iets minder value, 
                    ProductIDer++;
                    break;
                case 4: //Attribution-NonCommercial-ShareAlike
                    np = new Product(source.License, ProductIDer);
                    np.Value = source.Value + rand.Next(50, 100); //iets minder value,
                    ProductIDer++;
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
                                break;
                            }
                        }
                    }
                    break;
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
                        Agents[y].Products.RemoveAt(x);
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

                //hier moet dus een kans komen dat er verder word gewerkt aan iets wat bestaand

                //find product
                Agent B = returnRandomAgentWithProduct(A.ID);
                //println("Agent " + A.ID + " is takin " + B.ID + "s shit");

                //evaluate license
                Product P = evaluateSourceLicense(B, A);
                if (P != null)
                {
                    A.Products.Add(P);
                    NoProductsDispensed = false;
                }

                A.UpdateValue();
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

    }
}
