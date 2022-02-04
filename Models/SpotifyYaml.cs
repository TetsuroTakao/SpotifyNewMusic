using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyNewMusic.Models
{
    public class SpotifyYaml
    {
        public Album album { get; set; }
        public AlbumSimple albumsimple { get; set; }
        public TypedPageProperties albumsimplepage { get; set; }
        public AlbumTrackPage albumtrackpage { get; set; }
        public Artist artist { get; set; }
        public ArtistSimple artistsimple { get; set; }
        public Category category { get; set; }
        public TypedPageProperties categorypage { get; set; }
        public CurrentUserProfile currentuserprofile { get; set; }
        public FeaturedPlaylists featuredplaylists { get; set; }
        public string followers { get; set; }
        public Image image { get; set; }
        public Playlist playlist { get; set; }
        public PlaylistSimple playlistsimple { get; set; }
        public TypedPageProperties playlistsimplepage { get; set; }
        public PlaylistSnapshot playlistsnapshot { get; set; }
        public PlaylistTrack playlisttrack { get; set; }
        public PlaylistTrackPage playlisttrackpage { get; set; }
        public SavedTrack savedtrack { get; set; }
        public TypedPageProperties savedtrackpage { get; set; }
        public Search search { get; set; }
        public Spotify_Type track { get; set; }
        public TrackSimple tracksimple { get; set; }
        public TypedPageProperties tracksimplepage { get; set; }
        public UserFollowed userfollowed { get; set; }
        public UserProfile userprofile { get; set; }
        public class Artist : StringAdditionalproperties
        {
            public ArtistProperties properties { get; set; }
        }
        public class Album : StringAdditionalproperties
        {
            public AlbumProperties properties { get; set; }
        }
    }
    public class AlbumProperties
    {
        public Spotify_Type album_type { get; set; }
        public DescriptedItems artists { get; set; }
        public DescriptedItems available_markets { get; set; }
        public Copyrights copyrights { get; set; }
        public DescribedAdditionalProperties external_ids { get; set; }
        public DescribedAdditionalProperties external_urls { get; set; }
        public Genres genres { get; set; }
        public Spotify_Type href { get; set; }
        public Spotify_Type id { get; set; }
        public DescriptedItems images { get; set; }
        public Spotify_Type name { get; set; }
        public Spotify_Type popularity { get; set; }
        public Spotify_Type release_date { get; set; }
        public Spotify_Type release_date_precision { get; set; }
        public string tracks { get; set; }
        public Spotify_Type type { get; set; }
        public Uri uri { get; set; }
    }
    public class Spotify_Type : StringAdditionalproperties
    {
        public string description { get; set; }
    }
    public class DescriptedItems : Spotify_Type
    {
        public string items { get; set; }
    }
    public class Copyrights : Spotify_Type
    {
        public CopyrightsItems items { get; set; }
    }
    public class CopyrightsItems : StringAdditionalproperties
    {
        public CopyrightsProperties properties { get; set; }
    }
    public class CopyrightsProperties
    {
        public Spotify_Type text { get; set; }
        public Spotify_Type type { get; set; }
    }
    public class DescribedAdditionalProperties : Spotify_Type
    {
        public Spotify_Type additionalProperties { get; set; }
    }
    public class Genres : Spotify_Type
    {
        public StringAdditionalproperties items { get; set; }
    }
    public class AlbumSimple : StringAdditionalproperties
    {
        public AlbumSimpleProperties properties { get; set; }
    }
    public class AlbumSimpleProperties
    {
        public Spotify_Type album_type { get; set; }
        public Spotify_Type available_markets { get; set; }
        public DescribedAdditionalProperties external_urls { get; set; }
        public Spotify_Type href { get; set; }
        public Spotify_Type id { get; set; }
        public DescriptedItems images { get; set; }
        public Spotify_Type name { get; set; }
        public Spotify_Type type { get; set; }
        public Spotify_Type uri { get; set; }
    }
    public class AlbumTrackPage : StringAdditionalproperties
    {
        public AlbumTrackProperties properties { get; set; }
    }
    public class AlbumTrackProperties : PageProperties
    {
        public new string items { get; set; }
    }
    public class ArtistProperties
    {
        public ArtistExternal_Urls external_urls { get; set; }
        public Followers followers { get; set; }
        public Genres genres { get; set; }
        public Spotify_Type href { get; set; }
        public Spotify_Type id { get; set; }
        public DescriptedItems images { get; set; }
        public Spotify_Type name { get; set; }
        public Spotify_Type popularity { get; set; }
        public Spotify_Type type { get; set; }
        public Spotify_Type uri { get; set; }
    }
    public class ArtistExternal_Urls : Spotify_Type
    {
        public StringAdditionalproperties additionalProperties { get; set; }
    }
    public class StringAdditionalproperties
    {
        public string type { get; set; }
    }
    public class ArtistSimple : StringAdditionalproperties
    {
        public ArtistSimpleProperties properties { get; set; }
    }
    public class ArtistSimpleProperties
    {
        public ArtistExternal_Urls external_urls { get; set; }
        public Spotify_Type href { get; set; }
        public Spotify_Type id { get; set; }
        public Spotify_Type name { get; set; }
        public Spotify_Type type { get; set; }
        public Spotify_Type uri { get; set; }
    }
    public class Category : StringAdditionalproperties
    {
        public CategoryProperties properties { get; set; }
    }
    public class CategoryProperties
    {
        public Spotify_Type href { get; set; }
        public Icons icons { get; set; }
        public Spotify_Type id { get; set; }
        public Spotify_Type name { get; set; }
    }
    public class Icons : StringAdditionalproperties
    {
        public string items { get; set; }
    }
    public class TypedPageProperties : StringAdditionalproperties
    {
        public PageProperties properties { get; set; }
    }
    public class CurrentUserProfile : StringAdditionalproperties
    {
        public CurrentUserProperties properties { get; set; }
    }
    public class CurrentUserProperties
    {
        public Spotify_Type birthdate { get; set; }
        public Spotify_Type country { get; set; }
        public Spotify_Type displayName { get; set; }
        public Spotify_Type email { get; set; }
        public ArtistExternal_Urls external_urls { get; set; }
        public string followers { get; set; }
        public Spotify_Type href { get; set; }
        public Spotify_Type id { get; set; }
        public Spotify_Type product { get; set; }
        public Spotify_Type type { get; set; }
        public Spotify_Type uri { get; set; }
    }
    public class FeaturedPlaylists : StringAdditionalproperties
    {
        public FeaturedPlaylistsProperties properties { get; set; }
    }
    public class FeaturedPlaylistsProperties
    {
        public Spotify_Type message { get; set; }
        public string playlists { get; set; }
    }
    public class Followers : Spotify_Type
    {
        public FollowersProperties properties { get; set; }
    }
    public class Image : StringAdditionalproperties
    {
        public ImageProperties properties { get; set; }
    }
    public class ImageProperties
    {
        public Spotify_Type height { get; set; }
        public Spotify_Type url { get; set; }
        public Spotify_Type width { get; set; }
    }
    public class Playlist : StringAdditionalproperties
    {
        public PlaylistProperties properties { get; set; }
    }
    public class PlaylistProperties : PlaylistSimpleProperties
    {
        public Spotify_Type description { get; set; }
        public Followers followers { get; set; }
    }
    public class PlaylistSimple : StringAdditionalproperties
    {
        public PlaylistSimpleProperties properties { get; set; }
    }
    public class PlaylistSimpleProperties
    {
        public Spotify_Type collaborative { get; set; }
        public DescribedAdditionalProperties external_urls { get; set; }
        public Spotify_Type href { get; set; }
        public Spotify_Type id { get; set; }
        public DescriptedItems images { get; set; }
        public Spotify_Type name { get; set; }
        public Spotify_Type owner { get; set; }
        public Spotify_Type _public { get; set; }
        public Spotify_Type snapshot_id { get; set; }
        public Tracks tracks { get; set; }
        public Spotify_Type type { get; set; }
        public Spotify_Type uri { get; set; }
    }
    public class Tracks : StringAdditionalproperties
    {
        public FollowersProperties properties { get; set; }
    }
    public class PageProperties
    {
        public Spotify_Type href { get; set; }
        public DescriptedItems items { get; set; }
        public Spotify_Type limit { get; set; }
        public Spotify_Type next { get; set; }
        public Spotify_Type offset { get; set; }
        public Spotify_Type previous { get; set; }
        public Spotify_Type total { get; set; }
    }
    public class PlaylistSnapshot : StringAdditionalproperties
    {
        public PlaylistSnapshotProperties properties { get; set; }
    }
    public class PlaylistSnapshotProperties
    {
        public Spotify_Type snapshot_id { get; set; }
    }
    public class PlaylistTrack : StringAdditionalproperties
    {
        public PlaylistTrackProperties properties { get; set; }
    }
    public class PlaylistTrackProperties
    {
        public Spotify_Type added_at { get; set; }
        public string added_by { get; set; }
        public Spotify_Type is_local { get; set; }
        public string track { get; set; }
    }
    public class PlaylistTrackPage : StringAdditionalproperties
    {
        public PageProperties properties { get; set; }
    }
    public class SavedTrack : StringAdditionalproperties
    {
        public SavedTrackProperties properties { get; set; }
    }
    public class SavedTrackProperties
    {
        public Spotify_Type added_at { get; set; }
        public string track { get; set; }
    }
    public class Search : StringAdditionalproperties
    {
        public SearchProperties properties { get; set; }
    }
    public class SearchProperties
    {
        public SearchAlbums albums { get; set; }
        public SearchAlbums artists { get; set; }
        public SearchAlbums tracks { get; set; }
    }
    public class SearchAlbums : Spotify_Type
    {
        public PageProperties properties { get; set; }
    }
    public class Track2 : StringAdditionalproperties
    {
        public Properties38 properties { get; set; }
    }
    public class Properties38
    {
        public string album { get; set; }
        public DescriptedItems artists { get; set; }
        public DescriptedItems available_markets { get; set; }
        public Spotify_Type disc_number { get; set; }
        public Spotify_Type duration_ms { get; set; }
        public Spotify_Type _explicit { get; set; }
        public DescribedAdditionalProperties external_ids { get; set; }
        public External_Urls7 external_urls { get; set; }
        public Spotify_Type href { get; set; }
        public Spotify_Type id { get; set; }
        public Spotify_Type is_playable { get; set; }
        public Linked_From linked_from { get; set; }
        public Spotify_Type name { get; set; }
        public Spotify_Type preview_url { get; set; }
        public Spotify_Type track_number { get; set; }
        public Spotify_Type type { get; set; }
        public Spotify_Type uri { get; set; }
    }
    public class External_Urls7 : Spotify_Type
    {
        public DescriptedItems additionalProperties { get; set; }
    }
    public class Linked_From : Spotify_Type
    {
        public LinkedFromProperties properties { get; set; }
    }
    public class TrackSimple : StringAdditionalproperties
    {
        public TrackSimpleProperties properties { get; set; }
    }
    public class TrackSimpleProperties
    {
        public DescriptedItems artists { get; set; }
        public DescriptedItems available_markets { get; set; }
        public Spotify_Type disc_number { get; set; }
        public Spotify_Type duration_ms { get; set; }
        public Spotify_Type _explicit { get; set; }
        public DescribedAdditionalProperties external_urls { get; set; }
        public Spotify_Type href { get; set; }
        public Spotify_Type id { get; set; }
        public Spotify_Type is_playable { get; set; }
        public Linked_From linked_from { get; set; }
        public Spotify_Type name { get; set; }
        public Spotify_Type preview_url { get; set; }
        public Spotify_Type track_number { get; set; }
        public Spotify_Type type { get; set; }
        public Spotify_Type uri { get; set; }
    }
    public class LinkedFromProperties
    {
        public DescribedAdditionalProperties external_urls { get; set; }
        public Spotify_Type href { get; set; }
        public Spotify_Type id { get; set; }
        public Spotify_Type type { get; set; }
        public Spotify_Type uri { get; set; }
    }
    public class UserFollowed : StringAdditionalproperties
    {
        public UserFollowedProperties properties { get; set; }
    }
    public class UserFollowedProperties
    {
        public UserFollowedArtists artists { get; set; }
    }
    public class UserFollowedArtists : Spotify_Type
    {
        public UserFollowedArtistsProperties properties { get; set; }
    }
    public class UserFollowedArtistsProperties
    {
        public Cursor cursor { get; set; }
        public Spotify_Type href { get; set; }
        public DescriptedItems items { get; set; }
        public Spotify_Type limit { get; set; }
        public Spotify_Type next { get; set; }
        public Spotify_Type total { get; set; }
    }
    public class Cursor : Spotify_Type
    {
        public CursorProperties properties { get; set; }
    }
    public class CursorProperties
    {
        public Spotify_Type after { get; set; }
    }
    public class UserProfile : StringAdditionalproperties
    {
        public UserProfileProperties properties { get; set; }
    }
    public class UserProfileProperties
    {
        public Spotify_Type displayName { get; set; }
        public UserProfileExternal_Urls external_urls { get; set; }
        public string followers { get; set; }
        public Spotify_Type href { get; set; }
        public Spotify_Type id { get; set; }
        public Spotify_Type type { get; set; }
        public Spotify_Type uri { get; set; }
    }
    public class UserProfileExternal_Urls : Spotify_Type
    {
        public string additionalProperties { get; set; }
    }
    public class Scopes
    {
        public string playlistmodifyprivate { get; set; }
        public string playlistmodifypublic { get; set; }
        public string playlistreadcollaborative { get; set; }
        public string playlistreadprivate { get; set; }
        public string userfollowmodify { get; set; }
        public string userfollowread { get; set; }
        public string userlibrarymodify { get; set; }
        public string userlibraryread { get; set; }
        public string userreadbirthdate { get; set; }
        public string userreademail { get; set; }
        public string userreadprivate { get; set; }
    }
    public class Externaldocs
    {
        public string url { get; set; }
    }
}

