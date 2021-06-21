// unset

namespace Slicer.Middleware
{
    public static class MiddlewareContainerExtensions
    {
        public static MiddlewareContainer AddMiddleware(this MiddlewareContainer container, ISlicerMiddelware middelware)
        {
            container.Add(middelware);
            return container;
        }
    }
}