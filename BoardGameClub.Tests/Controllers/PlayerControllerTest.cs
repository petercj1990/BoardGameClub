using BoardGameClub.Controllers;
using BoardGameClub.Models;
using BoardGameClubEntities;
using Microsoft.AspNet.Identity;
using Moq;
using NUnit.Framework;
using SemanticComparison;
using SemanticComparison.Fluent;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace BoardGameClub.Tests.Controllers
{


  [TestFixture]
  public class PlayerControllerTest : PlayerController
  {
    

    [SetUp]
    public void SetUp()
    {

      var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
      userStoreMock.Setup(s => s.FindByIdAsync("testId")).ReturnsAsync(new ApplicationUser
      {
        Id = "eb218371-e5aa-4b18-9975-5197d242f130",
        Email = "petetjensen@email.com",
        UserName = "Peter Jensen"
      });
      var applicationUserManager = new ApplicationUserManager(userStoreMock.Object);

      gcMock = new Mock<GameContext>();
      p = new PlayerController
      {
        GetUserId = () => "eb218371-e5aa-4b18-9975-5197d242f130",
        GetUserName = () => "Peter Jensen",
        UserManager = applicationUserManager
      };
      p.db = gcMock.Object;

      will = new Player {Name = "Will Munn", Id = 3, AspNetUser_Id = "5fc4ced5-8f00-46ad-910e-2d3a38be9e59"};
      peter = new Player {Name = "Peter Jensen", Id = 1, AspNetUser_Id = "eb218371-e5aa-4b18-9975-5197d242f130"};
      Jen = new Player {Name = "Jenna Maroni", Id = 5, AspNetUser_Id = "im_a_test_Id"};
      players.Add(peter);
      players.Add(will);
      players.Add(Jen);
      var mockPl = new Mock<DbSet<Player>>();
      var qpl = players.AsQueryable();
      mockPl.As<IQueryable<Player>>().Setup(m => m.Provider).Returns(qpl.Provider);
      mockPl.As<IQueryable<Player>>().Setup(m => m.Expression).Returns(qpl.Expression);
      mockPl.As<IQueryable<Player>>().Setup(m => m.ElementType).Returns(qpl.ElementType);
      mockPl.Setup(c => c.Add(It.IsAny<Player>()))
        .Returns<Player>(arg => arg)
        .Callback<Player>(s => players.Add(s));
      //mockPS.As<IQueryable<PlaySession>>().Setup(m => m.GetEnumerator()).Returns(0 => qps.GetEnumerator());
      gcMock.Setup(c => c.Players).Returns(mockPl.Object);

      gloomHaven = new BoardGame
      {
        Id = 28,
        BGGID = 174420,
        Name = "Gloomhaven",
        TotalPlays = 0,
        MinPlayer = 1,
        MaxPlayer = 4,
        PlayingTime = 150,
        Image = "https://cf.geekdo-images.com/images/pic2437871_t.jpg",
        Description =
          "Gloomhaven is a game of Euro - inspired tactical combat in a persistent world of shifting motives.Players will take on the role of a wandering adventurer with their own special set of skills and their own reasons for travelling to this dark corner of the world.Players must work together out of necessity to clear out menacing dungeons and forgotten ruins.In the process they will enhance their abilities with experience and loot, discover new locations to explore and plunder, and expand an ever - branching story fueled by the decisions they make.< br />< br /> This is a game with a persistent and changing world that is ideally played over many game sessions.After a scenario, players will make decisions on what to do, which will determine how the story continues, kind of like a &ldquo; Choose Your Own Adventure&rdquo; book.Playing through a scenario is a cooperative affair where players will fight against automated monsters using an innovative card system to determine the order of play and what a player does on their turn.< br />< br /> Each turn a player chooses two cards to play out of their hand.The number on the top card determines their initiative for the round. Each card also has a top and bottom power, and when it is a player & rsquo; s turn in the initiative order, they determine whether to use the top power of one card and the bottom power of the other, or vice - versa.Players must be careful, though, because over time they will permanently lose cards from their hands.If they take too long to clear a dungeon, they may end up exhausted and be forced to retreat.< br />< br />",
        Category = "Miniatures"
      };
      bloodrage = new BoardGame
      {
        Id = 5,
        BGGID = 170216,
        Name = "Blood Rage",
        TotalPlays = 0,
        MinPlayer = 3,
        MaxPlayer = 4,
        PlayingTime = 90,
        Image = "https://cf.geekdo-images.com/images/pic2439223_t.jpg",
        Description =
          "&quot;Life is Battle; Battle is Glory; Glory is ALL&quot;<br/><br/>In Blood Rage, each player controls their own Viking clan&rsquo;s warriors, leader, and ship. Ragnar&ouml;k has come, and it&rsquo;s the end of the world! It&rsquo;s the Vikings&rsquo; last chance to go down in a blaze of glory and secure their place in Valhalla at Odin&rsquo;s side! For a Viking there are many pathways to glory. You can invade and pillage the land for its rewards, crush your opponents in epic battles, fulfill quests, increase your clan's stats, or even die gloriously either in battle or from Ragnar&ouml;k, the ultimate inescapable doom.<br/><br/>Most player strategies are guided by the cards drafted at the beginning of each of the three game rounds (or Ages). These &ldquo;Gods&rsquo; Gifts&rdquo; grant you numerous boons for your clan including: increased Viking strength and devious battle strategies, upgrades to your clan, or even the aid of legendary creatures from Norse mythology. They may also include various quests, from dominating specific provinces, to having lots of your Vikings sent to Valhalla. Most of these cards are aligned with one of the Norse gods, hinting at the kind of strategy they support. For example, Thor gives more glory for victory in battle, Heimdall grants you foresight and surprises, Tyr strengthens you in battle, while the trickster Loki actually rewards you for losing battles, or punishes the winner.<br/><br/>Players must choose their strategies carefully during the draft phase, but also be ready to adapt and react to their opponents&rsquo; strategies as the action phase unfolds. Battles are decided not only by the strength of the figures involved, but also by cards played in secret. By observing your opponent&rsquo;s actions and allegiances to specific gods, you may predict what card they are likely to play, and plan accordingly. Winning battles is not always the best course of action, as the right card can get you even more rewards by being crushed. The only losing strategy in Blood Rage is to shy away from battle and a glorious death!<br/><br/>",
        Category = "Mythology"
      };
      catan = new BoardGame
      {
        Id = 1,
        BGGID = 13,
        Name = "Catan",
        TotalPlays = 0,
        MinPlayer = null,
        MaxPlayer = null,
        PlayingTime = 120,
        Image = "https://cf.geekdo-images.com/images/pic2419375_t.jpg",
        Description =
          "In Catan (formerly The Settlers of Catan), players try to be the dominant force on the island of Catan by building settlements, cities, and roads. On each turn dice are rolled to determine what resources the island produces. Players collect these resources (cards)&mdash;wood, grain, brick, sheep, or stone&mdash;to build up their civilizations to get to 10 victory points and win the game.<br/><br/>Setup includes randomly placing large hexagonal tiles (each showing a resource or the desert) in a honeycomb shape and surrounding them with water tiles, some of which contain ports of exchange. Number disks, which will correspond to die rolls (two 6-sided dice are used), are placed on each resource tile. Each player is given two settlements (think: houses) and roads (sticks) which are, in turn, placed on intersections and borders of the resource tiles. Players collect a hand of resource cards based on which hex tiles their last-placed house is adjacent to. A robber pawn is placed on the desert tile.<br/><br/>A turn consists of possibly playing a development card, rolling the dice, everyone (perhaps) collecting resource cards based on the roll and position of houses (or upgraded cities&mdash;think: hotels) unless a 7 is rolled, turning in resource cards (if possible and desired) for improvements, trading cards at a port, and trading resource cards with other players. If a 7 is rolled, the active player moves the robber to a new hex tile and steals resource cards from other players who have built structures adjacent to that tile.<br/><br/>Points are accumulated by building settlements and cities, having the longest road and the largest army (from some of the development cards), and gathering certain development cards that simply award victory points. When a player has gathered 10 points (some of which may be held in secret), he announces his total and claims the win.<br/><br/>Catan has won multiple awards and is one of the most popular games in recent history due to its amazing ability to appeal to experienced gamers as well as those new to the hobby.<br/><br/>Die Siedler von Catan was originally published by KOSMOS and has gone through multiple editions. It was licensed by Mayfair and has undergone four editions as The Settlers of Catan. In 2015, it was formally renamed Catan to better represent itself as the core and base game of the Catan series. It has been re-published in two travel editions, portable edition and compact edition, as a special gallery edition (replaced in 2009 with a family edition), as an anniversary wooden edition, as a deluxe 3D collector's edition, in the basic Simply Catan, as a beginner version, and with an entirely new theme in Japan and Asia as Settlers of Catan: Rockman Edition. Numerous spin-offs and expansions have also been made for the game.<br/><br/> https://cf.geekdo-images.com/images/pic3711701_t.jpg	Cities of Splendor is a quartet of expansions for use with the Splendor base game. Each expansion is added to the basic game and they should be played separately.<br/><br/>The Cities replaces the noble tiles with 3 different city tiles (randomly taken from a pool of 14). The city tiles are objectives (in prestige points and/or development cards) and you need to fulfill one of them in order to win.<br/><br/>The Trading Posts are special bonuses you earn by acquiring an array of development cards: more prestige points from the 1st noble tile you receive, an extra token when you choose the &quot;Take 2 gem tokens of the same color&quot; action, and so on.<br/><br/>The Orient adds three decks of cards (one for each level of development cards). They are added on the right side of the regular cards and you place two of them face-up on the table for each level. The new cards have special powers (like double bonus cards or joker cards which take the color of one of the developments you already own).<br/><br/>The Strongholds expansion gives each player three towers (strongholds). When you acquire a new card, you must put a stronghold on an face-up card on the table. You're now the only player able to purchase/reserve it. You may also move one of your strongholds from one card to another one or remove another player's stronghold. When your three strongholds are on the same card, you can buy it after your regular action, allowing you to make two acquisitions in the same turn or buying that card after taking your tokens!<br/><br/> https://cf.geekdo-images.com/images/pic1904079_t.jpg	Splendor is a game of chip-collecting and card development. Players are merchants of the Renaissance trying to buy gem mines, means of transportation, shops&mdash;all in order to acquire the most prestige points. If you're wealthy enough, you might even receive a visit from a noble at some point, which of course will further increase your prestige.<br/><br/>On your turn, you may (1) collect chips (gems), or (2) buy and build a card, or (3) reserve one card. If you collect chips, you take either three different kinds of chips or two chips of the same kind. If you buy a card, you pay its price in chips and add it to your playing area. To reserve a card&mdash;in order to make sure you get it, or, why not, your opponents don't get it&mdash;you place it in front of you face down for later building; this costs you a round, but you also get gold in the form of a joker chip, which you can use as any gem.<br/><br/>All of the cards you buy increase your wealth as they give you a permanent gem bonus for later buys; some of the cards also give you prestige points. In order to win the game, you must reach 15 prestige points before your opponents do.<br/><br/>",
        Category = "Negotiation",
        Teams = 1
      };
      williamsburg = new BoardGame
      {
        Id = 4,
        BGGID = 369,
        Name = "Williamsburg",
        TotalPlays = 0,
        MinPlayer = 2,
        MaxPlayer = 7,
        PlayingTime = 69,
        Image = "https://wholelottanewmoney",
        Description =
          "we out here smonkin big doints",
        Category = "leet sniping"
      };

      boardgames.Add(bloodrage);
      boardgames.Add(catan);
      boardgames.Add(gloomHaven);
      boardgames.Add(williamsburg);
      var mockBG = new Mock<DbSet<BoardGame>>();
      var qbg = boardgames.AsQueryable();
      mockBG.As<IQueryable<BoardGame>>().Setup(m => m.Provider).Returns(qbg.Provider);
      mockBG.As<IQueryable<BoardGame>>().Setup(m => m.Expression).Returns(qbg.Expression);
      mockBG.As<IQueryable<BoardGame>>().Setup(m => m.ElementType).Returns(qbg.ElementType);
      mockBG.Setup(c => c.Add(It.IsAny<BoardGame>()))
        .Returns<BoardGame>(arg => arg)
        .Callback<BoardGame>(s => boardgames.Add(s));
      //mockPS.As<IQueryable<PlaySession>>().Setup(m => m.GetEnumerator()).Returns(0 => qps.GetEnumerator());
      gcMock.Setup(c => c.BoardGames).Returns(mockBG.Object);

      par1 = new Participant {Id = 1, Player_Id = 1, PlaySession_Id = 18, Team = 1, Winner = true};
      par2 = new Participant {Id = 2, Player_Id = 3, PlaySession_Id = 18, Team = 2, Winner = false};
      par3 = new Participant {Id = 3, Player_Id = 1, PlaySession_Id = 19};
      par4 = new Participant {Id = 4, Player_Id = 3, PlaySession_Id = 19};
      par5 = new Participant {Id = 5, Player_Id = 1, PlaySession_Id = 20};
      par6 = new Participant {Id = 6, Player_Id = 3, PlaySession_Id = 20};
      par7 = new Participant {Id = 7, Player_Id = 1, PlaySession_Id = 21};
      par8 = new Participant { Id = 7, Player_Id = 3, PlaySession_Id = 22 };

      participants.Add(par1);
      participants.Add(par2);
      participants.Add(par3);
      participants.Add(par4);
      participants.Add(par5);
      participants.Add(par6);
      participants.Add(par7);
      participants.Add(par8);


      var mockPt = new Mock<DbSet<Participant>>();
      var qpt = participants.AsQueryable();
      mockPt.As<IQueryable<Participant>>().Setup(m => m.Provider).Returns(qpt.Provider);
      mockPt.As<IQueryable<Participant>>().Setup(m => m.Expression).Returns(qpt.Expression);
      mockPt.As<IQueryable<Participant>>().Setup(m => m.ElementType).Returns(qpt.ElementType);
      mockPt.Setup(c => c.Add(It.IsAny<Participant>()))
        .Returns<Participant>(arg => arg)
        .Callback<Participant>(s => participants.Add(s));
      //mockPS.As<IQueryable<PlaySession>>().Setup(m => m.GetEnumerator()).Returns(0 => qps.GetEnumerator());
      gcMock.Setup(c => c.Participants).Returns(mockPt.Object);

      lib1 = new Library {Id = 1, userId = "eb218371-e5aa-4b18-9975-5197d242f130", bgId = 1, favorite = true};
      lib2 = new Library {Id = 2, userId = "eb218371-e5aa-4b18-9975-5197d242f130", bgId = 5};
      lib3 = new Library {Id = 3, userId = "5fc4ced5-8f00-46ad-910e-2d3a38be9e59", bgId = 28};
      var mockLib = new Mock<DbSet<Library>>();
      libraries.Add(lib1);
      libraries.Add(lib2);
      libraries.Add(lib3);
      var qlib = libraries.AsQueryable();
      mockLib.As<IQueryable<Library>>().Setup(lib => lib.Provider).Returns(qlib.Provider);
      mockLib.As<IQueryable<Library>>().Setup(m => m.Expression).Returns(qlib.Expression);
      mockLib.As<IQueryable<Library>>().Setup(m => m.ElementType).Returns(qlib.ElementType);
      mockLib.Setup(c => c.Add(It.IsAny<Library>()))
        .Returns<Library>(arg => arg)
        .Callback<Library>(s => libraries.Add(s));

      gcMock.Setup(c => c.Libraries).Returns(mockLib.Object);

      self = new AspNetUser {Id = "eb218371-e5aa-4b18-9975-5197d242f130", UserName = "Peter Jensen"};
      var mockAspUsers = new Mock<DbSet<AspNetUser>>();
      aspnetusers.Add(self);
      var qasp = aspnetusers.AsQueryable();
      mockAspUsers.As<IQueryable<AspNetUser>>().Setup(usr => usr.Provider).Returns(qasp.Provider);
      mockAspUsers.As<IQueryable<AspNetUser>>().Setup(usr => usr.Expression).Returns(qasp.Expression);
      mockAspUsers.As<IQueryable<AspNetUser>>().Setup(usr => usr.ElementType).Returns(qasp.ElementType);
      mockAspUsers.Setup(c => c.Add(It.IsAny<AspNetUser>()))
        .Returns<AspNetUser>(arg => arg)
        .Callback<AspNetUser>(s => aspnetusers.Add(s));

      gcMock.Setup(c => c.AspNetUsers).Returns(mockAspUsers.Object);

     
      ps1 = new PlaySession
      {
        Id = 18,
        DatePlayed = new DateTime(2017, 11, 15, 9, 30, 00),
        BoardGameId = 5,
        PlayTime = TimeSpan.Zero
      };
      ps2 = new PlaySession
      {
        Id = 19,
        DatePlayed = new DateTime(2017, 11, 24, 16, 20, 00),
        BoardGameId = 1,
        PlayTime = TimeSpan.Zero
      };
      ps3 = new PlaySession
      {
        Id = 20,
        DatePlayed = new DateTime(2017, 11, 17, 16, 20, 00),
        BoardGameId = 28,
        PlayTime = TimeSpan.Zero
      };
      ps4 = new PlaySession
      {
        Id = 21,
        DatePlayed = new DateTime(2017, 11, 24, 16, 20, 00),
        BoardGameId = 1,
        PlayTime = TimeSpan.Zero
      };
      ps5 = new PlaySession
      {
        Id = 22,
        DatePlayed = new DateTime(2017, 11, 17, 16, 20, 00),
        BoardGameId = 28,
        PlayTime = TimeSpan.Zero
      };

      playsessions.Add(ps1);
      playsessions.Add(ps2);
      playsessions.Add(ps3);
      playsessions.Add(ps4);
      playsessions.Add(ps5);

      var mockPS = new Mock<DbSet<PlaySession>>();
      var qps = playsessions.AsQueryable();
      mockPS.As<IQueryable<PlaySession>>().Setup(m => m.Provider).Returns(qps.Provider);
      mockPS.As<IQueryable<PlaySession>>().Setup(m => m.Expression).Returns(qps.Expression);
      mockPS.As<IQueryable<PlaySession>>().Setup(m => m.ElementType).Returns(qps.ElementType);
      mockPS.Setup(c => c.Add(It.IsAny<PlaySession>()))
        .Returns<PlaySession>(arg => arg)
        .Callback<PlaySession>(s => playsessions.Add(s));
      //mockPS.As<IQueryable<PlaySession>>().Setup(m => m.GetEnumerator()).Returns(0 => qps.GetEnumerator());
      gcMock.Setup(c => c.PlaySessions).Returns(mockPS.Object);
    }

    [TearDown]
    public void TearDown()
    {
      p = null;
    }

    private PlayerController p;
    private Mock<GameContext> gcMock { get; set; }
    public AspNetUser self;
    public PlaySession ps1;
    public PlaySession ps2;
    public PlaySession ps3;
    public PlaySession ps4;
    public PlaySession ps5;
    public Library lib1;
    public Library lib2;
    public Library lib3;
    public Player will;
    public Player peter;
    public Player Jen;
    public BoardGame gloomHaven;
    public BoardGame catan;
    public BoardGame bloodrage;
    public BoardGame williamsburg;
    public Participant par1;
    public Participant par2;
    public Participant par3;
    public Participant par4;
    public Participant par5;
    public Participant par6;
    public Participant par7;
    public Participant par8;

    public List<PlaySession> playsessions = new List<PlaySession>();

    public List<Player> players = new List<Player>();

    public List<BoardGame> boardgames = new List<BoardGame>();

    public List<Participant> participants = new List<Participant>();

    public List<Library> libraries = new List<Library>();

    public List<AspNetUser> aspnetusers = new List<AspNetUser>();

    [TestCase(1, true)]
    [TestCase(99, false)]
    public void testIsOwned(int id, bool actual)
    {
      Assert.AreEqual(actual, p.IsOwned(id));
    }

    [TestCase(5, true)]
    [TestCase(9, false)]
    public void DeleteGamesTest(int id, bool actual)
    {
      var value = libraries.FirstOrDefault(x => x.Id == id);
      var testLib = new List<Library>();
      var wasRemoved = false;
      testLib.Add(lib1);
      testLib.Add(lib2);
      testLib.Add(lib3);
      if (value != null)
        if (libraries.Any(x => x.Id == value.Id))
        {
          wasRemoved = true;
          testLib.Remove(value);
        }
      var results = p.DeleteGame(id);
      Assert.AreEqual(actual, results);
      if (wasRemoved)
        libraries.Add(value);
    }

    [TestCase(1, true)]
    [TestCase(5, true)]
    [TestCase(4, false)]
    public void FavoriteGameTest(int GameId, bool actual)
    {
      var testLib = new Library();
      if (libraries.Any(x => x.Id == GameId))
        testLib = libraries.FirstOrDefault(x => x.userId == peter.AspNetUser_Id && x.bgId == GameId);
      var result = p.FavoriteGame(GameId);
      Assert.AreEqual(actual, result);
      if (testLib.userId != null)
        Assert.AreEqual(testLib.favorite,
          libraries.FirstOrDefault(x => x.userId == peter.AspNetUser_Id && x.bgId == GameId).favorite);
    }

    [Ignore("to be used for uploading profile pic")]
    [TestCase("abcde")]
    public void CorrectFilesTest(string FileName)
    {
      Assert.IsTrue(p.CheckIfImageFile(FileName));
    }

    [Test]
    public void BoardgameDataTest()
    {
      var sample = "<boardgames termsofuse=\"http://boardgamegeek.com/xmlapi/termsofuse\">\n\t\t\t<boardgame objectid=\"209685\">\n\t\t\t<name primary=\"true\">Century: Spice Road</name>\t\t\t\n\t\t\t\t\t\t\t<yearpublished>2017</yearpublished>\n\t\t\t\t\t</boardgame>\n\t\t\t<boardgame objectid=\"233621\">\n\t\t\t<name primary=\"true\">Century: Spice Road – Bonus Cards</name>\t\t\t\n\t\t\t\t\t\t\t<yearpublished>2017</yearpublished>\n\t\t\t\t\t</boardgame>\n\t</boardgames>\n";
      var mockInterface = new Mock<webInterface>();
      mockInterface.Setup(m => m.StreamReadXml(It.IsAny<Uri>())).Returns(sample);
      p.requester = mockInterface.Object;

      var actual = p.BoardgameData("13");
      var expected =
        "{\"boardgames\":{\"termsofuse\":\"http://boardgamegeek.com/xmlapi/termsofuse\",\"_value\":[{\"boardgame\":{\"objectid\":\"209685\",\"_value\":[{\"name\":{\"primary\":\"true\",\"_value\":\"Century: Spice Road\"}},{\"yearpublished\":{\"_value\":\"2017\"}}]}},{\"boardgame\":{\"objectid\":\"233621\",\"_value\":[{\"name\":{\"primary\":\"true\",\"_value\":\"Century: Spice Road – Bonus Cards\"}},{\"yearpublished\":{\"_value\":\"2017\"}}]}}]}}";
      Assert.AreEqual(expected, actual);
      //Assert.AreEqual(bgResults, ExpectedResults);
    }

    [Test]
    public void FindBoardgameBGGTest()
    {
      var sample = "<boardgames termsofuse=\"http://boardgamegeek.com/xmlapi/termsofuse\">\n\t\t\t<boardgame objectid=\"209685\">\n\t\t\t<name primary=\"true\">Century: Spice Road</name>\t\t\t\n\t\t\t\t\t\t\t<yearpublished>2017</yearpublished>\n\t\t\t\t\t</boardgame>\n\t\t\t<boardgame objectid=\"233621\">\n\t\t\t<name primary=\"true\">Century: Spice Road – Bonus Cards</name>\t\t\t\n\t\t\t\t\t\t\t<yearpublished>2017</yearpublished>\n\t\t\t\t\t</boardgame>\n\t</boardgames>\n";
      var mockInterface = new Mock<webInterface>();
      var fakeXML = new Xml();
      var expected =
        "{\"boardgames\":{\"termsofuse\":\"http://boardgamegeek.com/xmlapi/termsofuse\",\"_value\":[{\"boardgame\":{\"objectid\":\"209685\",\"_value\":[{\"name\":{\"primary\":\"true\",\"_value\":\"Century: Spice Road\"}},{\"yearpublished\":{\"_value\":\"2017\"}}]}},{\"boardgame\":{\"objectid\":\"233621\",\"_value\":[{\"name\":{\"primary\":\"true\",\"_value\":\"Century: Spice Road – Bonus Cards\"}},{\"yearpublished\":{\"_value\":\"2017\"}}]}}]}}";

      fakeXML.DocumentContent= sample;
      mockInterface.Setup(m => m.StreamReadXml(It.IsAny<Uri>())).Returns(sample);
      p.requester = mockInterface.Object;
      var actual = p.FindBoardgame("catan");
      Assert.AreEqual(expected, actual);
    }


    [Test]
    public void AddingGameToLibraryAndBoardgamesTest()
    {
      boardgames.Clear();
      boardgames.Add(bloodrage);
      boardgames.Add(catan);
      boardgames.Add(gloomHaven);
      boardgames.Add(williamsburg);
      libraries.Clear();
      libraries.Add(lib1);
      libraries.Add(lib2);
      libraries.Add(lib3);
      var newBoardGame = new BoardGame
      {
        Id = 7,
        BGGID = 45,
        Category = "Strategy",
        Description = "you gonna get real mad",
        Image = "none",
        MaxPlayer = 4,
        MinPlayer = 1,
        Name = "photosynthesis",
        PlayingTime = 10,
        TotalPlays = 0
      };
      var testBoardGames = new List<BoardGame>();
      testBoardGames.Add(bloodrage);
      testBoardGames.Add(catan);
      testBoardGames.Add(gloomHaven);
      testBoardGames.Add(williamsburg);
      testBoardGames.Add(newBoardGame);
      var newLibrary = new Library
      {
        AspNetUser = null,
        Id = 0,
        userId = "eb218371-e5aa-4b18-9975-5197d242f130",
        bgId = 7,
        favorite = null
      };
      var TestLibrary = new List<Library>();
      TestLibrary.Add(lib1);
      TestLibrary.Add(lib2);
      TestLibrary.Add(lib3);
      TestLibrary.Add(newLibrary);
      p.AddGameToLibrary(newBoardGame);
      Assert.AreEqual(testBoardGames, boardgames);
      var newlibLikeness = new Likeness<List<Library>, List<Library>>(TestLibrary);
      var libEntryLikeness = new Likeness<Library, Library>(newLibrary);
      Assert.AreEqual(libEntryLikeness, libraries[3]);
      boardgames.Remove(newBoardGame);
      libraries.Remove(newLibrary);
    }

    [Test]
    public void AddingGameToLibraryButExsistsAsBGTest()
    {
      boardgames.Clear();
      boardgames.Add(bloodrage);
      boardgames.Add(catan);
      boardgames.Add(gloomHaven);
      boardgames.Add(williamsburg);
      libraries.Clear();
      libraries.Add(lib1);
      libraries.Add(lib2);
      libraries.Add(lib3);
      var testBoardGames = new List<BoardGame>();
      testBoardGames.Add(bloodrage);
      testBoardGames.Add(catan);
      testBoardGames.Add(gloomHaven);
      testBoardGames.Add(williamsburg);
      var newLibrary = new Library
      {
        AspNetUser = null,
        Id = 0,
        userId = "eb218371-e5aa-4b18-9975-5197d242f130",
        bgId = williamsburg.Id,
        favorite = null
      };
      var TestLibrary = new List<Library>();
      TestLibrary.Add(lib1);
      TestLibrary.Add(lib2);
      TestLibrary.Add(lib3);
      TestLibrary.Add(newLibrary);
      p.AddGameToLibrary(williamsburg);
      var testsLikeness = new Likeness<List<BoardGame>, List<BoardGame>>(testBoardGames);
      Assert.AreEqual(testBoardGames, boardgames);
      var newlibLikeness = new Likeness<Library, Library>(TestLibrary[3]);
      Assert.AreEqual(newlibLikeness, libraries[3]);
      libraries.Remove(newLibrary);
    }

    [Test]
    public void BGDoesntExsistTest()
    {
      var result = p.BG(99) as JsonResult;
      Assert.AreEqual(null, result.Data);
    }

    [Test]
    public void BGResultSearchTest()
    {
      boardgames.Clear();
      boardgames.Add(bloodrage);
      boardgames.Add(catan);
      boardgames.Add(gloomHaven);
      boardgames.Add(williamsburg);
      var TestUserList = new List<Player>();
      var testBoardGameList = new List<BoardGame>();
      testBoardGameList.Add(gloomHaven);
      var result = p.BoardGamesSearch("Glo");
      var ActualLikeness = new Likeness<List<BoardGame>, List<BoardGame>>(testBoardGameList);
      Assert.AreEqual(testBoardGameList, result);
    }

    [Test]
    public void FailsAddingGameToLibraryTest()
    {
      boardgames.Clear();
      boardgames.Add(bloodrage);
      boardgames.Add(catan);
      boardgames.Add(gloomHaven);
      libraries.Clear();
      libraries.Add(lib1);
      libraries.Add(lib2);
      libraries.Add(lib3);
      var testBoardGames = new List<BoardGame>();
      testBoardGames.Add(bloodrage);
      testBoardGames.Add(catan);
      testBoardGames.Add(gloomHaven);
      var TestLibrary = new List<Library>();
      TestLibrary.Add(lib1);
      TestLibrary.Add(lib2);
      TestLibrary.Add(lib3);
      p.AddGameToLibrary(catan);
      Assert.AreEqual(testBoardGames, boardgames);
      Assert.AreEqual(TestLibrary, libraries);
    }

    [Test]
    public void FindTest()
    {
      var actual = new PlayerDto
      {
        Id = will.Id,
        AspNetUser_Id = will.AspNetUser_Id,
        Name = will.Name
      };
      var actualBG = new List<BoardGame>();
      actualBG.Add(gloomHaven);

      actual.PlayersGames = actualBG;

      var LikenessBg = actual.AsSource().OfLikeness<PlayerDto>();
      var SteralizedBG = new Likeness<PlayerDto, object>(actual);
      var results = p.Find(3) as JsonResult;
      Assert.AreEqual(SteralizedBG, results.Data);
    }

    [Test]
    public void PSearchResultsTest()
    {
      //you may need to reset all values
      players.Clear();
      players.Add(peter);
      players.Add(Jen);
      players.Add(will);
      var TestUserList = new List<playerSearch>();
      var willSearch = new playerSearch(will.Id, will.Name);
        
      TestUserList.Add(willSearch);
      var ActualResults = new Likeness<List<playerSearch>, List<playerSearch>>(TestUserList);

      var result = p.PSearchResults("Will Munn");
      Assert.AreEqual(ActualResults, result);
    }

    [Test]
    public void SearchBGTest()
    {
      var result = p.BG(catan.Id) as JsonResult;
      var cLikeness = new Likeness<BoardGame, BoardGame>(catan);
      Assert.AreEqual(cLikeness, result.Data);
    }

    [Test]
    public void SelfTest()
    {
      var actual = new PlayerDto
      {
        Id = 1,
        AspNetUser_Id = "eb218371-e5aa-4b18-9975-5197d242f130",
        Name = "Peter Jensen"
      };
      var actualBG = new List<BoardGame>();
      actualBG.Add(catan);
      actualBG.Add(bloodrage);

      actual.PlayersGames = actualBG;

      var LikenessBg = actual.AsSource().OfLikeness<PlayerDto>();
      var SteralizedBG = new Likeness<PlayerDto, object>(actual);
      var results = p.Self() as JsonResult;
      Assert.AreEqual(SteralizedBG, results.Data);
    }

    [Test]
    public void TwoUserResultSearchTest()
    {
      players.Clear();
      players.Add(peter);
      players.Add(Jen);
      players.Add(will);
      var TestUserList = new List<playerSearch>();
      var peterSearch = new playerSearch(peter.Id, peter.Name);
     
      TestUserList.Add(peterSearch);
      var jenSearch = new playerSearch(Jen.Id, Jen.Name);
      TestUserList.Add(jenSearch);
      var ActualResults = new Likeness<List<playerSearch>, List<playerSearch>>(TestUserList);
      var result = p.PSearchResults("Jen");
      Assert.AreEqual(ActualResults, result);
    }

    [Test]
    public void GetPlaySessionsTest()
    {
      players.Clear();
      players.Add(peter);
      players.Add(Jen);
      players.Add(will);
      playsessions.Clear();
      playsessions.Clear();
      playsessions.Add(ps1);
      playsessions.Add(ps2);
      playsessions.Add(ps3);
      playsessions.Add(ps4);
      playsessions.Add(ps5);
      List<PlaySession> playsessionsResults = new List<PlaySession>();
      playsessionsResults.Add(ps1);
      playsessionsResults.Add(ps2);
      playsessionsResults.Add(ps3);
      playsessionsResults.Add(ps4);
      var actual = p.GetPlaySessionsByPlayer(1);
      Assert.AreEqual(playsessionsResults, actual);
    }

    [Test]
    public void ExtractPlaySessionsDataTest()
    {
      playsessions.Clear();
      ps1.BoardGame = catan;
      playsessions.Add(ps1);
      participants.Clear();
      participants.Add(par1);
      participants.Add(par2);
      players.Clear();
      players.Add(peter);
      players.Add(will);
      List<playerSearch> newParticipants = new List<playerSearch>();
      var playerSearch1 = new playerSearch(peter.Id,peter.Name);
      playerSearch1.Winner = true;
      playerSearch1.Team = 1;
      var playerSearch2 = new playerSearch(will.Id, will.Name);
      playerSearch2.Winner = true;
      playerSearch2.Team = 2;
      newParticipants.Add(playerSearch1);
      newParticipants.Add(playerSearch2);
      
      List<PlaySessionData> testPSD = new List<PlaySessionData>();
      var sample = new PlaySessionData(catan, newParticipants, TimeSpan.Zero, new DateTime(2017, 11, 15, 9, 30, 00).ToString());
      List<playerSearch> team1Members = new List<playerSearch>();
      team1Members.Add(playerSearch1);
      var team1 = new Team();
      team1.Name = 1;
      team1.Name = 1;
      team1.TeamMates = team1Members;
      team1.Winner = true;
      sample.Teams = new List<Team>();
      sample.Teams.Add(team1);
      List<playerSearch> team2Members = new List<playerSearch>();
      team2Members.Add(playerSearch2);
      var team2 = new Team();
      team2.Name = 2;
      team2.TeamMates = team2Members;
      team2.Winner = false;
      sample.Teams.Add(team2);
      testPSD.Add(sample);
      //heres the method
      List<PlaySessionData> returned = p.ExtractPlaySessionsData(playsessions, playerSearch1);
      //testPSD.ForEach(TestContext.WriteLine);
      TestContext.WriteLine(testPSD[0].BoardGame.Name.ToString());
      //returned.ForEach(TestContext.WriteLine);
      TestContext.WriteLine(returned[0].BoardGame.Name.ToString());
      var testLikeness = new Likeness<List<PlaySessionData>,List<PlaySessionData>>(testPSD);
      Assert.AreEqual(testLikeness, returned);  
    }

    [Test]
    public void ParsePlayersIntoTeams()
    {
      playsessions.Clear();
      ps1.BoardGame = catan;
      playsessions.Add(ps1);
      participants.Clear();
      participants.Add(par1);
      participants.Add(par2);
      players.Clear();
      players.Add(peter);
      players.Add(will);
      var playerSearch1 = new playerSearch(peter.Id, peter.Name);
      playerSearch1.Winner = true;
      playerSearch1.Team = 1;
      List<Team> actual = new List<Team>();
      Team sampleTeam = new Team();
      sampleTeam.Name = 1;
      sampleTeam.Winner = true;
      sampleTeam.TeamMates = new List<playerSearch>();
      sampleTeam.TeamMates.Add(playerSearch1);
      List<Team> TestTeamsList = new List<Team>();
      p.ParsePlayersIntoTeams(playerSearch1, TestTeamsList);
      List<Team> sampleTeamsList = new List<Team>();
      sampleTeamsList.Add(sampleTeam);
      var Sample = new Likeness<List<Team>, List<Team>>(sampleTeamsList);
      playsessions.Clear();
      Assert.AreEqual(Sample, TestTeamsList);
    }
    
    [Test]
    public void GetPlayerSearchTest()
    {
      players.Clear();
      players.Add(peter);
      players.Add(will);
      participants.Clear();
      participants.Add(par1);
      participants.Add(par2);
      List<playerSearch> actual = new List<playerSearch>();
      foreach (var player in players)
      {
        actual.Add(new playerSearch(player.Id,player.Name));
      }
      var results = p.GetPlayersSearch(participants);
      var likenessResults = new Likeness<List<playerSearch>, List<playerSearch>>(results);
      Assert.AreEqual(actual, likenessResults);
    }

    [Test]
    public void GetPlayerStatsTest()
    {
      p.GetPlayerStats(2);
    }
  }
}