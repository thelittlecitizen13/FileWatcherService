using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FileWatcherService
{
    class FileWatcherSystem
    {
        private string _monitoredPath;
        private ILogger _logger;
        private FileSystemWatcher _fileSystemWatcher;
        

        public FileWatcherSystem(ILogger logger, string pathToWatch)
        {
            _logger = logger;
            _monitoredPath = pathToWatch;
            _fileSystemWatcher = new FileSystemWatcher();
        }
        public void MonitorDirectory()
        {

            _fileSystemWatcher.Path = _monitoredPath;
            _fileSystemWatcher.Created += FileSystemWatcher_Created;
            _fileSystemWatcher.Renamed += FileSystemWatcher_Renamed;
            _fileSystemWatcher.Deleted += FileSystemWatcher_Deleted;
            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            _logger.LogInformation("File created: {0}", e.Name);
        }

        private void FileSystemWatcher_Renamed(object sender, FileSystemEventArgs e)
        {
            _logger.LogInformation("File renamed: {0}", e.Name);
        }

        private void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)

        {

            _logger.LogInformation("File deleted: {0}", e.Name);

        }

    }

}

