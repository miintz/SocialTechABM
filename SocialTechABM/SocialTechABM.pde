// TODO
/*
  new products get random license in case of non sharealike and noderivs license. Probably not realistic
 new products get a random added value, is this realistic?  
 how does commercial usage translate in value of product? we  may need some indicator of monetary value
 noderivs and nocommercial-noderivs are handled the same
 */

import java.util.Collections;

ArrayList<Product> FinishedProducts = new ArrayList<Product>();

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
  size(1000, 1000);

  Licenses.add("Attribution");
  Licenses.add("Attribution-ShareAlike");
  Licenses.add("Attribution-NoDerivs");
  Licenses.add("Attribution-NonCommercial");
  Licenses.add("Attribution-NonCommercial-ShareAlike");
  Licenses.add("Attribution-NonCommercial-NoDerivs");

  //Alles is attribution
  //2 zijn ShareAlike = alle dingen krijgen dus dezelfde ShareAlike, maar de content kan veranderen
  //2 zijn NoDerivs = alle content blijft hetzelfde,
  //3 zijn NonCommercial = content mag veranderen, maar de schommeling is lager.

  repopAgents();
}

void repopAgents()
{
  //reset id counter
  ProductIDer = 0;

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
    ProductIDer++;
    break;
  case 1: //Attribution-ShareAlike
    np = new Product(source.License, ProductIDer);
    np.Value = source.Value + int(random(50, 150));
    ProductIDer++;
    break;
  case 2: //Attribution-NoDerivs
    //check if this product is already in list of new agent
    present = false;
    for (int i = 0; i < A.Products.size(); i++)
    {
      if (A.Products.get(i).ID == source.ID)
        present = true;
    }
    
    if (!present)
    {
      np = new Product(source.License, source.ID);
      np.Value = source.Value;
    } else
    {            
      for (int p = 0; p < B.Products.size(); p++)
      {  
        int Bid = B.Products.get(p).ID;
        //bestaat deze al in de target?
        boolean intarget = false;
        for (int o = 0; o < A.Products.size(); o++)
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
          break;
        }
      }
    }
    break;
  case 3: //Attribution-NonCommercial   
    np = new Product(int(random(0, 5)), ProductIDer); //use whatever license
    np.Value = source.Value + int(random(50, 100)); //iets minder value, 
    ProductIDer++;
    break;
  case 4: //Attribution-NonCommercial-ShareAlike
    np = new Product(source.License, ProductIDer);
    np.Value = source.Value + int(random(50, 100)); //iets minder value,
    ProductIDer++;
    break;
  case 5: //Attribution-NonCommercial-NoDerivs
    //check if this product is already in list of new agent
    present = false;
    for (int i = 0; i < A.Products.size(); i++)
    {
      if (A.Products.get(i).ID == source.ID)
        present = true;
    }

    if (!present)
    {
      np = new Product(source.License, source.ID);
      np.Value = source.Value;
    } else
    {            
      for (int p = 0; p < B.Products.size(); p++)
      {  
        int Bid = B.Products.get(p).ID;
        //bestaat deze al in de target?
        boolean intarget = false;
        for (int o = 0; o < A.Products.size(); o++)
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
  ArrayList<Integer> indices = new ArrayList<Integer>();
  for (int u = 0; u < Agents.size(); u++)
  {
    if (Agents.get(u).Products.size() != 0 && Agents.get(u).ID != self)
    {
      indices.add(u);
    }
  }

  Collections.shuffle(indices);

  return Agents.get(indices.get(0));
}

void CheckForFinished()
{
  for (int y = 0; y < Agents.size(); y ++)
  {
    for (int x = 0; x < Agents.get(y).Products.size(); x++)
    {
      if (Agents.get(y).Products.get(x).Value > 850)
      {
        //throw in finished works
        FinishedProducts.add(Agents.get(y).Products.get(x));
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

    //hier moet dus een kans komen dat er verder word gewerkt aan iets wat bestaand

    //find product
    Agent B = returnRandomAgentWithProduct(A.ID);
    //println("Agent " + A.ID + " is takin " + B.ID + "s shit");

    //evaluate license
    Product P = evaluateSourceLicense(B, A);
    if(P != null)
    {
      A.Products.add(P);
      NoProductsDispensed = false;
    }         

    A.UpdateValue();
  }
  
  CurrentIteration++;
  
  if(NoProductsDispensed)
    println("No products were dispensed, end of test run / 100% convergence?");
  
  if (CurrentIteration == Iterations)  
  {
    Simulate = false;
    CurrentIteration = 0;

    // set complexity and also thing values
  }
}

void draw()
{
  background(255);  

  fill(0);

  textSize(12);

  text("Agents: " + AgentCount, 10, 20);
  text("Iterations: " + Iterations, 100, 20);
  text("Iteration time interval: " + IterationTime, 200, 20);
  text("Starting license: " + Licenses.get(SampleLicense), 375, 20);  

  if (Simulate)  
    text("Current iteration: " + CurrentIteration, 10, 40);

  text("Public space variety: " + PublicSpaceComplexity, 155, 40);
  text("Finished products: " + FinishedProducts.size(), 345, 40);

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
      text("P: " + Agents.get(i).Products.size(), x-10, y-30);
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