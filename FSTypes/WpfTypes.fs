namespace Sapientum.Types.Wpf

open Sapientum

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
    member x.Sync = _sync
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


open System.Windows.Documents
open System.Windows.Controls
open Sapientum.Types.Sape

type DataProviderForWaitingSites() =
    inherit WpfCollectionData()

    let _table = new Table( CellSpacing = 10.0 )
    let _handler (e:Windows.Navigation.RequestNavigateEventArgs) = 
        Diagnostics.Process.Start(e.Uri.ToString()) |> ignore
    let _map = ref Map.empty
    
    let navigateUri (urlLink:Link) = sprintf "%s%s" urlLink.SiteUrl urlLink.PageUri 

    member x.GetRows (urlLinks:Link list) =
        x.Caller(fun () ->
            _map := 
                seq {
                    for urlLink in urlLinks do
                        let rowGroup = new TableRowGroup()
                        _table.RowGroups.Add(rowGroup)
                        let row1 = new TableRow()
                        rowGroup.Rows.Add(row1)

                        let grid = new Grid()
                        let paragraph = new Paragraph()
                        paragraph.Inlines.Add(grid)
                        let tableCell1 = new TableCell(paragraph)
                        row1.Cells.Add(tableCell1)

                        grid.RowDefinitions.Add(new RowDefinition())
                        grid.RowDefinitions.Add(new RowDefinition())
                        grid.RowDefinitions.Add(new RowDefinition())
                        grid.RowDefinitions.Add(new RowDefinition())
                        
                        let stackPanel0 = new StackPanel(Orientation = Orientation.Horizontal)
                        let hyperlink = 
                            new Hyperlink(
                                new Run((navigateUri urlLink).Substring("http://".Length)),
                                NavigateUri = new Uri(navigateUri urlLink)
                            )
                        let label = new Label(Content = hyperlink)
                        hyperlink.RequestNavigate |> Event.add _handler |> ignore
                        stackPanel0.Children.Add(new CheckBox()) |> ignore
                        stackPanel0.Children.Add(label) |> ignore
                        Grid.SetRow(stackPanel0, 0)

                        let stackPanel1 = new StackPanel(Orientation = Orientation.Horizontal)
                        stackPanel1.Children.Add(new Label(Content = sprintf "Я:[%d]" urlLink.YandexGof)) |> ignore
                        stackPanel1.Children.Add(new Label(Content = sprintf "G:[%d]" urlLink.GoogleGof)) |> ignore                
                        stackPanel1.Children.Add(new Label(Content = sprintf "Дата: %s" (urlLink.DatePlaced.ToShortDateString()))) |> ignore
                        Grid.SetRow(stackPanel1, 1)

                        let stackPanel2 = new StackPanel(Orientation = Orientation.Horizontal)
                        stackPanel2.Children.Add(new Label(Content = sprintf "Текст: %s" (urlLink.Text))) |> ignore
                        Grid.SetRow(stackPanel2, 2)

                        let stackPanel3 = new StackPanel(Orientation = Orientation.Horizontal)
                        let titleLabel = new Label(Content = "Title: ")
                        stackPanel3.Children.Add(titleLabel) |> ignore
                        Grid.SetRow(stackPanel3, 3)

                        grid.Children.Add(stackPanel0) |> ignore
                        grid.Children.Add(stackPanel1) |> ignore
                        grid.Children.Add(stackPanel2) |> ignore
                        grid.Children.Add(stackPanel3) |> ignore

                        yield (navigateUri urlLink),(urlLink,titleLabel)
                    } |> Map.ofSeq
            ()
        )

    member x.UpdateTitle (urlLink:Link, title:string) = 
        let _, titleLabel = Map.find (navigateUri urlLink) !_map
        x.Caller( fun() -> titleLabel.Content <- sprintf "Title: %s" title )
        ()
    
    member x.Metadata = _table

type DataProviderForSearchedSites() = 
    let _table = new Table()

    member x.BuildTable() = 
        for i in 0..10 do
            let rowGrp = new TableRowGroup()
            _table.RowGroups.Add(rowGrp)
            let row = new TableRow()
            rowGrp.Rows.Add(row)
            row.Cells.Add(new TableCell(new Paragraph(new Run(sprintf "datarow %d" i))))

            let pg = new Paragraph()
            let stackPanel = new StackPanel()
            let label1 = new Label ( Content = sprintf "1-Contents-%d" i )
            let label2 = new Label ( Content = sprintf "2-Contents-%d" i )
            let label3 = new Label ( Content = sprintf "3-Contents-%d" i )
            let label4 = new Label ( Content = sprintf "4-Contents-%d" i )
            stackPanel.Children.Add(label1) |> ignore
            stackPanel.Children.Add(label2) |> ignore
            stackPanel.Children.Add(label3) |> ignore
            stackPanel.Children.Add(label4) |> ignore

            pg.Inlines.Add(stackPanel)

            row.Cells.Add(new TableCell(pg))
        _table

    member x.AddRow (index:int) = 
        let rowGrp = new TableRowGroup()
        _table.RowGroups.Add(rowGrp)
        let row = new TableRow()
        rowGrp.Rows.Add(row)
        row.Cells.Add(new TableCell(new Paragraph(new Run(sprintf "datarow %d" index))))
    
    member x.Metadata = x.BuildTable()