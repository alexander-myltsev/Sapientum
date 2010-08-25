open Sapientum
open Sapientum.Helper
open Sapientum.Types.Wpf
open Sapientum.Types.Sape
open Sapientum.XmlRpc

open System
open System.IO
open System.Net
open System.Diagnostics
open System.Text
open System.Windows
open Sapientum.Types

open Microsoft.FSharp.Control.WebExtensions

open WpfControlLib

let ProcessLogin (sapeApi:SapeApi) (loginInfoWpfData:LoginInfo) (passwordHash:String) (projectsWpfData:Projects) = 
    async {
        Debug.WriteLine("login launched...")
        loginInfoWpfData.Status <- LoginStatus.LoggingIn
        Threading.Thread.Sleep 5000
        loginInfoWpfData.PasswordHash <- passwordHash
        
        let result = sapeApi.Login loginInfoWpfData.CurrentLogin loginInfoWpfData.PasswordHash true
        match result with 
        | None ->
            loginInfoWpfData.Status <- LoginStatus.LoginFailed
        | Some res ->
            let userInfo = sapeApi.GetUserInfo()
            loginInfoWpfData.Balance <- userInfo.Balance
            loginInfoWpfData.LoggedLogin <- userInfo.Login
            loginInfoWpfData.Status <- LoginStatus.LoggedIn
            Debug.WriteLine(sprintf "login processed for %s. result: %d. balance: %f" userInfo.Login res userInfo.Balance)

            async {
                let categories = sapeApi.GetCategories()
                let domainZones = sapeApi.GetDomainZones()
                let regions = sapeApi.GetRegions()
                let yacaCategories = sapeApi.GetYacaCategories()
                ()
            } |> Async.Start
            
            let getProjectLinks (project:Project) = async {
                let projectUrls = 
                    sapeApi.GetProjectUrls project.Id 
                    |> List.map (fun url -> new Types.Wpf.ProjectUrl(url.Id, url.Name))
                let wpfProject = new Types.Wpf.Project(project.Id, project.Name, projectUrls)
                Debug.WriteLine(sprintf "projectUrls.Length: %d" projectUrls.Length)
                return wpfProject
            }
            let projects = sapeApi.GetProjects()
            let wpfPrjs = 
                projects 
                |> List.map getProjectLinks 
                |> Async.Parallel 
                |> Async.RunSynchronously
            projectsWpfData.SetProjects wpfPrjs
        
    } |> Async.Start

let getTitle (headers:WebHeaderCollection) (responseStream:Stream) = 
    async {
        let buffer = Array.create 2048 0uy
        let! bytesRead = responseStream.AsyncRead (buffer, 0, buffer.Length)
        let memoryStream = new MemoryStream(buffer, 0, bytesRead)
        let html = 
            try
                let header = headers.[HttpResponseHeader.ContentType]
                match header with 
                | ParseRegex "charset=([-\w\d]+)" ["utf8"] -> memoryStream |> streamReader Encoding.UTF8 |> StreamReader.readToEnd
                | ParseRegex "charset=([-\w\d]+)" ["windows-1251"] -> memoryStream |> streamReader (Encoding.GetEncoding 1251) |> StreamReader.readToEnd
                | ParseRegex "charset=([-\w\d]+)" [charset] -> memoryStream |> streamReader (Encoding.GetEncoding charset) |> StreamReader.readToEnd
                | h -> 
                    let html = memoryStream |> streamReader (Encoding.GetEncoding 1251) |> StreamReader.readToEnd
                    match html with 
                    | ParseRegex @"charset=([-\w\d]+)" [charset] -> 
                        memoryStream.Position<-0L
                        let buffer = Array.create 2048 0uy
                        let bytesRead = memoryStream.Read(buffer, 0, buffer.Length)
                        Encoding.GetEncoding(charset).GetString(buffer, 0, bytesRead)
                    | _ -> html
            with 
            | :? Exception as ex -> 
                let buffer = Array.create 2000 0uy
                let bytesRead = memoryStream.Read(buffer, 0, buffer.Length)
                Encoding.GetEncoding(1251).GetString(buffer, 0, bytesRead)
        match html with
        | ParseRegex @"<title>(.*?)</title>" [title] -> return title
        | _ -> return "не найден на странице"
    }

let ProcessProjectSelection (sapeApi:SapeApi) (prjUrlId:Id) (dataProviderForWaitingSites:DataProviderForWaitingSites) = 
    async {
        let urlLinks = sapeApi.GetUrlLinks prjUrlId UrlLinkStatus.WaitSEO
        Debug.WriteLine(sprintf "urlLinks.Length: %d" urlLinks.Length)
        dataProviderForWaitingSites.GetRows urlLinks
        let getTitleAsync (urlLink:Link) = async {
                let urlStr = sprintf "%s%s" urlLink.SiteUrl urlLink.PageUri
                let req = HttpWebRequest.Create(urlStr, Timeout = 5000)
                try 
                    let! resp = req.AsyncGetResponse()
                    Debug.WriteLine(sprintf "req.AsyncGetResponse(): %s" urlLink.SiteUrl)
                    let respStream = resp.GetResponseStream()
                    //Debug.WriteLine(sprintf "streamString.Substring(0,10): %s" (streamString.Substring(0,10)))
                    let! title = getTitle resp.Headers respStream
                    dataProviderForWaitingSites.UpdateTitle (urlLink, title)
                with
                | _ -> 
                    Debug.WriteLine(sprintf "req.AsyncGetResponse(): (exc) %s" urlLink.SiteUrl)
                    dataProviderForWaitingSites.UpdateTitle (urlLink, "Не получен")

                req.Abort()
            }
        let k = urlLinks 
                |> List.map getTitleAsync 
                |> Async.Parallel 
                |> Async.Ignore |> Async.Start
                //|> Async.RunSynchronously


//        urlLinks |> List.iter (fun urlLink ->
//            let urlStr = sprintf "%s%s" urlLink.SiteUrl urlLink.PageUri
//            let req = HttpWebRequest.Create(urlStr, Timeout = 10000)
//            try 
//                let resp = req.GetResponse()
//                Debug.WriteLine(sprintf "req.GetResponse(): %s" urlLink.SiteUrl)
//                let respStream = resp.GetResponseStream()
//                //use reader = new StreamReader(stream)
//                //let! streamString = reader.AsyncReadToEnd()
//                //Debug.WriteLine(sprintf "streamString.Substring(0,10): %s" (streamString.Substring(0,10)))
//                let title = getTitle resp.Headers respStream
//                //addDataPiece title
//                dataProviderForWaitingSites.UpdateTitle (urlLink, title)
//            with
//            | _ -> 
//                Debug.WriteLine(sprintf "req.GetResponse(): (exc) %s" urlLink.SiteUrl)
//                dataProviderForWaitingSites.UpdateTitle (urlLink, "Не получен")
//
//            req.Abort()
//        )
        ()
    } |> Async.Start

[<STAThread>]
do 
    let sapeApi = new SapeApi()
    let app = new Windows.Application()
    let win = new MainWindow()
    let loginInfoWpfData = win.GetLoginInfo()
    let projectsWpfData = win.GetProjects()
    let dataProviderForSearchedSites = win.GetDataProviderForSearchedSites()
    let dataProviderForWaitingSites  = win.GetDataProviderForWaitingSites()
#if DEBUG
    ProcessLogin sapeApi loginInfoWpfData (win.GetPassword().ToMD5Hash()) projectsWpfData
#endif
    win.ButtonLoginClick 
    |> Event.add (fun x -> ProcessLogin sapeApi loginInfoWpfData (win.GetPassword().ToMD5Hash()) projectsWpfData)
//    win.ProjectUrlSelected
//    |> Event.add (fun prjUrlId -> ProcessProjectSelection sapeApi prjUrlId)
    win.WaitingSitesRefreshClick
    |> Event.add (fun prjUrlId ->
            ProcessProjectSelection sapeApi prjUrlId dataProviderForWaitingSites
        )
    win.SearchSitesRequested
    |> Event.add (fun customFilter -> 
            let filters = sapeApi.GetFilters()
            let sites = sapeApi.SearchSites customFilter.ProjectUrlId (customFilter.ToXmlRpcStruct())
            dataProviderForSearchedSites.AddRow 3
            Debug.WriteLine(sprintf "filters.Length: %d" filters.Length)
        )

//        System.Threading.ThreadPool.QueueUserWorkItem(
//            fun state -> 
//                let rec loop() =
//                    let loginInfo = win.GetLoginInfo()
//                    loginInfo.Login <- System.DateTime.Now.ToString()
//                    //System.Diagnostics.Debug.WriteLine(sprintf "loginInfo:%s" loginInfo.Login)
//                    System.Threading.Thread.Sleep 1000
//                    loop()
//                loop()
//        ) |> ignore

    app.Run(win) |> ignore