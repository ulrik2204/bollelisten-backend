using Microsoft.AspNetCore.Mvc;

namespace API.Models;

public class SlackSlashCommandRequest
{
    [FromForm(Name = "token")]
    public string? Token { get; set; }

    [FromForm(Name = "team_id")]
    public string? TeamId { get; set; }

    [FromForm(Name = "channel_id")]
    public string? ChannelId { get; set; }

    [FromForm(Name = "user_id")]
    public string? UserId { get; set; }

    [FromForm(Name = "user_name")]
    public string? UserName { get; set; }

    [FromForm(Name = "command")]
    public string? Command { get; set; }

    [FromForm(Name = "text")]
    public string? Text { get; set; }

    [FromForm(Name = "response_url")]
    public string? ResponseUrl { get; set; }
}

public record SlackResponse(string response_type, string text);
