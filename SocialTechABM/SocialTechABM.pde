Agent[] Agents;

int AgentCount = 20; //laten we simpel beginnen (ofzo)
int PublicSpaceComplexity; //variety in available data
int GroupStrength; //strength of relations

boolean ShowIDs = true;

void setup()
{
  size(1000, 1000);
}

void draw()
{
  background(255);

  fill(0);

  textSize(12);
  text("Agents: " + AgentCount, 10, 20);

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

    if(ShowIDs)
    {
      fill(0);    
      text(i, x-10, y+5);
    }
  }
}

void keyPressed()
{
  switch(key)
  {
  case 'Q':
    //GAAAAAN
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
  case 'n':
    ShowIDs = (ShowIDs) ? false : true;
    break;
  }
}