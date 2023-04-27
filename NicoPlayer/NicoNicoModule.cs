using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using NicoNicoToolkit;
using System;
using System.IO;

public class NicoNicoModule : ModuleBase<SocketCommandContext>
{
    private readonly string _niconicoEmail = "YOUR_NICONICO_EMAIL";
    private readonly string _niconicoPassword = "YOUR_NICONICO_PASSWORD";
    private readonly string _ffmpegPath = "path/to/ffmpeg.exe";

    [Command("playnico")]
    public async Task PlayNicoAsync(string videoId)


