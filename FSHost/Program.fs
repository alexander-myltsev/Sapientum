open Sapientum
open Sapientum.Helper
open Sapientum.Types.Wpf
open Sapientum.Types.Sape
open Sapientum.XmlRpc

open System
open System.Diagnostics
open System.Windows
open Sapientum.Types

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

let ProcessProjectSelection (sapeApi:SapeApi) (prjUrlId:Id) = 
    async {
        let urlLinks = sapeApi.GetUrlLinks prjUrlId UrlLinkStatus.WaitSEO
        Debug.WriteLine(sprintf "urlLinks.Length: %d" urlLinks.Length)
        ()
    } |> Async.Start

[<STAThread>]
do 
    let sapeApi = new SapeApi()
    let app = new Windows.Application()
    let win = new DataTestWindow()
    let loginInfoWpfData = win.GetLoginInfo()
    let projectsWpfData = win.GetProjects()
    win.ButtonLoginClick 
    |> Event.add (fun _ -> ProcessLogin sapeApi loginInfoWpfData (win.GetPassword().ToMD5Hash()) projectsWpfData)
    win.ProjectUrlSelected
    |> Event.add (fun prjUrlId -> ProcessProjectSelection sapeApi prjUrlId)

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