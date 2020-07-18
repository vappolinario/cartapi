using System;

namespace GrainInterfaces.States
{
    [Serializable]
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductCategory { get; set; }
        public string SubCategory { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; }
        public double ProductWeight { get; set; }
    }
}
