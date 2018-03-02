public class Integer2 {
    public int x { get; set; }
    public int y { get; set; }

    public Integer2 zero {
        get {
            return new Integer2(0, 0);
        }
    }

    public Integer2(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public override bool Equals(object obj) {
        if (obj is Integer2)
            return x.Equals((obj as Integer2).x) && y.Equals((obj as Integer2).y);
        else return false;
    }

    public override string ToString() {
        return string.Format("[Integer2: x={0}, y={1}]", x, y);
    }
}
