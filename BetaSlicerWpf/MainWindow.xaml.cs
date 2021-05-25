using BetaSlicerCommon;
using BetaSlicerCommon.WPF;
using QuantumConcepts.Formats.StereoLithography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BetaSlicerWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //KeyboardCamera camera;
        MouseOrbitCamera mouseCamera;
        DeltaTime deltaTime;

        private void SetupView()
        {
            // Declare scene objects.
            Viewport3D viewport3D = new Viewport3D();
            Model3DGroup model3DGroup = new Model3DGroup();
            GeometryModel3D geometryModel = new GeometryModel3D();
            ModelVisual3D modelVisual3D = new ModelVisual3D();
            // Defines the camera used to view the 3D object. In order to view the 3D object,
            // the camera must be positioned and pointed such that the object is within view
            // of the camera.

            SetupLighting(model3DGroup);

            // Apply the mesh to the geometry model.
            // geometryModel.Geometry = GetExampleGeometry();
            // geometryModel.Geometry = GetStlGeometry(@"e:\Downloads\_3D Print Models\Butterfly\files\Articulated_Butterfly.stl");
            // geometryModel.Geometry = GetStlGeometry(@"e:\Downloads\_3D Print Models\Gear_Bearing\bearing5.stl");
            geometryModel.Geometry = GetStlGeometry(@"e:\reposNew\BetaSlicer\BetaSlicer\TestStl\TestPart2.stl");

            geometryModel.Material = GetDefaultMaterial();
            geometryModel.BackMaterial = GetDefaultMaterial();
            //geometryModel.Transform = GetExampleTransform();

            // Add the geometry model to the model group.
            model3DGroup.Children.Add(geometryModel);

            // printer Bed
            GeometryModel3D printerBedGeometry = new GeometryModel3D();
            printerBedGeometry.Geometry = MeshGeometryHelper.CreatePrinterBed(100, 100);
            printerBedGeometry.Material = GetDefaultMaterial();
            //printerBedGeometry.BackMaterial = GetDefaultMaterial();
            model3DGroup.Children.Add(printerBedGeometry);

            // Add the group of models to the ModelVisual3d.
            modelVisual3D.Content = model3DGroup;

            SetupCamera(viewport3D, geometryModel.Geometry);

            //
            viewport3D.Children.Add(modelVisual3D);

            // Apply the viewport to the page so it will be rendered.
            this.Content = viewport3D;

            deltaTime = new DeltaTime();
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        private Transform3D GetExampleTransform()
        {
            // Apply a transform to the object. In this sample, a rotation transform is applied,
            // rendering the 3D object rotated.
            RotateTransform3D myRotateTransform3D = new RotateTransform3D();
            AxisAngleRotation3D myAxisAngleRotation3d = new AxisAngleRotation3D();
            myAxisAngleRotation3d.Axis = new Vector3D(0, 3, 0);
            myAxisAngleRotation3d.Angle = 40;
            myRotateTransform3D.Rotation = myAxisAngleRotation3d;

            return myRotateTransform3D;
        }

        DiffuseMaterial GetDefaultMaterial()
        {
            // The material specifies the material applied to the 3D object. In this sample a
            // linear gradient covers the surface of the 3D object.

            // Create a horizontal linear gradient with four stops.
            LinearGradientBrush myHorizontalGradient = new LinearGradientBrush();
            myHorizontalGradient.StartPoint = new Point(0, 0.5);
            myHorizontalGradient.EndPoint = new Point(1, 0.5);
            myHorizontalGradient.GradientStops.Add(new GradientStop(Colors.Yellow, 0.0));
            myHorizontalGradient.GradientStops.Add(new GradientStop(Colors.Red, 0.25));
            myHorizontalGradient.GradientStops.Add(new GradientStop(Colors.Blue, 0.75));
            myHorizontalGradient.GradientStops.Add(new GradientStop(Colors.LimeGreen, 1.0));

            // Define material and apply to the mesh geometries.
            DiffuseMaterial myMaterial = new DiffuseMaterial(myHorizontalGradient);

            return myMaterial;
        }


        private void SetupLighting(Model3DGroup model3DGroup)
        {
            // Define the lights cast in the scene. Without light, the 3D object cannot
            // be seen. Note: to illuminate an object from additional directions, create
            // additional lights.
            DirectionalLight myDirectionalLight = CreateLight(new Vector3D(-0.61, -0.5, -0.61));
            model3DGroup.Children.Add(myDirectionalLight);

            DirectionalLight myDirectionalLight2 = CreateLight(new Vector3D(0.61, -0.5, -0.61));
            model3DGroup.Children.Add(myDirectionalLight2);

            DirectionalLight myDirectionalLight3 = CreateLight(new Vector3D(0.61, -0.5, 0.61));
            model3DGroup.Children.Add(myDirectionalLight3);
        }

        private static DirectionalLight CreateLight(Vector3D direction)
        {
            DirectionalLight myDirectionalLight = new DirectionalLight();
            myDirectionalLight.Color = Colors.White;
            myDirectionalLight.Direction = direction;
            return myDirectionalLight;
        }

        private void SetupCamera(Viewport3D viewport, Geometry3D geometry)
        {
            //camera = new KeyboardCamera();
            mouseCamera = new MouseOrbitCamera(this);

            Vector3D lookAt = MeshGeometryHelper.GetCenter(geometry);

            double zDistance = 10 * geometry.Bounds.Size.Z;

            // Screen far ==> negative axis
            // Screen close ==> positive axis

            // Zoom out in z direction by ZDistance amount
            Vector3D cameraPosition = new Vector3D(0, 0, 1) * zDistance - lookAt;

            // Zoom out by each axis to get a nice diagonal view
            //Vector3D cameraPosition = new Vector3D(-1, -1, 1) * zDistance - lookAt;
            
            Vector3D lookDirection = lookAt - cameraPosition;
            lookDirection.Normalize();

            Vector3D up = Vector3D.CrossProduct(lookDirection, new Vector3D(-1, 0, 0));

            mouseCamera.perspectiveCamera.LookDirection = lookDirection;
            mouseCamera.perspectiveCamera.Position = (Point3D)cameraPosition;
            mouseCamera.perspectiveCamera.UpDirection = up;

            // Asign the camera to the viewport
            viewport.Camera = mouseCamera.perspectiveCamera;
        }

        private MeshGeometry3D GetStlGeometry(string fileName)
        {
            IEnumerable<Facet> facets = StlFacetProvider.ReadFacets(fileName);
            MeshGeometry3D meshGeometry = MeshGeometryHelper.CreateFromFacets(facets);

            return meshGeometry;
        }

        private MeshGeometry3D GetExampleGeometry()
        {

            // The geometry specifes the shape of the 3D plane. In this sample, a flat sheet
            // is created.
            MeshGeometry3D myMeshGeometry3D = new MeshGeometry3D();

            // Create a collection of normal vectors for the MeshGeometry3D.
            Vector3DCollection myNormalCollection = new Vector3DCollection();
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myMeshGeometry3D.Normals = myNormalCollection;

            // Create a collection of vertex positions for the MeshGeometry3D.
            Point3DCollection myPositionCollection = new Point3DCollection();
            myPositionCollection.Add(new Point3D(-0.5, -0.5, 0.5));
            myPositionCollection.Add(new Point3D(0.5, -0.5, 0.5));
            myPositionCollection.Add(new Point3D(0.5, 0.5, 0.5));
            //myPositionCollection.Add(new Point3D(0.5, 0.5, 0.5));
            myPositionCollection.Add(new Point3D(-0.5, 0.5, 0.5));
            //myPositionCollection.Add(new Point3D(-0.5, -0.5, 0.5));
            myMeshGeometry3D.Positions = myPositionCollection;

            // Create a collection of texture coordinates for the MeshGeometry3D.
            PointCollection myTextureCoordinatesCollection = new PointCollection();
            myTextureCoordinatesCollection.Add(new Point(0, 0));
            myTextureCoordinatesCollection.Add(new Point(1, 0));
            myTextureCoordinatesCollection.Add(new Point(1, 1));
            //myTextureCoordinatesCollection.Add(new Point(1, 1));
            myTextureCoordinatesCollection.Add(new Point(0, 1));
            //myTextureCoordinatesCollection.Add(new Point(0, 0));
            myMeshGeometry3D.TextureCoordinates = myTextureCoordinatesCollection;

            // Create a collection of triangle indices for the MeshGeometry3D.
            Int32Collection myTriangleIndicesCollection = new Int32Collection();
            myTriangleIndicesCollection.Add(0);
            myTriangleIndicesCollection.Add(1);
            myTriangleIndicesCollection.Add(2);
            myTriangleIndicesCollection.Add(0);
            myTriangleIndicesCollection.Add(2);
            myTriangleIndicesCollection.Add(3);
            myMeshGeometry3D.TriangleIndices = myTriangleIndicesCollection;

            return myMeshGeometry3D;
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            deltaTime.Update();

            mouseCamera.Update(deltaTime);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //camera.Update(e.Key);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetupView();
        }
    }
}
