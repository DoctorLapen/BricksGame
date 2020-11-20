using UnityEngine;

namespace SuperBricks
{
   public  class Cell:ICell
   {
       public CellType Type { get; set; }
       public Color Color { get; set; }

       public Cell()
       {
           Type = CellType.Empty;
           Color = Color.white;
       }
       public Cell(CellType type,Color color)
       {
           Type = type;
           Color = color;
       }

   }

   public interface ICell
   {
       CellType Type { get; set; }
       Color Color { get; set; }
   }
}