using System.ComponentModel;

namespace Munin.DAL
{
    public enum Material
    {
        Book,
        Document,
        Picture,
        Sequence
    }

    public enum PictureMaterial
    {
        [Description("Papirbillede")]
        Papirbillede = 1,
        [Description("Negativ")]
        Negativ = 2,
        [Description("Dias")]
        Dias = 3,
        [Description("CD")]
        Cd = 4,
        [Description("DVD")]
        Dvd = 5,
        [Description("Andet")]
        Andet = 6
    }

    public enum PaperEnum
    {
        [Description("Fyens Stiftstidende")]
        FyensStiftsTidende = 1,
        [Description("Fyens Tidende")]
        FyensTidende = 2,
        [Description("Ekstra Bladet")]
        EkstraBladet = 3,
        [Description("BT")]
        BT = 4,
        [Description("Berlingske Tidende")]
        BerlingskeTidende = 5,
        [Description("Politiken")]
        Politiken = 6,
        [Description("Uge Avisen")]
        UgeAvisen = 7,
        [Description("Odense Posten")]
        OdensePosten = 8,
        [Description("Dalum Hjallese Ugeavis")]
        DalumHallese = 9,
        [Description("Anden Avis")]
        AndenAvis = 10,
        [Description("Postal Post")]
        PostalPost = 11,
        [Description("Dagens Nye Dagblad")]
        DagensNyeDagblad = 12,
        [Description("Ugeskrift for Læger")]
        UgeskriftForLaeger = 13,
        [Description("MorgenPosten")]
        MorgenPosten = 14,
        [Description("Fyens Amtsavis")]
        FyensAmtsAvis = 15,
    }

    public enum ChirchEvent
    {
        Born,
        Christening,
        Wedding,
        Burial
    }

    public enum MapType
    {
        [Description("A4")]
        A4,
        [Description("Atlasblad")]
        Atlasblad,
        [Description("Grundtegning")]
        Grundtegning,
        [Description("LuftFoto")]
        Luftfoto,
        [Description("Matrikel Kort")]
        Matrikelkort,
        [Description("Målebordsblad")]
        Maalebordsblad,
        [Description("Termisk Kort - Planche")]
        Planche,
        [Description("Tegning")]
        Tegning,
        [Description("Termisk kort")]
        Tematisk,
        [Description("Topografisk kort")]
        Topografisk,
        [Description("UTM")]
        UTM,
        [Description("Andet")]
        Udefineret
    }


    public enum MapMaterial
    {
        [Description("A4")]
        A4,
        [Description("Farve Reproduktion")]
        Farvereprodukt,
        [Description("Farvetryk")]
        Farvetryk,
        [Description("Fotogramatisk")]
        Fotogrametrisk,
        [Description("Fotokopi på plast")]
        FotokopiPlast,
        [Description("Fotokopi")]
        Fotokopi,
        [Description("Foto litografi")]
        FotoLitografi,
        [Description("Hylde loft 85")]
        HyldeLoft85,
        [Description("Karton")]
        Karton,
        [Description("Karton bag 82A")]
        KartonBag82A,
        [Description("Karton Reol 96A")]
        KartonReol96a,
        [Description("Karton Reol 97")]
        KartonReol97,
        [Description("Karton på kortskab")]
        KartonKortskab,
        [Description("Luftfoto bag 82")]
        LuftfotoBag82,
        [Description("Lyskopi")]
        Lyskopi,
        [Description("Lystryk")]
        Lystryk,
        [Description("Negativ Reol 96")]
        NegativReol96,
        [Description("Negativ Reol 97")]
        NegativReol97,
        [Description("OPH A4")]
        OHPA4,
        [Description("OPH Folie")]
        OHPFolie,
        [Description("OPH Transparent")]
        OHPTransparent,
        [Description("Opklæbet")]
        Opklæbet,
        [Description("Opklæbet Karton")]
        KartonOpklaebet,
        [Description("Original se 99")]
        OriginalSe99,
        [Description("Original skab 99")]
        OriginalSkab99,
        [Description("Papir")]
        Papir,
        [Description("Planche")]
        planche,
        [Description("Plast")]
        Plast,
        [Description("Kort lærred")] //på kortlærred
        Kortlærred,
        [Description("Samlet Planche")]
        SamletPlanche,
        [Description("TransParent")]
        Transparent,
        [Description("Andet")]
        Andet

    }

    public enum SequenceType
    {
        [Description("8mm Smalfilm")]
        Smalfilm8mm,
        [Description("8mm Smalfilm Super")]
        Superfilm8mm,
        [Description("8mm Smalfilm farve")]
        Filmfarve8mm,
        [Description("8mm Smalfilm SH ")]
        SmalfilmSH8mm,
        [Description("8mm Smalfilm 9,5 mm")]
        mmSmalfilm95mm,
        [Description("Beretning Produktion")]
        Produktion,
        [Description("DVD")]
        DVD,
        [Description("DVS")]
        DVS,
        [Description("Foredrag")]
        Foredrag,
        [Description("Voice Recorder")]
        VoiceRecorder,
        [Description("LY")]
        LY,
        [Description("Lydbåand")]
        Lydbånd,
        [Description("Kassettebånd (MC)")]
        MC,
        [Description("Kassttebånd  (MC) dobbelt")]
        MC2x,
        [Description("Små Enakter")]
        SmåEnakter,
        [Description("VHS")]
        VHS,
        [Description("Video")]
        video,
        [Description("Andet")]
        Andet,
    }

    public enum ArchiveType
    {
        Andet,
        A,
        P,
        I,
        E,
        F,        
    }

    public enum Cover
    {
        Andet,
        O,
        K,
        L,
        B        
    }
}
