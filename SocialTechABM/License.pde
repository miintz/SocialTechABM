public class License 
{
  public int ID;
  public String Name;  

  //beiden 0 tot 100
  public int CommunicationChanceMod;
  public int CreativeFreedomMod;

  License(int _id, String _name) 
  {
    Name = _name;
    ID = _id;
  } 
}