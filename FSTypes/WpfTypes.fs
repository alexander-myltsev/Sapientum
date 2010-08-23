namespace Sapientum.Types.Wpf

open System
open System.Collections.Generic
open System.Collections.Specialized
open System.Collections.ObjectModel
open System.ComponentModel

type Name = String
type Id = Int32

type LoginStatus = 
    | LoggedOff = 1
    | LoggingIn = 2
    | LoggedIn = 3
    | LoginFailed = 4

type WpfData() = 
    let _propertyChangedEvent = new Event<_,_>()
    let _sync = new Object()

    member x.PropertyChangedEvent = _propertyChangedEvent

    member x.This = x

    member x.TriggerChangeProperties changedPropertiesNames = 
        ("This" :: changedPropertiesNames)
        |> List.iter (fun changedPropertiesName -> _propertyChangedEvent.Trigger(x, new PropertyChangedEventArgs(changedPropertiesName)))
    
    member x.Set (property: 'T ref) value changedPropertiesNames = 
        lock _sync (fun () ->
            property := value
            x.TriggerChangeProperties changedPropertiesNames
        )

    interface INotifyPropertyChanged with
        [<CLIEvent>]
        member x.PropertyChanged = _propertyChangedEvent.Publish

type WpfCollectionData() = 
    inherit WpfData()

    let _collectionChangedEvent = new Event<_,_>()

    let currentDispatcher = Windows.Threading.Dispatcher.CurrentDispatcher
    let action0 = new Action<unit->unit>(fun f -> f())
    let action1 = new Action<'T->unit, 'T>(fun f x -> f(x))
    
    member x.Caller (f:unit->unit) =
        currentDispatcher.Invoke(
            Windows.Threading.DispatcherPriority.Normal, 
            (action0:Action<unit->unit>), f) |> ignore

    member x.Caller (f:'T->unit, arg:'T) = 
        currentDispatcher.Invoke(
            Windows.Threading.DispatcherPriority.Normal, 
            (action1:Action<'T->unit, 'T>), f, arg) |> ignore

    interface INotifyCollectionChanged with
        [<CLIEvent>]
        member x.CollectionChanged = _collectionChangedEvent.Publish

type LoginInfo() =
    inherit WpfData()
 
    let _currentLogin = ref "sanyok_m" //String.Empty
    let _passwordHash = ref String.Empty
    let _status       = ref LoginStatus.LoggedOff

    let _loggedLogin  = ref String.Empty
    let _balance      = ref 0.0

    member x.CurrentLogin 
        with get() = !_currentLogin
        and set v = x.Set _currentLogin v ["Login"]

    member x.PasswordHash
        with get() = !_passwordHash
        and set v = x.Set _passwordHash v ["PasswordHash"]

    member x.Status 
        with get() = !_status
        and set v = x.Set _status v ["Status"]

    member x.LoggedLogin
        with get() = !_loggedLogin
        and set v = x.Set _loggedLogin v ["LoggedLogin"]

    member x.Balance
        with get() = !_balance
        and set v = x.Set _balance v ["Balance"]

type ProjectUrl (id:Id, name:Name) =
    inherit WpfData()

    let _name = ref name
    
    member x.UrlId = id

    member x.UrlName 
        with get() = !_name
        and set v = x.Set _name v ["UrlName"]

type Project (id:Id, name:Name, urls:ProjectUrl list) =
    inherit WpfData()

    let _name = ref name
    let _urls = 
        let urls' = urls |> List.sortBy (fun x -> x.UrlName) |> List.rev 
        new List<ProjectUrl>(urls')

    member x.Name 
        with get() = !_name
        and set v = x.Set _name v ["Name"]
    member x.Urls with get() = _urls

//    member x.AddProjectUrls(urls:ProjectUrl list) = 
//        _urls.AddRange(urls)
//        x.TriggerChangeProperties ["Urls"]

type Projects() = 
    inherit WpfCollectionData()

    let _projects = new ObservableCollection<Project>()

    member x.AddProject(prj:Project) = 
        x.Caller(_projects.Add, prj)
        x.TriggerChangeProperties ["ProjectsCount"; "Projects"]

    member x.SetProjects(prjs:Project seq) = 
        //x.Caller(_projects.Clear)
        prjs 
        |> Seq.sortBy (fun x -> x.Name) 
        |> List.ofSeq |> List.rev |> Seq.ofList
        |> Seq.iter (fun prj -> x.Caller(_projects.Add, prj))
        x.TriggerChangeProperties ["ProjectsCount"; "Projects"]

    member x.ProjectsCount = _projects.Count

    member x.Projects = _projects

module DataProvider = 
    let mutable Login = "login"