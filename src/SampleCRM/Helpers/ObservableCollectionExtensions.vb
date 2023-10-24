Imports System.Runtime.CompilerServices
Imports System.Collections.ObjectModel
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Reflection

Module ObservableCollectionExtensions
    <Extension()>
    Sub AddRange(Of T)(ByVal collection As ObservableCollection(Of T), ByVal items As IEnumerable(Of T))
        For Each item In items
            collection.Add(item)
        Next
    End Sub

    <Extension()>
    Sub InsertRange(Of T)(ByVal collection As ObservableCollection(Of T), ByVal items As IEnumerable(Of T))
        Dim enumerable = If(TryCast(items, List(Of T)), items.ToList())

        If collection Is Nothing OrElse items Is Nothing OrElse Not enumerable.Any() Then
            Return
        End If

        Dim type As Type = collection.[GetType]()
        type.InvokeMember("CheckReentrancy", BindingFlags.Instance Or BindingFlags.InvokeMethod Or BindingFlags.NonPublic, Nothing, collection, Nothing)
        Dim itemsProp = type.BaseType.GetProperty("Items", BindingFlags.NonPublic Or BindingFlags.FlattenHierarchy Or BindingFlags.Instance)
        Dim privateItems = TryCast(itemsProp.GetValue(collection), IList(Of T))

        For Each item In enumerable
            privateItems.Add(item)
        Next

        type.InvokeMember("OnPropertyChanged", BindingFlags.Instance Or BindingFlags.InvokeMethod Or BindingFlags.NonPublic, Nothing, collection, New Object() {New PropertyChangedEventArgs("Count")})
        type.InvokeMember("OnPropertyChanged", BindingFlags.Instance Or BindingFlags.InvokeMethod Or BindingFlags.NonPublic, Nothing, collection, New Object() {New PropertyChangedEventArgs("Item[]")})
        type.InvokeMember("OnCollectionChanged", BindingFlags.Instance Or BindingFlags.InvokeMethod Or BindingFlags.NonPublic, Nothing, collection, New Object() {New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)})
    End Sub
End Module
