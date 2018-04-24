using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BoardGameClubEntities;
using Microsoft.AspNet.Identity;

namespace BoardGameClub.Controllers
{
  //[EnableCors(origins: "*", headers: "*", methods: "*")]

  public class PlaySessionController : Controller
  {
    public ApplicationSignInManager _userManager;
    public Func<string> GetUserId;

    public PlaySessionController()
    {
      db = new GameContext();
      var _dependency = typeof(SqlProviderServices);
      GetUserId = () => User.Identity.GetUserId();
    }

    public GameContext db { get; set; }


    public ActionResult PlaySessions()
    {
      ViewBag.Message = "PlaySessions";

      return View();
    }

    [HttpPost]
    public ActionResult getplaySessions()
    {
      try
      {
        var allPlays = db.PlaySessions.Select(p => p.Id).ToList();
        var results = getPlaysessionData(allPlays);
        return Json(results, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "failed finding playsessions");
      }
    }

    [HttpPost]
    public ActionResult getplaySession(List<int> id)
    {
      try
      {
        var PlaySessionSelect = getPlaysessionData(id);
        return Json(PlaySessionSelect, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "failed finding playsessions");
      }
    }

    [HttpPost]
    public List<PlaySessionDTO> getPlaysessionData(List<int> sessions)
    {
      var results = new List<PlaySessionDTO>();

      foreach (var play in sessions)
      {
        var newPlaySession = db.PlaySessions.FirstOrDefault(s => s.Id.Equals(play));
        var psBoardgame = db.BoardGames.Where(s => s.Id == newPlaySession.BoardGameId).FirstOrDefault();
        var participants = db.Participants.Where(p => p.PlaySession_Id == newPlaySession.Id).ToList();
        var psPlayers = new List<playerSearch>();
        ExtractParticipants(participants, psPlayers);
        var stuff = newPlaySession.DatePlayed.Date;
        var PlaySessionFinal = new PlaySessionDTO(psBoardgame, psPlayers, newPlaySession.PlayTime,
        newPlaySession.DatePlayed.ToString(), newPlaySession.Id);
        results.Add(PlaySessionFinal);
      }
      return results;
    }

    private void ExtractParticipants(List<Participant> participants, List<playerSearch> psPlayers)
    {
      foreach (var participant in participants)
      {
        var newPlayer = db.Players.FirstOrDefault(p => p.Id.Equals(participant.Player_Id));
        psPlayers.Add(new playerSearch(newPlayer.Id, newPlayer.Name));
      }
    }

    [HttpPost]
    public ActionResult PlayerSearch(string searchText)
    {
      try
      {
        var playerSearch = new List<Player>();
        playerSearch = db.Players.Where(p => p.Name.Contains(searchText)).ToList();
        var pSearchResults = new List<playerSearch>();
        GetPlayerValues(playerSearch, pSearchResults);
        return Json(pSearchResults);
      }
      catch (Exception e)
      {
        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Search Failed");
      }
    }

    private static void GetPlayerValues(List<Player> playerSearch, List<playerSearch> pSearchResults)
    {
      foreach (var player in playerSearch)
      {
        var addMe = new playerSearch(player.Id, player.Name);
        pSearchResults.Add(addMe);
      }
    }

    [HttpPost]
    public ActionResult AddPlaySession(PlaySession playSession, List<int> playerIds)
    {
      try
      {
       
        db.PlaySessions.Add(playSession);
        db.SaveChanges();
        //var newest = db.PlaySessions.FirstOrDefault(x => x.BoardGameId == playSession.BoardGameId && x.DatePlayed == playSession.DatePlayed);
        var CurrSession = db.PlaySessions.Where(x => x.BoardGameId == playSession.BoardGameId).ToList().Last();
        var sessionId = CurrSession.Id;
        SaveParticipants(playerIds, sessionId);
        var psPlayers = new List<playerSearch>();
        foreach(var playerId in playerIds)
        {
          string currName = db.Players.Where(x => x.Id == playerId).Select(x => x.Name).FirstOrDefault();
          psPlayers.Add(new playerSearch(playerId, currName));
        }
        var NewPlaysession = new PlaySessionDTO(CurrSession.BoardGame, psPlayers, CurrSession.PlayTime,
          CurrSession.DatePlayed.ToString(), CurrSession.Id);
        return Json(NewPlaysession);
      }
      catch (Exception e)
      {
        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "failed" + e);
      }
    }
    [HttpPost]
    public bool DeletePlaySession(int SessionId)
    {
      try
      {
        var deleteMe = db.PlaySessions.FirstOrDefault(x =>x.Id == SessionId);
        if (deleteMe == null)
        {
          return false;
        }
        db.PlaySessions.Remove(deleteMe);
        db.SaveChanges();
        return true;
      }
      catch
      {
        return false;
      }
    }
    private void SaveParticipants(List<int> playerIds, int sessionId)
    {
      foreach (var player in playerIds)
      {
        var participant = Participant(player, sessionId);
        db.Participants.Add(participant);
      }
      db.SaveChanges();
    }

    public Participant Participant(int playerId, int sessionId)
    {
      var participant = new Participant();
      participant.Player_Id = playerId;
      participant.PlaySession_Id = sessionId;
      return participant;
    }

    [HttpGet]
    public ActionResult Self()
    {
      try
      {
        var UserId = GetUserId();
        var Player = getPlayerData(UserId);
        return Json(Player, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "finding self failed");
      }
    }
    [HttpPost]
    public ActionResult UpdatePlaySessionRecord(List<Participant> participants, int PSId)
    {
      try
      {
        foreach(var participant in participants)
        {
          var update = db.Participants.FirstOrDefault(x => x.Player_Id == participant.Id && x.PlaySession_Id == PSId);
          update.Winner = participant.Winner;
        }
        var PSUPdate= db.PlaySessions.SingleOrDefault(x => x.Id == PSId);
        PSUPdate.Recorded = true;
        db.SaveChanges();

        return new HttpStatusCodeResult(HttpStatusCode.Accepted, "updated win/loss reccord");
      }
      catch
      {
        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "failed update");
      }
    }

    private PlayerDto getPlayerData(string playerId)
    {
      var currPlayer = new PlayerDto();
      var player = db.Players.FirstOrDefault(x => x.AspNetUser_Id == playerId);
      currPlayer.Id = player.Id;
      currPlayer.Name = player.Name;
      currPlayer.AspNetUser_Id = player.AspNetUser_Id;
      return currPlayer;
    }

    [HttpGet]
    public ActionResult BGcollection()
    {
      var usersGames = new List<BoardGame>();
      try
      {
        var playerId = GetUserId();
        var lib = db.Libraries.Where(l => l.userId == playerId).ToList();
        FindGames(lib, usersGames);
        return Json(usersGames, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "finding Player Library Failed");
      }
    }

    private void FindGames(List<Library> lib, List<BoardGame> usersGames)
    {
      foreach (var entry in lib)
      {
        var id = entry.bgId;
        var realId = id.GetValueOrDefault();
        var bgEntry = db.BoardGames.FirstOrDefault(x => x.Id == realId);
        usersGames.Add(bgEntry);
      }
    }

    public class playerSearch
    {
      public playerSearch(int Id, string Name)
      {
        this.Id = Id;
        this.Name = Name;
      }
      public int Id { get; set; }
      public string Name { get; set; }
      public bool winner { get; set; }
    }

    public class PlaySessionDTO
    {

      public PlaySessionDTO(BoardGame boardGame, List<playerSearch> participants, TimeSpan playTime, string datePlayed, int id)
      {
        BoardGame = boardGame;
        Participants = participants;
        PlayTime = playTime;
        DatePlayed = datePlayed;
        Id = id;
      }
      public int Id { get; set; }
      public BoardGame BoardGame { get; set; }
      public List<playerSearch> Participants { get; set; }
      public TimeSpan PlayTime { get; set; }
      public string DatePlayed { get; set; }
      public bool Recorded { get; set; }
    }
  }
}
