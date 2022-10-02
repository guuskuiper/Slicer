namespace Slicer.Models;

public class Rect
{
    public long left;
    public long top;
    public long right;
    public long bottom;

    public Rect(long l, long t, long r, long b)
    {
        this.left = l;
        this.top = t;
        this.right = r;
        this.bottom = b;
    }

    public Rect(Rect rect)
    {
        this.left = rect.left;
        this.top = rect.top;
        this.right = rect.right;
        this.bottom = rect.bottom;
    }
}