// unset

using Slicer.Models;

namespace Slicer.Slicer.Output
{
    public interface IGcodeCommands
    {
        string TravelZ(double z, double speed);
        string TravelXY(Point2D pt, double speed);
        string Extrude(Point2D pt, double volume, double speed);
    }
}