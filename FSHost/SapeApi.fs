module Sapientum.XmlRpc

open Sapientum.Types.Sape

open System

open CookComputing.XmlRpc

//[<XmlRpcUrl("http://xmlrpc.sape.ru/api_xmlrpc.php")>]
//[<XmlRpcUrl("http://api.sape.ru/xmlrpc/")>]
[<XmlRpcUrl("http://api.sape.ru/xmlrpc/?v=extended")>]
type ISapeApi = 
    inherit IXmlRpcProxy

    /// (int) sape.login((char) login, (char) password [, (boolean) md5 = false]). 
    /// Авторизация в системе, необходимо вызывать в начале каждой сессии. Далее при каждом следующем запросе нужно передавать все вернувшиеся cookie. Если параметр md5 == true, то пароль должен быть в захеширован алгоритмом MD5. Результатом выполнения функции является id пользователя.
    [<XmlRpcMethod("sape.login")>]
    abstract Login : String -> String -> Boolean -> Int32

    /// (array) sape.get_user(). информация о пользователе:
    ///   * login,
    ///   * e-mail,
    ///   * баланс,
    ///   * число купленных ссылок,
    ///   * число купленных ссылок в статусе ОК,
    ///   * месячный бюджет,
    ///   * месячный бюджет по статусу ОК.
    [<XmlRpcMethod("sape.get_user")>]
    abstract GetUserInfo : unit -> XmlRpcStruct
  
    /// (array) sape.get_projects([(boolean)  show_deleted])
    [<XmlRpcMethod("sape.get_projects")>]
    abstract GetProjects : Boolean -> XmlRpcStruct array

    /// (array) sape.get_urls((int) project_id [, (boolean) show_deleted])
    [<XmlRpcMethod("sape.get_urls")>]
    abstract GetProjectUrls : Id -> Boolean -> XmlRpcStruct array

    [<XmlRpcMethod("sape.get_url_links")>]
    //abstract GetUrlLinks : Int32 -> String -> DateTime -> DateTime -> Int32 -> XmlRpcStruct array
    abstract GetUrlLinks : Id -> String -> XmlRpcStruct array
  
    [<XmlRpcMethod("sape.get_url_links")>]
    //abstract GetUrlLinks : Int32 -> String -> DateTime -> DateTime -> Int32 -> XmlRpcStruct array
    abstract GetUrlsLinks : Id array -> String -> XmlRpcStruct array

    [<XmlRpcMethod("sape.get_categories")>]
    abstract GetCategories : unit -> XmlRpcStruct array

    [<XmlRpcMethod("sape.get_domain_zones")>]
    abstract GetDomainZones : unit -> XmlRpcStruct array

    [<XmlRpcMethod("sape.get_regions")>]
    abstract GetRegions : unit -> XmlRpcStruct array

    [<XmlRpcMethod("sape.get_yaca_categories")>]
    abstract GetYacaCategories : unit -> XmlRpcStruct array

    [<XmlRpcMethod("sape.get_balance")>]
    abstract GetBalance : unit -> float

    [<XmlRpcMethod("sape.search_sites")>]
    abstract SearchSites : Id -> XmlRpcStruct -> int -> int -> XmlRpcStruct array

    [<XmlRpcMethod("sape.search_pages")>]
    abstract SearchPages : Id -> Id -> XmlRpcStruct -> XmlRpcStruct array

    /// (int) sape.placement_create((int)  page_id, (int) url_id, (char|int) anchor[, (float) max_price)]
    [<XmlRpcMethod("sape.placement_create")>]
    abstract PlacementCreate : Id -> Id -> int -> int
  
    /// (array) sape.get_filters([(boolean)  show_params = false]). 
    /// Получение списка фильтров, если указан параметр show_params, то возвращает параметры фильтра в виде хэша.
    [<XmlRpcMethod("sape.get_filters")>]
    abstract GetFilters : bool -> XmlRpcStruct array
  
    /// (array) sape.get_project_links((int)  project_id [, (char) status, (timestamp) date_start, (timestamp) date_stop, (int) pn = 0])
    [<XmlRpcMethod("sape.get_project_links")>]
    abstract GetProjectLinks : Id -> XmlRpcStruct array

    /// (bool) sape.placement_accept_seo((int) id) — 
    /// одобрение ссылки оптимизатором. Необходимо одобрять ссылки в статусе WAIT_SEO
    [<XmlRpcMethod("sape.placement_accept_seo")>]
    abstract PlacementAccept : Id -> bool

type UrlLinkStatus = 
    | WaitWM
    | WaitSEO
    | Ok
    | Error
    | Sleep

    member x.StringValue = 
        match x with
        | WaitWM -> "WAIT_WM"
        | WaitSEO -> "WAIT_SEO"
        | Ok -> "OK"
        | Error -> "ERROR"
        | Sleep -> "SLEEP"

type SapeApi() = 
    let sapeProxy = XmlRpcProxyGen.Create<ISapeApi>()
  
    member x.Login login password isMd5 = 
        try 
            let result = sapeProxy.Login login password isMd5
            Some result
        with
        | :? XmlRpcFaultException as ex when ex.FaultCode = 666 -> None

    member x.GetUserInfo() = sapeProxy.GetUserInfo() |> UserInfo.create

    member x.GetProjects() = sapeProxy.GetProjects false |> Array.map Project.create |> List.ofArray

    member x.GetProjectUrls id = sapeProxy.GetProjectUrls id false |> Array.map ProjectUrl.create |> List.ofArray

    member x.GetFilters() = sapeProxy.GetFilters true |> Array.map Filter.create |> List.ofArray

    //member x.SearchSites urlId filter pageNumber positionsCount = sapeProxy.SearchSites urlId filter pageNumber positionsCount

    member x.SearchSites urlId filter =
        let sitesCount = 2000
        let rec searchSites pageNumber (resultSites:XmlRpcStruct list) =
            let res = 
                try 
                    sapeProxy.SearchSites urlId filter pageNumber sitesCount |> List.ofArray |> Option.Some
                with
                    | :? XmlRpcFaultException as ex when ex.FaultCode = 667 -> Some []
                    | :? XmlRpcFaultException as ex when ex.FaultCode = 7000 -> printfn "%s" ex.Message; None
                    | ex -> raise ex
            //printfn "resultSites.Length = %d" resultSites.Length
            System.Diagnostics.Debug.WriteLine(sprintf "количество скачанных заголовков сайтов: %d" resultSites.Length)
            match res with 
            | Some [] -> resultSites |> List.map Site.create  //searchSites pageNumber resultSites
            | Some sites -> 
            if sites.Length = sitesCount then searchSites (pageNumber + 1) (List.append resultSites sites)
            else (List.append resultSites sites) |> List.map Site.create
            | None -> resultSites |> List.map Site.create 
        searchSites 0 []

    member x.GetProjectLinks projectId = sapeProxy.GetProjectLinks projectId

    member x.GetUrlsLinks urlsIds (status:UrlLinkStatus) = sapeProxy.GetUrlsLinks urlsIds status.StringValue |> Array.map Link.create |> List.ofArray

    member x.GetUrlLinks  urlId   (status:UrlLinkStatus) = sapeProxy.GetUrlLinks  urlId   status.StringValue |> Array.map Link.create |> List.ofArray

    member x.SearchPages urlId siteId filter = sapeProxy.SearchPages urlId siteId filter |> Array.map Page.create |> List.ofArray

    member x.PlacementCreate pageId urlId anchor = sapeProxy.PlacementCreate pageId urlId anchor

    member x.GetCategories() = sapeProxy.GetCategories()

    member x.GetDomainZones() = sapeProxy.GetDomainZones()

    member x.GetRegions() = sapeProxy.GetRegions()

    member x.GetYacaCategories() = sapeProxy.GetYacaCategories()

    member x.PlacementAccept id = sapeProxy.PlacementAccept id