using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        MorgenPosten = 14
    }
}
