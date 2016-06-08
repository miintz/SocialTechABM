public class Agent
{
  int ID;
  ArrayList<Product> Products; // 0 is geen property (kan een agent meerdere properties hebben?)
  int TotalValue;
  //int[] Relations; 

  Agent(int _id, boolean _product)
  {
    ID = _id;

    Products = new ArrayList<Product>();    
    
    if (_product)
    {
      Product p = new Product(SampleLicense, ProductIDer);
      Products.add(p);
      ProductIDer++;
    }
  }
  
  public void UpdateValue()
  {
    TotalValue = 0;
    for(int u = 0; u < Products.size(); u++)
    {
      TotalValue += Products.get(u).Value;
    }
  }
  
  public void PrintProduct()
  {
    println("Products of Agent " + ID);
    if(Products.size() != 0)
    {
      for(int i = 0; i < Products.size(); i++)
      {
        println(i + ": " + Products.get(i).Value + " " + Licenses.get(Products.get(i).License) + " ID: " + Products.get(i).ID);
      }
    }
    else
      println("No products");
  }
}