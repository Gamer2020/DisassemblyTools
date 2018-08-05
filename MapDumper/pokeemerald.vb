Module pokeemerald
    Public Enum EM_Songs
        MUS_DUMMY = 0
        SE_KAIFUKU = 1 'Healing = Item
        SE_PC_LOGIN = 2 'PC = Logon
        SE_PC_OFF = 3 'PC = Shutdown
        SE_PC_ON = 4 'PC = Startup
        SE_SELECT = 5 'Cursor = Selection
        SE_WIN_OPEN = 6 'Start = Menu
        SE_WALL_HIT = 7 'Wall = Bump
        SE_DOOR = 8 'Opening = Door
        SE_KAIDAN = 9 'Stairs
        SE_DANSA = 10 'Ledge
        SE_JITENSYA = 11 'Bicycle = Bell
        SE_KOUKA_L = 12 'Not = Very = Effective
        SE_KOUKA_M = 13 'Normal = Effectiveness
        SE_KOUKA_H = 14 'Super = Effective
        SE_BOWA2 = 15 'Pokémon = Withdrawal
        SE_POKE_DEAD = 16 'Pokémon = Fainted
        SE_NIGERU = 17 'Flee = from = Wild = Battle
        SE_JIDO_DOA = 18 'Pokémon = Center = Door
        SE_NAMINORI = 19 'Briney 's = Ship
        SE_BAN = 20 'Bang
        SE_PIN = 21 'Exclamation = Bubble
        SE_BOO = 22 'Contest = Jam
        SE_BOWA = 23 'Giving = Poké = Ball = to = Nurse, = Poké = Ball = Wiggle
        SE_JYUNI = 24 'Places = in = Contest = Appearing
        SE_A = 25 'Bard = A
        SE_I = 26 'Bard = I
        SE_U = 27 'Bard = U
        SE_E = 28 'Bard = E
        SE_O = 29 'Bard = O
        SE_N = 30 'Bard = N
        SE_SEIKAI = 31 'Success
        SE_HAZURE = 32 'Failure
        SE_EXP = 33 'Exp. = Bar
        SE_JITE_PYOKO = 34 'Bunny = Hop
        SE_MU_PACHI = 35
        SE_TK_KASYA = 36 'Mossdeep = Gym/Trick = House = Switch
        SE_FU_ZAKU = 37
        SE_FU_ZAKU2 = 38
        SE_FU_ZUZUZU = 39 'Lavaridge = Gym = Warp
        SE_RU_GASHIN = 40 'Sootopolis = Gym = - = Stairs = Appear
        SE_RU_GASYAN = 41 'Sootopolis = Gym = - = Ice = Breaking
        SE_RU_BARI = 42 'Sootopolis = Gym = - = Walking = on = Ice
        SE_RU_HYUU = 43 'Falling = Down
        SE_KI_GASYAN = 44
        SE_TK_WARPIN = 45 'Warp = In
        SE_TK_WARPOUT = 46 'Warp = Out
        SE_TU_SAA = 47 'Repel
        SE_HI_TURUN = 48 'Moving = Obstacle = in = Fortree = Gym
        SE_TRACK_MOVE = 49 'Moving = Truck
        SE_TRACK_STOP = 50 'Moving = Truck = Stop
        SE_TRACK_HAIKI = 51 'Moving = Truck = Unload
        SE_TRACK_DOOR = 52 'Moving = Truck = Door
        SE_MOTER = 53
        SE_CARD = 54
        SE_SAVE = 55 'Save
        SE_KON = 56 'Poké = Ball = Bounce = 1
        SE_KON2 = 57 'Poké = Ball = Bounce = 2
        SE_KON3 = 58 'Poké = Ball = Bounce = 3
        SE_KON4 = 59 'Poké = Ball = Bounce = 4
        SE_SUIKOMU = 60 'Poké = Ball = Trade
        SE_NAGERU = 61 'Poké = Ball = Throw
        SE_TOY_C = 62 'Note = C
        SE_TOY_D = 63 'Note = D
        SE_TOY_E = 64 'Note = E
        SE_TOY_F = 65 'Note = F
        SE_TOY_G = 66 'Note = G
        SE_TOY_A = 67 'Note = A
        SE_TOY_B = 68 'Note = B
        SE_TOY_C1 = 69 'Note = High = C
        SE_MIZU = 70 'Puddle
        SE_HASHI = 71 'Boardwalk
        SE_DAUGI = 72 'Slots = Credits
        SE_PINPON = 73 'Ding-dong!
        SE_FUUSEN1 = 74 'Red = Balloon
        SE_FUUSEN2 = 75 'Blue = Balloon
        SE_FUUSEN3 = 76 'Yellow = Balloon
        SE_TOY_KABE = 77 'Breakable = Door
        SE_TOY_DANGO = 78 'Mud = Ball
        SE_DOKU = 79 'Overworld = Poison = Damage
        SE_ESUKA = 80 'Escalator
        SE_T_AME = 81 'Rain
        SE_T_AME_E = 82 'Rain = Stop
        SE_T_OOAME = 83 'Heavy = Rain
        SE_T_OOAME_E = 84 'Heavy = Rain = Stop
        SE_T_KOAME = 85 'Light = Rain
        SE_T_KOAME_E = 86 'Light = Rain = Stop
        SE_T_KAMI = 87 'Thunder
        SE_T_KAMI2 = 88 'Thunder = 2
        SE_ELEBETA = 89 'Elevator
        SE_HINSI = 90 'Low = Health
        SE_EXPMAX = 91 'Exp. = Max
        SE_TAMAKORO = 92 'Roulette = Ball
        SE_TAMAKORO_E = 93 'Roulette = Ball = 2
        SE_BASABASA = 94
        SE_REGI = 95 'Cash = Register
        SE_C_GAJI = 96 'Contest = Hearts
        SE_C_MAKU_U = 97 'Contest = Curtain = rise
        SE_C_MAKU_D = 98 'Contest = Curtain = fall
        SE_C_PASI = 99
        SE_C_SYU = 100
        SE_C_PIKON = 101 'Pokémon = Appears = in = Contest
        SE_REAPOKE = 102 'Shiny = Pokémon
        SE_OP_BASYU = 103 'Opening = Movie = -> = Title = Screen = whoosh
        SE_BT_START = 104 'Battle = Mugshot = whoosh
        SE_DENDOU = 105 'Audience = Cheering
        SE_JIHANKI = 106 'Vending = Machine
        SE_TAMA = 107 'Orb = Used
        SE_Z_SCROLL = 108 'Pokédex = Scrolling
        SE_Z_PAGE = 109 'Pokédex = Page
        SE_PN_ON = 110 'PokéNav = On
        SE_PN_OFF = 111 'PokéNav = Off
        SE_Z_SEARCH = 112 'Pokédex = Search
        SE_TAMAGO = 113 'Egg = hatch
        SE_TB_START = 114 'Battle = - = Poké = Ball = Tray = slide = in
        SE_TB_KON = 115 'Battle = - = Poké = Ball = Tray = ball = sound
        SE_TB_KARA = 116 'Battle = - = Poké = Ball = Tray = slide = out
        SE_BIDORO = 117
        SE_W085 = 118 'Thunderbolt
        SE_W085B = 119 'Thunderbolt = 2
        SE_W231 = 120 'Harden
        SE_W171 = 121 'Nightmare
        SE_W233 = 122 'Vital = Throw
        SE_W233B = 123 'Vital = Throw = 2
        SE_W145 = 124 'Bubble
        SE_W145B = 125 'Bubble = 2
        SE_W145C = 126 'Bubble = 3
        SE_W240 = 127 'Rain = Dance
        SE_W015 = 128 'Cut
        SE_W081 = 129 'String = Shot
        SE_W081B = 130 'String = Shot = 2
        SE_W088 = 131 'Rock = Throw
        SE_W016 = 132 'Gust
        SE_W016B = 133 'Gust = 2
        SE_W003 = 134 'DoubleSlap
        SE_W104 = 135 'Double = Team
        SE_W013 = 136 'Razor = Wind
        SE_W196 = 137 'Icy = Wind
        SE_W086 = 138 'Thunder = Wave
        SE_W004 = 139 'Comet = Punch
        SE_W025 = 140 'Mega = Kick
        SE_W025B = 141 'Mega = Kick = 2
        SE_W152 = 142 'Crabhammer
        SE_W026 = 143 'Jump = Kick
        SE_W172 = 144 'Flame = Wheel
        SE_W172B = 145 'Flame = Wheel = 2
        SE_W053 = 146 'Flamethrower
        SE_W007 = 147 'Fire = Punch
        SE_W092 = 148 'Toxic
        SE_W221 = 149 'Sacred = Fire
        SE_W221B = 150 'Sacred = Fire = 2
        SE_W052 = 151 'Ember
        SE_W036 = 152 'Take = Down
        SE_W059 = 153 'Blizzard
        SE_W059B = 154 'Blizzard = 2
        SE_W010 = 155 'Scratch
        SE_W011 = 156 'Vicegrip
        SE_W017 = 157 'Wing = Attack
        SE_W019 = 158 'Fly
        SE_W028 = 159 'Sand-Attack
        SE_W013B = 160 'Razor = Wind = 2
        SE_W044 = 161 'Bite
        SE_W029 = 162 'Headbutt
        SE_W057 = 163 'Surf
        SE_W056 = 164 'Hydro = Pump
        SE_W250 = 165 'Whirlpool
        SE_W030 = 166 'Horn = Attack
        SE_W039 = 167 'Tail = Whip
        SE_W054 = 168 'Mist
        SE_W077 = 169 'PoisonPowder
        SE_W020 = 170 'Bind
        SE_W082 = 171 'Dragon = Rage
        SE_W047 = 172 'Sing
        SE_W195 = 173 'Perish = Song
        SE_W006 = 174 'Pay = Day
        SE_W091 = 175 'Dig
        SE_W146 = 176 'Dizzy = Punch
        SE_W120 = 177 'Self-Destruct
        SE_W153 = 178 'Explosion
        SE_W071B = 179 'Absorb = 2
        SE_W071 = 180 'Absorb
        SE_W103 = 181 'Screech
        SE_W062 = 182 'BubbleBeam
        SE_W062B = 183 'BubbleBeam = 2
        SE_W048 = 184 'Supersonic
        SE_W187 = 185 'Belly = Drum
        SE_W118 = 186 'Metronome
        SE_W155 = 187 'Bonemerang
        SE_W122 = 188 'Lick
        SE_W060 = 189 'Psybeam
        SE_W185 = 190 'Faint = Attack
        SE_W014 = 191 'Swords = Dance
        SE_W043 = 192 'Leer
        SE_W207 = 193 'Swagger
        SE_W207B = 194 'Swagger = 2
        SE_W215 = 195 'Heal = Bell
        SE_W109 = 196 'Confuse = Ray
        SE_W173 = 197 'Snore
        SE_W280 = 198 'Brick = Break
        SE_W202 = 199 'Giga = Drain
        SE_W060B = 200 'Psybeam = 2
        SE_W076 = 201 'SolarBeam
        SE_W080 = 202 'Petal = Dance
        SE_W100 = 203 'Teleport
        SE_W107 = 204 'Minimize
        SE_W166 = 205 'Sketch
        SE_W129 = 206 'Swift
        SE_W115 = 207 'Reflect
        SE_W112 = 208 'Barrier
        SE_W197 = 209 'Detect
        SE_W199 = 210 'Lock-On
        SE_W236 = 211 'Moonlight
        SE_W204 = 212 'Charm
        SE_W268 = 213 'Charge
        SE_W070 = 214 'Strength
        SE_W063 = 215 'Hyper = Beam
        SE_W127 = 216 'Waterfall
        SE_W179 = 217 'Reversal
        SE_W151 = 218 'Acid = Armor
        SE_W201 = 219 'Sandstorm
        SE_W161 = 220 'Tri-Attack
        SE_W161B = 221 'Tri-Attack = 2
        SE_W227 = 222 'Encore
        SE_W227B = 223 'Encore = 2
        SE_W226 = 224 'Baton = Pass
        SE_W208 = 225 'Milk = Drink
        SE_W213 = 226 'Attract
        SE_W213B = 227 'Attract = 2
        SE_W234 = 228 'Morning = Sun
        SE_W260 = 229 'Flatter
        SE_W328 = 230 'Sand = Tomb
        SE_W320 = 231 'GrassWhistle
        SE_W255 = 232 'Spit = Up
        SE_W291 = 233 'Dive
        SE_W089 = 234 'Earthquake
        SE_W239 = 235 'Twister
        SE_W230 = 236 'Sweet = Scent
        SE_W281 = 237 'Yawn
        SE_W327 = 238 'Sky = Uppercut
        SE_W287 = 239 'Stat = Increased
        SE_W257 = 240 'Heat = Wave
        SE_W253 = 241 'Uproar
        SE_W258 = 242 'Hail
        SE_W322 = 243 'Cosmic = Power
        SE_W298 = 244 'Teeter = Dance
        SE_W287B = 245 'Stat = Decreased
        SE_W114 = 246 'Haze
        SE_W063B = 247 'Hyper = Beam = 2
        'FRLG = SFX below
        SE_RG_W_DOOR = 248 'Door
        SE_RG_CARD1 = 249 'Trainer = Card = 1
        SE_RG_CARD2 = 250 'Trainer = Card = 2
        SE_RG_CARD3 = 251 'Trainer = Card = 3
        SE_RG_BAG1 = 252 'Bag = Scroll
        SE_RG_BAG2 = 253 'Bag = Pocket = Change
        SE_RG_GETTING = 254
        SE_RG_SHOP = 255 'Cash = Register
        SE_RG_KITEKI = 256 'S.S. = Anne = Horn
        SE_RG_HELP_OP = 257 'Help = Menu = Open
        SE_RG_HELP_CL = 258 'Help = Menu = Close
        SE_RG_HELP_NG = 259 'Help = Menu = Error
        SE_RG_DEOMOV = 260 'Deoxys = Moves
        SE_RG_EXCELLENT = 261
        SE_RG_NAWAMISS = 262
        'end FRLG SFX
        SE_TOREEYE = 263 'Trainer 's = Eye = Call
        SE_TOREOFF = 264 'Trainer 's = Eye = Hang = Up
        SE_HANTEI1 = 265 'Battle = Arena = Time 's = Up = 1
        SE_HANTEI2 = 266 'Battle = Arena = Time 's = Up = 2
        SE_CURTAIN = 267 'Battle = Pike = Curtain = Open
        SE_CURTAIN1 = 268 'Battle = Pike = Curtain = Close
        SE_USSOKI = 269 'Sudowoodo
        MUS_TETSUJI = 350 'Littleroot = Town = Test 'TETSUJI '
        MUS_FIELD13 = 351 'GSC = - = Route = 38
        MUS_KACHI22 = 352 'Wild = Pokémon = Defeated
        MUS_KACHI2 = 353 'Wild = Pokémon = Defeated = with = Intro
        MUS_KACHI3 = 354 'Gym = Leader = Defeated
        MUS_KACHI5 = 355 'Victory! = Elite = Four
        MUS_PCC = 356 'Crystal = - = Pokémon = Communication = Center
        MUS_NIBI = 357 'GSC = - = Viridian/Saffron/Pewter/etc
        MUS_SUIKUN = 358 'Crystal = - = Battle! = Legendary = Beasts
        MUS_DOORO1 = 359 'Route = 101
        MUS_DOORO_X1 = 360 'Route = 110
        MUS_DOORO_X3 = 361 'Route = 120
        MUS_MACHI_S2 = 362 'Petalburg = City
        MUS_MACHI_S4 = 363 'Oldale/Lavaridge = Town
        MUS_GIM = 364 'Gym
        MUS_NAMINORI = 365 'Surfing
        MUS_DAN01 = 366 'Caves = and = Darkness
        MUS_FANFA1 = 367 'Level = Up!
        MUS_ME_ASA = 368 'Pokémon = Healed
        MUS_ME_BACHI = 369 'Obtained = a = Badge!
        MUS_FANFA4 = 370 'Obtained = an = Item!
        MUS_FANFA5 = 371 'Your = Pokémon = Just = Evolved!
        MUS_ME_WAZA = 372 'Obtained = a = TM/HM!
        MUS_BIJYUTU = 373 'Lilycove = Museum
        MUS_DOORO_X4 = 374 'Route = 122/Intro
        MUS_FUNE_KAN = 375 'Slateport = Museum
        MUS_ME_SHINKA = 376 'Evolution = Intro
        MUS_SHINKA = 377 'Evolution
        MUS_ME_WASURE = 378 'Move = Deleted/Messed = Up = Appeal
        MUS_SYOUJOEYE = 379 'Encounter! = Tuber
        MUS_BOYEYE = 380 'Encounter! = Boy
        MUS_DAN02 = 381 'Abandoned = Ship/Southern = Island
        MUS_MACHI_S3 = 382 'Fortree = City/Pacifidlog = Town
        MUS_ODAMAKI = 383 'Professor = Birch 's = Lab
        MUS_B_TOWER = 384 'Battle = Tower = (RS)
        MUS_SWIMEYE = 385 'Encounter! = Swimmer
        MUS_DAN03 = 386 'Meteor = Falls/Cave = of = Origin
        MUS_ME_KINOMI = 387 'Obtained = a = Berry!
        MUS_ME_TAMA = 388 'Awakening = the = Super-Ancient = Pokémon
        MUS_ME_B_BIG = 389 'Slots = Jackpot!
        MUS_ME_B_SMALL = 390 'Slots = Victory!
        MUS_ME_ZANNEN = 391 'Too = bad!
        MUS_BD_TIME = 392 'Roulette!
        MUS_TEST1 = 393 'Contest = Test = 1
        MUS_TEST2 = 394 'Contest = Test = 2
        MUS_TEST3 = 395 'Contest = Test = 3
        MUS_TEST4 = 396 'Contest = Test = 4
        MUS_TEST = 397 'Encounter! = Gentleman
        MUS_GOMACHI0 = 398 'Verdanturf = Town
        MUS_GOTOWN = 399 'Rustboro/Mauville/Mossdeep = City
        MUS_POKECEN = 400 'Pokémon = Center
        MUS_NEXTROAD = 401 'Route = 104
        MUS_GRANROAD = 402 'Route = 119
        MUS_CYCLING = 403 'Cycling
        MUS_FRIENDLY = 404 'Pokémart
        MUS_MISHIRO = 405 'Littleroot = Town
        MUS_TOZAN = 406 'Sky = Pillar
        MUS_GIRLEYE = 407 'Encounter! = Girl
        MUS_MINAMO = 408 'Lilycove = City
        MUS_ASHROAD = 409 'Route = 111
        MUS_EVENT0 = 410 'Help = me!
        MUS_DEEPDEEP = 411 'Underwater
        MUS_KACHI1 = 412 'Victory! = Trainer
        MUS_TITLE3 = 413 'Title = Screen
        MUS_DEMO1 = 414 'Opening = Movie
        MUS_GIRL_SUP = 415 'Encounter! = May
        MUS_HAGESHII = 416 'Encounter! = Biker
        MUS_KAKKOII = 417 'Encounter! = Electric = Trainer
        MUS_KAZANBAI = 418 'Route = 113
        MUS_AQA_0 = 419 'Encounter! = Team = Aqua
        MUS_TSURETEK = 420 'Follow = Me!
        MUS_BOY_SUP = 421 'Encounter! = Brendan
        MUS_RAINBOW = 422 'Ever = Grande = City
        MUS_AYASII = 423 'Encounter! = Psychic
        MUS_KACHI4 = 424 'Victory! = Aqua/Magma = Grunt
        MUS_ROPEWAY = 425 'Cable = Car
        MUS_CASINO = 426 'Game = Corner
        MUS_HIGHTOWN = 427 'Dewford = Town
        MUS_SAFARI = 428 'Safari = Zone
        MUS_C_ROAD = 429 'Victory = Road
        MUS_AJITO = 430 'Aqua/Magma = Hideout
        MUS_M_BOAT = 431 'Sailing
        MUS_M_DUNGON = 432 'Mt. = Pyre = (Inside)
        MUS_FINECITY = 433 'Slateport = City
        MUS_MACHUPI = 434 'Mt. = Pyre = (Outside)
        MUS_P_SCHOOL = 435 'Pokémon = Trainer 's = School
        MUS_DENDOU = 436 'You 're = the = Champion!
        MUS_TONEKUSA = 437 'Fallarbor = Town
        MUS_MABOROSI = 438 'Sealed = Chamber
        MUS_CON_FAN = 439 'Obtained = a = Contest = Ribbon!
        MUS_CONTEST0 = 440 'Pokémon = Contest
        MUS_MGM0 = 441 'Encounter! = Team = Magma
        MUS_T_BATTLE = 442 'Opening = Battle
        MUS_OOAME = 443 'The = Flood
        MUS_HIDERI = 444 'The = Drought
        MUS_RUNECITY = 445 'Sootopolis = City
        MUS_CON_K = 446 'Contest/Berry = Blending = Results
        MUS_EIKOU_R = 447 'Hall = of = Fame
        MUS_KARAKURI = 448 'Trick = House
        MUS_HUTAGO = 449 'Encounter! = Kid
        MUS_SITENNOU = 450 'Encounter! = Elite = Four
        MUS_YAMA_EYE = 451 'Encounter! = Hiker
        MUS_CONLOBBY = 452 'Contest = Lobby
        MUS_INTER_V = 453 'Encounter! = Gabby = and = Ty
        MUS_DAIGO = 454 'Encounter! = Wallace
        MUS_THANKFOR = 455 'Credits
        MUS_END = 456 'The = End
        MUS_B_FRONTIER = 457 'Battle = Frontier
        MUS_B_ARENA = 458 'Battle = Arena
        MUS_ME_POINTGET = 459 'Obtained = Battle = Points!
        MUS_ME_TORE_EYE = 460 'Registered = Trainer!
        MUS_PYRAMID = 461 'Battle = Pyramid
        MUS_PYRAMID_TOP = 462 'Top = of = the = Battle = Pyramid
        MUS_B_PALACE = 463 'Battle = Palace
        MUS_REKKUU_KOURIN = 464 'Rayquaza = Enters
        MUS_SATTOWER = 465 'Battle = Tower = (Emerald)
        MUS_ME_SYMBOLGET = 466 'Obtained = a = Frontier = Symbol!
        MUS_B_DOME = 467 'Battle = Dome
        MUS_B_TUBE = 468 'Battle = Pike
        MUS_B_FACTORY = 469 'Battle = Factory
        MUS_VS_REKKU = 470 'Battle! = Legendary = Pokémon
        MUS_VS_FRONT = 471 'Battle! = Frontier = Brain
        MUS_VS_MEW = 472 'Battle! = Mew
        MUS_B_DOME1 = 473 'Battle = Dome = Lobby
        MUS_BATTLE27 = 474 'Battle! = Wild = Pokémon
        MUS_BATTLE31 = 475 'Battle! = Team = Aqua/Magma
        MUS_BATTLE20 = 476 'Battle! = Trainer
        MUS_BATTLE32 = 477 'Battle! = Gym = Leader
        MUS_BATTLE33 = 478 'Battle! = Champion
        MUS_BATTLE36 = 479 'Battle! = Regi = Trio
        MUS_BATTLE34 = 480 'Battle! = Legendary = Pokémon = (dupe)
        MUS_BATTLE35 = 481 'Battle! = Rival
        MUS_BATTLE38 = 482 'Battle! = Elite = Four
        MUS_BATTLE30 = 483 'Battle! = Archie/Maxie
        'FRLG Music Below
        MUS_RG_ANNAI = 484 'Follow = Me!
        MUS_RG_SLOT = 485 'Game = Corner
        MUS_RG_AJITO = 486 'Rocket = Hideout
        MUS_RG_GYM = 487 'Gym
        MUS_RG_PURIN = 488 'Jigglypuff 's = Song
        MUS_RG_DEMO = 489 'Opening = Movie
        MUS_RG_TITLE = 490 'Title = Screen
        MUS_RG_GUREN = 491 'Cinnabar = Island
        MUS_RG_SHION = 492 'Lavender = Town
        MUS_RG_KAIHUKU = 493 'RBY = Pokémon = Center = Healing
        MUS_RG_CYCLING = 494 'Cycling
        MUS_RG_ROCKET = 495 'Encounter! = Team = Rocket
        MUS_RG_SHOUJO = 496 'Encounter! = Girl
        MUS_RG_SHOUNEN = 497 'Encounter! = Boy
        MUS_RG_DENDOU = 498 'You 're = the = Champion!
        MUS_RG_T_MORI = 499 'Viridian = Forest
        MUS_RG_OTSUKIMI = 500 'Mt. = Moon
        MUS_RG_POKEYASHI = 501 'Pokémon = Mansion
        MUS_RG_ENDING = 502 'Credits
        MUS_RG_LOAD01 = 503 'Route = 1
        MUS_RG_OPENING = 504 'Route = 24/Intro
        MUS_RG_LOAD02 = 505 'Route = 3
        MUS_RG_LOAD03 = 506 'Route = 11
        MUS_RG_CHAMP_R = 507 'Victory = Road/Indigo = Plateau
        MUS_RG_VS_GYM = 508 'Battle! = Gym = Leader/Elite = Four
        MUS_RG_VS_TORE = 509 'Battle! = Trainer
        MUS_RG_VS_YASEI = 510 'Battle! = Wild = Pokémon
        MUS_RG_VS_LAST = 511 'Battle! = Champion
        MUS_RG_MASARA = 512 'Pallet = Town
        MUS_RG_KENKYU = 513 'Professor = Oak 's = Lab
        MUS_RG_OHKIDO = 514 'Professor = Oak 's = Theme
        MUS_RG_POKECEN = 515 'Pokémon = Center
        MUS_RG_SANTOAN = 516 'S.S. = Anne
        MUS_RG_NAMINORI = 517 'Surfing
        MUS_RG_P_TOWER = 518 'Pokémon = Tower
        MUS_RG_SHIRUHU = 519 'Silph = Co.
        MUS_RG_HANADA = 520 'Cerulean/Fuschia = City
        MUS_RG_TAMAMUSI = 521 'Celadon = City
        MUS_RG_WIN_TRE = 522 'Victory! = Trainer
        MUS_RG_WIN_YASEI = 523 'Victory! = Wild = Pokémon
        MUS_RG_WIN_GYM = 524 'Victory! = Gym = Leader
        MUS_RG_KUCHIBA = 525 'Vermillion = City
        MUS_RG_NIBI = 526 'Viridian/Saffron/Pewter = City
        MUS_RG_RIVAL1 = 527 'Encounter! = Rival
        MUS_RG_RIVAL2 = 528 'Rival 's = Exit
        MUS_RG_FAN2 = 529 'Fanfare = 2
        MUS_RG_FAN5 = 530 'Obtained = a = Starter!
        MUS_RG_FAN6 = 531 'Pokémon = Caught! = (Used = in = Emerald)
        MUS_ME_RG_PHOTO = 532 'Trainer = Photo
        MUS_RG_TITLEROG = 533 'Game = Freak
        MUS_RG_GET_YASEI = 534 'Pokémon = Caught = Victory = Theme
        MUS_RG_SOUSA = 535 'Starting = Tutorial
        MUS_RG_SEKAIKAN = 536 'Starting = Tutorial = 2
        MUS_RG_SEIBETU = 537 'Starting = Tutorial = 3
        MUS_RG_JUMP = 538 'Jumping = Minigame
        MUS_RG_UNION = 539 'Union = Room
        MUS_RG_NETWORK = 540 'Network = Center
        MUS_RG_OKURIMONO = 541 'Mystery = Gift
        MUS_RG_KINOMIKUI = 542
        MUS_RG_NANADUNGEON = 543 'Sevii = Caves/Altering = Cave = (Mt. = Moon)
        MUS_RG_OSHIE_TV = 544 'Follow = Me!
        MUS_RG_NANASHIMA = 545 'Sevii = Islands = Routes = (Lake = of = Rage)
        MUS_RG_NANAISEKI = 546 'Sevii = Forests = (Viridian = Forest)
        MUS_RG_NANA123 = 547 'Sevii = Islands = 1-3 = (Viridian/Saffron/Pewter)
        MUS_RG_NANA45 = 548 'Sevii = Islands = 4-5 = (Azalea = Town)
        MUS_RG_NANA67 = 549 'Sevii = Islands = 6-7 = (Violet = City)
        MUS_RG_POKEFUE = 550 'Poké = Flute
        MUS_RG_VS_DEO = 551 'Battle! = Deoxys
        MUS_RG_VS_MYU2 = 552 'Battle! = Mewtwo
        MUS_RG_VS_DEN = 553 'Battle! = Legendary = Birds
        MUS_RG_EXEYE = 554 'Encounter! = Gym = Leader
        MUS_RG_DEOEYE = 555 'Encounter! = Deoxys
        MUS_RG_T_TOWER = 556 'Trainer = Tower = (Gym)
        MUS_RG_SLOWMASARA = 557 'Pallet = Town = (Hall = of = Fame = remix)
        MUS_RG_TVNOIZE = 558 'Teachy = TV
        PH_TRAP_BLEND = 559
        PH_TRAP_HELD = 560
        PH_TRAP_SOLO = 561
        PH_FACE_BLEND = 562
        PH_FACE_HELD = 563
        PH_FACE_SOLO = 564
        PH_CLOTH_BLEND = 565
        PH_CLOTH_HELD = 566
        PH_CLOTH_SOLO = 567
        PH_DRESS_BLEND = 568
        PH_DRESS_HELD = 569
        PH_DRESS_SOLO = 570
        PH_FLEECE_BLEND = 571
        PH_FLEECE_HELD = 572
        PH_FLEECE_SOLO = 573
        PH_KIT_BLEND = 574
        PH_KIT_HELD = 575
        PH_KIT_SOLO = 576
        PH_PRICE_BLEND = 577
        PH_PRICE_HELD = 578
        PH_PRICE_SOLO = 579
        PH_LOT_BLEND = 580
        PH_LOT_HELD = 581
        PH_LOT_SOLO = 582
        PH_GOAT_BLEND = 583
        PH_GOAT_HELD = 584
        PH_GOAT_SOLO = 585
        PH_THOUGHT_BLEND = 586
        PH_THOUGHT_HELD = 587
        PH_THOUGHT_SOLO = 588
        PH_CHOICE_BLEND = 589
        PH_CHOICE_HELD = 590
        PH_CHOICE_SOLO = 591
        PH_MOUTH_BLEND = 592
        PH_MOUTH_HELD = 593
        PH_MOUTH_SOLO = 594
        PH_FOOT_BLEND = 595
        PH_FOOT_HELD = 596
        PH_FOOT_SOLO = 597
        PH_GOOSE_BLEND = 598
        PH_GOOSE_HELD = 599
        PH_GOOSE_SOLO = 600
        PH_STRUT_BLEND = 601
        PH_STRUT_HELD = 602
        PH_STRUT_SOLO = 603
        PH_CURE_BLEND = 604
        PH_CURE_HELD = 605
        PH_CURE_SOLO = 606
        PH_NURSE_BLEND = 607
        PH_NURSE_HELD = 608
        PH_NURSE_SOLO = 609
    End Enum

    Public Enum EM_Map_Battle_Scene
        MAP_BATTLE_SCENE_NORMAL
        MAP_BATTLE_SCENE_GYM
        MAP_BATTLE_SCENE_MAGMA
        MAP_BATTLE_SCENE_AQUA
        MAP_BATTLE_SCENE_SIDNEY
        MAP_BATTLE_SCENE_PHOEBE
        MAP_BATTLE_SCENE_GLACIA
        MAP_BATTLE_SCENE_DRAKE
        MAP_BATTLE_SCENE_FRONTIER
    End Enum

    Public Enum EM_Map_Type
        MAP_TYPE_0
        MAP_TYPE_TOWN
        MAP_TYPE_CITY
        MAP_TYPE_ROUTE
        MAP_TYPE_UNDERGROUND
        MAP_TYPE_UNDERWATER
        MAP_TYPE_6
        MAP_TYPE_7
        MAP_TYPE_INDOOR
        MAP_TYPE_SECRET_BASE
    End Enum

    Public Enum EM_Map_Names
        MAPSEC_LITTLEROOT_TOWN
        MAPSEC_OLDALE_TOWN
        MAPSEC_DEWFORD_TOWN
        MAPSEC_LAVARIDGE_TOWN
        MAPSEC_FALLARBOR_TOWN
        MAPSEC_VERDANTURF_TOWN
        MAPSEC_PACIFIDLOG_TOWN
        MAPSEC_PETALBURG_CITY
        MAPSEC_SLATEPORT_CITY
        MAPSEC_MAUVILLE_CITY
        MAPSEC_RUSTBORO_CITY
        MAPSEC_FORTREE_CITY
        MAPSEC_LILYCOVE_CITY
        MAPSEC_MOSSDEEP_CITY
        MAPSEC_SOOTOPOLIS_CITY
        MAPSEC_EVER_GRANDE_CITY
        MAPSEC_ROUTE_101
        MAPSEC_ROUTE_102
        MAPSEC_ROUTE_103
        MAPSEC_ROUTE_104
        MAPSEC_ROUTE_105
        MAPSEC_ROUTE_106
        MAPSEC_ROUTE_107
        MAPSEC_ROUTE_108
        MAPSEC_ROUTE_109
        MAPSEC_ROUTE_110
        MAPSEC_ROUTE_111
        MAPSEC_ROUTE_112
        MAPSEC_ROUTE_113
        MAPSEC_ROUTE_114
        MAPSEC_ROUTE_115
        MAPSEC_ROUTE_116
        MAPSEC_ROUTE_117
        MAPSEC_ROUTE_118
        MAPSEC_ROUTE_119
        MAPSEC_ROUTE_120
        MAPSEC_ROUTE_121
        MAPSEC_ROUTE_122
        MAPSEC_ROUTE_123
        MAPSEC_ROUTE_124
        MAPSEC_ROUTE_125
        MAPSEC_ROUTE_126
        MAPSEC_ROUTE_127
        MAPSEC_ROUTE_128
        MAPSEC_ROUTE_129
        MAPSEC_ROUTE_130
        MAPSEC_ROUTE_131
        MAPSEC_ROUTE_132
        MAPSEC_ROUTE_133
        MAPSEC_ROUTE_134
        MAPSEC_UNDERWATER_124
        MAPSEC_UNDERWATER_125
        MAPSEC_UNDERWATER_126
        MAPSEC_UNDERWATER_127
        MAPSEC_UNDERWATER_SOOTOPOLIS
        MAPSEC_GRANITE_CAVE
        MAPSEC_MT_CHIMNEY
        MAPSEC_SAFARI_ZONE
        MAPSEC_BATTLE_FRONTIER
        MAPSEC_PETALBURG_WOODS
        MAPSEC_RUSTURF_TUNNEL
        MAPSEC_ABANDONED_SHIP
        MAPSEC_NEW_MAUVILLE
        MAPSEC_METEOR_FALLS
        MAPSEC_METEOR_FALLS2
        MAPSEC_MT_PYRE
        MAPSEC_AQUA_HIDEOUT_OLD
        MAPSEC_SHOAL_CAVE
        MAPSEC_SEAFLOOR_CAVERN
        MAPSEC_UNDERWATER_128
        MAPSEC_VICTORY_ROAD
        MAPSEC_MIRAGE_ISLAND
        MAPSEC_CAVE_OF_ORIGIN
        MAPSEC_SOUTHERN_ISLAND
        MAPSEC_FIERY_PATH
        MAPSEC_FIERY_PATH2
        MAPSEC_JAGGED_PASS
        MAPSEC_JAGGED_PASS2
        MAPSEC_SEALED_CHAMBER
        MAPSEC_UNDERWATER_SEALED_CHAMBER
        MAPSEC_SCORCHED_SLAB
        MAPSEC_ISLAND_CAVE
        MAPSEC_DESERT_RUINS
        MAPSEC_ANCIENT_TOMB
        MAPSEC_INSIDE_OF_TRUCK
        MAPSEC_SKY_PILLAR
        MAPSEC_SECRET_BASE
        MAPSEC_DYNAMIC
        MAPSEC_PALLET_TOWN
        MAPSEC_VIRIDIAN_CITY
        MAPSEC_PEWTER_CITY
        MAPSEC_CERULEAN_CITY
        MAPSEC_LAVENDER_TOWN
        MAPSEC_VERMILION_CITY
        MAPSEC_CELADON_CITY
        MAPSEC_FUCHSIA_CITY
        MAPSEC_CINNABAR_ISLAND
        MAPSEC_INDIGO_PLATEAU
        MAPSEC_SAFFRON_CITY
        MAPSEC_ROUTE_4_FLYDUP
        MAPSEC_ROUTE_10_FLYDUP
        MAPSEC_ROUTE_1
        MAPSEC_ROUTE_2
        MAPSEC_ROUTE_3
        MAPSEC_ROUTE_4
        MAPSEC_ROUTE_5
        MAPSEC_ROUTE_6
        MAPSEC_ROUTE_7
        MAPSEC_ROUTE_8
        MAPSEC_ROUTE_9
        MAPSEC_ROUTE_10
        MAPSEC_ROUTE_11
        MAPSEC_ROUTE_12
        MAPSEC_ROUTE_13
        MAPSEC_ROUTE_14
        MAPSEC_ROUTE_15
        MAPSEC_ROUTE_16
        MAPSEC_ROUTE_17
        MAPSEC_ROUTE_18
        MAPSEC_ROUTE_19
        MAPSEC_ROUTE_20
        MAPSEC_ROUTE_21
        MAPSEC_ROUTE_22
        MAPSEC_ROUTE_23
        MAPSEC_ROUTE_24
        MAPSEC_ROUTE_25
        MAPSEC_VIRIDIAN_FOREST
        MAPSEC_MT_MOON
        MAPSEC_S_S_ANNE
        MAPSEC_UNDERGROUND_PATH
        MAPSEC_UNDERGROUND_PATH_2
        MAPSEC_DIGLETTS_CAVE
        MAPSEC_KANTO_VICTORY_ROAD
        MAPSEC_ROCKET_HIDEOUT
        MAPSEC_SILPH_CO
        MAPSEC_POKEMON_MANSION
        MAPSEC_KANTO_SAFARI_ZONE
        MAPSEC_POKEMON_LEAGUE
        MAPSEC_ROCK_TUNNEL
        MAPSEC_SEAFOAM_ISLANDS
        MAPSEC_POKEMON_TOWER
        MAPSEC_CERULEAN_CAVE
        MAPSEC_POWER_PLANT
        MAPSEC_ONE_ISLAND
        MAPSEC_TWO_ISLAND
        MAPSEC_THREE_ISLAND
        MAPSEC_FOUR_ISLAND
        MAPSEC_FIVE_ISLAND
        MAPSEC_SEVEN_ISLAND
        MAPSEC_SIX_ISLAND
        MAPSEC_KINDLE_ROAD
        MAPSEC_TREASURE_BEACH
        MAPSEC_CAPE_BRINK
        MAPSEC_BOND_BRIDGE
        MAPSEC_THREE_ISLE_PORT
        MAPSEC_SEVII_ISLE_6
        MAPSEC_SEVII_ISLE_7
        MAPSEC_SEVII_ISLE_8
        MAPSEC_SEVII_ISLE_9
        MAPSEC_RESORT_GORGEOUS
        MAPSEC_WATER_LABYRINTH
        MAPSEC_FIVE_ISLE_MEADOW
        MAPSEC_MEMORIAL_PILLAR
        MAPSEC_OUTCAST_ISLAND
        MAPSEC_GREEN_PATH
        MAPSEC_WATER_PATH
        MAPSEC_RUIN_VALLEY
        MAPSEC_TRAINER_TOWER
        MAPSEC_CANYON_ENTRANCE
        MAPSEC_SEVAULT_CANYON
        MAPSEC_TANOBY_RUINS
        MAPSEC_SEVII_ISLE_22
        MAPSEC_SEVII_ISLE_23
        MAPSEC_SEVII_ISLE_24
        MAPSEC_NAVEL_ROCK
        MAPSEC_MT_EMBER
        MAPSEC_BERRY_FOREST
        MAPSEC_ICEFALL_CAVE
        MAPSEC_ROCKET_WAREHOUSE
        MAPSEC_TRAINER_TOWER_2
        MAPSEC_DOTTED_HOLE
        MAPSEC_LOST_CAVE
        MAPSEC_PATTERN_BUSH
        MAPSEC_ALTERING_CAVE
        MAPSEC_TANOBY_CHAMBERS
        MAPSEC_THREE_ISLE_PATH
        MAPSEC_TANOBY_KEY
        MAPSEC_BIRTH_ISLAND
        MAPSEC_MONEAN_CHAMBER
        MAPSEC_LIPTOO_CHAMBER
        MAPSEC_WEEPTH_CHAMBER
        MAPSEC_DILFORD_CHAMBER
        MAPSEC_SCUFIB_CHAMBER
        MAPSEC_RIXY_CHAMBER
        MAPSEC_VIAPOIS_CHAMBER
        MAPSEC_EMBER_SPA
        MAPSEC_SPECIAL_AREA
        MAPSEC_AQUA_HIDEOUT
        MAPSEC_MAGMA_HIDEOUT
        MAPSEC_MIRAGE_TOWER
        MAPSEC_BIRTH_ISLAND_2
        MAPSEC_FARAWAY_ISLAND
        MAPSEC_ARTISAN_CAVE
        MAPSEC_MARINE_CAVE
        MAPSEC_UNDERWATER_MARINE_CAVE
        MAPSEC_TERRA_CAVE
        MAPSEC_UNDERWATER_TERRA_CAVE
        MAPSEC_UNDERWATER_UNK1
        MAPSEC_UNDERWATER_129
        MAPSEC_DESERT_UNDERPASS
        MAPSEC_ALTERING_CAVE_2
        MAPSEC_NAVEL_ROCK2
        MAPSEC_TRAINER_HILL
        MAPSEC_NONE
    End Enum

    Public Enum EM_Weather
        WEATHER_NONE = 0
        WEATHER_CLOUDS = 1
        WEATHER_SUNNY = 2
        WEATHER_RAIN_LIGHT = 3
        WEATHER_SNOW = 4
        WEATHER_RAIN_MED = 5
        WEATHER_FOG_1 = 6
        WEATHER_ASH = 7
        WEATHER_SANDSTORM = 8
        WEATHER_FOG_2 = 9
        WEATHER_FOG_3 = 10
        WEATHER_SHADE = 11
        WEATHER_DROUGHT = 12
        WEATHER_RAIN_HEAVY = 13
        WEATHER_BUBBLES = 14
        WEATHER_ROUTE119_CYCLE = 20
        WEATHER_ROUTE123_CYCLE = 21
    End Enum

End Module
