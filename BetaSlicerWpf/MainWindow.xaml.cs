using BetaSlicerCommon;
using BetaSlicerCommon.WPF;
using QuantumConcepts.Formats.StereoLithography;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        Color frontBedColor = Color.FromArgb(255, 250, 250, 224);
        Color backBedColor = Color.FromArgb(100, 250, 250, 224);

        DiffuseMaterial normalModelMaterial;
        DiffuseMaterial transparentModelMaterial;

        Model3DGroup model3DGroup = new Model3DGroup();


        public bool IsTransparent { get; set; }

        private void SetupView()
        {
            // Declare scene objects.
            Viewport3D viewport3D = new Viewport3D();
            ModelVisual3D modelVisual3D = new ModelVisual3D();
            // Defines the camera used to view the 3D object. In order to view the 3D object,
            // the camera must be positioned and pointed such that the object is within view
            // of the camera.

            SetupLighting(model3DGroup);
            SetupMaterials();

            GenerateModels();

            // Add the group of models to the ModelVisual3d.
            modelVisual3D.Content = model3DGroup;

            SetupCamera(viewport3D);

            //
            viewport3D.Children.Add(modelVisual3D);

            // Apply the viewport to the page so it will be rendered.
            dockPanel.Children.Add(viewport3D);

            deltaTime = new DeltaTime();
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        private void GenerateModels()
        {
            string stlPath = @"..\..\..\..\TestStl\";
            GeometryModel3D geometryModel = new GeometryModel3D();
            //geometryModel.Geometry = GetStlGeometry(System.IO.Path.Combine(stlPath, "TestPart2.stl"));
            geometryModel.Geometry = GetStlGeometry(System.IO.Path.Combine(stlPath, "bearing5.stl"));

            geometryModel.Material = normalModelMaterial;
            geometryModel.BackMaterial = GetDefaultMaterial();
            //geometryModel.Transform = GetExampleTransform();

            // Add the geometry model to the model group.
            model3DGroup.Children.Add(geometryModel);

            // printer Bed
            GeometryModel3D printerBedGeometry = new GeometryModel3D();
            printerBedGeometry.Geometry = MeshGeometryHelper.CreatePrinterBed(100, 100);
            printerBedGeometry.Material = GetDiffuseMaterial(frontBedColor);
            printerBedGeometry.BackMaterial = GetDiffuseMaterial(backBedColor);

            model3DGroup.Children.Add(printerBedGeometry);
   
        }

        private void SetupMaterials()
        {
            Color modelColor = Color.FromArgb(255, 100, 100, 250);
            Color transparentModelColor = Color.FromArgb(100, 100, 100, 250);

            normalModelMaterial = GetDiffuseMaterial(modelColor);
            transparentModelMaterial = GetDiffuseMaterial(transparentModelColor);
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

        DiffuseMaterial GetDiffuseMaterial(Color color)
        {
            DiffuseMaterial material = new DiffuseMaterial(new SolidColorBrush(color));
            return material;
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
            AmbientLight ambient = new AmbientLight(Colors.White);
            ambient.Color = Color.FromArgb(255, 50, 50, 50);
            model3DGroup.Children.Add(ambient);
            //return;
            // Define the lights cast in the scene. Without light, the 3D object cannot
            // be seen. Note: to illuminate an object from additional directions, create
            // additional lights.

            model3DGroup.Children.Add(CreateLight(Color.FromArgb(255, 150, 150, 150), new Vector3D(-1, -1, -0.5)));
            model3DGroup.Children.Add(CreateLight(Color.FromArgb(255, 150, 150, 150), new Vector3D(1, 1, 0.5)));

            model3DGroup.Children.Add(CreateLight(Color.FromArgb(255, 150, 150, 150), new Vector3D(0, 0, -1)));

            //DirectionalLight myDirectionalLight2 = CreateLight(new Vector3D(0.61, -0.5, -0.61));
            //model3DGroup.Children.Add(myDirectionalLight2);

            //DirectionalLight myDirectionalLight3 = CreateLight(new Vector3D(0.61, -0.5, 0.61));
            //model3DGroup.Children.Add(myDirectionalLight3);
        }

        private static DirectionalLight CreateLight(Color color, Vector3D direction)
        {
            DirectionalLight myDirectionalLight = new DirectionalLight();
            myDirectionalLight.Color = color;
            myDirectionalLight.Direction = direction;
            return myDirectionalLight;
        }

        private void SetupCamera(Viewport3D viewport)
        {

            //camera = new KeyboardCamera();
            mouseCamera = new MouseOrbitCamera(this);
            mouseCamera.XAngle = 50;
            mouseCamera.Zoom = 400;

            // Asign the camera to the viewport
            viewport.Camera = mouseCamera.perspectiveCamera;
        }

        private MeshGeometry3D GetStlGeometry(string fileName)
        {
            IEnumerable<Facet> facets = StlFacetProvider.ReadFacets(fileName);
            MeshGeometry3D meshGeometry = MeshGeometryHelper.CreateFromFacets(facets);
            //MeshGeometry3D meshGeometry = MeshGeometryHelper.CreateFromFacetsCached(facets);



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

            mouseCamera?.Update(deltaTime);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //camera.Update(e.Key);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetupView();
        }

        private void mnuFileExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void mnuFileImport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cbTransparentOutside_Click(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine(IsTransparent);
            DiffuseMaterial material;
            if (cbTransparentOutside.IsChecked == true)
            {
                material = transparentModelMaterial;
            }
            else
            {
                material = normalModelMaterial;
            }

            foreach (Model3D model in model3DGroup.Children)
            {
                GeometryModel3D geomModel = model as GeometryModel3D;
                if (geomModel == null)
                    continue;

                geomModel.Material = material;
                break; // Criminal hack to avoid changing the texture of the print bed
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SlicePreviewButton_Click(object sender, RoutedEventArgs e)
        {
            model3DGroup.Children.Clear();
            List<Point3D> points = new List<Point3D>();
            points.Add(new Point3D(5, 0, 0));
            points.Add(new Point3D(15, 0, 0));
            points.Add(new Point3D(15, 10, 0));
            points.Add(new Point3D(5, 20, 0));
            points.Add(new Point3D(40, 0, 0));
            points.Add(new Point3D(5, 50, 0));
            points.Add(new Point3D(105, 50, 0));

            IEnumerable<MeshGeometry3D> rectangles = GeometryHelper.CreateRectangle(points);
            foreach (MeshGeometry3D mesh in rectangles)
            {
                GeometryModel3D geom = new GeometryModel3D();
                geom.Material = GetDiffuseMaterial(Colors.Purple);
                geom.BackMaterial = GetDiffuseMaterial(Colors.Cyan);
                geom.Geometry = mesh;
                model3DGroup.Children.Add(geom);
            }
            SetupLighting(model3DGroup);
        }

        private void btnNormalView_Click(object sender, RoutedEventArgs e)
        {
            model3DGroup.Children.Clear();
            GenerateModels();
            SetupLighting(model3DGroup);
        }
    }
}
