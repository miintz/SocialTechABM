class Agent
{
  int ID;
  int PropertyLicense; //0 is geen property (kan een agent meerdere properties hebben?)
  
  int[] Relations; //relatie met de andere agents
  
  Agent(int _id, int _PropertyLicense)
  {
    ID = _id;
    PropertyLicense = _PropertyLicense;
  }
}