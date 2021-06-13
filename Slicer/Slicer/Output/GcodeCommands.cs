// unset

using ClipperLib;

namespace Slicer.Slicer.Output
{
    public class GcodeCommands : IGcodeCommands
    {
        public string TravelZ(double z, double speed)
        {
            return $"G0 Z{z:F3} F{speed:F3}";
        }

        public string TravelXY(IntPoint pt, double speed)
        {
            return $"G0 X{(pt.X / 1000.0):F3} Y{(pt.Y / 1000.0):F3} F{speed:F3}";
        }

        public string Extrude(IntPoint pt, double volume, double speed)
        {
            return $"G1 X{(pt.X / 1000.0):F3} Y{(pt.Y / 1000.0):F3} F{speed:F3} E{volume:F3}";
        }
    }
}