using System;
using System.Collections.Generic;
using System.Linq;

namespace GrainInterfaces.States
{
  [Serializable]
  public class Cart
  {
      public Guid Id { get; set; }
      public List<Product> Products { get; set; }
      public string PromoCode { get; set; }
      public decimal Total => Products?.Sum(p => p.ProductPrice) ?? 0m;
  }
}
