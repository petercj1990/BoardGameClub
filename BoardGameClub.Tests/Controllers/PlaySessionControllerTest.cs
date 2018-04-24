using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BoardGameClub.Controllers;
using BoardGameClubEntities;
using Moq;
using NUnit.Framework;
using SemanticComparison;

namespace BoardGameClub.Tests.Controllers
{
  [TestFixture]
  public class PlaySessionControllerTest
  {
    public PlaySessionController ps;
    private Mock<GameContext> gcMock;


    public PlaySession ps1;
    public PlaySession ps2;
    public PlaySession ps3;
    public Library lib1;
    public Library lib2;
    public Player will;
    public Player peter;
    public Player Jen;
    public BoardGame gloomHaven;
    public BoardGame catan;
    public BoardGame bloodrage;
    public Participant par1;
    public Participant par2;
    public Participant par3;
    public Participant par4;
    public Participant par5;
    public Participant par6;

    public List<PlaySession> playsessions = new List<PlaySession>();
    public List<Player> players = new List<Player>();
    public List<BoardGame> boardgames = new List<BoardGame>();
    public List<Participant> participants = new List<Participant>();
    public List<Library> libraries = new List<Library>();


    [SetUp]
    public void SetUp()
    {

      gcMock = new Mock<GameContext>();
      ps = new PlaySessionController
      {
        GetUserId = () => "eb218371-e5aa-4b18-9975-5197d242f130"
      };
      ps.db = gcMock.Object;
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

      playsessions.Add(ps1);
      playsessions.Add(ps2);
      playsessions.Add(ps3);
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


      will = new Player {Name = "Will Munn", Id = 3, AspNetUser_Id = "5fc4ced5-8f00-46ad-910e-2d3a38be9e59"};
      peter = new Player {Name = "Peter Jensen", Id = 1, AspNetUser_Id = "eb218371-e5aa-4b18-9975-5197d242f130"};
      Jen = new Player {Name = "Jenna Maroni", Id = 5, AspNetUser_Id = "im_a_test_Id"};
      players.Add(peter);
      players.Add(will);
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
        Category = "Negotiation"
      };

      boardgames.Add(bloodrage);
      boardgames.Add(catan);
      boardgames.Add(gloomHaven);
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

      par1 = new Participant {Id = 1, Player_Id = 1, PlaySession_Id = 18};
      par2 = new Participant {Id = 2, Player_Id = 3, PlaySession_Id = 18};
      par3 = new Participant {Id = 3, Player_Id = 1, PlaySession_Id = 19};
      par4 = new Participant {Id = 4, Player_Id = 3, PlaySession_Id = 19};
      par5 = new Participant {Id = 5, Player_Id = 1, PlaySession_Id = 20};
      par6 = new Participant {Id = 6, Player_Id = 3, PlaySession_Id = 20};
      //par7 = new Participant { Id = 7, Player_Id = 1, PlaySession_Id = 21 };
      //par8 = new Participant { Id = 7, Player_Id = 1, PlaySession_Id = 21 };

      participants.Add(par1);
      participants.Add(par2);
      participants.Add(par3);
      participants.Add(par4);
      participants.Add(par5);
      participants.Add(par6);

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

      lib1 = new Library {Id = 1, userId = "eb218371-e5aa-4b18-9975-5197d242f130", bgId = 1};
      lib2 = new Library {Id = 1, userId = "eb218371-e5aa-4b18-9975-5197d242f130", bgId = 5};
      var mockLib = new Mock<DbSet<Library>>();
      libraries.Add(lib1);
      libraries.Add(lib2);
      var qlib = libraries.AsQueryable();
      mockLib.As<IQueryable<Library>>().Setup(lib => lib.Provider).Returns(qlib.Provider);
      mockLib.As<IQueryable<Library>>().Setup(m => m.Expression).Returns(qlib.Expression);
      mockLib.As<IQueryable<Library>>().Setup(m => m.ElementType).Returns(qlib.ElementType);
      mockLib.Setup(c => c.Add(It.IsAny<Library>()))
        .Returns<Library>(arg => arg)
        .Callback<Library>(s => libraries.Add(s));
      //mockPS.As<IQueryable<PlaySession>>().Setup(m => m.GetEnumerator()).Returns(0 => qps.GetEnumerator());
      gcMock.Setup(c => c.Libraries).Returns(mockLib.Object);
    }

    [TearDown]
    public void TearDown()
    {
      ps = null;
    }



    [TestCase("Pet", "Peter Jensen")]
    [TestCase("Will M", "Will Munn")]
    [TestCase("Jen", "Jenna Maroni", "Peter Jensen")]
    [TestCase("notarealperson")]
    public void SearchTest(string searchString, params string[] actual)
    {
      var result = ps.PlayerSearch(searchString) as JsonResult;
      var searchObject = result;
      var testList = new List<playerSearch>();
      foreach (var name in actual)
      foreach (var player in players)
        if (player.Name == name)
        {
          var actualAsSearch = new playerSearch(player.Id, player.Name);
          testList.Add(actualAsSearch);
        }
      var SerchLikeness = new Likeness<List<playerSearch>, List<playerSearch>>(testList);

      Assert.AreEqual(SerchLikeness, result.Data);
      result = null;
    }

    [Test]
    public void SelfTest()
    {
      var results = ps.Self() as JsonResult;
      var peterdto = new PlayerDto();
      peterdto.Id = peter.Id;
      peterdto.AspNetUser_Id = peter.AspNetUser_Id;
      peterdto.Name = peter.Name;
      var peterLikeness = new Likeness<PlayerDto, PlayerDto>(peterdto);
      Assert.AreEqual(peterLikeness, results.Data);
    }

    [Test]
    public void AddPlaySessionTest()
    {
      var playSession = new PlaySession();
      playSession.Id = 22;
      playSession.BoardGame = catan;
      playSession.BoardGameId = 1;
      playSession.PlayTime = TimeSpan.Zero;
      var curr = DateTime.Now;
      playSession.DatePlayed = curr;
      var PlayerIds = new List<int>();
      PlayerIds.Add(1);
      var results = ps.AddPlaySession(playSession, PlayerIds);
      //Assert.AreEqual(new HttpStatusCodeResult(HttpStatusCode.Accepted, "playsessionCreated"), results );
      var finalPlaySession = new Likeness<PlaySession, PlaySession>(playSession);
      Assert.AreEqual(1, participants.Where(x => x.PlaySession_Id == 22).ToList().Count());
      Assert.AreEqual(playsessions.FirstOrDefault(x => x.Id == 22), playSession);
      playsessions.Remove(playSession);
    }

    [Test]
    public void BGcollectionTest()
    {
      boardgames.Clear();
      boardgames.Add(catan);
      boardgames.Add(bloodrage);
      boardgames.Add(gloomHaven);
      libraries.Clear();
      libraries.Add(lib1);
      libraries.Add(lib2);
      var ExpectedGames = new List<BoardGame>();
      ExpectedGames.Add(catan);
      ExpectedGames.Add(bloodrage);
      var results = ps.BGcollection() as JsonResult;
      Assert.AreEqual(ExpectedGames, results.Data);
    }

    [Test]
    public void GetPlaySessionsTest()
    {
      playsessions.Clear();
      playsessions.Add(ps1);
      playsessions.Add(ps2);
      playsessions.Add(ps3);
      var trueValue = new List<PlaySessionData>();
      var playerSet = new List<playerSearch>();
      playerSet.Add(new playerSearch(1, "Peter Jensen"));
      playerSet.Add(new playerSearch(3, "Will Munn"));

      var final = new Likeness<List<PlaySessionData>, List<PlaySessionData>>(trueValue);
      var results = ps.getplaySessions() as JsonResult;
      Assert.AreEqual(final, results.Data);
    }
  }
}