using iTunesLib;

namespace iTunesAssistantLib
{
    public interface IAppClassWrapper
    {
        IITLibraryPlaylist LibraryPlaylist { get; }
        IITTrackCollection SelectedTracks { get; }
    }
}