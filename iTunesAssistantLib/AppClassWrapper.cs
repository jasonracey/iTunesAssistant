using iTunesLib;

namespace iTunesAssistantLib
{
    public class AppClassWrapper : IAppClassWrapper
    {
        private readonly iTunesAppClass _app;

        public IITLibraryPlaylist LibraryPlaylist => _app.LibraryPlaylist;
        public IITTrackCollection SelectedTracks => _app.SelectedTracks;

        public AppClassWrapper()
        {
            _app = new iTunesAppClass();
        }
    }
}
