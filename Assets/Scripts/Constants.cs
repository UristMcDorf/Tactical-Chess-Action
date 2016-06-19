using System.Collections.Generic;

public static class Constants {
    public const int k_xGridDimension = 8;
    public const int k_yGridDimension = 8;

    public const float k_xCellSize = 1f;
    public const float k_yCellSize = 1f;

    public static readonly List<string> k_unitTypes = new List<string>(new string[] {"Bishop", "King", "Knight", "Pawn", "Rook", "Queen"});
    public static readonly List<string> k_unitOrigins = new List<string>(new string[] {"Default", "Beasts", "Devout", "Mages", "Military", "Primal", "Shadows"});
    public static readonly List<string> k_unitSides = new List<string>(new string[] {"Black","White"});
}
