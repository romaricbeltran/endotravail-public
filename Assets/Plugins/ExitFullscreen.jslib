var plugin = {
    ExitFullScreen : function(url)
    {
        exitFullscreen();
    },
};
mergeInto(LibraryManager.library, plugin);