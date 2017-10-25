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

namespace BoardGameClub.Controllers
{
  [EnableCors(origins: "*", headers: "*", methods: "*")]
        public class PlayerController : Controller
        {
        
        private ApplicationDbContext db = ApplicationDbContext.Create();
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
        public async Task<ActionResult> Self(FormCollection collection)
        {
          try
          {
              var CurrentUser =  User.Identity.GetUserId();
              ApplicationUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
              Player Player = getPlayerData(User.Identity.GetUserId());
              List<BoardGame> Library = getPlayerLibrary(user.Id);
              var AllPlayerData = new PlayerData(Player,  Library);
              return GetJsonContentResults(AllPlayerData);
          }
          catch
          {
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "finding self failed");
          }
        }

        private Player getPlayerData(string playerId)
        {
            Player currPlayer = new Player();
            try
            {
               currPlayer = (from x in db.Players
                            where x.AspNetUser_Id == playerId
                            select x).FirstOrDefault();
            }
            catch
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
          List<Library> lib = (from x in db.Libraries
                               where x.AspNetUsers.Equals(playerId)
                               select x).ToList();
          foreach (var entry in lib)
          {
            BoardGame bgEntry = (from x in db.Boardgames
                                 where x.Libraries.Equals(entry.Id)
                                 select x).FirstOrDefault();
            usersGames.Add(bgEntry);
          }
        }
        catch
        {
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
        
        // POST: Player/Create
        [HttpPost]
        public ActionResult AddGameToLibrary(FormCollection collection)
        {
            
            try
            {
                var boardgame = new BoardGame { };
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: Player/Boardgame
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



        // POST: Player/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Player/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Player/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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
}
