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
 
#if DEBUG
    let _currentLogin = ref "sanyok_m"
#else
    let _currentLogin = ref String.Empty
#endif
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
        let urls' = urls |> List.sortBy (fun x -> x.UrlName) // |> List.rev 
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
        x.Caller(_projects.Clear)
        prjs 
        |> Seq.sortBy (fun x -> x.Name) 
        //|> List.ofSeq |> List.rev |> Seq.ofList
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
    let _map : Map<string,(Link*TextBlock)> ref = ref Map.empty

    let _placements = new System.Collections.Generic.List<(CheckBox*Link)>()
    
    let navigateUri (urlLink:Link) = sprintf "%s%s" urlLink.SiteUrl urlLink.PageUri 

    let _id = ref 0
    member x.GetId() = !_id

    member x.GetRows (urlLinks:Link list) =
        _id := !_id + 1
        _map := Map.empty
        _placements.Clear()

        x.Caller(fun () ->
            _table.RowGroups.Clear()

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
                        let checkBox = new CheckBox(Margin = new Windows.Thickness(0.0,5.0,0.0,0.0))
                        stackPanel0.Children.Add(checkBox) |> ignore
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
                        let textBlock = 
                            new TextBlock(
                                Text = "Title: ", 
                                TextWrapping = Windows.TextWrapping.Wrap,
                                //Width = Double.NaN,
                                Height = 60.0)
                        //stackPanel3.Children.Add(textBlock) |> ignore
                        paragraph.Inlines.Add(textBlock)
                        Grid.SetRow(stackPanel3, 3)

                        grid.Children.Add(stackPanel0) |> ignore
                        grid.Children.Add(stackPanel1) |> ignore
                        grid.Children.Add(stackPanel2) |> ignore
                        grid.Children.Add(stackPanel3) |> ignore

                        _placements.Add((checkBox,urlLink))

                        yield (navigateUri urlLink),(urlLink,textBlock)
                    } |> Map.ofSeq
            ()
        )

    member x.Highlight (words:string list) =
        x.Caller ( fun _ -> 
            !_map |> Map.iter (fun navUri (urlLink,textBlock) -> 
                                    match words |> List.tryFind (textBlock.Text.ToLower().Contains) with
                                    | Some word -> textBlock.Background <- Windows.Media.Brushes.Yellow
                                    | None -> textBlock.Background <- null)
        )

    member x.UpdateTitle (urlLink:Link, title:string) = 
        match Map.tryFind (navigateUri urlLink) !_map with
        | Some (_, titleLabel) -> x.Caller( fun() -> titleLabel.Text <- sprintf "Title: %s" title )
        | None -> ()

    member x.GetPlacements() = 
        _placements 
        |> Seq.filter (fun (checkBox,_) -> checkBox.IsChecked.Value)
        |> Seq.map (fun (checkBox,urlLink) -> (checkBox.IsEnabled <- false; urlLink))
        |> List.ofSeq
    
    member x.Metadata = _table

type DataProviderForSearchedSites() = 
    inherit WpfCollectionData()

    let _table = new Table( CellSpacing = 10.0 )
    let _handler (e:Windows.Navigation.RequestNavigateEventArgs) = 
        Diagnostics.Process.Start(e.Uri.ToString()) |> ignore
    let _map : Map<Id, (Site*Paragraph*Collections.Generic.List<TextBlock>)> ref = ref Map.empty
    let _placements = new System.Collections.Generic.List<(CheckBox*Page)>()

    let _id = ref 0
    member x.GetId() = !_id
    
    member x.GetRows (sites:Site list) =
        _id := !_id + 1
        _map := Map.empty
        _placements.Clear()

        x.Caller(fun () ->
            _table.RowGroups.Clear()

            _map := 
                seq {
                    for site in sites do
                        let rowGroup = new TableRowGroup()
                        _table.RowGroups.Add(rowGroup)
                        let row1 = new TableRow()
                        rowGroup.Rows.Add(row1)

                        let grid = new Grid()
                        let paragraph = new Paragraph()
                        paragraph.BorderThickness <- new Windows.Thickness(1.0, 1.0, 1.0, 1.0)
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
                                new Run(site.Url),
                                NavigateUri = new Uri(site.Url),
                                Foreground = Windows.Media.Brushes.Green
                            )
                        let label = new Label(
                                        Content = hyperlink, 
                                        Margin = new Windows.Thickness(0.0,3.0,0.0,0.0),
                                        FontWeight = Windows.FontWeights.UltraBold)
                        hyperlink.RequestNavigate |> Event.add _handler |> ignore
                        //stackPanel0.Children.Add(new CheckBox()) |> ignore
                        stackPanel0.Children.Add(label) |> ignore
                        Grid.SetRow(stackPanel0, 0)

                        let stackPanel1 = new StackPanel(Orientation = Orientation.Horizontal)
                        stackPanel1.Children.Add(new Label(Content = sprintf "ID: %d" site.Id)) |> ignore
                        stackPanel1.Children.Add(new Label(Content = sprintf "тИЦ: %d" site.CitationIndex)) |> ignore
                        stackPanel1.Children.Add(new Label(Content = sprintf "PR: %d" site.Pr)) |> ignore
                        stackPanel1.Children.Add(new Label(Content = sprintf "G:[%d]" site.GoogleGof)) |> ignore
                        stackPanel1.Children.Add(new Label(Content = sprintf "Я:[%d]" site.YandexGof)) |> ignore
                        Grid.SetRow(stackPanel1, 1)

                        let boolToRu f = if f then "да" else "нет"
                        
                        let stackPanel2 = new StackPanel(Orientation = Orientation.Horizontal)
                        stackPanel2.Children.Add(new Label(Content = sprintf "DMOZ: %s" (boolToRu site.IsInDmoz))) |> ignore
                        stackPanel2.Children.Add(new Label(Content = sprintf "YACA: %s" (boolToRu site.IsInYaca))) |> ignore
                        stackPanel2.Children.Add(new Label(Content = sprintf "Блок в Я: %s" (boolToRu site.IsBlockedInYandex))) |> ignore
                        stackPanel2.Children.Add(new Label(Content = sprintf "Домен ур.: %d" site.DomainLevel)) |> ignore
                        Grid.SetRow(stackPanel2, 2)

                        //let links = new StackPanel(Margin = new Windows.Thickness(5.0,5.0,0.0,0.0), CanVerticallyScroll = true, Height = 150.0)
                        //let links = new ListBox(Margin = new Windows.Thickness(5.0,5.0,0.0,0.0))
                        //Grid.SetRow(links, 3)

                        grid.Children.Add(stackPanel0) |> ignore
                        grid.Children.Add(stackPanel1) |> ignore
                        grid.Children.Add(stackPanel2) |> ignore
                        //grid.Children.Add(links) |> ignore

                        yield site.Id,(site, paragraph, new Collections.Generic.List<TextBlock>())
                    } |> Map.ofSeq
            ()
        )
    member x.Highlight (words:string list) =
        x.Caller ( fun _ -> 
            !_map |> Map.iter (fun navUri (urlLink,paragraph,textBlocksList) -> 
                                    textBlocksList |> Seq.iter (fun textBlock ->
                                        match words |> List.tryFind (textBlock.Text.ToLower().Contains) with
                                        | Some word -> textBlock.Background <- Windows.Media.Brushes.Yellow
                                        | None -> textBlock.Background <- null)
                                    )
        )

    member x.UpdateSitePage (site:Site, page:Page, title:string) = 
        x.Caller(fun () ->
            match Map.tryFind site.Id !_map with
            | Some (_,links,textBlocksList) ->
                let stackPanel = new StackPanel(Margin = new Windows.Thickness(10.0,0.0,0.0,0.0))
            
                let hyperlink = 
                    new Hyperlink(
                        new Run(page.Uri),
                        NavigateUri = new Uri(sprintf "%s%s" site.Url page.Uri)
                    )
                hyperlink.RequestNavigate |> Event.add _handler |> ignore
                let label = new Label(Content = hyperlink)
                let stackPanel1 = new StackPanel(Orientation = Orientation.Horizontal)
                let checkBox = new CheckBox(Margin = new Windows.Thickness(0.0,5.0,0.0,0.0))
                stackPanel1.Children.Add(checkBox) |> ignore
                stackPanel1.Children.Add(label) |> ignore

                let stackPanel2 = new StackPanel(Orientation = Orientation.Horizontal)
                stackPanel2.Children.Add(new Label(Content = sprintf "ВС: %d" page.ExtLinks)) |> ignore
                stackPanel2.Children.Add(new Label(Content = sprintf "УВ: %d" page.Level)) |> ignore
                stackPanel2.Children.Add(new Label(Content = sprintf "PR: %d" page.Pr)) |> ignore
                stackPanel2.Children.Add(new Label(Content = sprintf "Мест: %d" page.FreePlaces)) |> ignore
                stackPanel2.Children.Add(new Label(Content = sprintf "Цена: %.2f" page.Price)) |> ignore

                stackPanel.Children.Add(stackPanel1) |> ignore
                stackPanel.Children.Add(stackPanel2) |> ignore
                //stackPanel.Children.Add(new Label(Content = sprintf "Title: %s" title)) |> ignore

                _placements.Add((checkBox,page))

                let textBlock = new TextBlock(
                                    Text = sprintf "Title: %s" title, 
                                    TextWrapping = Windows.TextWrapping.Wrap,
                                    Height = Double.NaN, 
                                    Margin = new Windows.Thickness(10.0,0.0,0.0,0.0))
                textBlocksList.Add textBlock

                //links.Items.Add(stackPanel) |> ignore
                links.Inlines.Add(stackPanel)
                links.Inlines.Add(textBlock)
            | None -> ()
        )

    member x.GetPlacements() =
        _placements 
        |> Seq.filter (fun (checkBox,_) -> checkBox.IsChecked.Value)
        |> Seq.map (fun (checkBox,page) -> (checkBox.IsEnabled <- false; page))
        |> List.ofSeq
    
    member x.Metadata = _table

type DataProviderForClosedSites() = 
    inherit WpfCollectionData()

    let _table = new Table( CellSpacing = 10.0 )
    let _handler (e:Windows.Navigation.RequestNavigateEventArgs) = 
        Diagnostics.Process.Start(e.Uri.ToString()) |> ignore
    let _map : Map<Id, (Site*Paragraph*Collections.Generic.List<TextBlock>)> ref = ref Map.empty
    let _placements = new System.Collections.Generic.List<(CheckBox*Page)>()

    let _id = ref 0
    member x.GetId() = !_id
    
    member x.GetRows (sites:Site list) =
        _id := !_id + 1
        _map := Map.empty
        _placements.Clear()

        x.Caller(fun () ->
            _table.RowGroups.Clear()

            _map := 
                seq {
                    for site in sites do
                        let rowGroup = new TableRowGroup()
                        _table.RowGroups.Add(rowGroup)
                        let row1 = new TableRow()
                        rowGroup.Rows.Add(row1)

                        let grid = new Grid()
                        let paragraph = new Paragraph()
                        paragraph.BorderThickness <- new Windows.Thickness(1.0, 1.0, 1.0, 1.0)
                        paragraph.Inlines.Add(grid)
                        let tableCell1 = new TableCell(paragraph)
                        row1.Cells.Add(tableCell1)

                        grid.RowDefinitions.Add(new RowDefinition())
                        grid.RowDefinitions.Add(new RowDefinition())
                        grid.RowDefinitions.Add(new RowDefinition())
                        grid.RowDefinitions.Add(new RowDefinition())
                        
                        let stackPanel0 = new StackPanel(Orientation = Orientation.Horizontal)
                        let label = new Label(
                                        Content = "Закрытый URL",
                                        Margin = new Windows.Thickness(0.0,3.0,0.0,0.0),
                                        FontWeight = Windows.FontWeights.UltraBold)
                        stackPanel0.Children.Add(label) |> ignore
                        Grid.SetRow(stackPanel0, 0)

                        let stackPanel1 = new StackPanel(Orientation = Orientation.Horizontal)
                        stackPanel1.Children.Add(new Label(Content = sprintf "ID: %d" site.Id)) |> ignore
                        stackPanel1.Children.Add(new Label(Content = sprintf "тИЦ: %d" site.CitationIndex)) |> ignore
                        stackPanel1.Children.Add(new Label(Content = sprintf "PR: %d" site.Pr)) |> ignore
                        stackPanel1.Children.Add(new Label(Content = sprintf "G:[%d]" site.GoogleGof)) |> ignore
                        stackPanel1.Children.Add(new Label(Content = sprintf "Я:[%d]" site.YandexGof)) |> ignore
                        Grid.SetRow(stackPanel1, 1)

                        let boolToRu f = if f then "да" else "нет"
                        
                        let stackPanel2 = new StackPanel(Orientation = Orientation.Horizontal)
                        stackPanel2.Children.Add(new Label(Content = sprintf "DMOZ: %s" (boolToRu site.IsInDmoz))) |> ignore
                        stackPanel2.Children.Add(new Label(Content = sprintf "YACA: %s" (boolToRu site.IsInYaca))) |> ignore
                        stackPanel2.Children.Add(new Label(Content = sprintf "Блок в Я: %s" (boolToRu site.IsBlockedInYandex))) |> ignore
                        stackPanel2.Children.Add(new Label(Content = sprintf "Домен ур.: %d" site.DomainLevel)) |> ignore
                        Grid.SetRow(stackPanel2, 2)

                        //let links = new StackPanel(Margin = new Windows.Thickness(5.0,5.0,0.0,0.0), CanVerticallyScroll = true, Height = 150.0)
                        //let links = new ListBox(Margin = new Windows.Thickness(5.0,5.0,0.0,0.0))
                        //Grid.SetRow(links, 3)

                        grid.Children.Add(stackPanel0) |> ignore
                        grid.Children.Add(stackPanel1) |> ignore
                        grid.Children.Add(stackPanel2) |> ignore
                        //grid.Children.Add(links) |> ignore

                        yield site.Id,(site, paragraph, new Collections.Generic.List<TextBlock>())
                    } |> Map.ofSeq
            ()
        )
//    member x.Highlight (words:string list) =
//        x.Caller ( fun _ -> 
//            !_map |> Map.iter (fun navUri (urlLink,paragraph,textBlocksList) -> 
//                                    textBlocksList |> Seq.iter (fun textBlock ->
//                                        match words |> List.tryFind (textBlock.Text.ToLower().Contains) with
//                                        | Some word -> textBlock.Background <- Windows.Media.Brushes.Yellow
//                                        | None -> textBlock.Background <- null)
//                                    )
//        )

    member x.UpdateSitePage (site:Site, page:Page) = 
        x.Caller(fun () ->
            match Map.tryFind site.Id !_map with
            | Some (_,links,textBlocksList) ->
                let stackPanel = new StackPanel(Margin = new Windows.Thickness(10.0,0.0,0.0,0.0))
            
//                let hyperlink = 
//                    new Hyperlink(
//                        new Run(page.Uri),
//                        NavigateUri = new Uri(sprintf "%s%s" site.Url page.Uri)
//                    )
//                hyperlink.RequestNavigate |> Event.add _handler |> ignore
//                let label = new Label(Content = hyperlink)
//                let stackPanel1 = new StackPanel(Orientation = Orientation.Horizontal)
                let checkBox = new CheckBox(Margin = new Windows.Thickness(0.0,5.0,0.0,0.0))
//                stackPanel1.Children.Add(checkBox) |> ignore
//                stackPanel1.Children.Add(label) |> ignore

                let stackPanel2 = new StackPanel(Orientation = Orientation.Horizontal)
                stackPanel2.Children.Add(checkBox) |> ignore
                stackPanel2.Children.Add(new Label(Content = sprintf "ВС: %d" page.ExtLinks)) |> ignore
                stackPanel2.Children.Add(new Label(Content = sprintf "УВ: %d" page.Level)) |> ignore
                stackPanel2.Children.Add(new Label(Content = sprintf "PR: %d" page.Pr)) |> ignore
                stackPanel2.Children.Add(new Label(Content = sprintf "Мест: %d" page.FreePlaces)) |> ignore
                stackPanel2.Children.Add(new Label(Content = sprintf "Цена: %.2f" page.Price)) |> ignore

//                stackPanel.Children.Add(stackPanel1) |> ignore
                stackPanel.Children.Add(stackPanel2) |> ignore
                //stackPanel.Children.Add(new Label(Content = sprintf "Title: %s" title)) |> ignore

                _placements.Add((checkBox,page))

//                let textBlock = new TextBlock(
//                                    Text = sprintf "Закрытый URL", 
//                                    TextWrapping = Windows.TextWrapping.Wrap,
//                                    Height = Double.NaN, 
//                                    Margin = new Windows.Thickness(10.0,0.0,0.0,0.0))
//                textBlocksList.Add textBlock

                //links.Items.Add(stackPanel) |> ignore
                links.Inlines.Add(stackPanel)
//                links.Inlines.Add(textBlock)
            | None -> ()
        )

    member x.GetPlacements() =
        _placements 
        |> Seq.filter (fun (checkBox,_) -> checkBox.IsChecked.Value)
        |> Seq.map (fun (checkBox,page) -> (checkBox.IsEnabled <- false; page))
        |> List.ofSeq
    
    member x.Metadata = _table

type EventType = 
    | Login of LoginInfo
    | WaitingSitesRefresh of Id
    | SearchSites of CustomFilter
    | DownloadFoundOpenedSites of (Id*CustomFilter*Site list)
    | PlaceFoundClosedSites of (Id*CustomFilter*Site list)
    | PlaceOpenedPages of Id
    | PlaceClosedPages of Id
    | PlaceWaitingPages of Id
    | HighlightWaitingPages of String array
    | HighlightSearchedPages of String array

type UIEvent = delegate of Object*EventType->unit

type UIState() = 
    inherit WpfData()

    let _openedSites : Site list ref = ref List.empty
    let _closedSites : Site list ref = ref List.empty

    member x.OpenedSites 
        with get() = !_openedSites
        and set v = 
            _openedSites := v
            x.TriggerChangeProperties ["OpenedSites"]

    member x.ClosedSites
        with get() = !_closedSites
        and set v = 
            _closedSites := v
            x.TriggerChangeProperties ["ClosedSites"]