function VideoPlayerMenuShow(videoName, videoFilePath) {
    var width = 550;
    var height = 300;
    $("#playerVideoName").val(videoName);

    $("#playerVideoFon").css('display', 'block');
    $("#playerMenu").css('width', width + 'px');
    $("#playerMenu").css('height', height + 'px');
    $("#playerMenu").css('margin-top', -height / 2 + 'px');
    $("#playerMenu").css('margin-left', -width / 2 + 'px');

    jwplayer("playerVideoSwf").setup({
        file: videoFilePath,
        flashplayer: "../../UserControls/VideoPlayer/player.swf",
        width: width,
        height: height
    });
}

function VideoPlayerMenuHidden() {
    jwplayer("playerVideoSwf").stop();
    $("#playerVideoFon").css('display', 'none');
}