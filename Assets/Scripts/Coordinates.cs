public class Coordinates {
    int _x = 0;
    public int x
    {
        get
        {
            return _x;
        }
        set
        {
            _x = value;
        }
    }

    int _y = 0;
    public int y
    {
        get
        {
            return _y;
        }
        set
        {
            _y = value;
        }
    }

    public Coordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override bool Equals(object other)
    {
        return other is Coordinates && this.Equals((Coordinates)other);
    }

    public bool Equals(Coordinates otherCoordinates)
    {
        return x == otherCoordinates.x && y == otherCoordinates.y;
    }

    public override int GetHashCode()
    {
        return x.GetHashCode() ^ y.GetHashCode();
    }

    public override string ToString()
    {
        return "x=" + x + ";y=" + y;
    }

    public bool AreCorrect()
    {
        return x >= 0 && x < Constants.k_xGridDimension && y >= 0 && y < Constants.k_yGridDimension;
    }
}
