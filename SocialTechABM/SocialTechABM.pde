int Iterations = 20; //well, iterations. 
int CurrentIteration = 0;
int IterationTime = 5; //frames
int AgentCount = 20; //laten we simpel beginnen (ofzo)

int PublicSpaceComplexity; //variety in available data
int GroupStrength; //strength of relations

int SampleLicense = 0;

boolean ShowIDs = true;
boolean Simulate = false;

ArrayList<License> Licenses = new ArrayList<License>();
ArrayList<Agent> Agents = new ArrayList<Agent>();

void setup()
{
  size(1000, 1000);

  //populate licences
  Licenses.add(new License(0, "Do whatever"));
  Licenses.get(0).CommunicationChanceMod = 10;
  Licenses.get(0).CreativeFreedomMod = 100;
  
  Licenses.add(new License(1, "Lenient"));
  Licenses.get(1).CommunicationChanceMod = 25;
  Licenses.get(1).CreativeFreedomMod = 80;
  
  Licenses.add(new License(2, "Moderate"));
  Licenses.get(2).CommunicationChanceMod = 35;
  Licenses.get(2).CreativeFreedomMod = 60;
  
  Licenses.add(new License(3, "Cumbersome"));
  Licenses.get(3).CommunicationChanceMod = 50;
  Licenses.get(3).CreativeFreedomMod = 35;
  
  Licenses.add(new License(4, "Heavy"));
  Licenses.get(4).CommunicationChanceMod = 75;
  Licenses.get(4).CreativeFreedomMod = 25;
  
  Licenses.add(new License(5, "Satan"));
  Licenses.get(5).CommunicationChanceMod = 100;
  Licenses.get(5).CreativeFreedomMod = 10;

  for (int u = 0; u < AgentCount; u++)
  {    
    Agents.add(new Agent(u, 0));
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
  text("License allowed: " + Licenses.get(SampleLicense).Name, 375, 20);
  text("CC / CF: " + Licenses.get(SampleLicense).CommunicationChanceMod + " / " +  Licenses.get(SampleLicense).CreativeFreedomMod, 575, 20);
    
  if (Simulate)
    text("Current iteration: " + CurrentIteration, 10, 40);

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
    }
  }

  if (Simulate && frameCount % IterationTime == 0)  
    Simulate();
}

void Simulate()
{
  for (int i = 0; i < AgentCount - 1; i++)
  {
  }

  if (CurrentIteration < Iterations)  
    CurrentIteration++;
  else
  {
    Simulate = false;
    CurrentIteration = 0;
    //set complexity and also thing values
  }
}

void keyPressed()
{
  switch(key)
  {
  case 'q':
    Simulate = true;
    break;
  case '[':
    if (AgentCount > 10)
      AgentCount -= 10;
    break;
  case ']':
    AgentCount += 10;
    break;
  case '-':
    if (AgentCount > 0)
      AgentCount--;
    break;
  case '=':
    AgentCount++;
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
    if(SampleLicense < 5)
      SampleLicense++;
    break;
  case ',':
    if(SampleLicense > 0)
      SampleLicense--;
    break;
  }
}