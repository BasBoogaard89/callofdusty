namespace Infrastructure.Context;

public static class ExampleDataSeeder
{
    private static readonly Theme[] Themes = new[]
    {
        new Theme { Description = "Military" },
        new Theme { Description = "Fantasy" },
    };

    private static readonly RoomCategory[] RoomCategories = new[]
    {
        new RoomCategory { Description = "Toilet" },
        new RoomCategory { Description = "Keuken" },
        new RoomCategory { Description = "Gang" },
        new RoomCategory { Description = "Slaapkamer" },
        new RoomCategory { Description = "Douche" },
        new RoomCategory { Description = "Woonkamer" },
        new RoomCategory { Description = "Berging" },
        new RoomCategory { Description = "Balkon" },
    };

    private static readonly ChoreCategory[] ChoreCategories = new[]
    {
        new ChoreCategory { Description = "WC-pot" },
        new ChoreCategory { Description = "Muren" },
        new ChoreCategory { Description = "Vloeren" },
        new ChoreCategory { Description = "Koffieapparaat" },
        new ChoreCategory { Description = "Lampen" },
        new ChoreCategory { Description = "Deurklinken" },
        new ChoreCategory { Description = "Ramen" },
        new ChoreCategory { Description = "Stofzuigen" },
        new ChoreCategory { Description = "Dweilen" },
        new ChoreCategory { Description = "Afzuigkap" },
        new ChoreCategory { Description = "Koelkast" },
        new ChoreCategory { Description = "Douche" },
        new ChoreCategory { Description = "Bed" },
    };

    private static TextTemplate GetTextTemplate(string description, int themeId, CategoryType categoryType, int categoryId = 0)
    {
        return new TextTemplate
        {
            Description = description,
            CategoryType = categoryType,
            CategoryId = categoryId,
            ThemeId = themeId,
        };
    }

    private static TextFragment GetTextFragment(string key, string value, int textTemplateId)
    {
        return new TextFragment
        {
            Key = key,
            Value = value,
            TextTemplateId = textTemplateId
        };
    }

    private static void AddFragment(AppDbContext context, int templateId, params (string Key, string[] Lines)[] blocks)
    {
        foreach (var (key, lines) in blocks)
            context.TextFragment.AddRange(lines.Select(v => GetTextFragment(key, v, templateId)));
    }

    static (string Desc, (string Key, string[] Options)[] Keys) Tpl(string desc, params (string Key, string[] Options)[] keys) => (desc, keys);

    private static void AddTemplates(AppDbContext context, int theme, CategoryType type, int category, params (string Desc, (string Key, string[] Options)[] Keys)[] variants)
    {
        foreach (var v in variants)
            AddTemplate(context, theme, type, category, v.Desc, v.Keys);
    }

    static TextTemplate AddTemplate(AppDbContext context, int theme, CategoryType type, int category, string desc, params (string Key, string[] Options)[] keys)
    {
        var t = GetTextTemplate(desc, theme, type, category);
        context.TextTemplate.Add(t);
        context.SaveChanges();

        context.TextFragment.AddRange(
            keys.SelectMany(k => k.Options.Select(v => GetTextFragment(k.Key, v, t.Id)))
        );

        context.SaveChanges();
        return t;
    }

    private static async Task EnsureRolesExist(RoleManager<IdentityRole> roleManager, IEnumerable<string> roleNames)
    {
        foreach (var role in roleNames)
            if (!await roleManager.RoleExistsAsync(role))
                _ = await roleManager.CreateAsync(new IdentityRole(role));
    }

    public static async Task Seed(IServiceProvider services)
    {
        var dbContext = services.GetRequiredService<AppDbContext>();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.Migrate();

        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        await EnsureRolesExist(roleManager, ["Admin", "User"]);

        if (await userManager.FindByNameAsync("admin") is null)
        {
            var admin = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@example.com",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(admin, "P@ssword123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            } else
            {
                throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));
            }
        }

        SeedData(dbContext);
    }

    public static void SeedData(AppDbContext context)
    {
        context.Theme.AddRange(Themes);
        context.RoomCategory.AddRange(RoomCategories);
        context.ChoreCategory.AddRange(ChoreCategories);

        context.SaveChanges();

        var themeId = context.Theme.ToDictionary(t => t.Description, t => t.Id);
        var roomCatId = context.RoomCategory.ToDictionary(r => r.Description, r => r.Id);
        var choreCatId = context.ChoreCategory.ToDictionary(c => c.Description, c => c.Id);

        context.TextTemplate.AddRange(
            GetTextTemplate("@m1 @m2 @m3 \n\n @m4 @m5 @m6", themeId["Military"], CategoryType.Intro),
            GetTextTemplate("@f1 @f2 @f3 \n\n @f4 @f5 @f6", themeId["Fantasy"], CategoryType.Intro)
        );

        context.SaveChanges();

        var textTemplateId = context.TextTemplate.ToDictionary(t => t.Description, t => t.Id);

        #region Quest description
        var tidM = textTemplateId["@m1 @m2 @m3 \n\n @m4 @m5 @m6"]; // Military
        var tidF = textTemplateId["@f1 @f2 @f3 \n\n @f4 @f5 @f6"]; // Fantasy

        // --------------------
        // MILITARY
        // --------------------
        AddFragment(context, tidM,
            ("m1", [
                "Het commandocentrum meldt een anomalie in Sector Delta; verkenningsdrones keren terug met ruis en half waarneembare silhouetten.",
            "Operatie Dweilstorm is geclassificeerd als dringend; het terrein is bekend, maar de situatie is volledig vloeibaar.",
            "Satellieten pikken ongebruikelijke patronen op in het leefgebied; commandanten vermoeden een sluimerende dreiging.",
            "De nachtpatrouille rapporteert stilte die té stil is; protocollen voor nabijheidsdreiging zijn geactiveerd.",
            "Er is een lek in de linie: klein, onopvallend, maar precies waar het pijn doet.",
            "In het rapport staat één zin onderstreept: ‘Dit voelt niet goed.’",
            "De briefing was kort en koel; de implicaties spreken harder dan de woorden.",
            "De grensposten zagen niets—en juist dat maakt het verdacht.",
            "Het hoofdkwartier heeft je naam rood omcirkeld in het logboek.",
            "Er wordt gefluisterd in de gangen: ‘Dit wordt een operatie voor de boeken.’"
            ]),
            ("m2", [
                "Tijd is een luxe die we vandaag niet hebben; elke minuut vergroot de kans op escalatie.",
            "De klok tikt mee met de tegenstander; vertraging is gelijk aan terreinverlies.",
            "Een adempauze lijkt verleidelijk, maar het momentum is onze enige bondgenoot.",
            "De aanvoerlijnen staan onder druk; falen betekent tekorten aan het front.",
            "We lopen één stap voor, hooguit; draai je om, en je bent twee achter.",
            "De vijand kiest de nacht, wij kiezen de precisie.",
            "Commando schat het risico hoog, maar de beloning is controle.",
            "Elke seconde zonder zicht is een seconde aan de ander gegeven.",
            "Er is geen uitstel; de situatie verslechtert bij stilstand.",
            "De marges zijn flinterdun—fout is einde verhaal."
            ]),
            ("m3", [
                "Localiseer de bron van de verstoring en stel een perimeter veilig die standhoudt.",
            "Herwin informatie, herstel orde, en markeer het gebied voor latere versterking.",
            "Verken, bevestig, neutraliseer—en documenteer alles wat niet klopt.",
            "Identificeer de oorzaak, ontwijk ruis, en zorg dat de sector weer stil ademt.",
            "Stel diagnostiek op: wie, wat, waar—en vooral waarom hier.",
            "Breng de situatie terug naar normaalbedrijf, zonder sporen van paniek.",
            "Verwijder complicaties; laat de omgeving achter alsof dreiging nooit bestond.",
            "Zorg dat de route vrij is voor wie na jou komt.",
            "Leg elk detail vast; bewijs wint rapporten.",
            "Schakel risico's uit voordat ze kansen worden voor de vijand."
            ]),
            ("m4", [
                "Koude tocht trekt door kaal beton; in elke echo zit een halve waarheid.",
            "Metalen trappen zingen zacht, alsof ze weten wat jij nog moet ontdekken.",
            "De lucht ruikt naar olie en oude regen; sporen verdwijnen sneller dan voetstappen.",
            "Een rode waas flikkert onregelmatig, als een hartslag buiten ritme.",
            "In de verte kraakt iets, ritmisch, koppig, alsof het niet wil breken.",
            "De stilte duwt terug; je adem klinkt als een bevel.",
            "Donkere hoeken lijken te luisteren; licht laat meer zien dan je lief is.",
            "De vloer draagt verhalen van vorige operaties, maar geeft niets prijs.",
            "Wind ritselt door losse kabels, als gefluister in een vreemde taal.",
            "Ver weg pulseert een generator, als een hartslag in metaal."
            ]),
            ("m5", [
                "Hou je hoofd koel en je blik smal; details winnen oorlogen.",
            "Geen heldendom vandaag—alleen vakwerk.",
            "Wees stil waar lawaai verwacht wordt, en zichtbaar waar niemand kijkt.",
            "Routine is je schild; improvisatie is je zwaard.",
            "Onthoud: ordelijkheid is besmettelijk, net als chaos.",
            "Als het simpel lijkt, ben je iets over het hoofd aan het zien.",
            "De kaart liegt nooit, maar ze vertelt ook niet alles.",
            "Discipline eerst, roem later.",
            "Focus op de missie, niet op het applaus.",
            "Kalmte weegt zwaarder dan kogels."
            ]),
            ("m6", [
                "Check je uitrusting, bevestig je route, en meld ‘in positie’ bij het commandocentrum.",
            "Neem het initiatief, schaal waar nodig op, en rapporteer alleen wat telt.",
            "Beweeg licht, denk zwaarder, en laat geen sporen na behalve orde.",
            "Synchroniseer met je team, stel je signaal veilig, en ga.",
            "Zet je timer op stilte en je kompas op vastberaden.",
            "Wanneer het licht knippert, beweeg; wanneer het dooft, beslis.",
            "Je missie start bij de drempel; je succes bij de terugkeer.",
            "Ga nu—en laat het gebied stiller achter dan je het vond.",
            "Maak ons trots en keer terug zonder littekens.",
            "Verlaat de basis alsof de geschiedenis je op de hielen zit."
            ])
        );

        // --------------------
        // FANTASY
        // --------------------
        AddFragment(context, tidF,
            ("f1", [
                "De heraut fluistert van een barst in het lot; oude kaarten beginnen zich te herschrijven.",
            "In de gildezaal dooft een kaars zonder wind; het is het soort stilte dat verhalen wakker maakt.",
            "Een ravenboodschap draagt slechts as en een zegel dat niemand herkent.",
            "De dorpsoudste droomde tweemaal hetzelfde teken; dat gebeurt alleen vlak voor gedonder.",
            "Lang onderdrukte namen fluisteren weer door de heggen; iemand of iets heeft ze wakker geschud.",
            "Een onwillige ster viel achter de heuvels; de val liet sporen in meer dan de lucht.",
            "De markt staat vol, maar iedereen spreekt zachter; voorgevoelens wegen zwaarder dan kramen.",
            "Iemand trok een zwaard dat nooit bot werd; het mes trilde alsof het iets wist.",
            "Het bos gaf een pad prijs dat er gisteren nog niet lag.",
            "De bard sloeg een valse noot en de zaal werd stil; ook dat is een omen, zeggen ze."
            ]),
            ("f2", [
                "Tijd is als zand in een gescheurde buidel; het glipt sneller weg na elke stap.",
            "De sluier tussen werelden rafelt; elke scheur laat iets hongerigs door.",
            "De raad vergadert onrustig; uitstel voedt slechts de schaduw die al aan tafel zit.",
            "Het gewas buigt te vroeg; wanneer de natuur haast maakt, moet jij dat ook.",
            "Een belofte aan de dageraad tikt; als zij komt en jij nog staat, komt zij zonder je te groeten.",
            "De poorten kraken op vreemde uren; rust roest, maar dit kraakt gevaarlijk hard.",
            "Wie te lang luistert naar het bos, hoort zijn eigen naam terugroepen.",
            "De rivier zingt hoger; water waarschuwt wanneer woorden falen.",
            "Twee zonnewijzers, één schaduw; de tijd zelf is in discussie met je plannen.",
            "De kasteelkelders ruiken naar storm; bliksem zoekt altijd eerst de twijfelaars."
            ]),
            ("f3", [
                "Zoek de bron van het gefluister en bind haar met een naam die nog niet vergeten is.",
            "Herstel de grenssteen en laat het land weer weten waar het begint en eindigt.",
            "Ontneem de nacht haar voordeel; zet licht waar het pijn doet en zet stilte waar ze gilt.",
            "Breng waarheid terug uit plekken waar men haar meestal kwijtraakt.",
            "Spreek met wat leeft tussen wortels; zij weten meer dan mensen willen opbiechten.",
            "Haal de doorn uit de poot van het dorp; het loopt al te lang mank.",
            "Verzamel tekens van verandering en leg ze vast voor de Raad der Wijzen.",
            "Maak de weg veilig voor wie komt, ook als jij de voetstappen wist.",
            "Ontwar de betovering die als een knoop in de lucht hangt.",
            "Noem het beest bij zijn ware naam en laat het zichzelf verliezen."
            ]),
            ("f4", [
                "Mist kneedt zich tussen de heggen; elk hek kraakt alsof het wil praten.",
            "Lantaarns wiegen moeizaam; hun licht is dapperder dan hun vlam groot is.",
            "De stenen brug draagt geruchten; water vergeet nooit wat men erin fluistert.",
            "In de verte rinkelt een klok met één slag te veel.",
            "Over het veld trekt een kou die niet van het weer is.",
            "Het woud ademt langzaam in; je voelt het uitblazen in je nek.",
            "De maan hangt laag als een munt die nog betaald moet worden.",
            "Er ligt een smaak van ijzer op de lucht, als een belofte die scherp is.",
            "Een sleeptoon van krekels valt plots stil; zelfs het kleine volk luistert mee.",
            "De heuvel gloeit net niet; net genoeg om vragen te stellen."
            ]),
            ("f5", [
                "Wees hoffelijk voor het onbekende; het is vaak familie van wat je mist.",
            "Moed is niet schreeuwen, maar blijven staan als niemand kijkt.",
            "Vraag het juiste, en de wereld antwoordt; vraag te veel, en ze zwijgt uit beleefdheid.",
            "Leer de namen van dingen; wat je juist noemt, luistert beter.",
            "Geen glans zonder vuil; poets met beleid en je vindt de kern.",
            "Houd je belofte klein maar breekbaar; zo draag je haar met zorg.",
            "Wees niet sneller dan je schaduw; hij kent de rechte weg.",
            "Kies je metgezellen met oor voor stilte en oog voor sporen.",
            "Mild voor mensen, streng voor het kwaad; zo blijft je zwaard scherp waar het telt.",
            "Een klein gebaar op tijd is groter dan groot vertoon te laat."
            ]),
            ("f6", [
                "Sjor je mantel, stem je kompas, en spreek een woord van moed uit—al is het voor jezelf.",
            "Neem licht voor de schaduw en stilte voor het lawaai; ga voor het dorp voordat het jou vooruit is.",
            "Zoek het pad dat jou zoekt; je merkt het aan hoe het onder je voeten past.",
            "Groet de poortwachter, hoor de waarschuwing, en ga toch.",
            "Vul je veldfles, sluit je riem, en kies het eerste kruispunt met vaste hand.",
            "Raap je moed bijeen zoals kruimels; genoeg bij elkaar maakt een maaltijd.",
            "Zet je stap vóór je twijfel; laat de rest volgen uit gewoonte.",
            "Laat een klein teken achter voor wie je zoekt—en geen spoor voor wie je volgt.",
            "Vraag om zegen als je kunt, en om vergeving als je moet; ga nu.",
            "De wereld wacht niet—maar ze merkt het wél als je komt."
            ])
        );

        #endregion

        #region Military

        var M_Toilet_sector = new[] { "W.C.-01", "Foxtrot-2", "Keramieklijn", "Sanitaire Voorpost", "Stalldek Noord", "Compartment 3A", "Hygiene Hub", "Deck Bravo", "Blok B", "Ruiterhok West" };
        var M_Toilet_activiteit = new[] { "onregelmatige flow", "calcificatie aan rand", "geurverstoringen", "spettervuur", "drukval in spoel", "onbekende vlekken", "scharniergekraak", "waterlijn troebel", "sensor meldt afvalspoor", "ventilatie-tekort" };
        var M_Toilet_status = new[] { "risico: middel", "glansindex gedaald", "perimeter instabiel", "inspectie noodzakelijk", "uitrusting vereist", "luchtkwaliteit suboptimaal", "gebruik gepauzeerd", "randcompromis", "veiligheidslicht: oranje", "situatie beheersbaar" };
        var M_Toilet_actie = new[] { "Protocol Borstel-Delta", "Ontkalk rand en kom", "Volledige desinfectie", "Ventileer en reset", "Spoel twee cycli", "Scharnier smeren", "Vlekken neutraliseren", "Waterlijn polijsten", "Geuroverlast afvoeren", "Vrijgave na controle" };
        var M_Toilet_observatie = new[] { "flow gedraagt zich grillig", "kalk bouwt zich op langs rim", "geurprofiel wijkt af van baseline", "spatpatroon buiten spec", "spoeldruk fluctueert", "vlekken zonder duidelijke herkomst", "deksel scharniert schokkerig", "waterlijn toont troebelbeeld", "sensor detecteert microresidu", "ventilatie is traag en luid" };
        var M_Toilet_prioriteit = new[] { "laag", "middel", "hoog", "kritiek", "tijd-gevoelig", "routine", "spoed", "preventief", "inspectie", "noodzakelijk" };
        var M_Toilet_risico = new[] { "splashback", "moraalverlies", "glijgevaar", "geurincident", "hygiënelacune", "rapport-gevoelig", "oppervlakte-slijtage", "handschoenbreuk", "bezoeker-onvrede", "routine-afwijking" };
        var M_Toilet_code = new[] { "ALFA", "BRAVO", "CHARLIE", "DELTA", "ECHO", "FOXTROT", "GOLF", "HOTEL", "INDIA", "JULIET" };
        var M_Toilet_label = new[] { "GROEN", "GEEL", "ORANJE", "ROOD", "BLAUW", "PAARS", "AMBER", "GRIJS", "ZWART", "WIT" };
        var M_Toilet_procedure = new[] { "Borstel-Delta uitvoeren", "Ontkalk-cyclus starten", "Volledige desinfectie", "Ventilatie verhogen en resetten", "Dubbele spoel uitvoeren", "Scharnieren smeren", "Vlekken neutraliseren", "Waterlijn polijsten", "Geurfilter wisselen", "Eindcontrole en vrijgave" };

        AddTemplates(context, themeId["Military"], CategoryType.Room, roomCatId["Toilet"],
            Tpl("Sector @sector meldt @activiteit. Status: @status. Actie: @actie.",
                ("sector", M_Toilet_sector),
                ("activiteit", M_Toilet_activiteit),
                ("status", M_Toilet_status),
                ("actie", M_Toilet_actie)
            ),
            Tpl("Locatie: @sector. Observatie: @observatie. Orders: @actie.",
                ("sector", M_Toilet_sector),
                ("observatie", M_Toilet_observatie),
                ("actie", M_Toilet_actie)
            ),
            Tpl("Rapport @code — @sector: @activiteit. Risico: @risico. Maatregel: @actie.",
                ("code", M_Toilet_code),
                ("sector", M_Toilet_sector),
                ("activiteit", M_Toilet_activiteit),
                ("risico", M_Toilet_risico),
                ("actie", M_Toilet_actie)
            ),
            Tpl("Zone @sector. Toestand: @status. Uitvoering: @procedure.",
                ("sector", M_Toilet_sector),
                ("status", M_Toilet_status),
                ("procedure", M_Toilet_procedure)
            ),
            Tpl("Checkpoint @sector actief. Prioriteit: @prioriteit. Procedure: @procedure.",
                ("sector", M_Toilet_sector),
                ("prioriteit", M_Toilet_prioriteit),
                ("procedure", M_Toilet_procedure)
            ),
            Tpl("Kaartupdate — @sector gelabeld @label. Situatie: @status. Volgende stap: @actie.",
                ("sector", M_Toilet_sector),
                ("label", M_Toilet_label),
                ("status", M_Toilet_status),
                ("actie", M_Toilet_actie)
            )
        );

        // ============ MILITARY → KEUKEN ============
        var M_Keuken_sector = new[] {
                "Galley Bravo","Kookpost Delta","Schaftunit A1","Counter Sigma","Snijvlak Omega",
                "Rooster Bay","Filter Hub","Opslag Niche","Warmte-Deck","Spoelstraat Kappa"
            };
        var M_Keuken_activiteit = new[] {
                "vetfilm op aanrecht","filterdruk te hoog","kruimelinbraak","pannen carbonisatie","afzuig ruis",
                "vloercoating glibberig","handgrepen kleverig","kooklucht persistent","koelkastcondens","splashzone actief"
            };
        var M_Keuken_status = new[] {
                "voedselveiligheid onder marge","luchtflow 60%","oppervlaktes dof","hittezones ongelijk","randen vergeten",
                "sensoriek vervuild","inspectie pending","tijd voor reset","risico: laag-middel","nette oplevering haalbaar"
            };
        var M_Keuken_actie = new[] {
                "Ontvetten & desinfecteren","Filters resetten","Roosters borstelen","Kruimelroutes vegen","Moppen en antislip",
                "Handgrepen polijsten","Afzuig boosten","Pannen rehabiliteren","Temperatuur loggen","Vrijgave 'FoodSafe'"
            };
        var M_Keuken_observatie = new[] {
                "vetfilm zichtbaar op blad","filter ΔP te hoog","kruimels langs snijzone","pannen dragen koollaag","afzuig debiet laag",
                "vloer toont sliprisico","grepen voelen kleverig","kooklucht blijft hangen","condens in koelkast","spatzone rond fornuis"
            };
        var M_Keuken_procedure = new[] {
                "Ontvet-cyclus","Filterwissel","Roosterreiniging","Dieptereiniging werkblad","Antislip-mop",
                "Handgreeppolijst","Afzuig-boost","Pannenreanimatie","Thermolog","Eindcontrole"
            };
        var M_Keuken_risico = new[] {
                "kruisbesmetting","brandvlekken","slipgevaar","sensor-fout","warmte-accumulatie",
                "inspectierisico","luchtkwaliteit","krasvorming","glasschade","hygiënelek"
            };

        AddTemplates(context, themeId["Military"], CategoryType.Room, roomCatId["Keuken"],
            Tpl("Zone @sector meldt @activiteit. Status: @status. Actie: @actie.",
                ("sector", M_Keuken_sector), ("activiteit", M_Keuken_activiteit), ("status", M_Keuken_status), ("actie", M_Keuken_actie)
            ),
            Tpl("Locatie: @sector. Observatie: @observatie. Orders: @actie.",
                ("sector", M_Keuken_sector), ("observatie", M_Keuken_observatie), ("actie", M_Keuken_actie)
            ),
            Tpl("Rapport @code — @sector: @activiteit. Risico: @risico. Maatregel: @actie.",
                ("code", M_Toilet_code), ("sector", M_Keuken_sector), ("activiteit", M_Keuken_activiteit), ("risico", M_Keuken_risico), ("actie", M_Keuken_actie)
            ),
            Tpl("Zone @sector. Toestand: @status. Uitvoering: @procedure.",
                ("sector", M_Keuken_sector), ("status", M_Keuken_status), ("procedure", M_Keuken_procedure)
            ),
            Tpl("Checkpoint @sector actief. Prioriteit: @prioriteit. Procedure: @procedure.",
                ("sector", M_Keuken_sector), ("prioriteit", M_Toilet_prioriteit), ("procedure", M_Keuken_procedure)
            ),
            Tpl("Kaartupdate — @sector gelabeld @label. Situatie: @status. Volgende stap: @actie.",
                ("sector", M_Keuken_sector), ("label", M_Toilet_label), ("status", M_Keuken_status), ("actie", M_Keuken_actie)
            )
        );


        // ============ MILITARY → GANG ============
        var M_Gang_sector = new[] {
                "Corridor Alpha","Gangpad Noord","Linkway Beta","Transitring Delta","Sluiskamer West",
                "Service Loop","Deckspine","Perimeter Hall","Trappenhuis Z","Doorzone 4"
            };
        var M_Gang_activiteit = new[] {
                "stofpluimen bij plint","mat verzadigd","schakelaar-vet","armaturen dof","deurrails piepen",
                "hoekcluster vuil","zand-infiltratie","lintparade","strepen op vloer","spinnenwebbruggen"
            };
        var M_Gang_status = new[] {
                "doorstroom remmend","zichtbaarheid oké","geluid boven norm","frictie toegenomen","esthetiek laag",
                "risico glijden: licht","toegang belemmerd","controle nodig","veiligheid gewaarborgd","inspectie naderend"
            };
        var M_Gang_actie = new[] {
                "Z-vegen & nat moppen","Matten resetten","Touchpoints reinigen","Armaturen poetsen","Rails borstelen/smeren",
                "Hoeken detailen","Zandbarrière plaatsen","Lint verwijderen","Vloer neutraliseren","Doorgang vrijgeven"
            };
        var M_Gang_observatie = new[] {
                "plintzone vangt stof","matten verliezen grip","schakelaars glanzen verkeerd","armaturen ogen dof","rails piepen bij belasting",
                "hoeken missen routine","zand spoort vanaf ingang","losse linten op looproute","strepen na nat weer","spinrag in hoge hoeken"
            };
        var M_Gang_procedure = new[] {
                "Z-vegencyclus","Mat-reset","Schakelaar-sanitatie","Armatuurpolijst","Railbrush + lube",
                "Hoek-detail","Zandbarrière plaatsen","Lintverwijdering","Friction normalisatie","Eindcontrole"
            };
        var M_Gang_risico = new[] {
                "slipgevaar","struikelkans","doorstroomverlies","stofallergie","zichtverlies",
                "plintschade","elektrisch risico (schakelaar)","valgevaar trap","klachtenbeleving","inspectie-afkeur"
            };

        AddTemplates(context, themeId["Military"], CategoryType.Room, roomCatId["Gang"],
            Tpl("Route @sector meldt @activiteit. Status: @status. Actie: @actie.",
                ("sector", M_Gang_sector), ("activiteit", M_Gang_activiteit), ("status", M_Gang_status), ("actie", M_Gang_actie)
            ),
            Tpl("Locatie: @sector. Observatie: @observatie. Orders: @actie.",
                ("sector", M_Gang_sector), ("observatie", M_Gang_observatie), ("actie", M_Gang_actie)
            ),
            Tpl("Rapport @code — @sector: @activiteit. Risico: @risico. Maatregel: @actie.",
                ("code", M_Toilet_code), ("sector", M_Gang_sector), ("activiteit", M_Gang_activiteit), ("risico", M_Gang_risico), ("actie", M_Gang_actie)
            ),
            Tpl("Zone @sector. Toestand: @status. Uitvoering: @procedure.",
                ("sector", M_Gang_sector), ("status", M_Gang_status), ("procedure", M_Gang_procedure)
            ),
            Tpl("Checkpoint @sector actief. Prioriteit: @prioriteit. Procedure: @procedure.",
                ("sector", M_Gang_sector), ("prioriteit", M_Toilet_prioriteit), ("procedure", M_Gang_procedure)
            ),
            Tpl("Kaartupdate — @sector gelabeld @label. Situatie: @status. Volgende stap: @actie.",
                ("sector", M_Gang_sector), ("label", M_Toilet_label), ("status", M_Gang_status), ("actie", M_Gang_actie)
            )
        );


        // ============ MILITARY → SLAAPKAMER ============
        var M_Slaap_sector = new[] {
                "Kwartier Rust","Quarters Q1","Slaapzone Bravo","Pod Alpha","Private Module",
                "Rustpost Delta","Nachtzijde","Kasthoek West","Raamfront","Spiegelpost"
            };
        var M_Slaap_activiteit = new[] {
                "stoflaag onder bed","kussens uit lijn","kozijnen stoffig","gordijnrand dof","pluis paradeert",
                "handgrepen vettig","spiegel met vlekken","nachtkast-kruimels","vloerstrepen","lucht muf"
            };
        var M_Slaap_status = new[] {
                "comfort onder norm","visuele ruis aanwezig","luchtkwaliteit matig","slaaphygiëne middel","presentatie onvoldoende",
                "geluid nihil","reset gewenst","tijd voor ordening","risico: laag","inspectie kan volgen"
            };
        var M_Slaap_actie = new[] {
                "Linnen strak trekken","Diepzuigen en vegen","Kozijn reinigen","Kussens opschudden","Mop op laag tempo",
                "Grepen polijsten","Spiegel helder maken","Kasthoek ordenen","Ventilatie boost","Ruststatus 'operabel' loggen"
            };
        var M_Slaap_observatie = new[] {
                "stofnesten achter poten","plooien asymmetrisch","pollen op frame","gordijn laat stof los","pluisbanen richting deur",
                "grepen tonen vingerafdrukken","spiegelwaas zichtbaar","nachtkast verzamelt microkruimels","streepjes na sokken","lucht staat stil"
            };
        var M_Slaap_procedure = new[] {
                "Linnen-aanspanning","Deep vacuum","Kussen-reset","Kozijn-wipe","Low-speed mop",
                "Grepen-polish","Spiegelpolijst","Kasthoek-organize","VentBoost","Eindrapport Rust"
            };
        var M_Slaap_risico = new[] {
                "allergenen","slaapcomfortverlies","stootgevaar","uitglijden","luchtkwaliteit",
                "presentatieverlies","inspectie-delay","pluisaccumulatie","irritatie ogen","rustverstoring"
            };

        AddTemplates(context, themeId["Military"], CategoryType.Room, roomCatId["Slaapkamer"],
            Tpl("Kwartier @sector meldt @activiteit. Status: @status. Actie: @actie.",
                ("sector", M_Slaap_sector), ("activiteit", M_Slaap_activiteit), ("status", M_Slaap_status), ("actie", M_Slaap_actie)
            ),
            Tpl("Locatie: @sector. Observatie: @observatie. Orders: @actie.",
                ("sector", M_Slaap_sector), ("observatie", M_Slaap_observatie), ("actie", M_Slaap_actie)
            ),
            Tpl("Rapport @code — @sector: @activiteit. Risico: @risico. Maatregel: @actie.",
                ("code", M_Toilet_code), ("sector", M_Slaap_sector), ("activiteit", M_Slaap_activiteit), ("risico", M_Slaap_risico), ("actie", M_Slaap_actie)
            ),
            Tpl("Zone @sector. Toestand: @status. Uitvoering: @procedure.",
                ("sector", M_Slaap_sector), ("status", M_Slaap_status), ("procedure", M_Slaap_procedure)
            ),
            Tpl("Checkpoint @sector actief. Prioriteit: @prioriteit. Procedure: @procedure.",
                ("sector", M_Slaap_sector), ("prioriteit", M_Toilet_prioriteit), ("procedure", M_Slaap_procedure)
            ),
            Tpl("Kaartupdate — @sector gelabeld @label. Situatie: @status. Volgende stap: @actie.",
                ("sector", M_Slaap_sector), ("label", M_Toilet_label), ("status", M_Slaap_status), ("actie", M_Slaap_actie)
            )
        );

        // ============ MILITARY → DOUCHE ============
        var M_Douche_sector = new[] {
                "Hydro-Post Echo","Shower Cell Delta","Tegelveld Bravo","Drain Node Foxtrot","Glass Shield Alpha",
                "Waterkast Zeta","Sproeikop Grid","Voegsector 7","Silicone Line","Mistkamer Lambda"
            };
        var M_Douche_activiteit = new[] {
                "kalk op kraan","zeepfilm op glas","afvoer traag","haren in rooster","sproeipatroon ongelijk",
                "tegels slipgevaar","voegen donker","siliconen mat","spiegelwaas","ventilatie traag"
            };
        var M_Douche_status = new[] {
                "zicht beperkt","flow verstoord","veiligheidsrisico: licht","onderhoud vereist","glans laag",
                "geur aanwezig","sensor alert","gebruik beperkt","herstel haalbaar","omgeving nat"
            };
        var M_Douche_actie = new[] {
                "Ontkalk & schrob voegen","Rooster openen en reinigen","Sproeikoppen reinigen","Antislip-moppen","Glas polijsten",
                "Kranen laten glanzen","Ventilatie verhogen","Drogen & nalopen","Zone markeren 'fris'","Hydro afsluiten"
            };
        var M_Douche_observatie = new[] {
                "kalkkraag rond kraanbasis","film sluipt over glas","water blijft staan bij afvoer","rooster vangt vezels","jets spatten scheef",
                "tegel voelt glad","voeg toont schaduw","siliconen voelt stroef","spiegel blijft nevelig","fan blijft achter"
            };
        var M_Douche_procedure = new[] {
                "Decalcify","Grout-scrub","Trap-clean","Nozzle-clean","Anti-slip mop",
                "Glass polish","Hardware polish","Vent High","Dry & Inspect","Close Hydro"
            };
        var M_Douche_risico = new[] {
                "slipgevaar","schimmelgroei","verstopping","kalkaanslag","geurincident",
                "glasbreuk","corrosie","waterlek","zichtverlies","ventilatie-overbelasting"
            };

        AddTemplates(context, themeId["Military"], CategoryType.Room, roomCatId["Douche"],
            Tpl("Bassin @sector meldt @activiteit. Status: @status. Actie: @actie.",
                ("sector", M_Douche_sector), ("activiteit", M_Douche_activiteit), ("status", M_Douche_status), ("actie", M_Douche_actie)
            ),
            Tpl("Locatie: @sector. Observatie: @observatie. Orders: @actie.",
                ("sector", M_Douche_sector), ("observatie", M_Douche_observatie), ("actie", M_Douche_actie)
            ),
            Tpl("Rapport @code — @sector: @activiteit. Risico: @risico. Maatregel: @actie.",
                ("code", M_Toilet_code), ("sector", M_Douche_sector), ("activiteit", M_Douche_activiteit), ("risico", M_Douche_risico), ("actie", M_Douche_actie)
            ),
            Tpl("Zone @sector. Toestand: @status. Uitvoering: @procedure.",
                ("sector", M_Douche_sector), ("status", M_Douche_status), ("procedure", M_Douche_procedure)
            ),
            Tpl("Checkpoint @sector actief. Prioriteit: @prioriteit. Procedure: @procedure.",
                ("sector", M_Douche_sector), ("prioriteit", M_Toilet_prioriteit), ("procedure", M_Douche_procedure)
            ),
            Tpl("Kaartupdate — @sector gelabeld @label. Situatie: @status. Volgende stap: @actie.",
                ("sector", M_Douche_sector), ("label", M_Toilet_label), ("status", M_Douche_status), ("actie", M_Douche_actie)
            )
        );

        #endregion

        #region Fantasy

        // ===== Common pools =====
        var F_Code = new[] { "RUNE-01", "RUNE-02", "RUNE-03", "RUNE-04", "RUNE-05", "RUNE-06", "RUNE-07", "RUNE-08", "RUNE-09", "RUNE-10" };
        var F_Label = new[] { "GEZEGEND", "STIL", "SCHADUW", "VLAM", "MIST", "GLANS", "ROEST", "STORM", "HEILIG", "HOUDING" };
        var F_Prioriteit = new[] { "laag", "middel", "hoog", "kritiek", "spoed", "ritueel", "patrouille", "nachtwake", "dageraad", "herstel" };

        /// ============ FANTASY → TOILET ============
        var F_Toilet_sector = new[] {
                "Troonkamer Klein","Privy Niche","Latrine van ’t Hof","Zilverpot","Stille Kamer",
                "Waterkast der Pages","Noodhuisje","Kroonhok","Hygiene-alcove","Schamelhok"
            };
        var F_Toilet_activiteit = new[] {
                "kalkvloek op de ring","fluistergeur in de kom","spoelbezwering traag","ketting klaagt",
                "vlekken zonder wapen","water nevelig","deksel kraakt vreemd","spatten buiten de runen",
                "druk zakt weg","ventelucht zwak"
            };
        var F_Toilet_status = new[] {
                "zegen verzwakt","glans verbleekt","perimeter wankel","inspectie vereist","hulpmiddelen nodig",
                "lucht is muf","gebruik gepauzeerd","rand beschadigd","sein: amber","onder controle"
            };
        var F_Toilet_actie = new[] {
                "borstelbezwering uitvoeren","ring ontkalken","volledige zuivering","luchtgeesten roepen (ventileren)",
                "dubbel spoelen","scharnieren zalven","vlekken bannen","waterlijn polijsten","geurfilter wisselen","eindzegen & vrijgave"
            };
        var F_Toilet_observatie = new[] {
                "water aarzelt in de kom","kalk kruipt rond de kroon","geur wijkt af van het boek",
                "spatten doorbreken het patroon","druk valt bij de spreuk","vlek zonder afkomst",
                "deksel zucht bij openen","lijn van water is troebel","microspoor op de rand","ventelucht houdt de adem in"
            };
        var F_Toilet_risico = new[] {
                "glijgevaar","gemor van dorpelingen","geurintrige","hygiënegat","kroniekschade",
                "slijtage van porselein","handschoenbreuk","onvrede bezoek","ritmebreuk routine","tijdverlies"
            };
        var F_Toilet_procedure = new[] {
                "Borstel-ritueel","Ontkalkingsspreuk","Volledige zuivering","Vent-boost",
                "Dubbel Spoel","Scharnierzalf","Vlekban","Lijnpolijst","Filterwissel","Eindzegen"
            };

        AddTemplates(context, themeId["Fantasy"], CategoryType.Room, roomCatId["Toilet"],
            Tpl("Plek @sector meldt @activiteit. Status: @status. Actie: @actie.",
                ("sector", F_Toilet_sector), ("activiteit", F_Toilet_activiteit), ("status", F_Toilet_status), ("actie", F_Toilet_actie)
            ),
            Tpl("Locatie: @sector. Observatie: @observatie. Orders: @actie.",
                ("sector", F_Toilet_sector), ("observatie", F_Toilet_observatie), ("actie", F_Toilet_actie)
            ),
            Tpl("Rune @code — @sector: @activiteit. Risico: @risico. Maatregel: @actie.",
                ("code", F_Code), ("sector", F_Toilet_sector), ("activiteit", F_Toilet_activiteit), ("risico", F_Toilet_risico), ("actie", F_Toilet_actie)
            ),
            Tpl("Zaal @sector. Toestand: @status. Uitvoering: @procedure.",
                ("sector", F_Toilet_sector), ("status", F_Toilet_status), ("procedure", F_Toilet_procedure)
            ),
            Tpl("Wachtpost @sector actief. Prioriteit: @prioriteit. Procedure: @procedure.",
                ("sector", F_Toilet_sector), ("prioriteit", F_Prioriteit), ("procedure", F_Toilet_procedure)
            ),
            Tpl("Kaartupdate — @sector gemerkt @label. Situatie: @status. Volgende stap: @actie.",
                ("sector", F_Toilet_sector), ("label", F_Label), ("status", F_Toilet_status), ("actie", F_Toilet_actie)
            )
        );

        /// ============ FANTASY → KEUKEN ============
        var F_Keuken_sector = new[] {
                "Haardhoek","Brouwtafel","Snijplank der Gilde","Kruidenrek","Koperketel",
                "Afzuigkap","Spoelbekken","Voorraadkist","Ovenmond","Vaatrek"
            };
        var F_Keuken_activiteit = new[] {
                "vetsluier op blad","filter zucht","kruimelspoor actief","pannen zwart geblakerd","damp blijft hangen",
                "vloer wil glijden","grepen kleven","geur van gisteren","condens op glas","spatzone bij de ketel"
            };
        var F_Keuken_status = new[] {
                "voedselzegen dun","luchtstroom laag","oppervlak dof","hitte ongelijk","randen vergeten",
                "sensoren vervuild","inspectie aanstaande","tijd voor reset","risico: laag-middel","oplevering haalbaar"
            };
        var F_Keuken_actie = new[] {
                "ontvetten & zuiveren","filters vernieuwen","roosters borstelen","kruimels vegen","antislip moppen",
                "grepen polijsten","afzuig bezweren","pannen herleven","temperatuur loggen","vrijgave 'FoodSafe'"
            };
        var F_Keuken_observatie = new[] {
                "vetfilm op blad","filterdruk te hoog","kruimels bij snijzone","koollaag op pannen","afzuig debiet laag",
                "vloer glad bij ingang","kleverige grepen","kooklucht blijft hangen","koelkast condenseert","spatzone actief"
            };
        var F_Keuken_risico = new[] {
                "kruisbesmetting","brandvlek","slipgevaar","sensorfout","warmte-accumulatie",
                "inspectierisico","luchtkwaliteit","krasvorming","glasschade","hygiënelek"
            };
        var F_Keuken_procedure = new[] {
                "Ontvet-cyclus","Filterwissel","Roosterreiniging","Dieptereiniging blad","Antislip-mop",
                "Grepen-polijst","Afzuig-boost","Pannenreanimatie","Thermolog","Eindcontrole"
            };

        AddTemplates(context, themeId["Fantasy"], CategoryType.Room, roomCatId["Keuken"],
            Tpl("Zaal @sector meldt @activiteit. Status: @status. Actie: @actie.",
                ("sector", F_Keuken_sector), ("activiteit", F_Keuken_activiteit), ("status", F_Keuken_status), ("actie", F_Keuken_actie)
            ),
            Tpl("Plaats: @sector. Observatie: @observatie. Orders: @actie.",
                ("sector", F_Keuken_sector), ("observatie", F_Keuken_observatie), ("actie", F_Keuken_actie)
            ),
            Tpl("Rune @code — @sector: @activiteit. Risico: @risico. Maatregel: @actie.",
                ("code", F_Code), ("sector", F_Keuken_sector), ("activiteit", F_Keuken_activiteit), ("risico", F_Keuken_risico), ("actie", F_Keuken_actie)
            ),
            Tpl("Werkhoek @sector. Toestand: @status. Uitvoering: @procedure.",
                ("sector", F_Keuken_sector), ("status", F_Keuken_status), ("procedure", F_Keuken_procedure)
            ),
            Tpl("Wachtpost @sector actief. Prioriteit: @prioriteit. Procedure: @procedure.",
                ("sector", F_Keuken_sector), ("prioriteit", F_Prioriteit), ("procedure", F_Keuken_procedure)
            ),
            Tpl("Kaartupdate — @sector gemerkt @label. Situatie: @status. Volgende stap: @actie.",
                ("sector", F_Keuken_sector), ("label", F_Label), ("status", F_Keuken_status), ("actie", F_Keuken_actie)
            )
        );

        /// ============ FANTASY → GANG ============
        var F_Gang_sector = new[] {
                "Poortgang","Burchtpad","Trappenhuis","Lantaarnboog","Voorhal",
                "Plintgang","Middenpad","Meterhoek","Deurmat","Overloop"
            };
        var F_Gang_activiteit = new[] {
                "stofpluimen bij plint","mat verzadigd","schakelaar vettig","armaturen dof","deurrails piepen",
                "hoeken vol web","zand sluipt mee","lint op route","strepen na regen","echo te luid"
            };
        var F_Gang_status = new[] {
                "doorstroom traag","zicht: oké","geluid boven norm","frictie toegenomen","aanzicht laag",
                "sliprisico: licht","toegang belemmerd","controle nodig","veiligheid gewaarborgd","inspectie op komst"
            };
        var F_Gang_actie = new[] {
                "Z-vegen & moppen","matten resetten","touchpoints reinigen","armaturen polijsten","rails borstelen/smeren",
                "hoeken detailen","zandbarrière leggen","lint verwijderen","vloer neutraliseren","doorgang vrijgeven"
            };
        var F_Gang_observatie = new[] {
                "plint vangt stof","matten verliezen grip","schakelaars glanzen fout","armaturen ogen dof","rails piepen onder last",
                "hoeken missen beurt","zand vanaf poort","los lint op route","strepen door vocht","galm in de hal"
            };
        var F_Gang_risico = new[] {
                "slipgevaar","struikelkans","doorstroomverlies","stofallergie","zichtverlies",
                "plintschade","elektrisch risico","valtrap","klachtbeleving","afkeur inspectie"
            };
        var F_Gang_procedure = new[] {
                "Z-vegencyclus","Mat-reset","Schakelaar-sanitatie","Armatuurpolijst","Railbrush + zalf",
                "Hoek-detail","Barrière leggen","Lintverwijdering","Friction normaliseren","Eindcontrole"
            };

        AddTemplates(context, themeId["Fantasy"], CategoryType.Room, roomCatId["Gang"],
            Tpl("Route @sector meldt @activiteit. Status: @status. Actie: @actie.",
                ("sector", F_Gang_sector), ("activiteit", F_Gang_activiteit), ("status", F_Gang_status), ("actie", F_Gang_actie)
            ),
            Tpl("Plaats: @sector. Observatie: @observatie. Orders: @actie.",
                ("sector", F_Gang_sector), ("observatie", F_Gang_observatie), ("actie", F_Gang_actie)
            ),
            Tpl("Rune @code — @sector: @activiteit. Risico: @risico. Maatregel: @actie.",
                ("code", F_Code), ("sector", F_Gang_sector), ("activiteit", F_Gang_activiteit), ("risico", F_Gang_risico), ("actie", F_Gang_actie)
            ),
            Tpl("Galerij @sector. Toestand: @status. Uitvoering: @procedure.",
                ("sector", F_Gang_sector), ("status", F_Gang_status), ("procedure", F_Gang_procedure)
            ),
            Tpl("Wachtpost @sector actief. Prioriteit: @prioriteit. Procedure: @procedure.",
                ("sector", F_Gang_sector), ("prioriteit", F_Prioriteit), ("procedure", F_Gang_procedure)
            ),
            Tpl("Kaartupdate — @sector gemerkt @label. Situatie: @status. Volgende stap: @actie.",
                ("sector", F_Gang_sector), ("label", F_Label), ("status", F_Gang_status), ("actie", F_Gang_actie)
            )
        );

        /// ============ FANTASY → SLAAPKAMER ============
        var F_Slaap_sector = new[] {
                "Kamer der Rust","Kwartier van de Held","Nachtpost","Kasthoek","Raamkijk",
                "Spiegelnis","Gordijnrand","Onderbed","Deurpost","Haardzijde"
            };
        var F_Slaap_activiteit = new[] {
                "stoflaag onder bed","kussens uit lijn","kozijnen stoffig","gordijnrand dof","pluis paradeert",
                "grepen vettig","spiegel met vlekken","nachtkast kruimelt","vloer streperig","lucht staat stil"
            };
        var F_Slaap_status = new[] {
                "comfort onder norm","visuele ruis aanwezig","luchtkwaliteit matig","slaaphygiëne middel","aanzicht mager",
                "geluid stil","reset gewenst","ordening nodig","risico: laag","inspectie kan volgen"
            };
        var F_Slaap_actie = new[] {
                "linnen strak trekken","diepzuigen & vegen","kozijn reinigen","kussens opschudden","langzame mop",
                "grepen polijsten","spiegel helder maken","kasthoek ordenen","ventilatie boost","ruststatus loggen"
            };
        var F_Slaap_observatie = new[] {
                "stofnesten bij poten","plooien asymmetrisch","pollen op frame","gordijn geeft stof af","pluisbanen naar deur",
                "vingers op grepen","spiegelwaas zichtbaar","microkruimels op kast","streepjes na sokken","lucht muf"
            };
        var F_Slaap_risico = new[] {
                "allergenen","comfortverlies","stootgevaar","uitglijden","luchtkwaliteit",
                "presentatieverlies","inspectie-delay","pluisaccumulatie","oogirritatie","rustverstoring"
            };
        var F_Slaap_procedure = new[] {
                "Linnen-aanspanning","Deep vacuum","Kussen-reset","Kozijn-wipe","Low-speed mop",
                "Grepen-polish","Spiegelpolijst","Kasthoek-organize","Vent-boost","Eindrapport Rust"
            };

        AddTemplates(context, themeId["Fantasy"], CategoryType.Room, roomCatId["Slaapkamer"],
            Tpl("Kamer @sector meldt @activiteit. Status: @status. Actie: @actie.",
                ("sector", F_Slaap_sector), ("activiteit", F_Slaap_activiteit), ("status", F_Slaap_status), ("actie", F_Slaap_actie)
            ),
            Tpl("Plaats: @sector. Observatie: @observatie. Orders: @actie.",
                ("sector", F_Slaap_sector), ("observatie", F_Slaap_observatie), ("actie", F_Slaap_actie)
            ),
            Tpl("Rune @code — @sector: @activiteit. Risico: @risico. Maatregel: @actie.",
                ("code", F_Code), ("sector", F_Slaap_sector), ("activiteit", F_Slaap_activiteit), ("risico", F_Slaap_risico), ("actie", F_Slaap_actie)
            ),
            Tpl("Kwartier @sector. Toestand: @status. Uitvoering: @procedure.",
                ("sector", F_Slaap_sector), ("status", F_Slaap_status), ("procedure", F_Slaap_procedure)
            ),
            Tpl("Wachtpost @sector actief. Prioriteit: @prioriteit. Procedure: @procedure.",
                ("sector", F_Slaap_sector), ("prioriteit", F_Prioriteit), ("procedure", F_Slaap_procedure)
            ),
            Tpl("Kaartupdate — @sector gemerkt @label. Situatie: @status. Volgende stap: @actie.",
                ("sector", F_Slaap_sector), ("label", F_Label), ("status", F_Slaap_status), ("actie", F_Slaap_actie)
            )
        );

        /// ============ FANTASY → DOUCHE ============
        var F_Douche_sector = new[] {
                "Badhuisnis","Glaswand","Afvoerput","Kraanwerk","Tegelveld",
                "Siliconerand","Voegstrook","Sproeikop","Spiegel","Stoomhoek"
            };
        var F_Douche_activiteit = new[] {
                "kalkbezwering nodig","zeepfilm sluipt","afvoer loopt traag","haren in rooster","straal schiet scheef",
                "tegels glad","voegen verduisteren","silicone mat","spiegelwaas blijft","stoom blijft hangen"
            };
        var F_Douche_status = new[] {
                "zicht beperkt","flow verstoord","veiligheidsrisico: licht","onderhoud vereist","glans laag",
                "geur aanwezig","sensor alert","gebruik beperkt","herstel haalbaar","omgeving nat"
            };
        var F_Douche_actie = new[] {
                "ontkalk & schrob voegen","rooster openen en reinigen","sproeikoppen reinigen","antislip-moppen","glas polijsten",
                "kranen laten glanzen","ventilatie verhogen","drogen & nalopen","zone markeren 'fris'","hydro afsluiten"
            };
        var F_Douche_observatie = new[] {
                "kalkkraag rond kraan","film over glas","water blijft staan","rooster vangt vezels","jets gaan scheef",
                "tegel voelt glad","voeg toont schaduw","silicone stroef","spiegel blijft nevelig","stoom blijft hangen"
            };
        var F_Douche_risico = new[] {
                "slipgevaar","schimmelgroei","verstopping","kalkaanslag","geurincident",
                "glasbreuk","corrosie","waterlek","zichtverlies","ventilatie-overbelasting"
            };
        var F_Douche_procedure = new[] {
                "Decalcify","Grout-scrub","Trap-clean","Nozzle-clean","Anti-slip mop",
                "Glass polish","Hardware polish","Vent High","Dry & Inspect","Close Hydro"
            };

        AddTemplates(context, themeId["Fantasy"], CategoryType.Room, roomCatId["Douche"],
            Tpl("Badhuis @sector meldt @activiteit. Status: @status. Actie: @actie.",
                ("sector", F_Douche_sector), ("activiteit", F_Douche_activiteit), ("status", F_Douche_status), ("actie", F_Douche_actie)
            ),
            Tpl("Plaats: @sector. Observatie: @observatie. Orders: @actie.",
                ("sector", F_Douche_sector), ("observatie", F_Douche_observatie), ("actie", F_Douche_actie)
            ),
            Tpl("Rune @code — @sector: @activiteit. Risico: @risico. Maatregel: @actie.",
                ("code", F_Code), ("sector", F_Douche_sector), ("activiteit", F_Douche_activiteit), ("risico", F_Douche_risico), ("actie", F_Douche_actie)
            ),
            Tpl("Stoomnis @sector. Toestand: @status. Uitvoering: @procedure.",
                ("sector", F_Douche_sector), ("status", F_Douche_status), ("procedure", F_Douche_procedure)
            ),
            Tpl("Wachtpost @sector actief. Prioriteit: @prioriteit. Procedure: @procedure.",
                ("sector", F_Douche_sector), ("prioriteit", F_Prioriteit), ("procedure", F_Douche_procedure)
            ),
            Tpl("Kaartupdate — @sector gemerkt @label. Situatie: @status. Volgende stap: @actie.",
                ("sector", F_Douche_sector), ("label", F_Label), ("status", F_Douche_status), ("actie", F_Douche_actie)
            )
        );

        #endregion

        context.Room.AddRange(
        new Room
        {
            Description = "Toilet",
            CategoryId = roomCatId["Toilet"],
        },
        new Room
        {
            Description = "Keuken",
            CategoryId = roomCatId["Keuken"],
        },
        new Room
        {
            Description = "Gang",
            CategoryId = roomCatId["Gang"],
        },
        new Room
        {
            Description = "Slaapkamer",
            CategoryId = roomCatId["Slaapkamer"],
        },
        new Room
        {
            Description = "Douche",
            CategoryId = roomCatId["Douche"],
        },
        new Room
        {
            Description = "Woonkamer",
            CategoryId = roomCatId["Woonkamer"],
        },
        new Room
        {
            Description = "Berging",
            CategoryId = roomCatId["Berging"],
        },
        new Room
        {
            Description = "Balkon",
            CategoryId = roomCatId["Balkon"],
        });

        context.SaveChanges();

        var roomId = context.Room.ToDictionary(t => t.Description, t => t.Id);

        context.Chore.AddRange(
            // Toilet
            new Chore
            {
                Description = "WC-pot schoonmaken",
                DurationMinutes = 10,
                FrequencyDays = 7,
                DirtinessFactor = DirtinessFactor.Extreme,
                RoomId = roomCatId["Toilet"],
                CategoryId = choreCatId["WC-pot"],
            },
            new Chore
            {
                Description = "Toiletvloer dweilen",
                DurationMinutes = 10,
                FrequencyDays = 30,
                DirtinessFactor = DirtinessFactor.Medium,
                RoomId = roomCatId["Toilet"],
                CategoryId = choreCatId["Dweilen"],
            },
            new Chore
            {
                Description = "Toiletmuren afnemen",
                DurationMinutes = 15,
                FrequencyDays = 90,
                DirtinessFactor = DirtinessFactor.Low,
                RoomId = roomCatId["Toilet"],
                CategoryId = choreCatId["Muren"],
            },
            new Chore
            {
                Description = "Deurklink reinigen (toilet)",
                DurationMinutes = 2,
                FrequencyDays = 180,
                DirtinessFactor = DirtinessFactor.Low,
                RoomId = roomCatId["Toilet"],
                CategoryId = choreCatId["Deurklinken"],
            },

            // Keuken
            new Chore
            {
                Description = "Aanrecht schoonmaken",
                DurationMinutes = 10,
                FrequencyDays = 3,
                DirtinessFactor = DirtinessFactor.Extreme,
                RoomId = roomCatId["Keuken"],
                CategoryId = choreCatId["Vloeren"],
            },
            new Chore
            {
                Description = "Koffieapparaat reinigen",
                DurationMinutes = 10,
                FrequencyDays = 60,
                DirtinessFactor = DirtinessFactor.Medium,
                RoomId = roomCatId["Keuken"],
                CategoryId = choreCatId["Koffieapparaat"],
            },
            new Chore
            {
                Description = "Keukenvloer dweilen",
                DurationMinutes = 20,
                FrequencyDays = 30,
                DirtinessFactor = DirtinessFactor.Medium,
                RoomId = roomCatId["Keuken"],
                CategoryId = choreCatId["Dweilen"],
            },
            new Chore
            {
                Description = "Afzuigkapfilters schoonmaken",
                DurationMinutes = 30,
                FrequencyDays = 180,
                DirtinessFactor = DirtinessFactor.High,
                RoomId = roomCatId["Keuken"],
                CategoryId = choreCatId["Afzuigkap"],
            },
            new Chore
            {
                Description = "Koelkast schoonmaken",
                DurationMinutes = 45,
                FrequencyDays = 90,
                DirtinessFactor = DirtinessFactor.High,
                RoomId = roomCatId["Keuken"],
                CategoryId = choreCatId["Koelkast"],
            },

            // Gang
            new Chore
            {
                Description = "Gang stofzuigen",
                DurationMinutes = 15,
                FrequencyDays = 30,
                DirtinessFactor = DirtinessFactor.Medium,
                RoomId = roomCatId["Gang"],
                CategoryId = choreCatId["Stofzuigen"],
            },
            new Chore
            {
                Description = "Gangvloer dweilen",
                DurationMinutes = 15,
                FrequencyDays = 60,
                DirtinessFactor = DirtinessFactor.Low,
                RoomId = roomCatId["Gang"],
                CategoryId = choreCatId["Dweilen"],
            },
            new Chore
            {
                Description = "Ganglamp afstoffen",
                DurationMinutes = 5,
                FrequencyDays = 365,
                DirtinessFactor = DirtinessFactor.Low,
                RoomId = roomCatId["Gang"],
                CategoryId = choreCatId["Lampen"],
            },

            // Slaapkamer
            new Chore
            {
                Description = "Bed verschonen",
                DurationMinutes = 20,
                FrequencyDays = 30,
                DirtinessFactor = DirtinessFactor.High,
                RoomId = roomCatId["Slaapkamer"],
                CategoryId = choreCatId["Bed"],
            },
            new Chore
            {
                Description = "Slaapkamer stofzuigen",
                DurationMinutes = 15,
                FrequencyDays = 30,
                DirtinessFactor = DirtinessFactor.Medium,
                RoomId = roomCatId["Slaapkamer"],
                CategoryId = choreCatId["Stofzuigen"],
            },
            new Chore
            {
                Description = "Slaapkamervloer dweilen",
                DurationMinutes = 20,
                FrequencyDays = 60,
                DirtinessFactor = DirtinessFactor.Low,
                RoomId = roomCatId["Slaapkamer"],
                CategoryId = choreCatId["Dweilen"],
            },

            // Douche
            new Chore
            {
                Description = "Douchewand schoonmaken",
                DurationMinutes = 20,
                FrequencyDays = 30,
                DirtinessFactor = DirtinessFactor.High,
                RoomId = roomCatId["Douche"],
                CategoryId = choreCatId["Douche"],
            },
            new Chore
            {
                Description = "Doucheafvoer schoonmaken",
                DurationMinutes = 20,
                FrequencyDays = 180,
                DirtinessFactor = DirtinessFactor.High,
                RoomId = roomCatId["Douche"],
                CategoryId = choreCatId["Douche"],
            },
            new Chore
            {
                Description = "Douchevloer dweilen",
                DurationMinutes = 15,
                FrequencyDays = 30,
                DirtinessFactor = DirtinessFactor.Medium,
                RoomId = roomCatId["Douche"],
                CategoryId = choreCatId["Dweilen"],
            },

            // Woonkamer
            new Chore
            {
                Description = "Woonkamer stofzuigen",
                DurationMinutes = 20,
                FrequencyDays = 30,
                DirtinessFactor = DirtinessFactor.Medium,
                RoomId = roomCatId["Woonkamer"],
                CategoryId = choreCatId["Stofzuigen"],
            },
            new Chore
            {
                Description = "Woonkamer vloer dweilen",
                DurationMinutes = 20,
                FrequencyDays = 60,
                DirtinessFactor = DirtinessFactor.Medium,
                RoomId = roomCatId["Woonkamer"],
                CategoryId = choreCatId["Dweilen"],
            },
            new Chore
            {
                Description = "Ramen zemen (woonkamer)",
                DurationMinutes = 40,
                FrequencyDays = 180,
                DirtinessFactor = DirtinessFactor.Low,
                RoomId = roomCatId["Woonkamer"],
                CategoryId = choreCatId["Ramen"],
            },
            new Chore
            {
                Description = "Lampen afstoffen (woonkamer)",
                DurationMinutes = 10,
                FrequencyDays = 365,
                DirtinessFactor = DirtinessFactor.Low,
                RoomId = roomCatId["Woonkamer"],
                CategoryId = choreCatId["Lampen"],
            },

            // Berging
            new Chore
            {
                Description = "Berging vloer dweilen",
                DurationMinutes = 30,
                FrequencyDays = 365,
                DirtinessFactor = DirtinessFactor.Low,
                RoomId = roomCatId["Berging"],
                CategoryId = choreCatId["Dweilen"],
            },

            // Balkon
            new Chore
            {
                Description = "Balkon vegen/stofzuigen",
                DurationMinutes = 15,
                FrequencyDays = 60,
                DirtinessFactor = DirtinessFactor.Medium,
                RoomId = roomCatId["Balkon"],
                CategoryId = choreCatId["Stofzuigen"],
            },
            new Chore
            {
                Description = "Balkon dweilen",
                DurationMinutes = 20,
                FrequencyDays = 180,
                DirtinessFactor = DirtinessFactor.Low,
                RoomId = roomCatId["Balkon"],
                CategoryId = choreCatId["Dweilen"],
            },
            new Chore
            {
                Description = "Balkonramen zemen",
                DurationMinutes = 40,
                FrequencyDays = 365,
                DirtinessFactor = DirtinessFactor.Low,
                RoomId = roomCatId["Balkon"],
                CategoryId = choreCatId["Ramen"],
            }
        );

        context.SaveChanges();

        var choreId = context.Chore.ToDictionary(t => t.Description, t => t.Id);

        //context.History.AddRange(
        //new History
        //{
        //    ChoreId = choreId["WC schoonmaken"],
        //    DateCompleted = DateTime.Now.AddDays(-5),
        //},
        //new History
        //{
        //    ChoreId = choreId["Vloer schoonmaken"],
        //    DateCompleted = DateTime.Now.AddDays(-15),
        //},
        //new History
        //{
        //    ChoreId = choreId["Muren schoonmaken"],
        //    DateCompleted = DateTime.Now.AddDays(-20),
        //},
        //new History
        //{
        //    ChoreId = choreId["Koffieapparaat schoonmaken"],
        //    DateCompleted = DateTime.Now.AddDays(-1000),
        //});

        //context.SaveChanges();
    }
}
