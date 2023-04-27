using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Net.Labs;

public class NicoNicoDiscordBot
{
    private DiscordSocketClient _client;
    private CommandService _commands;
    private IServiceProvider _services;

    public async Task RunBotAsync()
    {
        _client = new DiscordSocketClient();
        _commands = new CommandService();

        _services = ConfigureServices();

        _client.Log += Log;

        await RegisterSlashCommandsAsync();

        await _client.LoginAsync(TokenType.Bot, "YOUR_BOT_TOKEN");
        await _client.StartAsync();

        await Task.Delay(-1);
    }

    private IServiceProvider ConfigureServices()
    {
        return new ServiceCollection()
            .BuildServiceProvider();
    }

    private Task Log(LogMessage arg)
    {
        Console.WriteLine(arg);
        return Task.CompletedTask;
    }

    private async Task RegisterSlashCommandsAsync()
    {
        _client.SlashCommandExecuted += HandleSlashCommandAsync;

        // スラッシュコマンドの登録
        await _client.RegisterSlashCommandAsync("playnico", "ニコニコ動画から動画をダウンロードし、音声ファイルに変換し、ボイスチャンネルで再生します。", new List<SlashCommandOptionBuilder>
        {
            new SlashCommandOptionBuilder()
                .WithName("video_id")
                .WithDescription("再生する動画のID")
                .WithType(ApplicationCommandOptionType.String)
                .WithRequired(true)
        });
    }

    private async Task HandleSlashCommandAsync(SocketSlashCommand arg)
    {
        // playnicoコマンドの処理
        if (arg.Command.Name == "playnico")
        {
            string videoId = arg.Data.Options.First().Value.ToString();
            var commandResult = await _commands.ExecuteAsync(arg, "PlayNicoAsync", new object[] { videoId }, _services);

            if (commandResult.IsSuccess)
            {
                await arg.RespondAsync("動画の再生を開始します。");
            }
            else
            {
                await arg.RespondAsync($"エラーが発生しました: {commandResult.ErrorReason}");
            }
        }
    }
}
