using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BoardGameClubEntities;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Provider;

namespace BoardGameClub.Controllers
{
    public class StatsController : Controller
    {
      public GameContext db { get; set; }
      public Func<string> GetUserId;
      public Func<string> GetUserName;
      public StatsController()
      {
        db = new GameContext();
        var _dependency = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        GetUserId = () => User.Identity.GetUserId();
      }

      public List<PlayerStats> AnalyzePlayStats(List<PlaySessionData> playSessions, playerSearch self)
      {
        List<PlayerStats> statsList = new List<PlayerStats>();
        foreach (var playSession in playSessions)
        {
          var selfPS = new playerSearch(self.Id, self.Name);
          selfPS.Team = 0;
          selfPS.Winner = playSession.Participants.Where(x => x.Name == self.Name).Select(x => x.Winner)
            .FirstOrDefault();
          var playerTeam = playSession.Participants.Where(x => x.Name == self.Name).Select(x =>x.Team).FirstOrDefault();
          var players = playSession.Participants;
          foreach (var player in players)
          {
            if (player.Name == self.Name)
            {
              continue;
            }
            if (!statsList.Any(x => x.Name == player.Name))
            {
              var newStats = new PlayerStats(player.Id,player.Name);
              statsList.Add(newStats);
            }
            var opponentUpdate = statsList.FirstOrDefault(x => x.Name == player.Name);
            opponentUpdate.Plays = ++opponentUpdate.Plays;
            if (playerTeam == 0)
            {
              CalculateNonTeam(statsList, player, selfPS);
            }
            else if (playerTeam != player.Team )
            {
              CalculateNemisis(statsList, player);
            }
            else
            {
              CalculateAlly(statsList,player);
            }
          }
        }
        return statsList;
      }

    public List<PlayerStats> AnalyzeBGPlayStats(List<PlaySessionData> playSessions, playerSearch self)
    {
      List<PlayerStats> statsList = new List<PlayerStats>();
      foreach (var playSession in playSessions)
      {
        var selfPS = new playerSearch(self.Id, self.Name);
        selfPS.Team = 0;
        selfPS.Winner = playSession.Participants.Where(x => x.Name == self.Name).Select(x => x.Winner)
          .FirstOrDefault();
        var playerTeam = playSession.Participants.Where(x => x.Name == self.Name).Select(x => x.Team).FirstOrDefault();
        var players = playSession.Participants;
        foreach (var player in players)
        {
          if (player.Name == self.Name)
          {
            var newStats = new PlayerStats(player.Id, player.Name);
            statsList.Add(newStats);
          }
          if (!statsList.Any(x => x.Name == player.Name))
          {
            continue;
          }
          var opponentUpdate = statsList.FirstOrDefault(x => x.Name == player.Name);
          opponentUpdate.Plays = ++opponentUpdate.Plays;
          if (playerTeam == 0)
          {
            CalculateNonTeam(statsList, player, selfPS);
          }
          else if (playerTeam != player.Team)
          {
            CalculateNemisis(statsList, player);
          }
          else
          {
            CalculateAlly(statsList, player);
          }
        }
      }
      return statsList;
    }


    public void CalculateNonTeam(List<PlayerStats> opponentsList, playerSearch opponent, playerSearch Self)
      {
        var opponentUpdate = opponentsList.FirstOrDefault(x => x.Name == opponent.Name);
        if (opponent.Winner == null && Self.Winner == null)
        {
          return;
        }
        else if (opponent.Winner.Value && Self.Winner.Value)
        {
          opponentUpdate.Ties = ++opponentUpdate.Ties;
        }
        else if(!opponent.Winner.Value && Self.Winner.Value)
        {
          opponentUpdate.Losses = ++opponentUpdate.Losses;
        }
        else if (opponent.Winner.Value && !Self.Winner.Value)
        {
          opponentUpdate.Wins = ++opponentUpdate.Wins;
        } 
        else
        {
          opponentUpdate.SharedLoss = ++opponentUpdate.SharedLoss;
        }
      }
      public void CalculateAlly(List<PlayerStats> opponentsList, playerSearch opponent)
      {
      if (opponent.Winner == null)
      {
        return;
      }
      if (opponent.Winner.Value)
        {
          var opponentUpdate = opponentsList.FirstOrDefault(x => x.Name == opponent.Name);
          opponentUpdate.Wins = ++opponentUpdate.Wins;
          opponentUpdate.TeamMateWin = ++opponentUpdate.TeamMateWin;
        }
        else
        {
          var opponentUpdate = opponentsList.FirstOrDefault(x => x.Name == opponent.Name);
          opponentUpdate.Losses = ++opponentUpdate.Losses;
          opponentUpdate.TeamMateLoss = ++opponentUpdate.TeamMateLoss;
        }
      }
      public void CalculateNemisis(List<PlayerStats> opponentsList, playerSearch opponent)
      {
        if (opponent.Winner == null)
        {
          return;
        }
        if (opponent.Winner.Value)
        {
          var opponentUpdate = opponentsList.FirstOrDefault(x => x.Name == opponent.Name);
          opponentUpdate.Wins = ++opponentUpdate.Wins;
          opponentUpdate.OpponentWin = ++opponentUpdate.OpponentWin;
        }
        else
        {
          var opponentUpdate = opponentsList.FirstOrDefault(x => x.Name == opponent.Name);
          opponentUpdate.Losses = ++opponentUpdate.Losses;
          opponentUpdate.OpponentLoss = ++opponentUpdate.OpponentLoss;
        }
      }

      public List<BGStats> CalculateBGStats(List<PlaySessionData> PlaySessionsStats, playerSearch Player)
      {
        var BGStatsList = new List<BGStats>();
        foreach (var currPlaySessionData in PlaySessionsStats)
        {
          if (!BGStatsList.Any(x => x.Id == currPlaySessionData.BoardGame.Id))
          {
            var newBGSTATS = new BGStats(currPlaySessionData.BoardGame.Id, currPlaySessionData.BoardGame.Name);
            
            BGStatsList.Add(newBGSTATS);
          }
          List<PlaySessionData> singletonSearch = new List<PlaySessionData>();
          singletonSearch.Add(currPlaySessionData);
          var Sorted = AnalyzeBGPlayStats(singletonSearch, Player);
          if (Sorted.Count == 1)
          {
            RecordBGUpdate(BGStatsList, Sorted[0], currPlaySessionData.BoardGame.Id);
          }
        }
        return BGStatsList;
      }
      public void RecordBGUpdate(List<BGStats> statList, PlayerStats PlayerRecord, int id)
      {
        var bgStat = statList.First(x => x.Id == id);
        bgStat.Record.Losses += PlayerRecord.Losses;
        bgStat.Record.Wins += PlayerRecord.Wins;
        bgStat.Record.OpponentLoss += PlayerRecord.OpponentLoss;
        bgStat.Record.OpponentWin += PlayerRecord.OpponentWin;
        bgStat.Record.Plays += PlayerRecord.Plays;
        bgStat.Record.SharedLoss += PlayerRecord.SharedLoss;
        bgStat.Record.TeamMateLoss += PlayerRecord.TeamMateLoss;
        bgStat.Record.TeamMateWin += PlayerRecord.TeamMateWin;
        bgStat.Record.Ties += PlayerRecord.Ties;
      }

      public Object PlayerBoardGameAwards(List<BGStats> stats)
      {
        var MostPlayed = stats.OrderByDescending(i => i.Record.Plays).Where(x => x.Record.Plays > 0).Take(1).ToList();
        var MostWon = stats.OrderByDescending(i => i.Record.Wins).Where(x => x.Record.Wins > 0).Take(1).ToList();
        var MostLost = stats.OrderByDescending(i => i.Record.Losses).Where(x => x.Record.Losses > 0).Take(1).ToList();
      return new {MostPlayed, MostWon, MostLost };
      }
      public Relations RelationsLabler(List<PlayerStats> statsList)
      {
        Relations playerRelations = new Relations();
      
        playerRelations.Buddies = statsList.OrderByDescending(i => i.Plays).Where(x => x.Plays > 0).Take(5).ToList();
        playerRelations.Conqueror = statsList.OrderByDescending(i => i.Wins).Where(x => x.Wins > 0).Take(5).ToList();
        playerRelations.Fodder = statsList.OrderByDescending(i => i.Losses).Where(x => x.Losses > 0).Take(5).ToList();
        playerRelations.AshamedLosers = statsList.OrderByDescending(i => i.SharedLoss).Where(x => x.SharedLoss > 0).Take(5).ToList();
        playerRelations.CourageousAllies = statsList.OrderByDescending(i => i.TeamMateWin).Where(x => x.TeamMateWin > 0).Take(5).ToList();
        playerRelations.FrailAllies = statsList.OrderByDescending(i => i.TeamMateLoss).Where(x => x.TeamMateLoss > 0).Take(5).ToList();
        playerRelations.CowardlyVillan = statsList.OrderByDescending(i => i.OpponentLoss).Where(x => x.OpponentWin > 0).Take(5).ToList();
        playerRelations.DastardlyVillan = statsList.OrderByDescending(i => i.OpponentWin).Where(x => x.OpponentLoss > 0).Take(5).ToList();
        playerRelations.TieGuys = statsList.OrderByDescending(i => i.Ties).Where(x => x.Ties > 0).Take(5).ToList();
        playerRelations.WinPercentage = statsList.Where(i => i.Wins + i.Losses >=1).OrderByDescending(i => i.Wins / (i.Wins + i.Losses)).ToList();
        return playerRelations;
      }
  };

  public class BGStats
  {
    public BGStats(int Id, string Name)
    {
      this.Id = Id;
      this.Name = Name;
      this.Record = new PlayerStats(Id, Name);
      this.Related = new Relations();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public PlayerStats Record { get; set; }
    public Relations Related { get; set; }
  }

  public class PlayerStats
  {
    public PlayerStats(int Id, string Name)
    {
      this.Id = Id;
      this.Name = Name;
      this.Wins = 0;
      this.Losses = 0;
      this.TeamMateLoss = 0;
      this.TeamMateWin = 0;
      this.OpponentLoss = 0;
      this.OpponentWin = 0;
      this.SharedLoss = 0;
      this.Ties = 0;
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int Ties { get; set; }
    public int SharedLoss { get; set; }
    public int TeamMateLoss { get; set; }
    public int TeamMateWin { get; set; }
    public int OpponentLoss { get; set; }
    public int OpponentWin { get; set; }
    public int Plays { get; set; }
  }

  public class Relations
  {
    public Relations()
    {
      this.Fodder = new List<PlayerStats>();
      this.Conqueror = new List<PlayerStats>();
      this.DastardlyVillan = new List<PlayerStats>();
      this.CowardlyVillan = new List<PlayerStats>();
      this.CourageousAllies = new List<PlayerStats>();
      this.AshamedLosers = new List<PlayerStats>();
      this.TieGuys = new List<PlayerStats>();
      this.FrailAllies = new List<PlayerStats>();
      this.Buddies = new List<PlayerStats>();
    }
    public List<PlayerStats> Fodder { get; set; } /*Most wins against*/
    public List<PlayerStats> Conqueror { get; set; }  /*Most losses against*/
    public List<PlayerStats> DastardlyVillan { get; set; }  /*Most team losses against*/
    public List<PlayerStats> CowardlyVillan { get; set; }   /*Most team wins against*/
    public List<PlayerStats> CourageousAllies { get; set; }   /*Most team wins with*/
    public List<PlayerStats> FrailAllies { get; set; }  /*Most team losses with*/
    public List<PlayerStats> TieGuys { get; set; }  /*Most ties with*/
    public List<PlayerStats> AshamedLosers { get; set; }  /*Most shared losses*/
    public List<PlayerStats> Buddies { get; set; } /*most Plays together*/
    public List<PlayerStats> WinPercentage { get; set; }
  }
}
