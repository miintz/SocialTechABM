public class Product
{
  int ID;  
  int Value;
  
  public int License;
  
  //product heeft een origin, wie is het werk begonnen
  //de origin veranderd niet, elke license verplicht dat je de originator credit. 
  int OriginProductID;
  int OriginAgent;
  String NameOfOriginAgent;
  
  //product heeft ook een lijst met mensen die er aan meegewerkt hebben.  
  //deze word niet gebruikt met NoDerivs.
  int[] Contributors;
  
  Product(int _l, int _id)
  {
    License = _l;
    Value = 500;
    ID = _id;
  }
}