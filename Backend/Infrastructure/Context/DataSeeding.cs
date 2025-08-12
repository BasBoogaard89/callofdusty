namespace Infrastructure.Context;

public static class ExampleDataSeeder
{
    private static readonly Theme[] Themes = new[]
    {
        new Theme { Description = "Detective" },
        new Theme { Description = "Fantasy" },
        new Theme { Description = "Lovecraft" },
        new Theme { Description = "Military" },
        new Theme { Description = "Post-apocalyptic" },
        new Theme { Description = "Sci-fi" }
    };

    private static readonly RoomCategory[] RoomCategories = new[]
    {
        new RoomCategory { Description = "Toilet" },
        new RoomCategory { Description = "Keuken" },
        new RoomCategory { Description = "Gang" },
        new RoomCategory { Description = "Slaapkamer" },
        new RoomCategory { Description = "Douche" },
    };

    private static readonly ChoreCategory[] ChoreCategories = new[]
    {
        new ChoreCategory { Description = "WC-pot" },
        new ChoreCategory { Description = "Muur" },
        new ChoreCategory { Description = "Vloer" },
        new ChoreCategory { Description = "Koffieapparaat" },
        new ChoreCategory { Description = "Lampen" },
        new ChoreCategory { Description = "Deurklinken" }
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

    public static void Seed(AppDbContext context)
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
            GetTextTemplate("@f1 @f2 @f3 \n\n @f4 @f5 @f6", themeId["Fantasy"], CategoryType.Intro),
            GetTextTemplate("@s1 @s2 @s3 \n\n @s4 @s5 @s6", themeId["Sci-fi"], CategoryType.Intro),
            GetTextTemplate("@d1 @d2 @d3 \n\n @d4 @d5 @d6", themeId["Detective"], CategoryType.Intro),
            GetTextTemplate("@l1 @l2 @l3 \n\n @l4 @l5 @l6", themeId["Lovecraft"], CategoryType.Intro),
            GetTextTemplate("@p1 @p2 @p3 \n\n @p4 @p5 @p6", themeId["Post-apocalyptic"], CategoryType.Intro)
        );

        context.SaveChanges();

        var textTemplateId = context.TextTemplate.ToDictionary(t => t.Description, t => t.Id);

        #region Quest description
        var tidM = textTemplateId["@m1 @m2 @m3 \n\n @m4 @m5 @m6"]; // Military
        var tidF = textTemplateId["@f1 @f2 @f3 \n\n @f4 @f5 @f6"]; // Fantasy
        var tidS = textTemplateId["@s1 @s2 @s3 \n\n @s4 @s5 @s6"]; // Sci-fi
        var tidD = textTemplateId["@d1 @d2 @d3 \n\n @d4 @d5 @d6"]; // Detective
        var tidP = textTemplateId["@p1 @p2 @p3 \n\n @p4 @p5 @p6"]; // Post-Apocalyptic
        var tidL = textTemplateId["@l1 @l2 @l3 \n\n @l4 @l5 @l6"]; // Lovecraftian

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

        // --------------------
        // SCI-FI
        // --------------------
        AddFragment(context, tidS,
            ("s1", [
                "Station Aegis registreert een faseverschuiving in het onderhoudsgrid; sensoren krijgen kippenvel, als dat kon.",
        "Een spookpakket sluipt door de netwerken; de checksum glimlacht vals.",
        "Een stille corridor licht blauw op; iemand heeft de fysica op shuffle gezet.",
        "De hub stuurt pings en krijgt echo’s terug met accent; dit is geen lokale storing.",
        "In het dok drijft een schaduw zonder massa; de camera noemt het 'mogelijk vriendelijk'.",
        "Het onderhoudslog bevat een taak die niemand heeft ingepland—met jouw naam erop.",
        "Een vergeten beacon meldt zich na jaren stilte en vraagt om koffie en aandacht.",
        "De kunstmatige dageraad flikkert; de simulatie heeft heimwee naar echt licht.",
        "De lift vraagt om een bestemming die niet in de software bestaat.",
        "We vangen een signatuur op die lijkt op ons—maar dan vóór we bestonden."
            ]),
            ("s2", [
                "Het venster voor veilige interventie is smal; na T+12 sluit de omgeving zich als een vuist.",
        "Containment draait op beleefdheidsniveau; nog één fout en het wordt eerlijk.",
        "Energiepieken rimpelen door de ringen; inertie houdt niet van verrassingen.",
        "Vertraging geeft het systeem tijd om creatief te worden; dat wil je niet.",
        "De sensorsuite vraagt al om herstart; straks vraagt hij om een priester.",
        "We zitten één fout van een onbetaalde les in kosmologie.",
        "De drift groeit; hoe later je grijpt, hoe verder het glijdt.",
        "De AI knippert te snel; dat is haar manier van zweten.",
        "Alarmen fluisteren nog; straks gaan ze zingen.",
        "We lenen stabiliteit van morgen; graag vandaag terugbrengen."
            ]),
            ("s3", [
                "Traceer de anomalie, isoleer haar domein, en kalibreer de omgeving terug naar gezond verstand.",
        "Herstel de primaire loop en laat de randgevallen weer keurig aan de rand staan.",
        "Scan, verifieer, patch; leg alles vast wat schreeuwt of te keurig zwijgt.",
        "Plaats bakenen, zet soft-locks, en sluit de lus op jouw voorwaarden.",
        "Verzamel diagnostische breadcrumbs en breng ze samen tot één eerlijk verhaal.",
        "Reconfigureer de sector en geef het systeem weer iets om in te geloven.",
        "Snij de ruis uit het signaal en laat het overblijvende zichzelf verraden.",
        "Herstart alleen wat terugkomt; markeer de rest als folklore.",
        "Zet een sandbox om het onbekende en leer het goede manieren.",
        "Stuur het protocol vooruit en de paniek achteruit."
            ]),
            ("s4", [
                "Neon ademt in het ritme van koelmotoren; elk licht weet meer dan het zegt.",
        "De gang ruikt naar ozon en oude software; nostalgie met een bijsmaak van risico.",
        "Ramen tonen sterren als stil publiek; applaus krijg je hier niet, alleen vacuüm.",
        "Een drone zweeft aan de rand van zijn pad, alsof nieuwsgierigheid een bug is.",
        "De vloerplaten zingen zacht; een oud schip, een nieuwe melodie.",
        "Hologrammen knipperen, net genoeg om te doen alsof ze leven.",
        "Een koelblauwe mist rolt uit een ventilatieas alsof hij iets komt uitleggen.",
        "De corridor is schoon, té schoon; alsof een poetsprotocol zijn ziel heeft verkocht.",
        "Overal zachte piepjes; kalm, maar met de energie van roddels.",
        "De schaduw van een planeet schuift over het dok; iedereen doet even alsof het normaal is."
            ]),
            ("s5", [
                "Werk precies; de ruimte straft benaderingen met wiskunde.",
        "Wees vriendelijk voor systemen; ze onthouden alles, vooral wrok.",
        "Test wat je gelooft en geloof wat blijft staan na het testen.",
        "Lees de logboeken zoals een priester zijn boek; er staat meer tussen de regels.",
        "Minimaliseer drama, maximaliseer constanten.",
        "Maak het simpel genoeg om morgen te begrijpen.",
        "Als iets te mooi draait om waar te zijn, log je te weinig.",
        "Curiositeit eerst, overtuiging later; omwisselen mag niet.",
        "Fouten zijn data met attitude; behandel ze beleefd.",
        "Laat elegantie het bijproduct zijn van noodzaak."
            ]),
            ("s6", [
                "Sluit je suit, sync je HUD, en vraag toestemming om het onbekende netjes te maken.",
        "Start de diagnostiek, zet je timer op nuchter, en stap de corridor in.",
        "Neem het korte pad langs de luchtlocks; elegantie is voor later.",
        "Ping je team, claim het raam, en ga voordat de kans dichtklapt.",
        "Laat je zwaarte achter bij de deur; neem alleen massa mee die beweegt.",
        "Vergrendel twijfel achter een wachtwoord dat je nu even vergeet.",
        "Kies de stille modus; laat het resultaat voor jou spreken.",
        "Bevestig je route, vergrendel je keuze, en zet de eerste stap alsof hij vanzelf ging.",
        "Upload later een rapport; upload nu jezelf naar de missie.",
        "Ga. De ruimte heeft geen geduld, maar wél respect voor afmakers."
            ])
        );

        // --------------------
        // DETECTIVE / FILM NOIR
        // --------------------
        AddFragment(context, tidD,
            ("d1", [
                "De regen was goedkoop vanavond, maar nat genoeg om verhalen los te weken.",
        "De stad rook naar natte kranten en halve waarheden.",
        "Het begon zoals altijd: met een deur die piepte alsof hij wist wat erachter zat.",
        "Ze stapte binnen met ogen die koffie sterker konden maken.",
        "De rook hing dik, net als de leugens.",
        "De zaak begon met een belletje en eindigde met een kater, zoals altijd.",
        "Iemand had iets laten vallen, en het stonk naar problemen.",
        "Mijn kantoor was koud, maar de klus klonk warm betaald.",
        "Het was laat genoeg dat de stad alleen nog de verkeerde mensen overhield.",
        "Ze zei dat het simpel was. Ze loog."
            ]),
            ("d2", [
                "De klok tikte harder dan mijn hartslag, en dat was geen compliment.",
        "Wachten was gevaarlijker dan lopen.",
        "Ergens daarbuiten werd mijn naam gefluisterd, en niet zacht.",
        "Iedere minuut gaf de vijand een nieuwe alibi.",
        "Het soort klus dat koud werd als je te lang keek.",
        "De lucht hing vol antwoorden die ik niet wilde horen.",
        "Traag zijn betekende dat iemand anders sneller was.",
        "Ik rook problemen, en ze waren vers.",
        "Ergens sloot een deur die ik beter open had gehouden.",
        "Het raam van geluk stond op een kier, en de wind was niet van mijn kant."
            ]),
            ("d3", [
                "Volg het spoor, maar laat je niet zien.",
        "Vang de rat voordat hij het schip verlaat.",
        "Zoek uit wie liegt en wie beter liegt.",
        "Haal het bewijs, gooi de rest weg.",
        "Praat met de juiste mensen op het verkeerde moment.",
        "Ontmasker de dader, maar hou hem in de schaduw.",
        "Vind de waarheid, verberg hem indien nodig.",
        "Kijk waar niemand kijkt, luister waar iedereen praat.",
        "Snij de leugen open en kijk wat eruit loopt.",
        "Volg het geld, ook als het stinkt."
            ]),
            ("d4", [
                "De straatlantaarns flikkerden alsof ze twijfelden aan hun baan.",
        "Een kat schoot weg, bang voor meer dan ik.",
        "De regen waste niets schoon.",
        "De stad ademde langzaam, maar rookte snel.",
        "Verkeerslichten knipperden op ritme met mijn twijfel.",
        "Het asfalt was nat, de verhalen droog.",
        "Een sirene huilde in de verte, voor iemand anders.",
        "De stad sliep, maar niet diep.",
        "Een straatveger keek weg toen ik langs kwam.",
        "De lucht rook naar bliksem zonder regen."
            ]),
            ("d5", [
                "In mijn werk win je zelden, je verliest alleen trager.",
        "Vertrouw niemand, zelfs jezelf niet.",
        "Eerlijkheid is duurder dan kogels.",
        "Soms moet je vies spelen om schoon te winnen.",
        "Iedereen heeft een prijs, sommige hoger dan ze waard zijn.",
        "Je kunt geen schaduw volgen in het donker.",
        "De waarheid is zelden gezellig gezelschap.",
        "Als het te makkelijk gaat, ben je vergeten wie je tegenwerkt.",
        "Altijd achterom kijken is vermoeiend, maar soms overleef je erdoor.",
        "Soms is zwijgen de luidste keuze."
            ]),
            ("d6", [
                "Trek je jas aan en laat de rest achter.",
        "Pak je notitieboek, we gaan de waarheid opschrijven voordat iemand hem steelt.",
        "Vul je aansteker, dit wordt een lange nacht.",
        "Zet je hoed op, trek de regen in, en zoek antwoorden.",
        "Laat je geweten thuis, het gaat toch in de weg lopen.",
        "Stap naar buiten voordat het plan verandert.",
        "Neem een foto van je gezicht, voor het geval we je moeten zoeken.",
        "Haal diep adem, je hebt hem straks nodig.",
        "Vergeet het huisnummer, onthoud de gezichten.",
        "Ga nu, voordat het donker nieuwsgierig wordt."
            ])
        );

        // --------------------
        // POST-APOCALYPTIC
        // --------------------
        AddFragment(context, tidP,
            ("p1", [
                "De zon hing dof achter stofwolken, als een lamp met te veel verleden.",
        "De lucht smaakte naar roest en herinneringen.",
        "Iemand had de wereld uitgezet, maar de rommel bleef aan.",
        "De wind droeg meer as dan verhalen.",
        "Een skelet van een gebouw knarste bij elke zucht.",
        "Het landschap was een foto die te lang in de zon had gelegen.",
        "Niets bewoog, behalve de dingen die dat niet moesten.",
        "De stilte was niet leeg, alleen vol dingen die geen geluid meer maken.",
        "Je hoorde de aarde kraken onder haar eigen gewicht.",
        "Zelfs het stof leek moe van zichzelf."
            ]),
            ("p2", [
                "De filters piepten; je had nog lucht voor een goed verhaal of een slecht einde.",
        "Elke minuut hier was een weddenschap met straling.",
        "De zon zakte snel, en 's nachts had de wereld meer tanden.",
        "Je telde kogels en twijfelde wie sneller op waren.",
        "De Geigerteller klonk als regen op een metalen dak—te gezellig om goed te zijn.",
        "Je waterfilter had kuren, en dit was geen plek om dorst te krijgen.",
        "Wie te langzaam beweegt, eindigt als decorstuk.",
        "Er waren meer voetstappen dan mensen.",
        "De lucht werd dikker; dat was geen regen die eraan kwam.",
        "Zelfs de ratten trokken weg, en die weten het altijd eerder."
            ]),
            ("p3", [
                "Vind de capsule voordat iemand zonder geweten hem vindt.",
        "Zoek eten, water, en iets dat op hoop lijkt.",
        "Herstel de generator en hoop dat hij je niet opeet.",
        "Verken de ruïnes en neem alleen mee wat je kunt dragen én verdedigen.",
        "Breng het bericht naar de kolonie, ook als ze je niet geloven.",
        "Ontmantel de val voordat de val jou ontmantelt.",
        "Verzamel de brandstof en deel alleen als het moet.",
        "Beveilig de doorgang voor de karavaan.",
        "Haal de onderdelen op, al liggen ze in verboden grond.",
        "Vind uit wie je volgt, en waarom ze ademen alsof het hun laatste keer is."
            ]),
            ("p4", [
                "De wind floot door lege ramen als een hongerige hond.",
        "De weg kraakte onder gebarsten asfalt.",
        "Een wolk van giftig stof trok voorbij als een luie storm.",
        "Het bos was dood, maar nog steeds vol ogen.",
        "De rivier borrelde iets dat geen water was.",
        "De lucht gloeide zacht, alsof hij zich schaamde.",
        "Kapot glas knisperde onder je laarzen.",
        "Een billboard stond nog, zijn boodschap verbleekt tot ruis.",
        "De lucht rook naar verbrand plastic en natte aarde.",
        "Schaduwen bewogen zonder dat er iets omheen liep."
            ]),
            ("p5", [
                "Alles heeft waarde, tot het breekt.",
        "Vertrouw alleen wat je zelf hebt gerepareerd.",
        "Delen is nobel, maar nobelen gaan vroeg dood.",
        "Elke kogel is een belofte die je maar één keer kunt nakomen.",
        "Schoon water is waardevoller dan waarheid.",
        "Als je iets niet kunt dragen, laat het achter voor iemand die dat wel kan.",
        "Beloftes tellen minder dan voorraad.",
        "Overleven is geen wedstrijd, maar je kunt wel verliezen.",
        "Alles wat glimt trekt aandacht, meestal de verkeerde.",
        "Niemand leeft hier zonder vuile handen."
            ]),
            ("p6", [
                "Laad je wapen en neem alleen mee wat je niet kunt missen.",
        "Check je masker en stap in de wind.",
        "Maak je tas lichter, je pad zal dat niet zijn.",
        "Zet je stappen snel, de grond houdt niet van gasten.",
        "Neem afscheid alsof je niet terugkomt.",
        "Doe het licht uit achter je, het is al donker genoeg.",
        "Loop door, ook als de weg je niet kent.",
        "Pak je kans, voor iemand anders hem inpakt.",
        "Vergeet wat je achterlaat, onthoud waar je heen gaat.",
        "Ga, voordat de horizon beslist dat je blijft."
            ])
        );

        // --------------------
        // LOVECRAFTIAN HORROR
        // --------------------
        AddFragment(context, tidL,
            ("l1", [
                "Aan de rand van het dorp gromde de zee zonder golven, alsof iets onder het water ademhaalde.",
        "De maan hing scheef en glimlachte verkeerd; iemand had haar leren lachen.",
        "Een kaart van de kust tekende vannacht zichzelf opnieuw, met lijnen die niemand had gevraagd.",
        "Het museum sluit vroeg, maar de vitrines fluisteren door; namen met te veel medeklinkers.",
        "In de kelder van de bibliotheek is een trap die nergens heen gaat—behalve naar beneden.",
        "De vissers kwamen terug zonder vangst, maar met een verhaal dat niet wilde meewerken.",
        "Een kind tekende een cirkel met twaalf staarten; niemand durfde te vragen waar de staarten van waren.",
        "De wind sprak in een dialect dat zelfs de duinen niet herkenden.",
        "Een drooggevallen put zong één toon, precies tussen troost en waarschuwing.",
        "De sterren deden hun best om niet te knipperen; het was niet hun beurt om te bewegen."
            ]),
            ("l2", [
                "Tijd loopt hier niet recht; hij wurmt zich door sleutelgaten en laat sporen achter.",
        "Wie te lang luistert, onthoudt dingen die hij nooit geleerd heeft.",
        "De lucht is zwaar van beloften die niemand heeft gedaan.",
        "De stad sluit zijn ogen, maar niet om te slapen.",
        "Elke minuut zonder antwoord is een uitnodiging voor iets dat niet schrijft.",
        "De kade telt voetstappen die niet in de richting horen te gaan.",
        "Spiegels worden nerveus in kleine kamers; ze houden niet van medekijkers.",
        "Wie hier stil staat, wordt opgemerkt door dingen zonder ogen.",
        "De kleren op de lijn bewegen tegen de wind in; iemand groet terug.",
        "Er is haast, maar niemand weet waarvan we te laat komen."
            ]),
            ("l3", [
                "Daal in waar de kaarten wit worden en noteer wat wit probeert te verbergen.",
        "Zoek de bron van het geluid dat te laag is om te horen, maar hoog genoeg om te missen.",
        "Herplaats de stenen in het patroon dat niemand wil herkennen.",
        "Open het boek dat zichzelf niet wil openen en lees alleen de lege bladzijden.",
        "Breng terug wat de zee niet wil houden en leg het waar het niet kan liggen.",
        "Verzwijg de naam die je straks leert; spreek hem nooit voluit.",
        "Bezoek de kelder die boven je ligt en tel de treden die je niet nam.",
        "Markeer de plek waar schaduwen naar binnen vallen, tegen het licht in.",
        "Vraag aan de priemgetallen waarom ze vandaag samen willen zijn.",
        "Leg stilte neer op de drempel en kijk wie eroverheen stapt."
            ]),
            ("l4", [
                "De haven ruikt naar kelp en oud ijzer; water dat langer leeft dan mensen.",
        "Muren zweten zout in kamers zonder ramen.",
        "Een lantaarn brandt met een kleur die geen naam wil dragen.",
        "Planken kraken op plekken waar niemand loopt; hout onthoudt meer dan wij.",
        "Ver op zee gloeit een horizon die hier niet thuishoort.",
        "De mist heeft gewicht, en hij kiest wie hij draagt.",
        "Er hangt een kaart aan de muur die elke nacht een nieuw eiland tekent.",
        "In de kerk tikt iets mee met de klok, maar net na de seconde.",
        "De sterren spiegelen niet in het water; het water kijkt terug.",
        "Onder de pier zingt een koor zonder mond; het kent je naam niet, maar oefent wel."
            ]),
            ("l5", [
                "Geloof wat je ziet pas nadat je het hebt tegengesproken.",
        "Noem niets bij zijn echte naam; niet vandaag.",
        "Breek de cirkel alleen om hem groter te maken.",
        "Draag stilte als wapen en twijfel als schild.",
        "Wie antwoorden zoekt, moet eerst leren vergeten.",
        "De grens tussen nieuwsgierigheid en roep is dunner dan een blad papier.",
        "Loop langzaam, zodat tijd je niet inhaalt.",
        "Vertrouw op touw, niet op trappen; touw liegt minder vaak.",
        "Kijk niet te lang terug; sommige blikken komen mee.",
        "Wees beleefd tegen deuren; ze onthouden wie bonkt."
            ]),
            ("l6", [
                "Neem een lamp en een leugen mee, en kies welke je eerst gebruikt.",
        "Vouw je moed op tot een zakdoek en steek hem weg; je zult hem nodig hebben.",
        "Trek de kelderdeur open en tel hardop tot je er niet meer bent.",
        "Ga naar de kade voor de eerste mist, en luister niet naar je naam.",
        "Knoop het touw vast aan iets dat hier gebleven is, en daal af.",
        "Schrijf een brief aan jezelf en laat hem ongeopend achter.",
        "Neem zout, een sleutel, en de gewoonte om zacht te lopen.",
        "Als de klok slaat, ga; als zij zwijgt, ga sneller.",
        "Laat het licht aan voor wie terugkomt, ook als jij dat niet bent.",
        "Ga nu. Laat de zee het laatste woord houden."
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

        #region Detective

        // ===== Common pools =====
        var D_Code = new[] { "CASE-01", "CASE-02", "CASE-03", "CASE-04", "CASE-05", "CASE-06", "CASE-07", "CASE-08", "CASE-09", "CASE-10" };
        var D_Label = new[] { "KOUD", "LAUW", "WARM", "HEET", "NAT", "STOF", "GLAD", "DONKER", "DRUK", "STIL" };
        var D_Prioriteit = new[] { "laag", "middel", "hoog", "kritiek", "spoed", "routine", "preventief", "nachtklus", "ochtendklus", "eind-van-de-dag" };

        // ============ DETECTIVE → TOILET ============
        var D_Toilet_sector = new[] { "WC-cabine 1","Cabine B","Porseleinen troon","Achterste hok","Kleine kamer",
            "Sanitair achterzaal","Het hok","Latrine links","Latrine rechts","Hoekcabine" };
        var D_Toilet_activiteit = new[] { "stille lekkage","vage geur","spatten op tegels","ketting kraakt","spoeling traag",
            "vlekken zonder alibi","papieren spoor","kalkring groeit","ventilatie doet schimmig","deksel wiebelt" };
        var D_Toilet_status = new[] { "verdacht: laag","verdacht: middel","verdacht: hoog","scene verstoord","glans onder norm",
            "luchtkwaliteit twijfelachtig","gebruik gepauzeerd","rand beschadigd","inspectie nodig","situatie beheersbaar" };
        var D_Toilet_actie = new[] { "borstel & bleek","ontkalk rand","tegels dweilen","scharnier smeren","dubbel spoelen",
            "lucht verversen","waterlijn polijsten","geurfilter wisselen","sporen wissen","eindcontrole & vrijgave" };
        var D_Toilet_observatie = new[] { "water twijfelt in de kom","kalk werkt aan een legende","geur wijkt af van normaal",
            "spatten buiten patroon","druk valt weg bij spoelen","vlek zonder geschiedenis","deksel piept bij aanhouding",
            "waterlijn toont troebel beeld","sensor ziet microresidu","ventilatie bromt te laag" };
        var D_Toilet_risico = new[] { "glijpartij","klacht bij bewoner","geurincident","hygiënegat","rapportgevoelig",
            "oppervlakteslijtage","handschoenbreuk","onvrede bij bezoek","routine-afwijking","tijdverlies" };
        var D_Toilet_procedure = new[] { "borstelprotocol uitvoeren","ontkalk-cyclus starten","volledige desinfectie",
            "ventilatie verhogen en resetten","dubbele spoel zetten","scharnieren smeren","vlekken neutraliseren",
            "waterlijn polijsten","geurfilter wisselen","eindcontrole doen" };

        AddTemplates(context, themeId["Detective"], CategoryType.Room, roomCatId["Toilet"],
            Tpl("Plek @sector meldt @activiteit. Status: @status. Actie: @actie.",
                ("sector", D_Toilet_sector), ("activiteit", D_Toilet_activiteit), ("status", D_Toilet_status), ("actie", D_Toilet_actie)
            ),
            Tpl("Locatie: @sector. Observatie: @observatie. Orders: @actie.",
                ("sector", D_Toilet_sector), ("observatie", D_Toilet_observatie), ("actie", D_Toilet_actie)
            ),
            Tpl("Dossier @code — @sector: @activiteit. Risico: @risico. Maatregel: @actie.",
                ("code", D_Code), ("sector", D_Toilet_sector), ("activiteit", D_Toilet_activiteit), ("risico", D_Toilet_risico), ("actie", D_Toilet_actie)
            ),
            Tpl("Scene @sector. Toestand: @status. Uitvoering: @procedure.",
                ("sector", D_Toilet_sector), ("status", D_Toilet_status), ("procedure", D_Toilet_procedure)
            ),
            Tpl("Checkpoint @sector actief. Prioriteit: @prioriteit. Procedure: @procedure.",
                ("sector", D_Toilet_sector), ("prioriteit", D_Prioriteit), ("procedure", D_Toilet_procedure)
            ),
            Tpl("Kaartupdate — @sector gelabeld @label. Situatie: @status. Volgende stap: @actie.",
                ("sector", D_Toilet_sector), ("label", D_Label), ("status", D_Toilet_status), ("actie", D_Toilet_actie)
            )
        );

        // ============ DETECTIVE → KEUKEN ============
        var D_Keuken_sector = new[] { "Aanrecht oost","Kookplaat","Koffiehoek","Koelkastvak","Achterkamer",
            "Afzuigkap","Spoelbak","Snijtafel","Kruidenrek","Vaatrek" };
        var D_Keuken_activiteit = new[] { "vetlaag in het zicht","filter zucht","kruimelroute actief","pannen zwart geblakerd","damp blijft hangen",
            "vloer wil glijden","handgrepen plakken","geur van gisteren","condens op glas","spatzone rond fornuis" };
        var D_Keuken_status = new[] { "hygiëne: twijfel","luchtstroom laag","oppervlakken dof","hitte ongelijk","randen vergeten",
            "sensoren vuil","inspectie pending","tijd voor reset","risico: laag-middel","op te leveren" };
        var D_Keuken_actie = new[] { "ontvetten & desinfecteren","filters resetten","roosters borstelen","kruimels vegen","moppen antislip",
            "grepen polijsten","afzuig boosten","pannen reconditioneren","temperatuur loggen","vrijgave 'FoodSafe'" };
        var D_Keuken_observatie = new[] { "vetfilm op blad","filterdruk te hoog","kruimels bij snijzone","pannen dragen koollaag","afzuig debiet laag",
            "vloer toont sliprisico","grepen voelen kleverig","kooklucht blijft hangen","condens in koelkast","spatzone actief" };
        var D_Keuken_risico = new[] { "kruisbesmetting","brandvlekken","slipgevaar","sensorfout","warmte-accumulatie",
            "inspectierisico","luchtkwaliteit","krasvorming","glasschade","hygiënelek" };
        var D_Keuken_procedure = new[] { "ontvet-cyclus","filterwissel","roosterreiniging","dieptereiniging blad","antislip-mop",
            "handgreeppolijst","afzuigboost","pannenreanimatie","thermolog","eindcontrole" };

        AddTemplates(context, themeId["Detective"], CategoryType.Room, roomCatId["Keuken"],
            Tpl("Locatie @sector meldt @activiteit. Status: @status. Actie: @actie.",
                ("sector", D_Keuken_sector), ("activiteit", D_Keuken_activiteit), ("status", D_Keuken_status), ("actie", D_Keuken_actie)
            ),
            Tpl("Plaats: @sector. Observatie: @observatie. Orders: @actie.",
                ("sector", D_Keuken_sector), ("observatie", D_Keuken_observatie), ("actie", D_Keuken_actie)
            ),
            Tpl("Dossier @code — @sector: @activiteit. Risico: @risico. Maatregel: @actie.",
                ("code", D_Code), ("sector", D_Keuken_sector), ("activiteit", D_Keuken_activiteit), ("risico", D_Keuken_risico), ("actie", D_Keuken_actie)
            ),
            Tpl("Scene @sector. Toestand: @status. Uitvoering: @procedure.",
                ("sector", D_Keuken_sector), ("status", D_Keuken_status), ("procedure", D_Keuken_procedure)
            ),
            Tpl("Checkpoint @sector actief. Prioriteit: @prioriteit. Procedure: @procedure.",
                ("sector", D_Keuken_sector), ("prioriteit", D_Prioriteit), ("procedure", D_Keuken_procedure)
            ),
            Tpl("Kaartupdate — @sector gelabeld @label. Situatie: @status. Volgende stap: @actie.",
                ("sector", D_Keuken_sector), ("label", D_Label), ("status", D_Keuken_status), ("actie", D_Keuken_actie)
            )
        );

        // ============ DETECTIVE → GANG ============
        var D_Gang_sector = new[] { "Hal noord","Portiek","Trapopgang","Voordeurzone","Achterdeur",
            "Langs de plint","Middenpad","Hoek bij meter","Deurmat","Overloop" };
        var D_Gang_activiteit = new[] { "stof nestelt zich","mat verzadigd","schakelaar vet","armaturen dof","deurrails piepen",
            "hoek verzamelt web","zand sluipt mee","lint op route","strepen na regen","echo te luid" };
        var D_Gang_status = new[] { "doorloop traag","zicht: oké","geluid boven norm","frictie toegenomen","aanzicht laag",
            "sliprisico: licht","toegang belemmerd","controle nodig","veiligheid gewaarborgd","inspectie op komst" };
        var D_Gang_actie = new[] { "Z-vegen & nat moppen","matten resetten","touchpoints reinigen","armaturen poetsen","rails borstelen/smeren",
            "hoeken detailen","zandbarrière plaatsen","lint verwijderen","vloer neutraliseren","doorgang vrijgeven" };
        var D_Gang_observatie = new[] { "plint vangt stof","matten verliezen grip","schakelaars glanzen verkeerd","armaturen ogen dof","rails piepen bij belasting",
            "hoeken missen routine","zand vanaf ingang","losse linten","strepen na nat weer","galm in de hal" };
        var D_Gang_risico = new[] { "slipgevaar","struikelkans","doorstroomverlies","stofallergie","zichtverlies",
            "plintschade","elektrisch risico","valgevaar trap","klachtenbeleving","inspectie-afkeur" };
        var D_Gang_procedure = new[] { "Z-vegencyclus","mat-reset","schakelaar-sanitatie","armatuurpolijst","railbrush + lube",
            "hoek-detail","zandbarrière","lintverwijdering","friction normaliseren","eindcontrole" };

        AddTemplates(context, themeId["Detective"], CategoryType.Room, roomCatId["Gang"],
            Tpl("Route @sector meldt @activiteit. Status: @status. Actie: @actie.",
                ("sector", D_Gang_sector), ("activiteit", D_Gang_activiteit), ("status", D_Gang_status), ("actie", D_Gang_actie)
            ),
            Tpl("Plaats: @sector. Observatie: @observatie. Orders: @actie.",
                ("sector", D_Gang_sector), ("observatie", D_Gang_observatie), ("actie", D_Gang_actie)
            ),
            Tpl("Dossier @code — @sector: @activiteit. Risico: @risico. Maatregel: @actie.",
                ("code", D_Code), ("sector", D_Gang_sector), ("activiteit", D_Gang_activiteit), ("risico", D_Gang_risico), ("actie", D_Gang_actie)
            ),
            Tpl("Scene @sector. Toestand: @status. Uitvoering: @procedure.",
                ("sector", D_Gang_sector), ("status", D_Gang_status), ("procedure", D_Gang_procedure)
            ),
            Tpl("Checkpoint @sector actief. Prioriteit: @prioriteit. Procedure: @procedure.",
                ("sector", D_Gang_sector), ("prioriteit", D_Prioriteit), ("procedure", D_Gang_procedure)
            ),
            Tpl("Kaartupdate — @sector gelabeld @label. Situatie: @status. Volgende stap: @actie.",
                ("sector", D_Gang_sector), ("label", D_Label), ("status", D_Gang_status), ("actie", D_Gang_actie)
            )
        );

        // ============ DETECTIVE → SLAAPKAMER ============
        var D_Slaap_sector = new[] { "Bedzijde","Kastwand","Nachtkast","Raamkozijn","Vloerzone",
            "Spiegelhoek","Gordijnrand","Onder het bed","Deurpost","Hoek bij radiator" };
        var D_Slaap_activiteit = new[] { "stoflaag meldt zich","kussens uit lijn","kozijnen stoffig","gordijnrand dof","pluis op mars",
            "grepen vettig","spiegel met vlekken","nachtkast kruimelt","streepjes op vloer","lucht staat stil" };
        var D_Slaap_status = new[] { "comfort onder norm","visuele ruis aanwezig","luchtkwaliteit matig","slaaphygiëne middel","aanzicht mager",
            "geluid nihil","reset gewenst","ordening nodig","risico: laag","controle kan volgen" };
        var D_Slaap_actie = new[] { "linnen strak trekken","diepzuigen & vegen","kozijn reinigen","kussens opschudden","mop langzaam",
            "grepen polijsten","spiegel helder maken","kasthoek ordenen","ventilatie boost","ruststatus loggen" };
        var D_Slaap_observatie = new[] { "stofnesten bij poten","plooien asymmetrisch","pollen op frame","gordijn geeft stof af","pluisbanen naar deur",
            "grepen tonen vingers","spiegelwaas zichtbaar","microkruimels op kast","streepjes na sokken","lucht muf" };
        var D_Slaap_risico = new[] { "allergenen","comfortverlies","stootgevaar","uitglijden","luchtkwaliteit",
            "presentatieverlies","inspectie-delay","pluisaccumulatie","oogirritatie","rustverstoring" };
        var D_Slaap_procedure = new[] { "linnen-aanspanning","deep vacuum","kussen-reset","kozijn-wipe","low-speed mop",
            "greep-polish","spiegelpolijst","kasthoek-organize","vent-boost","eindrapport rust" };

        AddTemplates(context, themeId["Detective"], CategoryType.Room, roomCatId["Slaapkamer"],
            Tpl("Kamer @sector meldt @activiteit. Status: @status. Actie: @actie.",
                ("sector", D_Slaap_sector), ("activiteit", D_Slaap_activiteit), ("status", D_Slaap_status), ("actie", D_Slaap_actie)
            ),
            Tpl("Plaats: @sector. Observatie: @observatie. Orders: @actie.",
                ("sector", D_Slaap_sector), ("observatie", D_Slaap_observatie), ("actie", D_Slaap_actie)
            ),
            Tpl("Dossier @code — @sector: @activiteit. Risico: @risico. Maatregel: @actie.",
                ("code", D_Code), ("sector", D_Slaap_sector), ("activiteit", D_Slaap_activiteit), ("risico", D_Slaap_risico), ("actie", D_Slaap_actie)
            ),
            Tpl("Scene @sector. Toestand: @status. Uitvoering: @procedure.",
                ("sector", D_Slaap_sector), ("status", D_Slaap_status), ("procedure", D_Slaap_procedure)
            ),
            Tpl("Checkpoint @sector actief. Prioriteit: @prioriteit. Procedure: @procedure.",
                ("sector", D_Slaap_sector), ("prioriteit", D_Prioriteit), ("procedure", D_Slaap_procedure)
            ),
            Tpl("Kaartupdate — @sector gelabeld @label. Situatie: @status. Volgende stap: @actie.",
                ("sector", D_Slaap_sector), ("label", D_Label), ("status", D_Slaap_status), ("actie", D_Slaap_actie)
            )
        );

        // ============ DETECTIVE → DOUCHE ============
        var D_Douche_sector = new[] { "Doucheruimte","Glaswand","Afvoerput","Kraanwerk","Tegelveld",
            "Silicone rand","Voegstrook","Sproeikop","Spiegel","Ventilatiehoek" };
        var D_Douche_activiteit = new[] { "kalk wint terrein","zeepfilm sluipt","afvoer loopt traag","haren in rooster","straal schiet scheef",
            "tegels glad","voegen verduisteren","silicone mat","spiegelwaas blijft","ventilatie zucht" };
        var D_Douche_status = new[] { "zicht beperkt","flow verstoord","veiligheidsrisico: licht","onderhoud vereist","glans laag",
            "geur aanwezig","sensor alert","gebruik beperkt","herstel haalbaar","omgeving nat" };
        var D_Douche_actie = new[] { "ontkalk & schrob voegen","rooster openen en reinigen","sproeikoppen reinigen","antislip-moppen","glas polijsten",
            "kranen laten glanzen","ventilatie verhogen","drogen & nalopen","zone markeren 'fris'","hydro afsluiten" };
        var D_Douche_observatie = new[] { "kalkkraag rond kraan","film over glas","water blijft staan","rooster vangt vezels","jets spatten scheef",
            "tegel voelt glad","voeg toont schaduw","silicone stroef","spiegel blijft nevelig","fan blijft achter" };
        var D_Douche_risico = new[] { "slipgevaar","schimmelgroei","verstopping","kalkaanslag","geurincident",
            "glasbreuk","corrosie","waterlek","zichtverlies","ventilatie-overbelasting" };
        var D_Douche_procedure = new[] { "decalcify","grout-scrub","trap-clean","nozzle-clean","anti-slip mop",
            "glass polish","hardware polish","vent high","dry & inspect","close hydro" };

        AddTemplates(context, themeId["Detective"], CategoryType.Room, roomCatId["Douche"],
            Tpl("Scene @sector meldt @activiteit. Status: @status. Actie: @actie.",
                ("sector", D_Douche_sector), ("activiteit", D_Douche_activiteit), ("status", D_Douche_status), ("actie", D_Douche_actie)
            ),
            Tpl("Plaats: @sector. Observatie: @observatie. Orders: @actie.",
                ("sector", D_Douche_sector), ("observatie", D_Douche_observatie), ("actie", D_Douche_actie)
            ),
            Tpl("Dossier @code — @sector: @activiteit. Risico: @risico. Maatregel: @actie.",
                ("code", D_Code), ("sector", D_Douche_sector), ("activiteit", D_Douche_activiteit), ("risico", D_Douche_risico), ("actie", D_Douche_actie)
            ),
            Tpl("Scene @sector. Toestand: @status. Uitvoering: @procedure.",
                ("sector", D_Douche_sector), ("status", D_Douche_status), ("procedure", D_Douche_procedure)
            ),
            Tpl("Checkpoint @sector actief. Prioriteit: @prioriteit. Procedure: @procedure.",
                ("sector", D_Douche_sector), ("prioriteit", D_Prioriteit), ("procedure", D_Douche_procedure)
            ),
            Tpl("Kaartupdate — @sector gelabeld @label. Situatie: @status. Volgende stap: @actie.",
                ("sector", D_Douche_sector), ("label", D_Label), ("status", D_Douche_status), ("actie", D_Douche_actie)
            )
        );

        #endregion

        #region PostApocalyptic

        // ===== Common pools =====
        var P_Code = new[] { "CACHE-01", "CACHE-02", "CACHE-03", "CACHE-04", "CACHE-05", "CACHE-06", "CACHE-07", "CACHE-08", "CACHE-09", "CACHE-10" };
        var P_Label = new[] { "DROOG", "STOF", "ROOD", "GEVAAR", "SCHAARS", "VEILIG", "TWEEDE KEUZE", "KRITIEK", "SPOED", "RUST" };
        var P_Prioriteit = new[] { "laag", "middel", "hoog", "kritiek", "spoed", "routine", "preventief", "schaduwdienst", "daglicht", "schemer" };

        /// ============ POST-APOC → TOILET ============
        var P_Toilet_sector = new[] {
            "Latrine 3","Sanibox B","Putdeksel Noord","Tank-WC","Noodhok","Keramiekrest A","Spoelton","Zinkgat","Hoekcabine","Filterbak"
};
        var P_Toilet_activiteit = new[] {
            "pomp hapert","kalk van vóór de val","geur lekt door","ketting vast","water roestig","deksel scheef",
            "vlekken hardnekkig","afvoer traag","ventilatie stil","sensoren zwijgen"
};
        var P_Toilet_status = new[] {
            "risico: middel","glans is verleden tijd","perimeter instabiel","inspectie noodzakelijk","gereedschap vereist",
            "luchtkwaliteit laag","gebruik gepauzeerd","rand beschadigd","oranje markering","situatie beheersbaar"
};
        var P_Toilet_actie = new[] {
            "ontkalk rand & kom","dubbel spoelen (handpomp)","volledige desinfectie","ventileren en resetten","scharnieren smeren",
            "vlekken neutraliseren","waterlijn polijsten","geurfilter wisselen","afvoer vrijmaken","eindcontrole & vrijgave"
        };
        var P_Toilet_observatie = new[] {
            "water aarzelt in de kom","kalk vreet zich terug","geur zit in de voegen","spatten buiten patroon",
            "druk zakt bij spoelen","vlek zonder geschiedenis","deksel scharniert stroef","troebele waterlijn",
            "microresidu op rand","ventilatie bromt niet meer"
        };
        var P_Toilet_risico = new[] {
            "glijgevaar","ziekterisico","geurinvasie","hygiënegat","rapportgevoelig",
            "slijtage oppervlak","handschoenbreuk","bewonersklacht","routine-afwijking","tijdverlies"
        };
        var P_Toilet_procedure = new[] {
            "borstel & ontkalk","desinfectie volledig","ventilatie-boost","dubbele spoel","scharnier-smeer",
            "vlekneutralisatie","waterlijn-polijst","filterwissel","afvoer-schoon","eindcontrole"
        };

        AddTemplates(context, themeId["Post-apocalyptic"], CategoryType.Room, roomCatId["Toilet"],
            Tpl("Zone @sector meldt @activiteit. Status: @status. Actie: @actie.",
                ("sector", P_Toilet_sector), ("activiteit", P_Toilet_activiteit), ("status", P_Toilet_status), ("actie", P_Toilet_actie)
            ),
            Tpl("Locatie: @sector. Observatie: @observatie. Orders: @actie.",
                ("sector", P_Toilet_sector), ("observatie", P_Toilet_observatie), ("actie", P_Toilet_actie)
            ),
            Tpl("Rapport @code — @sector: @activiteit. Risico: @risico. Maatregel: @actie.",
                ("code", P_Code), ("sector", P_Toilet_sector), ("activiteit", P_Toilet_activiteit), ("risico", P_Toilet_risico), ("actie", P_Toilet_actie)
            ),
            Tpl("Sector @sector. Toestand: @status. Uitvoering: @procedure.",
                ("sector", P_Toilet_sector), ("status", P_Toilet_status), ("procedure", P_Toilet_procedure)
            ),
            Tpl("Checkpoint @sector actief. Prioriteit: @prioriteit. Procedure: @procedure.",
                ("sector", P_Toilet_sector), ("prioriteit", P_Prioriteit), ("procedure", P_Toilet_procedure)
            ),
            Tpl("Kaartupdate — @sector gelabeld @label. Situatie: @status. Volgende stap: @actie.",
                ("sector", P_Toilet_sector), ("label", P_Label), ("status", P_Toilet_status), ("actie", P_Toilet_actie)
            )
        );

        /// ============ POST-APOC → KEUKEN ============
        var P_Keuken_sector = new[] {
            "Vuurplaats","Kookvat","Bliktafel","Rantsoenplank","Koffiekrat",
            "Afzuigkap","Spoelbak","Snijbord","Opslagrek","Vaatrek"
        };
        var P_Keuken_activiteit = new[] {
            "vetlaag kruipt terug","filter hoest stof","kruimelpad actief","pannen verkoold","damp blijft hangen",
            "vloer wil glijden","handgrepen plakken","geur van gisteren","condens op deur","spatzone rond vuur"
        };
        var P_Keuken_status = new[] {
            "voedselveiligheid dun","luchtstroom laag","oppervlakken dof","hitte ongelijk","randen vergeten",
            "sensoren smerig","inspectie pending","tijd voor reset","risico: laag-middel","oplevering haalbaar"
        };
        var P_Keuken_actie = new[] {
            "ontvetten & desinfecteren","filters wisselen","roosters borstelen","kruimels vegen","antislip moppen",
            "grepen polijsten","afzuig boosten","pannen reconditioneren","temperatuur loggen","vrijgave 'FoodSafe'"
        };
        var P_Keuken_observatie = new[] {
            "vetfilm op blad","filterdruk te hoog","kruimels bij snijzone","koollaag op pannen","afzuig debiet laag",
            "vloer glad bij ingang","kleverige grepen","kooklucht hangt lang","koelkast condenseert","spatzone actief"
        };
        var P_Keuken_risico = new[] {
            "kruisbesmetting","brandvlekken","slipgevaar","sensorfout","warmte-accumulatie",
            "inspectierisico","luchtkwaliteit","krasvorming","glasschade","hygiënelek"
        };
        var P_Keuken_procedure = new[] {
            "ontvet-cyclus","filterwissel","roosterreiniging","dieptereiniging blad","antislip-mop",
            "handgreeppolijst","afzuig-boost","pannenreanimatie","thermolog","eindcontrole"
        };

        AddTemplates(context, themeId["Post-apocalyptic"], CategoryType.Room, roomCatId["Keuken"],
            Tpl("Zone @sector meldt @activiteit. Status: @status. Actie: @actie.",
                ("sector", P_Keuken_sector), ("activiteit", P_Keuken_activiteit), ("status", P_Keuken_status), ("actie", P_Keuken_actie)
            ),
            Tpl("Locatie: @sector. Observatie: @observatie. Orders: @actie.",
                ("sector", P_Keuken_sector), ("observatie", P_Keuken_observatie), ("actie", P_Keuken_actie)
            ),
            Tpl("Rapport @code — @sector: @activiteit. Risico: @risico. Maatregel: @actie.",
                ("code", P_Code), ("sector", P_Keuken_sector), ("activiteit", P_Keuken_activiteit), ("risico", P_Keuken_risico), ("actie", P_Keuken_actie)
            ),
            Tpl("Sector @sector. Toestand: @status. Uitvoering: @procedure.",
                ("sector", P_Keuken_sector), ("status", P_Keuken_status), ("procedure", P_Keuken_procedure)
            ),
            Tpl("Checkpoint @sector actief. Prioriteit: @prioriteit. Procedure: @procedure.",
                ("sector", P_Keuken_sector), ("prioriteit", P_Prioriteit), ("procedure", P_Keuken_procedure)
            ),
            Tpl("Kaartupdate — @sector gelabeld @label. Situatie: @status. Volgende stap: @actie.",
                ("sector", P_Keuken_sector), ("label", P_Label), ("status", P_Keuken_status), ("actie", P_Keuken_actie)
            )
        );

        /// ============ POST-APOC → GANG ============
        var P_Gang_sector = new[] {
            "Sluispad","Betonstrip","Tunnelmond","Portaal","Trapkuil",
            "Plintstrook","Middenpad","Meterhoek","Deurmat","Overloop"
        };
        var P_Gang_activiteit = new[] {
            "stof trekt sporen","mat verzadigd","schakelaar vettig","armaturen dof","deurrails janken",
            "hoek verzamelt web","zand dringt binnen","lint op route","strepen na regen","galm te luid"
        };
        var P_Gang_status = new[] {
            "doorloop traag","licht: oké","geluid boven norm","frictie toegenomen","aanzicht laag",
            "sliprisico: licht","toegang belemmerd","controle nodig","veiligheid voorlopig","inspectie op komst"
        };
        var P_Gang_actie = new[] {
            "Z-vegen & nat moppen","matten resetten","touchpoints reinigen","armaturen poetsen","rails borstelen/smeren",
            "hoeken detailen","zandbarrière plaatsen","lint verwijderen","vloer neutraliseren","doorgang vrijgeven"
        };
        var P_Gang_observatie = new[] {
            "plint vangt stof","matten verliezen grip","schakelaars glanzen fout","armaturen ogen dof","rails piepen onder last",
            "hoeken missen beurt","zand vanaf ingang","los lint op route","strepen door vocht","echo in nissen"
        };
        var P_Gang_risico = new[] {
            "slipgevaar","struikelkans","doorstroomverlies","stofallergie","zichtverlies",
            "plintschade","elektrisch risico","valgevaar trap","klachtenbeleving","afkeur inspectie"
        };
        var P_Gang_procedure = new[] {
            "Z-vegencyclus","mat-reset","schakelaar-sanitatie","armatuurpolijst","railbrush + lube",
            "hoek-detail","barrière leggen","lintverwijdering","friction normaliseren","eindcontrole"
        };

        AddTemplates(context, themeId["Post-apocalyptic"], CategoryType.Room, roomCatId["Gang"],
            Tpl("Route @sector meldt @activiteit. Status: @status. Actie: @actie.",
                ("sector", P_Gang_sector), ("activiteit", P_Gang_activiteit), ("status", P_Gang_status), ("actie", P_Gang_actie)
            ),
            Tpl("Locatie: @sector. Observatie: @observatie. Orders: @actie.",
                ("sector", P_Gang_sector), ("observatie", P_Gang_observatie), ("actie", P_Gang_actie)
            ),
            Tpl("Rapport @code — @sector: @activiteit. Risico: @risico. Maatregel: @actie.",
                ("code", P_Code), ("sector", P_Gang_sector), ("activiteit", P_Gang_activiteit), ("risico", P_Gang_risico), ("actie", P_Gang_actie)
            ),
            Tpl("Sector @sector. Toestand: @status. Uitvoering: @procedure.",
                ("sector", P_Gang_sector), ("status", P_Gang_status), ("procedure", P_Gang_procedure)
            ),
            Tpl("Checkpoint @sector actief. Prioriteit: @prioriteit. Procedure: @procedure.",
                ("sector", P_Gang_sector), ("prioriteit", P_Prioriteit), ("procedure", P_Gang_procedure)
            ),
            Tpl("Kaartupdate — @sector gelabeld @label. Situatie: @status. Volgende stap: @actie.",
                ("sector", P_Gang_sector), ("label", P_Label), ("status", P_Gang_status), ("actie", P_Gang_actie)
            )
        );

        /// ============ POST-APOC → SLAAPKAMER ============
        var P_Slaap_sector = new[] {
            "Stapelbed","Kratkast","Nachtplank","Luikkozijn","Matrasveld",
            "Spiegelhoek","Gordijnrand","Onderbed","Deurpost","Radiatornis"
        };
        var P_Slaap_activiteit = new[] {
            "stoflaag keert terug","kussens uit lijn","kozijnen stoffig","gordijnrand dof","pluis paradeert",
            "grepen vettig","spiegel vlekkerig","nachtplank kruimelt","vloer streperig","lucht muf"
        };
        var P_Slaap_status = new[] {
            "comfort onder norm","visuele ruis aanwezig","luchtkwaliteit matig","slaaphygiëne middel","aanzicht mager",
            "geluid nihil","reset gewenst","ordening nodig","risico: laag","controle mogelijk"
        };
        var P_Slaap_actie = new[] {
            "linnen strak trekken","diepzuigen & vegen","kozijn reinigen","kussens opschudden","langzame mop",
            "grepen polijsten","spiegel helder maken","kasthoek ordenen","ventilatie boost","ruststatus loggen"
        };
        var P_Slaap_observatie = new[] {
            "stofnesten bij poten","plooien asymmetrisch","pollen op frame","gordijn geeft stof af","pluisbanen naar deur",
            "vingers op grepen","spiegelwaas zichtbaar","microkruimels op plank","streepjes na sokken","lucht staat stil"
        };
        var P_Slaap_risico = new[] {
            "allergenen","comfortverlies","stootgevaar","uitglijden","luchtkwaliteit",
            "presentatieverlies","inspectie-delay","pluisaccumulatie","oogirritatie","rustverstoring"
        };
        var P_Slaap_procedure = new[] {
            "linnen-aanspanning","deep vacuum","kussen-reset","kozijn-wipe","low-speed mop",
            "greep-polish","spiegelpolijst","kast-organize","vent-boost","eindrapport rust"
        };

        AddTemplates(context, themeId["Post-apocalyptic"], CategoryType.Room, roomCatId["Slaapkamer"],
            Tpl("Kwartier @sector meldt @activiteit. Status: @status. Actie: @actie.",
                ("sector", P_Slaap_sector), ("activiteit", P_Slaap_activiteit), ("status", P_Slaap_status), ("actie", P_Slaap_actie)
            ),
            Tpl("Locatie: @sector. Observatie: @observatie. Orders: @actie.",
                ("sector", P_Slaap_sector), ("observatie", P_Slaap_observatie), ("actie", P_Slaap_actie)
            ),
            Tpl("Rapport @code — @sector: @activiteit. Risico: @risico. Maatregel: @actie.",
                ("code", P_Code), ("sector", P_Slaap_sector), ("activiteit", P_Slaap_activiteit), ("risico", P_Slaap_risico), ("actie", P_Slaap_actie)
            ),
            Tpl("Sector @sector. Toestand: @status. Uitvoering: @procedure.",
                ("sector", P_Slaap_sector), ("status", P_Slaap_status), ("procedure", P_Slaap_procedure)
            ),
            Tpl("Checkpoint @sector actief. Prioriteit: @prioriteit. Procedure: @procedure.",
                ("sector", P_Slaap_sector), ("prioriteit", P_Prioriteit), ("procedure", P_Slaap_procedure)
            ),
            Tpl("Kaartupdate — @sector gelabeld @label. Situatie: @status. Volgende stap: @actie.",
                ("sector", P_Slaap_sector), ("label", P_Label), ("status", P_Slaap_status), ("actie", P_Slaap_actie)
            )
        );

        /// ============ POST-APOC → DOUCHE ============
        var P_Douche_sector = new[] {
            "Solar-douche","Glaswand","Afvoerput","Kraanwerk","Tegelveld",
            "Siliconerand","Voegstrook","Sproeikop","Spiegel","Venthoek"
        };
        var P_Douche_activiteit = new[] {
            "kalk kruipt op","zeepfilm sluipt","afvoer loopt traag","vezels in rooster","straal schiet scheef",
            "tegels glad","voegen verduisteren","silicone mat","spiegelwaas blijft","fan zucht"
        };
        var P_Douche_status = new[] {
            "zicht beperkt","flow verstoord","veiligheidsrisico: licht","onderhoud vereist","glans laag",
            "geur aanwezig","sensor alert","gebruik beperkt","herstel haalbaar","omgeving nat"
        };
        var P_Douche_actie = new[] {
            "ontkalk & schrob voegen","rooster openen en reinigen","sproeikoppen reinigen","antislip moppen","glas polijsten",
            "kranen laten glanzen","ventilatie verhogen","drogen & nalopen","zone markeren 'fris'","hydro afsluiten"
        };
        var P_Douche_observatie = new[] {
            "kalkkraag rond kraan","film over glas","water blijft staan","rooster vangt vezels","jets gaan scheef",
            "tegel voelt glad","voeg toont schaduw","silicone stroef","spiegel blijft nevelig","fan blijft achter"
        };
        var P_Douche_risico = new[] {
            "slipgevaar","schimmelgroei","verstopping","kalkaanslag","geurincident",
            "glasbreuk","corrosie","waterlek","zichtverlies","ventilatie-overbelasting"
        };
        var P_Douche_procedure = new[] {
            "decalcify","grout-scrub","trap-clean","nozzle-clean","anti-slip mop",
            "glass polish","hardware polish","vent high","dry & inspect","close hydro"
        };

        AddTemplates(context, themeId["Post-apocalyptic"], CategoryType.Room, roomCatId["Douche"],
            Tpl("Bassin @sector meldt @activiteit. Status: @status. Actie: @actie.",
                ("sector", P_Douche_sector), ("activiteit", P_Douche_activiteit), ("status", P_Douche_status), ("actie", P_Douche_actie)
            ),
            Tpl("Locatie: @sector. Observatie: @observatie. Orders: @actie.",
                ("sector", P_Douche_sector), ("observatie", P_Douche_observatie), ("actie", P_Douche_actie)
            ),
            Tpl("Rapport @code — @sector: @activiteit. Risico: @risico. Maatregel: @actie.",
                ("code", P_Code), ("sector", P_Douche_sector), ("activiteit", P_Douche_activiteit), ("risico", P_Douche_risico), ("actie", P_Douche_actie)
            ),
            Tpl("Sector @sector. Toestand: @status. Uitvoering: @procedure.",
                ("sector", P_Douche_sector), ("status", P_Douche_status), ("procedure", P_Douche_procedure)
            ),
            Tpl("Checkpoint @sector actief. Prioriteit: @prioriteit. Procedure: @procedure.",
                ("sector", P_Douche_sector), ("prioriteit", P_Prioriteit), ("procedure", P_Douche_procedure)
            ),
            Tpl("Kaartupdate — @sector gelabeld @label. Situatie: @status. Volgende stap: @actie.",
                ("sector", P_Douche_sector), ("label", P_Label), ("status", P_Douche_status), ("actie", P_Douche_actie)
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

        #region Lovecraft

        // ===== Common pools =====
        var L_Code = new[] { "SIGIL-01", "SIGIL-02", "SIGIL-03", "SIGIL-04", "SIGIL-05", "SIGIL-06", "SIGIL-07", "SIGIL-08", "SIGIL-09", "SIGIL-10" };
        var L_Label = new[] { "STILTE", "ZOUT", "SCHADUW", "AFGROND", "BRANDING", "KOU", "MIST", "ROEST", "ZWART", "AMBER" };
        var L_Prioriteit = new[] { "laag", "middel", "hoog", "kritiek", "spoed", "ritueel", "nachtwake", "dageraad", "waakzaam", "herstel" };

        /// ============ LOVECRAFT → TOILET ============
        var L_Toilet_sector = new[] {
            "Putkamer","Zilt privaat","Achterste cel","Kelderlatrine","Porselein-nis",
            "Afvoer-schacht","Ronde kamer","Dieptehok","Keramiekkring","Stille kamer"
        };
        var L_Toilet_activiteit = new[] {
            "fluistergeur stijgt op","kalk tekent cirkels","wateroppervlak trilt stil","ketting tikt ritmisch",
            "spoeling vertraagt","vlek zonder herkomst","deksel zucht bij openen","water is troebelblauw",
            "afvoer murmelt","ventilatie zwijgt"
        };
        var L_Toilet_status = new[] {
            "realiteit dun aan randen","glans verdwenen","perimeter onrustig","inspectie vereist",
            "hulpmiddelen nodig","lucht muf en oud","gebruik gepauzeerd","rand gekerfd","sein: amber","onder controle"
        };
        var L_Toilet_actie = new[] {
            "ontkalkingsritueel uitvoeren","rand zuiveren","volledige lustratie","tocht oproepen (ventileren)",
            "dubbel spoelen","scharnieren zalven","vlek bannen","waterlijn polijsten","filter wisselen","eindzegen & vrijgave"
        };
        var L_Toilet_observatie = new[] {
            "water aarzelt alsof het luistert","kalk kruipt in patronen","geur wijkt af van herinnering",
            "spatten vormen runen","druk zakt bij de laatste slag","vlek vertelt geen verhaal",
            "deksel kraakt als stem","lijn van water is grijs","microspoor langs de rand","ventelucht houdt in"
        };
        var L_Toilet_risico = new[] {
            "slipgevaar","onwel worden","geurinvasie","hygiënekloof","rapport-gevoelig",
            "slijtage porselein","handschoenbreuk","onvrede bezoek","ritmebreuk routine","tijdverlies"
        };
        var L_Toilet_procedure = new[] {
            "Borstel-liturgie","Ontkalkingspsalm","Volledige lustratie","Vent-gezang",
            "Dubbel spoel","Scharnierzalf","Vlekban","Lijnpolijst","Filterwissel","Eindzegel"
        };

        AddTemplates(context, themeId["Lovecraft"], CategoryType.Room, roomCatId["Toilet"],
            Tpl("Zone @sector meldt @activiteit. Status: @status. Actie: @actie.",
                ("sector", L_Toilet_sector), ("activiteit", L_Toilet_activiteit), ("status", L_Toilet_status), ("actie", L_Toilet_actie)
            ),
            Tpl("Locatie: @sector. Observatie: @observatie. Orders: @actie.",
                ("sector", L_Toilet_sector), ("observatie", L_Toilet_observatie), ("actie", L_Toilet_actie)
            ),
            Tpl("Signatuur @code — @sector: @activiteit. Risico: @risico. Maatregel: @actie.",
                ("code", L_Code), ("sector", L_Toilet_sector), ("activiteit", L_Toilet_activiteit), ("risico", L_Toilet_risico), ("actie", L_Toilet_actie)
            ),
            Tpl("Sector @sector. Toestand: @status. Uitvoering: @procedure.",
                ("sector", L_Toilet_sector), ("status", L_Toilet_status), ("procedure", L_Toilet_procedure)
            ),
            Tpl("Wachtpost @sector actief. Prioriteit: @prioriteit. Procedure: @procedure.",
                ("sector", L_Toilet_sector), ("prioriteit", L_Prioriteit), ("procedure", L_Toilet_procedure)
            ),
            Tpl("Kaartupdate — @sector gemerkt @label. Situatie: @status. Volgende stap: @actie.",
                ("sector", L_Toilet_sector), ("label", L_Label), ("status", L_Toilet_status), ("actie", L_Toilet_actie)
            )
        );

        /// ============ LOVECRAFT → KEUKEN ============
        var L_Keuken_sector = new[] {
            "Haardnis","Brouwtafel","Zilt aanrecht","Kruiden-nis","Koperketel",
            "Afzuigkap","Spoelbekken","Voorraadkist","Ovenmond","Vaatrek"
        };
        var L_Keuken_activiteit = new[] {
            "vetsluier kruipt","filter ademt zwaar","kruimelspoor keert terug","pannen dragen zwart geheugen","damp blijft hangen",
            "vloer wil glijden","grepen plakken vreemd","lucht smaakt gisteren","condens tekent druppels","spatzone fluistert"
        };
        var L_Keuken_status = new[] {
            "voedselzegen dun","luchtstroom laag","oppervlak dof","hitte ongelijk","randen vergeten",
            "sensoren zwijgen","inspectie aanstaand","tijd voor reset","risico: laag-middel","oplevering haalbaar"
        };
        var L_Keuken_actie = new[] {
            "ontvetten & zuiveren","filters vernieuwen","roosters borstelen","kruimels vegen","antislip moppen",
            "grepen polijsten","afzuig bezweren","pannen herleven","temperatuur loggen","vrijgave 'FoodSafe'"
        };
        var L_Keuken_observatie = new[] {
            "vetfilm op blad","filterdruk te hoog","kruimels bij snijzone","koollaag op pannen","afzuig debiet laag",
            "vloer glad bij toegang","kleverige grepen","kooklucht blijft hangen","koelkast condenseert","spatzone actief"
        };
        var L_Keuken_risico = new[] {
            "kruisbesmetting","brandvlek","slipgevaar","sensorfout","warmte-accumulatie",
            "inspectierisico","luchtkwaliteit","krasvorming","glasschade","hygiënelek"
        };
        var L_Keuken_procedure = new[] {
            "Ontvet-cyclus","Filterwissel","Roosterreiniging","Dieptereiniging blad","Antislip-mop",
            "Grepen-polijst","Afzuig-gezang","Pannenreanimatie","Thermolog","Eindcontrole"
        };

        AddTemplates(context, themeId["Lovecraft"], CategoryType.Room, roomCatId["Keuken"],
            Tpl("Zaal @sector meldt @activiteit. Status: @status. Actie: @actie.",
                ("sector", L_Keuken_sector), ("activiteit", L_Keuken_activiteit), ("status", L_Keuken_status), ("actie", L_Keuken_actie)
            ),
            Tpl("Plaats: @sector. Observatie: @observatie. Orders: @actie.",
                ("sector", L_Keuken_sector), ("observatie", L_Keuken_observatie), ("actie", L_Keuken_actie)
            ),
            Tpl("Signatuur @code — @sector: @activiteit. Risico: @risico. Maatregel: @actie.",
                ("code", L_Code), ("sector", L_Keuken_sector), ("activiteit", L_Keuken_activiteit), ("risico", L_Keuken_risico), ("actie", L_Keuken_actie)
            ),
            Tpl("Werkhoek @sector. Toestand: @status. Uitvoering: @procedure.",
                ("sector", L_Keuken_sector), ("status", L_Keuken_status), ("procedure", L_Keuken_procedure)
            ),
            Tpl("Wachtpost @sector actief. Prioriteit: @prioriteit. Procedure: @procedure.",
                ("sector", L_Keuken_sector), ("prioriteit", L_Prioriteit), ("procedure", L_Keuken_procedure)
            ),
            Tpl("Kaartupdate — @sector gemerkt @label. Situatie: @status. Volgende stap: @actie.",
                ("sector", L_Keuken_sector), ("label", L_Label), ("status", L_Keuken_status), ("actie", L_Keuken_actie)
            )
        );

        /// ============ LOVECRAFT → GANG ============
        var L_Gang_sector = new[] {
            "Piergang","Zilt portaal","Trapnis","Voorhal","Achterdeur",
            "Plintgang","Middenpad","Meterhoek","Deurmat","Overloop"
        };
        var L_Gang_activiteit = new[] {
            "stof weeft webben","mat drinkt te veel","schakelaar vettig","armaturen dof","rails kermen",
            "hoeken houden schaduw","zand kruipt binnen","lint op route","strepen na regen","echo blijft hangen"
        };
        var L_Gang_status = new[] {
            "doorloop traag","zicht: oké","geluid boven norm","frictie toegenomen","aanzicht laag",
            "sliprisico: licht","toegang belemmerd","controle nodig","veiligheid voorlopig","inspectie op komst"
        };
        var L_Gang_actie = new[] {
            "Z-vegen & moppen","matten resetten","touchpoints reinigen","armaturen polijsten","rails borstelen/smeren",
            "hoeken detailen","zandbarrière leggen","lint verwijderen","vloer neutraliseren","doorgang vrijgeven"
        };
        var L_Gang_observatie = new[] {
            "plint vangt oud stof","matten verliezen grip","schakelaars glanzen fout","armaturen ogen dof","rails piepen onder last",
            "nis houdt web vast","zand vanaf kade","los lint op route","strepen door vocht","galm in de hal"
        };
        var L_Gang_risico = new[] {
            "slipgevaar","struikelkans","doorstroomverlies","stofallergie","zichtverlies",
            "plintschade","elektrisch risico","trapval","klachtbeleving","afkeur inspectie"
        };
        var L_Gang_procedure = new[] {
            "Z-vegencyclus","Mat-reset","Schakelaar-sanitatie","Armatuurpolijst","Railbrush + zalf",
            "Hoek-detail","Barrière leggen","Lintverwijdering","Friction normaliseren","Eindcontrole"
        };

        AddTemplates(context, themeId["Lovecraft"], CategoryType.Room, roomCatId["Gang"],
            Tpl("Route @sector meldt @activiteit. Status: @status. Actie: @actie.",
                ("sector", L_Gang_sector), ("activiteit", L_Gang_activiteit), ("status", L_Gang_status), ("actie", L_Gang_actie)
            ),
            Tpl("Plaats: @sector. Observatie: @observatie. Orders: @actie.",
                ("sector", L_Gang_sector), ("observatie", L_Gang_observatie), ("actie", L_Gang_actie)
            ),
            Tpl("Signatuur @code — @sector: @activiteit. Risico: @risico. Maatregel: @actie.",
                ("code", L_Code), ("sector", L_Gang_sector), ("activiteit", L_Gang_activiteit), ("risico", L_Gang_risico), ("actie", L_Gang_actie)
            ),
            Tpl("Galerij @sector. Toestand: @status. Uitvoering: @procedure.",
                ("sector", L_Gang_sector), ("status", L_Gang_status), ("procedure", L_Gang_procedure)
            ),
            Tpl("Wachtpost @sector actief. Prioriteit: @prioriteit. Procedure: @procedure.",
                ("sector", L_Gang_sector), ("prioriteit", L_Prioriteit), ("procedure", L_Gang_procedure)
            ),
            Tpl("Kaartupdate — @sector gemerkt @label. Situatie: @status. Volgende stap: @actie.",
                ("sector", L_Gang_sector), ("label", L_Label), ("status", L_Gang_status), ("actie", L_Gang_actie)
            )
        );

        /// ============ LOVECRAFT → SLAAPKAMER ============
        var L_Slaap_sector = new[] {
            "Kamer der Rust","Kwartier aan Zee","Nachtpost","Kasthoek","Raamkozijn",
            "Spiegelnis","Gordijnrand","Onderbed","Deurpost","Radiatornis"
        };
        var L_Slaap_activiteit = new[] {
            "stof ligt als as","kussens uit lijn","kozijnen stoffig","gordijnrand dof","pluis trekt sporen",
            "grepen vettig","spiegel met sluier","nachtkast kruimelt","vloer streperig","lucht staat stil"
        };
        var L_Slaap_status = new[] {
            "comfort onder norm","visuele ruis aanwezig","luchtkwaliteit matig","slaaphygiëne middel","aanzicht mager",
            "geluid stil","reset gewenst","ordening nodig","risico: laag","inspectie kan volgen"
        };
        var L_Slaap_actie = new[] {
            "linnen strak trekken","diepzuigen & vegen","kozijn reinigen","kussens opschudden","langzame mop",
            "grepen polijsten","spiegel helder maken","kasthoek ordenen","ventilatie boost","ruststatus loggen"
        };
        var L_Slaap_observatie = new[] {
            "stofnesten bij poten","plooien asymmetrisch","pollen op frame","gordijn geeft stof af","pluisbanen naar deur",
            "vingers op grepen","spiegelwaas zichtbaar","microkruimels op kast","streepjes na sokken","lucht muf"
        };
        var L_Slaap_risico = new[] {
            "allergenen","comfortverlies","stootgevaar","uitglijden","luchtkwaliteit",
            "presentatieverlies","inspectie-delay","pluisaccumulatie","oogirritatie","rustverstoring"
        };
        var L_Slaap_procedure = new[] {
            "Linnen-aanspanning","Deep vacuum","Kussen-reset","Kozijn-wipe","Low-speed mop",
            "Grepen-polish","Spiegelpolijst","Kasthoek-organize","Vent-boost","Eindrapport Rust"
        };

        AddTemplates(context, themeId["Lovecraft"], CategoryType.Room, roomCatId["Slaapkamer"],
            Tpl("Kwartier @sector meldt @activiteit. Status: @status. Actie: @actie.",
                ("sector", L_Slaap_sector), ("activiteit", L_Slaap_activiteit), ("status", L_Slaap_status), ("actie", L_Slaap_actie)
            ),
            Tpl("Plaats: @sector. Observatie: @observatie. Orders: @actie.",
                ("sector", L_Slaap_sector), ("observatie", L_Slaap_observatie), ("actie", L_Slaap_actie)
            ),
            Tpl("Signatuur @code — @sector: @activiteit. Risico: @risico. Maatregel: @actie.",
                ("code", L_Code), ("sector", L_Slaap_sector), ("activiteit", L_Slaap_activiteit), ("risico", L_Slaap_risico), ("actie", L_Slaap_actie)
            ),
            Tpl("Kamer @sector. Toestand: @status. Uitvoering: @procedure.",
                ("sector", L_Slaap_sector), ("status", L_Slaap_status), ("procedure", L_Slaap_procedure)
            ),
            Tpl("Wachtpost @sector actief. Prioriteit: @prioriteit. Procedure: @procedure.",
                ("sector", L_Slaap_sector), ("prioriteit", L_Prioriteit), ("procedure", L_Slaap_procedure)
            ),
            Tpl("Kaartupdate — @sector gemerkt @label. Situatie: @status. Volgende stap: @actie.",
                ("sector", L_Slaap_sector), ("label", L_Label), ("status", L_Slaap_status), ("actie", L_Slaap_actie)
            )
        );

        /// ============ LOVECRAFT → DOUCHE ============
        var L_Douche_sector = new[] {
            "Badhuisnis","Glaswand","Afvoerput","Kraanwerk","Tegelveld",
            "Siliconerand","Voegstrook","Sproeikop","Spiegel","Stoomhoek"
        };
        var L_Douche_activiteit = new[] {
            "kalk groeit als koraal","zeepfilm sluipt","afvoer loopt traag","vezels in rooster","straal schiet scheef",
            "tegels glad","voegen verduisteren","silicone stroef","spiegelwaas blijft","stoom blijft hangen"
        };
        var L_Douche_status = new[] {
            "zicht beperkt","flow verstoord","veiligheidsrisico: licht","onderhoud vereist","glans laag",
            "geur aanwezig","sensor alert","gebruik beperkt","herstel haalbaar","omgeving nat"
        };
        var L_Douche_actie = new[] {
            "ontkalk & schrob voegen","rooster openen en reinigen","sproeikoppen reinigen","antislip-moppen","glas polijsten",
            "kranen laten glanzen","ventilatie verhogen","drogen & nalopen","zone markeren 'fris'","hydro afsluiten"
        };
        var L_Douche_observatie = new[] {
            "kalk kraagt rond kraan","film over glas","water blijft staan","rooster vangt vezels","jets gaan scheef",
            "tegel voelt glad","voeg toont schaduw","silicone stroef","spiegel blijft nevelig","stoom hangt"
        };
        var L_Douche_risico = new[] {
            "slipgevaar","schimmelgroei","verstopping","kalkaanslag","geurincident",
            "glasbreuk","corrosie","waterlek","zichtverlies","ventilatie-overbelasting"
        };
        var L_Douche_procedure = new[] {
            "Decalcify","Grout-scrub","Trap-clean","Nozzle-clean","Anti-slip mop",
            "Glass polish","Hardware polish","Vent High","Dry & Inspect","Close Hydro"
        };

        AddTemplates(context, themeId["Lovecraft"], CategoryType.Room, roomCatId["Douche"],
            Tpl("Badhuis @sector meldt @activiteit. Status: @status. Actie: @actie.",
                ("sector", L_Douche_sector), ("activiteit", L_Douche_activiteit), ("status", L_Douche_status), ("actie", L_Douche_actie)
            ),
            Tpl("Plaats: @sector. Observatie: @observatie. Orders: @actie.",
                ("sector", L_Douche_sector), ("observatie", L_Douche_observatie), ("actie", L_Douche_actie)
            ),
            Tpl("Signatuur @code — @sector: @activiteit. Risico: @risico. Maatregel: @actie.",
                ("code", L_Code), ("sector", L_Douche_sector), ("activiteit", L_Douche_activiteit), ("risico", L_Douche_risico), ("actie", L_Douche_actie)
            ),
            Tpl("Stoomnis @sector. Toestand: @status. Uitvoering: @procedure.",
                ("sector", L_Douche_sector), ("status", L_Douche_status), ("procedure", L_Douche_procedure)
            ),
            Tpl("Wachtpost @sector actief. Prioriteit: @prioriteit. Procedure: @procedure.",
                ("sector", L_Douche_sector), ("prioriteit", L_Prioriteit), ("procedure", L_Douche_procedure)
            ),
            Tpl("Kaartupdate — @sector gemerkt @label. Situatie: @status. Volgende stap: @actie.",
                ("sector", L_Douche_sector), ("label", L_Label), ("status", L_Douche_status), ("actie", L_Douche_actie)
            )
        );

        #endregion

        #region SciFi

        // ===== Common pools =====
        var S_Code = new[] { "NOVA-01", "NOVA-02", "NOVA-03", "NOVA-04", "NOVA-05", "NOVA-06", "NOVA-07", "NOVA-08", "NOVA-09", "NOVA-10" };
        var S_Label = new[] { "STABIEL", "WAARSCHUWING", "KRITIEK", "VACUÜM", "RAD", "THERM", "O2", "LOCKDOWN", "IDLE", "SERVICE" };
        var S_Prioriteit = new[] { "laag", "middel", "hoog", "kritiek", "spoed", "routine", "preventief", "nachtshift", "dagdienst", "onderhoudsvenster" };

        /// ============ SCI-FI → TOILET ============
        var S_Toilet_sector = new[] {
            "SanUnit-01","VacFlush Bay","Lav-Module B","Waste Recycler","San Deck Aft",
            "Cubicle Grid C","BioLoop Node","Hygiene Pod","Stall Deck 3","Service Hatch WC"
        };
        var S_Toilet_activiteit = new[] {
            "drukval in vacuümspoel","residulog triggert","filter verzadigd","actuator piept","seal micro-lek",
            "calcificatie op rim","geurtrace off-baseline","waterlijn troebel","pomp duty-cycle fluctueert","fan draait traag"
        };
        var S_Toilet_status = new[] {
            "biohazard: laag","glansindex gedaald","perimeter instabiel","service vereist","luchtkwaliteit suboptimaal",
            "gebruik gepauzeerd","rand beschadigd","sein: amber","diagnose nodig","situatie beheersbaar"
        };
        var S_Toilet_actie = new[] {
            "VacFlush kalibreren","ontkalk rand & kom","UV-desinfectie","ventilatie boosten","dubbele spoelcyclus",
            "scharnier smeren","biofilm neutraliseren","waterlijn polijsten","filter wisselen","eindcontrole & vrijgave"
        };
        var S_Toilet_observatie = new[] {
            "drukverschil instabiel","kalkgroei langs rim","geurprofiel wijkt af","spatpatroon buiten spec",
            "spoeldruk fluctueert","vlek zonder herkomst","hinge jitter aanwezig","troebel waterbeeld","microresidu gedetecteerd","fan rpm onder norm"
        };
        var S_Toilet_risico = new[] {
            "splashback","morale dip","slipgevaar","geurincident","biofilmvorming",
            "rapport-gevoelig","oppervlakte-slijtage","handschoenbreuk","bezoeker-onvrede","routine-afwijking"
        };
        var S_Toilet_procedure = new[] {
            "VacFlush-kalibratie","Ontkalk-cyclus","UV-Desinfectie","Vent-boost",
            "Dubbel spoelen","Scharnier-smeer","Vlek-neutralisatie","Lijn-polijst","Filterwissel","Eindcontrole"
        };

        AddTemplates(context, themeId["Sci-fi"], CategoryType.Room, roomCatId["Toilet"],
            Tpl("Module @sector meldt @activiteit. Status: @status. Actie: @actie.",
                ("sector", S_Toilet_sector), ("activiteit", S_Toilet_activiteit), ("status", S_Toilet_status), ("actie", S_Toilet_actie)
            ),
            Tpl("Locatie: @sector. Observatie: @observatie. Orders: @actie.",
                ("sector", S_Toilet_sector), ("observatie", S_Toilet_observatie), ("actie", S_Toilet_actie)
            ),
            Tpl("Log @code — @sector: @activiteit. Risico: @risico. Maatregel: @actie.",
                ("code", S_Code), ("sector", S_Toilet_sector), ("activiteit", S_Toilet_activiteit), ("risico", S_Toilet_risico), ("actie", S_Toilet_actie)
            ),
            Tpl("Sector @sector. Toestand: @status. Uitvoering: @procedure.",
                ("sector", S_Toilet_sector), ("status", S_Toilet_status), ("procedure", S_Toilet_procedure)
            ),
            Tpl("Checkpoint @sector actief. Prioriteit: @prioriteit. Procedure: @procedure.",
                ("sector", S_Toilet_sector), ("prioriteit", S_Prioriteit), ("procedure", S_Toilet_procedure)
            ),
            Tpl("Kaartupdate — @sector gelabeld @label. Situatie: @status. Volgende stap: @actie.",
                ("sector", S_Toilet_sector), ("label", S_Label), ("status", S_Toilet_status), ("actie", S_Toilet_actie)
            )
        );

        /// ============ SCI-FI → KEUKEN ============
        var S_Keuken_sector = new[] {
            "Galley Deck","Prep Station Sigma","Synth-Brew Nook","Cold Storage K","Filter Hub","Sink Bay",
            "Cutting Grid","Spice Rack Neo","Oven Core","Dish Rail"
        };
        var S_Keuken_activiteit = new[] {
            "vetfilm op werkblad","filter ΔP te hoog","kruimelpad actief","pannen carbonaatlaag","damp blijft hangen",
            "vloercoating glibberig","handgrepen kleverig","geurretentie hoog","condens op deur","spatzone hyperactief"
        };
        var S_Keuken_status = new[] {
            "voedselveiligheid dun","luchtflow 60%","oppervlakken dof","hittezones ongelijk","randen vergeten",
            "sensoriek vervuild","inspectie pending","reset aanbevolen","risico: laag-middel","oplevering haalbaar"
        };
        var S_Keuken_actie = new[] {
            "ontvetten & desinfecteren","filters resetten","roosters borstelen","kruimelroutes vegen","antislip moppen",
            "handgrepen polijsten","afzuig boosten","pannen rehabiliteren","temperatuur loggen","vrijgave 'FoodSafe'"
        };
        var S_Keuken_observatie = new[] {
            "vetfilm zichtbaar","ΔP filter boven norm","kruimels bij snijvlak","pannen dragen koollaag","afzuig debiet laag",
            "vloer toont sliprisico","grepen voelen kleverig","kooklucht blijft hangen","koelkast condenseert","spatzone actief"
        };
        var S_Keuken_risico = new[] {
            "kruisbesmetting","brandvlekken","slipgevaar","sensorfout","warmte-accumulatie",
            "inspectierisico","luchtkwaliteit","krasvorming","glasschade","hygiënelek"
        };
        var S_Keuken_procedure = new[] {
            "Ontvet-cyclus","Filterwissel","Roosterreiniging","Dieptereiniging blad","Antislip-mop",
            "Handgreeppolijst","Afzuig-boost","Pannenreanimatie","Thermolog","Eindcontrole"
        };

        AddTemplates(context, themeId["Sci-fi"], CategoryType.Room, roomCatId["Keuken"],
            Tpl("Zone @sector meldt @activiteit. Status: @status. Actie: @actie.",
                ("sector", S_Keuken_sector), ("activiteit", S_Keuken_activiteit), ("status", S_Keuken_status), ("actie", S_Keuken_actie)
            ),
            Tpl("Locatie: @sector. Observatie: @observatie. Orders: @actie.",
                ("sector", S_Keuken_sector), ("observatie", S_Keuken_observatie), ("actie", S_Keuken_actie)
            ),
            Tpl("Log @code — @sector: @activiteit. Risico: @risico. Maatregel: @actie.",
                ("code", S_Code), ("sector", S_Keuken_sector), ("activiteit", S_Keuken_activiteit), ("risico", S_Keuken_risico), ("actie", S_Keuken_actie)
            ),
            Tpl("Sector @sector. Toestand: @status. Uitvoering: @procedure.",
                ("sector", S_Keuken_sector), ("status", S_Keuken_status), ("procedure", S_Keuken_procedure)
            ),
            Tpl("Checkpoint @sector actief. Prioriteit: @prioriteit. Procedure: @procedure.",
                ("sector", S_Keuken_sector), ("prioriteit", S_Prioriteit), ("procedure", S_Keuken_procedure)
            ),
            Tpl("Kaartupdate — @sector gelabeld @label. Situatie: @status. Volgende stap: @actie.",
                ("sector", S_Keuken_sector), ("label", S_Label), ("status", S_Keuken_status), ("actie", S_Keuken_actie)
            )
        );

        /// ============ SCI-FI → GANG ============
        var S_Gang_sector = new[] {
            "Corridor A2","Transit Ring","Airlock Approach","Service Spine","Lift Lobby",
            "Access Tunnel","Perimeter Hall","Stairwell Z","Doorzone 4","Crossway Beta"
        };
        var S_Gang_activiteit = new[] {
            "stofpluimen bij plint","mat verzadigd","schakelaar-vet","armaturen dof","rails piepen",
            "hoekcluster vuil","zand-infiltratie","lint op route","strepen op vloer","galm te luid"
        };
        var S_Gang_status = new[] {
            "doorstroom remmend","zicht: oké","geluid boven norm","frictie toegenomen","aanzicht laag",
            "sliprisico: licht","toegang belemmerd","controle nodig","veiligheid gewaarborgd","inspectie naderend"
        };
        var S_Gang_actie = new[] {
            "Z-vegen & nat moppen","matten resetten","touchpoints reinigen","armaturen poetsen","rails borstelen/smeren",
            "hoeken detailen","zandbarrière plaatsen","lint verwijderen","vloer neutraliseren","doorgang vrijgeven"
        };
        var S_Gang_observatie = new[] {
            "plint vangt stof","mat verliest grip","schakelaars glanzen fout","armaturen ogen dof","rails piepen onder last",
            "hoeken missen routine","zand vanaf airlock","los lint op route","strepen door vocht","echo in de hal"
        };
        var S_Gang_risico = new[] {
            "slipgevaar","struikelkans","doorstroomverlies","stofallergie","zichtverlies",
            "plintschade","elektrisch risico","valgevaar trap","klachtbeleving","afkeur inspectie"
        };
        var S_Gang_procedure = new[] {
            "Z-vegencyclus","Mat-reset","Schakelaar-sanitatie","Armatuurpolijst","Railbrush + lube",
            "Hoek-detail","Zandbarrière","Lintverwijdering","Friction normaliseren","Eindcontrole"
        };

        AddTemplates(context, themeId["Sci-fi"], CategoryType.Room, roomCatId["Gang"],
            Tpl("Route @sector meldt @activiteit. Status: @status. Actie: @actie.",
                ("sector", S_Gang_sector), ("activiteit", S_Gang_activiteit), ("status", S_Gang_status), ("actie", S_Gang_actie)
            ),
            Tpl("Locatie: @sector. Observatie: @observatie. Orders: @actie.",
                ("sector", S_Gang_sector), ("observatie", S_Gang_observatie), ("actie", S_Gang_actie)
            ),
            Tpl("Log @code — @sector: @activiteit. Risico: @risico. Maatregel: @actie.",
                ("code", S_Code), ("sector", S_Gang_sector), ("activiteit", S_Gang_activiteit), ("risico", S_Gang_risico), ("actie", S_Gang_actie)
            ),
            Tpl("Sector @sector. Toestand: @status. Uitvoering: @procedure.",
                ("sector", S_Gang_sector), ("status", S_Gang_status), ("procedure", S_Gang_procedure)
            ),
            Tpl("Checkpoint @sector actief. Prioriteit: @prioriteit. Procedure: @procedure.",
                ("sector", S_Gang_sector), ("prioriteit", S_Prioriteit), ("procedure", S_Gang_procedure)
            ),
            Tpl("Kaartupdate — @sector gelabeld @label. Situatie: @status. Volgende stap: @actie.",
                ("sector", S_Gang_sector), ("label", S_Label), ("status", S_Gang_status), ("actie", S_Gang_actie)
            )
        );

        /// ============ SCI-FI → SLAAPKAMER ============
        var S_Slaap_sector = new[] {
            "Crew Quarters","Pod Alpha","Bunk Stack","Locker Bay","Viewport",
            "Mirror Panel","Curtain Rail","Underbunk","Doorframe","Heater Niche"
        };
        var S_Slaap_activiteit = new[] {
            "stoflaag onder bunk","kussens uit lijn","kozijnen stoffig","gordijnrand dof","pluis paradeert",
            "handgrepen vettig","spiegel met vlekken","nachtplank kruimelt","vloer streperig","lucht staat stil"
        };
        var S_Slaap_status = new[] {
            "comfort onder norm","visuele ruis aanwezig","luchtkwaliteit matig","slaaphygiëne middel","aanzicht mager",
            "geluid nihil","reset gewenst","ordening nodig","risico: laag","inspectie kan volgen"
        };
        var S_Slaap_actie = new[] {
            "linnen strak trekken","diepzuigen & vegen","kozijn reinigen","kussens opschudden","low-speed mop",
            "grepen polijsten","spiegel helder maken","kasthoek ordenen","ventilatie boost","ruststatus loggen"
        };
        var S_Slaap_observatie = new[] {
            "stofnesten bij poten","plooien asymmetrisch","pollen op frame","gordijn geeft stof af","pluisbanen naar deur",
            "vingers op grepen","spiegelwaas zichtbaar","microkruimels op plank","streepjes na sokken","lucht muf"
        };
        var S_Slaap_risico = new[] {
            "allergenen","comfortverlies","stootgevaar","uitglijden","luchtkwaliteit",
            "presentatieverlies","inspectie-delay","pluisaccumulatie","oogirritatie","rustverstoring"
        };
        var S_Slaap_procedure = new[] {
            "Linnen-aanspanning","Deep vacuum","Kussen-reset","Kozijn-wipe","Low-speed mop",
            "Grepen-polish","Spiegelpolijst","Kasthoek-organize","Vent-boost","Eindrapport Rust"
        };

        AddTemplates(context, themeId["Sci-fi"], CategoryType.Room, roomCatId["Slaapkamer"],
            Tpl("Kwartier @sector meldt @activiteit. Status: @status. Actie: @actie.",
                ("sector", S_Slaap_sector), ("activiteit", S_Slaap_activiteit), ("status", S_Slaap_status), ("actie", S_Slaap_actie)
            ),
            Tpl("Locatie: @sector. Observatie: @observatie. Orders: @actie.",
                ("sector", S_Slaap_sector), ("observatie", S_Slaap_observatie), ("actie", S_Slaap_actie)
            ),
            Tpl("Log @code — @sector: @activiteit. Risico: @risico. Maatregel: @actie.",
                ("code", S_Code), ("sector", S_Slaap_sector), ("activiteit", S_Slaap_activiteit), ("risico", S_Slaap_risico), ("actie", S_Slaap_actie)
            ),
            Tpl("Sector @sector. Toestand: @status. Uitvoering: @procedure.",
                ("sector", S_Slaap_sector), ("status", S_Slaap_status), ("procedure", S_Slaap_procedure)
            ),
            Tpl("Checkpoint @sector actief. Prioriteit: @prioriteit. Procedure: @procedure.",
                ("sector", S_Slaap_sector), ("prioriteit", S_Prioriteit), ("procedure", S_Slaap_procedure)
            ),
            Tpl("Kaartupdate — @sector gelabeld @label. Situatie: @status. Volgende stap: @actie.",
                ("sector", S_Slaap_sector), ("label", S_Label), ("status", S_Slaap_status), ("actie", S_Slaap_actie)
            )
        );

        /// ============ SCI-FI → DOUCHE ============
        var S_Douche_sector = new[] {
            "Hygiene Pod","Glass Shield","Drain Node","Valve Assembly","Tile Field",
            "Sealant Line","Grout Strip","Nozzle Array","Mirror Panel","Vent Stack"
        };
        var S_Douche_activiteit = new[] {
            "kalk op fixtures","zeepfilm op glas","afvoer traag","vezels in rooster","straal ongelijk",
            "tegels slipgevaar","voegen donker","sealant mat","spiegelwaas","ventilatie traag"
        };
        var S_Douche_status = new[] {
            "zicht beperkt","flow verstoord","veiligheidsrisico: licht","onderhoud vereist","glans laag",
            "geur aanwezig","sensor alert","gebruik beperkt","herstel haalbaar","omgeving nat"
        };
        var S_Douche_actie = new[] {
            "ontkalk & schrob voegen","rooster openen en reinigen","sproeikoppen reinigen","antislip-moppen","glas polijsten",
            "hardware laten glanzen","ventilatie verhogen","drogen & nalopen","zone markeren 'fris'","hydro afsluiten"
        };
        var S_Douche_observatie = new[] {
            "kalkkraag rond kraanbasis","film over glas","water blijft staan bij afvoer","rooster vangt vezels","jets spatten scheef",
            "tegel voelt glad","voeg toont schaduw","sealant stroef","spiegel blijft nevelig","fan blijft achter"
        };
        var S_Douche_risico = new[] {
            "slipgevaar","schimmelgroei","verstopping","kalkaanslag","geurincident",
            "glasbreuk","corrosie","waterlek","zichtverlies","ventilatie-overbelasting"
        };
        var S_Douche_procedure = new[] {
            "Decalcify","Grout-scrub","Trap-clean","Nozzle-clean","Anti-slip mop",
            "Glass polish","Hardware polish","Vent High","Dry & Inspect","Close Hydro"
        };

        AddTemplates(context, themeId["Sci-fi"], CategoryType.Room, roomCatId["Douche"],
            Tpl("Bassin @sector meldt @activiteit. Status: @status. Actie: @actie.",
                ("sector", S_Douche_sector), ("activiteit", S_Douche_activiteit), ("status", S_Douche_status), ("actie", S_Douche_actie)
            ),
            Tpl("Locatie: @sector. Observatie: @observatie. Orders: @actie.",
                ("sector", S_Douche_sector), ("observatie", S_Douche_observatie), ("actie", S_Douche_actie)
            ),
            Tpl("Log @code — @sector: @activiteit. Risico: @risico. Maatregel: @actie.",
                ("code", S_Code), ("sector", S_Douche_sector), ("activiteit", S_Douche_activiteit), ("risico", S_Douche_risico), ("actie", S_Douche_actie)
            ),
            Tpl("Sector @sector. Toestand: @status. Uitvoering: @procedure.",
                ("sector", S_Douche_sector), ("status", S_Douche_status), ("procedure", S_Douche_procedure)
            ),
            Tpl("Checkpoint @sector actief. Prioriteit: @prioriteit. Procedure: @procedure.",
                ("sector", S_Douche_sector), ("prioriteit", S_Prioriteit), ("procedure", S_Douche_procedure)
            ),
            Tpl("Kaartupdate — @sector gelabeld @label. Situatie: @status. Volgende stap: @actie.",
                ("sector", S_Douche_sector), ("label", S_Label), ("status", S_Douche_status), ("actie", S_Douche_actie)
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
        });

        context.SaveChanges();

        var roomId = context.Room.ToDictionary(t => t.Description, t => t.Id);

        context.Chore.AddRange(
        new Chore
        {
            Description = "WC schoonmaken",
            DurationMinutes = 10,
            FrequencyDays = 7,
            DirtinessFactor = DirtinessFactor.Extreme,
            RoomId = roomCatId["Toilet"],
            CategoryId = choreCatId["WC-pot"],
        },
        new Chore
        {
            Description = "Vloer schoonmaken",
            DurationMinutes = 5,
            FrequencyDays = 30,
            DirtinessFactor = DirtinessFactor.Medium,
            RoomId = roomCatId["Toilet"],
            CategoryId = choreCatId["Vloer"],
        },
        new Chore
        {
            Description = "Muur schoonmaken",
            DurationMinutes = 60,
            DirtinessFactor = DirtinessFactor.Low,
            RoomId = roomCatId["Toilet"],
            CategoryId = choreCatId["Muur"],
        },
        new Chore
        {
            Description = "Koffieapparaat schoonmaken",
            DurationMinutes = 5,
            FrequencyDays = 30,
            DirtinessFactor = DirtinessFactor.Extreme,
            RoomId = roomCatId["Keuken"],
            CategoryId = choreCatId["Koffieapparaat"],
        });

        context.SaveChanges();

        var choreId = context.Chore.ToDictionary(t => t.Description, t => t.Id);

        context.History.AddRange(
        new History
        {
            ChoreId = choreId["WC schoonmaken"],
            DateCompleted = DateTime.Now.AddDays(-50),
        },
        new History
        {
            ChoreId = choreId["Vloer schoonmaken"],
            DateCompleted = DateTime.Now.AddDays(0),
        },
        new History
        {
            ChoreId = choreId["Muur schoonmaken"],
            DateCompleted = DateTime.Now.AddDays(-100),
        },
        new History
        {
            ChoreId = choreId["Koffieapparaat schoonmaken"],
            DateCompleted = DateTime.Now.AddDays(-1000),
        });

        context.SaveChanges();
    }
}
