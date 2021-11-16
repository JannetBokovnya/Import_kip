<%@ Control Language="C#" AutoEventWireup="true" CodeFile="JWPlayer.ascx.cs" Inherits="UserControls_Jwplayer_VideoPlayer" %>

<!-- Свойства ширины и высоты playerMenu задаются JS VideoPlayerMenuShow()-->
<div id="playerVideoFon">
    <div id="playerMenu">
        <div id="playerTopMenu">
            <input id="playerVideoName" type="text" value=""/>
            <input type="image" src="../../Images/close_window-15.png" style="float:right; margin: 5px 5px 3px 0;"
                   onclick="VideoPlayerMenuHidden(); return false;"/>
        </div>
        <div id="playerVideoSwf">              
        </div>
    </div>
</div>