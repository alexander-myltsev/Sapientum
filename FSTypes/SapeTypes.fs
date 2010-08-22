namespace Sapientum.Types.Sape

open CookComputing.XmlRpc

open System

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
    let _id         = unbox<Id>     xmlRpcStruct.["id"]
    let _projectId  = unbox<Int32>  xmlRpcStruct.["project_id"]
    let _name       = unbox<Name>   xmlRpcStruct.["name"]
    let _url        = unbox<String> xmlRpcStruct.["url"]
  
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
    let _id  = unbox<Id>     xmlRpcStruct.["id"]
    let _url = unbox<String> xmlRpcStruct.["url"]
  
    member x.Id  = _id
    member x.Url = _url
  
    override x.ToString() = sprintf "%s" _url
  
    static member create xmlRpcStruct = new Site(xmlRpcStruct)

type Link(xmlRpcStruct:XmlRpcStruct) = 
    let _id      = unbox<Id>     xmlRpcStruct.["id"]
    let _text    = unbox<String> xmlRpcStruct.["txt"]
    let _siteUrl = unbox<String> xmlRpcStruct.["site_url"]
    let _pageUri = unbox<String> xmlRpcStruct.["page_uri"]
    let _price   = unbox<Double> xmlRpcStruct.["price"]
  
    member x.Id = _id
  
    override x.ToString() = sprintf "%d %s %s %s %.2f" _id _siteUrl _pageUri _text _price
  
    static member create xmlRpcStruct = new Link(xmlRpcStruct)

type Page(xmlRpcStruct:XmlRpcStruct) = 
    let _id = unbox<Id> xmlRpcStruct.["id"]
    let _pr = unbox<Int32> xmlRpcStruct.["pr"]
    let _extLinks = unbox<Int32> xmlRpcStruct.["ext_links"]
    let _uri = unbox<String> xmlRpcStruct.["uri"]
    let _freePlaces = unbox<Int32> xmlRpcStruct.["free_places"]
    let _price = unbox<Double> xmlRpcStruct.["price"]
    let _level = unbox<Int32> xmlRpcStruct.["level"]
  
    member x.Id    = _id
    member x.Uri   = _uri
    member x.Price = _price
  
    override x.ToString() = sprintf "%s %d %.2f" _uri _level _price
  
    static member create xmlRpcStruct = new Page(xmlRpcStruct)