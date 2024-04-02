using System;
using System.Collections.Generic;

namespace Client.ControllerContainer
{
    public class ControllerGroup
    {
        private List<IInitController> _startControllers = new();
        private List<IRunController> _runControllers = new();
        private List<IDisposable> _disposables = new();

        public ControllerGroup Add<T>(T controller)
        {
            if (controller is IInitController startController)
                _startControllers.Add(startController);

            if (controller is IRunController runController)
                _runControllers.Add(runController);

            if (controller is IDisposable disposableController)
                _disposables.Add(disposableController);

            return this;
        }

        public void Init()
        {
            foreach (var startController in _startControllers)
            {
                startController.Init();
            }
        }

        public void Run()
        {
            foreach (var runController in _runControllers)
            {
                runController.Run();
            }
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }

            _startControllers.Clear();
            _runControllers.Clear();
        }
    }
}