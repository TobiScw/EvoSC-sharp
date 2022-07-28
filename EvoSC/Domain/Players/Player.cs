﻿using System.Threading.Tasks;
using EvoSC.Interfaces.Players;
using GbxRemoteNet;
using GbxRemoteNet.Structs;
using Microsoft.EntityFrameworkCore;
using NLog.LayoutRenderers;

namespace EvoSC.Domain.Players;

public class Player : DatabasePlayer, IServerPlayer
{
    GbxRemoteClient IServerPlayer.Client => this.Client;
    private GbxRemoteClient Client { get; set; }

    /// <summary>
    /// Player's Ubisoft name.
    /// </summary>
    public string Name => UbisoftName;

    /// <summary>
    /// Extra information about the player on the server.
    /// </summary>
    public PlayerDetailedInfo? DetailedInfo { get; private set; }

    /// <summary>
    /// Information about the player on the server.
    /// </summary>
    public PlayerInfo Info { get; private set; }

    public Player(GbxRemoteClient client, DatabasePlayer dbPlayer, PlayerInfo info,
        PlayerDetailedInfo? detailedInfo = null) : base(dbPlayer)
    {
        Client = client;
        Info = info;
        DetailedInfo = detailedInfo;
    }

    /// <summary>
    /// Fetch player info from the server and create a new instance.
    /// </summary>
    /// <param name="client"></param>
    /// <param name="dbPlayer"></param>
    /// <returns></returns>
    public static async Task<Player> Create(GbxRemoteClient client, DatabaseContext dbContext, DatabasePlayer dbPlayer)
    {
        dbContext.Entry(dbPlayer).State = EntityState.Detached;

        var info = await client.GetPlayerInfoAsync(dbPlayer.Login);
        var detailed = await client.GetDetailedPlayerInfoAsync(dbPlayer.Login);

        return new Player(client, dbPlayer, info, detailed);
    }

    /// <summary>
    /// Update the player model's server state.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="detailedInfo"></param>
    public void Update(PlayerInfo info, PlayerDetailedInfo? detailedInfo = null)
    {
        Info = info;

        if (detailedInfo != null)
        {
            DetailedInfo = detailedInfo;
        }
    }

    /// <summary>
    /// Update the player model's server state.
    /// </summary>
    /// <param name="playerInfo"></param>
    /// <returns></returns>
    public void Update(SPlayerInfo playerInfo)
    {
        Info.Login = playerInfo.Login;
        Info.NickName = playerInfo.NickName;
        Info.PlayerId = playerInfo.PlayerId;
        Info.TeamId = playerInfo.TeamId;
        Info.SpectatorStatus = playerInfo.SpectatorStatus;
        Info.LadderRanking = playerInfo.LadderRanking;
        Info.Flags = playerInfo.Flags;

        if (DetailedInfo != null)
        {
            DetailedInfo.Login = playerInfo.Login;
            DetailedInfo.NickName = playerInfo.NickName;
            DetailedInfo.PlayerId = playerInfo.PlayerId;
            DetailedInfo.TeamId = playerInfo.TeamId;
        }
    }
}
