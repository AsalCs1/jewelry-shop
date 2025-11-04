using System;
using core.Entities;

namespace Core.Specifications;

public class BrandListSpecification : BaseSpecification<Product, string>
{
   public BrandListSpecification()
     {
        AddSelect(x => x.Brand);
        ApplyDistinct();
    } 
}
