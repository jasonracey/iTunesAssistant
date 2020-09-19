using System.Collections.Generic;
using System.Linq;
using iTunesLib;

namespace iTunesAssistantLib
{
    public class WorkflowOrchestrator
    {
        private readonly IAppClassWrapper _appClassWrapper;

        IWorkflowRunner _mergeAlbumsWorkflowRunner;
        IWorkflowRunner _importTrackNamesWorkflowRunner;
        IWorkflowRunner _albumWorkflowRunner;
        IWorkflowRunner _trackWorkflowRunner;

        private readonly Status _status;

        public WorkflowOrchestrator(
            IAppClassWrapper? appClassWrapper = null,
            IWorkflowRunner? mergeAlbumsWorkflowRunner = null,
            IWorkflowRunner? importTrackNamesWorkflowRunner = null,
            IWorkflowRunner? albumWorkflowRunner = null,
            IWorkflowRunner? trackWorkflowRunner = null)
        {
            _appClassWrapper = appClassWrapper ?? new AppClassWrapper();

            _mergeAlbumsWorkflowRunner = mergeAlbumsWorkflowRunner ?? new MergeAlbumsWorkflowRunner();
            _importTrackNamesWorkflowRunner = importTrackNamesWorkflowRunner ?? new ImportTrackNamesWorkflowRunner();
            _albumWorkflowRunner = albumWorkflowRunner ?? new AlbumWorkflowRunner();
            _trackWorkflowRunner = trackWorkflowRunner ?? new TrackWorkflowRunner();

            _status = new Status();
        }

        public string? Message 
        { 
            get { return _status.Message; }
        }

        public int ItemsProcessed
        {
            get { return _status.ItemsProcessed; }
        }

        public int ItemsTotal
        {
            get { return _status.ItemsTotal; }
        }

        public void Run(IEnumerable<Workflow> workflows)
        {
            if (workflows?.Count() == 0)
            {
                return;
            }

            _status.Update(0, _appClassWrapper.LibraryPlaylist.Tracks.Count, "Loading selected tracks...");

            if (_appClassWrapper.SelectedTracks == null)
            {
                return;
            }

            var tracksToFix = new List<IITTrack>();

            foreach (IITTrack? track in _appClassWrapper.SelectedTracks)
            {
                if (track != null)
                {
                    tracksToFix.Add(track);
                    _status.ItemsProcessed++;
                }
            }

            if (tracksToFix.Count == 0)
            {
                return;
            }

            if (workflows.Any(workflow => workflow.Name == WorkflowName.MergeAlbums))
            {
                _mergeAlbumsWorkflowRunner.Run(_status, tracksToFix);
            }

            var inputFilePath = workflows
                .FirstOrDefault(workflow => workflow.Name == WorkflowName.ImportTrackNames)?
                .FileName;

            if (!string.IsNullOrWhiteSpace(inputFilePath))
            {
                _importTrackNamesWorkflowRunner.Run(_status, tracksToFix, inputFilePath: inputFilePath);
            }

            var albumWorkflows = workflows.Where(workflow => workflow.Type == WorkflowType.Album);

            if (albumWorkflows.Any())
            {
                _albumWorkflowRunner.Run(_status, tracksToFix, albumWorkflows);
            }

            var trackWorkflows = workflows.Where(workflow => workflow.Type == WorkflowType.Track);

            if (trackWorkflows.Any())
            {
                _trackWorkflowRunner.Run(_status, tracksToFix, trackWorkflows);
            }
        }
    }
}
