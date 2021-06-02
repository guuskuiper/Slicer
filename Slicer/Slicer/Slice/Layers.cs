// unset

using Slicer.Models;
using Slicer.Utils;
using System;
using System.Collections.Generic;

namespace Slicer.Slicer.Slice
{
    public class Layers : ILayers
    {
        private readonly Project _project;
        private readonly ISliceLayer _sliceLayer;
        private readonly IParallelScope _parallelScope;

        public Layers(Project project, ISliceLayer sliceLayer, IParallelScope parallelScope)
        {
            _project = project;
            _sliceLayer = sliceLayer;
            _parallelScope = parallelScope;
        }

        public List<Layer> CreateLayers(STL stl, bool parallel)
        {
            var layers = CreateEmptyLayer(stl);

            if (parallel)
            {
                _parallelScope.Parallelize<Layer, ISliceLayer>(layers, (layer, sliceLayer) => sliceLayer.CreateLayerContour(layer, stl));
            }
            else
            {
                foreach (Layer emptyLayer in layers)
                {
                    _sliceLayer.CreateLayerContour(emptyLayer, stl);
                }
            }

            return layers;
        }

        private List<Layer> CreateEmptyLayer(STL stl)
        {
            var maxZ = GetMaxZ(stl);
            var layers = new List<Layer>();

            float h = _project.setting.LayerHeight;

            while (h < maxZ)
            {
                layers.Add(new Layer
                {
                    Height = h,
                    Thickness = _project.setting.LayerHeight
                });

                h += _project.setting.LayerHeight;
            }

            return layers;
        }

        private float GetMaxZ(STL stl)
        {
            float currentMax = 0;
            foreach (Triangle triangle in stl.Triangles)
            {
                var max = Math.Max(Math.Max(triangle.Vertex1.Z, triangle.Vertex2.Z), triangle.Vertex3.Y);
                currentMax = Math.Max(currentMax, max);
            }
            return currentMax;
        }
    }
}