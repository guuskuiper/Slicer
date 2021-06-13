// unset

using ClipperLib;

namespace Slicer.Slicer.Output
{
    public interface IGcodeCommands
    {
        string TravelZ(double z, double speed);
        string TravelXY(IntPoint pt, double speed);
        string Extrude(IntPoint pt, double volume, double speed);
    }
}