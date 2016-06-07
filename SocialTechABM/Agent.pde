public class Agent
{
  int ID;
  Product[] Products; // 0 is geen property (kan een agent meerdere properties hebben?)
  
  //int[] Relations; 
  
  Agent(int _id, boolean _product)
  {
    ID = _id;
    
     if(_product)
     {
       Product p = new Product(SampleLicense);
     }
  }
}
