
using BoardGameClubEntities;
using BoardGameClub.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
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

namespace BoardGameClub.Controllers
{
  [EnableCors(origins: "*", headers: "*", methods: "*")]
        public class PlayerController : Controller
        {

        private GameContext db = new GameContext();
        
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
              return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
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
        public JsonResult Self(FormCollection collection)
        {
          try
          {
              var UserId =  User.Identity.GetUserId();
              ApplicationUser user =  UserManager.FindById(UserId);
              PlayerDto Player = getPlayerData(UserId);
              List<BoardGame> Library = getPlayerLibrary(UserId);
              
              Player.PlayersGames = Library;

              return Json(new { Player = Player}, JsonRequestBehavior.AllowGet);
          }
          catch(Exception e)
          {
              return Json(new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "finding self failed"));
          }
        }

        [HttpGet]
        public JsonResult Find(int Id)
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

            return Json(new { Player = Player }, JsonRequestBehavior.AllowGet);
          }
          catch (Exception e)
          {
            return Json(new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "finding self failed"));
          }
        }
        public class playerSearch
        {
          public int Id { get; set; }
          public string Name { get; set; }

        }

        [HttpPost]
        public JsonResult Search(string searchText)
        {
          try
          {
            List<Player> playerSearch = new List<Player>();
            List<BoardGame> BGSearch = new List<BoardGame>();
            playerSearch = db.Players.Where(p => p.Name.Contains(searchText)).ToList();
            List<playerSearch> pSearchResults = new List<playerSearch>();
            foreach (var player in playerSearch)
            {
                playerSearch addMe = new playerSearch();
                addMe.Id = player.Id;
                addMe.Name = player.Name;
                pSearchResults.Add(addMe);
            }
            BGSearch = db.BoardGames.Where(b => b.Name.Contains(searchText)).ToList();
            return Json(new { PlayerResults = pSearchResults, BGResults = BGSearch });
          }
          catch(Exception e)
          {
            return Json(new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Search Failed"));
          }
        }

        private PlayerDto getPlayerData(string playerId)
        {
            PlayerDto currPlayer = new PlayerDto();
            try
            {
              var player = db.Players.FirstOrDefault(x => x.AspNetUser_Id == playerId);

              currPlayer.Id = player.Id;
              currPlayer.Name = player.Name;
              currPlayer.AspNetUser_Id = player.AspNetUser_Id;
            }
            catch(Exception ex)
            {
              new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "finding Player Failed");
            }
            return currPlayer;
        }
        private List<BoardGame> getPlayerLibrary(string playerId)
        {
          List<BoardGame> usersGames = new List<BoardGame>();
          try
          {
              var lib = db.Libraries.Where(l => l.userId == playerId);
              lib.ToList();
              foreach (var entry in lib)
              {
                int? id = entry.bgId;
                int realId = id.GetValueOrDefault();

                BoardGame bgEntry = db.BoardGames.FirstOrDefault(x => x.Id == realId);

                usersGames.Add(bgEntry);
              }
              return usersGames;
            }
              catch(Exception e)
              {
                Console.WriteLine(e);
                new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "finding Player Library Failed");
              }
            return usersGames;
        }

        [HttpGet]
        public String FindBoardgame(string searchText)
        {
            var fullURL = "https://www.boardgamegeek.com/xmlapi/search?search=" + searchText;
            var request = (HttpWebRequest)WebRequest.Create(fullURL);
            request.ContentType = "application/xml";
            request.Accept = "application/xml";
            var response = (HttpWebResponse)request.GetResponse();
            XmlDocument xmlDoc = new XmlDocument();
            string str = null;
            using (var stream = response.GetResponseStream())
            {
              using (var reader = new StreamReader(stream))
              {
                str = reader.ReadToEnd();
              }
            }
            return new JavaScriptSerializer().Serialize(GetXmlData(XElement.Parse(str)));
        }

        

        private static Dictionary<string, object> GetXmlData(XElement xml)
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
                var userName = User.Identity.GetUserName();
                var currentUser = (from x in db.AspNetUsers
                                   where x.UserName.Equals(userName.ToString())
                                   select x).First();

                if (!db.BoardGames.Any(b => b.BGGID == newBoardgame.BGGID))
                {
                  db.BoardGames.Add(newBoardgame);
                  db.SaveChanges();
                }
                var userId = User.Identity.GetUserId();

                var userLibrary = (from b in db.Libraries
                                    where b.userId.Equals(userId)
                                    select b).ToList();
                foreach (var entry in userLibrary)
                {
                    if (db.BoardGames.Any(b => b.Id == entry.bgId)){
                      //reject adding a boardgame that the user already has
                      //return View();
                    }
                }

                BoardGame bgLink = (from x in db.BoardGames
                                       where x.BGGID.Equals(newBoardgame.BGGID)
                                       select x).FirstOrDefault();
                
                Library library = new Library();
                library.bgId = bgLink.Id;
                library.userId = userId;
                db.Libraries.Add(library);
                db.SaveChanges();
                return View("");
              }
            catch
            {
                return View("");
            }
        }

        [HttpPost]
        public JsonResult AddFriend(int Id)
        {
          try
          {
            var userId = User.Identity.GetUserId();
            
            return Json("no complaints here");
          }
          catch
          {
            return Json("u suck");
          }
        }
        // POST: Player/Boardgame
        [HttpGet]
        public JsonResult BG(int GameId)
        {
          try
          {
            BoardGame bgEntry = db.BoardGames.FirstOrDefault(x => x.Id == GameId);

            return Json(bgEntry, JsonRequestBehavior.AllowGet);
          }
          catch
          {
            return Json(new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "finding Player Library Failed"));
          }
        }
        [HttpGet]
        public bool IsOwned (int GameId)
        {
            var UserId = User.Identity.GetUserId();
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
            var UserId = User.Identity.GetUserId();
            var deleteMe = db.Libraries.Where(x => x.userId == UserId && x.bgId == GameId).FirstOrDefault();
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
            var UserId = User.Identity.GetUserId();
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
            var entry = db.Entry(favorMe);
            entry.Property(e => e.favorite).IsModified = true;
            db.SaveChanges(); 
            return true;
          }
          catch (Exception e)
        {
            return false;
          }
        }

        // POST: Player/BoardgameData
        [HttpGet]
        public string BoardgameData(string GameId)
        {
            WebRequest request = (HttpWebRequest)WebRequest.Create("http://www.boardgamegeek.com/xmlapi/boardgame/" + GameId);
            request.ContentType = "application/xml";
            var response = (HttpWebResponse)request.GetResponse();
            XmlDocument xmlDoc = new XmlDocument();
            string str = null;
            using (var stream = response.GetResponseStream())
            {
              using (var reader = new StreamReader(stream))
              {
                str = reader.ReadToEnd();
              }
            }
            return new JavaScriptSerializer().Serialize(GetXmlData(XElement.Parse(str)));
        }


        public ActionResult updateProfilePic(HttpPostedFileBase file)
        {
          if (file != null)
          {
            string pic = System.IO.Path.GetFileName(file.FileName);
            string path = System.IO.Path.Combine(
                                   Server.MapPath("~/Profile/le"), pic);
            // file is uploaded
            file.SaveAs(path);

            // save the image path path to the database or you can send image 
            // directly to database
            // in-case if you want to store byte[] ie. for DB
            using (MemoryStream ms = new MemoryStream())
            {
              file.InputStream.CopyTo(ms);
              byte[] array = ms.GetBuffer();
            }

          }
          // after successfully uploading redirect the user
          return RedirectToAction("actionname", "controller name");
        }

        public ContentResult GetJsonContentResults(object data)
        {
          var camelCaseFormatter = new JsonSerializerSettings();
          camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();

          var JsonResult = new ContentResult
          {
            Content = JsonConvert.SerializeObject(data, camelCaseFormatter),
            ContentType = "application/json"
          };
          return JsonResult;
        }


  }

  public class PlayerDto
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string AspNetUser_Id { get; set; }
    public List<BoardGame> PlayersGames { get; set; }
  }
}
