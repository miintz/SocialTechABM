public class Agent
{
  int ID;
  String Name;
  ArrayList<Product> Products; // 0 is geen property (kan een agent meerdere properties hebben?)
  int TotalValue;
  //int[] Relations; 

  Agent(int _id, boolean _product)
  {
    ID = _id;

    Products = new ArrayList<Product>();    

    int index = int(random(0, AgentNames.size()));

    Name = AgentNames.get(index);
    AgentNames.remove(index);
    
    if (_product)
    {
      Product p = new Product(SampleLicense, ProductIDer);
            
      p.OriginAgent = ID;
      p.NameOfOriginAgent = Name;
      p.OriginProductID = ProductIDer;
      
      Products.add(p);      
      ProductIDer++;      
    }
  }

  public void UpdateValue()
  {
    TotalValue = 0;
    for (int u = 0; u < Products.size (); u++)
    {
      TotalValue += Products.get(u).Value;
    }
  }

  public void PrintProduct()
  {
    println("Products of Agent " + Name);
    if (Products.size() != 0)
    {
      for (int i = 0; i < Products.size (); i++)
      {
        println(i + ": " + Products.get(i).Value + " " + Licenses.get(Products.get(i).License) + " ID: " + Products.get(i).ID + " OID: " + Products.get(i).OriginProductID + "   NOA: " + Products.get(i).NameOfOriginAgent);
      }
    } else
      println("No products");
  }
}