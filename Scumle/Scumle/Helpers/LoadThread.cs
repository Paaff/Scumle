using Scumle.Model;
using Scumle.Model.Shapes;
using Scumle.UndeRedo;
using Scumle.UndeRedo.Commands;
using Scumle.ViewModel;
using Scumle.ViewModel.Shapes;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Windows;

namespace Scumle.Helpers
{
    /// <summary>
    /// Thread that loads saved files from XML in a new thread
    /// </summary>
    class LoadThread : BaseThread
    {
        private ObservableCollection<ILine> lines;
        private List<ModelBase> loadedModelsList;
        private ObservableCollection<IShape> shapes;
        private string path;
        private double _loadingOffSet = 5;

        public LoadThread(ObservableCollection<IShape> shapes, ObservableCollection<ILine> lines, string v)
        {
            this.shapes = shapes;
            this.lines = lines;
            this.path = v;
        }

        protected override void RunThread()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                loadedModelsList = GenericSerializer.convertFromXML<List<ModelBase>>(path);
                loading(loadedModelsList);
            });
        
        }

        private void loading(List<ModelBase> loadedModelsList)
        {
            IList<ILine> linesAdd = new List<ILine>();
            IList<IShape> shapesAdd = new List<IShape>();
            foreach (var loadedModel in loadedModelsList)
            {

                if (loadedModel is UMLClass)
                {
                    var actualUMLClass = loadedModel as UMLClass;
                    var storedColor = Color.FromRgb(actualUMLClass.ColorR, actualUMLClass.ColorG, actualUMLClass.ColorB);

                    IShape actualViewModel = new UMLClassViewModel(new UMLClass(actualUMLClass.X + _loadingOffSet, actualUMLClass.Y + _loadingOffSet, actualUMLClass.Width, actualUMLClass.Height,
                                                                           actualUMLClass.Name, storedColor, actualUMLClass.ID, actualUMLClass.UMLFields, actualUMLClass.UMLMethods));

                    shapesAdd.Add(actualViewModel);
                }
                else if (loadedModel is BasicShape)
                {
                    var actualBasicShape = loadedModel as BasicShape;
                    var storedColor = Color.FromRgb(actualBasicShape.ColorR, actualBasicShape.ColorG, actualBasicShape.ColorB);


                    IShape actualViewModel = new BasicShapeViewModel(new BasicShape(actualBasicShape.Type, actualBasicShape.X + _loadingOffSet, actualBasicShape.Y + _loadingOffSet,
                                                                           actualBasicShape.Width, actualBasicShape.Height, storedColor, actualBasicShape.ID));
                    shapesAdd.Add(actualViewModel);

                }
                else if (loadedModel is Line)
                {

                    var actualLine = loadedModel as Line;
                    string fromID = actualLine.StoreFromId;
                    string toID = actualLine.StoreToId;
                    IPoint cpFrom = null;
                    IPoint cpTo = null;

                    foreach (var viewModel in shapesAdd)
                    {
                        var actualViewModel = viewModel as IShape;

                        if (actualViewModel.ID == fromID)
                        {

                            cpFrom = actualViewModel.ConnectionPoints.ElementAt(0);
                        }

                        if (actualViewModel.ID == toID)
                        {

                            cpTo = actualViewModel.ConnectionPoints.ElementAt(0);

                        }
                    }

                    if (cpFrom != null || cpTo != null) { linesAdd.Add(new LineViewModel(new Line(actualLine.Type, cpFrom, cpTo))); }

                }

            }

            new MultiUndoRedoCommand(new List<UndoRedoCommand>() {
                new ShapeAddCommand(shapes, shapesAdd),
                new LineAddCommand(lines, linesAdd)
            }).Execute();
        }
    }
}
