
using BoardGameClubEntities;
using BoardGameClub.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Migrations;
using System.Net.Http;
using Microsoft.Owin.Security.Provider;


namespace BoardGameClub.Controllers
{
  [EnableCors(origins: "*", headers: "*", methods: "*")]
        public class PlayerController : Controller
        {
        public webInterface requester { get; set; }
        
        public GameContext db { get; set; }
          public StatsController Stats { get; set; }
          public Func<string> GetUserId;
        public Func<string> GetUserName;
        public PlayerController()
        {
          db = new GameContext();
          requester = new webInterface();
          var _dependency = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
          GetUserId = () => User.Identity.GetUserId();
          GetUserName = () => User.Identity.GetUserName();
          Stats = new StatsController();
        }

        public interface IHttpWebRequestFactory
        {
          HttpWebRequest Create(string uri);
        }

        public ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
              return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
              _userManager = value;
            }
        }
        [Authorize]
        public ActionResult Players()
        {
          ViewBag.Message = "Players";

          return View();
        }
        class PlayerData
        {
          public List<BoardGame> BGList;
          public Player player;
          public PlayerData(Player player,  List<BoardGame> BGList)
          {
            this.player = player;
            this.BGList = BGList;
          }
        }
        // Get: Player/Self
        [HttpGet]
        public ActionResult Self()
        {
          try
          {
            var UserId = GetUserId();
              ApplicationUser user =  UserManager.FindById(UserId);
              PlayerDto Player = getPlayerData(UserId);
              List<BoardGame> Library = getPlayerLibrary(UserId);
              Player.PlayersGames = Library;
              return Json(Player, JsonRequestBehavior.AllowGet);
          }
          catch(Exception e)
          {
              return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "finding self failed");
          }
        }

        [HttpGet]
        public ActionResult Find(int Id)
        {
          try
          {
             Player currPlayer = new Player();
            currPlayer = db.Players.FirstOrDefault(x => x.Id == Id);
            var user_id = currPlayer.AspNetUser_Id;
            ApplicationUser user = UserManager.FindById(user_id);
            PlayerDto Player = getPlayerData(user_id);
            List<BoardGame> Library = getPlayerLibrary(user_id);
            Player.PlayersGames = Library;
            return Json( Player , JsonRequestBehavior.AllowGet);
          }
          catch (Exception e)
          {
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "finding self failed");
          }
        }
        

        [HttpPost]
        public ActionResult Search(string searchText)
        {
          try
          {
            List<Player> playerSearch = new List<Player>();
            
            var pSearchResults = PSearchResults(searchText);
            var BGSearch = BoardGamesSearch(searchText);
            return Json(new { PlayerResults = pSearchResults, BGResults = BGSearch });
          }
          catch(Exception e)
          {
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Search Failed");
          }
        }

        public List<playerSearch> PSearchResults(string searchText)
        {
          List<Player> playerSearch = new List<Player>();
          playerSearch = db.Players.Where(p => p.Name.Contains(searchText)).ToList();
          List<playerSearch> pSearchResults = new List<playerSearch>();
          PlayerSearchExtraction(playerSearch, pSearchResults);
          return pSearchResults;
        }

        public List<BoardGame> BoardGamesSearch(string searchText)
        {
          List<BoardGame> BGSearch = new List<BoardGame>();
          BGSearch = db.BoardGames.Where(b => b.Name.Contains(searchText)).ToList();
          return BGSearch;
        }

        private static void PlayerSearchExtraction(List<Player> playerSearch, List<playerSearch> pSearchResults)
        {
          foreach (var player in playerSearch)
          {
            playerSearch addMe = new playerSearch(player.Id, player.Name);
            pSearchResults.Add(addMe);
          }
        }

        private PlayerDto getPlayerData(string playerId)
        {
          PlayerDto currPlayer = new PlayerDto();
          
          var player = db.Players.FirstOrDefault(x => x.AspNetUser_Id == playerId);
          currPlayer.Id = player.Id;
          currPlayer.Name = player.Name;
          currPlayer.AspNetUser_Id = player.AspNetUser_Id;
          currPlayer.ProfilePicPath = player.ProfilePicPath;
          
          return currPlayer;
        }
        private List<BoardGame> getPlayerLibrary(string playerId)
        {
          List<BoardGame> usersGames = new List<BoardGame>();
          try
          {
              var lib = db.Libraries.Where(l => l.userId == playerId);
              lib.ToList();
              GetGamesFromLibraries(lib, usersGames);
              return usersGames;
            }
              catch(Exception e)
              {
                new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "finding Player Library Failed: " + e);
              }
            return usersGames;
        }

        private void GetGamesFromLibraries(IQueryable<Library> lib, List<BoardGame> usersGames)
        {
          foreach (var entry in lib)
          {
            int? id = entry.bgId;
            int realId = id.GetValueOrDefault();
            BoardGame bgEntry = db.BoardGames.FirstOrDefault(x => x.Id == realId);
            usersGames.Add(bgEntry);
          }
        }

        public  Dictionary<string, object> GetXmlData(XElement xml)
        {
          var attr = xml.Attributes().ToDictionary(d => d.Name.LocalName, d => (object)d.Value);
          if (xml.HasElements) attr.Add("_value", xml.Elements().Select(e => GetXmlData(e)));
          else if (!xml.IsEmpty) attr.Add("_value", xml.Value);

          return new Dictionary<string, object> { { xml.Name.LocalName, attr } };
        }

        // POST: Player/AddGameToLibrary
        
        [HttpPost]
        public ActionResult AddGameToLibrary(BoardGame newBoardgame)
        {
            try
            {
              var userId = GetUserId();
              var currentUser = db.AspNetUsers.FirstOrDefault(x => x.Id.Equals(userId));
              if (!db.BoardGames.Any(b => b.BGGID == newBoardgame.BGGID))
              {
                db.BoardGames.Add(newBoardgame);
                db.SaveChanges();
              }
              

              if (ChecksForDuplicateLibraryEntry(newBoardgame, userId))
              {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Failed adding Game to Library because it was duplicate");
              }
              BoardGame bgLink = (from x in db.BoardGames
                                       where x.BGGID.Equals(newBoardgame.BGGID)
                                       select x).FirstOrDefault();
                
              Library library = new Library();
              library.bgId = bgLink.Id;
              library.userId = userId;
              db.Libraries.Add(library);
              db.SaveChanges();
              return new HttpStatusCodeResult(HttpStatusCode.Accepted, "You did it" );
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Failed adding Game to Library: " + e);
            }
        }

        private bool ChecksForDuplicateLibraryEntry(BoardGame newBoardgame, string userId)
        {
          var userLibrary = (from b in db.Libraries
            where b.userId.Equals(userId)
            select b).ToList();
          foreach (var entry in userLibrary)
          {
            var bgCheck = db.BoardGames.Any(b => b.Id == entry.bgId);
            if (bgCheck)
            {
              var bg = db.BoardGames.FirstOrDefault(b => b.Id == entry.bgId);
              if (bg.BGGID == newBoardgame.BGGID)
              {
                return true;
              }
            }
          }
          return false;
        }

        //[HttpPost]
        //public JsonResult AddFriend(int Id)
        //{
        //  try
        //  {
        //    var userId = GetUserId();
            
        //    return Json("no complaints here");
        //  }
        //  catch
        //  {
        //    return Json("u suck");
        //  }
        //}
        // POST: Player/Boardgame
        [HttpGet]
        public ActionResult BG(int GameId)
        {
          try
          {
            BoardGame bgEntry = db.BoardGames.FirstOrDefault(x => x.Id == GameId);

            return Json(bgEntry, JsonRequestBehavior.AllowGet);
          }
          catch
          {
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "finding Player Library Failed");
          }
        }

        
        [HttpGet]
        public bool IsOwned (int GameId)
        {
            var UserId = GetUserId();
            if(db.Libraries.Any(x => x.userId == UserId && x.bgId == GameId))
            {
              return true;
            }
            return false;
        }
        [HttpPost]
        public bool DeleteGame(int GameId)
        {
          try
          {
            var UserId = GetUserId();
            var deleteMe = db.Libraries.FirstOrDefault(x => x.userId == UserId && x.bgId == GameId);
            if (deleteMe == null)
            {
              return false;
            }
            db.Libraries.Remove(deleteMe);
            db.SaveChanges();
            return true;
          }
          catch
          {
            return false;
          }
        }

        [HttpPost]
        public bool FavoriteGame(int GameId)
        {
          try
          {
            var UserId = GetUserId();
            var favorMe = db.Libraries.FirstOrDefault(x => x.userId == UserId && x.bgId == GameId);
            if(favorMe.favorite == true)
            {
              favorMe.favorite = false;
            }
            else
            {
              favorMe.favorite = true;
            }
            db.Libraries.Attach(favorMe);
            db.SaveChanges(); 
            return true;
          }
          catch
          {
            return false;
          }
        }

        // POST: Player/BoardgameData
        [HttpGet]
        public string BoardgameData(string GameId)
        {
          var url = "http://www.boardgamegeek.com/xmlapi/boardgame/" + GameId;
          var uri = new Uri(url);
          var str = requester.StreamReadXml(uri);
          var results = new JavaScriptSerializer().Serialize(GetXmlData(XElement.Parse(str)));
          return results;
        }

        [HttpGet]
        public String FindBoardgame(string searchText)
        {

          var fullURL = "https://www.boardgamegeek.com/xmlapi/search?search=" + searchText;
          var str = requester.StreamReadXml(new Uri(fullURL));
          var results = new JavaScriptSerializer().Serialize(GetXmlData(XElement.Parse(str)));
          return results;
        }

        [HttpPost]
        public ActionResult UploadProfilePic(HttpPostedFileBase file)
        {
          if (file != null)
          {
            string pic = System.IO.Path.GetFileName(file.FileName);
            var FileName = GetUserName() + pic;
            FileName = FileName.Replace(" ", String.Empty);
            string path = System.IO.Path.Combine(
            Server.MapPath("~/Content/profilePics"), FileName);
            // file is uploaded
            file.SaveAs(path);
            var userId = GetUserId();
            var updatePlayer = db.Players.SingleOrDefault(x => x.AspNetUser_Id == userId);
            if (updatePlayer.ProfilePicPath == null)
            {
          //logic here to remove image file;
              if (System.IO.File.Exists(@updatePlayer.ProfilePicPath))
              {
                System.IO.File.Delete(@updatePlayer.ProfilePicPath);
              }
            }

            updatePlayer.ProfilePicPath = FileName;
            db.Players.AddOrUpdate(updatePlayer);
            db.SaveChanges();
          }
          else
          {
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "No File");
          }
          // after successfully uploading redirect the user
          return new HttpStatusCodeResult(HttpStatusCode.Accepted, "profile Pic Uploaded");
        }

        [HttpPost]
        public List<PlaySession> GetPlaySessionsByPlayer(int id)
        {
          List<Participant> psPlayer = db.Participants.Where(x => x.Player_Id == id).ToList();
          List<PlaySession> ResultPlaysessions = new List<PlaySession>();
          foreach (var participants in psPlayer)
          {
            var result = db.PlaySessions.Where(x => x.Id == participants.PlaySession_Id).First();
            if(result != null)
            {
              ResultPlaysessions.Add(result);
            }
          } 
          
          return ResultPlaysessions;
        }

        [HttpGet]
        public ActionResult GetStatsID(string ID)
        {
          if (ID == "self")
          {
            ID = GetUserId();
          }
          else
          {
            var parsedVal = Int32.Parse(ID);
            ID = db.Players.Where(x => x.Id == parsedVal).Select(x => x.AspNetUser_Id).FirstOrDefault();
          }
          int playerID = db.Players.Where(x => x.AspNetUser_Id == ID).Select(x => x.Id).FirstOrDefault();
          return GetPlayerStats(playerID);
        }

        public ActionResult GetPlayerStats(int id)
        {
          try
          {
            var searchplayer = db.Players.FirstOrDefault(x => x.Id == id);
            playerSearch playerRequest = new playerSearch(searchplayer.Id, searchplayer.Name);
            var PlayerStats = GetPlaySessionsByPlayer(id);
            List<PlaySessionData> ReturnedStats = ExtractPlaySessionsData(PlayerStats, playerRequest);
            var PSStatsList = Stats.AnalyzePlayStats(ReturnedStats, playerRequest);
            var PlayerRelations = Stats.RelationsLabler(PSStatsList);
            var BGStatsList = Stats.CalculateBGStats(ReturnedStats, playerRequest);
            var BGRecords = Stats.PlayerBoardGameAwards(BGStatsList);
            return Json(new { PSStatsList, BGStatsList ,BGRecords, PlayerRelations }, JsonRequestBehavior.AllowGet);
          }
          catch(Exception e)
          {
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "failed finding stats" + e);
          }
        }

        public List<PlaySessionData> ExtractPlaySessionsData(List<PlaySession> PlayerStats, playerSearch player)
        {
          List<PlaySessionData> ReturnedStats = new List<PlaySessionData>();
          foreach (var evalPlaySession in PlayerStats)
          {
            var Participants = db.Participants.Where(x => x.PlaySession_Id == evalPlaySession.Id).ToList();
            var players = GetPlayersSearch(Participants);
            if (evalPlaySession.BoardGame == null)
            {
              evalPlaySession.BoardGame = db.BoardGames.FirstOrDefault(x => x.Id == evalPlaySession.BoardGameId);
            }
            PlaySessionData currPlaysession = new PlaySessionData(evalPlaySession.BoardGame, players, evalPlaySession.PlayTime,
              evalPlaySession.DatePlayed.ToString());
            if (evalPlaySession.BoardGame.Teams!=null)
            {
              List<Team> psTeams = new List<Team>();
              foreach (var teamMates in players)
              {
                ParsePlayersIntoTeams(teamMates, psTeams);
                currPlaysession.Teams = psTeams;
              }
            }
            ReturnedStats.Add(currPlaysession);
          }
          return ReturnedStats;
        }

        public void ParsePlayersIntoTeams(playerSearch teamMate, List<Team> teams)
        {
          if (teams.Count == 0)
          {
            Team addTeam = new Team();
            List <playerSearch> startPlayerList = new List<playerSearch>();
            startPlayerList.Add(teamMate);
            if (teamMate.Team.HasValue)
            {
            addTeam.Name = teamMate.Team.Value;
            }
            addTeam.TeamMates = startPlayerList;
            if (teamMate.Winner.HasValue)
            {
              addTeam.Winner = teamMate.Winner.Value;
            }
            teams.Add(addTeam);
            return;
          }
          foreach (var team in teams)
          {
            if (team.Name == teamMate.Team)
            {
              List<playerSearch> curTeam = team.TeamMates;
              curTeam.Add(teamMate);
              team.TeamMates = curTeam;
              return;
            }
          }
          Team nonExsistantTeam = new Team();
          List<playerSearch> nonExsistantPlayerList = new List<playerSearch>();
          if (teamMate.Winner.HasValue)
          {
            nonExsistantTeam.Winner = teamMate.Winner.Value;
          }
          nonExsistantPlayerList.Add(teamMate);
          nonExsistantTeam.TeamMates = nonExsistantPlayerList;
          nonExsistantTeam.Winner = teamMate.Winner.Value;
          teams.Add(nonExsistantTeam);
          return;
        }

        public List<playerSearch> GetPlayersSearch(List<Participant> participants)
        {
          List<playerSearch> playerSearchList = new List<playerSearch>();
          foreach (var participant in participants)
          {
            var currPlayer = db.Players.Where(x => x.Id == participant.Player_Id).FirstOrDefault();
            playerSearch newPlayer = new playerSearch(currPlayer.Id, currPlayer.Name);
            if(participant.Winner.HasValue)
            {
              newPlayer.Winner = participant.Winner.Value;
            }
            if (participant.Team.HasValue)
            {
              newPlayer.Team = participant.Team.Value;
            }
            playerSearchList.Add(newPlayer);
          }
          return playerSearchList;
        }

        [HttpGet]
        public ActionResult GetBGPlayStats(int GameId)
        {
          try
          {
            var myId = GetUserId();
            var selfData = db.Players.FirstOrDefault(x => x.AspNetUser_Id == myId);
            playerSearch Self = new playerSearch(selfData.Id, selfData.Name);
            List<PlaySession> relatedPlayers = db.PlaySessions.Where(x => x.BoardGameId == GameId).ToList();
            List<PlaySessionData> ReturnedStats = ExtractPlaySessionsData(relatedPlayers, Self);
            var playerStats = Stats.AnalyzePlayStats(ReturnedStats, Self);
            
            var nulPlayer = new playerSearch(0, null);
            var SelfBGStats = Stats.AnalyzePlayStats(ReturnedStats,Self);
            var BGRecordStats = Stats.AnalyzePlayStats(ReturnedStats, nulPlayer);
            var BGPlayerRelations = Stats.RelationsLabler(BGRecordStats);
            var totalPlays = ReturnedStats.Count;
            return Json(new { SelfBGStats, ReturnedStats, totalPlays, BGPlayerRelations }, JsonRequestBehavior.AllowGet);
          }
          catch(Exception e)
          {
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "failed finding stats" + e);
          }
        }

        [HttpGet]
        public ActionResult GetBGOwners(int GameId)
        {
          try
          {
            var users = new List<Player>();
            var Libraries = db.Libraries.Where(x => x.bgId == GameId).ToList();
            foreach (var user in Libraries)
            {
              users.Add(db.Players.FirstOrDefault(x => x.AspNetUser_Id == user.userId));
            }
            return Json(users, JsonRequestBehavior.AllowGet);
          }
          catch (Exception e)
          {
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "failed finding stats" + e.Message);
          }
        }

    public bool CheckIfImageFile(string FileName)
        {
          return false;
        }
  }

  public class Team
  {
    public List<playerSearch> TeamMates { get; set; }
    public bool Winner { get; set; }
    public int Name { get; set; }
  }
  public class PlaySessionData
  {
    public PlaySessionData(BoardGame boardGame, List<playerSearch> participants, TimeSpan playTime, string datePlayed)
    {
      BoardGame = boardGame;
      Participants = participants;
      PlayTime = playTime;
      DatePlayed = datePlayed;
    }
    public BoardGame BoardGame { get; set; }
    public List<playerSearch> Participants { get; set; }
    public TimeSpan PlayTime { get; set; }
    public string DatePlayed { get; set; }
    public List<Team> Teams { get; set; }
  }
  public class PlayerDto
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string AspNetUser_Id { get; set; }
    public string ProfilePicPath { get; set; }
    public List<BoardGame> PlayersGames { get; set; }
  }
  public class playerSearch
  {
    public playerSearch(int id, string name)
    {
      Id = id;
      Name = name;
      Winner = null;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public bool? Winner { get; set; }
    public int? Team { get; set; }
  }
  public class webInterface
  {
    public virtual string StreamReadXml(Uri uri)
    {
      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
      request.ContentType = "application/xml";
      request.Accept = "application/xml";
      var response =(HttpWebResponse)request.GetResponse();
      XmlDocument xmlDoc = new XmlDocument();
      string str = null;
      using (var stream = response.GetResponseStream())
      {
        using (var reader = new StreamReader(stream))
        {
          str = reader.ReadToEnd();
        }
      }
      return str;
    }
  }
}
