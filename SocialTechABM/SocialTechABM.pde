int Iterations = 1; //well, iterations. 
int CurrentIteration = 0;
int IterationTime = 5; //frames
int AgentCount = 8; //laten we simpel beginnen (ofzo)
int AgentProductInit = 8; //number of agents that start with a product.
int PublicSpaceComplexity; //variety in available data
int GroupStrength; //strength of relations

int SampleLicense = 1;

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

  repopAgents();
}

void repopAgents()
{
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

void draw()
{
  background(255);

  fill(0);

  textSize(12);
  text("Agents: " + AgentCount, 10, 20);
  text("Iterations: " + Iterations, 100, 20);
  text("Iteration time interval: " + IterationTime, 200, 20);
  text("License allowed: " + SampleLicense, 375, 20);  

  if (Simulate)  
    text("Current iteration: " + CurrentIteration, 10, 40);

  text("Public space variety: " + PublicSpaceComplexity, 155, 40);
  text("Group strength: " + GroupStrength, 345, 40);

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
      text(i, x-10, y+5);
      //text(Licenses.get(Agents.get(i).PropertyLicense).Name, x-10, y-30);
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

void Simulate()
{
  for (int i = 0; i < AgentCount; i++)
  {
    
  }

  CurrentIteration++;

  if (CurrentIteration == Iterations)  
  {
    Simulate = false;
    CurrentIteration = 0;

    // set complexity and also thing values
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
  }
}

