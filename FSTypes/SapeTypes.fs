namespace Sapientum.Types.Sape

open Sapientum
open Sapientum.Helper

open CookComputing.XmlRpc

open System
open System.Text

type UriStr = string
and HostStr = string
and TitleStr = string
and HtmlStr = string
and HeadersStr = string
and CookieStr = string

type Name = String
type Id = Int32

type UserInfo(xmlRpcStruct:XmlRpcStruct) = 
    let _login                = unbox<String> xmlRpcStruct.["login"]
    let _hash                 = unbox<String> xmlRpcStruct.["hash"] 
    let _balance              = unbox<Double> xmlRpcStruct.["balance"]
    let _seoNofLinks          = unbox<Int32>  xmlRpcStruct.["seo_nof_links"]
    let _seoNofLinksOk        = unbox<Int32>  xmlRpcStruct.["seo_nof_links_ok"]
    let _seoBudgetMonthly     = unbox<Double> xmlRpcStruct.["seo_budget_monthly"]
    let _seoBudgetMonthlyReal = unbox<Double> xmlRpcStruct.["seo_budget_monthly_real"]
    let _email                = unbox<String> xmlRpcStruct.["email"]

    member x.Login   = _login
    member x.Hash    = _hash
    member x.Balance = _balance
  
    override x.ToString() = sprintf "логин: %s | баланс: %f" _login _balance
  
    static member create xmlRpcStruct = new UserInfo(xmlRpcStruct)

type Project(xmlRpcStruct:XmlRpcStruct) = 
    let _name             = unbox<Name>     xmlRpcStruct.["name"]
    let _id               = unbox<Id>       xmlRpcStruct.["id"]
    let _dateCreated      = unbox<DateTime> xmlRpcStruct.["date_created"]
    let _amountYesterday  = unbox<Double>   xmlRpcStruct.["amount_yesterday"]
    let _amountToday      = unbox<Double>   xmlRpcStruct.["amount_today"]
    let _amountTotal      = unbox<Double>   xmlRpcStruct.["amount_total"]
  
    member x.Name        = _name
    member x.Id          = _id
    member x.DateCreated = _dateCreated
  
    override x.ToString() = sprintf "проект: %A" _name
  
    static member create xmlRpcStruct = new Project(xmlRpcStruct)

type ProjectUrl(xmlRpcStruct:XmlRpcStruct) = 
    let _id                 = unbox<Id>       xmlRpcStruct.["id"]
    let _projectId          = unbox<int>      xmlRpcStruct.["project_id"]
    let _name               = unbox<Name>     xmlRpcStruct.["name"]
    let _url                = unbox<string>   xmlRpcStruct.["url"]
    let _amountYesterday    = unbox<float>    xmlRpcStruct.["amount_yesterday"]
    let _amountToday        = unbox<float>    xmlRpcStruct.["amount_today"]
    let _amountTotal        = unbox<float>    xmlRpcStruct.["amount_total"]
    let _dateCreated        = unbox<DateTime> xmlRpcStruct.["date_created"]
  
    member x.Id         = _id
    member x.ProjectId  = _projectId
    member x.Name       = _name
    member x.Url        = _url
  
    override x.ToString() = sprintf "урл: %A" _name
  
    static member create xmlRpcStruct = new ProjectUrl(xmlRpcStruct)

type FilterParams(xmlRpcStruct:XmlRpcStruct) = 
    let getValue (keyName:String) (defaultValue:'T) = 
        let value = xmlRpcStruct.[keyName]
        if value = null then defaultValue
        else unbox<'T> value
  
//    let _words               = unbox<String>  xmlRpcStruct.["words"]
//    let _priceFrom           = unbox<Double>  xmlRpcStruct.["price_from"]
//    let _domainLevel         = unbox<Int32>   xmlRpcStruct.["domain_level"]
//    let _extLinksForecast    = unbox<Int32>   xmlRpcStruct.["ext_links_forecast"]
//    let _extLinks            = unbox<Int32>   xmlRpcStruct.["ext_links"]
//    let _nogood              = unbox<Int32>   xmlRpcStruct.["nogood"]
//    let _cyFrom              = unbox<Int32>   xmlRpcStruct.["cy_from"]
//    let _noDoubleInProject   = unbox<Int32>   xmlRpcStruct.["no_double_in_project"]
//    let _flagOnlyWhiteList   = unbox<Int32>   xmlRpcStruct.["flag_only_white_list"]
//    let _domainZones         = unbox<String>  xmlRpcStruct.["domain_zones"]
//    let _flagBlockedInYandex = unbox<Int32>   xmlRpcStruct.["flag_blocked_in_yandex"]
//    let _levelFrom           = unbox<Int32>   xmlRpcStruct.["level_from"]
//    let _inDmoz              = unbox<Int32>   xmlRpcStruct.["in_dmoz"]
//    let _inYaca              = unbox<Int32>   xmlRpcStruct.["in_yaca"]
//    let _daysOldWhois        = unbox<Int32>   xmlRpcStruct.["days_old_whois"]
//    let _price2              = unbox<Double>  xmlRpcStruct.["price_2"]
//    let _flagBlockedInGoogle = unbox<Int32>   xmlRpcStruct.["flag_blocked_in_google"]
//    let _cy2                 = unbox<Int32>   xmlRpcStruct.["cy_2"]
//    let _level2              = unbox<Int32>   xmlRpcStruct.["level_2"]

    let _words               = getValue "words" String.Empty
    let _priceFrom           = getValue "price_from" 0.0
    let _domainLevel         = getValue "domain_level" 0
    let _extLinksForecast    = getValue "ext_links_forecast" 0
    let _extLinks            = getValue "ext_links" 0
    let _nogood              = getValue "nogood" 0
    let _cyFrom              = getValue "cy_from" 0
    let _noDoubleInProject   = getValue "no_double_in_project" 0
    let _flagOnlyWhiteList   = getValue "flag_only_white_list" 0
    let _domainZones         = getValue "domain_zones" String.Empty
    let _flagBlockedInYandex = getValue "flag_blocked_in_yandex" 0
    let _levelFrom           = getValue "level_from" 0
    let _inDmoz              = getValue "in_dmoz" 0
    let _inYaca              = getValue "in_yaca" 0
    let _daysOldWhois        = getValue "days_old_whois" 0
    let _price2              = getValue "price_2" 0.0
    let _flagBlockedInGoogle = getValue "flag_blocked_in_google" 0
    let _cy2                 = getValue "cy_2" 0
    let _level2              = getValue "level_2" 0

    member x.Words               = _words
    member x.PriceFrom           = _priceFrom
    member x.DomainLevel         = _domainLevel
    member x.ExtLinksForecast    = _extLinksForecast
    member x.ExtLinks            = _extLinks
    member x.Nogood              = _nogood
    member x.CyFrom              = _cyFrom
    member x.NoDoubleInProject   = _noDoubleInProject
    member x.FlagOnlyWhiteList   = _flagOnlyWhiteList
    member x.DomainZones         = _domainZones
    member x.FlagBlockedInYandex = _flagBlockedInYandex
    member x.LevelFrom           = _levelFrom
    member x.InDmoz              = _inDmoz
    member x.InYaca              = _inYaca
    member x.DaysOldWhois        = _daysOldWhois
    member x.PriceTo             = _price2
    member x.FlagBlockedInGoogle = _flagBlockedInGoogle
    member x.Cy2                 = _cy2
    member x.LevelTo             = _level2
  
    member x.Origin = xmlRpcStruct
  
    override x.ToString() = sprintf "<%s %.2f-%.2f>" _words _priceFrom _price2
  
    static member create xmlRpcStruct = new FilterParams(xmlRpcStruct)
    static member create (words:String, priceFrom:Double, priceTo:Double) =
        let xmlRpcStruct = new XmlRpcStruct()
        xmlRpcStruct.["words"]      <- words
        xmlRpcStruct.["price_from"] <- priceFrom
        xmlRpcStruct.["price_2"]    <- priceTo
        FilterParams.create xmlRpcStruct

type Filter(xmlRpcStruct:XmlRpcStruct) = 
    let _id     = unbox<Id>           xmlRpcStruct.["id"]
    let _name   = unbox<Name>         xmlRpcStruct.["name"]
    let _params = unbox<XmlRpcStruct> xmlRpcStruct.["params"] |> FilterParams.create
  
    member x.Id     = _id
    member x.Name   = _name
    member x.Params = _params
    member x.Origin = xmlRpcStruct
  
    override x.ToString() = sprintf "фильтр: %s | параметры: %s" _name (_params.ToString())
  
    static member create xmlRpcStruct = new Filter(xmlRpcStruct) 

type SiteForUrl(xmlRpcStruct:XmlRpcStruct) = 
    let _id          = unbox<Id>     xmlRpcStruct.["id"]
    let _domainLevel = unbox<Int32>  xmlRpcStruct.["domain_level"]
    let _url         = unbox<String> xmlRpcStruct.["url"]
  
    member x.Id          = _id
    member x.DomainLevel = _domainLevel
    member x.Url         = _url
  
    override x.ToString() = sprintf "%d %s" x.Id x.Url
  
    static member create xmlRpcStruct = new SiteForUrl(xmlRpcStruct)

type Site(xmlRpcStruct:XmlRpcStruct) = 
    let _id                = unbox<Id>     xmlRpcStruct.["id"]
    let _url               = unbox<string> xmlRpcStruct.["url"]
    let _googleGof         = unbox<int>    xmlRpcStruct.["nof_pages_in_google"]
    let _yandexGof         = unbox<int>    xmlRpcStruct.["nof_pages_in_yandex"]
    let _isBlockedInYandex = unbox<bool>   xmlRpcStruct.["blocked_in_yandex"]
    let _citationIndex     = unbox<int>    xmlRpcStruct.["cy"]
    let _isInDmoz          = unbox<bool>   xmlRpcStruct.["in_dmoz"]
    let _domainLevel       = unbox<int>    xmlRpcStruct.["domain_level"]
    let _categoryId        = unbox<int>    xmlRpcStruct.["category_id"]
    let _isInYaca          = unbox<bool>   xmlRpcStruct.["in_yaca"]
    let _pr                = unbox<int>    xmlRpcStruct.["pr"]

    member x.Id                 = _id
    member x.Url                = _url
    member x.GoogleGof          = _googleGof
    member x.YandexGof          = _yandexGof
    member x.IsBlockedInYandex  = _isBlockedInYandex
    member x.CitationIndex      = _citationIndex
    member x.IsInDmoz           = _isInDmoz
    member x.DomainLevel        = _domainLevel
    member x.CategoryId         = _categoryId
    member x.IsInYaca           = _isInYaca
    member x.Pr                 = _pr

    override x.ToString() = sprintf "%s" _url
  
    static member create xmlRpcStruct = new Site(xmlRpcStruct)

type Link(xmlRpcStruct:XmlRpcStruct) = 
    let _id            = unbox<Id>       xmlRpcStruct.["id"]
    let _text          = unbox<string>   xmlRpcStruct.["txt"]
    let _siteUrl       = unbox<string>   xmlRpcStruct.["site_url"]
    let _pageUri       = unbox<string>   xmlRpcStruct.["page_uri"]
    let _price         = unbox<double>   xmlRpcStruct.["price"]
    let _yandexGof     = unbox<int>      xmlRpcStruct.["nof_yandex"]
    let _googleGof     = unbox<int>      xmlRpcStruct.["nof_google"]
    let _pr            = unbox<int>      xmlRpcStruct.["page_pr"]
    let _datePlaced    = unbox<DateTime> xmlRpcStruct.["date_placed"]
    let _citationIndex = unbox<int>      xmlRpcStruct.["site_cy"]
    let _pageLevel     = unbox<int>      xmlRpcStruct.["page_level"]
    let _extLinks      = unbox<int>      xmlRpcStruct.["page_nof_ext_links"]
  
    member x.Id            = _id
    member x.Text          = _text
    member x.SiteUrl       = _siteUrl
    member x.PageUri       = _pageUri
    member x.Price         = _price
    member x.YandexGof     = _yandexGof
    member x.GoogleGof     = _googleGof
    member x.Pr            = _pr
    member x.DatePlaced    = _datePlaced
    member x.CitationIndex = _citationIndex
    member x.PageLevel     = _pageLevel
    member x.ExtLinks     = _extLinks
  
    override x.ToString() = sprintf "%d %s %s %s %.2f" _id _siteUrl _pageUri _text _price
  
    static member create xmlRpcStruct = new Link(xmlRpcStruct)

type Page(xmlRpcStruct:XmlRpcStruct) = 
    let _id         = unbox<Id>     xmlRpcStruct.["id"]
    let _pr         = unbox<int>    xmlRpcStruct.["pr"]
    let _extLinks   = unbox<int>    xmlRpcStruct.["ext_links"]
    let _uri        = unbox<string> xmlRpcStruct.["uri"]
    let _freePlaces = unbox<int>    xmlRpcStruct.["free_places"]
    let _price      = unbox<double> xmlRpcStruct.["price"]
    let _level      = unbox<int>    xmlRpcStruct.["level"]
  
    member x.Id         = _id
    member x.Pr         = _pr
    member x.ExtLinks   = _extLinks
    member x.Uri        = _uri
    member x.FreePlaces = _freePlaces
    member x.Price      = _price
    member x.Level      = _level
  
    override x.ToString() = sprintf "%s %d %.2f" _uri _level _price
  
    static member create xmlRpcStruct = new Page(xmlRpcStruct)

// ----- CustomFilter -----

type SearchArea = 
    | AllSites = 0
    | PrimaryBase = 1
    | DubiousContent = 2

type TripleAnswer = 
    | Yes = 0
    | No = 1
    | DoesNotMatter = 2

type DateAdded = 
    | WholeTime = 0
    | Today = 1
    | Last3Days = 2
    | Last7Days = 3
    | LastMonth = 4

type PagesFromSite = 
    | Single
    | Optimal
    | All

type Categories = 
    | All
    | Selected of string array

type DomainLevel = 
    | AllLevels
    | Level2
    | Level3

type CustomFilter(projectUrlId:int, searchArea:SearchArea, prRange:int option*int option, currentCitationIndexRange:int option*int option, 
                  externalLinksCount:int option, externalLinksForecastCount:int option, priceRange:float option*float option, domainDaysAge:int option, 
                  isDmoz:TripleAnswer, isYaca:TripleAnswer, domainLevel:DomainLevel, nestedLevel:int*int,
                  siteCategories:Categories, yacaCategories:Categories, regions:Categories, domainZones:Categories,
                  words:string, dateAdded:DateAdded, dontShowWithPlacedLinks:bool,
                  isInYandex:TripleAnswer, isInGoogle:TripleAnswer, pagesFromSite:PagesFromSite) = 
    
    static let _siteCategories = ref Map.empty
    static let _yacaCategories = ref Map.empty
    static let _regions = ref Map.empty
    static let _domainZones = ref Map.empty

    static member UpdateCategories (siteCategories:XmlRpcStruct array) (domainZones:XmlRpcStruct array) 
                                   (regions:XmlRpcStruct array) (yacaCategories:XmlRpcStruct array) = 
        _siteCategories := 
            siteCategories 
            |> Array.map (fun siteCategory -> (unbox<Name> siteCategory.["name"],unbox<Id> siteCategory.["id"]))
            |> Map.ofArray
        _yacaCategories :=
            yacaCategories
            |> Array.map (fun siteCategory -> (unbox<Name> siteCategory.["name"],unbox<Id> siteCategory.["id"]))
            |> Map.ofArray
        _regions :=
            regions
            |> Array.map (fun siteCategory -> (unbox<Name> siteCategory.["name"],unbox<Id> siteCategory.["id"]))
            |> Map.ofArray
        _domainZones :=
            domainZones
            |> Array.map (fun siteCategory -> (unbox<Name> siteCategory.["zone"],unbox<Id> siteCategory.["id"]))
            |> Map.ofArray
        ()

    member x.ProjectUrlId = projectUrlId

    member x.ToXmlRpcStruct() = 
        let xmlRpcStruct = new XmlRpcStruct()
        xmlRpcStruct.Add("words", words)
        
        match currentCitationIndexRange with
        | Some cciFrom, Some cciTo -> ( xmlRpcStruct.Add("cy_from", cciFrom); xmlRpcStruct.Add("cy_2", cciTo) )
        | Some cciFrom, None -> xmlRpcStruct.Add("cy_from", cciFrom)
        | None, Some cciTo -> xmlRpcStruct.Add("cy_2", cciTo)
        | None, None -> ()

        match externalLinksForecastCount with
        | Some elc -> xmlRpcStruct.Add("ext_links_forecast", elc)
        | None -> ()

        match nestedLevel with
        | lvlFrom, lvlTo -> (xmlRpcStruct.Add("level_from", lvlFrom); xmlRpcStruct.Add("level_2", lvlTo))

        match domainLevel with
        | AllLevels -> xmlRpcStruct.Add("domain_level", 0)
        | Level2 -> xmlRpcStruct.Add("domain_level", 2)
        | Level3 -> xmlRpcStruct.Add("domain_level", 3)

        match siteCategories with
        | All -> ()
        | Selected categories -> 
            let arr = 
                categories 
                |> Array.fold (fun (sb:StringBuilder) cat -> 
                    match !_siteCategories |> Map.tryFind cat with
                    | Some x -> sb.Append(sprintf "_%d" x)
                    | None -> sb) (new StringBuilder())
                |> StringBuilder.toString
            xmlRpcStruct.Add("categories", arr.Substring(1))

        match yacaCategories with
        | All -> ()
        | Selected categories -> 
            let arr = 
                categories 
                |> Array.fold (fun (sb:StringBuilder) cat -> 
                    match !_yacaCategories |> Map.tryFind cat with
                    | Some x -> sb.Append(sprintf "_%d" x)
                    | None -> sb) (new StringBuilder())
                |> StringBuilder.toString
            xmlRpcStruct.Add("yaca_categories", arr.Substring(1))

        match regions with 
        | All -> ()
        | Selected categories -> 
            let arr = 
                categories 
                |> Array.fold (fun (sb:StringBuilder) cat -> 
                    match !_regions |> Map.tryFind cat with
                    | Some x -> sb.Append(sprintf "_%d" x)
                    | None -> sb) (new StringBuilder())
                |> StringBuilder.toString
            xmlRpcStruct.Add("regions", arr.Substring(1))

        match domainZones with
        | All -> ()
        | Selected categories -> 
            let arr = 
                categories 
                |> Array.fold (fun (sb:StringBuilder) cat -> 
                    match !_domainZones |> Map.tryFind cat with
                    | Some x -> sb.Append(sprintf "_%d" x)
                    | None -> sb) (new StringBuilder())
                |> StringBuilder.toString
            xmlRpcStruct.Add("domain_zones", arr.Substring(1))

        match dateAdded with 
        | DateAdded.WholeTime -> ()
        | DateAdded.Today -> xmlRpcStruct.Add("date_added", DateTime.Now.ToString("dd.MM.yyyy H:mm:ss"))
        | DateAdded.Last3Days -> xmlRpcStruct.Add("date_added", (DateTime.Now - TimeSpan.FromDays(3.0)).ToString("dd.MM.yyyy H:mm:ss"))
        | DateAdded.Last7Days -> xmlRpcStruct.Add("date_added", (DateTime.Now - TimeSpan.FromDays(7.0)).ToString("dd.MM.yyyy H:mm:ss"))
        | DateAdded.LastMonth -> xmlRpcStruct.Add("date_added", (DateTime.Now - TimeSpan.FromDays(30.0)).ToString("dd.MM.yyyy H:mm:ss"))
        | _ -> failwith "unexpected dateAdded"

        match pagesFromSite with
        | PagesFromSite.Single -> xmlRpcStruct.Add("pages_per_site", "one")
        | PagesFromSite.Optimal -> xmlRpcStruct.Add("pages_per_site", "preferred")
        | PagesFromSite.All -> ()

        xmlRpcStruct.Add("no_double_in_project", if dontShowWithPlacedLinks then 0 else 1)

        xmlRpcStruct.Add("flag_blocked_in_google", (int)isInGoogle)

        xmlRpcStruct.Add("flag_blocked_in_yandex", (int)isInYandex)

        match domainDaysAge with
        | Some days -> xmlRpcStruct.Add("days_old_whois", days)
        | None -> ()

        match priceRange with
        | Some priceFrom, Some priceTo -> ( xmlRpcStruct.Add("price_from", priceFrom); xmlRpcStruct.Add("price_2", priceTo) )
        | Some priceFrom, None -> xmlRpcStruct.Add("price_from", priceFrom)
        | None, Some priceTo -> xmlRpcStruct.Add("price_2", priceTo)
        | None, None -> ()

        match externalLinksCount with
        | Some count -> xmlRpcStruct.Add("ext_links", count)
        | None -> ()

        xmlRpcStruct.Add("in_yaca", (int)isYaca)

        xmlRpcStruct.Add("in_dmoz", (int)isDmoz)

        match prRange with
        | Some prFrom, Some prTo -> ( xmlRpcStruct.Add("pr_from", prFrom); xmlRpcStruct.Add("pr_2", prTo) )
        | Some prFrom, None -> xmlRpcStruct.Add("pr_from", prFrom)
        | None, Some prTo -> xmlRpcStruct.Add("pr_2", prTo)
        | None, None -> ()

        xmlRpcStruct.Add("nogood", (int)searchArea)
    
        xmlRpcStruct

    static member Create(projectUrlId:int, searchAreaPrimaryBase:bool, searchAreaDubiousContent:bool, searchAreaAllSites:bool,
                         prFrom:string, prTo:string, currentCitationIndexFrom:string, currentCitationIndexTo:string,
                         externalLinksCount:string, externalLinksForecastCount:string, priceFrom:string, priceTo:string, domainDaysAge:string,
                         isDmoz:int, isYaca:int, 
                         domainLevelAllLevels:bool, domainLevelIs2nd:bool, domainLevelIs3rd:bool,
                         nestedLevelMain:bool, nestedLevel2nd:bool, nestedLevel3rd:bool,
                         siteCategoriesIsAll:bool, siteCategoriesSelected:string array,
                         yacaCategoriesIsAll:bool, yacaCategoriesSelected:string array,
                         regionsIsAll:bool, regionsSelected:string array,
                         domainZonesIsAll:bool, domainZonesSelected:string array,
                         words:string, dateAdded:int, dontShowWithPlacedLinks:bool,
                         isInYandex:int, isInGoogle:int, 
                         pagesFromSiteSingle:bool, pagesFromSiteOptimal:bool, pagesFromSiteAll:bool) = 
        let searchArea = 
            match searchAreaPrimaryBase, searchAreaDubiousContent, searchAreaAllSites with 
            | true, false, false -> SearchArea.PrimaryBase
            | false, true, false -> SearchArea.DubiousContent
            | false, false, true -> SearchArea.AllSites
            | _ -> failwith "Filter.Create/searchArea"
        let prRange = (Helper.Parse<int>(prFrom), Helper.Parse<int>(prTo))
        let currentCitationIndexRange = (Helper.Parse<int>(currentCitationIndexFrom), Helper.Parse<int>(currentCitationIndexTo))
        let externalLinksCount = Helper.Parse<int>(externalLinksCount)
        let externalLinksForecastCount = Helper.Parse<int>(externalLinksForecastCount)
        let priceRange = (Helper.Parse<float>(priceFrom), Helper.Parse<float>(priceTo))
        let domainDaysAge = Helper.Parse<int>(domainDaysAge)
        let isDmoz = enum<TripleAnswer> isDmoz
        let isYaca = enum<TripleAnswer> isYaca
        let domainLevel = 
            match domainLevelAllLevels, domainLevelIs2nd, domainLevelIs3rd with 
            | true, false, false -> DomainLevel.AllLevels
            | false, true, false -> DomainLevel.Level2
            | false, false, true -> DomainLevel.Level3
            | _ -> failwith "Filter.Create/domainLevel"
        let netstedLevel =
            let getNestedLevelFrom = 
                match nestedLevelMain, nestedLevel2nd, nestedLevel3rd with
                | true, _, _ -> 1
                | false, true, _ -> 2
                | false, false, true -> 3
                | false, false, false -> 0
            let getNestedLevelTo = 
                match nestedLevelMain, nestedLevel2nd, nestedLevel3rd with
                | true, false, false -> 1
                | _, true, false -> 2
                | _, _, true -> 3
                | false, false, false -> 0
            getNestedLevelFrom,getNestedLevelTo
        let siteCategories = 
            if siteCategoriesIsAll then Categories.All
            else Categories.Selected(siteCategoriesSelected)
        let yacaCategories = 
            if yacaCategoriesIsAll then Categories.All
            else Categories.Selected(yacaCategoriesSelected)
        let regions = 
            if regionsIsAll then Categories.All
            else Categories.Selected(regionsSelected)
        let domainZones = 
            if domainZonesIsAll then Categories.All
            else Categories.Selected(domainZonesSelected)
//        let words = 
//            let wordsSplitted = words.Split([| "," |], StringSplitOptions.RemoveEmptyEntries)
//            if wordsSplitted.Length = 0 then None
//            else Some ( wordsSplitted |> Array.map (fun x -> x.Trim()) )
        let dateAdded = enum<DateAdded> dateAdded
        let isInYandex = enum<TripleAnswer> isInYandex
        let isInGoogle = enum<TripleAnswer> isInGoogle
        let pagesFromSite = 
            match pagesFromSiteSingle, pagesFromSiteOptimal, pagesFromSiteAll with
            | true, false, false -> PagesFromSite.Single
            | false, true, false -> PagesFromSite.Optimal
            | false, false, true -> PagesFromSite.All
            | _ -> failwith "Filter.Create/pagesFromSite"
        let filter = new CustomFilter(projectUrlId, searchArea, prRange, currentCitationIndexRange, externalLinksCount, externalLinksForecastCount, priceRange, domainDaysAge, 
                                      isDmoz, isYaca, domainLevel, netstedLevel, siteCategories, yacaCategories, regions, domainZones, 
                                      words, dateAdded, dontShowWithPlacedLinks, isInYandex, isInGoogle, pagesFromSite)
        filter