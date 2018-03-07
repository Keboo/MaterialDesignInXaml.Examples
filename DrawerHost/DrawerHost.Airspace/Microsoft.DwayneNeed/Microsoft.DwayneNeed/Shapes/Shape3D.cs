using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using Microsoft.DwayneNeed.Media;
using System.Windows.Markup;
using System.Windows.Controls;
using Microsoft.DwayneNeed.Controls;
using Microsoft.DwayneNeed.Extensions;

namespace Microsoft.DwayneNeed.Shapes
{
    /// <summary>
    ///     The abstract base class for 3D shapes.
    /// </summary>
    /// <remarks>
    ///     Shape3D inherits from UIElement3D, which provides the rich set of
    ///     input properties, methods, and events.
    ///     
    ///     The geometry for a Shape3D is provided by derived types.
    ///     
    ///     The material for the surface of a Shape3D is normally defined by
    ///     2D Visuals, though derived types may specify the materials
    ///     underneath the visuals.
    /// </remarks>
    [ContentProperty("Material")]
    public abstract class Shape3D : UIElement3D
    {
        public static DependencyProperty FrontMaterialProperty =
            DependencyProperty.Register(
                "FrontMaterial",
                typeof(Shape3DMaterial),
                typeof(Shape3D),
                new PropertyMetadata(
                    new Shape3DMaterial(new DiffuseMaterial(Brushes.Red)),
                    new PropertyChangedCallback(OnPropertyChangedAffectsModel)));

        public Shape3DMaterial FrontMaterial
        {
            get { return (Shape3DMaterial)GetValue(FrontMaterialProperty); }
            set { SetValue(FrontMaterialProperty, value); }
        }

        public static DependencyProperty BackMaterialProperty =
            DependencyProperty.Register(
                "BackMaterial",
                typeof(Shape3DMaterial),
                typeof(Shape3D),
                new PropertyMetadata(
                    new Shape3DMaterial(new DiffuseMaterial(Brushes.Red)),
                    new PropertyChangedCallback(OnPropertyChangedAffectsModel)));

        public Shape3DMaterial BackMaterial
        {
            get { return (Shape3DMaterial)GetValue(BackMaterialProperty); }
            set { SetValue(BackMaterialProperty, value); }
        }

        public static DependencyProperty CacheScaleProperty =
            DependencyProperty.Register(
                "CacheScale",
                typeof(CacheScale),
                typeof(Shape3D),
                new PropertyMetadata(
                    CacheScale.Auto,
                    new PropertyChangedCallback(OnPropertyChangedAffectsModel)));

        public CacheScale CacheScale
        {
            get { return (CacheScale)GetValue(CacheScaleProperty); }
            set { SetValue(CacheScaleProperty, value); }
        }

        public static DependencyProperty SurfaceWidthProperty =
            DependencyProperty.Register(
                "SurfaceWidth",
                typeof(double),
                typeof(ParametricShape3D),
                new PropertyMetadata(
                    0.0,
                    new PropertyChangedCallback(OnPropertyChangedAffectsModel)));

        public double SurfaceWidth
        {
            get { return (double)GetValue(SurfaceWidthProperty); }
            set { SetValue(SurfaceWidthProperty, value); }
        }

        public static DependencyProperty SurfaceHeightProperty =
            DependencyProperty.Register(
                "SurfaceHeight",
                typeof(double),
                typeof(Shape3D),
                new PropertyMetadata(
                    0.0,
                    new PropertyChangedCallback(OnPropertyChangedAffectsModel)));

        public double SurfaceHeight
        {
            get { return (double)GetValue(SurfaceHeightProperty); }
            set { SetValue(SurfaceHeightProperty, value); }
        }

        public static DependencyProperty MeshTransformProperty =
            DependencyProperty.Register(
                "MeshTransform",
                typeof(Transform3D),
                typeof(Shape3D),
                new PropertyMetadata(
                    new ScaleTransform3D(100.0, 100.0, 100.0),
                    new PropertyChangedCallback(OnPropertyChangedAffectsModel)));

        public Transform3D MeshTransform
        {
            get { return (Transform3D)GetValue(MeshTransformProperty); }
            set { SetValue(MeshTransformProperty, value); }
        }

        protected abstract MeshGeometry3D GetGeometry();

        /// <summary>
        ///     Updates the model for this shape.
        /// </summary>
        /// <remarks>
        ///     The model is determined by what needs to be displayed.  If no
        ///     interactive visual needs to be displayed, then a simple
        ///     ModelVisual3D is used to display a GeometryModel3D.  If an
        ///     interactive visual needs to be displayed, then a
        ///     Viewport2DVisual3D is used.
        /// </remarks>
        protected override void OnUpdateModel()
        {
            MeshGeometry3D geometry = GetGeometry();
            for (int i = 0; i < geometry.Positions.Count; i++)
            {
                geometry.Positions[i] = MeshTransform.Transform(geometry.Positions[i]);
            }

            Size surfaceSize = geometry.CalculateIdealTextureSize();
            SurfaceWidth = surfaceSize.Width;
            SurfaceHeight = surfaceSize.Height;

            MeshGeometry3D frontGeometry = null;
            Shape3DMaterial frontMaterial = FrontMaterial;
            if (frontMaterial != null)
            {
                frontGeometry = geometry;
                if (frontGeometry == null)
                {
                    frontMaterial = null;
                }
            }

            MeshGeometry3D backGeometry = null;
            Shape3DMaterial backMaterial = BackMaterial;
            if (backMaterial != null)
            {
                backGeometry = CreateBackGeometry(geometry);
                if (backGeometry == null)
                {
                    backMaterial = null;
                }
            }


            EnsureChildren(frontMaterial, backMaterial);
            UpdateChild(_frontChild, frontGeometry, frontMaterial);
            UpdateChild(_backChild, backGeometry, backMaterial);
            return;
        }

        /// <summary>
        ///     Returns the number of children of this Visual3D.
        /// </summary>
        protected override sealed int Visual3DChildrenCount
        {
            get
            {
                int numChildren = 0;
                
                if (_frontChild != null)
                {
                    numChildren++;
                }

                if (_backChild != null)
                {
                    numChildren++;
                }

                return numChildren;
            }
        }

        /// <summary>
        ///     Returns the requested child identified by an index.
        /// </summary>
        /// <param name="index">
        ///     The index that identifies the child to return.
        /// </param>
        /// <returns>
        ///     The child Visual3D identified by the index.
        /// </returns>
        protected override sealed Visual3D GetVisual3DChild(int index)
        {
            if(index == 0)
            {
                if (_frontChild != null)
                {
                    return _frontChild;
                }
                else if (_backChild != null)
                {
                    return _backChild;
                }
            }
            else if (index == 1)
            {
                if (_frontChild != null && _backChild != null)
                {
                    return _backChild;
                }
            }

            throw new IndexOutOfRangeException("index");
        }

        public UIElement GrabFrontChildElement()
        {
            UIElement e = (UIElement)((Viewport2DVisual3D)_frontChild).Visual;
            ((Viewport2DVisual3D)_frontChild).Visual = null;
            return e;
        }

        // All derived classes can use this as the property change handler for
        // properties that affect the model.
        protected static void OnPropertyChangedAffectsModel(Object sender, DependencyPropertyChangedEventArgs e)
        {
            Shape3D _this = (Shape3D)sender;
            _this.InvalidateModel();
        }

        private void EnsureChildren(Shape3DMaterial frontMaterial, Shape3DMaterial backMaterial)
        {
            Visual3D frontChild = EnsureChild(_frontChild, frontMaterial);
            if (frontChild != _frontChild)
            {
                if (_frontChild != null)
                {
                    RemoveVisual3DChild(_frontChild);
                }

                _frontChild = frontChild;

                if (_frontChild != null)
                {
                    AddVisual3DChild(_frontChild);
                }
            }

            Visual3D backChild = EnsureChild(_backChild, backMaterial);
            if (backChild != _backChild)
            {
                if (_backChild != null)
                {
                    RemoveVisual3DChild(_backChild);
                }

                _backChild = backChild;

                if (_backChild != null)
                {
                    AddVisual3DChild(_backChild);
                }
            }
        }

        private Visual3D EnsureChild(Visual3D currentChild, Shape3DMaterial material)
        {
            Visual3D newChild = null;

            if (material != null)
            {
                if (material.HasElement)
                {
                    // We need a Viewport2DVisual3D to display an element.
                    if(currentChild is Viewport2DVisual3D)
                    {
                        newChild = currentChild;
                    }
                    else
                    {
                        //Viewbox viewbox = new Viewbox();
                        //viewbox.StretchDirection = StretchDirection.Both;
                        //viewbox.Stretch = Stretch.Fill;

                        Border border = new Border();
                        border.UseLayoutRounding = true;
                        border.Background = Brushes.Green;
                        //border.Child = viewbox;

                        Viewport2DVisual3D viewport = new Viewport2DVisual3D();
                        viewport.Visual = border;

                        newChild = viewport;
                    }

                    // Set the appropriate caching strategy.
                    CacheScale cacheScale = CacheScale;
                    if (cacheScale == null)
                    {
                        // Remove any VisualBrush caching.
                        RenderOptions.SetCachingHint(newChild, CachingHint.Unspecified);

                        // Remove any BitmapCache.
                        ((Viewport2DVisual3D)newChild).CacheMode = null;
                    }
                    else if (cacheScale.IsAuto)
                    {
                        // Remove any BitmapCache.
                        ((Viewport2DVisual3D)newChild).CacheMode = null;

                        // Specify VisualBrush caching with 2x min and max
                        // thresholds.
                        RenderOptions.SetCachingHint(newChild, CachingHint.Cache);
                        RenderOptions.SetCacheInvalidationThresholdMinimum(newChild, 0.5);
                        RenderOptions.SetCacheInvalidationThresholdMaximum(newChild, 2.0);
                    }
                    else
                    {
                        // Remove any VisualBrush caching.
                        RenderOptions.SetCachingHint(newChild, CachingHint.Unspecified);

                        // Set a BitmapCache with the appropriate scale.
                        BitmapCache bitmapCache = ((Viewport2DVisual3D)newChild).CacheMode as BitmapCache;
                        if (bitmapCache == null)
                        {
                            ((Viewport2DVisual3D)newChild).CacheMode = new BitmapCache(cacheScale.Scale);
                        }
                        else
                        {
                            bitmapCache.RenderAtScale = cacheScale.Scale;
                        }
                    }
                }
                else
                {
                    Debug.Assert(material.HasMaterial);

                    // We need a ModelVisual3D to display the material.
                    if(currentChild is ModelVisual3D)
                    {
                        Debug.Assert(((ModelVisual3D)currentChild).Content is GeometryModel3D);
                        newChild = currentChild;
                    }
                    else
                    {
                        newChild = new ModelVisual3D();
                        ((ModelVisual3D)newChild).Content = new GeometryModel3D();
                    }
                }
            }

            return newChild;
        }

        private void UpdateChild(Visual3D child, MeshGeometry3D geometry, Shape3DMaterial material)
        {
            if(material != null)
            {
                if(material.HasElement)
                {
                    Viewport2DVisual3D viewport = (Viewport2DVisual3D) child;
                    viewport.Geometry = geometry;
                    viewport.Material = HostingMaterial;
                    
                    // Set the size of the root element to the surface size.
                    Border border = (Border) viewport.Visual;
                    border.Width = SurfaceWidth;
                    border.Height = SurfaceHeight;
                    border.Child = material.GetElement(this);

                    // Add the material.
                    //Viewbox viewbox = (Viewbox)border.Child;
                    //viewbox.Width = SurfaceWidth;
                    //viewbox.Height = SurfaceHeight;
                    //viewbox.Child = material.GetElement(this);

                    // Set the mesh into the visualizer.
                    //viewbox.Child = new MeshTextureCoordinateVisualizer();
                    //((MeshTextureCoordinateVisualizer)viewbox.Child).Mesh = geometry;
                }
                else
                {
                    Debug.Assert(material.HasMaterial);
                
                    GeometryModel3D model = (GeometryModel3D)((ModelVisual3D)child).Content;
                    model.Geometry = geometry;
                    model.Material = material.GetMaterial(this);
                }
            }
        }

        private DiffuseMaterial HostingMaterial
        {
            get
            {
                if (_hostingMaterial == null)
                {
                    _hostingMaterial = new DiffuseMaterial();
                    _hostingMaterial.Color = Colors.White;
                    Viewport2DVisual3D.SetIsVisualHostMaterial(_hostingMaterial, true);

                    _hostingMaterial.Freeze();
                }

                return _hostingMaterial;
            }
        }

        private static MeshGeometry3D CreateBackGeometry(MeshGeometry3D frontMesh)
        {
            if (frontMesh == null)
            {
                return null;
            }

            MeshGeometry3D backMesh = new MeshGeometry3D();

            // Simply share the same (frozen) collections for positions and
            // texture coordinates.
            backMesh.Positions = frontMesh.Positions;
            backMesh.TextureCoordinates = frontMesh.TextureCoordinates;

            // Make a copy of the triangle indices and wind them backwards.
            backMesh.TriangleIndices = new Int32Collection(frontMesh.TriangleIndices);
            int numTriangles = backMesh.TriangleIndices.Count/3;
            for (int iTriangle = 0; iTriangle < numTriangles; iTriangle++)
            {
                int temp = backMesh.TriangleIndices[iTriangle * 3];
                backMesh.TriangleIndices[iTriangle * 3] = backMesh.TriangleIndices[iTriangle * 3 + 2];
                backMesh.TriangleIndices[iTriangle * 3 + 2] = temp;
            }

            // Make a copy of the normals and reverse their direction.
            backMesh.Normals = new Vector3DCollection(frontMesh.Normals);
            int numNormals = backMesh.Normals.Count;
            for (int iNormal = 0; iNormal < numNormals; iNormal++)
            {
                backMesh.Normals[iNormal] *= -1.0;
            }

            backMesh.Freeze();
            return backMesh; 
        }

        private Visual3D _frontChild;
        private Visual3D _backChild;
        private DiffuseMaterial _hostingMaterial;
    }
}
