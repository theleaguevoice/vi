using System;
using System.IO;
using Core.Static;

namespace Core.Abstraction
{
    public abstract class TextFileWatcher<T> : IProcess
    {
        private string _watchPath;
        private T _lastState;
        private readonly IFileHandler<T> _fileHandler;
        private FileSystemWatcher _watcher = null;
        public bool IsRunning { get; set; }

        public TextFileWatcher(string watchPath, IFileHandler<T> fileHandler)
        {
            _watchPath = watchPath;
            _fileHandler = fileHandler;
            IsRunning = false;
        }

        public TextFileWatcher(IFileHandler<T> fileHandler)
        {
            _fileHandler = fileHandler;
        }

        private void ConfigureWatcher()
        {
            if (_watcher != null)
                return;

            if (string.IsNullOrWhiteSpace(_watchPath))
                return;

            _watcher = new FileSystemWatcher(_watchPath)
            {
                NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName |
                               NotifyFilters.DirectoryName,
                Filter = "lockfile"
            };


            _watcher.Created += OnFileChanged;
            _watcher.Changed += OnFileChanged;
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            Utils.ReadFile(e.FullPath, out var file);
            DecodeInput(file);
        }

        private void DecodeInput(string input)
        {
            try
            {
                var decodedFile = _fileHandler.Decode(input);

                if (Equals(decodedFile.ToString(), _lastState.ToString())) return;

                _lastState = decodedFile;
                OnFileChanged(decodedFile);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        protected abstract void OnFileChanged(T file);

        public void SetPath(string newPath)
        {
            _watchPath = newPath;
            if (_watcher == null)
                ConfigureWatcher();
            else
                _watcher.Path = _watchPath;
        }

        public void Start()
        {
            var content = GetContent(true);
            if (content != null)
                OnFileChanged(content);

            ConfigureWatcher();
            _watcher.EnableRaisingEvents = true;
            IsRunning = true;
        }

        public T GetContent(bool fresh)
        {
            if (!fresh)
                return _lastState;

            var lockfilePath = Path.Combine(_watchPath, "lockfile");
            Utils.ReadFile(lockfilePath, out var file);
            var decodedFile = _fileHandler.Decode(file);
            _lastState = decodedFile;
            return decodedFile;
        }


        public void Stop()
        {
            IsRunning = false;
            _watcher.Dispose();
        }
    }
}