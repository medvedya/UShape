using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace UShape.PolyGeneration
{
    interface IPolyGenerator
    {
        bool Generate(PolyShape polyShape);
    }
}
