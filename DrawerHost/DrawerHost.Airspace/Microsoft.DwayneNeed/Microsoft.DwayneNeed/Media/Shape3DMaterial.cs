using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using System.Windows;
using Microsoft.DwayneNeed.Shapes;
using System.ComponentModel;

namespace Microsoft.DwayneNeed.Media
{
    /// <summary>
    ///     Shape3DMaterial describes the material to use to display on the 
    ///     surface of a shape.
    /// </summary>
    /// <remarks>
    ///     This can be either:
    ///     1) A non-interactive Material, possibly deferred
    ///     2) A an interactive UIElement, possibly deferred
    /// </remarks>
    public class Shape3DMaterial
    {
        public Shape3DMaterial(Material material)
        {
            _getMaterial = (shape) => material;
        }

        public Shape3DMaterial(Func<Shape3D, Material> getMaterial)
        {
            _getMaterial = getMaterial;
        }

        public Shape3DMaterial(UIElement element)
        {
            _getElement = (shape) => element;
        }

        public Shape3DMaterial(Func<Shape3D, UIElement> getElement)
        {
            _getElement = getElement;
        }

        public static implicit operator Shape3DMaterial(Material material)
        {
            return new Shape3DMaterial(material);
        }

        public static implicit operator Shape3DMaterial(UIElement element)
        {
            return new Shape3DMaterial(element);
        }

        public bool HasMaterial
        {
            get
            {
                return _getMaterial != null;
            }
        }

        public bool HasElement
        {
            get
            {
                return _getElement != null;
            }
        }

        public Material GetMaterial(Shape3D shape)
        {
            if (_getMaterial != null)
            {
                return _getMaterial(shape);
            }
            return null;
        }

        public UIElement GetElement(Shape3D shape)
        {
            if (_getElement != null)
            {
                return _getElement(shape);
            }
            return null;
        }

        private Func<Shape3D, Material> _getMaterial;
        private Func<Shape3D, UIElement> _getElement;
    }
}
