using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scumle.Model;
using Scumle.ViewModel;
using Scumle.ViewModel.Shapes;
using System.Threading;
using System.Windows;

namespace Scumle.Helpers
{
    /// <summary>
    /// Thread that saves the diagram to a file in a new thread
    /// </summary>
    class SaveThread : BaseThread
    {
        private ObservableCollection<ILine> lines;
        private ObservableCollection<IShape> shapes;
        private string path;

        public SaveThread(ObservableCollection<IShape> shapes, ObservableCollection<ILine> lines, string _currentFilePath)
        {
            this.shapes = shapes;
            this.lines = lines;
            this.path = _currentFilePath;
        }

        protected override void RunThread()
        {            
            Application.Current.Dispatcher.Invoke(() =>
            {
                 GenericSerializer.convertToXML(saving(shapes, lines), path);
            });
           
        }

        public List<ModelBase> saving(ObservableCollection<IShape> shapesToSave, ObservableCollection<ILine> linesToSave)
        {
            // List containing all models to be saved.
            List<ModelBase> modelsToSave = new List<ModelBase>();

            foreach (var ViewModel in shapesToSave)
            {
                if (ViewModel is UMLClassViewModel)
                {
                    var actualViewModel = ViewModel as UMLClassViewModel;
                    modelsToSave.Add(actualViewModel.Shape);
                }
                else if (ViewModel is BasicShapeViewModel)
                {
                    var actualViewModel = ViewModel as BasicShapeViewModel;
                    modelsToSave.Add(actualViewModel.Shape);
                }
            }

            foreach (var viewModel in linesToSave)
            {
                var actualModel = (viewModel as LineViewModel).Model as Line;
                modelsToSave.Add(actualModel);
            }
            return modelsToSave;

        }
    }
}
