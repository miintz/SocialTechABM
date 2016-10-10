// TODO
/*
   new products get random license in case of non sharealike and noderivs license. Probably not realistic
 new products get a random added value, is this realistic?  
 
 how does commercial usage translate in value of product? we  may need some indicator of monetary value
 
 noderivs and nocommercial-noderivs are handled the same
 
 thought: it makes sense that products with a higher value eventually become non-derivs to maintain standards.
 */

import java.util.Collections;
private ArrayList<String> AgentNames = new ArrayList<String>();
ArrayList<Product> SolvedProducts = new ArrayList<Product>();

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

boolean PrintInfo = true;
boolean ShowIDs = true;
boolean Simulate = false;

ArrayList<Agent> Agents = new ArrayList<Agent>();
ArrayList<String> Licenses = new ArrayList<String>();

void setup()
{  
  PopulateNames();

  size(1000, 1000);

  Licenses.add("Attribution");
  Licenses.add("Attribution-ShareAlike");
  Licenses.add("Attribution-NoDerivs");
  Licenses.add("Attribution-NonCommercial");
  Licenses.add("Attribution-NonCommercial-ShareAlike");
  Licenses.add("Attribution-NonCommercial-NoDerivs");

  SampleLicense = 2;

  //Alles is attribution
  //2 zijn ShareAlike = alle dingen krijgen dus dezelfde ShareAlike, maar de content kan veranderen
  //2 zijn NoDerivs = alle content blijft hetzelfde,
  //3 zijn NonCommercial = content mag veranderen, maar de schommeling is lager.

  repopAgents();
}

void repopAgents()
{
  PopulateNames();

  //reset id counter
  ProductIDer = 0;

  SolvedProducts.clear();

  //grap some numbers for product assignment
  int[] ProductAgents = new int[AgentProductInit];

  for (int j = 0; j < AgentProductInit; j++) { 
    ProductAgents[j] = -1;
  }

  Agents = new ArrayList<Agent>();

  for (int y = 0; y < ProductAgents.length; y++)
  {
    boolean unique = false;
    int r = 0;

    while (!unique)
    {
      r = int(random(0, AgentCount));
      boolean present = false;

      for (int x  = 0; x < ProductAgents.length; x++)
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

  for (int u = 0; u < AgentCount; u++)
  {    
    Agent a = new Agent(u, false);

    //assign products if number equals a different number
    for (int n = 0; n < ProductAgents.length; n++)
    {
      if (u == ProductAgents[n])
      {
        a = new Agent(u, true);
        break;
      }
    }    

    Agents.add(a);
  }
}

Product evaluateSourceLicense(Agent B, Agent A)
{
  ArrayList<Product> Prods = B.Products;

  //grab a random for now
  Product source = Prods.get(int(random(0, Prods.size())));

  int License = source.License;
  boolean present = false;
  Product np = null;

  switch(License)
  {
  case 0: //Attribution
    int ne = int(random(0, 5));  
    np = new Product(ne, ProductIDer); //use whatever license
    np.Value = source.Value + int(random(50, 150));

    np.OriginAgent = source.OriginAgent;
    np.NameOfOriginAgent = source.NameOfOriginAgent;

    ProductIDer++;
    break;
  case 1: //Attribution-ShareAlike
    np = new Product(source.License, ProductIDer);
    np.Value = source.Value + int(random(50, 150));

    np.OriginAgent = source.OriginAgent;
    np.NameOfOriginAgent = source.NameOfOriginAgent;

    ProductIDer++;
    break;
  case 2: //Attribution-NoDerivs
    //check if this product is already in list of new agent
    present = false;
    for (int i = 0; i < A.Products.size (); i++)
    {
      if (A.Products.get(i).ID == source.ID)
        present = true;
    }

    if (!present)
    {
      np = new Product(source.License, source.ID);
      np.Value = source.Value;
      ProductIDer++;
    } else
    {            
      for (int p = 0; p < B.Products.size (); p++)
      {  
        int Bid = B.Products.get(p).ID;
        //bestaat deze al in de target?
        boolean intarget = false;
        for (int o = 0; o < A.Products.size (); o++)
        {
          if (A.Products.get(o).ID == Bid)
          {            
            intarget = true;
            break;
          }
        }

        if (!intarget)
        {
          np = new Product(B.Products.get(p).License, B.Products.get(p).ID);
          np.Value = B.Products.get(p).Value;
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
    np = new Product(int(random(0, 5)), ProductIDer); //use whatever license
    np.Value = source.Value + int(random(50, 100)); //iets minder value, 
    ProductIDer++;

    np.OriginAgent = source.OriginAgent;
    np.NameOfOriginAgent = source.NameOfOriginAgent;
    break;
  case 4: //Attribution-NonCommercial-ShareAlike
    np = new Product(source.License, ProductIDer);
    np.Value = source.Value + int(random(50, 100)); //iets minder value,
    ProductIDer++;

    np.OriginAgent = source.OriginAgent;
    np.NameOfOriginAgent = source.NameOfOriginAgent;
    break;
  case 5: //Attribution-NonCommercial-NoDerivs
    //check if this product is already in list of new agent
    present = false;
    for (int i = 0; i < A.Products.size (); i++)
    {
      if (A.Products.get(i).ID == source.ID)
        present = true;
    }

    if (!present)
    {
      np = new Product(source.License, source.ID);
      np.Value = source.Value;
      ProductIDer++;
    } else
    {            
      for (int p = 0; p < B.Products.size (); p++)
      {  
        int Bid = B.Products.get(p).ID;
        //bestaat deze al in de target?
        boolean intarget = false;
        for (int o = 0; o < A.Products.size (); o++)
        {
          if (A.Products.get(o).ID == Bid)
          {
            intarget = true;
            break;
          }
        }

        if (!intarget)
        {
          np = new Product(B.Products.get(p).License, B.Products.get(p).ID);
          np.Value = B.Products.get(p).Value;
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
  ArrayList<Integer> indices = new ArrayList<Integer>();
  for (int u = 0; u < Agents.size (); u++)
  {
    if (Agents.get(u).Products.size() != 0 && Agents.get(u).ID != self)
    {
      indices.add(u);
    }
  }

  Collections.shuffle(indices);

  return Agents.get(indices.get(0));
}

boolean ContainsProduct(Product A, Agent B)
{
  for (Product P : B.Products)
  {
    if (P.OriginProductID == A.OriginProductID)
      return true;
  }

  return false;
}

Product GetAgentOwnProduct(Agent A) //dit returned altijd de eerste
{
  for (Product P : A.Products)
  {
    if (P.OriginAgent == A.ID)
      return P;
  }

  return null;
}

void CheckForFinished()
{
  for (int y = 0; y < Agents.size (); y ++)
  {
    for (int x = 0; x < Agents.get (y).Products.size(); x++)
    {
      if (Agents.get(y).Products.get(x).Value > 850)
      {
        //throw in finished works
        SolvedProducts.add(Agents.get(y).Products.get(x));
        Agents.get(y).Products.remove(x);
      }
    }
  }
}

void Simulate()
{
  CheckForFinished();

  boolean NoProductsDispensed = true;

  for (int i = 0; i < AgentCount; i++)
  {
    //make every agent take stuff from other agents
    Agent A = Agents.get(i);

    //paar mogelijkheden. 
    /*
    1. agent pakt een nieuw product
     2. agent werkt verder aan eigen product.
     */


    //30-70 in het voordeel van doorwerken
    //is het sharealike? score hoger dan iets, kans van 25 dat de noderivs eraf gaat    

    boolean WorkOnOwnProduct = random(0, 100) > 70 ? true : false;
    boolean Default = !WorkOnOwnProduct;

    if (WorkOnOwnProduct)
    {
      //2. divergence (ratio van authors en het aantal producten)
      //2.1 op basis daarvan kijken of bepaalde agents het beter doen dan anderen (puur op basis van licentie) 
      //3. kans op ontwikkeling eigen product

      Product P = GetAgentOwnProduct(A);
      if (P != null)
      {
        P.Value += random(75, 150); //random score...
        println("Agent " + A.Name + " worked on own product (ID: " +P.ID+ " OID: " + P.OriginProductID + ") , new value: " + P.Value);
        if (P.Value > 750)
        {
          //waarom gebruiken mensen noderivs? Aan de ene kant om de ontwikkeling te beschermen, aan de andere kant om het resultaat te beschermen?
          if (P.License == 2 || P.License == 5)
          {
            //dit is dus in het geval dat de voortgang word beschermd, nu is die voortgang bij een punt dat het 'vrijgegeven' kan worden              
            boolean ChangeLicense = random(0, 100) < 15 ? false : true;              
            if (ChangeLicense)
            {
              //nu is het noderivs. pak een andere 
              ArrayList<Integer> li = new ArrayList<Integer>();

              li.add(0);
              li.add(1);
              li.add(3);
              li.add(4);

              Collections.shuffle(li);

              P.License = li.get(0);

              println("... and changed license to: " + Licenses.get(P.License));

              //nu alle andere producten vinden die afkomstig van deze agent en ook de license veranderen
              for (Agent B : Agents)
              {
                for (Product O : B.Products)
                {
                  if (O.OriginProductID == P.OriginProductID)
                  {                   
                    O.License = P.License;
                    println("... and " + B.Name + " thus changed product license of " + O.OriginProductID + " to " + Licenses.get(O.License));
                  }
                }
              }
            }
          }
        }
      } else
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
          A.Products.add(P);
          NoProductsDispensed = false;
        }
      }         

      A.UpdateValue();
    }
  }

  CurrentIteration++;

  if (NoProductsDispensed)
    println("No products were dispensed, end of test run / 100% convergence?");

  if (CurrentIteration == Iterations)  
  {
    Simulate = false;
    CurrentIteration = 0;

    // set complexity and also thing values
  }
}

boolean NoobMode = true;

void draw()
{
  background(255);  

  fill(0);

  textSize(12);

  if (!NoobMode)
  {
    text("Agents: " + AgentCount, 10, 20);
    text("Iterations: " + Iterations, 100, 20);
    text("Iteration time interval: " + IterationTime, 200, 20);
    text("Starting license: " + Licenses.get(SampleLicense), 375, 20);  
    text("press v for controls", width - 125, 20);

    if (Simulate)  
      text("Current iteration: " + CurrentIteration, 10, 40);

    text("Public space variety: " + PublicSpaceComplexity, 155, 40);
    text("Solved products: " + SolvedProducts.size(), 345, 40);

    ellipseMode(CENTER);

    strokeWeight(1);
    stroke(0);

    fill(255);

    float AngleFrag = PI * 2 / AgentCount;
    for (int i = 0; i < AgentCount; i++ )
    {
      float a = AngleFrag * i;

      float x = (width / 2) + (400 * cos(a));
      float y = (width / 2) + (400 * sin(a));

      fill(255);
      ellipse(x, y, 50 - (AgentCount / 2), 50  - (AgentCount / 2));

      if (ShowIDs)
      {
        fill(0);
        if (i == CurrentAgent)
          textSize(20);
        else
          textSize(12);

        text(i, x-10, y+5);
        text(Agents.get(i).Name + " P: " + Agents.get(i).Products.size(), x-10, y-30);
      }
    }

    //public space complexity, strokewidth
    strokeWeight(PublicSpaceComplexity);
    fill(0, 0, 0, 0);
    ellipse(width / 2, height / 2, 900, 900);

    //overal group strength
    //wat hier?

    if (Simulate && frameCount % IterationTime == 0)  
      Simulate();
  }
  else
  {
    text("press v for model", width - 125, 20);
    
    text(", or . : change initial license type", 10, 20);
    text("- or = : increase/decrease number of agents by 1", 10, 40);
    text("[ or ] : increase/decrease number of agents by 10", 10, 60);
    text("q : run simulation", 10, 80);
    text("o or p : increase/decrease number of iterations", 10, 100);
    text("k or l: increase/decrease iteration speed (purely cosmetic)", 10, 120);
    text("n: show/hide agent names" , 10, 140);
    text("arrow key left/right: Goto next agent (printed BOLD)", 10, 160);
    text("spacebar: print info of selected agent (look console)", 10, 180);
  }
}

void keyPressed()
{  
  switch(key)
  {
  case 'd':
    PrintInfo = (PrintInfo) ? false : true;
    break;
  case 'q':
    Simulate = true;
    break;
  case '[':
    if (AgentCount > 10)
      AgentCount -= 10;

    repopAgents();
    break;
  case ']':
    AgentCount += 10;
    repopAgents();
    break;
  case '-':
    if (AgentCount > 0)
      AgentCount--;

    repopAgents();
    break;
  case '=':
    AgentCount++;    
    repopAgents();
    break;
  case 'o':
    if (Iterations > 0)
      Iterations --;
    break;
  case 'p':
    Iterations++;
    break;
  case 'k':
    if (IterationTime > 0)
      IterationTime --;
    break;
  case 'l':
    IterationTime++;
    break;
  case 'n':
    ShowIDs = (ShowIDs) ? false : true;
    break;
  case '.':
    if (SampleLicense < 5)
      SampleLicense++;

    repopAgents();
    break;
  case ',':
    if (SampleLicense > 0)
      SampleLicense--;

    repopAgents();
    break;
  case 'v':
    NoobMode = (NoobMode ? false : true);
    break;
  }

  switch(keyCode)
  {
  case 39: //->
    if (CurrentAgent != Agents.size() - 1)
      CurrentAgent++;
    else
      CurrentAgent = 0;
    break;
  case 37: //<-
    if (CurrentAgent != 0)
      CurrentAgent--;
    else
      CurrentAgent = Agents.size() - 1;
    break;
  case 32:
    Agents.get(CurrentAgent).PrintProduct();
    break;
  }
}

private void PopulateNames() //slecht, elke agent heeft nu deze lijst
{
  AgentNames = new ArrayList<String>();

  AgentNames.add("Aaron");
  AgentNames.add("Benny");
  AgentNames.add("Claude");
  AgentNames.add("Dwayne");
  AgentNames.add("Ezekiel");
  AgentNames.add("Fred");
  AgentNames.add("Gunther");
  AgentNames.add("Heather");
  AgentNames.add("Ivan");
  AgentNames.add("James");
  AgentNames.add("Karl");
  AgentNames.add("Leeroy");
  AgentNames.add("Martin");
  AgentNames.add("Nolan");
  AgentNames.add("Olga");
  AgentNames.add("Peter");
  AgentNames.add("Quintus");
  AgentNames.add("Reed");
  AgentNames.add("Steve");
  AgentNames.add("Tyrone");
  AgentNames.add("Uma");
  AgentNames.add("Victor");
  AgentNames.add("Wesley");
  AgentNames.add("Xavier");
  AgentNames.add("Yvette");
  AgentNames.add("Zeke");

  AgentNames.add("Anton");
  AgentNames.add("Bradley");
  AgentNames.add("Christian");
  AgentNames.add("Devon");
  AgentNames.add("Eloy");
  AgentNames.add("Frank");
  AgentNames.add("George");
  AgentNames.add("Hetfield");
  AgentNames.add("Irene");
  AgentNames.add("Johnny");
  AgentNames.add("Keith");
  AgentNames.add("LeBron");
  AgentNames.add("Michael");
  AgentNames.add("Ned");
  AgentNames.add("Octavo");
  AgentNames.add("Pavel");
  AgentNames.add("Quinn");
  AgentNames.add("Ravel");
  AgentNames.add("Stephen");
  AgentNames.add("Tola");
  AgentNames.add("Ulf");
  AgentNames.add("Val");
  AgentNames.add("Walter");
  AgentNames.add("Xander");
  AgentNames.add("Yale");
  AgentNames.add("Zack");
}